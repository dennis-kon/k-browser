Imports System.Drawing
Imports System.Windows.Forms
Imports Advance_Webbrowser
Imports Xunit

Public Class IconBindingTests

    Private Shared Sub AssertOpaquePixelsNear(img As Image, expected As Color, tolerance As Integer)
        Dim opaque = IconServiceRenderTests.Pixels(DirectCast(img, Bitmap)).Where(Function(p) p.A >= 200).ToList()
        Assert.NotEmpty(opaque)
        Assert.All(opaque, Sub(p)
                               Assert.InRange(CInt(p.R), expected.R - tolerance, expected.R + tolerance)
                               Assert.InRange(CInt(p.G), expected.G - tolerance, expected.G + tolerance)
                               Assert.InRange(CInt(p.B), expected.B - tolerance, expected.B + tolerance)
                           End Sub)
    End Sub

    <Fact>
    Public Sub SetIcon_ToolStripButton_UsesForeColor()
        RunSta(Sub()
                   Using btn As New ToolStripButton() With {.ForeColor = Color.Red}
                       IconService.SetIcon(btn, FluentIcon.Home, 20)

                       AssertOpaquePixelsNear(btn.Image, Color.Red, 12)
                       Assert.Equal(ToolStripItemImageScaling.None, btn.ImageScaling)
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub RefreshIcons_RecolorsAfterForeColorChange()
        RunSta(Sub()
                   Using form As New Form(), menu As New MenuStrip()
                       Dim parent As New ToolStripMenuItem("Parent")
                       Dim child As New ToolStripMenuItem("Child") With {.ForeColor = Color.Red}
                       parent.DropDownItems.Add(child)
                       menu.Items.Add(parent)
                       form.Controls.Add(menu)
                       IconService.SetIcon(child, FluentIcon.Home)

                       child.ForeColor = Color.Blue
                       IconService.RefreshIcons(form)

                       AssertOpaquePixelsNear(child.Image, Color.Blue, 12)
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub RefreshIcons_KeepsExplicitColor()
        RunSta(Sub()
                   Using form As New Form(), lbl As New Label()
                       form.Controls.Add(lbl)
                       Dim green = Color.FromArgb(30, 150, 60)
                       IconService.SetIcon(lbl, FluentIcon.Shield, 32, green)

                       lbl.ForeColor = Color.White
                       IconService.RefreshIcons(form)

                       AssertOpaquePixelsNear(lbl.Image, green, 12)
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub Disabled_UsesBlendedColor()
        RunSta(Sub()
                   Using btn As New ToolStripButton() With {.ForeColor = Color.White, .BackColor = Color.Black}
                       IconService.SetIcon(btn, FluentIcon.Home, 20)

                       btn.Enabled = False

                       AssertOpaquePixelsNear(btn.Image, Color.FromArgb(128, 128, 128), 3)
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub Reenabled_RestoresForeColor()
        RunSta(Sub()
                   Using btn As New ToolStripButton() With {.ForeColor = Color.White, .BackColor = Color.Black}
                       IconService.SetIcon(btn, FluentIcon.Home, 20)
                       btn.Enabled = False

                       btn.Enabled = True

                       AssertOpaquePixelsNear(btn.Image, Color.White, 3)
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub SetIcon_SupportsButtonLabelPictureBox_AndRejectsTextBox()
        RunSta(Sub()
                   Using btn As New Button() With {.Text = "Reload"}, lbl As New Label(), pic As New PictureBox(), txt As New TextBox()
                       IconService.SetIcon(btn, FluentIcon.ArrowClockwise)
                       IconService.SetIcon(lbl, FluentIcon.Key, 32)
                       IconService.SetIcon(pic, FluentIcon.History, 32)

                       Assert.NotNull(btn.Image)
                       Assert.Equal(TextImageRelation.ImageBeforeText, btn.TextImageRelation)
                       Assert.NotNull(lbl.Image)
                       Assert.NotNull(pic.Image)
                       Assert.Throws(Of ArgumentException)(Sub() IconService.SetIcon(txt, FluentIcon.Home))
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub ClearIcon_StopsRefresh()
        RunSta(Sub()
                   Using form As New Form(), strip As New ToolStrip(), custom As New Bitmap(16, 16)
                       Dim item As New ToolStripButton()
                       strip.Items.Add(item)
                       form.Controls.Add(strip)
                       IconService.SetIcon(item, FluentIcon.Globe)

                       IconService.ClearIcon(item)
                       item.Image = custom
                       IconService.RefreshIcons(form)

                       Assert.Same(custom, item.Image)
                       Assert.False(IconService.IsBound(item))
                   End Using
               End Sub)
    End Sub

    <Fact>
    Public Sub ApplyTheme_RecolorsBoundIcons()
        RunSta(Sub()
                   Dim wasDark = ThemeManager.IsDarkMode
                   Try
                       Using form As New Form(), strip As New ToolStrip()
                           Dim item As New ToolStripButton()
                           strip.Items.Add(item)
                           form.Controls.Add(strip)
                           IconService.SetIcon(item, FluentIcon.Home, 20)

                           ThemeManager.IsDarkMode = True
                           ThemeManager.ApplyTheme(form)
                           AssertOpaquePixelsNear(item.Image, ThemeManager.DarkText, 12)

                           ThemeManager.IsDarkMode = False
                           ThemeManager.ApplyTheme(form)
                           AssertOpaquePixelsNear(item.Image, ThemeManager.LightText, 12)
                       End Using
                   Finally
                       ThemeManager.IsDarkMode = wasDark
                   End Try
               End Sub)
    End Sub

End Class
