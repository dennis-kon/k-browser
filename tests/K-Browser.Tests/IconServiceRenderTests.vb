Imports System.Drawing
Imports Advance_Webbrowser
Imports Xunit

Public Class IconServiceRenderTests

    Private Shared ReadOnly Lime As Color = Color.FromArgb(161, 197, 74)

    <Fact>
    Public Sub GetIcon_SizeScalesWithDpi()
        Assert.Equal(16, IconService.GetIcon(FluentIcon.Home, 16, Color.Black, 96).Width)
        Assert.Equal(24, IconService.GetIcon(FluentIcon.Home, 16, Color.Black, 144).Width)
        Assert.Equal(25, IconService.GetIcon(FluentIcon.Home, 20, Color.Black, 120).Height)
    End Sub

    <Fact>
    Public Sub GetIcon_DrawsInRequestedColor()
        Dim bmp = IconService.GetIcon(FluentIcon.Home, 20, Lime, 96)
        Dim opaque = Pixels(bmp).Where(Function(p) p.A > 200).ToList()

        Assert.NotEmpty(opaque)
        Assert.All(opaque, Sub(p)
                               Assert.InRange(CInt(p.R), Lime.R - 12, Lime.R + 12)
                               Assert.InRange(CInt(p.G), Lime.G - 12, Lime.G + 12)
                               Assert.InRange(CInt(p.B), Lime.B - 12, Lime.B + 12)
                           End Sub)
    End Sub

    <Fact>
    Public Sub GetIcon_GlyphCentredAndFillsBox()
        Dim bmp = IconService.GetIcon(FluentIcon.Home, 20, Color.Black, 96)
        Dim minX = Integer.MaxValue, minY = Integer.MaxValue, maxX = -1, maxY = -1
        For y = 0 To bmp.Height - 1
            For x = 0 To bmp.Width - 1
                If bmp.GetPixel(x, y).A > 0 Then
                    minX = Math.Min(minX, x) : maxX = Math.Max(maxX, x)
                    minY = Math.Min(minY, y) : maxY = Math.Max(maxY, y)
                End If
            Next
        Next

        Assert.True(maxX - minX + 1 >= 10, $"glyph width {maxX - minX + 1}")
        Assert.True(maxY - minY + 1 >= 10, $"glyph height {maxY - minY + 1}")
        Assert.InRange((minX + maxX + 1) / 2.0, 8.5, 11.5)
        Assert.InRange((minY + maxY + 1) / 2.0, 8.5, 11.5)
    End Sub

    <Fact>
    Public Sub GetIcon_AllEnumValuesRenderDistinctGlyphs()
        Dim icons = [Enum].GetValues(GetType(FluentIcon)).Cast(Of FluentIcon)().ToList()
        Dim hashes = icons.Select(Function(i) PixelSignature(IconService.GetIcon(i, 20, Color.Black, 96))).ToList()

        Assert.Equal(icons.Count, hashes.Distinct().Count())
    End Sub

    <Fact>
    Public Sub GetIcon_IsCached()
        Dim a = IconService.GetIcon(FluentIcon.Star, 16, Color.Black, 96)
        Dim b = IconService.GetIcon(FluentIcon.Star, 16, Color.Black, 96)
        Dim c = IconService.GetIcon(FluentIcon.Star, 16, Color.Red, 96)

        Assert.Same(a, b)
        Assert.NotSame(a, c)
    End Sub

    <Fact>
    Public Sub GetIcon_SmallSizesStayCrisp()
        ' Size-specific designs keep 1px strokes on the pixel grid, so a 16px icon has 8+ near-solid pixels
        ' (the 20px designs of these icons shrunk to 16px have 1-7).
        For Each icon In {FluentIcon.Home, FluentIcon.ArrowLeft, FluentIcon.Star}
            Dim bmp = IconService.GetIcon(icon, 16, Color.Black, 96)
            Assert.True(Pixels(bmp).Count(Function(p) p.A >= 200) >= 8, $"{icon} has too few solid pixels at 16px")
        Next
    End Sub

    <Fact>
    Public Sub GetIcon_BetweenDesignSizes_DrawsDesignUnscaledAndCentred()
        ' 32px at 125% is a 40px box; Home has 32 and 48 designs. The 32 design must be drawn at its own
        ' size (crisp) and centred, not stretched to 40px (blurred strokes).
        Dim native = InkBounds(IconService.GetIcon(FluentIcon.Home, 32, Color.Black, 96))
        Dim boxed = IconService.GetIcon(FluentIcon.Home, 32, Color.Black, 120)
        Dim ink = InkBounds(boxed)

        Assert.Equal(40, boxed.Width)
        Assert.InRange(ink.Width, native.Width - 1, native.Width + 1)
        Assert.InRange(ink.Height, native.Height - 1, native.Height + 1)
        Assert.InRange(ink.X + ink.Width / 2.0, 18.5, 21.5)
        Assert.InRange(ink.Y + ink.Height / 2.0, 18.5, 21.5)
    End Sub

    Private Shared Function InkBounds(bmp As Bitmap) As Rectangle
        Dim minX = Integer.MaxValue, minY = Integer.MaxValue, maxX = -1, maxY = -1
        For y = 0 To bmp.Height - 1
            For x = 0 To bmp.Width - 1
                If bmp.GetPixel(x, y).A > 0 Then
                    minX = Math.Min(minX, x) : maxX = Math.Max(maxX, x)
                    minY = Math.Min(minY, y) : maxY = Math.Max(maxY, y)
                End If
            Next
        Next
        Return Rectangle.FromLTRB(minX, minY, maxX + 1, maxY + 1)
    End Function

    Friend Shared Function Pixels(bmp As Bitmap) As IEnumerable(Of Color)
        Dim result As New List(Of Color)
        For y = 0 To bmp.Height - 1
            For x = 0 To bmp.Width - 1
                result.Add(bmp.GetPixel(x, y))
            Next
        Next
        Return result
    End Function

    Friend Shared Function PixelSignature(bmp As Bitmap) As String
        Return String.Join(",", Pixels(bmp).Select(Function(p) p.ToArgb()))
    End Function

End Class
