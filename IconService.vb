Imports System.Collections.Concurrent
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Runtime.CompilerServices
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

''' <summary>
''' Renders Fluent UI System Icons from the embedded icon font into cached, DPI-sized bitmaps.
''' Bitmaps returned by GetIcon are shared from the cache: callers must never dispose them.
''' </summary>
Public NotInheritable Class IconService

    Private Const FontResourceName As String = "KBrowser.Fonts.FluentSystemIcons-Regular.ttf"

    Private Shared ReadOnly IconFontFamily As New Lazy(Of FontFamily)(AddressOf LoadFontFamily)
    Private Shared ReadOnly Cache As New ConcurrentDictionary(Of (FluentIcon, Integer, Integer), Bitmap)()
    Private Shared ReadOnly DrawLock As New Object()

    ' Kept alive for the process lifetime: GDI+ reads font data from this memory on every draw.
    Private Shared _fontCollection As PrivateFontCollection
    Private Shared _fontMemory As IntPtr

    Private Shared ReadOnly _systemDpi As New Lazy(Of Integer)(Function()
                                                                   Using g = Graphics.FromHwnd(IntPtr.Zero)
                                                                       Return CInt(g.DpiX)
                                                                   End Using
                                                               End Function)

    Private Sub New()
    End Sub

    ''' <summary>
    ''' The system DPI captured once at first use (the app is System-aware, so it doesn't change).
    ''' </summary>
    Public Shared ReadOnly Property SystemDpi As Integer
        Get
            Return _systemDpi.Value
        End Get
    End Property

    ''' <summary>
    ''' Loads the icon font and installs the icon-aware tool strip renderer. Call once at application startup.
    ''' </summary>
    Public Shared Sub Initialize()
        Dim unused = IconFontFamily.Value
        ToolStripManager.Renderer = New IconToolStripRenderer()
    End Sub

    Public Shared Function GetIcon(icon As FluentIcon, logicalSize As Integer, color As Color) As Bitmap
        Return GetIcon(icon, logicalSize, color, SystemDpi)
    End Function

    Public Shared Function GetIcon(icon As FluentIcon, logicalSize As Integer, color As Color, dpi As Integer) As Bitmap
        Dim pixelSize = CInt(Math.Round(logicalSize * dpi / 96.0))
        Return Cache.GetOrAdd((icon, pixelSize, color.ToArgb()),
                              Function(key)
                                  Dim design = PickDesign(icon, pixelSize)
                                  Return Render(design.CodePoint, Math.Min(design.Size, pixelSize), pixelSize, color)
                              End Function)
    End Function

    ''' <summary>
    ''' High-quality resize of an arbitrary image (e.g. a favicon) to the DPI-scaled square for logicalSize.
    ''' The caller owns the returned bitmap.
    ''' </summary>
    Public Shared Function ScaleImage(img As Image, logicalSize As Integer) As Bitmap
        Dim pixelSize = CInt(Math.Round(logicalSize * SystemDpi / 96.0))
        Dim bmp As New Bitmap(pixelSize, pixelSize, PixelFormat.Format32bppPArgb)
        Using g = Graphics.FromImage(bmp)
            g.InterpolationMode = InterpolationMode.HighQualityBicubic
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.DrawImage(img, New Rectangle(0, 0, pixelSize, pixelSize))
        End Using
        Return bmp
    End Function

    ''' <summary>
    ''' Fluent draws each icon separately for several pixel sizes. Use the largest design that fits
    ''' the target size, or the smallest if none fits. The caller draws it at its own size (never
    ''' enlarged) so 1px strokes stay on the pixel grid.
    ''' </summary>
    Private Shared Function PickDesign(icon As FluentIcon, pixelSize As Integer) As (Size As Integer, CodePoint As Integer)
        Dim designs = FluentIconGlyphs.Designs(icon)
        Dim chosen = designs(0)
        For Each candidate In designs
            If candidate.Size <= pixelSize Then chosen = candidate
        Next
        Return chosen
    End Function

    ''' <summary>
    ''' Draws the glyph at emSize pixels, centred in a pixelSize square bitmap.
    ''' </summary>
    Private Shared Function Render(codePoint As Integer, emSize As Integer, pixelSize As Integer, color As Color) As Bitmap
        Dim bmp As New Bitmap(pixelSize, pixelSize, PixelFormat.Format32bppPArgb)
        ' The glyph is drawn as a vector path centred on its own outline: the font's line height is
        ' taller than its em box, so text-layout centring would push icons off-centre (or, with
        ' GenericTypographic's LineLimit flag, not draw them at all). The offset is rounded to whole
        ' pixels so the design's pixel grid lines up with the bitmap's.
        ' GDI+ use of a shared private font is not safe to run concurrently.
        SyncLock DrawLock
            Using path As New GraphicsPath(),
                  g = Graphics.FromImage(bmp),
                  brush As New SolidBrush(color)
                path.AddString(Char.ConvertFromUtf32(codePoint), IconFontFamily.Value, FontStyle.Regular, emSize, PointF.Empty, StringFormat.GenericTypographic)
                Dim bounds = path.GetBounds()
                Using centre As New Matrix()
                    centre.Translate(CSng(Math.Round(pixelSize / 2.0 - (bounds.X + bounds.Width / 2.0))),
                                     CSng(Math.Round(pixelSize / 2.0 - (bounds.Y + bounds.Height / 2.0))))
                    path.Transform(centre)
                End Using
                g.Clear(Color.Transparent)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.FillPath(brush, path)
            End Using
        End SyncLock
        Return bmp
    End Function

    ' ---------------------------------------------------------------------------------------
    ' Bindings: which UI element shows which icon, so icons can be re-coloured on theme change
    ' and when the element is enabled/disabled. Weak keys let disposed forms be collected.
    ' ---------------------------------------------------------------------------------------

    Private NotInheritable Class IconBinding
        Public Icon As FluentIcon
        Public LogicalSize As Integer
        Public ExplicitColor As Color?
    End Class

    Private Shared ReadOnly Bindings As New ConditionalWeakTable(Of Object, IconBinding)()

    ''' <summary>
    ''' Shows a Fluent icon on a toolbar/menu item, coloured from the item's ForeColor unless a colour is given.
    ''' </summary>
    Public Shared Sub SetIcon(item As ToolStripItem, icon As FluentIcon, Optional logicalSize As Integer = 16, Optional color As Color? = Nothing)
        If item Is Nothing Then Throw New ArgumentNullException(NameOf(item))
        Bind(item, icon, logicalSize, color)
        item.ImageScaling = ToolStripItemImageScaling.None
    End Sub

    ''' <summary>
    ''' Shows a Fluent icon on a Button, Label or PictureBox, coloured from its ForeColor unless a colour is given.
    ''' </summary>
    Public Shared Sub SetIcon(target As Control, icon As FluentIcon, Optional logicalSize As Integer = 16, Optional color As Color? = Nothing)
        If target Is Nothing Then Throw New ArgumentNullException(NameOf(target))
        If Not (TypeOf target Is Button OrElse TypeOf target Is Label OrElse TypeOf target Is PictureBox) Then
            Throw New ArgumentException($"Icons are supported on Button, Label and PictureBox, not {target.GetType().Name}.", NameOf(target))
        End If
        Bind(target, icon, logicalSize, color)
        Dim btn = TryCast(target, Button)
        If btn IsNot Nothing AndAlso btn.Text <> "" Then btn.TextImageRelation = TextImageRelation.ImageBeforeText
    End Sub

    ''' <summary>
    ''' Forgets the icon binding (e.g. before showing a favicon instead). Leaves the current image in place.
    ''' </summary>
    Public Shared Sub ClearIcon(target As Object)
        If target IsNot Nothing Then Bindings.Remove(target)
    End Sub

    Public Shared Function IsBound(target As Object) As Boolean
        Dim binding As IconBinding = Nothing
        Return target IsNot Nothing AndAlso Bindings.TryGetValue(target, binding)
    End Function

    ''' <summary>
    ''' Re-renders every bound icon under root (controls, tool strip items, drop-downs, context menus)
    ''' from its host's current colours.
    ''' </summary>
    Public Shared Sub RefreshIcons(root As Control)
        If root Is Nothing Then Return
        RefreshTarget(root)
        If root.ContextMenuStrip IsNot Nothing Then RefreshIcons(root.ContextMenuStrip)
        Dim strip = TryCast(root, ToolStrip)
        If strip IsNot Nothing Then RefreshItems(strip.Items)
        For Each child As Control In root.Controls
            RefreshIcons(child)
        Next
    End Sub

    Private Shared Sub RefreshItems(items As ToolStripItemCollection)
        For Each item As ToolStripItem In items
            RefreshTarget(item)
            Dim dropDown = TryCast(item, ToolStripDropDownItem)
            If dropDown IsNot Nothing AndAlso dropDown.HasDropDownItems Then RefreshItems(dropDown.DropDownItems)
        Next
    End Sub

    Private Shared Sub Bind(target As Object, icon As FluentIcon, logicalSize As Integer, color As Color?)
        Dim isNew = Not IsBound(target)
        Bindings.AddOrUpdate(target, New IconBinding With {.Icon = icon, .LogicalSize = logicalSize, .ExplicitColor = color})
        If isNew Then
            Dim item = TryCast(target, ToolStripItem)
            If item IsNot Nothing Then
                AddHandler item.EnabledChanged, Sub(s, e) RefreshTarget(s)
            Else
                AddHandler DirectCast(target, Control).EnabledChanged, Sub(s, e) RefreshTarget(s)
            End If
        End If
        RefreshTarget(target)
    End Sub

    Private Shared Sub RefreshTarget(target As Object)
        Dim binding As IconBinding = Nothing
        If Not Bindings.TryGetValue(target, binding) Then Return

        Dim item = TryCast(target, ToolStripItem)
        Dim ctrl = TryCast(target, Control)
        Dim fore = If(item IsNot Nothing, item.ForeColor, ctrl.ForeColor)
        Dim back = If(item IsNot Nothing, item.BackColor, ctrl.BackColor)
        Dim enabled = If(item IsNot Nothing, item.Enabled, ctrl.Enabled)

        Dim color = If(binding.ExplicitColor, fore)
        If Not enabled Then color = Blend(color, back)
        Dim bmp = GetIcon(binding.Icon, binding.LogicalSize, color)

        If item IsNot Nothing Then
            item.Image = bmp
        ElseIf TypeOf ctrl Is Button Then
            DirectCast(ctrl, Button).Image = bmp
        ElseIf TypeOf ctrl Is Label Then
            DirectCast(ctrl, Label).Image = bmp
        ElseIf TypeOf ctrl Is PictureBox Then
            DirectCast(ctrl, PictureBox).Image = bmp
        End If
    End Sub

    ''' <summary>
    ''' Disabled colour: per-channel average of foreground and background, fully opaque.
    ''' </summary>
    Private Shared Function Blend(fore As Color, back As Color) As Color
        Return Color.FromArgb(255,
                              CInt(Math.Round((CInt(fore.R) + back.R) / 2.0)),
                              CInt(Math.Round((CInt(fore.G) + back.G) / 2.0)),
                              CInt(Math.Round((CInt(fore.B) + back.B) / 2.0)))
    End Function

    Private Shared Function LoadFontFamily() As FontFamily
        Using stream = GetType(IconService).Assembly.GetManifestResourceStream(FontResourceName)
            If stream Is Nothing Then Throw New InvalidOperationException("Embedded icon font not found: " & FontResourceName)
            Dim bytes(CInt(stream.Length) - 1) As Byte
            stream.ReadExactly(bytes, 0, bytes.Length)

            _fontMemory = Marshal.AllocCoTaskMem(bytes.Length)
            Marshal.Copy(bytes, 0, _fontMemory, bytes.Length)
            _fontCollection = New PrivateFontCollection()
            _fontCollection.AddMemoryFont(_fontMemory, bytes.Length)
            Return _fontCollection.Families(0)
        End Using
    End Function

End Class
