Imports System.Collections.Concurrent
Imports System.Drawing
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.Runtime.InteropServices

''' <summary>
''' Renders Fluent UI System Icons from the embedded icon font into cached, DPI-sized bitmaps.
''' Bitmaps returned by GetIcon are shared from the cache: callers must never dispose them.
''' </summary>
Public NotInheritable Class IconService

    Private Const FontResourceName As String = "KBrowser.Fonts.FluentSystemIcons-Resizable.ttf"

    ' Filled glyph code points, only for icons the app shows filled.
    Private Shared ReadOnly FilledCodePoints As New Dictionary(Of FluentIcon, Integer) From {
        {FluentIcon.Star, 61788}
    }

    Private Shared ReadOnly IconFontFamily As New Lazy(Of FontFamily)(AddressOf LoadFontFamily)
    Private Shared ReadOnly Cache As New ConcurrentDictionary(Of (FluentIcon, Integer, Integer, Boolean), Bitmap)()
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
    ''' Loads the icon font. Call once at application startup.
    ''' </summary>
    Public Shared Sub Initialize()
        Dim unused = IconFontFamily.Value
    End Sub

    Public Shared Function GetIcon(icon As FluentIcon, logicalSize As Integer, color As Color, Optional filled As Boolean = False) As Bitmap
        Return GetIcon(icon, logicalSize, color, SystemDpi, filled)
    End Function

    Public Shared Function GetIcon(icon As FluentIcon, logicalSize As Integer, color As Color, dpi As Integer, filled As Boolean) As Bitmap
        Dim codePoint = CInt(icon)
        If filled AndAlso Not FilledCodePoints.TryGetValue(icon, codePoint) Then
            Throw New ArgumentException($"No filled variant is registered for {icon}.", NameOf(filled))
        End If

        Dim pixelSize = CInt(Math.Round(logicalSize * dpi / 96.0))
        Return Cache.GetOrAdd((icon, pixelSize, color.ToArgb(), filled),
                              Function(key) Render(codePoint, pixelSize, color))
    End Function

    Private Shared Function Render(codePoint As Integer, pixelSize As Integer, color As Color) As Bitmap
        Dim bmp As New Bitmap(pixelSize, pixelSize, PixelFormat.Format32bppPArgb)
        ' The glyph is drawn as a vector path centred on its own outline: the font's line height is
        ' taller than its em box, so text-layout centring would push icons off-centre (or, with
        ' GenericTypographic's LineLimit flag, not draw them at all).
        ' GDI+ use of a shared private font is not safe to run concurrently.
        SyncLock DrawLock
            Using path As New GraphicsPath(),
                  g = Graphics.FromImage(bmp),
                  brush As New SolidBrush(color)
                path.AddString(ChrW(codePoint), IconFontFamily.Value, FontStyle.Regular, pixelSize, PointF.Empty, StringFormat.GenericDefault)
                Dim bounds = path.GetBounds()
                Using centre As New Matrix()
                    centre.Translate(pixelSize / 2.0F - (bounds.X + bounds.Width / 2.0F),
                                     pixelSize / 2.0F - (bounds.Y + bounds.Height / 2.0F))
                    path.Transform(centre)
                End Using
                g.Clear(Color.Transparent)
                g.SmoothingMode = SmoothingMode.AntiAlias
                g.FillPath(brush, path)
            End Using
        End SyncLock
        Return bmp
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
