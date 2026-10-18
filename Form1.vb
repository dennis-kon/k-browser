Imports System.IO
Imports System.Net
Imports Microsoft.Win32
Imports Microsoft.Web.WebView2.WinForms
Imports Microsoft.Web.WebView2.Core

Public Class Form1

    Public WithEvents wb As WebView2
    Dim isUserAgentSet As Boolean
    Dim MyUserAgent As String = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36 Edg/120.0.0.0"
    Dim rightNow As DateTime = DateTime.Now
    Public full As Boolean = False
    Public Event FileDownload As EventHandler

    Private ReadOnly _credentialDetectionService As New CredentialDetectionService()
    Private ReadOnly _passwordManagerService As New PasswordManagerService()

    Public Sub New()
        InitializeComponent()
        isUserAgentSet = False
        AddHandler _credentialDetectionService.CredentialsCaptured, AddressOf OnCredentialsCaptured
        AddHandler _credentialDetectionService.LoginFormDetected, AddressOf OnLoginFormDetected
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

    ''' <summary>Batches frequent My.Settings writes (history) into one save every few seconds.</summary>
    Friend Shared ReadOnly SettingsSaver As New DeferredSave(Sub() My.Settings.Save(), 5000)

    Public Shared Function GetHomePageUrl() As String
        Return AppManager.HomePageOrDefault(My.Settings.HomePageUrl)
    End Function

    Public Async Sub NavigateActiveTab(ByVal url As String)
        If String.IsNullOrWhiteSpace(url) Then Return
        Dim target As String = AppManager.ResolveUrlOrSearch(url, My.Settings.SearchEngine)

        Dim activeBrws As WebView2 = Nothing
        If TabControl1.SelectedTab IsNot Nothing AndAlso TabControl1.SelectedTab.Controls.Count > 0 Then
            activeBrws = TryCast(TabControl1.SelectedTab.Controls(0), WebView2)
        End If

        If activeBrws IsNot Nothing Then
            If activeBrws.CoreWebView2 IsNot Nothing Then
                activeBrws.CoreWebView2.Navigate(target)
            Else
                activeBrws.Source = New Uri(target)
            End If
        Else
            Await CreateNewTab(target)
        End If
    End Sub

    Private Function GetActiveWebView() As WebView2
        If TabControl1.SelectedTab IsNot Nothing AndAlso TabControl1.SelectedTab.Controls.Count > 0 Then
            Return TryCast(TabControl1.SelectedTab.Controls(0), WebView2)
        End If
        Return Nothing
    End Function

    Public Sub OpenDevTools()
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.CoreWebView2.OpenDevToolsWindow()
        End If
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

    ''' <summary>
    ''' Assigns Fluent icons to the toolbar and menus. Colours follow the theme via ThemeManager.ApplyTheme.
    ''' </summary>
    Private Sub ApplyIcons()
        ' Toolbar (20px)
        IconService.SetIcon(Back, FluentIcon.ArrowLeft, 20)
        IconService.SetIcon(ToolStripButton2, FluentIcon.ArrowRight, 20)
        IconService.SetIcon(ToolStripButton3, FluentIcon.ArrowClockwise, 20)
        IconService.SetIcon(ToolStripButton4, FluentIcon.Home, 20)
        IconService.SetIcon(ToolStripButton7, FluentIcon.TabAdd, 20)
        IconService.SetIcon(ToolStripButton6, FluentIcon.Dismiss, 20)
        IconService.SetIcon(ToolStripButton5, FluentIcon.ArrowEnter, 20)
        IconService.SetIcon(ToolStripButton8, FluentIcon.Rss, 20)
        IconService.SetIcon(ToolStripButton9, FluentIcon.Star, 20)
        IconService.SetIcon(tsbAdBlockBadge, FluentIcon.Shield, 20)
        IconService.SetIcon(btnPrivateMode, FluentIcon.Incognito, 20)

        ' Menus (16px)
        IconService.SetIcon(ToolStripMenuItem5, FluentIcon.Open)
        IconService.SetIcon(OpePageInNToolStripMenuItem, FluentIcon.Open)
        IconService.SetIcon(NewTabToolStripMenuItem1, FluentIcon.TabAdd)
        IconService.SetIcon(CloseTabToolStripMenuItem, FluentIcon.Dismiss)
        IconService.SetIcon(NewWindowToolStripMenuItem, FluentIcon.WindowNew)
        IconService.SetIcon(PrivateWindowToolStripMenuItem, FluentIcon.Incognito)
        IconService.SetIcon(DefualtToolStripMenuItem, FluentIcon.Document)
        IconService.SetIcon(SaveFileToolStripMenuItem, FluentIcon.Save)
        IconService.SetIcon(PrintToolStripMenuItem, FluentIcon.Print)
        IconService.SetIcon(ExitToolStripMenuItem, FluentIcon.SignOut)
        IconService.SetIcon(CutToolStripMenuItem, FluentIcon.Cut)
        IconService.SetIcon(CopyToolStripMenuItem, FluentIcon.Copy)
        IconService.SetIcon(ZooomToolStripMenuItem, FluentIcon.ZoomIn)
        IconService.SetIcon(ImageToolStripMenuItem, FluentIcon.Screenshot)
        IconService.SetIcon(HistoryToolStripMenuItem, FluentIcon.History)
        IconService.SetIcon(BackToolStripMenuItem, FluentIcon.ArrowLeft)
        IconService.SetIcon(ForwardToolStripMenuItem, FluentIcon.ArrowRight)
        IconService.SetIcon(ReloadToolStripMenuItem1, FluentIcon.ArrowClockwise)
        IconService.SetIcon(StopToolStripMenuItem1, FluentIcon.Dismiss)
        IconService.SetIcon(ToolStripMenuItem3, FluentIcon.Home)
        IconService.SetIcon(CookieToolStripMenuItem, FluentIcon.Cookies)
        IconService.SetIcon(CookieViewerToolStripMenuItem, FluentIcon.Broom)
        IconService.SetIcon(ViewToolStripMenuItem1, FluentIcon.Bookmark)
        IconService.SetIcon(DarkModeToolStripMenuItem, FluentIcon.WeatherMoon)
        IconService.SetIcon(LinksToolStripMenuItem, FluentIcon.Search)
        IconService.SetIcon(SocialNetworksToolStripMenuItem, FluentIcon.People)
        IconService.SetIcon(SettingsToolStripMenuItem, FluentIcon.Settings)
        IconService.SetIcon(DownloadManagerToolStripMenuItem, FluentIcon.ArrowDownload)
        IconService.SetIcon(FtpClientToolStripMenuItem, FluentIcon.Server)
        IconService.SetIcon(TaskToolStripMenuItem, FluentIcon.DataUsage)
        IconService.SetIcon(SetHomePageToolStripMenuItem, FluentIcon.Rss)
        IconService.SetIcon(MediaToolStripMenuItem, FluentIcon.Video)
        IconService.SetIcon(SendALinkToolStripMenuItem, FluentIcon.Send)
        IconService.SetIcon(InternetSettingsToolStripMenuItem, FluentIcon.GlobeDesktop)
        IconService.SetIcon(ProxySettingsToolStripMenuItem, FluentIcon.Globe)
        IconService.SetIcon(AboutToolStripMenuItem, FluentIcon.Info)
        IconService.SetIcon(CheckForUpdatesToolStripMenuItem, FluentIcon.ArrowSync)

        ' Search-engine and social links: globe until the site's favicon loads (see LoadBrandIconsAsync)
        brandDomains = New Dictionary(Of ToolStripMenuItem, String) From {
            {BingToolStripMenuItem, "bing.com"}, {GoogleToolStripMenuItem1, "google.com"},
            {YahooToolStripMenuItem, "yahoo.com"}, {EBayToolStripMenuItem, "ebay.com"},
            {MSNToolStripMenuItem, "msn.com"}, {DuckDuckGoToolStripMenuItem, "duckduckgo.com"},
            {VimeoToolStripMenuItem, "vimeo.com"}, {AccuWeatherToolStripMenuItem, "accuweather.com"},
            {FacebookToolStripMenuItem, "facebook.com"}, {TwitterToolStripMenuItem, "x.com"},
            {GoogleToolStripMenuItem, "google.com"}, {LinkedinToolStripMenuItem, "linkedin.com"},
            {InstagramToolStripMenuItem, "instagram.com"}, {VkToolStripMenuItem, "vk.com"},
            {PinterestToolStripMenuItem, "pinterest.com"}}
        For Each item In brandDomains.Keys
            IconService.SetIcon(item, FluentIcon.Globe)
        Next
    End Sub

    Private brandDomains As Dictionary(Of ToolStripMenuItem, String)

    ''' <summary>
    ''' Swaps each brand link's globe for the site's real favicon once it has been fetched (or read from cache).
    ''' Items whose favicon can't be fetched keep the globe and are retried the next time the menu opens.
    ''' </summary>
    Private Async Sub LoadBrandIconsAsync(sender As Object, e As EventArgs) Handles LinksToolStripMenuItem.DropDownOpening, SocialNetworksToolStripMenuItem.DropDownOpening
        Dim menu = DirectCast(sender, ToolStripMenuItem)
        Dim pending = menu.DropDownItems.OfType(Of ToolStripMenuItem)().Where(Function(i) brandDomains.ContainsKey(i) AndAlso IconService.IsBound(i)).ToList()
        For Each item In pending
            Dim favicon = Await FaviconService.Default.GetFaviconAsync(brandDomains(item))
            If favicon IsNot Nothing AndAlso Not item.IsDisposed AndAlso IconService.IsBound(item) Then
                IconService.ClearIcon(item)
                item.Image = IconService.ScaleImage(favicon, 16)
            End If
        Next
    End Sub

    Private Async Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            ' Parse the filter lists in the background while the window and browser engine start up.
            Dim rulesLoading = AdBlockEngine.EnsureRulesLoadedAsync()
            ApplyIcons()
            ThemeManager.ApplyTheme(Me)
            UpdateDarkModeMenuCheckedState()
            Dim userScriptsLoading = UserScriptManager.EnsureLoadedAsync()
            If My.Settings.FullScreenOnStartup Then
                FullScreen()
                full = True
            End If
            Dim settingsSvc As New SettingsService()
            Dim appSettings As PrivacySettingsModel = Await settingsSvc.LoadSettingsAsync()
            Dim sessionRestored As Boolean = False

            If appSettings IsNot Nothing AndAlso appSettings.StartupMode = "RestoreSession" Then
                Try
                    Dim sessionSvc As New SessionService()
                    Dim session As SessionModel = Await sessionSvc.LoadSessionAsync()
                    If session IsNot Nothing AndAlso session.Tabs IsNot Nothing AndAlso session.Tabs.Count > 0 Then
                        ' Only the tab that was selected is loaded now; the others load when first opened.
                        _restoringSession = True
                        Try
                            For Each tabItem In session.Tabs
                                If Not String.IsNullOrWhiteSpace(tabItem.Url) Then AddPendingTab(tabItem.Url)
                            Next
                            If TabControl1.TabPages.Count > 0 Then
                                TabControl1.SelectedIndex = Math.Max(0, Math.Min(session.SelectedTab, TabControl1.TabPages.Count - 1))
                            End If
                        Finally
                            _restoringSession = False
                        End Try
                        If TabControl1.SelectedTab IsNot Nothing Then
                            TabControl1_SelectedIndexChanged(TabControl1, EventArgs.Empty)
                        End If
                        sessionRestored = (TabControl1.TabPages.Count > 0)
                    End If
                Catch exSession As Exception
                    System.Diagnostics.Debug.WriteLine("Form1: Error restoring session: " & exSession.Message)
                End Try
            End If

            If Not sessionRestored Then
                Dim initialUrl As String = GetHomePageUrl()
                Select Case My.Settings.StartupBehavior
                    Case 1
                        initialUrl = "about:blank"
                    Case 2
                        If My.Settings.History IsNot Nothing AndAlso My.Settings.History.Count > 0 Then
                            Dim lastEntry As String = My.Settings.History(My.Settings.History.Count - 1)
                            ' History entries are stored as "Title|URL|DateTime" — extract the URL portion
                            Dim parts() As String = lastEntry.Split("|"c)
                            If parts.Length >= 2 Then
                                initialUrl = parts(1)
                            Else
                                initialUrl = lastEntry
                            End If
                        End If
                    Case 3
                        initialUrl = GetHomePageUrl()
                End Select
                Dim brws = Await CreateNewTab(initialUrl)
            End If
            Dim unusedTask = System.Threading.Tasks.Task.Run(Function() AdBlockEngine.CheckAndAutoUpdateListsAsync())
            If tsbAdBlockBadge IsNot Nothing Then
                tsbAdBlockBadge.Visible = My.Settings.ShowBlockedCount AndAlso My.Settings.AdBlockerEnabled
                tsbAdBlockBadge.Text = AdBlockEngine.TotalBlockedCount.ToString()
            End If

            AddHandler SettingsManager.SettingsChanged, AddressOf OnSettingsChanged
            StartMemorySaverTimer()

            If My.Settings.MainSize.Width > 200 AndAlso My.Settings.MainSize.Height > 200 Then
                Me.Size = My.Settings.MainSize
            End If
            If CalendarToolStripMenuItem IsNot Nothing Then
                CalendarToolStripMenuItem.Text = DateTime.Now.ToLongDateString()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Form1_Load error: " & ex.Message)
        End Try
    End Sub

    Private Sub OnSettingsChanged(ByVal sender As Object, ByVal e As EventArgs)
        If Me.InvokeRequired Then
            Me.BeginInvoke(Sub() OnSettingsChanged(sender, e))
            Return
        End If

        If tsbAdBlockBadge IsNot Nothing Then
            tsbAdBlockBadge.Visible = My.Settings.ShowBlockedCount AndAlso My.Settings.AdBlockerEnabled
            tsbAdBlockBadge.Text = AdBlockEngine.TotalBlockedCount.ToString()
        End If

        ' Memory Saver may have been switched on or off: update the background tabs' memory targets.
        TabLifecycleManager.OnTabSelectionChanged(TabControl1, My.Settings.MemorySaverEnabled)

        For Each page As TabPage In TabControl1.TabPages
            If page.Controls.Count > 0 Then
                Dim brws = TryCast(page.Controls(0), WebView2)
                If brws IsNot Nothing Then
                    AdBlockRequestFilter.Apply(brws.CoreWebView2, My.Settings.AdBlockerEnabled)
                    Select Case My.Settings.FontSize
                        Case 0 : brws.ZoomFactor = 0.85
                        Case 2 : brws.ZoomFactor = 1.25
                        Case 3 : brws.ZoomFactor = 1.5
                        Case Else : brws.ZoomFactor = 1.0
                    End Select
                End If
            End If
        Next

        Try
            Dim privacySvc As New PrivacySettingsService()
            Task.Run(Async Function()
                         Dim pSettings = Await privacySvc.LoadPrivacySettingsAsync()
                         Me.BeginInvoke(Sub()
                                            For Each page As TabPage In TabControl1.TabPages
                                                If page.Controls.Count > 0 Then
                                                    Dim brws = TryCast(page.Controls(0), WebView2)
                                                    If brws IsNot Nothing Then
                                                        privacySvc.ApplyThirdPartyCookieBlocking(brws, pSettings.BlockThirdPartyCookies)
                                                    End If
                                                End If
                                            Next
                                        End Sub)
                     End Function)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error applying privacy settings in OnSettingsChanged: " & ex.Message)
        End Try
    End Sub



    Private Sub CloseCurrentTab()
        If TabControl1.SelectedTab IsNot Nothing Then
            Dim selectedTab As TabPage = TabControl1.SelectedTab
            If selectedTab.Controls.Count > 0 Then
                Dim browserControl As WebView2 = TryCast(selectedTab.Controls(0), WebView2)
                If browserControl IsNot Nothing Then
                    If browserControl.CoreWebView2 IsNot Nothing Then
                        RemoveHandler browserControl.CoreWebView2.NavigationStarting, AddressOf WebView2_NavigationStarting
                        RemoveHandler browserControl.CoreWebView2.FrameNavigationStarting, AddressOf WebView2_FrameNavigationStarting
                        RemoveHandler browserControl.CoreWebView2.NavigationCompleted, AddressOf WebView2_NavigationCompleted
                        RemoveHandler browserControl.CoreWebView2.SourceChanged, AddressOf WebView2_SourceChanged
                        RemoveHandler browserControl.CoreWebView2.DocumentTitleChanged, AddressOf WebView2_DocumentTitleChanged
                        RemoveHandler browserControl.CoreWebView2.HistoryChanged, AddressOf WebView2_HistoryChanged
                        RemoveHandler browserControl.CoreWebView2.PermissionRequested, AddressOf WebView2_PermissionRequested
                        RemoveHandler browserControl.CoreWebView2.DownloadStarting, AddressOf WebView2_DownloadStarting
                        RemoveHandler browserControl.CoreWebView2.ProcessFailed, AddressOf WebView2_ProcessFailed
                        RemoveHandler browserControl.CoreWebView2.NewWindowRequested, AddressOf WebView2_NewWindowRequested
                        RemoveHandler browserControl.CoreWebView2.WebResourceRequested, AddressOf WebView2_WebResourceRequested
                    End If
                    TabLifecycleManager.OnTabClosed(browserControl)
                    AdBlockEngine.RemoveTab(browserControl)
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
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Await brws.CoreWebView2.ExecuteScriptAsync("window.print()")
        End If
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.CoreWebView2.ShowPrintUI(CoreWebView2PrintDialogKind.Browser)
        End If
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.CoreWebView2.ShowPrintUI(CoreWebView2PrintDialogKind.Browser)
        End If
    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Dim msg As String
        Dim title As String
        Dim style As MsgBoxStyle
        Dim response As MsgBoxResult
        msg = "Are you sure you want to exit?"
        style = MsgBoxStyle.Information Or MsgBoxStyle.YesNo
        title = "K-Browser"
        response = MsgBox(msg, style, title)
        If response = MsgBoxResult.Yes Then
            Me.Close()
        End If
    End Sub

    Private Async Sub SourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceToolStripMenuItem.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Try
                Dim htmlJson As String = Await brws.CoreWebView2.ExecuteScriptAsync("document.documentElement.outerHTML")
                Dim htmlText As String = htmlJson
                If htmlText.StartsWith("""") AndAlso htmlText.EndsWith("""") Then
                    Try
                        htmlText = System.Text.RegularExpressions.Regex.Unescape(htmlText.Substring(1, htmlText.Length - 2))
                    Catch
                        htmlText = htmlJson.Substring(1, htmlJson.Length - 2).Replace("\""", """").Replace("\n", vbCrLf).Replace("\r", "").Replace("\\", "\")
                    End Try
                End If
                Dim pageUrl As String = brws.CoreWebView2.Source
                Dim pageTitle As String = brws.CoreWebView2.DocumentTitle
                Source.SetSourceData(htmlText, pageUrl, pageTitle)
                Source.Show()
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error getting HTML source: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub ViewToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewToolStripMenuItem1.Click
        Bookmarks.ShowDialog()
    End Sub

    Private Sub BookmarkThisPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BookmarkThisPageToolStripMenuItem.Click, ToolStripButton9.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Dim currentUrl As String = brws.CoreWebView2.Source
            Dim currentTitle As String = If(String.IsNullOrWhiteSpace(brws.CoreWebView2.DocumentTitle), currentUrl, brws.CoreWebView2.DocumentTitle)
            If Not String.IsNullOrWhiteSpace(currentUrl) AndAlso currentUrl <> "about:blank" Then
                If My.Settings.BookmarksTreeData Is Nothing Then
                    My.Settings.BookmarksTreeData = New System.Collections.Specialized.StringCollection()
                End If
                My.Settings.BookmarksTreeData.Add("URL:0:" & currentTitle & ":" & currentUrl)
                My.Settings.Save()
                MessageBox.Show("Page bookmarked successfully!", "Bookmark Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
        End If
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        OpenDevTools()
    End Sub

    Private Sub HistoryToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistoryToolStripMenuItem.Click
        History.ShowDialog()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Back.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing AndAlso brws.CanGoBack Then
            brws.GoBack()
        End If
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing AndAlso brws.CanGoForward Then
            brws.GoForward()
        End If
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.Reload()
        End If
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        NavigateActiveTab(GetHomePageUrl())
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

    ' The Edit menu owns Ctrl+X/C/V/A, so those shortcuts never reach a focused
    ' text box on their own. Returns the focused address/search box, if any.
    Private Function GetFocusedTextBox() As TextBoxBase
        If ToolStripTextBox1.Focused Then Return ToolStripTextBox1.TextBox
        Return TryCast(Me.ActiveControl, TextBoxBase)
    End Function

    Private Async Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        Dim tb = GetFocusedTextBox()
        If tb IsNot Nothing Then tb.Cut() : Return
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then Await brws.CoreWebView2.ExecuteScriptAsync("document.execCommand('cut')")
    End Sub

    Private Async Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        Dim tb = GetFocusedTextBox()
        If tb IsNot Nothing Then tb.Copy() : Return
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then Await brws.CoreWebView2.ExecuteScriptAsync("document.execCommand('copy')")
    End Sub

    Private Async Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        Dim tb = GetFocusedTextBox()
        If tb IsNot Nothing Then tb.Paste() : Return
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then Await brws.CoreWebView2.ExecuteScriptAsync("document.execCommand('paste')")
    End Sub

    Private Async Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        Dim tb = GetFocusedTextBox()
        If tb IsNot Nothing Then tb.SelectAll() : Return
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then Await brws.CoreWebView2.ExecuteScriptAsync("document.execCommand('selectAll')")
    End Sub

    Private Sub SeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SeToolStripMenuItem.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing Then
            brws.Focus()
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
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.ZoomFactor += 0.15
        End If
    End Sub

    Private Sub ZoomOutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ZoomOutToolStripMenuItem.Click
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing AndAlso brws.ZoomFactor > 0.3 Then
            brws.ZoomFactor -= 0.15
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


    Private Sub SetHomePageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SetHomePageToolStripMenuItem.Click
        NavigateActiveTab(GetHomePageUrl())
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
        If e.Button = MouseButtons.Left Then
            ContextMenuStrip1.Show(CType(sender, Control), e.Location)
        End If
    End Sub

    Private Sub CookieToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CookieToolStripMenuItem.Click
        CookieViewer.ShowDialog()
    End Sub

    Private Sub SubmitFeedbackToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
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

    Private Sub GopherToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        NavigateActiveTab("http://wt.gopherite.org/")
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
        tab.Text = "Loading..."
        Dim brws As WebView2 = AddBrowserControl(tab)

        Me.TabControl1.TabPages.Add(tab)
        Me.TabControl1.SelectedTab = tab

        Return Await LoadBrowserIntoTabAsync(brws, targetUrl)
    End Function

    ''' <summary>A restored tab that has not been opened yet: it gets its browser the first time it is selected.</summary>
    Private Class PendingTab
        Public Url As String
    End Class

    Private _restoringSession As Boolean

    ''' <summary>Adds a tab that shows the address's site name and loads nothing until it is selected.</summary>
    Private Sub AddPendingTab(ByVal url As String)
        Dim tab As New TabPage() With {.Text = AppManager.TabTitleForUrl(url), .Tag = New PendingTab With {.Url = url}, .ToolTipText = url}
        Me.TabControl1.TabPages.Add(tab)
    End Sub

    ''' <summary>Gives a restored tab its browser and loads its page, the first time it is selected.</summary>
    Private Async Sub EnsureTabLoaded(ByVal tab As TabPage)
        Dim pending = TryCast(tab?.Tag, PendingTab)
        If pending Is Nothing OrElse tab.Controls.Count > 0 Then Return
        tab.Tag = Nothing
        Try
            Await LoadBrowserIntoTabAsync(AddBrowserControl(tab), pending.Url)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Form1: Error loading restored tab (" & pending.Url & "): " & ex.Message)
        End Try
    End Sub

    Private Function AddBrowserControl(ByVal tab As TabPage) As WebView2
        Dim brws As New WebView2()
        brws.Name = "WebView2"
        brws.Dock = DockStyle.Fill
        tab.Controls.Add(brws)
        If tab Is TabControl1.SelectedTab Then wb = brws
        Return brws
    End Function

    Private Async Function LoadBrowserIntoTabAsync(ByVal brws As WebView2, ByVal targetUrl As String) As System.Threading.Tasks.Task(Of WebView2)
        Try
            Dim env = Await TabProcessManager.GetSharedEnvironmentAsync()
            Await brws.EnsureCoreWebView2Async(env)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("EnsureCoreWebView2Async failed: " & ex.Message)
        End Try

        If brws.CoreWebView2 IsNot Nothing Then
            ThemeManager.ApplyWebView2Theme(brws)
            Await UserScriptManager.RegisterScriptsForTabAsync(brws)

            Try
                Dim privacySvc As New PrivacySettingsService()
                Dim pSettings = Await privacySvc.LoadPrivacySettingsAsync()
                privacySvc.ApplyThirdPartyCookieBlocking(brws, pSettings.BlockThirdPartyCookies)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error applying privacy settings to new tab: " & ex.Message)
            End Try

            brws.CoreWebView2.Settings.IsStatusBarEnabled = True
            brws.CoreWebView2.Settings.AreDefaultScriptDialogsEnabled = True
            brws.CoreWebView2.Settings.IsScriptEnabled = True
            brws.CoreWebView2.Settings.IsWebMessageEnabled = True
            brws.CoreWebView2.Settings.AreDevToolsEnabled = True

            Select Case My.Settings.FontSize
                Case 0 : brws.ZoomFactor = 0.85
                Case 2 : brws.ZoomFactor = 1.25
                Case 3 : brws.ZoomFactor = 1.5
                Case Else : brws.ZoomFactor = 1.0
            End Select

            AddHandler brws.CoreWebView2.NavigationStarting, AddressOf WebView2_NavigationStarting
            AddHandler brws.CoreWebView2.FrameNavigationStarting, AddressOf WebView2_FrameNavigationStarting
            AddHandler brws.CoreWebView2.ContentLoading, AddressOf WebView2_ContentLoading
            AddHandler brws.CoreWebView2.DOMContentLoaded, AddressOf WebView2_DOMContentLoaded
            AddHandler brws.CoreWebView2.NavigationCompleted, AddressOf WebView2_NavigationCompleted
            AddHandler brws.CoreWebView2.SourceChanged, AddressOf WebView2_SourceChanged
            AddHandler brws.CoreWebView2.DocumentTitleChanged, AddressOf WebView2_DocumentTitleChanged
            AddHandler brws.CoreWebView2.HistoryChanged, AddressOf WebView2_HistoryChanged
            AddHandler brws.CoreWebView2.PermissionRequested, AddressOf WebView2_PermissionRequested
            AddHandler brws.CoreWebView2.DownloadStarting, AddressOf WebView2_DownloadStarting
            AddHandler brws.CoreWebView2.ProcessFailed, AddressOf WebView2_ProcessFailed
            AddHandler brws.CoreWebView2.NewWindowRequested, AddressOf WebView2_NewWindowRequested
            AddHandler brws.CoreWebView2.WebMessageReceived, AddressOf WebView2_WebMessageReceived

            AddHandler brws.CoreWebView2.WebResourceRequested, AddressOf WebView2_WebResourceRequested
            AdBlockRequestFilter.Apply(brws.CoreWebView2, My.Settings.AdBlockerEnabled)
        End If

        Dim effectiveUrl As String = targetUrl
        If String.IsNullOrEmpty(effectiveUrl) Then
            effectiveUrl = AppManager.NewTabUrl(My.Settings.NewTabPage, My.Settings.HomePageUrl)
        End If

        Dim targetFix As String = AppManager.ResolveUrlOrSearch(effectiveUrl, My.Settings.SearchEngine)

        ' Without this the first page after startup would load before the filter lists are parsed.
        If My.Settings.AdBlockerEnabled Then
            Try
                Await AdBlockEngine.EnsureRulesLoadedAsync()
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Ad-block rules failed to load: " & ex.Message)
            End Try
        End If

        If brws.CoreWebView2 IsNot Nothing Then
            brws.CoreWebView2.Navigate(targetFix)
        Else
            brws.Source = New Uri(targetFix)
        End If

        Return brws
    End Function

    Private Sub WebView2_DownloadStarting(ByVal sender As Object, ByVal e As CoreWebView2DownloadStartingEventArgs)
        Try
            If Not String.IsNullOrWhiteSpace(My.Settings.DownloadsFolder) AndAlso System.IO.Directory.Exists(My.Settings.DownloadsFolder) Then
                e.ResultFilePath = System.IO.Path.Combine(My.Settings.DownloadsFolder, System.IO.Path.GetFileName(e.ResultFilePath))
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub WebView2_PermissionRequested(ByVal sender As Object, ByVal e As CoreWebView2PermissionRequestedEventArgs)
        Select Case e.PermissionKind
            Case CoreWebView2PermissionKind.Camera
                If Not My.Settings.PermissionCamera Then e.State = CoreWebView2PermissionState.Deny
            Case CoreWebView2PermissionKind.Microphone
                If Not My.Settings.PermissionMic Then e.State = CoreWebView2PermissionState.Deny
            Case CoreWebView2PermissionKind.Geolocation
                If Not My.Settings.PermissionLocation Then e.State = CoreWebView2PermissionState.Deny
            Case CoreWebView2PermissionKind.Notifications
                If Not My.Settings.PermissionNotifications Then e.State = CoreWebView2PermissionState.Deny
        End Select
    End Sub

    Private Sub WebView2_ProcessFailed(ByVal sender As Object, ByVal e As CoreWebView2ProcessFailedEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If core Is Nothing Then Return

        System.Diagnostics.Debug.WriteLine("WebView2 ProcessFailed: Kind=" & e.ProcessFailedKind.ToString() & ", Reason=" & e.Reason.ToString())

        ' Handle render process crashes/freezes gracefully without crashing the main application
        If e.ProcessFailedKind = CoreWebView2ProcessFailedKind.RenderProcessExited OrElse
           e.ProcessFailedKind = CoreWebView2ProcessFailedKind.RenderProcessUnresponsive OrElse
           e.ProcessFailedKind = CoreWebView2ProcessFailedKind.FrameRenderProcessExited Then

            For Each page As TabPage In TabControl1.TabPages
                If page.Controls.Count > 0 Then
                    Dim browserControl = TryCast(page.Controls(0), WebView2)
                    If browserControl IsNot Nothing AndAlso browserControl.CoreWebView2 Is core Then
                        page.Text = "Tab Crashed"

                        If e.ProcessFailedKind = CoreWebView2ProcessFailedKind.RenderProcessExited Then
                            Dim result = MessageBox.Show("A web page process on '" & page.Text & "' has crashed unexpectedly." & vbCrLf & vbCrLf & "Would you like to reload this tab?", "Tab Crashed - K-Browser", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                            If result = DialogResult.Yes Then
                                Try
                                    browserControl.CoreWebView2.Reload()
                                Catch
                                End Try
                            End If
                        ElseIf e.ProcessFailedKind = CoreWebView2ProcessFailedKind.RenderProcessUnresponsive Then
                            Dim result = MessageBox.Show("The page on '" & page.Text & "' has become unresponsive." & vbCrLf & vbCrLf & "Would you like to force reload it?", "Page Unresponsive - K-Browser", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                            If result = DialogResult.Yes Then
                                Try
                                    browserControl.CoreWebView2.Reload()
                                Catch
                                End Try
                            End If
                        End If
                        Exit For
                    End If
                End If
            Next
        End If
    End Sub

    Private Async Sub WebView2_NewWindowRequested(ByVal sender As Object, ByVal e As CoreWebView2NewWindowRequestedEventArgs)
        e.Handled = True
        ' WebView2 has no popup blocker of its own: without this check a page can open
        ' tabs (and steal focus) from a script with no click behind it. On unless switched off in Settings.
        If Not e.IsUserInitiated AndAlso My.Settings.PopUpBlockerEnabled Then
            Label1.Text = "Popup blocked: " & e.Uri
            Return
        End If
        If Not String.IsNullOrWhiteSpace(e.Uri) Then
            Await CreateNewTab(e.Uri)
        End If
    End Sub

    Private Function GetWebView2FromCore(ByVal core As CoreWebView2) As WebView2
        If core Is Nothing Then Return Nothing
        For Each page As TabPage In TabControl1.TabPages
            If page.Controls.Count > 0 Then
                Dim brws = TryCast(page.Controls(0), WebView2)
                If brws IsNot Nothing AndAlso brws.CoreWebView2 Is core Then
                    Return brws
                End If
            End If
        Next
        Return Nothing
    End Function

    Private Sub WebView2_WebResourceRequested(ByVal sender As Object, ByVal e As CoreWebView2WebResourceRequestedEventArgs)
        If Not My.Settings.AdBlockerEnabled Then Return

        ' NEVER block top-level document navigations (main page itself)
        If e.ResourceContext = CoreWebView2WebResourceContext.Document Then
            Return
        End If

        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing Then
            If AdBlockEngine.IsSiteDisabled(core.Source) Then
                Return
            End If
        End If

        ' Protect stylesheets & fonts from substring path blocking so pages never lose CSS or typography
        If e.ResourceContext = CoreWebView2WebResourceContext.Stylesheet OrElse e.ResourceContext = CoreWebView2WebResourceContext.Font Then
            If Not AdBlockEngine.ShouldBlockDomainOnly(e.Request.Uri) Then
                Return
            End If
        End If

        If AdBlockEngine.ShouldBlock(e.Request.Uri) Then
            If core IsNot Nothing Then
                Try
                    e.Response = core.Environment.CreateWebResourceResponse(Nothing, 403, "Blocked by AdBlocker", "Content-Type: text/plain")
                Catch
                End Try
            End If

            Dim brws = GetWebView2FromCore(core)
            If brws IsNot Nothing Then
                AdBlockEngine.RecordBlockedItem(brws, e.Request.Uri)
            Else
                AdBlockEngine.TotalBlockedCount += 1
            End If
            IncrementBlockedAdCount()
        End If
    End Sub

    Private _badgeTimer As System.Windows.Forms.Timer

    ''' <summary>
    ''' A page can block hundreds of requests while loading; the badge text (which re-lays out the toolbar)
    ''' is refreshed at most four times a second instead of once per blocked request.
    ''' </summary>
    Private Sub IncrementBlockedAdCount()
        If Me.InvokeRequired Then
            Me.BeginInvoke(Sub() IncrementBlockedAdCount())
            Return
        End If
        If _badgeTimer Is Nothing Then
            _badgeTimer = New System.Windows.Forms.Timer() With {.Interval = 250}
            AddHandler _badgeTimer.Tick, Sub()
                                             _badgeTimer.Stop()
                                             UpdateBlockedAdBadge()
                                         End Sub
        End If
        If Not _badgeTimer.Enabled Then _badgeTimer.Start()
    End Sub

    Private Sub UpdateBlockedAdBadge()
        If tsbAdBlockBadge IsNot Nothing Then
            tsbAdBlockBadge.Text = AdBlockEngine.TotalBlockedCount.ToString()
            tsbAdBlockBadge.Visible = My.Settings.ShowBlockedCount AndAlso My.Settings.AdBlockerEnabled
        End If
    End Sub

    Private Sub tsbAdBlockBadge_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsbAdBlockBadge.Click
        Dim activeBrws = GetActiveWebView()
        Dim currentUrl As String = ""
        If activeBrws IsNot Nothing AndAlso activeBrws.CoreWebView2 IsNot Nothing Then
            currentUrl = activeBrws.CoreWebView2.Source
        End If

        Using popup As New AdBlockPopup(activeBrws, currentUrl)
            popup.ShowDialog(Me)
        End Using

        If tsbAdBlockBadge IsNot Nothing Then
            tsbAdBlockBadge.Visible = My.Settings.ShowBlockedCount AndAlso My.Settings.AdBlockerEnabled
            tsbAdBlockBadge.Text = AdBlockEngine.TotalBlockedCount.ToString()
        End If
    End Sub

    Private Function IsActiveTabWebView(ByVal core As CoreWebView2) As Boolean
        If core Is Nothing Then Return False
        Dim activeBrws = GetActiveWebView()
        Return activeBrws IsNot Nothing AndAlso activeBrws.CoreWebView2 Is core
    End Function

    Private Async Sub ResetProgressBar()
        Try
            Await System.Threading.Tasks.Task.Delay(1000)
            ProgressBar1.Value = 0
        Catch
        End Try
    End Sub

    Private Sub ProgressBar1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ProgressBar1.Click
        Dim activeBrws = GetActiveWebView()
        If activeBrws IsNot Nothing AndAlso activeBrws.CoreWebView2 IsNot Nothing Then
            activeBrws.Stop()
            ProgressBar1.Value = 0
            Label1.Text = "Loading stopped"
        End If
    End Sub

    Private Sub WebView2_NavigationStarting(ByVal sender As Object, ByVal e As CoreWebView2NavigationStartingEventArgs)
        Dim coreSender = TryCast(sender, CoreWebView2)
        If coreSender IsNot Nothing Then
            Dim navBrws = GetWebView2FromCore(coreSender)
            If navBrws IsNot Nothing Then
                AdBlockEngine.ClearTabBlockedItems(navBrws)
            End If
        End If

        If IsActiveTabWebView(coreSender) Then
            ProgressBar1.Visible = True
            ProgressBar1.Value = 25
            Label1.Text = "Connecting: " & e.Uri
        End If

        Dim url As String = e.Uri
        If String.IsNullOrEmpty(url) OrElse url = "about:blank" Then Return

        ' 1. HTTPS-Only Mode Check
        If My.Settings.HttpsOnlyMode AndAlso url.StartsWith("http://", StringComparison.OrdinalIgnoreCase) Then
            e.Cancel = True
            Dim httpsUrl As String = "https://" & url.Substring(7)
            Dim core = TryCast(sender, CoreWebView2)
            If core IsNot Nothing Then core.Navigate(httpsUrl)
            Return
        End If

        ' 2. Ad Blocker Check
        If My.Settings.AdBlockerEnabled Then
            Dim adDomains As String() = {"doubleclick.net", "adservice.google.com", "adnxs.com", "googlesyndication.com", "taboola.com", "outbrain.com"}
            If SiteFilter.MatchesAny(url, adDomains) Then
                e.Cancel = True
                Return
            End If
        End If

        ' 3. Per-Site JavaScript Check
        If My.Settings.JsDisabledSites IsNot Nothing AndAlso My.Settings.JsDisabledSites.Count > 0 Then
            Dim core = TryCast(sender, CoreWebView2)
            If core IsNot Nothing Then
                Dim isJsDisabled As Boolean = False
                If Uri.IsWellFormedUriString(url, UriKind.Absolute) Then
                    For Each disabledDomain As String In My.Settings.JsDisabledSites
                        If Not String.IsNullOrEmpty(disabledDomain) AndAlso SiteFilter.UrlMatchesEntry(url, disabledDomain) Then
                            isJsDisabled = True
                            Exit For
                        End If
                    Next
                End If
                core.Settings.IsScriptEnabled = Not isJsDisabled
            End If
        End If

        ' 4. Blocked Sites Check
        If My.Settings.BlockedSites IsNot Nothing Then
            For Each blockedUrl As String In My.Settings.BlockedSites
                If Not String.IsNullOrEmpty(blockedUrl) Then
                    If SiteFilter.UrlMatchesEntry(url, blockedUrl) Then
                        e.Cancel = True
                        MsgBox("This site is blocked by K-Browser Settings.", MsgBoxStyle.Critical, "Access Blocked")
                        Return
                    End If
                End If
            Next
        End If

        ' 5. Phishing Sites Check
        If My.Settings.UsePhishingFilter AndAlso My.Settings.PhishingSites IsNot Nothing Then
            For Each phishingUrl As String In My.Settings.PhishingSites
                If Not String.IsNullOrEmpty(phishingUrl) Then
                    If SiteFilter.UrlMatchesEntry(url, phishingUrl) Then
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

    ''' <summary>
    ''' NavigationStarting only sees the main frame. Apply the same HTTPS-only, blocked-site and phishing rules
    ''' to iframes (silently: a dialog per embedded frame would be unusable).
    ''' </summary>
    Private Sub WebView2_FrameNavigationStarting(ByVal sender As Object, ByVal e As CoreWebView2NavigationStartingEventArgs)
        Dim phishingList As IEnumerable(Of String) = Nothing
        If My.Settings.UsePhishingFilter AndAlso My.Settings.PhishingSites IsNot Nothing Then
            phishingList = My.Settings.PhishingSites.Cast(Of String)()
        End If
        Dim blockedList As IEnumerable(Of String) = Nothing
        If My.Settings.BlockedSites IsNot Nothing Then blockedList = My.Settings.BlockedSites.Cast(Of String)()

        If SiteFilter.IsFrameBlocked(e.Uri, My.Settings.HttpsOnlyMode, blockedList, phishingList) Then e.Cancel = True
    End Sub

    Private Sub WebView2_ContentLoading(ByVal sender As Object, ByVal e As CoreWebView2ContentLoadingEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If IsActiveTabWebView(core) Then
            ProgressBar1.Value = 60
            Label1.Text = "Loading content..."
        End If
    End Sub

    Private Async Sub WebView2_DOMContentLoaded(ByVal sender As Object, ByVal e As CoreWebView2DOMContentLoadedEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If IsActiveTabWebView(core) Then
            ProgressBar1.Value = 85
            Label1.Text = "Rendering..."
        End If

        If core IsNot Nothing Then
            Dim activeBrws = GetWebView2FromCore(core)
            If activeBrws IsNot Nothing Then
                Await _credentialDetectionService.ScanPageAsync(activeBrws)
            End If
        End If
    End Sub

    Private Sub WebView2_NavigationCompleted(ByVal sender As Object, ByVal e As CoreWebView2NavigationCompletedEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If IsActiveTabWebView(core) Then
            ProgressBar1.Value = 100
            Label1.Text = If(e.IsSuccess, "Done", "Failed")
            ResetProgressBar()
        End If

        If core IsNot Nothing AndAlso e.IsSuccess Then
            Dim currentUri As String = core.Source
            Dim activeBrws = GetActiveWebView()
            If activeBrws IsNot Nothing AndAlso activeBrws.CoreWebView2 Is core Then
                ToolStripTextBox1.Text = currentUri
            End If
            ' History is now recorded in DocumentTitleChanged where the page title is available
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

        ' Record history as "Title|URL|DateTime" so the History form can display the page title. Titles change
        ' often (unread counters, players), so the settings file is written at most once per few seconds.
        Try
            If My.Settings.History Is Nothing Then My.Settings.History = New System.Collections.Specialized.StringCollection()
            If HistoryRecorder.Record(My.Settings.History, core.DocumentTitle, core.Source, DateTime.Now) Then
                SettingsSaver.Request()
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error recording history: " & ex.Message)
        End Try
    End Sub

    Private Sub WebView2_HistoryChanged(ByVal sender As Object, ByVal e As Object)
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing AndAlso wb IsNot Nothing AndAlso wb.CoreWebView2 Is core Then
            Back.Enabled = wb.CanGoBack
            ToolStripButton2.Enabled = wb.CanGoForward
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If _restoringSession Then Return
        EnsureTabLoaded(Me.TabControl1.SelectedTab)

        If Me.TabControl1.SelectedTab IsNot Nothing AndAlso Me.TabControl1.SelectedTab.Controls.Count > 0 Then
            wb = TryCast(Me.TabControl1.SelectedTab.Controls(0), WebView2)
            If wb IsNot Nothing Then
                Back.Enabled = wb.CanGoBack
                ToolStripButton2.Enabled = wb.CanGoForward
                If wb.CoreWebView2 IsNot Nothing Then
                    ToolStripTextBox1.Text = wb.CoreWebView2.Source
                    Label1.Text = wb.CoreWebView2.DocumentTitle
                    Try
                        wb.CoreWebView2.Resume()
                    Catch ex As Exception
                    End Try
                End If
            End If
        Else
            wb = Nothing
        End If

        ' Memory Saver: resume the active tab and note when the others went to the background.
        TabLifecycleManager.OnTabSelectionChanged(Me.TabControl1, My.Settings.MemorySaverEnabled)
    End Sub

    Private _memorySaverTimer As System.Windows.Forms.Timer

    ''' <summary>Checks once a minute for background tabs that Memory Saver should put to sleep.</summary>
    Private Sub StartMemorySaverTimer()
        If _memorySaverTimer IsNot Nothing Then Return
        _memorySaverTimer = New System.Windows.Forms.Timer() With {.Interval = 60000}
        AddHandler _memorySaverTimer.Tick, Sub() TabLifecycleManager.SuspendIdleTabs(TabControl1, My.Settings.MemorySaverEnabled)
        _memorySaverTimer.Start()
    End Sub



    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        NavigateActiveTab("https://k-browser.com/")
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
            Dim brws = GetActiveWebView()
            If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
                Await brws.CoreWebView2.ExecuteScriptAsync("document.body.dir = 'ltr'")
            End If
            mnuLeftToRight.Checked = True
            If mnuRightToLeft IsNot Nothing Then mnuRightToLeft.Checked = False
        Catch ex As Exception
        End Try
    End Sub

    Private Async Sub mnuRightToLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuRightToLeft.Click
        Try
            Dim brws = GetActiveWebView()
            If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
                Await brws.CoreWebView2.ExecuteScriptAsync("document.body.dir = 'rtl'")
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
        ' The address bar text becomes another program's command line: only a plain http(s) address
        ' may go through (never something like "--flag"), see ExternalBrowser.
        Dim address As String = ExternalBrowser.SafeUrlArgument(ToolStripTextBox1.Text)
        If address Is Nothing Then
            MessageBox.Show("Only http:// and https:// addresses can be opened in another browser.", "Open Page", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        On Error Resume Next
        Dim psi As New System.Diagnostics.ProcessStartInfo("iexplore")
        psi.UseShellExecute = True
        psi.Arguments = address
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
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.Reload()
        End If
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
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            brws.Reload()
        End If
    End Sub

    Private Async Sub NewTabToolStripMenuItem2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem2.Click
        Try
            Await CreateNewTab()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub DefualtToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DefualtToolStripMenuItem.Click
        NavigateActiveTab(GetHomePageUrl())
    End Sub

    Private Sub HomeToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HomeToolStripMenuItem1.Click
        NavigateActiveTab(GetHomePageUrl())
    End Sub

    Private Sub BlancPageToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlancPageToolStripMenuItem.Click
        NavigateActiveTab("about:blank")
    End Sub

    Private Sub ToolStripSplitButton1_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs)
        ToolStripTextBox1.Focus()
    End Sub

    Private Sub ToolStripMenuItem3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem3.Click
        NavigateActiveTab(GetHomePageUrl())
    End Sub

    ''' <summary>
    ''' Lets a closed window be freed: the settings event is shared (it would keep this window, and every
    ''' browser in it, alive), and the tab trackers hold on to each tab's browser.
    ''' </summary>
    Private Sub Form1_FormClosed(sender As Object, e As FormClosedEventArgs) Handles Me.FormClosed
        RemoveHandler SettingsManager.SettingsChanged, AddressOf OnSettingsChanged
        _memorySaverTimer?.Dispose()
        _memorySaverTimer = Nothing
        _badgeTimer?.Dispose()
        _badgeTimer = Nothing
        For Each page As TabPage In TabControl1.TabPages
            Dim brws = If(page.Controls.Count > 0, TryCast(page.Controls(0), WebView2), Nothing)
            If brws IsNot Nothing Then
                TabLifecycleManager.OnTabClosed(brws)
                AdBlockEngine.RemoveTab(brws)
            End If
        Next
    End Sub

    Private Sub Form1_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        Try
            My.Settings.MainLocation = Me.Location
            My.Settings.MainSize = Me.Size
            SettingsSaver.Request()
            SettingsSaver.Flush()

            ' Save active session to Session.json if StartupMode = RestoreSession
            SaveCurrentSessionIfEnabled()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Form1: Error on FormClosing: " & ex.Message)
        End Try
    End Sub

    Private Sub SaveCurrentSessionIfEnabled()
        Try
            Dim settingsSvc As New SettingsService()
            Dim settingsModel As PrivacySettingsModel = settingsSvc.LoadSettings()
            If settingsModel IsNot Nothing AndAlso settingsModel.StartupMode = "RestoreSession" Then
                Dim session As New SessionModel()
                If TabControl1 IsNot Nothing AndAlso TabControl1.TabPages.Count > 0 Then
                    For i As Integer = 0 To TabControl1.TabPages.Count - 1
                        Dim tp As TabPage = TabControl1.TabPages(i)
                        Dim pending = TryCast(tp.Tag, PendingTab)
                        If pending IsNot Nothing Then
                            ' Restored tab that was never opened: keep its address for next time.
                            session.Tabs.Add(New TabItemModel With {.Url = pending.Url, .Pinned = False})
                        ElseIf tp.Controls.Count > 0 AndAlso TypeOf tp.Controls(0) Is WebView2 Then
                            Dim wv As WebView2 = DirectCast(tp.Controls(0), WebView2)
                            Dim urlStr As String = String.Empty
                            If wv.Source IsNot Nothing Then
                                urlStr = wv.Source.ToString()
                            ElseIf wv.CoreWebView2 IsNot Nothing Then
                                urlStr = wv.CoreWebView2.Source
                            End If

                            If Not String.IsNullOrWhiteSpace(urlStr) Then
                                session.Tabs.Add(New TabItemModel With {.Url = urlStr, .Pinned = False})
                            End If
                        End If
                    Next
                    session.SelectedTab = Math.Max(0, Math.Min(TabControl1.SelectedIndex, session.Tabs.Count - 1))
                End If
                Dim sessionSvc As New SessionService()
                sessionSvc.SaveSession(session)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Form1: Error saving session on exit: " & ex.Message)
        End Try
    End Sub

    Private Sub largestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then brws.ZoomFactor = 1.5
        largestToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then brws.ZoomFactor = 0.7
        smallestToolStripMenuItem.Checked = True
    End Sub

    Private Sub largerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then brws.ZoomFactor = 1.25
        largerToolStripMenuItem.Checked = True
    End Sub

    Private Sub mediumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mediumToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then brws.ZoomFactor = 1.0
        mediumToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        Dim brws = GetActiveWebView()
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then brws.ZoomFactor = 0.85
        smallerToolStripMenuItem.Checked = True
    End Sub

    Dim mg_enable As Boolean '// Is gesturing?
    Dim mg_x, mg_y, mg_dist As Integer '// original x, y location of mouse, gesture distance
    Dim mg_direction As String '// Result of gestures

    Private Sub Form1_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles Me.MouseDown
        If e.Button = MouseButtons.Right Then '// If it's right.
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
        If e.Button = MouseButtons.Right Then '// If it's right.
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

    Private Sub SettingsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SettingsToolStripMenuItem.Click
        Settings.ShowDialog()
    End Sub

    Private Sub wb_ResizeEnd(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.ResizeEnd
        Try
            My.Settings.MainSize = Me.Size
            My.Settings.Save()
        Catch ex As Exception

        End Try
    End Sub



    Private Sub KBrowserToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KBrowserToolStripMenuItem.Click
        NavigateActiveTab("https://k-browser.com/")
    End Sub

    Private Sub DarkModeToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DarkModeToolStripMenuItem.Click
        ThemeManager.IsDarkMode = Not ThemeManager.IsDarkMode
        UpdateDarkModeMenuCheckedState()

        ' Apply theme to Form1 and all open tabs
        ThemeManager.ApplyTheme(Me)
        For Each page As TabPage In TabControl1.TabPages
            If page.Controls.Count > 0 Then
                Dim brws = TryCast(page.Controls(0), WebView2)
                If brws IsNot Nothing Then
                    ' Pages see the new colour scheme straight away (prefers-color-scheme), so no reload is needed.
                    ThemeManager.ApplyWebView2Theme(brws)
                End If
            End If
        Next
    End Sub

    Private Sub UpdateDarkModeMenuCheckedState()
        If DarkModeToolStripMenuItem IsNot Nothing Then
            DarkModeToolStripMenuItem.Checked = ThemeManager.IsDarkMode
        End If
    End Sub

    ''' <summary>
    ''' Opens a new Private Browsing window via the toolbar button.
    ''' Delegates all private session logic to PrivateSessionService.
    ''' </summary>
    Private Sub btnPrivateMode_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnPrivateMode.Click
        OpenPrivateWindow()
    End Sub

    ''' <summary>
    ''' Opens a new Private Browsing window via the File menu item (Ctrl+Shift+N).
    ''' </summary>
    Private Sub PrivateWindowToolStripMenuItem_Click(ByVal sender As Object, ByVal e As EventArgs) Handles PrivateWindowToolStripMenuItem.Click
        OpenPrivateWindow()
    End Sub

    ''' <summary>
    ''' Creates a new PrivateSessionService and opens a PrivateBrowserForm.
    ''' Each call creates a completely isolated private session with its own
    ''' WebView2 environment and temporary user data folder.
    ''' </summary>
    Private Sub OpenPrivateWindow()
        Try
            Dim sessionService As New PrivateSessionService()
            Dim privateForm As New PrivateBrowserForm(sessionService)
            privateForm.Show()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Form1: Error opening private window: " & ex.Message)
            MessageBox.Show("Failed to open private browsing window." & vbCrLf & "Please try again.",
                           "Private Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    ' ─────────────────────────────────────────────────
    ' Credential Detection & Password Manager Handlers
    ' ─────────────────────────────────────────────────

    Private Sub WebView2_WebMessageReceived(ByVal sender As Object, ByVal e As CoreWebView2WebMessageReceivedEventArgs)
        _credentialDetectionService.HandleWebMessage(sender, e)
    End Sub

    Private Async Sub OnCredentialsCaptured(ByVal sender As Object, ByVal e As CredentialCapturedEventArgs)
        Dim domain As String = PasswordManagerService.ExtractDomain(e.Url)
        ' A dismissed prompt mutes this site for the session, so a page cannot nag the user into a corner.
        Dim muteSite As Boolean = False
        Try
            Using dlg As New SavePasswordForm(domain, e.Username)
                dlg.ShowDialog(Me)
                Select Case dlg.UserChoice
                    Case SavePasswordChoice.Save
                        Await _passwordManagerService.SaveCredentialAsync(e.Url, e.Username, e.Password)
                        MessageBox.Show("Password saved successfully.", "Password Saved", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Case SavePasswordChoice.Never
                        _passwordManagerService.AddBlockedSite(e.Url)
                    Case SavePasswordChoice.NotNow
                        muteSite = True
                End Select
            End Using
        Finally
            _credentialDetectionService.ReleaseCapturePrompt(e.Url, muteSite)
        End Try
    End Sub

    Private Async Sub OnLoginFormDetected(ByVal sender As Object, ByVal e As LoginFormDetectedEventArgs)
        If e.SavedCredentials Is Nothing OrElse e.SavedCredentials.Count = 0 Then Return

        Dim selectedCred As CredentialEntry = Nothing
        If e.SavedCredentials.Count = 1 Then
            selectedCred = e.SavedCredentials(0)
            Dim result = MessageBox.Show(
                "Saved login detected for " & selectedCred.Website & vbCrLf & vbCrLf &
                "Username: " & selectedCred.Username & vbCrLf & vbCrLf &
                "Use this account?",
                "Saved Login Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result <> DialogResult.Yes Then Return
        Else
            ' Multiple accounts
            Dim accountList As String = String.Join(vbCrLf, e.SavedCredentials.Select(Function(c, idx) (idx + 1).ToString() & ". " & c.Username))
            Dim result = MessageBox.Show(
                "Multiple saved accounts detected for " & e.SavedCredentials(0).Website & ":" & vbCrLf & vbCrLf &
                accountList & vbCrLf & vbCrLf &
                "Autofill with " & e.SavedCredentials(0).Username & "?",
                "Saved Logins Detected", MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = DialogResult.Yes Then
                selectedCred = e.SavedCredentials(0)
            Else
                Return
            End If
        End If

        If selectedCred IsNot Nothing Then
            ' Pass the URL the credentials were offered for so the fill is refused if the tab has
            ' navigated elsewhere while the confirmation dialog was open.
            Await _credentialDetectionService.InjectAutofillAsync(e.WebView, e.Url, selectedCred.Username, selectedCred.Password)
            Await _passwordManagerService.UpdateLastUsedAsync(selectedCred.Website, selectedCred.Username)
        End If
    End Sub

End Class

