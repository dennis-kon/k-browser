Imports System.Collections.Generic
Imports System.IO
Imports System.Windows.Forms

Public Class Settings

    Private m_jsDisabledDraft As New List(Of String)()
    Private m_blockedSitesDraft As New List(Of String)()
    Private m_isSyncingDns As Boolean = False

    Private Sub Settings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
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
    End Sub

    Private Sub LoadAllSettings()
        ' Browser Settings
        txtHomePage.Text = If(String.IsNullOrEmpty(My.Settings.HomePageUrl), "https://www.google.com", My.Settings.HomePageUrl)
        Select Case My.Settings.StartupBehavior
            Case 1 : rbStartupBlank.Checked = True
            Case 2 : rbStartupRestore.Checked = True
            Case 3 : rbStartupSpecific.Checked = True
            Case Else : rbStartupHome.Checked = True
        End Select
        cmbNewTab.SelectedIndex = Math.Max(0, Math.Min(2, My.Settings.NewTabPage))
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

        ' Performance
        chkMemorySaver.Checked = My.Settings.MemorySaverEnabled
        chkHardwareAccel.Checked = My.Settings.HardwareAcceleration
        cmbDoH.SelectedIndex = Math.Max(0, Math.Min(3, My.Settings.DnsOverHttpsProvider))

        m_isSyncingDns = True
        txtDoHCustom.Text = If(My.Settings.CustomDnsServer IsNot Nothing, My.Settings.CustomDnsServer, "")
        txtCustomDns.Text = txtDoHCustom.Text
        m_isSyncingDns = False

        ' Advanced
        chkEnableProxy.Checked = My.Settings.CustomProxyEnabled
        txtProxyHost.Text = My.Settings.CustomProxyHost
        txtProxyPort.Text = My.Settings.CustomProxyPort.ToString()
    End Sub

    Private Sub SaveAllSettings()
        ' Browser Settings
        My.Settings.HomePageUrl = txtHomePage.Text.Trim()
        If rbStartupBlank.Checked Then
            My.Settings.StartupBehavior = 1
        ElseIf rbStartupRestore.Checked Then
            My.Settings.StartupBehavior = 2
        ElseIf rbStartupSpecific.Checked Then
            My.Settings.StartupBehavior = 3
        Else
            My.Settings.StartupBehavior = 0
        End If
        My.Settings.NewTabPage = cmbNewTab.SelectedIndex
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

        ' Performance
        My.Settings.MemorySaverEnabled = chkMemorySaver.Checked
        My.Settings.HardwareAcceleration = chkHardwareAccel.Checked
        My.Settings.DnsOverHttpsProvider = cmbDoH.SelectedIndex

        ' Advanced
        My.Settings.CustomDnsServer = If(txtCustomDns.Text.Trim() <> "", txtCustomDns.Text.Trim(), txtDoHCustom.Text.Trim())
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

    Private Sub txtDoHCustom_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtDoHCustom.TextChanged
        If Not m_isSyncingDns Then
            m_isSyncingDns = True
            txtCustomDns.Text = txtDoHCustom.Text
            m_isSyncingDns = False
        End If
    End Sub

    Private Sub txtCustomDns_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtCustomDns.TextChanged
        If Not m_isSyncingDns Then
            m_isSyncingDns = True
            txtDoHCustom.Text = txtCustomDns.Text
            m_isSyncingDns = False
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

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        SaveAllSettings()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnApply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnApply.Click
        SaveAllSettings()
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
