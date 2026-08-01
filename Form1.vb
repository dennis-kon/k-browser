Imports System.IO
Imports System.Net
Imports Microsoft.Win32

Public Class Form1

    Private faviconCache As New Dictionary(Of String, Image)()
    Private Const MAX_FAVICON_CACHE As Integer = 100
    Public WithEvents wb As WebBrowser
    Dim isUserAgentSet As Boolean
    Dim MyUserAgent As String = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.1; Trident/6.0; K-Browser 4.8.1)"
    Dim rightNow As DateTime = DateTime.Now
    Public full As Boolean = False
    Public Event FileDownload As EventHandler
    Public WithEvents oDoc As HtmlDocument
    Private Declare Function SetWindowPos Lib "user32.dll" Alias "SetWindowPos" (ByVal hWnd As IntPtr, ByVal hWndIntertAfter As IntPtr, ByVal X As Integer, ByVal Y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal uFlags As Integer) As Boolean
    Private Declare Function GetSystemMetrics Lib "user32.dll" Alias "GetSystemMetrics" (ByVal Which As Integer) As Integer

    Public Sub New()
        SetBrowserFeatureControl()
        InitializeComponent()
        wb = New WebBrowser()
        wb.ScriptErrorsSuppressed = True
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

    Public Sub NavigateActiveTab(ByVal url As String)
        If String.IsNullOrWhiteSpace(url) Then Return
        Dim target As String = AppManager.FixURL(url)
        If wb IsNot Nothing Then
            wb.Navigate(target)
        Else
            CreateNewTab(target)
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

    Private Function IsPopupWindow() As Boolean
        ' Safely determine whether the active element is a popup (BODY/IFRAME)
        Try
            If wb Is Nothing OrElse wb.Document Is Nothing Then
                Return False
            End If
            Dim el As HtmlElement = wb.Document.ActiveElement
            If el Is Nothing Then
                Return False
            End If
            Dim tag As String = If(el.TagName, "").ToUpperInvariant()
            Return (tag = "BODY" Or tag = "IFRAME")
        Catch ex As Exception
            ' On error, treat as not a popup
            Return False
        End Try
    End Function

    Private Sub Loading(ByVal sender As Object, ByVal e As Windows.Forms.WebBrowserProgressChangedEventArgs)
        Try
            ProgressBar1.Maximum = e.MaximumProgress
            ProgressBar1.Value = e.CurrentProgress
            Dim currentBrowser = TryCast(sender, WebBrowser)
            If currentBrowser IsNot Nothing Then
                Label1.Text = currentBrowser.StatusText
            End If
        Catch ex As Exception
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

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim brws As WebBrowser = Nothing
        Try
            brws = CreateNewTab(Path.Combine(Application.StartupPath, "homepage", "index.html"))
            brws.Name = "k-Browser"
            Me.TabControl1.SelectedTab.Text = "Loading..."
            Me.Size = My.Settings.MainSize
            Me.Location = My.Settings.MainLocation
        Catch ex As Exception
        End Try
        CalendarToolStripMenuItem.Text = DateTime.Now.ToLongDateString()
        If brws IsNot Nothing Then
            Label1.Text = brws.StatusText
        End If
        Me.SetStyle(System.Windows.Forms.ControlStyles.SupportsTransparentBackColor, True)
        Me.BackColor = System.Drawing.Color.Transparent
        If wb IsNot Nothing Then
            wb.IsWebBrowserContextMenuEnabled = True
        End If
    End Sub



    Private Sub CloseCurrentTab()
        If TabControl1.SelectedTab IsNot Nothing Then
            Dim selectedTab As TabPage = TabControl1.SelectedTab
            If selectedTab.Controls.Count > 0 Then
                Dim browserControl As WebBrowser = TryCast(selectedTab.Controls(0), WebBrowser)
                If browserControl IsNot Nothing Then
                    RemoveHandler browserControl.ProgressChanged, AddressOf Loading
                    RemoveHandler browserControl.Navigating, AddressOf WebBrowser_Navigating
                    RemoveHandler browserControl.DocumentCompleted, AddressOf WebBrowser_DocumentCompleted_SuppressErrors
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
        Dim cdlOpen As New OpenFileDialog
        Try
            cdlOpen.Filter = "HTML Files (*.html)|*.html|TextFiles" &
                "(*.txt)|*.txt|Gif Files (*.gif)|*.gif|JPEG Files (*.jpg)|*.jpeg|" &
                "PNG Files (*.png)|*.png|Art Files (*.art)|*.art|AU Fles (*.au)|*.au|" &
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
        If wb IsNot Nothing Then wb.ShowSaveAsDialog()
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        If wb IsNot Nothing Then wb.ShowPrintDialog()
    End Sub

    Private Sub PrintPreviewToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintPreviewToolStripMenuItem.Click
        If wb IsNot Nothing Then wb.ShowPrintPreviewDialog()
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

    Private Sub SourceToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SourceToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then
            Source.Show()
            Source.RichTextBox1.Text = Me.wb.DocumentText
        End If
    End Sub

    Private Sub PropertiesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PropertiesToolStripMenuItem.Click
        If wb IsNot Nothing Then wb.ShowPropertiesDialog()
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
        If wb IsNot Nothing AndAlso wb.CanGoBack Then wb.GoBack()
    End Sub

    Private Sub ToolStripButton2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton2.Click
        If wb IsNot Nothing AndAlso wb.CanGoForward Then wb.GoForward()
    End Sub

    Private Sub ToolStripButton3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton3.Click
        If wb IsNot Nothing Then wb.Refresh()
    End Sub

    Private Sub ToolStripButton4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton4.Click
        If wb IsNot Nothing Then wb.GoHome()
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
            CreateNewTab()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ToolStripButton6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton6.Click
        CloseCurrentTab()
    End Sub

    Private Sub CutToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CutToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then wb.Document.ExecCommand("Cut", False, vbNull)
    End Sub

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then wb.Document.ExecCommand("copy", False, vbNull)
    End Sub

    Private Sub PasteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PasteToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then wb.Document.ExecCommand("paste", False, vbNull)
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then wb.Document.ExecCommand("SelectAll", False, vbNull)
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
        Try
            Dim Res As Object = Nothing
            Dim MyWeb As Object
            MyWeb = wb.ActiveXInstance
            MyWeb.ExecWB(Exec.OLECMDID_OPTICAL_ZOOM,
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
            MyWeb.ExecWB(Exec.OLECMDID_OPTICAL_ZOOM,
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
        If wb IsNot Nothing Then wb.GoHome()
    End Sub

    Private Sub NewTabToolStripMenuItem1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles NewTabToolStripMenuItem1.Click
        Try
            CreateNewTab()
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
            wb.Navigate(lOpen.FileName)
        End If
    End Sub

    Private Sub ToolStripButton1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        If wb IsNot Nothing Then
            wb.Navigate("http://www.google.com/search?hl=en&q=" & Uri.EscapeDataString(searchTextBox2.Text))
            My.Settings.History.Add(searchTextBox2.Text)
            My.Settings.Save()
            History.ListBox1.Items.Add(searchTextBox2.Text)
        End If
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




    Private Shared ReadOnly IgnoredUrls As New System.Collections.Generic.HashSet(Of String)()

    Private Function CreateNewTab(Optional ByVal targetUrl As String = "") As WebBrowser
        Dim tab As New TabPage()
        Dim brws As New WebBrowser()
        brws.Name = "WebBrowser"
        brws.Dock = DockStyle.Fill
        brws.ScriptErrorsSuppressed = True
        tab.Text = "New Tab"
        tab.Controls.Add(brws)
        
        AddHandler brws.ProgressChanged, AddressOf Loading
        AddHandler brws.Navigating, AddressOf WebBrowser_Navigating
        AddHandler brws.DocumentCompleted, AddressOf WebBrowser_DocumentCompleted_SuppressErrors
        
        Me.TabControl1.TabPages.Add(tab)
        Me.TabControl1.SelectedTab = tab
        
        If Not String.IsNullOrEmpty(targetUrl) Then
            brws.Navigate(targetUrl)
        End If
        
        Return brws
    End Function

    Private Sub WebBrowser_DocumentCompleted_SuppressErrors(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs)
        Try
            Dim currentBrowser As WebBrowser = TryCast(sender, WebBrowser)
            If currentBrowser IsNot Nothing AndAlso currentBrowser.Document IsNot Nothing AndAlso currentBrowser.Document.Window IsNot Nothing Then
                RemoveHandler currentBrowser.Document.Window.Error, AddressOf Window_Error
                AddHandler currentBrowser.Document.Window.Error, AddressOf Window_Error
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub WebBrowser_Navigating(ByVal sender As Object, ByVal e As WebBrowserNavigatingEventArgs)
        Dim currentBrowser As WebBrowser = TryCast(sender, WebBrowser)
        If currentBrowser Is Nothing Then Return
        
        Dim url As String = e.Url.ToString()
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
                            currentBrowser.Navigate(url)
                        End If
                        Return
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If Me.TabControl1.SelectedTab IsNot Nothing AndAlso Me.TabControl1.SelectedTab.Controls.Count > 0 Then
            wb = TryCast(Me.TabControl1.SelectedTab.Controls(0), WebBrowser)
        Else
            wb = Nothing
        End If
    End Sub

    Private Sub wb_DocumentCompleted(ByVal sender As Object, ByVal e As WebBrowserDocumentCompletedEventArgs) Handles wb.DocumentCompleted
        Dim pageUrl As Uri = wb.Url
        If pageUrl Is Nothing OrElse pageUrl.HostNameType <> UriHostNameType.Dns Then
            img.BackgroundImage = Nothing
            Return
        End If
        Dim hostKey As String = pageUrl.Host.ToLowerInvariant()
        If faviconCache.ContainsKey(hostKey) Then
            img.BackgroundImage = faviconCache(hostKey)
            Return
        End If
        Dim iconUrl As String = Nothing
        Try
            Dim doc As HtmlDocument = wb.Document
            If doc IsNot Nothing Then
                For Each link As HtmlElement In doc.GetElementsByTagName("link")
                    Dim rel As String = link.GetAttribute("rel")
                    If rel IsNot Nothing AndAlso rel.ToLowerInvariant().Contains("icon") Then
                        iconUrl = link.GetAttribute("href")
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            iconUrl = Nothing
        End Try
        Dim faviconUri As Uri = Nothing
        If Not String.IsNullOrEmpty(iconUrl) Then
            Try
                faviconUri = New Uri(pageUrl, iconUrl)
            Catch ex As Exception
                faviconUri = Nothing
            End Try
        End If
        If faviconUri Is Nothing Then
            faviconUri = New Uri(pageUrl.Scheme & "://" & pageUrl.Host & "/favicon.ico")
        End If
        System.Diagnostics.Debug.WriteLine("Fetching favicon: " & faviconUri.ToString())
        System.Threading.Tasks.Task.Run(Function()
                                            Try
                                                Dim req As HttpWebRequest = CType(WebRequest.Create(faviconUri), HttpWebRequest)
                                                req.AllowAutoRedirect = True
                                                req.Timeout = 5000
                                                req.UserAgent = MyUserAgent
                                                Using res As HttpWebResponse = CType(req.GetResponse(), HttpWebResponse)
                                                    If res.StatusCode = HttpStatusCode.OK Then
                                                        Using ms As New IO.MemoryStream()
                                                            Using rspStream As IO.Stream = res.GetResponseStream()
                                                                If rspStream Is Nothing Then Return Nothing
                                                                rspStream.CopyTo(ms)
                                                            End Using
                                                            ms.Position = 0
                                                            Try
                                                                Using favImg As Image = Image.FromStream(ms)
                                                                    Return New Bitmap(favImg) ' Clone to release MemoryStream
                                                                End Using
                                                            Catch exImg As Exception
                                                                System.Diagnostics.Debug.WriteLine("Image.FromStream failed: " & exImg.ToString())
                                                                Return Nothing
                                                            End Try
                                                        End Using

                                                    Else
                                                        System.Diagnostics.Debug.WriteLine("Favicon returned status: " & res.StatusCode.ToString())
                                                        Return Nothing
                                                    End If
                                                End Using
                                            Catch wex As WebException
                                                Dim resp = TryCast(wex.Response, HttpWebResponse)
                                                If resp IsNot Nothing Then
                                                    System.Diagnostics.Debug.WriteLine("WebException fetching favicon: " & resp.StatusCode.ToString())
                                                Else
                                                    System.Diagnostics.Debug.WriteLine("WebException fetching favicon: " & wex.Message)
                                                End If
                                                Return Nothing
                                            Catch ex As Exception
                                                System.Diagnostics.Debug.WriteLine("Exception fetching favicon: " & ex.ToString())
                                                Return Nothing
                                            End Try
                                        End Function).ContinueWith(Sub(t)
                                                                       Dim resultImg As Image = Nothing
                                                                       If t.Status = System.Threading.Tasks.TaskStatus.RanToCompletion Then
                                                                           resultImg = t.Result
                                                                       End If
                                                                       If Me.IsHandleCreated Then
                                                                           Me.Invoke(Sub()
                                                                                         If resultImg IsNot Nothing Then
                                                                                             If faviconCache.Count >= MAX_FAVICON_CACHE Then
                                                                                                 For Each kvp In faviconCache.Values.ToList()
                                                                                                     kvp.Dispose()
                                                                                                 Next
                                                                                                 faviconCache.Clear()
                                                                                             End If
                                                                                             img.BackgroundImage = resultImg
                                                                                             faviconCache(hostKey) = resultImg
                                                                                         Else
                                                                                             img.BackgroundImage = Nothing
                                                                                         End If
                                                                                     End Sub)
                                                                       End If
                                                                   End Sub, System.Threading.Tasks.TaskScheduler.Default)
    End Sub

    Private Sub wb_Navigated(ByVal sender As Object, ByVal e As System.Windows.Forms.WebBrowserNavigatedEventArgs) Handles wb.Navigated
        Dim pageUrl As Uri = wb.Url
        If pageUrl IsNot Nothing Then
            ToolStripTextBox1.Text = pageUrl.ToString()
        Else
            ToolStripTextBox1.Text = String.Empty
            img.BackgroundImage = Nothing
            Return
        End If
        If pageUrl.HostNameType <> UriHostNameType.Dns Then
            img.BackgroundImage = Nothing
            Return
        End If
        Dim hostKey As String = pageUrl.Host.ToLowerInvariant()
        If faviconCache.ContainsKey(hostKey) Then
            img.BackgroundImage = faviconCache(hostKey)
            Return
        End If
        ' Try to get favicon from <link rel="icon"> in DocumentCompleted
        img.BackgroundImage = Nothing
    End Sub

    Private Sub Window_Error(ByVal sender As Object, ByVal e As HtmlElementErrorEventArgs)
        ' Ignore the error and suppress the error dialog box. 
        e.Handled = True
    End Sub


    Private Sub CheckForUpdatesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckForUpdatesToolStripMenuItem.Click
        wb.Navigate("k-browser.com")
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

    Private Sub mnuLeftToRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuLeftToRight.Click
        Try
            Dim cur As WebBrowser = TryCast(Me.TabControl1.SelectedTab?.Controls(0), WebBrowser)
            If cur IsNot Nothing AndAlso cur.Document IsNot Nothing Then
                cur.Document.RightToLeft = False
            Else
                System.Diagnostics.Debug.WriteLine("mnuLeftToRight_Click: browser or document is not ready.")
            End If

            ' Keep menu checks consistent
            mnuLeftToRight.Checked = True
            If mnuRightToLeft IsNot Nothing Then mnuRightToLeft.Checked = False
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("mnuLeftToRight_Click exception: " & ex.ToString())
            Throw
        End Try
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
        If Not address.StartsWith("http://") And
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
            CreateNewTab()
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
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then
            wb.Document.ExecCommand("FontSize", True, "4")
        End If
        largestToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallestToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallestToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then
            wb.Document.ExecCommand("FontSize", True, "0")
        End If
        smallestToolStripMenuItem.Checked = True
    End Sub

    Private Sub largerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles largerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then
            wb.Document.ExecCommand("FontSize", True, "3")
        End If
        largerToolStripMenuItem.Checked = True
    End Sub

    Private Sub mediumToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mediumToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then
            wb.Document.ExecCommand("FontSize", True, "2")
        End If
        mediumToolStripMenuItem.Checked = True
    End Sub

    Private Sub smallerToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles smallerToolStripMenuItem.Click
        smallerToolStripMenuItem.Checked = False
        smallestToolStripMenuItem.Checked = False
        mediumToolStripMenuItem.Checked = False
        largerToolStripMenuItem.Checked = False
        largestToolStripMenuItem.Checked = False
        If wb IsNot Nothing AndAlso wb.Document IsNot Nothing Then
            wb.Document.ExecCommand("FontSize", True, "1")
        End If
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

    Private Sub wb_DocumentTitleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles wb.DocumentTitleChanged
        Label1.Text = wb.DocumentTitle
    End Sub
    Public Function PopulateUrlList() As List(Of String)
        Dim regKey As String = "Software\Microsoft\Internet Explorer\TypedURLs"
        Dim urlList As New List(Of String)()
        Try
            Using subKey As RegistryKey = Registry.CurrentUser.OpenSubKey(regKey)
                If subKey IsNot Nothing Then
                    Dim counter As Integer = 1
                    While True
                        Dim sValName As String = "url" + counter.ToString()
                        Dim url As String = TryCast(subKey.GetValue(sValName), String)
                        If String.IsNullOrEmpty(url) Then
                            Exit While
                        End If
                        urlList.Add(url)
                        counter += 1
                    End While
                End If
            End Using
        Catch ex As Exception
        End Try
        Return urlList
    End Function


    Private Sub ToolStripMenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripMenuItem5.Click
        On Error Resume Next
        Dim psi As New System.Diagnostics.ProcessStartInfo("iexplore")
        psi.Arguments = ToolStripTextBox1.Text
        System.Diagnostics.Process.Start(psi)
    End Sub

    Private Sub KBrowserToolStripMenuItem_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles KBrowserToolStripMenuItem.Click
        wb.Navigate("http://k-browser.com")
    End Sub

    Private Sub SToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SToolStripMenuItem.Click
        System.Diagnostics.Process.Start("mailto:""denkon24@yahoo.com")
    End Sub
End Class
