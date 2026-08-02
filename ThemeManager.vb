Imports System.Drawing
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Class ThemeManager

    ' Windows DWM Immersive Dark Mode API
    <DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function DwmSetWindowAttribute(ByVal hwnd As IntPtr, ByVal attr As Integer, ByRef attrValue As Integer, ByVal attrSize As Integer) As Integer
    End Function

    Private Const DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 As Integer = 19
    Private Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20

    Public Shared Property IsDarkMode As Boolean = False

    ' Theme Color Tokens
    Public Shared ReadOnly DarkFormBg As Color = Color.FromArgb(28, 36, 52)
    Public Shared ReadOnly DarkPanelBg As Color = Color.FromArgb(37, 47, 67)
    Public Shared ReadOnly DarkInputBg As Color = Color.FromArgb(24, 30, 44)
    Public Shared ReadOnly DarkText As Color = Color.FromArgb(235, 240, 245)
    Public Shared ReadOnly DarkSubText As Color = Color.FromArgb(160, 175, 195)
    Public Shared ReadOnly DarkBorder As Color = Color.FromArgb(50, 62, 85)
    Public Shared ReadOnly DarkAccent As Color = Color.FromArgb(161, 197, 74) ' Lime Green

    Public Shared ReadOnly LightFormBg As Color = Color.FromArgb(248, 250, 255)
    Public Shared ReadOnly LightPanelBg As Color = Color.FromArgb(245, 247, 250)
    Public Shared ReadOnly LightInputBg As Color = Color.White
    Public Shared ReadOnly LightText As Color = Color.FromArgb(28, 36, 52)
    Public Shared ReadOnly LightSubText As Color = Color.Gray
    Public Shared ReadOnly LightBorder As Color = Color.FromArgb(226, 232, 240)

    ''' <summary>
    ''' Applies the current Dark or Light theme to the target Form, window title bar, and all child controls.
    ''' </summary>
    Public Shared Sub ApplyTheme(ByVal form As Form)
        If form Is Nothing OrElse form.IsDisposed Then Return

        ' 1. Apply Native Windows 10/11 Dark Title Bar
        Try
            Dim useDark As Integer = If(IsDarkMode, 1, 0)
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, useDark, Marshal.SizeOf(useDark))
            DwmSetWindowAttribute(form.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, useDark, Marshal.SizeOf(useDark))
        Catch
        End Try

        ' 2. Apply Form Background and Text
        form.BackColor = If(IsDarkMode, DarkFormBg, LightFormBg)
        form.ForeColor = If(IsDarkMode, DarkText, LightText)

        ' 3. Apply Controls Recursively
        ApplyControlsTheme(form.Controls)

        form.Refresh()
    End Sub

    ''' <summary>
    ''' Recursively styles a collection of Windows Forms controls based on current theme state.
    ''' </summary>
    Public Shared Sub ApplyControlsTheme(ByVal controls As Control.ControlCollection)
        If controls Is Nothing Then Return

        For Each ctrl As Control In controls
            If ctrl Is Nothing Then Continue For

            If TypeOf ctrl Is Panel Then
                Dim pnl = DirectCast(ctrl, Panel)
                pnl.BackColor = If(IsDarkMode, DarkPanelBg, LightPanelBg)
                pnl.ForeColor = If(IsDarkMode, DarkText, LightText)

            ElseIf TypeOf ctrl Is GroupBox Then
                Dim gb = DirectCast(ctrl, GroupBox)
                gb.BackColor = If(IsDarkMode, DarkFormBg, LightFormBg)
                gb.ForeColor = If(IsDarkMode, DarkText, LightText)

            ElseIf TypeOf ctrl Is TextBox Then
                Dim tb = DirectCast(ctrl, TextBox)
                tb.BackColor = If(IsDarkMode, DarkInputBg, LightInputBg)
                tb.ForeColor = If(IsDarkMode, DarkText, LightText)
                tb.BorderStyle = BorderStyle.FixedSingle

            ElseIf TypeOf ctrl Is ComboBox Then
                Dim cb = DirectCast(ctrl, ComboBox)
                cb.BackColor = If(IsDarkMode, DarkInputBg, LightInputBg)
                cb.ForeColor = If(IsDarkMode, DarkText, LightText)

            ElseIf TypeOf ctrl Is ListBox Then
                Dim lb = DirectCast(ctrl, ListBox)
                lb.BackColor = If(IsDarkMode, DarkInputBg, LightInputBg)
                lb.ForeColor = If(IsDarkMode, DarkText, LightText)

            ElseIf TypeOf ctrl Is TreeView Then
                Dim tv = DirectCast(ctrl, TreeView)
                tv.BackColor = If(IsDarkMode, DarkInputBg, LightInputBg)
                tv.ForeColor = If(IsDarkMode, DarkText, LightText)
                tv.LineColor = If(IsDarkMode, DarkSubText, LightSubText)

            ElseIf TypeOf ctrl Is ListView Then
                Dim lv = DirectCast(ctrl, ListView)
                lv.BackColor = If(IsDarkMode, DarkInputBg, LightInputBg)
                lv.ForeColor = If(IsDarkMode, DarkText, LightText)

            ElseIf TypeOf ctrl Is Button Then
                Dim btn = DirectCast(ctrl, Button)
                btn.BackColor = If(IsDarkMode, DarkPanelBg, LightPanelBg)
                btn.ForeColor = If(IsDarkMode, DarkText, LightText)
                btn.FlatStyle = FlatStyle.Flat
                btn.FlatAppearance.BorderColor = If(IsDarkMode, DarkBorder, LightBorder)

            ElseIf TypeOf ctrl Is Label Then
                Dim lbl = DirectCast(ctrl, Label)
                If lbl.ForeColor = LightSubText OrElse lbl.ForeColor = Color.Gray Then
                    lbl.ForeColor = If(IsDarkMode, DarkSubText, LightSubText)
                Else
                    lbl.ForeColor = If(IsDarkMode, DarkText, LightText)
                End If

            ElseIf TypeOf ctrl Is CheckBox Then
                Dim chk = DirectCast(ctrl, CheckBox)
                chk.ForeColor = If(IsDarkMode, DarkText, LightText)

            ElseIf TypeOf ctrl Is TabControl Then
                Dim tc = DirectCast(ctrl, TabControl)
                tc.BackColor = If(IsDarkMode, DarkFormBg, LightFormBg)
                For Each page As TabPage In tc.TabPages
                    page.BackColor = If(IsDarkMode, DarkFormBg, LightFormBg)
                    page.ForeColor = If(IsDarkMode, DarkText, LightText)
                    ApplyControlsTheme(page.Controls)
                Next

            ElseIf TypeOf ctrl Is ToolStrip OrElse TypeOf ctrl Is StatusStrip OrElse TypeOf ctrl Is MenuStrip Then
                Dim ts = DirectCast(ctrl, ToolStrip)
                ts.BackColor = If(IsDarkMode, DarkPanelBg, LightPanelBg)
                ts.ForeColor = If(IsDarkMode, DarkText, LightText)
                ApplyToolStripItemsTheme(ts.Items)

            ElseIf TypeOf ctrl Is ContextMenuStrip Then
                Dim cms = DirectCast(ctrl, ContextMenuStrip)
                cms.BackColor = If(IsDarkMode, DarkPanelBg, LightPanelBg)
                cms.ForeColor = If(IsDarkMode, DarkText, LightText)
                ApplyToolStripItemsTheme(cms.Items)

            ElseIf TypeOf ctrl Is WebView2 Then
                Dim brws = DirectCast(ctrl, WebView2)
                ApplyWebView2Theme(brws)
            End If

            ' Recurse into child controls if container
            If ctrl.HasChildren AndAlso Not (TypeOf ctrl Is TabControl) Then
                ApplyControlsTheme(ctrl.Controls)
            End If
        Next
    End Sub

    ''' <summary>
    ''' Recursively styles ToolStrip/MenuStrip items and sub-menus.
    ''' </summary>
    Private Shared Sub ApplyToolStripItemsTheme(ByVal items As ToolStripItemCollection)
        If items Is Nothing Then Return

        For Each item As ToolStripItem In items
            If item Is Nothing Then Continue For

            item.BackColor = If(IsDarkMode, DarkPanelBg, LightPanelBg)
            item.ForeColor = If(IsDarkMode, DarkText, LightText)

            If TypeOf item Is ToolStripDropDownItem Then
                Dim dropDownItem = DirectCast(item, ToolStripDropDownItem)
                If dropDownItem.HasDropDownItems Then
                    ApplyToolStripItemsTheme(dropDownItem.DropDownItems)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Applies PreferredColorScheme (Dark or Light) and background color to a WebView2 control.
    ''' </summary>
    Public Shared Sub ApplyWebView2Theme(ByVal brws As WebView2)
        If brws IsNot Nothing Then
            Try
                brws.DefaultBackgroundColor = If(IsDarkMode, DarkFormBg, Color.White)
                brws.BackColor = If(IsDarkMode, DarkFormBg, LightFormBg)
                If brws.CoreWebView2 IsNot Nothing Then
                    brws.CoreWebView2.Profile.PreferredColorScheme = If(IsDarkMode, CoreWebView2PreferredColorScheme.Dark, CoreWebView2PreferredColorScheme.Light)
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error setting WebView2 PreferredColorScheme: " & ex.Message)
            End Try
        End If
    End Sub

End Class
