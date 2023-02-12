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
Imports System.Runtime.InteropServices

Public Class Form1

    Public WithEvents wb As WebBrowser
    Dim isUserAgentSet As Boolean
    Dim MyUserAgent As String = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/6.0; K-Browser 4.8.1)"
    Dim rightNow As DateTime = DateTime.Now
    Dim tab As New TabPage
    Dim brws As New WebBrowser
    Public full As Boolean = False
    Public Event FileDownload As EventHandler
    Public WithEvents oDoc As HtmlDocument
    Private Declare Function SetWindowPos Lib "user32.dll" Alias "SetWindowPos" (ByVal hWnd As IntPtr, ByVal hWndIntertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    Private Declare Function GetSystemMetrics Lib "user32.dll" Alias "GetSystemMetrics" (ByVal Which As Integer) As Integer

    Public Sub New()
        InitializeComponent()
        wb = New WebBrowser
        isUserAgentSet = False
    End Sub

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

    Private Function IsPopupWindow() As Boolean
        On Error Resume Next
        If wb.Document.ActiveElement.TagName = "BODY" Or wb.Document.ActiveElement.TagName = "IFRAME" Then
            IsPopupWindow = True
        Else
            IsPopupWindow = False
        End If
    End Function

    Private Sub Loading(ByVal sender As Object, ByVal e As Windows.Forms.WebBrowserProgressChangedEventArgs)
        Try
            ProgressBar1.Maximum = e.MaximumProgress
            ProgressBar1.Value = e.CurrentProgress
            Label1.Text = brws.StatusText
        Catch ex As Exception
        End Try
    End Sub

 

    Private Sub NewWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewWindowToolStripMenuItem.Click
        Dim newform As New Form1
        ' remember that form1 not form
        newform = New Form1
        newform.Show()
    End Sub

    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyValue = Keys.F6 Then
            ToolStripTextBox1.Focus()
        End If
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            wb = brws
            brws.Name = "k-Browser"
            brws.Dock = DockStyle.Fill
            tab.Text = wb.DocumentTitle
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab
            wb.Navigate(Application.StartupPath & "/homepage/index.html")
            Me.Size = My.Settings.MainSize
            Me.Location = My.Settings.MainLocation
            AddHandler brws.ProgressChanged, AddressOf Loading

            brws.ScriptErrorsSuppressed = True
            Dim nav As String = wb.Document.ActiveElement.GetAttribute("href")
            wb.Navigate(nav)
        Catch ex As Exception
        End Try
        CalendarToolStripMenuItem.Text = DateTime.Now.ToLongDateString()
        Label1.Text = brws.StatusText
        Me.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
        Me.BackColor = System.Drawing.Color.Transparent
        wb.IsWebBrowserContextMenuEnabled = True
       
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
                wb.Navigate(cdlOpen.FileName)
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message.ToString)
        End Try
    End Sub

    Private Sub SaveFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveFileToolStripMenuItem.Click
        wb.ShowSaveAsDialog()
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        wb.ShowPrintDialog()
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        wb.ShowPrintPreviewDialog()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Are you sure you want to exit?"   ' shows your message you can change it
        style = MsgBoxStyle.Information Or _
            MsgBoxStyle.YesNo 'The dialog will be a Yes No answer
        title = "K-Browser"   ' What did you name you application?
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Yes Then   ' if the user chooses Yes it is going to execute the Me.Close() which will close the programme
            'Else it will still show up.

            Me.Close()

        End If
    End Sub

    Private Sub SourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceToolStripMenuItem.Click
        Source.Show()
        Source.RichTextBox1.Text = Me.wb.DocumentText
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        wb.ShowPropertiesDialog()
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            wb.Navigate(ToolStripTextBox1.Text)
            My.Settings.History.Add(wb.Url.ToString)
            My.Settings.Save()
            AddHandler wb.ProgressChanged, AddressOf Loading

            History.ListBox1.Items.Add(ToolStripTextBox1.Text)
        Catch ex As Exception
        End Try
    End Sub

    Private Sub BookmarkThisPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BookmarkThisPageToolStripMenuItem.Click
        Try
            My.Settings.Bookmarks.Add(wb.Url.ToString)
            My.Settings.Save()
            MsgBox(wb.Url.ToString & " Has Been Bookmarked!", MsgBoxStyle.OkOnly, "K-Browser")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ViewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem1.Click
        Bookmarks.ShowDialog()
    End Sub

    Private Sub HistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryToolStripMenuItem.Click
        History.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back.Click
        wb.GoBack()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        wb.GoForward()
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        wb.Refresh()
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        wb.GoHome()
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        Try
            wb.Navigate(ToolStripTextBox1.Text)
            My.Settings.History.Add(wb.Url.ToString)
            My.Settings.Save()
            AddHandler wb.ProgressChanged, AddressOf Loading
            History.ListBox1.Items.Add(ToolStripTextBox1.Text)
            wb.ScriptErrorsSuppressed = True
        Catch ex As Exception
        End Try
        img.BackgroundImage = Nothing
    End Sub

    Private Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Name = "WebBrowser"
            brws.Dock = DockStyle.Fill
            tab.Text = "New Tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab
            AddHandler brws.ProgressChanged, AddressOf Loading

            brws.ScriptErrorsSuppressed = True
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        TabControl1.Controls.Remove(TabControl1.SelectedTab)
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        wb.Document.ExecCommand("Cut", False, vbNull)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        'WebBrowser1.Document.ExecCommand("copy", False, vbNull)
        wb.Document.ExecCommand("copy", False, vbNull)
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        'WebBrowser1.Document.ExecCommand("paste", False, vbNull)
        wb.Document.ExecCommand("paste", False, vbNull)
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        'WebBrowser1.Document.ExecCommand("SelectAll", False, vbNull)
        wb.Document.ExecCommand("SelectAll", False, vbNull)
    End Sub

    Private Sub SeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeToolStripMenuItem.Click
        ' WebBrowser1.Focus()
        ' SendKeys.Send("^f")
        wb.Focus()
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
            MyWeb = wb.ActiveXInstance
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
            MyWeb = wb.ActiveXInstance
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

    Private Sub DownloadManagerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DownloadManagerToolStripMenuItem.Click
        downman.ShowDialog()
    End Sub

    Private Sub CookieViewerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CookieViewerToolStripMenuItem.Click
        System.Diagnostics.Process.Start("rundll32.exe", "InetCpl.cpl,ClearMyTracksByProcess 4351")
    End Sub

    Private Sub ToolStripButton8_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton8.Click
        Rss.ShowDialog()
    End Sub

    Private Sub ToolStripButton9_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton9.Click
        Try
            My.Settings.Bookmarks.Add(wb.Url.ToString)
            My.Settings.Save()
            MsgBox(wb.Url.ToString & "Has Been Bookmarked!", MsgBoxStyle.OkOnly, "K-Browser")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub SetHomePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetHomePageToolStripMenuItem.Click
        Rss.ShowDialog()
    End Sub

    Private Sub NewTabToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem1.Click
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Dock = DockStyle.Fill
            tab.Text = "New Tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab

            AddHandler wb.ProgressChanged, AddressOf Loading

            wb.ScriptErrorsSuppressed = True
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ToolStripTextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripTextBox1.GotFocus
        ToolStripTextBox1.BackColor = Color.White
        ToolStripTextBox1.SelectAll()
    End Sub

    Private Sub ToolStripTextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles ToolStripTextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            wb.Navigate(ToolStripTextBox1.Text)
        End If
    End Sub

    Private Sub CloseTabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseTabToolStripMenuItem.Click
        TabControl1.Controls.Remove(TabControl1.SelectedTab)
    End Sub

    Private Sub ExitToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem1.Click
        Me.Close()
    End Sub


    Private Sub ToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem1.Click
        Dim lOpen As New OpenFileDialog
        lOpen.FileName = ""
        lOpen.Filter = "PDF Files(*.pdf)|*.pdf|All Files(*.*)|*.*"
        lOpen.ShowDialog()
        If lOpen.FileName <> "" Then
            wb.Navigate(lOpen.FileName)
        End If
    End Sub

    Private Sub ToolStripButton1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        Me.brws.Navigate(My.Settings.sem)
        wb.Navigate("http://www.google.com./search?hl=en&q=" & searchTextBox2.Text)
        History.ListBox1.Items.Add(searchTextBox2.Text)
    End Sub

    Private Sub searchTextBox2_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles searchTextBox2.KeyDown
        If e.KeyCode = Keys.Enter Then
            wb.Navigate("http://www.google.com/search?hl=en&q=" & searchTextBox2.Text)
            History.ListBox1.Items.Add(searchTextBox2.Text)
        End If
    End Sub

    Private Sub SendALinkToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendALinkToolStripMenuItem.Click
        System.Diagnostics.Process.Start("outlook")
    End Sub

    Private Sub wb_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged
        Try
            If Me.WindowState = FormWindowState.Normal Then
                My.Settings.MainLocation = Me.Location
                My.Settings.Save()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub wb_Click(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ContextMenuStrip1.Show(CType(sender, Control), e.Location)
        End If
    End Sub

    Private Sub CookieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CookieToolStripMenuItem.Click
        CookieViewer.ShowDialog()
    End Sub

    Private Sub SubmitFeedbackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubmitFeedbackToolStripMenuItem.Click
        wb.Navigate("https://www.k-browser.com/")
    End Sub

    Private Sub ShareThisOnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShareThisOnToolStripMenuItem.Click
        fb.ShowDialog()
    End Sub

    Private Sub ToolStripSplitButton1_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton1.ButtonClick
        wb.Navigate("https://www.facebook.com/login.php")
        History.ListBox1.Items.Add(searchTextBox2.Text)
    End Sub

    Private Sub ToolStripSplitButton2_ButtonClick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton2.ButtonClick
        wb.Navigate("https://twitter.com/login")
        History.ListBox1.Items.Add(searchTextBox2.Text)
    End Sub

    Private Sub TweetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TweetToolStripMenuItem.Click
        tw.ShowDialog()
    End Sub

    Private Sub HideMainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideMainMenuToolStripMenuItem.Click
        If Me.MenuStrip1.Visible Then
            Me.MenuStrip1.Visible = False
            Me.ShowToolStripMenuItem.Enabled = True
        Else : Me.MenuStrip1.Visible = True
        End If
    End Sub

    Private Sub ShowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowToolStripMenuItem.Click
        If Me.ToolStrip1.Visible Then
            Me.ToolStrip1.Visible = False
            Me.ShowToolStripMenuItem.Enabled = True
        Else : Me.ToolStrip1.Visible = True
        End If
    End Sub

    Private Sub ShowMainMenuToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowMainMenuToolStripMenuItem.Click
        If Me.MenuStrip1.Visible = False Then
            Me.MenuStrip1.Visible = True
        End If
    End Sub

    Private Sub ShowNavigationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowNavigationToolStripMenuItem.Click
        If Me.ToolStrip1.Visible = False Then
            Me.ToolStrip1.Visible = True
        End If
    End Sub

    Private Sub HideBarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HideBarToolStripMenuItem.Click
        If Me.StatusStrip1.Visible Then
            Me.StatusStrip1.Visible = False
            Me.ShowToolStripMenuItem.Enabled = True
        Else : Me.StatusStrip1.Visible = True
        End If
    End Sub

    Private Sub ShowStatusBarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ShowStatusBarToolStripMenuItem.Click
        If Me.StatusStrip1.Visible = False Then
            Me.StatusStrip1.Visible = True
        End If
    End Sub

    Private Sub ToolStripMenuItem4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem4.Click
        ToolStripTextBox1.Focus()
        ToolStripTextBox1.Text = ""
        ToolStripTextBox1.Focus()
    End Sub

    Private Sub FtpClientToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FtpClientToolStripMenuItem.Click
        ftp.ShowDialog()
    End Sub

    Private Sub BingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BingToolStripMenuItem.Click
        wb.Navigate("http://www.bing.com/")
    End Sub

    Private Sub GoogleToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoogleToolStripMenuItem1.Click
        wb.Navigate("http://www.google.com/")
    End Sub

    Private Sub YahooToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YahooToolStripMenuItem.Click
        wb.Navigate("http://www.yahoo.com/")
    End Sub

    Private Sub EBayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EBayToolStripMenuItem.Click
        wb.Navigate("http://www.ebay.com/")
    End Sub

    Private Sub MSNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MSNToolStripMenuItem.Click
        wb.Navigate("http://www.msn.com/?st=1")
    End Sub

    Private Sub DuckDuckGoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuckDuckGoToolStripMenuItem.Click
        wb.Navigate("https://duckduckgo.com/")
    End Sub

    Private Sub DogpileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DogpileToolStripMenuItem.Click
        wb.Navigate("http://www.dogpile.com/")
    End Sub

    Private Sub WebCrawlerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebCrawlerToolStripMenuItem.Click
        wb.Navigate("http://www.webcrawler.com/")
    End Sub

    Private Sub GopherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        wb.Navigate("http://wt.gopherite.org/")
    End Sub

    Private Sub LycosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LycosToolStripMenuItem.Click
        wb.Navigate("http://www.lycos.com/")
    End Sub

    Private Sub AccuWeatherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccuWeatherToolStripMenuItem.Click
        wb.Navigate("http://www.accuweather.com")
    End Sub

    Private Sub VimeoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VimeoToolStripMenuItem.Click
        wb.Navigate("https://vimeo.com/")
    End Sub

    Private Sub MediaToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        wb = Me.TabControl1.SelectedTab.Controls(0)
    End Sub

    Private Sub wb_DocumentCompleted(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles wb.DocumentCompleted
        History.ListBox1.Items.Add(ToolStripTextBox1.Text)
        isUserAgentSet = False
        Try
            ToolStripTextBox1.Text = wb.Url.ToString

            Me.Text = wb.DocumentTitle & " | K-Browser"
            If ToolStripTextBox1.Text = "about:blank" Then
                Me.Text = "K-Browser"
            End If
            Label1.Text = brws.StatusText
            ProgressBar1.Value = 0
            If Label1.Text = "Done" Then
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub wb_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles wb.Navigated

        ToolStripTextBox1.Text = wb.Url.ToString
        Dim url As Uri = New Uri(ToolStripTextBox1.Text)
        If url.HostNameType = UriHostNameType.Dns Then
            Dim icons = "https://" & url.Host & "/favicon.ico"
            Dim req As System.Net.WebRequest = System.Net.HttpWebRequest.Create(icons)
            Dim res As System.Net.HttpWebResponse = req.GetResponse()
            Dim stream As System.IO.Stream = res.GetResponseStream()
            Dim fav = Image.FromStream(stream)
            img.BackgroundImage = fav
        Else
            img.BackgroundImage = Nothing
        End If
    End Sub

    Private Sub wb_Navigating(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatingEventArgs) Handles wb.Navigating

    End Sub

    Private Sub wb_NewWindow(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles wb.NewWindow
        Dim myElement As HtmlElement = wb.Document.ActiveElement
        Dim target As String = myElement.GetAttribute("href")
        Dim newInstance As New Form1
        newInstance.Show()
        newInstance.wb.Navigate(target)
        e.Cancel = True
    End Sub


    Private Sub FacebookToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FacebookToolStripMenuItem.Click
        wb.Navigate("https://www.facebook.com")
    End Sub

    Private Sub TwitterToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TwitterToolStripMenuItem.Click
        wb.Navigate("https://twitter.com")
    End Sub

    Private Sub GoogleToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoogleToolStripMenuItem.Click
        wb.Navigate("https://plus.google.com")
    End Sub

    Private Sub LinkedinToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkedinToolStripMenuItem.Click
        wb.Navigate("http://www.linkedin.com")
    End Sub

    Private Sub InstagramToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles InstagramToolStripMenuItem.Click
        wb.Navigate("http://instagram.com/")
    End Sub

    Private Sub BadooToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BadooToolStripMenuItem.Click
        wb.Navigate("http://badoo.com")
    End Sub

    Private Sub VkToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VkToolStripMenuItem.Click
        wb.Navigate("http://vk.com/")
    End Sub

    Private Sub PinterestToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PinterestToolStripMenuItem.Click
        wb.Navigate("http://pinterest.com/")
    End Sub

    Private Sub FormspringToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FormspringToolStripMenuItem.Click
        wb.Navigate("http://www.formspring.me/")
    End Sub

    Private Sub KeekToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KeekToolStripMenuItem.Click
        wb.Navigate("http://www.keek.com")
    End Sub

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Settings.ShowDialog()
    End Sub

    Private Sub RunPhishingFilter()
        Dim BadLink As Boolean = False
        Dim oEl As HtmlElement
        Dim s As String
        Dim li As ListItem
        Dim ofrm As New Phising
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

    Private Sub PictureBox4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PictureBox4.Click
        Phising.ShowDialog()
    End Sub


    Private Sub BackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BackToolStripMenuItem.Click
        wb.GoBack()
    End Sub

    Private Sub ForwardToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForwardToolStripMenuItem.Click
        wb.GoForward()
    End Sub


    Private Sub StopToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StopToolStripMenuItem1.Click
        wb.Stop()
    End Sub

    Private Sub ImageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ImageToolStripMenuItem.Click
        Dim b As New Bitmap(wb.ClientSize.Width, wb.ClientSize.Height)
        Dim g As Graphics = Graphics.FromImage(b)
        g.CopyFromScreen(Me.PointToScreen(wb.Location), New Point(0, 0), wb.ClientSize)
        'The bitmap is ready. Do whatever you please with it!
        b.Save("c:\screenshot.jpg")
        MessageBox.Show("Screen Shot Saved! in C:/", _
            "Save the current screen")
    End Sub
    Private Sub Window_Error(ByVal sender As Object, ByVal e As HtmlElementErrorEventArgs)
        ' Ignore the error and suppress the error dialog box. 
        e.Handled = True
    End Sub


    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        wb.Navigate("https://www.k-browser.com/")
    End Sub

    Private Sub CPUStatsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CPUStatsToolStripMenuItem.Click
        wb.ShowPageSetupDialog()
    End Sub

    Private Sub Form1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        If e.Button = MouseButtons.Left Then
            Console.WriteLine("Left Mouse Button was clicked!")
        ElseIf e.Button = MouseButtons.Middle Then
            Console.WriteLine("Middle Mouse Button was clicked!")
        End If
    End Sub

    Private Sub mnuLeftToRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLeftToRight.Click
        wb.Document.RightToLeft = False
        mnuRightToLeft.Checked = False
    End Sub

    Private Sub mnuRightToLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRightToLeft.Click
        wb.Document.RightToLeft = True
        mnuLeftToRight.Checked = False
    End Sub

    Private Sub CalendarToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CalendarToolStripMenuItem.Click
        Form12.Show()
    End Sub

    Private Sub OpePageInNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpePageInNToolStripMenuItem.Click
        On Error Resume Next
        Dim psi As New System.Diagnostics.ProcessStartInfo("iexplore")
        psi.Arguments = ToolStripTextBox1.Text
        System.Diagnostics.Process.Start(psi)
    End Sub

    Private Sub ToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem2.Click
        Dim newform As New Form1
        ' remember that form1 not form
        newform = New Form1
        newform.Show()
    End Sub

    Private Sub TaskToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TaskToolStripMenuItem.Click
        task_manager.ShowDialog()
    End Sub

    Private Sub ToolStripTextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripTextBox1.LostFocus
        ToolStripTextBox1.BackColor = Color.Snow
    End Sub

    Private Sub wb_StatusTextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles wb.StatusTextChanged
        Label1.Text = wb.StatusText
    End Sub
    Private Sub wb_Navigate(ByVal address As String)

        If String.IsNullOrEmpty(address) Then Return
        If address.Equals("about:blank") Then Return
        If Not address.StartsWith("http://") And _
            Not address.StartsWith("https://") Then
            address = "http://" & address
        End If

    End Sub

    Private Sub ToolStripTextBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripTextBox1.Click
        ToolStripTextBox1.SelectAll()
    End Sub
    Private Sub wb_CanGoBackChanged(ByVal sender As Object, ByVal e As EventArgs) Handles wb.CanGoBackChanged
        ToolStripButton1.Enabled = wb.CanGoBack
    End Sub

    Private Sub wb_CanGoForwardChanged(ByVal sender As Object, ByVal e As EventArgs) Handles wb.CanGoForwardChanged
        ToolStripButton2.Enabled = wb.CanGoForward
    End Sub

    Private Sub ReloadToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadToolStripMenuItem1.Click
        wb.Refresh()
    End Sub

    Private Sub AutoToolStripMenuItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoToolStripMenuItem.CheckedChanged
        Select Case AutoToolStripMenuItem.Checked
            Case True
                'enable timer to refresh every 10000 milliseconds or 10 Seconds 
                '(Interval property)
                Timer1.Interval = 10000
                Timer1.Enabled = True
            Case Else
                Timer1.Enabled = False
        End Select
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        wb.Refresh(WebBrowserRefreshOption.Completely)
    End Sub

    Private Sub NewTabToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem2.Click
        Try
            Dim tab As New TabPage
            Dim brws As New WebBrowser
            brws.Name = "WebBrowser"
            brws.Dock = DockStyle.Fill
            tab.Text = "New Tab"
            tab.Controls.Add(brws)
            Me.TabControl1.TabPages.Add(tab)
            Me.TabControl1.SelectedTab = tab
            AddHandler brws.ProgressChanged, AddressOf Loading

            brws.ScriptErrorsSuppressed = True
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DefualtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefualtToolStripMenuItem.Click
        wb.Navigate("file://" & My.Application.Info.DirectoryPath & "/homepage/index.html")
    End Sub

    Private Sub HomeToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomeToolStripMenuItem1.Click
        wb.GoHome()
    End Sub

    Private Sub BlancPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlancPageToolStripMenuItem.Click
        wb.Navigate("about:blank")
    End Sub

    Private Sub ToolStripSplitButton1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripSplitButton1.DoubleClick
        ToolStripTextBox1.Focus()
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        wb.GoHome()
    End Sub

    Private Sub largestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        wb().Document.ExecCommand("FontSize", True, "4")
        largestToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        wb().Document.ExecCommand("FontSize", True, "0")
        smallestToolStripMenuItem.Checked = True
    End Sub

    Private Sub largerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        wb().Document.ExecCommand("FontSize", True, "3")
        largerToolStripMenuItem.Checked = True
    End Sub

    Private Sub mediumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mediumToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        wb().Document.ExecCommand("FontSize", True, "2")
        mediumToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        wb().Document.ExecCommand("FontSize", True, "1")
        smallerToolStripMenuItem.Checked = True
    End Sub

    Dim mg_enable As Boolean '// Is gesturing?
    Dim mg_x, mg_y, mg_dist As Integer '// original x, y location of mouse, gesture distance
    Dim mg_direction As String '// Result of gestures

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = Windows.Forms.MouseButtons.Right Then '// If it's right.
            mg_enable = True '// Start mouse gesture!
            mg_direction = "" '// Init gestures
            mg_dist = 20 '// 1 gesture per 20 dot moving
            mg_x = e.X '// Keep X location of mouse when right button was down.
            mg_y = e.Y '// Keep Y location of mouse when right button was down.
        End If
    End Sub

    Private Sub Form1_MouseMove(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseMove
        '// if in gesturing
        If mg_enable Then
            '// 
            If Math.Abs(mg_x - e.X) > mg_dist Or Math.Abs(mg_y - e.Y) > mg_dist Then
                '// direction decision by moving distance of x y
                If Math.Abs(mg_x - e.X) > Math.Abs(mg_y - e.Y) Then
                    '// if direction is left
                    If mg_x > e.X Then
                        '// set new location of mouse
                        mg_x = e.X
                        '// if it's not same last gesture then add gesture
                        If Microsoft.VisualBasic.Right(mg_direction, 1) <> "L" Then
                            mg_direction = mg_direction & "L"
                        End If
                    Else
                        mg_x = e.X
                        If Microsoft.VisualBasic.Right(mg_direction, 1) <> "R" Then
                            mg_direction = mg_direction & "R"
                        End If
                    End If
                Else
                    If mg_y > e.Y Then
                        mg_y = e.Y
                        If Microsoft.VisualBasic.Right(mg_direction, 1) <> "U" Then
                            mg_direction = mg_direction & "U"
                        End If
                    Else
                        mg_y = e.Y
                        If Microsoft.VisualBasic.Right(mg_direction, 1) <> "D" Then
                            mg_direction = mg_direction & "D"
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub Form1_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseUp
        If e.Button = Windows.Forms.MouseButtons.Right Then '// If it's right.
            mg_enable = False '// Stop mouse gesture.
            Select Case mg_direction '// Execute gesture.
                Case "DR" '// Exit
                    End
                Case "RU" '// Maximize window / Restore Window Size
                    If Me.WindowState = FormWindowState.Normal Then
                        Me.WindowState = FormWindowState.Maximized
                    Else
                        Me.WindowState = FormWindowState.Normal
                    End If
            End Select

        End If
    End Sub


    Private Sub ProxySettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProxySettingsToolStripMenuItem.Click
        Form3.ShowDialog()
    End Sub

    Private Sub wb_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        Try
            My.Settings.MainSize = Me.Size
            My.Settings.Save()
        Catch ex As Exception

        End Try
    End Sub

    Private Sub wb_DocumentTitleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wb.DocumentTitleChanged
        Label1.Text = wb.DocumentTitle
    End Sub
    Public Function PopulateUrlList() As List(Of String)
        Dim regKey As String = "Software\Microsoft\Internet Explorer\TypedURLs"
        Dim subKey As RegistryKey = Registry.CurrentUser.OpenSubKey(regKey)
        Dim url As String
        Dim urlList As New List(Of String)()
        Dim counter As Integer = 1
        While True
            Dim sValName As String = "url" + counter.ToString()
            url = DirectCast(subKey.GetValue(sValName), String)
            If DirectCast(url, Object) Is Nothing Then
                Exit While
            End If
            urlList.Add(url)
            counter += 1
        End While
        Return urlList
    End Function


    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click
        On Error Resume Next
        Dim psi As New System.Diagnostics.ProcessStartInfo("iexplore")
        psi.Arguments = ToolStripTextBox1.Text
        System.Diagnostics.Process.Start(psi)
    End Sub

    Private Sub KBrowserToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KBrowserToolStripMenuItem.Click
        wb.Navigate("https://www.k-browser.com/")
    End Sub

    Private Sub SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SToolStripMenuItem.Click
        System.Diagnostics.Process.Start("mailto:""denkon24@yahoo.com")
    End Sub
End Class
