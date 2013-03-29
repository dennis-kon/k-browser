Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text
Imports Microsoft.Win32
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Windows.Forms

Public Class Form1


    Public full As Boolean = False
    Private Declare Function SetWindowPos Lib "user32.dll" Alias "SetWindowPos" (ByVal hWnd As IntPtr, ByVal hWndIntertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    Private Declare Function GetSystemMetrics Lib "user32.dll" Alias "GetSystemMetrics" (ByVal Which As Integer) As Integer

    Public Sub NormalMode()
        Me.WindowState = FormWindowState.Normal
        Me.FormBorderStyle = FormBorderStyle.Sizable

        Me.TopMost = False
    End Sub
    Private Sub FullScreen()
        Me.WindowState = FormWindowState.Maximized
        Me.FormBorderStyle = FormBorderStyle.None
        Me.TopMost = True
    End Sub
    Public WithEvents oDoc As HtmlDocument
    Private Function IsPopupWindow() As Boolean
        On Error Resume Next
        If CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Document.ActiveElement.TagName = "BODY" Or CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Document.ActiveElement.TagName = "IFRAME" Then
            IsPopupWindow = True
        Else
            IsPopupWindow = False
        End If
    End Function

    Private Sub RunPhishingFilter()
        Dim BadLink As Boolean = False
        Dim oEl As HtmlElement
        Dim s As String
        Dim li As ListItem
        Dim ofrm As New frmPhising
        For Each oEl In oDoc.Links
            For Each s In My.Settings.PhishingSites
                If InStr(oEl.GetAttribute("HREF"), s) Then
                    li = New ListItem
                    li.Text = oEl.GetAttribute("HREF")
                    ofrm.lbPhishing.Items.Add(li)
                    BadLink = True
                End If
            Next
        Next
        If BadLink = True Then
            ofrm.ShowDialog()
        Else
            ofrm.Dispose()
        End If
    End Sub



    Private Sub Loading(ByVal sender As Object, ByVal e As Windows.Forms.WebBrowserProgressChangedEventArgs)
        Try
            ProgressBar1.Maximum = e.MaximumProgress
            ProgressBar1.Value = e.CurrentProgress
            Label1.Text = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).StatusText

        Catch ex As Exception

        End Try

    End Sub

    Private Sub Done(ByVal sender As Object, ByVal e As Windows.Forms.WebBrowserDocumentCompletedEventArgs)
        Try
            ToolStripTextBox1.Text = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString
            TabControl1.SelectedTab.Text = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentTitle
            Me.Text = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentTitle & " | k-browser"
            If ToolStripTextBox1.Text = "about:blank" Then
                Me.Text = "k-browser"
            End If
            Label1.Text = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).StatusText
            ProgressBar1.Value = 0
            If Label1.Text = "Done" Then

            End If


        Catch ex As Exception

        End Try

    End Sub


    Private Sub NewWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewWindowToolStripMenuItem.Click
        Dim newform As New Form1 ' remember that form1 not form
        newform.Show()

    End Sub

    Private Sub NewTabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem.Click
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Dock = DockStyle.Fill
            tab.Text = "tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab

            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ProgressChanged, AddressOf Loading
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentCompleted, AddressOf Done

        Catch ex As Exception

        End Try
    End Sub

     

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Dock = DockStyle.Fill
            tab.Text = "tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab
            brws.Navigate("about:blank")
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ProgressChanged, AddressOf Loading
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentCompleted, AddressOf Done
            CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ScriptErrorsSuppressed = True

        Catch ex As Exception

        End Try

    End Sub



    Private Sub DeleteTabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteTabToolStripMenuItem.Click
        TabControl1.Controls.Remove(TabControl1.SelectedTab)

    End Sub

    Private Sub OpenFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFileToolStripMenuItem.Click
        Dim cdlOpen As New OpenFileDialog
        Try
            cdlOpen.Filter = "HTML Files (*.html)|*.html|TextFiles" & _
                "(*.txt)|*.txt|Gif Files (*.gif)|*.gif|JPEG Files (*.jpg)|*.jpeg|" & _
                "PNG Files (*.png)|*.png|Art Files (*.art)|*.art|AU Fles (*.au)|*.au|" & _
                "AIFF Files (*.aif|*.aiff|XBM Files (*.xbm)|*.xbm|All Files (*.*)|*.*"
            cdlOpen.Title = " Open File "
            cdlOpen.ShowDialog()
            If cdlOpen.FileName > Nothing Then
                CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Navigate(cdlOpen.FileName)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try
    End Sub

    Private Sub SaveFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveFileToolStripMenuItem.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ShowSaveAsDialog()

    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ShowPrintDialog()
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ShowPrintPreviewDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        End

    End Sub

    Private Sub SourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceToolStripMenuItem.Click
        Dim client As New WebClient
        Dim url As String
        Dim newform2 As New System.Windows.Forms.Form
        Dim textboxx1 As New TextBox
        url = ToolStripTextBox1.Text
        Dim source As String = client.DownloadString(New Uri(url))
        newform2.Controls.Add(textboxx1)
        textboxx1.Multiline = True
        textboxx1.Dock = DockStyle.Fill
        url = textboxx1.Text
        textboxx1.Text = source
        newform2.Width = "900"
        newform2.Height = "700"
        newform2.Show()
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ShowPropertiesDialog()
    End Sub



    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Navigate(ToolStripTextBox1.Text)
            My.Settings.History.Add(CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString)
            My.Settings.Save()
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ProgressChanged, AddressOf Loading
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentCompleted, AddressOf Done

        Catch ex As Exception

        End Try
    End Sub

    Private Sub BookmarkThisPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BookmarkThisPageToolStripMenuItem.Click
        Try
            My.Settings.Bookmarks.Add(CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString)
            My.Settings.Save()
            MsgBox(CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString & " Has Been Bookmarked!", MsgBoxStyle.OkOnly, "k-browser")

        Catch ex As Exception

        End Try

    End Sub

    Private Sub ViewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem1.Click
        Bookmarks.ShowDialog()

    End Sub

    Private Sub HistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryToolStripMenuItem.Click
        History.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).GoBack()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).GoForward()
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Refresh()
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).GoHome()
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        Try
            CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Navigate(ToolStripTextBox1.Text)
            My.Settings.History.Add(CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString)
            My.Settings.Save()
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ProgressChanged, AddressOf Loading
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentCompleted, AddressOf Done

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Dock = DockStyle.Fill
            tab.Text = "tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab

            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ProgressChanged, AddressOf Loading
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentCompleted, AddressOf Done

        Catch ex As Exception

        End Try
    End Sub
    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        TabControl1.Controls.Remove(TabControl1.SelectedTab)

    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Document.ExecCommand("Cut", False, vbNull)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        'WebBrowser1.Document.ExecCommand("copy", False, vbNull)
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Document.ExecCommand("copy", False, vbNull)
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        'WebBrowser1.Document.ExecCommand("paste", False, vbNull)
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Document.ExecCommand("paste", False, vbNull)
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        'WebBrowser1.Document.ExecCommand("SelectAll", False, vbNull)
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Document.ExecCommand("SelectAll", False, vbNull)
    End Sub

    Private Sub SeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeToolStripMenuItem.Click
        ' WebBrowser1.Focus()
        '            SendKeys.Send("^f")
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Focus()
        SendKeys.Send("^f")
    End Sub
    Private Enum Exec
        OLECMDID_OPTICAL_ZOOM = 63
    End Enum
    Private Enum ExecOpt
        OLECMDEXECOPT_DODEFAULT = 0
        OLECMDEXECOPT_PROMPTUSER = 1
        OLECMDEXECOPT_DONTPROMPTUSER = 2
        OLECMDEXECOPT_SHOWHELP = 3
    End Enum
    Private Sub ZoomInToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomInToolStripMenuItem.Click
        Try
            Dim Res As Object = Nothing
            Dim MyWeb As Object
            MyWeb = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ActiveXInstance
            MyWeb.ExecWB(Exec.OLECMDID_OPTICAL_ZOOM, _
                  ExecOpt.OLECMDEXECOPT_DONTPROMPTUSER, 150, IntPtr.Zero)
        Catch ex As Exception
            '  MsgBox("Error:" & ex.Message)

        End Try


        My.Settings.zoom = 150
        My.Settings.Save()

    End Sub

    Private Sub ZoomOutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomOutToolStripMenuItem.Click
        Try
            Dim Res As Object = Nothing
            Dim MyWeb As Object
            MyWeb = CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ActiveXInstance
            MyWeb.ExecWB(Exec.OLECMDID_OPTICAL_ZOOM, _
                  ExecOpt.OLECMDEXECOPT_DONTPROMPTUSER, 100, IntPtr.Zero)
        Catch ex As Exception
            '  MsgBox("Error:" & ex.Message)

        End Try


        My.Settings.zoom = 100
        My.Settings.Save()
    End Sub
    Private Sub FullScreenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FullScreenToolStripMenuItem.Click
        If full Then
            NormalMode()
            full = False
        Else
            FullScreen()
            full = True
        End If
    End Sub

     

    Private Sub InternetSettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InternetSettingsToolStripMenuItem.Click
        Shell("rundll32.exe shell32.dll,Control_RunDLL inetcpl.cpl,,0", vbNormalFocus)

    End Sub

    Private Sub AboutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AboutToolStripMenuItem.Click
        AboutBox.ShowDialog()
    End Sub

    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Navigate("http://k-browser.site44.com/download.html")
    End Sub

    Private Sub DownloadManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadManagerToolStripMenuItem.Click
        mainForm.ShowDialog()
    End Sub
  
    Private Sub CookieViewerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CookieViewerToolStripMenuItem.Click
        System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351")
    End Sub

    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton8.Click
        frmRss.ShowDialog()
    End Sub

    Private Sub ToolStripButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton9.Click
        Try
            My.Settings.Bookmarks.Add(CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString)
            My.Settings.Save()
            MsgBox(CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Url.ToString & " Has Been Bookmarked!", MsgBoxStyle.OkOnly, "k-browser")

        Catch ex As Exception

        End Try
    End Sub

    Private Sub SetHomePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetHomePageToolStripMenuItem.Click
        frmRss.ShowDialog()
    End Sub

    Private Sub NewTabToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem1.Click
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Dock = DockStyle.Fill
            tab.Text = "tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab

            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).ProgressChanged, AddressOf Loading
            AddHandler CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).DocumentCompleted, AddressOf Done

        Catch ex As Exception

        End Try
    End Sub

    Private Sub ToolStripTextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles ToolStripTextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            CType(TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Navigate(ToolStripTextBox1.Text)
            History.ListBox1.Items.Add(ToolStripTextBox1.Text)
        End If

    End Sub
End Class
