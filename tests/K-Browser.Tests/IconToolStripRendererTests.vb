Imports System.Drawing
Imports System.Windows.Forms
Imports Advance_Webbrowser
Imports Xunit

Public Class IconToolStripRendererTests

    ''' <summary>WCAG contrast ratio between two opaque colours.</summary>
    Private Shared Function Contrast(a As Color, b As Color) As Double
        Dim lum = Function(c As Color)
                      Dim ch = Function(v As Integer)
                                   Dim s = v / 255.0
                                   Return If(s <= 0.03928, s / 12.92, Math.Pow((s + 0.055) / 1.055, 2.4))
                               End Function
                      Return 0.2126 * ch(c.R) + 0.7152 * ch(c.G) + 0.0722 * ch(c.B)
                  End Function
        Dim l1 = lum(a), l2 = lum(b)
        Return (Math.Max(l1, l2) + 0.05) / (Math.Min(l1, l2) + 0.05)
    End Function

    ''' <summary>Backgrounds a light dark-mode icon is drawn on: hover, press, checked, menu image column.</summary>
    Private Shared Function IconBackgrounds(t As ProfessionalColorTable) As IEnumerable(Of (Name As String, Value As Color))
        Return {
            ("ButtonSelectedHighlight", t.ButtonSelectedHighlight),
            ("ButtonSelectedGradientBegin", t.ButtonSelectedGradientBegin),
            ("ButtonSelectedGradientEnd", t.ButtonSelectedGradientEnd),
            ("ButtonPressedGradientBegin", t.ButtonPressedGradientBegin),
            ("ButtonPressedGradientEnd", t.ButtonPressedGradientEnd),
            ("ButtonCheckedGradientBegin", t.ButtonCheckedGradientBegin),
            ("ButtonCheckedGradientEnd", t.ButtonCheckedGradientEnd),
            ("CheckBackground", t.CheckBackground),
            ("CheckSelectedBackground", t.CheckSelectedBackground),
            ("CheckPressedBackground", t.CheckPressedBackground),
            ("MenuItemSelected", t.MenuItemSelected),
            ("MenuItemSelectedGradientBegin", t.MenuItemSelectedGradientBegin),
            ("MenuItemSelectedGradientEnd", t.MenuItemSelectedGradientEnd),
            ("MenuItemPressedGradientBegin", t.MenuItemPressedGradientBegin),
            ("MenuItemPressedGradientEnd", t.MenuItemPressedGradientEnd),
            ("ImageMarginGradientBegin", t.ImageMarginGradientBegin),
            ("ImageMarginGradientMiddle", t.ImageMarginGradientMiddle),
            ("ImageMarginGradientEnd", t.ImageMarginGradientEnd),
            ("ToolStripDropDownBackground", t.ToolStripDropDownBackground)}
    End Function

    <Fact>
    Public Sub DarkMode_IconBackgroundsContrastWithDarkText()
        Dim wasDark = ThemeManager.IsDarkMode
        Try
            ThemeManager.IsDarkMode = True
            Dim table = New IconToolStripRenderer().ColorTable

            Dim poor = IconBackgrounds(table).Where(Function(b) Contrast(b.Value, ThemeManager.DarkText) < 4.5).
                                              Select(Function(b) $"{b.Name} {b.Value} = {Contrast(b.Value, ThemeManager.DarkText):F1}:1").ToList()
            Assert.Empty(poor)
        Finally
            ThemeManager.IsDarkMode = wasDark
        End Try
    End Sub

    <Fact>
    Public Sub LightMode_KeepsProfessionalColours()
        Dim wasDark = ThemeManager.IsDarkMode
        Try
            ThemeManager.IsDarkMode = False
            Dim themed = IconBackgrounds(New IconToolStripRenderer().ColorTable).ToList()
            Dim standard = IconBackgrounds(New ProfessionalColorTable()).ToList()

            Assert.Equal(standard.Select(Function(b) b.Value.ToArgb()), themed.Select(Function(b) b.Value.ToArgb()))
        Finally
            ThemeManager.IsDarkMode = wasDark
        End Try
    End Sub

End Class
