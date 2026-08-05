Imports System.IO
Imports System.Runtime.InteropServices
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Private browser window form providing Edge InPrivate-style isolated browsing.
''' Uses a dedicated PrivateSessionService for environment creation and lifecycle management.
''' 
''' Key behaviors:
''' - WebView2 runs in a completely isolated environment (separate temp folder)
''' - No history is recorded to My.Settings.History
''' - Bookmarks are disabled with a user-facing message
''' - Downloads are allowed but not logged to normal download history
''' - All session data is destroyed when the window closes
''' - Dark theme is always applied as a visual indicator of private mode
''' </summary>
Public Class PrivateBrowserForm

    ' Windows DWM Immersive Dark Mode API for dark title bar
    <DllImport("dwmapi.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function DwmSetWindowAttribute(ByVal hwnd As IntPtr, ByVal attr As Integer, ByRef attrValue As Integer, ByVal attrSize As Integer) As Integer
    End Function

    Private Const DWMWA_USE_IMMERSIVE_DARK_MODE As Integer = 20
    Private Const DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 As Integer = 19

    Private ReadOnly _sessionService As PrivateSessionService
    Private _isInitialized As Boolean = False

    ''' <summary>
    ''' Creates a new PrivateBrowserForm with the given session service.
    ''' The session service provides the isolated WebView2 environment.
    ''' </summary>
    ''' <param name="sessionService">The PrivateSessionService managing this session's lifecycle.</param>
    Public Sub New(ByVal sessionService As PrivateSessionService)
        If sessionService Is Nothing Then
            Throw New ArgumentNullException(NameOf(sessionService))
        End If

        _sessionService = sessionService
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Initializes the private browser window: applies dark theme, dark title bar,
    ''' and initializes the WebView2 control with the private environment.
    ''' </summary>
    Private Async Sub PrivateBrowserForm_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        Try
            ' Apply dark title bar via Windows DWM API
            ApplyDarkTitleBar()

            ' Apply zoom from user settings
            ApplyZoomFromSettings()

            ' Initialize WebView2 with the private environment
            Await InitializePrivateWebViewAsync()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PrivateBrowserForm: Load error: " & ex.Message)
            MessageBox.Show("Failed to initialize private browsing window." & vbCrLf & vbCrLf & "Please ensure WebView2 Runtime is installed.",
                           "Private Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
        End Try
    End Sub

    ''' <summary>
    ''' Applies the Windows 10/11 dark title bar to the form.
    ''' </summary>
    Private Sub ApplyDarkTitleBar()
        Try
            Dim useDark As Integer = 1
            DwmSetWindowAttribute(Me.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, useDark, Marshal.SizeOf(useDark))
            DwmSetWindowAttribute(Me.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1, useDark, Marshal.SizeOf(useDark))
        Catch
            ' DWM API not available on older Windows versions — silently ignore
        End Try
    End Sub

    ''' <summary>
    ''' Applies the user's zoom preference from My.Settings to the private WebView.
    ''' </summary>
    Private Sub ApplyZoomFromSettings()
        Try
            Select Case My.Settings.FontSize
                Case 0 : wvPrivate.ZoomFactor = 0.85
                Case 2 : wvPrivate.ZoomFactor = 1.25
                Case 3 : wvPrivate.ZoomFactor = 1.5
                Case Else : wvPrivate.ZoomFactor = 1.0
            End Select
        Catch
            ' Silently default to 1.0 if settings are unavailable
        End Try
    End Sub

    ''' <summary>
    ''' Creates the private WebView2 environment and initializes the WebView2 control.
    ''' </summary>
    Private Async Function InitializePrivateWebViewAsync() As Task
        tslStatus.Text = "Initializing private session..."
        tspProgress.Value = 25

        ' Create the isolated environment via the session service
        Dim env As CoreWebView2Environment = Await _sessionService.CreatePrivateEnvironmentAsync()

        If env Is Nothing Then
            tslStatus.Text = "Failed to create private environment"
            MessageBox.Show("Could not create a private browsing environment." & vbCrLf & "Please try again.",
                           "Private Mode Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Me.Close()
            Return
        End If

        tspProgress.Value = 50

        ' Initialize WebView2 with the private environment
        Await wvPrivate.EnsureCoreWebView2Async(env)

        If wvPrivate.CoreWebView2 Is Nothing Then
            tslStatus.Text = "WebView2 initialization failed"
            Me.Close()
            Return
        End If

        tspProgress.Value = 75

        ' Configure WebView2 settings for private mode
        ConfigurePrivateWebViewSettings()

        ' Wire up event handlers
        AddHandler wvPrivate.CoreWebView2.NavigationStarting, AddressOf PrivateWebView_NavigationStarting
        AddHandler wvPrivate.CoreWebView2.NavigationCompleted, AddressOf PrivateWebView_NavigationCompleted
        AddHandler wvPrivate.CoreWebView2.SourceChanged, AddressOf PrivateWebView_SourceChanged
        AddHandler wvPrivate.CoreWebView2.DocumentTitleChanged, AddressOf PrivateWebView_DocumentTitleChanged
        AddHandler wvPrivate.CoreWebView2.NewWindowRequested, AddressOf PrivateWebView_NewWindowRequested
        AddHandler wvPrivate.CoreWebView2.DownloadStarting, AddressOf PrivateWebView_DownloadStarting

        ' Apply dark color scheme to WebView2
        Try
            wvPrivate.CoreWebView2.Profile.PreferredColorScheme = CoreWebView2PreferredColorScheme.Dark
        Catch
            ' Silently ignore if PreferredColorScheme is not supported
        End Try

        tspProgress.Value = 100
        tslStatus.Text = "Private Mode"
        _isInitialized = True

        ' Navigate to the home page
        Dim homeUrl As String = Form1.GetHomePageUrl()
        wvPrivate.CoreWebView2.Navigate(homeUrl)
    End Function

    ''' <summary>
    ''' Configures WebView2 settings appropriate for private browsing.
    ''' Disables password saving, autofill, and other persistence features.
    ''' </summary>
    Private Sub ConfigurePrivateWebViewSettings()
        If wvPrivate.CoreWebView2 Is Nothing Then Return

        With wvPrivate.CoreWebView2.Settings
            .IsStatusBarEnabled = True
            .AreDefaultScriptDialogsEnabled = True
            .IsScriptEnabled = True
            .IsWebMessageEnabled = True
            .AreDevToolsEnabled = True

            ' Disable persistence features for private mode
            .IsPasswordAutosaveEnabled = False
            .IsGeneralAutofillEnabled = False
        End With
    End Sub

    ' ─────────────────────────────────────────────────
    ' Navigation Toolbar Handlers
    ' ─────────────────────────────────────────────────

    Private Sub btnBack_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBack.Click
        If wvPrivate.CoreWebView2 IsNot Nothing AndAlso wvPrivate.CanGoBack Then
            wvPrivate.GoBack()
        End If
    End Sub

    Private Sub btnForward_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnForward.Click
        If wvPrivate.CoreWebView2 IsNot Nothing AndAlso wvPrivate.CanGoForward Then
            wvPrivate.GoForward()
        End If
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefresh.Click
        If wvPrivate.CoreWebView2 IsNot Nothing Then
            wvPrivate.Reload()
        End If
    End Sub

    Private Sub btnHome_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnHome.Click
        If wvPrivate.CoreWebView2 IsNot Nothing Then
            Dim homeUrl As String = Form1.GetHomePageUrl()
            Dim target As String = AppManager.ResolveUrlOrSearch(homeUrl, My.Settings.SearchEngine)
            wvPrivate.CoreWebView2.Navigate(target)
        End If
    End Sub

    Private Sub btnGo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGo.Click
        NavigateToAddress()
    End Sub

    Private Sub txtAddress_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs) Handles txtAddress.KeyDown
        If e.KeyCode = Keys.Enter Then
            NavigateToAddress()
        End If
    End Sub

    Private Sub txtAddress_GotFocus(ByVal sender As Object, ByVal e As EventArgs) Handles txtAddress.GotFocus
        txtAddress.SelectAll()
    End Sub

    ''' <summary>
    ''' Navigates the private WebView to the URL in the address bar.
    ''' Uses AppManager.ResolveUrlOrSearch to handle search queries vs direct URLs.
    ''' </summary>
    Private Sub NavigateToAddress()
        If String.IsNullOrWhiteSpace(txtAddress.Text) Then Return
        If wvPrivate.CoreWebView2 Is Nothing Then Return

        Dim target As String = AppManager.ResolveUrlOrSearch(txtAddress.Text, My.Settings.SearchEngine)
        wvPrivate.CoreWebView2.Navigate(target)
    End Sub

    ''' <summary>
    ''' Bookmark button handler — disabled in private mode.
    ''' </summary>
    Private Sub btnBookmark_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnBookmark.Click
        MessageBox.Show("Bookmarks are disabled in Private Mode." & vbCrLf & vbCrLf &
                       "Private browsing is designed to leave no trace. " &
                       "To bookmark a page, please use a regular browser window.",
                       "Private Mode", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    ' ─────────────────────────────────────────────────
    ' WebView2 Event Handlers (Private Mode)
    ' ─────────────────────────────────────────────────

    ''' <summary>
    ''' Handles navigation start — updates progress bar and status.
    ''' Note: We do NOT record any URLs to history in private mode.
    ''' </summary>
    Private Sub PrivateWebView_NavigationStarting(ByVal sender As Object, ByVal e As CoreWebView2NavigationStartingEventArgs)
        tspProgress.Visible = True
        tspProgress.Value = 30
        tslStatus.Text = "Connecting..."

        ' Apply HTTPS-Only Mode if enabled
        If My.Settings.HttpsOnlyMode AndAlso e.Uri.StartsWith("http://", StringComparison.OrdinalIgnoreCase) Then
            e.Cancel = True
            Dim httpsUrl As String = "https://" & e.Uri.Substring(7)
            Dim core = TryCast(sender, CoreWebView2)
            If core IsNot Nothing Then core.Navigate(httpsUrl)
            Return
        End If

        ' Apply blocked sites check
        If My.Settings.BlockedSites IsNot Nothing Then
            For Each blockedUrl As String In My.Settings.BlockedSites
                If Not String.IsNullOrEmpty(blockedUrl) Then
                    If e.Uri.ToLower().Contains(blockedUrl.ToLower()) Then
                        e.Cancel = True
                        MessageBox.Show("This site is blocked by K-Browser Settings.", "Access Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        Return
                    End If
                End If
            Next
        End If
    End Sub

    ''' <summary>
    ''' Handles navigation complete — updates progress bar and address bar.
    ''' NO history is recorded in private mode.
    ''' </summary>
    Private Sub PrivateWebView_NavigationCompleted(ByVal sender As Object, ByVal e As CoreWebView2NavigationCompletedEventArgs)
        tspProgress.Value = 100
        tslStatus.Text = If(e.IsSuccess, "Private Mode", "Failed")

        ' Update address bar with current URL
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing Then
            txtAddress.Text = core.Source
        End If

        ' Update navigation button states
        btnBack.Enabled = wvPrivate.CanGoBack
        btnForward.Enabled = wvPrivate.CanGoForward

        ' Reset progress bar after a short delay
        ResetProgressBarAsync()
    End Sub

    Private Async Sub ResetProgressBarAsync()
        Try
            Await System.Threading.Tasks.Task.Delay(800)
            tspProgress.Value = 0
        Catch
            ' Silently ignore if form is disposed during delay
        End Try
    End Sub

    ''' <summary>
    ''' Updates the address bar when the WebView source changes.
    ''' </summary>
    Private Sub PrivateWebView_SourceChanged(ByVal sender As Object, ByVal e As CoreWebView2SourceChangedEventArgs)
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing Then
            txtAddress.Text = core.Source
        End If
    End Sub

    ''' <summary>
    ''' Updates the window title with the page title — but does NOT record to history.
    ''' The window title always includes "Private" as a visual indicator.
    ''' </summary>
    Private Sub PrivateWebView_DocumentTitleChanged(ByVal sender As Object, ByVal e As Object)
        Dim core = TryCast(sender, CoreWebView2)
        If core IsNot Nothing AndAlso Not String.IsNullOrEmpty(core.DocumentTitle) Then
            Me.Text = core.DocumentTitle & " - K Browser - Private"
            tslStatus.Text = "Private Mode"
        End If

        ' IMPORTANT: Do NOT record history — this is private mode
        ' No writes to My.Settings.History
    End Sub

    ''' <summary>
    ''' Handles new window requests by navigating in the same private window.
    ''' Does not open new Form1 windows — keeps everything in the private session.
    ''' </summary>
    Private Sub PrivateWebView_NewWindowRequested(ByVal sender As Object, ByVal e As CoreWebView2NewWindowRequestedEventArgs)
        e.Handled = True
        If Not String.IsNullOrWhiteSpace(e.Uri) AndAlso wvPrivate.CoreWebView2 IsNot Nothing Then
            wvPrivate.CoreWebView2.Navigate(e.Uri)
        End If
    End Sub

    ''' <summary>
    ''' Handles download events — allows downloads but does NOT log them to normal download history.
    ''' </summary>
    Private Sub PrivateWebView_DownloadStarting(ByVal sender As Object, ByVal e As CoreWebView2DownloadStartingEventArgs)
        ' Allow the download to proceed with the default save location
        ' Do NOT log this download to any persistent download history
        Try
            If Not String.IsNullOrWhiteSpace(My.Settings.DownloadsFolder) AndAlso
               System.IO.Directory.Exists(My.Settings.DownloadsFolder) Then
                e.ResultFilePath = System.IO.Path.Combine(My.Settings.DownloadsFolder, System.IO.Path.GetFileName(e.ResultFilePath))
            End If
        Catch
            ' Silently use default download path if settings are unavailable
        End Try
    End Sub

    ' ─────────────────────────────────────────────────
    ' Keyboard Shortcuts
    ' ─────────────────────────────────────────────────

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, ByVal keyData As Keys) As Boolean
        Select Case keyData
            Case Keys.Alt Or Keys.Left
                If wvPrivate.CanGoBack Then wvPrivate.GoBack()
                Return True
            Case Keys.Alt Or Keys.Right
                If wvPrivate.CanGoForward Then wvPrivate.GoForward()
                Return True
            Case Keys.F5
                If wvPrivate.CoreWebView2 IsNot Nothing Then wvPrivate.Reload()
                Return True
            Case Keys.F6
                txtAddress.Focus()
                Return True
            Case Keys.Control Or Keys.L
                txtAddress.Focus()
                txtAddress.SelectAll()
                Return True
        End Select
        Return MyBase.ProcessCmdKey(msg, keyData)
    End Function

    ' ─────────────────────────────────────────────────
    ' Form Closing — Session Cleanup
    ' ─────────────────────────────────────────────────

    ''' <summary>
    ''' Handles form closing: cleans up the private session by clearing all browsing data
    ''' and deleting the temporary user data folder. Shows a dismissible notification.
    ''' </summary>
    Private Async Sub PrivateBrowserForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles MyBase.FormClosing
        Try
            ' Unhook event handlers to prevent callbacks during disposal
            If wvPrivate.CoreWebView2 IsNot Nothing Then
                RemoveHandler wvPrivate.CoreWebView2.NavigationStarting, AddressOf PrivateWebView_NavigationStarting
                RemoveHandler wvPrivate.CoreWebView2.NavigationCompleted, AddressOf PrivateWebView_NavigationCompleted
                RemoveHandler wvPrivate.CoreWebView2.SourceChanged, AddressOf PrivateWebView_SourceChanged
                RemoveHandler wvPrivate.CoreWebView2.DocumentTitleChanged, AddressOf PrivateWebView_DocumentTitleChanged
                RemoveHandler wvPrivate.CoreWebView2.NewWindowRequested, AddressOf PrivateWebView_NewWindowRequested
                RemoveHandler wvPrivate.CoreWebView2.DownloadStarting, AddressOf PrivateWebView_DownloadStarting
            End If

            ' Cleanup the private session (clear browsing data + delete temp folder)
            Await _sessionService.CleanupSessionAsync(wvPrivate)

            ' Dispose the WebView2 control
            Try
                wvPrivate.Dispose()
            Catch
                ' Silently ignore disposal errors
            End Try

            ' Dispose the session service
            _sessionService.Dispose()

            ' Show session closed notification (unless user opted out)
            ShowSessionClosedNotification()
        Catch ex As Exception
            ' Log technical error only — never log private URLs
            System.Diagnostics.Debug.WriteLine("PrivateBrowserForm: Error during closing cleanup: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Shows a dismissible notification that the private session has been closed
    ''' and all browsing data has been deleted. Includes a "Don't show again" checkbox.
    ''' </summary>
    Private Sub ShowSessionClosedNotification()
        Try
            ' Check if user opted out of this notification
            If My.Settings.SuppressPrivateCloseMessage Then
                Return
            End If

            ' Build the notification form programmatically
            Using notifyForm As New Form()
                notifyForm.Text = "Private Browsing Session Closed"
                notifyForm.Size = New Drawing.Size(420, 200)
                notifyForm.StartPosition = FormStartPosition.CenterScreen
                notifyForm.FormBorderStyle = FormBorderStyle.FixedDialog
                notifyForm.MaximizeBox = False
                notifyForm.MinimizeBox = False
                notifyForm.BackColor = Drawing.Color.FromArgb(28, 36, 52)
                notifyForm.ForeColor = Drawing.Color.FromArgb(235, 240, 245)

                ' Apply dark title bar
                Try
                    Dim useDark As Integer = 1
                    DwmSetWindowAttribute(notifyForm.Handle, DWMWA_USE_IMMERSIVE_DARK_MODE, useDark, Marshal.SizeOf(useDark))
                Catch
                End Try

                Dim iconLabel As New Label()
                iconLabel.Text = "🕶"
                iconLabel.Font = New Drawing.Font("Segoe UI", 24.0F)
                iconLabel.AutoSize = True
                iconLabel.Location = New Drawing.Point(20, 15)
                notifyForm.Controls.Add(iconLabel)

                Dim msgLabel As New Label()
                msgLabel.Text = "Private browsing session closed." & vbCrLf & vbCrLf & "Your browsing data has been deleted."
                msgLabel.Font = New Drawing.Font("Segoe UI", 10.0F)
                msgLabel.ForeColor = Drawing.Color.FromArgb(235, 240, 245)
                msgLabel.AutoSize = True
                msgLabel.Location = New Drawing.Point(80, 20)
                notifyForm.Controls.Add(msgLabel)

                Dim chkDontShow As New CheckBox()
                chkDontShow.Text = "Don't show this message again"
                chkDontShow.ForeColor = Drawing.Color.FromArgb(160, 175, 195)
                chkDontShow.Font = New Drawing.Font("Segoe UI", 8.5F)
                chkDontShow.AutoSize = True
                chkDontShow.Location = New Drawing.Point(80, 90)
                notifyForm.Controls.Add(chkDontShow)

                Dim btnOk As New Button()
                btnOk.Text = "OK"
                btnOk.Size = New Drawing.Size(90, 32)
                btnOk.Location = New Drawing.Point(300, 120)
                btnOk.FlatStyle = FlatStyle.Flat
                btnOk.BackColor = Drawing.Color.FromArgb(161, 197, 74)
                btnOk.ForeColor = Drawing.Color.FromArgb(28, 36, 52)
                btnOk.Font = New Drawing.Font("Segoe UI", 9.0F, Drawing.FontStyle.Bold)
                btnOk.FlatAppearance.BorderSize = 0
                AddHandler btnOk.Click, Sub(s, ev)
                                            If chkDontShow.Checked Then
                                                Try
                                                    My.Settings.SuppressPrivateCloseMessage = True
                                                    My.Settings.Save()
                                                Catch
                                                    ' Silently ignore if settings cannot be saved
                                                End Try
                                            End If
                                            notifyForm.Close()
                                        End Sub
                notifyForm.Controls.Add(btnOk)
                notifyForm.AcceptButton = btnOk

                notifyForm.ShowDialog()
            End Using
        Catch
            ' Silently ignore notification display errors
        End Try
    End Sub

End Class
