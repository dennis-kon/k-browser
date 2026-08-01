Imports System.IO
Imports System.Net
Imports Microsoft.Win32
Imports Microsoft.Web.WebView2.WinForms
Imports Microsoft.Web.WebView2.Core

Public Class Form1

    Private faviconCache As New Dictionary(Of String, Image)()
    Private Const MAX_FAVICON_CACHE As Integer = 100
    Public WithEvents wb As WebView2
    Dim isUserAgentSet As Boolean
    Dim MyUserAgent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0"
    Dim rightNow As DateTime = DateTime.Now
    Public full As Boolean = False
    Public Event FileDownload As EventHandler

    Public Sub New()
        SetBrowserFeatureControl()
        InitializeComponent()
        wb = New WebView2()
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

    Public Async Sub NavigateActiveTab(ByVal url As String)
        If String.IsNullOrWhiteSpace(url) Then Return
        Dim target As String = AppManager.FixURL(url)
        If wb IsNot Nothing Then
            If wb.CoreWebView2 IsNot Nothing Then
                wb.CoreWebView2.Navigate(target)
            Else
                wb.Source = New Uri(target)
            End If
        Else
            Await CreateNewTab(target)
        End If
    End Sub

    Public Sub OpenDevTools()
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
            wb.CoreWebView2.OpenDevToolsWindow()
        End If
    End Sub

    Private Sub SetBrowserFeatureControl()
        Try
            Dim exeName As String = System.IO.Path.GetFileName(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName)
            Dim targetExes As String() = {exeName, "K-Browser.exe", "K-Browser.vshost.exe", "devenv.exe"}
            
            Dim featureKeys As New Dictionary(Of String, Integer) From {
                {"FEATURE_BROWSER_EMULATION", 11001},
                {"FEATURE_GPU_RENDERING", 1},
                {"FEATURE_NATIVE_DOCUMENT_MODE", 1},
                {"FEATURE_SCRIPT_URL_MITIGATION", 1}
            }

            For Each featureKey As String In featureKeys.Keys
                Dim featureValue As Integer = featureKeys(featureKey)
                Using key As RegistryKey = Registry.CurrentUser.CreateSubKey("Software\Microsoft\Internet Explorer\Main\FeatureControl\" & featureKey)
                    If key IsNot Nothing Then
                        For Each targetExe As String In targetExes
                            If Not String.IsNullOrEmpty(targetExe) Then
                                key.SetValue(targetExe, featureValue, RegistryValueKind.DWord)
                            End If
                        Next
                    End If
                End Using
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Failed to set browser emulation registry key: " & ex.Message)
        End Try
    End Sub

    Private Sub NewWindowToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewWindowToolStripMenuItem.Click
        Dim newform As New Form1
        newform.Show()
    End Sub

    Private Sub Form1_KeyUp(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles Me.KeyUp
        If e.KeyValue = Keys.F6 Then
            ToolStripTextBox1.Focus()
        End If
    End Sub

    Private Async Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Dim brws = Await CreateNewTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
            Me.Size = My.Settings.MainSize
            Me.Location = My.Settings.MainLocation
        Catch ex As Exception
        End Try
        CalendarToolStripMenuItem.Text = DateTime.Now.ToLongDateString()
    End Sub



    Private Sub CloseCurrentTab()
        If TabControl1.SelectedTab IsNot Nothing Then
            Dim selectedTab As TabPage = TabControl1.SelectedTab
            If selectedTab.Controls.Count > 0 Then
                Dim browserControl As WebView2 = TryCast(selectedTab.Controls(0), WebView2)
                If browserControl IsNot Nothing Then
                    If browserControl.CoreWebView2 IsNot Nothing Then
                        RemoveHandler browserControl.CoreWebView2.NavigationStarting, AddressOf WebView2_NavigationStarting
                        RemoveHandler browserControl.CoreWebView2.NavigationCompleted, AddressOf WebView2_NavigationCompleted
                        RemoveHandler browserControl.CoreWebView2.SourceChanged, AddressOf WebView2_SourceChanged
                        RemoveHandler browserControl.CoreWebView2.DocumentTitleChanged, AddressOf WebView2_DocumentTitleChanged
                        RemoveHandler browserControl.CoreWebView2.HistoryChanged, AddressOf WebView2_HistoryChanged
                    End If
                    browserControl.Dispose()
                End If
            End If
            TabControl1.TabPages.Remove(selectedTab)
            selectedTab.Dispose()
        End If
    End Sub

    Private Sub DeleteTabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DeleteTabToolStripMenuItem.Click
        CloseCurrentTab()
    End Sub

    Private Sub OpenFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OpenFileToolStripMenuItem.Click
        Try
            Using cdlOpen As New OpenFileDialog()
                cdlOpen.Filter = "HTML Files (*.html)|*.html|Text Files (*.txt)|*.txt|Image Files (*.gif;*.jpg;*.jpeg;*.png)|*.gif;*.jpg;*.jpeg;*.png|Audio Files (*.au;*.aif;*.aiff)|*.au;*.aif;*.aiff|All Files (*.*)|*.*"
                cdlOpen.Title = "Open File"
                If cdlOpen.ShowDialog() = DialogResult.OK AndAlso Not String.IsNullOrEmpty(cdlOpen.FileName) Then
                    NavigateActiveTab(cdlOpen.FileName)
                End If
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error opening file: " & ex.Message)
        End Try
    End Sub

    Private Async Sub SaveFileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveFileToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
            Await wb.CoreWebView2.ExecuteScriptAsync("window.print()")
        End If
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then wb.CoreWebView2.ShowPrintUI(CoreWebView2PrintDialogKind.Browser)
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then wb.CoreWebView2.ShowPrintUI(CoreWebView2PrintDialogKind.Browser)
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Are you sure you want to exit?"   ' shows your message you can change it
        style = MsgBoxStyle.Information Or
            MsgBoxStyle.YesNo 'The dialog will be a Yes No answer
        title = "K-Browser"   ' What did you name you application?
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Yes Then   ' if the user chooses Yes it is going to execute the Me.Close() which will close the programme
            'Else it will still show up.

            Me.Close()

        End If
    End Sub

    Private Async Sub SourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
            Try
                Dim htmlJson As String = Await wb.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML")
                Dim htmlText As String = htmlJson
                If htmlText.StartsWith("""") AndAlso htmlText.EndsWith("""") Then
                    Try
                        htmlText = System.Text.RegularExpressions.Regex.Unescape(htmlText.Substring(1, htmlText.Length - 2))
                    Catch
                        htmlText = htmlJson.Substring(1, htmlJson.Length - 2).Replace("\""", """").Replace("\n", vbCrLf).Replace("\r", "").Replace("\\", "\")
                    End Try
                End If
                Source.Show()
                Source.RichTextBox1.Text = htmlText
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error getting HTML source: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        OpenDevTools()
    End Sub

    Private Sub BookmarkThisPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BookmarkThisPageToolStripMenuItem.Click
        Try
            If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
                Dim bmUrl As String = wb.CoreWebView2.Source
                If My.Settings.Bookmarks Is Nothing Then My.Settings.Bookmarks = New System.Collections.Specialized.StringCollection()
                My.Settings.Bookmarks.Add(bmUrl)
                My.Settings.Save()
                MsgBox(bmUrl & " Has Been Bookmarked!", MsgBoxStyle.OkOnly, "K-Browser")
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error bookmarking page: " & ex.Message)
        End Try
    End Sub

    Private Sub ViewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem1.Click
        Bookmarks.ShowDialog()
    End Sub

    Private Sub HistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryToolStripMenuItem.Click
        History.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back.Click
        If wb IsNot Nothing AndAlso wb.CanGoBack Then wb.GoBack()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        If wb IsNot Nothing AndAlso wb.CanGoForward Then wb.GoForward()
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        If wb IsNot Nothing Then wb.Reload()
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        NavigateActiveTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
    End Sub

    Private Sub ToolStripButton5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton5.Click
        If Not String.IsNullOrWhiteSpace(ToolStripTextBox1.Text) Then
            NavigateActiveTab(ToolStripTextBox1.Text)
        End If
    End Sub

    Private Async Sub ToolStripButton7_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton7.Click
        Try
            Await CreateNewTab()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        CloseCurrentTab()
    End Sub

    Private Async Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then Await wb.CoreWebView2.ExecuteScriptAsync("document.execCommand('cut')")
    End Sub

    Private Async Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then Await wb.CoreWebView2.ExecuteScriptAsync("document.execCommand('copy')")
    End Sub

    Private Async Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then Await wb.CoreWebView2.ExecuteScriptAsync("document.execCommand('paste')")
    End Sub

    Private Async Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then Await wb.CoreWebView2.ExecuteScriptAsync("document.execCommand('selectAll')")
    End Sub

    Private Sub SeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeToolStripMenuItem.Click
        If wb IsNot Nothing Then
            wb.Focus()
            SendKeys.Send("^f")
        End If
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
        If wb IsNot Nothing Then
            wb.ZoomFactor += 0.15
        End If
    End Sub

    Private Sub ZoomOutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomOutToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.ZoomFactor > 0.3 Then
            wb.ZoomFactor -= 0.15
        End If
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
            If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
                Dim url As String = wb.CoreWebView2.Source
                If My.Settings.Bookmarks Is Nothing Then My.Settings.Bookmarks = New System.Collections.Specialized.StringCollection()
                My.Settings.Bookmarks.Add(url)
                My.Settings.Save()
                MsgBox(url & " Has Been Bookmarked!", MsgBoxStyle.OkOnly, "K-Browser")
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error bookmarking page: " & ex.Message)
        End Try
    End Sub

    Private Sub SetHomePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetHomePageToolStripMenuItem.Click
        NavigateActiveTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
    End Sub

    Private Async Sub NewTabToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem1.Click
        Try
            Await CreateNewTab()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ToolStripTextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripTextBox1.GotFocus
        ToolStripTextBox1.BackColor = Color.White
        ToolStripTextBox1.SelectAll()
    End Sub

    Private Sub ToolStripTextBox1_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles ToolStripTextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            NavigateActiveTab(ToolStripTextBox1.Text)
        End If
    End Sub

    Private Sub CloseTabToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseTabToolStripMenuItem.Click
        CloseCurrentTab()
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
            NavigateActiveTab(lOpen.FileName)
        End If
    End Sub





    Private Sub SendALinkToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SendALinkToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("outlook") With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Unable to launch email client.", "K-Browser", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub Form1_LocationChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LocationChanged
        Try
            If Me.WindowState = FormWindowState.Normal Then
                My.Settings.MainLocation = Me.Location
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Form1_MouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseClick
        If e.Button = Windows.Forms.MouseButtons.Left Then
            ContextMenuStrip1.Show(CType(sender, Control), e.Location)
        End If
    End Sub

    Private Sub CookieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CookieToolStripMenuItem.Click
        CookieViewer.ShowDialog()
    End Sub

    Private Sub SubmitFeedbackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SubmitFeedbackToolStripMenuItem.Click
        NavigateActiveTab("https://www.k-browser.com/")
    End Sub

    Private Sub ShareThisOnToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles FacebookToolStripMenuItem.Click
        fb.ShowDialog()
    End Sub

    Private Sub TweetToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TwitterToolStripMenuItem.Click
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
        NavigateActiveTab("http://www.bing.com/")
    End Sub

    Private Sub GoogleToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GoogleToolStripMenuItem1.Click
        NavigateActiveTab("http://www.google.com/")
    End Sub

    Private Sub YahooToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles YahooToolStripMenuItem.Click
        NavigateActiveTab("http://www.yahoo.com/")
    End Sub

    Private Sub EBayToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles EBayToolStripMenuItem.Click
        NavigateActiveTab("http://www.ebay.com/")
    End Sub

    Private Sub MSNToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MSNToolStripMenuItem.Click
        NavigateActiveTab("http://www.msn.com/?st=1")
    End Sub

    Private Sub DuckDuckGoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DuckDuckGoToolStripMenuItem.Click
        NavigateActiveTab("https://duckduckgo.com/")
    End Sub

    Private Sub DogpileToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DogpileToolStripMenuItem.Click
        NavigateActiveTab("http://www.dogpile.com/")
    End Sub

    Private Sub WebCrawlerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles WebCrawlerToolStripMenuItem.Click
        NavigateActiveTab("http://www.webcrawler.com/")
    End Sub

    Private Sub GopherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NavigateActiveTab("http://wt.gopherite.org/")
    End Sub

    Private Sub LycosToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LycosToolStripMenuItem.Click
        NavigateActiveTab("http://www.lycos.com/")
    End Sub

    Private Sub AccuWeatherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AccuWeatherToolStripMenuItem.Click
        NavigateActiveTab("http://www.accuweather.com")
    End Sub

    Private Sub VimeoToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles VimeoToolStripMenuItem.Click
        NavigateActiveTab("https://vimeo.com/")
    End Sub




    Private Shared ReadOnly IgnoredUrls As New System.Collections.Generic.HashSet(Of String)()

    Public Async Function CreateNewTab(Optional ByVal targetUrl As String = "") As System.Threading.Tasks.Task(Of WebView2)
        Dim tab As New TabPage()
        Dim brws As New WebView2()
        brws.Name = "WebView2"
        brws.Dock = DockStyle.Fill
        tab.Text = "Loading..."
        tab.Controls.Add(brws)

        Me.TabControl1.TabPages.Add(tab)
        Me.TabControl1.SelectedTab = tab

        Try
            Await brws.EnsureCoreWebView2Async(Nothing)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("EnsureCoreWebView2Async failed: " & ex.Message)
        End Try

        If brws.CoreWebView2 IsNot Nothing Then
            brws.CoreWebView2.Settings.IsStatusBarEnabled = True
            brws.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = True
            brws.CoreWebView2.Settings.IsScriptEnabled = True
            brws.CoreWebView2.Settings.IsWebMessageEnabled = True
            brws.CoreWebView2.Settings.AreDevToolsEnabled = True

            AddHandler brws.CoreWebView2.NavigationStarting, AddressOf WebView2_NavigationStarting
            AddHandler brws.CoreWebView2.NavigationCompleted, AddressOf WebView2_NavigationCompleted
            AddHandler brws.CoreWebView2.SourceChanged, AddressOf WebView2_SourceChanged
            AddHandler brws.CoreWebView2.DocumentTitleChanged, AddressOf WebView2_DocumentTitleChanged
            AddHandler brws.CoreWebView2.HistoryChanged, AddressOf WebView2_HistoryChanged
        End If

        If Not String.IsNullOrEmpty(targetUrl) Then
            Dim targetFix As String = AppManager.FixURL(targetUrl)
            If brws.CoreWebView2 IsNot Nothing Then
                brws.CoreWebView2.Navigate(targetFix)
            Else
                brws.Source = New Uri(targetFix)
            End If
        End If

        Return brws
    End Function

    Private Sub WebView2_NavigationStarting(ByVal sender As Object, ByVal e As CoreWebView2NavigationStartingEventArgs)
        Dim url As String = e.Uri
        If String.IsNullOrEmpty(url) OrElse url = "about:blank" Then Return

        ' 1. Blocked Sites Check
        If My.Settings.BlockedSites IsNot Nothing Then
            For Each blockedUrl As String In My.Settings.BlockedSites
                If Not String.IsNullOrEmpty(blockedUrl) Then
                    If url.ToLower().Contains(blockedUrl.ToLower()) Then
                        e.Cancel = True
                        MsgBox("This site is blocked by K-Browser Settings.", MsgBoxStyle.Critical, "Access Blocked")
                        Return
                    End If
                End If
            Next
        End If

        ' 2. Phishing Sites Check
        If My.Settings.UsePhishingFilter AndAlso My.Settings.PhishingSites IsNot Nothing Then
            For Each phishingUrl As String In My.Settings.PhishingSites
                If Not String.IsNullOrEmpty(phishingUrl) Then
                    If url.ToLower().Contains(phishingUrl.ToLower()) Then
                        If IgnoredUrls.Contains(url) Then Return

                        e.Cancel = True

                        Dim warningForm As New Phising()
                        warningForm.lbPhishing.Items.Clear()
                        warningForm.lbPhishing.Items.Add("Detected Phishing URL:")
                        warningForm.lbPhishing.Items.Add(url)

                        Dim result As DialogResult = warningForm.ShowDialog()
                        If result = DialogResult.Ignore Then
                            IgnoredUrls.Add(url)
                            Dim core = TryCast(sender, CoreWebView2)
                            If core IsNot Nothing Then core.Navigate(url)
                        End If
                        Return
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub WebView2_NavigationCompleted(ByVal sender As Object, ByVal e As CoreWebView2NavigationCompletedEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing AndAlso e.IsSuccess Then
            Dim currentUri As String = core.Source
            If wb IsNot Nothing AndAlso wb.CoreWebView2 Is core Then
                ToolStripTextBox1.Text = currentUri
            End If
            Try
                If My.Settings.History Is Nothing Then My.Settings.History = New System.Collections.Specialized.StringCollection()
                My.Settings.History.Add(currentUri)
                My.Settings.Save()
                History.ListBox1.Items.Add(currentUri)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error recording history: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub WebView2_SourceChanged(ByVal sender As Object, ByVal e As CoreWebView2SourceChangedEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing AndAlso wb IsNot Nothing AndAlso wb.CoreWebView2 Is core Then
            ToolStripTextBox1.Text = core.Source
        End If
    End Sub

    Private Sub WebView2_DocumentTitleChanged(ByVal sender As Object, ByVal e As Object)
        Dim core = TryCast(sender, CoreWebView2)
        If core Is Nothing Then Return

        For Each page As TabPage In TabControl1.TabPages
            If page.Controls.Count > 0 Then
                Dim browserControl = TryCast(page.Controls(0), WebView2)
                If browserControl IsNot Nothing AndAlso browserControl.CoreWebView2 Is core Then
                    If Not String.IsNullOrEmpty(core.DocumentTitle) Then
                        page.Text = If(core.DocumentTitle.Length > 20, core.DocumentTitle.Substring(0, 17) & "...", core.DocumentTitle)
                    End If
                    If page Is TabControl1.SelectedTab Then
                        Label1.Text = core.DocumentTitle
                    End If
                    Exit For
                End If
            End If
        Next
    End Sub

    Private Sub WebView2_HistoryChanged(ByVal sender As Object, ByVal e As Object)
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing AndAlso wb IsNot Nothing AndAlso wb.CoreWebView2 Is core Then
            Back.Enabled = wb.CanGoBack
            ToolStripButton2.Enabled = wb.CanGoForward
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If Me.TabControl1.SelectedTab IsNot Nothing AndAlso Me.TabControl1.SelectedTab.Controls.Count > 0 Then
            wb = TryCast(Me.TabControl1.SelectedTab.Controls(0), WebView2)
            If wb IsNot Nothing Then
                Back.Enabled = wb.CanGoBack
                ToolStripButton2.Enabled = wb.CanGoForward
                If wb.CoreWebView2 IsNot Nothing Then
                    ToolStripTextBox1.Text = wb.CoreWebView2.Source
                    Label1.Text = wb.CoreWebView2.DocumentTitle
                End If
            End If
        Else
            wb = Nothing
        End If
    End Sub



    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        NavigateActiveTab("https://www.k-browser.com/")
    End Sub

    Private Sub CPUStatsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CPUStatsToolStripMenuItem.Click
        task_manager.ShowDialog()
    End Sub

    Private Sub Form1_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDoubleClick
        If e.Button = MouseButtons.Left Then
            Console.WriteLine("Left Mouse Button was clicked!")
        ElseIf e.Button = MouseButtons.Middle Then
            Console.WriteLine("Middle Mouse Button was clicked!")
        End If
    End Sub

    Private Async Sub mnuLeftToRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLeftToRight.Click
        Try
            If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
                Await wb.CoreWebView2.ExecuteScriptAsync("document.body.dir = 'ltr'")
            End If
            mnuLeftToRight.Checked = True
            If mnuRightToLeft IsNot Nothing Then mnuRightToLeft.Checked = False
        Catch ex As Exception
        End Try
    End Sub

    Private Async Sub mnuRightToLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRightToLeft.Click
        Try
            If wb IsNot Nothing AndAlso wb.CoreWebView2 IsNot Nothing Then
                Await wb.CoreWebView2.ExecuteScriptAsync("document.body.dir = 'rtl'")
            End If
            mnuLeftToRight.Checked = False
            If mnuRightToLeft IsNot Nothing Then mnuRightToLeft.Checked = True
        Catch ex As Exception
        End Try
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
        newform.Show()
    End Sub

    Private Sub TaskToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TaskToolStripMenuItem.Click
        task_manager.ShowDialog()
    End Sub

    Private Sub ToolStripTextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles ToolStripTextBox1.LostFocus
        ToolStripTextBox1.BackColor = Color.Snow
    End Sub

    Private Sub ToolStripTextBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripTextBox1.Click
        ToolStripTextBox1.SelectAll()
    End Sub
    Private Sub ReloadToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReloadToolStripMenuItem1.Click
        If wb IsNot Nothing Then wb.Reload()
    End Sub

    Private Sub AutoToolStripMenuItem_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles AutoToolStripMenuItem.CheckedChanged
        Select Case AutoToolStripMenuItem.Checked
            Case True
                Timer1.Interval = 10000
                Timer1.Enabled = True
            Case Else
                Timer1.Enabled = False
        End Select
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If wb IsNot Nothing Then wb.Reload()
    End Sub

    Private Async Sub NewTabToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem2.Click
        Try
            Await CreateNewTab()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DefualtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefualtToolStripMenuItem.Click
        NavigateActiveTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
    End Sub

    Private Sub HomeToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomeToolStripMenuItem1.Click
        NavigateActiveTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
    End Sub

    Private Sub BlancPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlancPageToolStripMenuItem.Click
        NavigateActiveTab("about:blank")
    End Sub

    Private Sub ToolStripSplitButton1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        ToolStripTextBox1.Focus()
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        NavigateActiveTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            My.Settings.MainLocation = Me.Location
            My.Settings.MainSize = Me.Size
            My.Settings.Save()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub largestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing Then wb.ZoomFactor = 1.5
        largestToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing Then wb.ZoomFactor = 0.7
        smallestToolStripMenuItem.Checked = True
    End Sub

    Private Sub largerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing Then wb.ZoomFactor = 1.25
        largerToolStripMenuItem.Checked = True
    End Sub

    Private Sub mediumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mediumToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing Then wb.ZoomFactor = 1.0
        mediumToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing Then wb.ZoomFactor = 0.85
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
                    Me.Close()
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

    Private Sub KBrowserToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KBrowserToolStripMenuItem.Click
        NavigateActiveTab("https://www.k-browser.com/")
    End Sub

    Private Sub SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SToolStripMenuItem.Click
        Try
            System.Diagnostics.Process.Start(New System.Diagnostics.ProcessStartInfo("mailto:denkon24@yahoo.com") With {.UseShellExecute = True})
        Catch ex As Exception
            MessageBox.Show("Unable to open email client.", "K-Browser", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class
