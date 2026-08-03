Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms

Public Class Settings

    Private m_jsDisabledDraft As New List(Of String)()
    Private m_blockedSitesDraft As New List(Of String)()
    Private m_isSyncingDns As Boolean = False

    Private ReadOnly _privacySettingsService As New PrivacySettingsService()
    Private ReadOnly _cookieService As New CookieService()
    Private ReadOnly _cookieExportService As New CookieExportService()
    Private ReadOnly _backupService As New BackupService()
    Private ReadOnly _restoreService As New RestoreService()
    Private ReadOnly _sessionService As New SessionService()
    Private ReadOnly _settingsService As New SettingsService()
    Private _privacyModel As PrivacySettingsModel = New PrivacySettingsModel()
    Private _loadedCookies As New List(Of CookieItem)()
    Private _activeWebView As Microsoft.Web.WebView2.WinForms.WebView2 = Nothing
    Private Const SearchPlaceholder As String = "Search website..."

    Private Async Sub Settings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ThemeManager.ApplyTheme(Me)
        Try
            If Form1.ActiveForm IsNot Nothing AndAlso Form1.ActiveForm.Icon IsNot Nothing Then
                Me.Icon = Form1.ActiveForm.Icon
            ElseIf Application.OpenForms.Count > 0 AndAlso Application.OpenForms(0).Icon IsNot Nothing Then
                Me.Icon = Application.OpenForms(0).Icon
            End If
        Catch
        End Try

        LoadAllSettings()
        FindActiveWebView()
        Await LoadPrivacyTabSettingsAsync()
        Await RefreshCookiesGridAsync()
    End Sub

    Private Sub FindActiveWebView()
        Try
            If Form1.ActiveForm IsNot Nothing AndAlso TypeOf Form1.ActiveForm Is Form1 Then
                Dim main As Form1 = DirectCast(Form1.ActiveForm, Form1)
                If main.TabControl1 IsNot Nothing AndAlso main.TabControl1.SelectedTab IsNot Nothing AndAlso main.TabControl1.SelectedTab.Controls.Count > 0 Then
                    _activeWebView = TryCast(main.TabControl1.SelectedTab.Controls(0), Microsoft.Web.WebView2.WinForms.WebView2)
                End If
            End If

            If _activeWebView Is Nothing Then
                For Each f As Form In Application.OpenForms
                    If TypeOf f Is Form1 Then
                        Dim main As Form1 = DirectCast(f, Form1)
                        If main.TabControl1 IsNot Nothing AndAlso main.TabControl1.SelectedTab IsNot Nothing AndAlso main.TabControl1.SelectedTab.Controls.Count > 0 Then
                            _activeWebView = TryCast(main.TabControl1.SelectedTab.Controls(0), Microsoft.Web.WebView2.WinForms.WebView2)
                            If _activeWebView IsNot Nothing Then Exit For
                        End If
                    End If
                Next
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Settings: Error discovering active WebView2 control: " & ex.Message)
        End Try
    End Sub

    Private Async Function LoadPrivacyTabSettingsAsync() As Task
        Try
            _privacyModel = Await _privacySettingsService.LoadPrivacySettingsAsync()
            chkBlockThirdPartyCookies.Checked = _privacyModel.BlockThirdPartyCookies

            ' Restore StartupMode
            If _privacyModel.StartupMode = "RestoreSession" Then
                rbContinueWhereLeftOff.Checked = True
                rbOpenNewTabPage.Checked = False
            Else
                rbOpenNewTabPage.Checked = True
                rbContinueWhereLeftOff.Checked = False
            End If

            ' Restore LastBackup label
            If _privacyModel.LastBackup.HasValue Then
                lblLastBackup.Text = "Last backup: " & _privacyModel.LastBackup.Value.ToString("dd MMM yyyy HH:mm")
            Else
                lblLastBackup.Text = "Last backup: Never"
            End If

            ' Restore saved column widths from Settings.json
            If _privacyModel.ColumnWidths IsNot Nothing AndAlso _privacyModel.ColumnWidths.Count > 0 Then
                For Each col As DataGridViewColumn In dgvCookies.Columns
                    If _privacyModel.ColumnWidths.ContainsKey(col.Name) Then
                        col.Width = _privacyModel.ColumnWidths(col.Name)
                    End If
                Next
            End If

            ' Restore Performance settings
            If _privacyModel.Performance IsNot Nothing Then
                chkUseHardwareAcceleration.Checked = _privacyModel.Performance.UseHardwareAcceleration
                chkFadeInactiveTabs.Checked = _privacyModel.Performance.FadeInactiveTabs
                chkPreloadPages.Checked = _privacyModel.Performance.PreloadPages
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Settings: Error loading Privacy Tab Settings: " & ex.Message)
        End Try
    End Function

    Private Sub LoadAllSettings()
        ' Browser Settings
        txtHomePage.Text = If(String.IsNullOrEmpty(My.Settings.HomePageUrl), "https://www.google.com", My.Settings.HomePageUrl)
        cmbSearchEngine.SelectedItem = If(String.IsNullOrEmpty(My.Settings.SearchEngine), "Google", My.Settings.SearchEngine)
        If cmbSearchEngine.SelectedIndex < 0 Then cmbSearchEngine.SelectedIndex = 0
        txtDownloads.Text = If(String.IsNullOrEmpty(My.Settings.DownloadsFolder), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads"), My.Settings.DownloadsFolder)
        cmbFontSize.SelectedIndex = Math.Max(0, Math.Min(3, My.Settings.FontSize))
        chkFullScreen.Checked = My.Settings.FullScreenOnStartup

        ' Privacy & Security
        chkPermCamera.Checked = My.Settings.PermissionCamera
        chkPermMic.Checked = My.Settings.PermissionMic
        chkPermLocation.Checked = My.Settings.PermissionLocation
        chkPermNotifications.Checked = My.Settings.PermissionNotifications
        chkIncognito.Checked = My.Settings.IncognitoEnabled
        chkHttpsOnly.Checked = My.Settings.HttpsOnlyMode
        chkAdBlocker.Checked = My.Settings.AdBlockerEnabled
        chkPhishing.Checked = My.Settings.UsePhishingFilter
        chkAllowPop.Checked = My.Settings.PopUpBlockerEnabled

        ' Load collections into isolated draft buffers
        m_jsDisabledDraft.Clear()
        If My.Settings.JsDisabledSites IsNot Nothing Then
            For Each s As String In My.Settings.JsDisabledSites
                If Not String.IsNullOrWhiteSpace(s) Then m_jsDisabledDraft.Add(s)
            Next
        End If
        LoadJsDisabledSites()

        m_blockedSitesDraft.Clear()
        If My.Settings.BlockedSites IsNot Nothing Then
            For Each s As String In My.Settings.BlockedSites
                If Not String.IsNullOrWhiteSpace(s) Then m_blockedSitesDraft.Add(s)
            Next
        End If
        LoadBlockedSites()

        ' Performance settings are loaded asynchronously from Settings.json via LoadPrivacyTabSettingsAsync()

        txtCustomDns.Text = If(My.Settings.CustomDnsServer IsNot Nothing, My.Settings.CustomDnsServer, "")

        ' Advanced
        chkEnableProxy.Checked = My.Settings.CustomProxyEnabled
        txtProxyHost.Text = My.Settings.CustomProxyHost
        txtProxyPort.Text = My.Settings.CustomProxyPort.ToString()
    End Sub

    Private Async Function SaveAllSettingsAsync() As Task
        ' Save standard WinForms settings
        SaveAllSettings()

        ' Save Privacy, Startup & Performance Settings to Settings.json
        If _privacyModel Is Nothing Then _privacyModel = New PrivacySettingsModel()
        _privacyModel.BlockThirdPartyCookies = chkBlockThirdPartyCookies.Checked
        _privacyModel.StartupMode = If(rbContinueWhereLeftOff.Checked, "RestoreSession", "NewTab")

        If _privacyModel.Performance Is Nothing Then _privacyModel.Performance = New PerformanceSettingsModel()
        _privacyModel.Performance.UseHardwareAcceleration = chkUseHardwareAcceleration.Checked
        _privacyModel.Performance.FadeInactiveTabs = chkFadeInactiveTabs.Checked
        _privacyModel.Performance.PreloadPages = chkPreloadPages.Checked

        ' Apply live preferences
        TabLifecycleManager.FadeInactiveTabsEnabled = chkFadeInactiveTabs.Checked

        ' Preserve column widths
        If _privacyModel.ColumnWidths Is Nothing Then _privacyModel.ColumnWidths = New Dictionary(Of String, Integer)()
        For Each col As DataGridViewColumn In dgvCookies.Columns
            _privacyModel.ColumnWidths(col.Name) = col.Width
        Next

        Try
            Await _privacySettingsService.SavePrivacySettingsAsync(_privacyModel)

            ' Apply privacy settings immediately to active WebView2 control
            If _activeWebView IsNot Nothing Then
                _privacySettingsService.ApplyThirdPartyCookieBlocking(_activeWebView, _privacyModel.BlockThirdPartyCookies)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Settings: Error saving privacy settings to Settings.json: " & ex.Message)
        End Try
    End Function

    Private Sub SaveAllSettings()
        ' Browser Settings
        My.Settings.HomePageUrl = txtHomePage.Text.Trim()
        My.Settings.StartupBehavior = If(rbContinueWhereLeftOff.Checked, 2, 0)
        My.Settings.SearchEngine = If(cmbSearchEngine.SelectedItem IsNot Nothing, cmbSearchEngine.SelectedItem.ToString(), "Google")
        My.Settings.DownloadsFolder = txtDownloads.Text.Trim()
        My.Settings.FontSize = cmbFontSize.SelectedIndex
        My.Settings.FullScreenOnStartup = chkFullScreen.Checked

        ' Privacy & Security
        My.Settings.PermissionCamera = chkPermCamera.Checked
        My.Settings.PermissionMic = chkPermMic.Checked
        My.Settings.PermissionLocation = chkPermLocation.Checked
        My.Settings.PermissionNotifications = chkPermNotifications.Checked
        My.Settings.IncognitoEnabled = chkIncognito.Checked
        My.Settings.HttpsOnlyMode = chkHttpsOnly.Checked
        My.Settings.AdBlockerEnabled = chkAdBlocker.Checked
        My.Settings.UsePhishingFilter = chkPhishing.Checked
        My.Settings.PopUpBlockerEnabled = chkAllowPop.Checked

        ' Commit Draft Collections
        If My.Settings.JsDisabledSites Is Nothing Then
            My.Settings.JsDisabledSites = New System.Collections.Specialized.StringCollection()
        End If
        My.Settings.JsDisabledSites.Clear()
        For Each s As String In m_jsDisabledDraft
            My.Settings.JsDisabledSites.Add(s)
        Next

        If My.Settings.BlockedSites Is Nothing Then
            My.Settings.BlockedSites = New System.Collections.Specialized.StringCollection()
        End If
        My.Settings.BlockedSites.Clear()
        For Each s As String In m_blockedSitesDraft
            My.Settings.BlockedSites.Add(s)
        Next



        ' Advanced
        My.Settings.CustomDnsServer = txtCustomDns.Text.Trim()
        My.Settings.CustomProxyEnabled = chkEnableProxy.Checked
        My.Settings.CustomProxyHost = txtProxyHost.Text.Trim()
        Dim port As Integer = 8080
        If Integer.TryParse(txtProxyPort.Text.Trim(), port) Then
            If port < 1 OrElse port > 65535 Then port = 8080
        Else
            port = 8080
        End If
        My.Settings.CustomProxyPort = port

        My.Settings.Save()
        SettingsManager.NotifySettingsChanged()
    End Sub

    Private Async Sub btnCreateBackup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCreateBackup.Click
        Using sfd As New SaveFileDialog()
            sfd.Title = "Save Browser Backup"
            sfd.Filter = "ZIP Archive (*.zip)|*.zip"
            sfd.FileName = BackupService.GetDefaultBackupFileName()
            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    btnCreateBackup.Enabled = False
                    Dim backupTime As DateTime = Await _backupService.CreateBackupAsync(sfd.FileName)
                    lblLastBackup.Text = "Last backup: " & backupTime.ToString("dd MMM yyyy HH:mm")
                    MessageBox.Show("Backup completed successfully.", "Backup Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Backup failed: " & ex.Message, "Backup Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    btnCreateBackup.Enabled = True
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnRestoreBackup_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRestoreBackup.Click
        Using ofd As New OpenFileDialog()
            ofd.Title = "Select Browser Backup File"
            ofd.Filter = "ZIP Archive (*.zip)|*.zip"
            If ofd.ShowDialog() = DialogResult.OK Then
                If Not _restoreService.ValidateBackupArchive(ofd.FileName) Then
                    MessageBox.Show("The selected file is not a valid browser backup archive.", "Invalid Backup Archive", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If

                Dim confirm As DialogResult = MessageBox.Show("Restoring browser settings will overwrite your current configuration." & vbCrLf & vbCrLf & "Continue?", "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
                If confirm <> DialogResult.Yes Then Return

                Try
                    btnRestoreBackup.Enabled = False
                    Await _restoreService.RestoreBackupAsync(ofd.FileName)

                    Dim restart As DialogResult = MessageBox.Show("Browser restart required." & vbCrLf & vbCrLf & "Restart now?", "Restart Required", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If restart = DialogResult.Yes Then
                        Application.Restart()
                        Environment.Exit(0)
                    Else
                        LoadAllSettings()
                        Await LoadPrivacyTabSettingsAsync()
                    End If
                Catch ex As Exception
                    MessageBox.Show("Restore failed: " & ex.Message, "Restore Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Finally
                    btnRestoreBackup.Enabled = True
                End Try
            End If
        End Using
    End Sub

    Private Async Sub btnResetSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnResetSettings.Click
        Dim confirm As DialogResult = MessageBox.Show("Restore all browser settings to their default values?" & vbCrLf & vbCrLf & "Bookmarks and downloads will not be deleted.", "Confirm Reset Settings", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirm <> DialogResult.Yes Then Return

        Try
            btnResetSettings.Enabled = False
            Await _restoreService.ResetSettingsAsync()
            LoadAllSettings()
            Await LoadPrivacyTabSettingsAsync()
            MessageBox.Show("All browser settings have been reset to default values.", "Settings Reset", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Reset settings failed: " & ex.Message, "Reset Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Finally
            btnResetSettings.Enabled = True
        End Try
    End Sub

    Private Async Function RefreshCookiesGridAsync() As Task
        If _activeWebView Is Nothing OrElse _activeWebView.CoreWebView2 Is Nothing Then
            dgvCookies.Rows.Clear()
            _loadedCookies.Clear()
            lblCookieCount.Text = "Current cookie count: 0"
            Return
        End If

        Try
            btnRefreshCookies.Enabled = False
            _loadedCookies = Await _cookieService.GetCookiesAsync(_activeWebView)
            PopulateCookiesGrid()
        Catch ex As Exception
            MessageBox.Show("Failed to retrieve cookies: " & ex.Message, "Cookie Retrieval Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            dgvCookies.Rows.Clear()
            lblCookieCount.Text = "Current cookie count: 0"
        Finally
            btnRefreshCookies.Enabled = True
        End Try
    End Function

    Private Sub PopulateCookiesGrid()
        dgvCookies.Rows.Clear()

        Dim query As String = If(txtCookieSearch.Text = SearchPlaceholder, "", txtCookieSearch.Text)
        Dim filteredList As List(Of CookieItem) = _cookieService.FilterCookies(_loadedCookies, query)

        For Each cookie In filteredList
            Dim rowIndex As Integer = dgvCookies.Rows.Add(
                cookie.Website,
                cookie.Name,
                cookie.Domain,
                cookie.Path,
                cookie.DisplayExpires,
                cookie.IsSecure,
                cookie.IsHttpOnly,
                cookie.SameSite
            )
            dgvCookies.Rows(rowIndex).Tag = cookie
        Next

        lblCookieCount.Text = "Current cookie count: " & filteredList.Count
    End Sub

    Private Sub txtCookieSearch_Enter(ByVal sender As Object, ByVal e As EventArgs) Handles txtCookieSearch.Enter
        If txtCookieSearch.Text = SearchPlaceholder Then
            txtCookieSearch.Text = ""
            txtCookieSearch.ForeColor = System.Drawing.Color.Black
        End If
    End Sub

    Private Sub txtCookieSearch_Leave(ByVal sender As Object, ByVal e As EventArgs) Handles txtCookieSearch.Leave
        If String.IsNullOrWhiteSpace(txtCookieSearch.Text) Then
            txtCookieSearch.Text = SearchPlaceholder
            txtCookieSearch.ForeColor = System.Drawing.Color.Gray
        End If
    End Sub

    Private Sub txtCookieSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCookieSearch.TextChanged
        If txtCookieSearch.Text <> SearchPlaceholder Then
            PopulateCookiesGrid()
        End If
    End Sub

    Private Async Sub btnRefreshCookies_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnRefreshCookies.Click
        Await RefreshCookiesGridAsync()
    End Sub

    Private Async Sub btnDeleteSelectedCookie_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteSelectedCookie.Click
        If dgvCookies.SelectedRows.Count = 0 Then
            MessageBox.Show("Please select a cookie from the table to delete.", "Delete Cookie", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedRow As DataGridViewRow = dgvCookies.SelectedRows(0)
        Dim cookieItem As CookieItem = TryCast(selectedRow.Tag, CookieItem)
        If cookieItem Is Nothing Then Return

        Dim confirmResult As DialogResult = MessageBox.Show("Delete this cookie?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        If confirmResult = DialogResult.Yes Then
            Try
                If _activeWebView IsNot Nothing AndAlso _activeWebView.CoreWebView2 IsNot Nothing Then
                    Dim deleted As Boolean = Await _cookieService.DeleteCookieAsync(_activeWebView, cookieItem)
                    If deleted Then
                        _loadedCookies.Remove(cookieItem)
                        PopulateCookiesGrid()
                    Else
                        MessageBox.Show("Could not locate the specified cookie in WebView2 store.", "Delete Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    End If
                Else
                    MessageBox.Show("WebView2 is not initialized.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("Error deleting cookie: " & ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnDeleteAllCookies_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDeleteAllCookies.Click
        Dim confirmResult As DialogResult = MessageBox.Show("Delete ALL stored cookies?" & vbCrLf & vbCrLf & "This action cannot be undone.", "Confirm Delete All", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
        If confirmResult = DialogResult.Yes Then
            Try
                If _activeWebView IsNot Nothing AndAlso _activeWebView.CoreWebView2 IsNot Nothing Then
                    _cookieService.DeleteAllCookies(_activeWebView)
                    _loadedCookies.Clear()
                    PopulateCookiesGrid()
                    MessageBox.Show("All cookies have been deleted successfully.", "Cookies Cleared", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    MessageBox.Show("WebView2 is not initialized.", "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Catch ex As Exception
                MessageBox.Show("Error deleting all cookies: " & ex.Message, "Delete Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Async Sub btnExportCookies_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnExportCookies.Click
        Dim query As String = If(txtCookieSearch.Text = SearchPlaceholder, "", txtCookieSearch.Text)
        Dim visibleCookies As List(Of CookieItem) = _cookieService.FilterCookies(_loadedCookies, query)

        If visibleCookies.Count = 0 Then
            MessageBox.Show("There are no cookies available to export.", "Export Cookies", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Using sfd As New SaveFileDialog()
            sfd.Title = "Export Cookies to CSV"
            sfd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
            sfd.FileName = "cookies.csv"

            If sfd.ShowDialog() = DialogResult.OK Then
                Try
                    Await _cookieExportService.ExportToCsvAsync(visibleCookies, sfd.FileName)
                    MessageBox.Show($"Successfully exported {visibleCookies.Count} cookies to " & Path.GetFileName(sfd.FileName), "Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show("Failed to export cookies: " & ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Using
    End Sub

    Private Sub dgvCookies_CellDoubleClick(ByVal sender As Object, ByVal e As DataGridViewCellEventArgs) Handles dgvCookies.CellDoubleClick
        If e.RowIndex < 0 OrElse e.RowIndex >= dgvCookies.Rows.Count Then Return

        Dim row As DataGridViewRow = dgvCookies.Rows(e.RowIndex)
        Dim cookieItem As CookieItem = TryCast(row.Tag, CookieItem)
        If cookieItem IsNot Nothing AndAlso Not String.IsNullOrWhiteSpace(cookieItem.Website) Then
            Dim targetUrl As String = cookieItem.Website.Trim()
            If Not targetUrl.StartsWith("http://", StringComparison.OrdinalIgnoreCase) AndAlso Not targetUrl.StartsWith("https://", StringComparison.OrdinalIgnoreCase) Then
                targetUrl = "https://" & targetUrl
            End If

            Try
                If Form1.ActiveForm IsNot Nothing AndAlso TypeOf Form1.ActiveForm Is Form1 Then
                    DirectCast(Form1.ActiveForm, Form1).NavigateActiveTab(targetUrl)
                    Me.Close()
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error opening website from Cookie Manager: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub LoadJsDisabledSites()
        lbJsDisabled.Items.Clear()
        For Each s As String In m_jsDisabledDraft
            lbJsDisabled.Items.Add(s)
        Next
    End Sub

    Private Sub LoadBlockedSites()
        lbBlocked.Items.Clear()
        For Each s As String In m_blockedSitesDraft
            lbBlocked.Items.Add(s)
        Next
    End Sub

    Private Sub btnAddJsDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddJsDomain.Click
        If Not String.IsNullOrWhiteSpace(txtJsDomain.Text) Then
            Dim domain As String = txtJsDomain.Text.Trim().ToLower()
            If Not m_jsDisabledDraft.Contains(domain) Then
                m_jsDisabledDraft.Add(domain)
                LoadJsDisabledSites()
            End If
            txtJsDomain.Text = String.Empty
        End If
    End Sub

    Private Sub btnRemoveJsDomain_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveJsDomain.Click
        If lbJsDisabled.SelectedItem IsNot Nothing Then
            m_jsDisabledDraft.Remove(lbJsDisabled.SelectedItem.ToString())
            LoadJsDisabledSites()
        End If
    End Sub

    Private Sub btnAddBlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBlock.Click
        If Not String.IsNullOrWhiteSpace(txtBlock.Text) Then
            Dim urlToAdd As String = AppManager.FixURL(txtBlock.Text)
            If Not m_blockedSitesDraft.Contains(urlToAdd) Then
                m_blockedSitesDraft.Add(urlToAdd)
                LoadBlockedSites()
            End If
            txtBlock.Text = String.Empty
        End If
    End Sub

    Private Sub btnRemoveBlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveBlock.Click
        If lbBlocked.SelectedItem IsNot Nothing Then
            m_blockedSitesDraft.Remove(lbBlocked.SelectedItem.ToString())
            LoadBlockedSites()
        End If
    End Sub



    Private Sub btnBrowseDownloads_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBrowseDownloads.Click
        Using fbd As New FolderBrowserDialog()
            fbd.Description = "Select Default Downloads Folder"
            If fbd.ShowDialog() = DialogResult.OK Then
                txtDownloads.Text = fbd.SelectedPath
            End If
        End Using
    End Sub

    Private Sub btnDefaultHomePage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDefaultHomePage.Click
        txtHomePage.Text = "https://www.google.com"
    End Sub

    Private Sub btnConfigureProxy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfigureProxy.Click
        Form3.ShowDialog()
    End Sub

    Private Async Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Await SaveAllSettingsAsync()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Async Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        Await SaveAllSettingsAsync()
        MessageBox.Show("Settings applied successfully!", "Browser Settings", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub btnConfigureAdBlocker_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnConfigureAdBlocker.Click
        AdBlockerSettings.ShowDialog()
        chkAdBlocker.Checked = My.Settings.AdBlockerEnabled
    End Sub

End Class

Namespace My
    Partial Friend Class MySettings

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("https://www.google.com")> _
        Public Property HomePageUrl() As String
            Get
                Return If(CStr(Me("HomePageUrl")), "https://www.google.com")
            End Get
            Set(ByVal value As String)
                Me("HomePageUrl") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("0")> _
        Public Property StartupBehavior() As Integer
            Get
                Return CInt(Me("StartupBehavior"))
            End Get
            Set(ByVal value As Integer)
                Me("StartupBehavior") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("0")> _
        Public Property NewTabPage() As Integer
            Get
                Return CInt(Me("NewTabPage"))
            End Get
            Set(ByVal value As Integer)
                Me("NewTabPage") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("Google")> _
        Public Property SearchEngine() As String
            Get
                Return If(CStr(Me("SearchEngine")), "Google")
            End Get
            Set(ByVal value As String)
                Me("SearchEngine") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")> _
        Public Property DownloadsFolder() As String
            Get
                Return CStr(Me("DownloadsFolder"))
            End Get
            Set(ByVal value As String)
                Me("DownloadsFolder") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("1")> _
        Public Property FontSize() As Integer
            Get
                Return CInt(Me("FontSize"))
            End Get
            Set(ByVal value As Integer)
                Me("FontSize") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("False")> _
        Public Property FullScreenOnStartup() As Boolean
            Get
                Return CBool(Me("FullScreenOnStartup"))
            End Get
            Set(ByVal value As Boolean)
                Me("FullScreenOnStartup") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property PermissionCamera() As Boolean
            Get
                Return CBool(Me("PermissionCamera"))
            End Get
            Set(ByVal value As Boolean)
                Me("PermissionCamera") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property PermissionMic() As Boolean
            Get
                Return CBool(Me("PermissionMic"))
            End Get
            Set(ByVal value As Boolean)
                Me("PermissionMic") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property PermissionLocation() As Boolean
            Get
                Return CBool(Me("PermissionLocation"))
            End Get
            Set(ByVal value As Boolean)
                Me("PermissionLocation") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property PermissionNotifications() As Boolean
            Get
                Return CBool(Me("PermissionNotifications"))
            End Get
            Set(ByVal value As Boolean)
                Me("PermissionNotifications") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("False")> _
        Public Property IncognitoEnabled() As Boolean
            Get
                Return CBool(Me("IncognitoEnabled"))
            End Get
            Set(ByVal value As Boolean)
                Me("IncognitoEnabled") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("False")> _
        Public Property HttpsOnlyMode() As Boolean
            Get
                Return CBool(Me("HttpsOnlyMode"))
            End Get
            Set(ByVal value As Boolean)
                Me("HttpsOnlyMode") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property AdBlockerEnabled() As Boolean
            Get
                Return CBool(Me("AdBlockerEnabled"))
            End Get
            Set(ByVal value As Boolean)
                Me("AdBlockerEnabled") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property MemorySaverEnabled() As Boolean
            Get
                Return CBool(Me("MemorySaverEnabled"))
            End Get
            Set(ByVal value As Boolean)
                Me("MemorySaverEnabled") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property HardwareAcceleration() As Boolean
            Get
                Return CBool(Me("HardwareAcceleration"))
            End Get
            Set(ByVal value As Boolean)
                Me("HardwareAcceleration") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("0")> _
        Public Property DnsOverHttpsProvider() As Integer
            Get
                Return CInt(Me("DnsOverHttpsProvider"))
            End Get
            Set(ByVal value As Integer)
                Me("DnsOverHttpsProvider") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")> _
        Public Property CustomDnsServer() As String
            Get
                Return CStr(Me("CustomDnsServer"))
            End Get
            Set(ByVal value As String)
                Me("CustomDnsServer") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("False")> _
        Public Property CustomProxyEnabled() As Boolean
            Get
                Return CBool(Me("CustomProxyEnabled"))
            End Get
            Set(ByVal value As Boolean)
                Me("CustomProxyEnabled") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")> _
        Public Property CustomProxyHost() As String
            Get
                Return CStr(Me("CustomProxyHost"))
            End Get
            Set(ByVal value As String)
                Me("CustomProxyHost") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("8080")> _
        Public Property CustomProxyPort() As Integer
            Get
                Return CInt(Me("CustomProxyPort"))
            End Get
            Set(ByVal value As Integer)
                Me("CustomProxyPort") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute()> _
        Public Property JsDisabledSites() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("JsDisabledSites"), Global.System.Collections.Specialized.StringCollection)
            End Get
            Set(ByVal value As Global.System.Collections.Specialized.StringCollection)
                Me("JsDisabledSites") = value
            End Set
        End Property

    End Class
End Namespace
