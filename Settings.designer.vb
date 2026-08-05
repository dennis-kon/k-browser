<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Settings
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        TabControl1 = New TabControl()
        tpBrowser = New TabPage()
        lblHomePage = New Label()
        txtHomePage = New TextBox()
        btnDefaultHomePage = New Button()
        lblSearchEngine = New Label()
        cmbSearchEngine = New ComboBox()
        lblDownloads = New Label()
        txtDownloads = New TextBox()
        btnBrowseDownloads = New Button()
        lblFontSize = New Label()
        cmbFontSize = New ComboBox()
        chkFullScreen = New CheckBox()
        grpStartupSection = New GroupBox()
        rbContinueWhereLeftOff = New RadioButton()
        lblContinueWhereLeftOffDesc = New Label()
        rbOpenNewTabPage = New RadioButton()
        lblOpenNewTabPageDesc = New Label()
        grpBackupRestore = New GroupBox()
        btnCreateBackup = New Button()
        btnRestoreBackup = New Button()
        btnResetSettings = New Button()
        lblLastBackup = New Label()
        ToolTip1 = New ToolTip()
        grpGraphics = New GroupBox()
        chkUseHardwareAcceleration = New CheckBox()
        lblGraphicsDesc = New Label()
        lblGraphicsStatus = New Label()
        grpTabs = New GroupBox()
        chkFadeInactiveTabs = New CheckBox()
        lblTabsDesc = New Label()
        lblTabsStatus = New Label()
        grpPageLoading = New GroupBox()
        chkPreloadPages = New CheckBox()
        lblPageLoadingDesc = New Label()
        lblPageLoadingInfo = New Label()
        lblPageLoadingStatus = New Label()
        tpPrivacyTab = New TabPage()
        grpCookies = New GroupBox()
        chkBlockThirdPartyCookies = New CheckBox()
        lblBlockThirdPartyDesc = New Label()
        grpCookieManager = New GroupBox()
        lblCookieCount = New Label()
        dgvCookies = New DataGridView()
        colWebsite = New DataGridViewTextBoxColumn()
        colCookieName = New DataGridViewTextBoxColumn()
        colDomain = New DataGridViewTextBoxColumn()
        colPath = New DataGridViewTextBoxColumn()
        colExpires = New DataGridViewTextBoxColumn()
        colSecure = New DataGridViewTextBoxColumn()
        colHttpOnly = New DataGridViewTextBoxColumn()
        colSameSite = New DataGridViewTextBoxColumn()
        btnExportCookies = New Button()
        btnDeleteAllCookies = New Button()
        btnDeleteSelectedCookie = New Button()
        btnRefreshCookies = New Button()
        txtCookieSearch = New TextBox()
        tpPasswords = New TabPage()
        grpSavedPasswords = New GroupBox()
        txtPasswordSearch = New TextBox()
        lblPasswordCount = New Label()
        dgvPasswords = New DataGridView()
        colPassWebsite = New DataGridViewTextBoxColumn()
        colPassUsername = New DataGridViewTextBoxColumn()
        colPassPassword = New DataGridViewTextBoxColumn()
        colPassCreated = New DataGridViewTextBoxColumn()
        colPassLastUsed = New DataGridViewTextBoxColumn()
        btnDeletePassword = New Button()
        btnDeleteAllPasswords = New Button()
        btnExportPasswords = New Button()
        btnImportPasswords = New Button()
        grpPasswordPreferences = New GroupBox()
        chkSavePasswords = New CheckBox()
        chkAutoFill = New CheckBox()
        chkRequireAuth = New CheckBox()
        tpPrivacy = New TabPage()
        grpPermissions = New GroupBox()
        chkPermCamera = New CheckBox()
        chkPermMic = New CheckBox()
        chkPermLocation = New CheckBox()
        chkPermNotifications = New CheckBox()
        chkIncognito = New CheckBox()
        chkHttpsOnly = New CheckBox()
        chkAdBlocker = New CheckBox()
        btnConfigureAdBlocker = New Button()
        chkPhishing = New CheckBox()
        chkAllowPop = New CheckBox()
        grpJsSites = New GroupBox()
        lbJsDisabled = New ListBox()
        txtJsDomain = New TextBox()
        btnAddJsDomain = New Button()
        btnRemoveJsDomain = New Button()
        grpBlocked = New GroupBox()
        lbBlocked = New ListBox()
        txtBlock = New TextBox()
        btnAddBlock = New Button()
        btnRemoveBlock = New Button()
        tpPerformance = New TabPage()
        grpGraphics = New GroupBox()
        chkUseHardwareAcceleration = New CheckBox()
        lblGraphicsDesc = New Label()
        lblGraphicsStatus = New Label()
        grpTabs = New GroupBox()
        chkFadeInactiveTabs = New CheckBox()
        lblTabsDesc = New Label()
        lblTabsStatus = New Label()
        grpPageLoading = New GroupBox()
        chkPreloadPages = New CheckBox()
        lblPageLoadingDesc = New Label()
        lblPageLoadingInfo = New Label()
        lblPageLoadingStatus = New Label()
        tpAdvanced = New TabPage()
        grpDns = New GroupBox()
        lblCustomDns = New Label()
        txtCustomDns = New TextBox()
        grpProxy = New GroupBox()
        chkEnableProxy = New CheckBox()
        lblProxyHost = New Label()
        txtProxyHost = New TextBox()
        lblProxyPort = New Label()
        txtProxyPort = New TextBox()
        btnConfigureProxy = New Button()
        Panel1 = New Panel()
        btnOK = New Button()
        btnCancel = New Button()
        btnApply = New Button()
        TabControl1.SuspendLayout()
        tpBrowser.SuspendLayout()
        grpStartupSection.SuspendLayout()
        grpBackupRestore.SuspendLayout()
        grpGraphics.SuspendLayout()
        grpTabs.SuspendLayout()
        grpPageLoading.SuspendLayout()
        tpPrivacyTab.SuspendLayout()
        grpCookies.SuspendLayout()
        grpCookieManager.SuspendLayout()
        CType(dgvCookies, ComponentModel.ISupportInitialize).BeginInit()
        tpPasswords.SuspendLayout()
        grpSavedPasswords.SuspendLayout()
        grpPasswordPreferences.SuspendLayout()
        CType(dgvPasswords, ComponentModel.ISupportInitialize).BeginInit()
        tpPrivacy.SuspendLayout()
        grpPermissions.SuspendLayout()
        grpJsSites.SuspendLayout()
        grpBlocked.SuspendLayout()
        tpPerformance.SuspendLayout()
        grpGraphics.SuspendLayout()
        grpTabs.SuspendLayout()
        grpPageLoading.SuspendLayout()
        tpAdvanced.SuspendLayout()
        grpDns.SuspendLayout()
        grpProxy.SuspendLayout()
        Panel1.SuspendLayout()
        SuspendLayout()
        ' 
        ' TabControl1
        ' 
        TabControl1.Controls.Add(tpBrowser)
        TabControl1.Controls.Add(tpPrivacyTab)
        TabControl1.Controls.Add(tpPasswords)
        TabControl1.Controls.Add(tpPrivacy)
        TabControl1.Controls.Add(tpPerformance)
        TabControl1.Controls.Add(tpAdvanced)
        TabControl1.Dock = DockStyle.Fill
        TabControl1.Location = New Point(0, 0)
        TabControl1.Name = "TabControl1"
        TabControl1.SelectedIndex = 0
        TabControl1.Size = New Size(609, 478)
        TabControl1.TabIndex = 0
        ' 
        ' tpBrowser
        ' 
        tpBrowser.Controls.Add(lblHomePage)
        tpBrowser.Controls.Add(txtHomePage)
        tpBrowser.Controls.Add(btnDefaultHomePage)
        tpBrowser.Controls.Add(lblSearchEngine)
        tpBrowser.Controls.Add(cmbSearchEngine)
        tpBrowser.Controls.Add(lblDownloads)
        tpBrowser.Controls.Add(txtDownloads)
        tpBrowser.Controls.Add(btnBrowseDownloads)
        tpBrowser.Controls.Add(grpStartupSection)
        tpBrowser.Controls.Add(grpBackupRestore)
        tpBrowser.Controls.Add(lblFontSize)
        tpBrowser.Controls.Add(cmbFontSize)
        tpBrowser.Controls.Add(chkFullScreen)
        tpBrowser.Location = New Point(4, 22)
        tpBrowser.Name = "tpBrowser"
        tpBrowser.Padding = New Padding(10)
        tpBrowser.Size = New Size(601, 452)
        tpBrowser.TabIndex = 0
        tpBrowser.Text = "Browser Settings"
        tpBrowser.UseVisualStyleBackColor = True
        ' 
        ' lblHomePage
        ' 
        lblHomePage.AutoSize = True
        lblHomePage.Location = New Point(13, 15)
        lblHomePage.Name = "lblHomePage"
        lblHomePage.Size = New Size(87, 13)
        lblHomePage.TabIndex = 0
        lblHomePage.Text = "Home Page URL:"
        ' 
        ' txtHomePage
        ' 
        txtHomePage.Location = New Point(135, 12)
        txtHomePage.Name = "txtHomePage"
        txtHomePage.Size = New Size(280, 21)
        txtHomePage.TabIndex = 1
        ' 
        ' btnDefaultHomePage
        ' 
        btnDefaultHomePage.Location = New Point(425, 10)
        btnDefaultHomePage.Name = "btnDefaultHomePage"
        btnDefaultHomePage.Size = New Size(110, 25)
        btnDefaultHomePage.TabIndex = 2
        btnDefaultHomePage.Text = "Use Default"
        btnDefaultHomePage.UseVisualStyleBackColor = True
        ' 
        ' lblSearchEngine
        ' 
        lblSearchEngine.AutoSize = True
        lblSearchEngine.Location = New Point(13, 45)
        lblSearchEngine.Name = "lblSearchEngine"
        lblSearchEngine.Size = New Size(117, 13)
        lblSearchEngine.TabIndex = 3
        lblSearchEngine.Text = "Default Search Engine:"
        ' 
        ' cmbSearchEngine
        ' 
        cmbSearchEngine.DropDownStyle = ComboBoxStyle.DropDownList
        cmbSearchEngine.FormattingEnabled = True
        cmbSearchEngine.Items.AddRange(New Object() {"Google", "Bing", "DuckDuckGo", "Yahoo"})
        cmbSearchEngine.Location = New Point(135, 42)
        cmbSearchEngine.Name = "cmbSearchEngine"
        cmbSearchEngine.Size = New Size(280, 21)
        cmbSearchEngine.TabIndex = 3
        ' 
        ' lblDownloads
        ' 
        lblDownloads.AutoSize = True
        lblDownloads.Location = New Point(13, 75)
        lblDownloads.Name = "lblDownloads"
        lblDownloads.Size = New Size(96, 13)
        lblDownloads.TabIndex = 4
        lblDownloads.Text = "Downloads Folder:"
        ' 
        ' txtDownloads
        ' 
        txtDownloads.Location = New Point(135, 72)
        txtDownloads.Name = "txtDownloads"
        txtDownloads.Size = New Size(280, 21)
        txtDownloads.TabIndex = 4
        ' 
        ' btnBrowseDownloads
        ' 
        btnBrowseDownloads.Location = New Point(425, 70)
        btnBrowseDownloads.Name = "btnBrowseDownloads"
        btnBrowseDownloads.Size = New Size(110, 25)
        btnBrowseDownloads.TabIndex = 5
        btnBrowseDownloads.Text = "Browse..."
        btnBrowseDownloads.UseVisualStyleBackColor = True
        ' 
        ' grpStartupSection
        ' 
        grpStartupSection.Controls.Add(rbContinueWhereLeftOff)
        grpStartupSection.Controls.Add(lblContinueWhereLeftOffDesc)
        grpStartupSection.Controls.Add(rbOpenNewTabPage)
        grpStartupSection.Controls.Add(lblOpenNewTabPageDesc)
        grpStartupSection.Location = New Point(13, 100)
        grpStartupSection.Name = "grpStartupSection"
        grpStartupSection.Size = New Size(530, 110)
        grpStartupSection.TabIndex = 6
        grpStartupSection.TabStop = False
        grpStartupSection.Text = "Startup"
        ' 
        ' rbContinueWhereLeftOff
        ' 
        rbContinueWhereLeftOff.AutoSize = True
        rbContinueWhereLeftOff.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        rbContinueWhereLeftOff.Location = New Point(15, 20)
        rbContinueWhereLeftOff.Name = "rbContinueWhereLeftOff"
        rbContinueWhereLeftOff.Size = New Size(175, 17)
        rbContinueWhereLeftOff.TabIndex = 0
        rbContinueWhereLeftOff.Text = "Continue where you left off"
        rbContinueWhereLeftOff.UseVisualStyleBackColor = True
        ' 
        ' lblContinueWhereLeftOffDesc
        ' 
        lblContinueWhereLeftOffDesc.AutoSize = True
        lblContinueWhereLeftOffDesc.ForeColor = Color.DimGray
        lblContinueWhereLeftOffDesc.Location = New Point(34, 38)
        lblContinueWhereLeftOffDesc.Name = "lblContinueWhereLeftOffDesc"
        lblContinueWhereLeftOffDesc.Size = New Size(375, 13)
        lblContinueWhereLeftOffDesc.TabIndex = 1
        lblContinueWhereLeftOffDesc.Text = "Restore all tabs from the previous browsing session when the browser starts."
        ' 
        ' rbOpenNewTabPage
        ' 
        rbOpenNewTabPage.AutoSize = True
        rbOpenNewTabPage.Checked = True
        rbOpenNewTabPage.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        rbOpenNewTabPage.Location = New Point(15, 60)
        rbOpenNewTabPage.Name = "rbOpenNewTabPage"
        rbOpenNewTabPage.Size = New Size(149, 17)
        rbOpenNewTabPage.TabIndex = 2
        rbOpenNewTabPage.TabStop = True
        rbOpenNewTabPage.Text = "Open the New Tab page"
        rbOpenNewTabPage.UseVisualStyleBackColor = True
        ' 
        ' lblOpenNewTabPageDesc
        ' 
        lblOpenNewTabPageDesc.AutoSize = True
        lblOpenNewTabPageDesc.ForeColor = Color.DimGray
        lblOpenNewTabPageDesc.Location = New Point(34, 78)
        lblOpenNewTabPageDesc.Name = "lblOpenNewTabPageDesc"
        lblOpenNewTabPageDesc.Size = New Size(160, 13)
        lblOpenNewTabPageDesc.TabIndex = 3
        lblOpenNewTabPageDesc.Text = "Always start with a new blank tab."
        ' 
        ' grpBackupRestore
        ' 
        grpBackupRestore.Controls.Add(lblLastBackup)
        grpBackupRestore.Controls.Add(btnResetSettings)
        grpBackupRestore.Controls.Add(btnRestoreBackup)
        grpBackupRestore.Controls.Add(btnCreateBackup)
        grpBackupRestore.Location = New Point(13, 218)
        grpBackupRestore.Name = "grpBackupRestore"
        grpBackupRestore.Size = New Size(530, 115)
        grpBackupRestore.TabIndex = 7
        grpBackupRestore.TabStop = False
        grpBackupRestore.Text = "Backup & Restore"
        ' 
        ' btnCreateBackup
        ' 
        btnCreateBackup.Location = New Point(15, 25)
        btnCreateBackup.Name = "btnCreateBackup"
        btnCreateBackup.Size = New Size(150, 32)
        btnCreateBackup.TabIndex = 0
        btnCreateBackup.Text = "📦 Create Backup"
        ToolTip1.SetToolTip(btnCreateBackup, "Create a compressed (.zip) backup of your browser settings, session, bookmarks, and preferences.")
        btnCreateBackup.UseVisualStyleBackColor = True
        ' 
        ' btnRestoreBackup
        ' 
        btnRestoreBackup.Location = New Point(175, 25)
        btnRestoreBackup.Name = "btnRestoreBackup"
        btnRestoreBackup.Size = New Size(150, 32)
        btnRestoreBackup.TabIndex = 1
        btnRestoreBackup.Text = "🔄 Restore Backup"
        ToolTip1.SetToolTip(btnRestoreBackup, "Restore browser settings, session, bookmarks, and preferences from a backup ZIP file.")
        btnRestoreBackup.UseVisualStyleBackColor = True
        ' 
        ' btnResetSettings
        ' 
        btnResetSettings.Location = New Point(335, 25)
        btnResetSettings.Name = "btnResetSettings"
        btnResetSettings.Size = New Size(150, 32)
        btnResetSettings.TabIndex = 2
        btnResetSettings.Text = "⚠️ Reset Settings"
        ToolTip1.SetToolTip(btnResetSettings, "Restore all browser settings to default values. Bookmarks and downloaded files will not be deleted.")
        btnResetSettings.UseVisualStyleBackColor = True
        ' 
        ' lblLastBackup
        ' 
        lblLastBackup.AutoSize = True
        lblLastBackup.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblLastBackup.ForeColor = Color.DimGray
        lblLastBackup.Location = New Point(15, 72)
        lblLastBackup.Name = "lblLastBackup"
        lblLastBackup.Size = New Size(111, 13)
        lblLastBackup.TabIndex = 3
        lblLastBackup.Text = "Last backup: Never"
        ' 
        ' lblFontSize
        ' 
        lblFontSize.AutoSize = True
        lblFontSize.Location = New Point(13, 345)
        lblFontSize.Name = "lblFontSize"
        lblFontSize.Size = New Size(55, 13)
        lblFontSize.TabIndex = 8
        lblFontSize.Text = "Font Size:"
        ' 
        ' cmbFontSize
        ' 
        cmbFontSize.DropDownStyle = ComboBoxStyle.DropDownList
        cmbFontSize.FormattingEnabled = True
        cmbFontSize.Items.AddRange(New Object() {"Small", "Medium", "Large", "Extra Large"})
        cmbFontSize.Location = New Point(135, 342)
        cmbFontSize.Name = "cmbFontSize"
        cmbFontSize.Size = New Size(280, 21)
        cmbFontSize.TabIndex = 8
        ' 
        ' chkFullScreen
        ' 
        chkFullScreen.AutoSize = True
        chkFullScreen.Location = New Point(135, 375)
        ' 
        ' tpPrivacyTab
        ' 
        tpPrivacyTab.Controls.Add(grpCookies)
        tpPrivacyTab.Controls.Add(grpCookieManager)
        tpPrivacyTab.Location = New Point(4, 22)
        tpPrivacyTab.Name = "tpPrivacyTab"
        tpPrivacyTab.Padding = New Padding(10)
        tpPrivacyTab.Size = New Size(601, 452)
        tpPrivacyTab.TabIndex = 4
        tpPrivacyTab.Text = "Cookies"
        tpPrivacyTab.UseVisualStyleBackColor = True
        ' 
        ' grpCookies
        ' 
        grpCookies.Controls.Add(chkBlockThirdPartyCookies)
        grpCookies.Controls.Add(lblBlockThirdPartyDesc)
        grpCookies.Location = New Point(10, 8)
        grpCookies.Name = "grpCookies"
        grpCookies.Size = New Size(578, 75)
        grpCookies.TabIndex = 0
        grpCookies.TabStop = False
        grpCookies.Text = "Cookies"
        ' 
        ' chkBlockThirdPartyCookies
        ' 
        chkBlockThirdPartyCookies.AutoSize = True
        chkBlockThirdPartyCookies.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        chkBlockThirdPartyCookies.Location = New Point(15, 20)
        chkBlockThirdPartyCookies.Name = "chkBlockThirdPartyCookies"
        chkBlockThirdPartyCookies.Size = New Size(168, 17)
        chkBlockThirdPartyCookies.TabIndex = 0
        chkBlockThirdPartyCookies.Text = "Block third-party cookies"
        chkBlockThirdPartyCookies.UseVisualStyleBackColor = True
        ' 
        ' lblBlockThirdPartyDesc
        ' 
        lblBlockThirdPartyDesc.AutoSize = True
        lblBlockThirdPartyDesc.ForeColor = Color.DimGray
        lblBlockThirdPartyDesc.Location = New Point(34, 40)
        lblBlockThirdPartyDesc.Name = "lblBlockThirdPartyDesc"
        lblBlockThirdPartyDesc.Size = New Size(364, 26)
        lblBlockThirdPartyDesc.TabIndex = 1
        lblBlockThirdPartyDesc.Text = "Blocks cookies from websites other than the one you are currently visiting." & vbCrLf & "This may prevent advertisers from tracking your browsing activity."
        ' 
        ' grpCookieManager
        ' 
        grpCookieManager.Controls.Add(lblCookieCount)
        grpCookieManager.Controls.Add(dgvCookies)
        grpCookieManager.Controls.Add(btnExportCookies)
        grpCookieManager.Controls.Add(btnDeleteAllCookies)
        grpCookieManager.Controls.Add(btnDeleteSelectedCookie)
        grpCookieManager.Controls.Add(btnRefreshCookies)
        grpCookieManager.Controls.Add(txtCookieSearch)
        grpCookieManager.Location = New Point(10, 89)
        grpCookieManager.Name = "grpCookieManager"
        grpCookieManager.Size = New Size(578, 311)
        grpCookieManager.TabIndex = 1
        grpCookieManager.TabStop = False
        grpCookieManager.Text = "Cookie Manager"
        ' 
        ' lblCookieCount
        ' 
        lblCookieCount.AutoSize = True
        lblCookieCount.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblCookieCount.Location = New Point(12, 288)
        lblCookieCount.Name = "lblCookieCount"
        lblCookieCount.Size = New Size(138, 13)
        lblCookieCount.TabIndex = 6
        lblCookieCount.Text = "Current cookie count: 0"
        ' 
        ' dgvCookies
        ' 
        dgvCookies.AllowUserToAddRows = False
        dgvCookies.AllowUserToDeleteRows = False
        dgvCookies.AllowUserToResizeRows = False
        dgvCookies.BackgroundColor = Color.White
        dgvCookies.BorderStyle = BorderStyle.Fixed3D
        dgvCookies.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvCookies.Columns.AddRange(New DataGridViewColumn() {colWebsite, colCookieName, colDomain, colPath, colExpires, colSecure, colHttpOnly, colSameSite})
        dgvCookies.Location = New Point(12, 50)
        dgvCookies.MultiSelect = False
        dgvCookies.Name = "dgvCookies"
        dgvCookies.ReadOnly = True
        dgvCookies.RowHeadersVisible = False
        dgvCookies.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvCookies.Size = New Size(509, 232)
        dgvCookies.TabIndex = 5
        ' 
        ' colWebsite
        ' 
        colWebsite.HeaderText = "Website"
        colWebsite.Name = "colWebsite"
        colWebsite.ReadOnly = True
        colWebsite.Width = 110
        ' 
        ' colCookieName
        ' 
        colCookieName.HeaderText = "Cookie Name"
        colCookieName.Name = "colCookieName"
        colCookieName.ReadOnly = True
        colCookieName.Width = 110
        ' 
        ' colDomain
        ' 
        colDomain.HeaderText = "Domain"
        colDomain.Name = "colDomain"
        colDomain.ReadOnly = True
        ' 
        ' colPath
        ' 
        colPath.HeaderText = "Path"
        colPath.Name = "colPath"
        colPath.ReadOnly = True
        colPath.Width = 50
        ' 
        ' colExpires
        ' 
        colExpires.HeaderText = "Expires"
        colExpires.Name = "colExpires"
        colExpires.ReadOnly = True
        colExpires.Width = 110
        ' 
        ' colSecure
        ' 
        colSecure.HeaderText = "Secure"
        colSecure.Name = "colSecure"
        colSecure.ReadOnly = True
        colSecure.Width = 55
        ' 
        ' colHttpOnly
        ' 
        colHttpOnly.HeaderText = "HttpOnly"
        colHttpOnly.Name = "colHttpOnly"
        colHttpOnly.ReadOnly = True
        colHttpOnly.Width = 60
        ' 
        ' colSameSite
        ' 
        colSameSite.HeaderText = "SameSite"
        colSameSite.Name = "colSameSite"
        colSameSite.ReadOnly = True
        colSameSite.Width = 65
        ' 
        ' btnExportCookies
        ' 
        btnExportCookies.Location = New Point(429, 19)
        btnExportCookies.Name = "btnExportCookies"
        btnExportCookies.Size = New Size(92, 25)
        btnExportCookies.TabIndex = 4
        btnExportCookies.Text = "Export Cookies"
        btnExportCookies.UseVisualStyleBackColor = True
        ' 
        ' btnDeleteAllCookies
        ' 
        btnDeleteAllCookies.Location = New Point(315, 19)
        btnDeleteAllCookies.Name = "btnDeleteAllCookies"
        btnDeleteAllCookies.Size = New Size(108, 25)
        btnDeleteAllCookies.TabIndex = 3
        btnDeleteAllCookies.Text = "Delete All Cookies"
        btnDeleteAllCookies.UseVisualStyleBackColor = True
        ' 
        ' btnDeleteSelectedCookie
        ' 
        btnDeleteSelectedCookie.Location = New Point(214, 19)
        btnDeleteSelectedCookie.Name = "btnDeleteSelectedCookie"
        btnDeleteSelectedCookie.Size = New Size(95, 25)
        btnDeleteSelectedCookie.TabIndex = 2
        btnDeleteSelectedCookie.Text = "Delete Selected"
        btnDeleteSelectedCookie.UseVisualStyleBackColor = True
        ' 
        ' btnRefreshCookies
        ' 
        btnRefreshCookies.Location = New Point(143, 19)
        btnRefreshCookies.Name = "btnRefreshCookies"
        btnRefreshCookies.Size = New Size(65, 25)
        btnRefreshCookies.TabIndex = 1
        btnRefreshCookies.Text = "Refresh"
        btnRefreshCookies.UseVisualStyleBackColor = True
        ' 
        ' txtCookieSearch
        ' 
        txtCookieSearch.ForeColor = Color.Gray
        txtCookieSearch.Location = New Point(12, 21)
        txtCookieSearch.Name = "txtCookieSearch"
        txtCookieSearch.Size = New Size(125, 21)
        txtCookieSearch.TabIndex = 0
        txtCookieSearch.Text = "Search website..."
        ' 
        ' tpPasswords
        ' 
        tpPasswords.Controls.Add(grpSavedPasswords)
        tpPasswords.Controls.Add(grpPasswordPreferences)
        tpPasswords.Location = New Point(4, 22)
        tpPasswords.Name = "tpPasswords"
        tpPasswords.Padding = New Padding(10)
        tpPasswords.Size = New Size(601, 452)
        tpPasswords.TabIndex = 5
        tpPasswords.Text = "Passwords"
        tpPasswords.UseVisualStyleBackColor = True
        ' 
        ' grpSavedPasswords
        ' 
        grpSavedPasswords.Controls.Add(txtPasswordSearch)
        grpSavedPasswords.Controls.Add(lblPasswordCount)
        grpSavedPasswords.Controls.Add(dgvPasswords)
        grpSavedPasswords.Controls.Add(btnDeletePassword)
        grpSavedPasswords.Controls.Add(btnDeleteAllPasswords)
        grpSavedPasswords.Controls.Add(btnExportPasswords)
        grpSavedPasswords.Controls.Add(btnImportPasswords)
        grpSavedPasswords.Location = New Point(10, 8)
        grpSavedPasswords.Name = "grpSavedPasswords"
        grpSavedPasswords.Size = New Size(578, 280)
        grpSavedPasswords.TabIndex = 0
        grpSavedPasswords.TabStop = False
        grpSavedPasswords.Text = "Saved Passwords"
        ' 
        ' txtPasswordSearch
        ' 
        txtPasswordSearch.ForeColor = Color.Gray
        txtPasswordSearch.Location = New Point(12, 20)
        txtPasswordSearch.Name = "txtPasswordSearch"
        txtPasswordSearch.Size = New Size(180, 21)
        txtPasswordSearch.TabIndex = 0
        txtPasswordSearch.Text = "Search passwords..."
        ' 
        ' lblPasswordCount
        ' 
        lblPasswordCount.AutoSize = True
        lblPasswordCount.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPasswordCount.Location = New Point(205, 23)
        lblPasswordCount.Name = "lblPasswordCount"
        lblPasswordCount.Size = New Size(144, 13)
        lblPasswordCount.TabIndex = 1
        lblPasswordCount.Text = "Saved passwords: 0"
        ' 
        ' dgvPasswords
        ' 
        dgvPasswords.AllowUserToAddRows = False
        dgvPasswords.AllowUserToDeleteRows = False
        dgvPasswords.AllowUserToResizeRows = False
        dgvPasswords.BackgroundColor = Color.White
        dgvPasswords.BorderStyle = BorderStyle.Fixed3D
        dgvPasswords.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize
        dgvPasswords.Columns.AddRange(New DataGridViewColumn() {colPassWebsite, colPassUsername, colPassPassword, colPassCreated, colPassLastUsed})
        dgvPasswords.Location = New Point(12, 48)
        dgvPasswords.MultiSelect = False
        dgvPasswords.Name = "dgvPasswords"
        dgvPasswords.ReadOnly = True
        dgvPasswords.RowHeadersVisible = False
        dgvPasswords.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        dgvPasswords.Size = New Size(554, 190)
        dgvPasswords.TabIndex = 2
        ' 
        ' colPassWebsite
        ' 
        colPassWebsite.HeaderText = "Website"
        colPassWebsite.Name = "colPassWebsite"
        colPassWebsite.ReadOnly = True
        colPassWebsite.Width = 120
        ' 
        ' colPassUsername
        ' 
        colPassUsername.HeaderText = "Username"
        colPassUsername.Name = "colPassUsername"
        colPassUsername.ReadOnly = True
        colPassUsername.Width = 150
        ' 
        ' colPassPassword
        ' 
        colPassPassword.HeaderText = "Password"
        colPassPassword.Name = "colPassPassword"
        colPassPassword.ReadOnly = True
        colPassPassword.Width = 90
        ' 
        ' colPassCreated
        ' 
        colPassCreated.HeaderText = "Created Date"
        colPassCreated.Name = "colPassCreated"
        colPassCreated.ReadOnly = True
        colPassCreated.Width = 95
        ' 
        ' colPassLastUsed
        ' 
        colPassLastUsed.HeaderText = "Last Used"
        colPassLastUsed.Name = "colPassLastUsed"
        colPassLastUsed.ReadOnly = True
        colPassLastUsed.Width = 95
        ' 
        ' btnDeletePassword
        ' 
        btnDeletePassword.Location = New Point(12, 244)
        btnDeletePassword.Name = "btnDeletePassword"
        btnDeletePassword.Size = New Size(110, 25)
        btnDeletePassword.TabIndex = 3
        btnDeletePassword.Text = "Delete Selected"
        btnDeletePassword.UseVisualStyleBackColor = True
        ' 
        ' btnDeleteAllPasswords
        ' 
        btnDeleteAllPasswords.Location = New Point(128, 244)
        btnDeleteAllPasswords.Name = "btnDeleteAllPasswords"
        btnDeleteAllPasswords.Size = New Size(90, 25)
        btnDeleteAllPasswords.TabIndex = 4
        btnDeleteAllPasswords.Text = "Delete All"
        btnDeleteAllPasswords.UseVisualStyleBackColor = True
        ' 
        ' btnExportPasswords
        ' 
        btnExportPasswords.Location = New Point(350, 244)
        btnExportPasswords.Name = "btnExportPasswords"
        btnExportPasswords.Size = New Size(105, 25)
        btnExportPasswords.TabIndex = 5
        btnExportPasswords.Text = "Export Passwords"
        btnExportPasswords.UseVisualStyleBackColor = True
        ' 
        ' btnImportPasswords
        ' 
        btnImportPasswords.Location = New Point(461, 244)
        btnImportPasswords.Name = "btnImportPasswords"
        btnImportPasswords.Size = New Size(105, 25)
        btnImportPasswords.TabIndex = 6
        btnImportPasswords.Text = "Import Passwords"
        btnImportPasswords.UseVisualStyleBackColor = True
        ' 
        ' grpPasswordPreferences
        ' 
        grpPasswordPreferences.Controls.Add(chkSavePasswords)
        grpPasswordPreferences.Controls.Add(chkAutoFill)
        grpPasswordPreferences.Controls.Add(chkRequireAuth)
        grpPasswordPreferences.Location = New Point(10, 294)
        grpPasswordPreferences.Name = "grpPasswordPreferences"
        grpPasswordPreferences.Size = New Size(578, 110)
        grpPasswordPreferences.TabIndex = 1
        grpPasswordPreferences.TabStop = False
        grpPasswordPreferences.Text = "Password Saving Preferences"
        ' 
        ' chkSavePasswords
        ' 
        chkSavePasswords.AutoSize = True
        chkSavePasswords.Location = New Point(15, 22)
        chkSavePasswords.Name = "chkSavePasswords"
        chkSavePasswords.Size = New Size(143, 17)
        chkSavePasswords.TabIndex = 0
        chkSavePasswords.Text = "Offer to save passwords"
        chkSavePasswords.UseVisualStyleBackColor = True
        ' 
        ' chkAutoFill
        ' 
        chkAutoFill.AutoSize = True
        chkAutoFill.Location = New Point(15, 47)
        chkAutoFill.Name = "chkAutoFill"
        chkAutoFill.Size = New Size(135, 17)
        chkAutoFill.TabIndex = 1
        chkAutoFill.Text = "Enable password autofill"
        chkAutoFill.UseVisualStyleBackColor = True
        ' 
        ' chkRequireAuth
        ' 
        chkRequireAuth.AutoSize = True
        chkRequireAuth.Location = New Point(15, 72)
        chkRequireAuth.Name = "chkRequireAuth"
        chkRequireAuth.Size = New Size(309, 17)
        chkRequireAuth.TabIndex = 2
        chkRequireAuth.Text = "Require Windows authentication before viewing passwords"
        chkRequireAuth.UseVisualStyleBackColor = True
        ' 
        ' tpPrivacy
        ' 
        tpPrivacy.Controls.Add(grpPermissions)
        tpPrivacy.Controls.Add(chkIncognito)
        tpPrivacy.Controls.Add(chkHttpsOnly)
        tpPrivacy.Controls.Add(chkAdBlocker)
        tpPrivacy.Controls.Add(btnConfigureAdBlocker)
        tpPrivacy.Controls.Add(chkPhishing)
        tpPrivacy.Controls.Add(chkAllowPop)
        tpPrivacy.Controls.Add(grpJsSites)
        tpPrivacy.Controls.Add(grpBlocked)
        tpPrivacy.Location = New Point(4, 22)
        tpPrivacy.Name = "tpPrivacy"
        tpPrivacy.Padding = New Padding(10)
        tpPrivacy.Size = New Size(601, 452)
        tpPrivacy.TabIndex = 1
        tpPrivacy.Text = "Privacy & Security"
        tpPrivacy.UseVisualStyleBackColor = True
        ' 
        ' grpPermissions
        ' 
        grpPermissions.Controls.Add(chkPermCamera)
        grpPermissions.Controls.Add(chkPermMic)
        grpPermissions.Controls.Add(chkPermLocation)
        grpPermissions.Controls.Add(chkPermNotifications)
        grpPermissions.Location = New Point(23, 10)
        grpPermissions.Name = "grpPermissions"
        grpPermissions.Size = New Size(523, 65)
        grpPermissions.TabIndex = 0
        grpPermissions.TabStop = False
        grpPermissions.Text = "Permission Manager"
        ' 
        ' chkPermCamera
        ' 
        chkPermCamera.AutoSize = True
        chkPermCamera.Checked = True
        chkPermCamera.CheckState = CheckState.Checked
        chkPermCamera.Location = New Point(15, 22)
        chkPermCamera.Name = "chkPermCamera"
        chkPermCamera.Size = New Size(91, 17)
        chkPermCamera.TabIndex = 0
        chkPermCamera.Text = "Allow Camera"
        chkPermCamera.UseVisualStyleBackColor = True
        ' 
        ' chkPermMic
        ' 
        chkPermMic.AutoSize = True
        chkPermMic.Checked = True
        chkPermMic.CheckState = CheckState.Checked
        chkPermMic.Location = New Point(130, 22)
        chkPermMic.Name = "chkPermMic"
        chkPermMic.Size = New Size(109, 17)
        chkPermMic.TabIndex = 1
        chkPermMic.Text = "Allow Microphone"
        chkPermMic.UseVisualStyleBackColor = True
        ' 
        ' chkPermLocation
        ' 
        chkPermLocation.AutoSize = True
        chkPermLocation.Checked = True
        chkPermLocation.CheckState = CheckState.Checked
        chkPermLocation.Location = New Point(260, 22)
        chkPermLocation.Name = "chkPermLocation"
        chkPermLocation.Size = New Size(94, 17)
        chkPermLocation.TabIndex = 2
        chkPermLocation.Text = "Allow Location"
        chkPermLocation.UseVisualStyleBackColor = True
        ' 
        ' chkPermNotifications
        ' 
        chkPermNotifications.AutoSize = True
        chkPermNotifications.Checked = True
        chkPermNotifications.CheckState = CheckState.Checked
        chkPermNotifications.Location = New Point(380, 22)
        chkPermNotifications.Name = "chkPermNotifications"
        chkPermNotifications.Size = New Size(113, 17)
        chkPermNotifications.TabIndex = 3
        chkPermNotifications.Text = "Allow Notifications"
        chkPermNotifications.UseVisualStyleBackColor = True
        ' 
        ' chkIncognito
        ' 
        chkIncognito.AutoSize = True
        chkIncognito.Location = New Point(23, 82)
        chkIncognito.Name = "chkIncognito"
        chkIncognito.Size = New Size(197, 17)
        chkIncognito.TabIndex = 1
        chkIncognito.Text = "Incognito / Private Mode by Default"
        chkIncognito.UseVisualStyleBackColor = True
        ' 
        ' chkHttpsOnly
        ' 
        chkHttpsOnly.AutoSize = True
        chkHttpsOnly.Location = New Point(270, 82)
        chkHttpsOnly.Name = "chkHttpsOnly"
        chkHttpsOnly.Size = New Size(237, 17)
        chkHttpsOnly.TabIndex = 2
        chkHttpsOnly.Text = "HTTPS-Only Mode (Upgrade HTTP requests)"
        chkHttpsOnly.UseVisualStyleBackColor = True
        ' 
        ' chkAdBlocker
        ' 
        chkAdBlocker.AutoSize = True
        chkAdBlocker.Checked = True
        chkAdBlocker.CheckState = CheckState.Checked
        chkAdBlocker.Location = New Point(23, 105)
        chkAdBlocker.Name = "chkAdBlocker"
        chkAdBlocker.Size = New Size(187, 17)
        chkAdBlocker.TabIndex = 3
        chkAdBlocker.Text = "Enable Ad Blocker (EasyList rules)"
        chkAdBlocker.UseVisualStyleBackColor = True
        ' 
        ' btnConfigureAdBlocker
        ' 
        btnConfigureAdBlocker.Location = New Point(220, 102)
        btnConfigureAdBlocker.Name = "btnConfigureAdBlocker"
        btnConfigureAdBlocker.Size = New Size(42, 23)
        btnConfigureAdBlocker.TabIndex = 8
        btnConfigureAdBlocker.Text = "..."
        btnConfigureAdBlocker.UseVisualStyleBackColor = True
        ' 
        ' chkPhishing
        ' 
        chkPhishing.AutoSize = True
        chkPhishing.Checked = True
        chkPhishing.CheckState = CheckState.Checked
        chkPhishing.Location = New Point(270, 105)
        chkPhishing.Name = "chkPhishing"
        chkPhishing.Size = New Size(179, 17)
        chkPhishing.TabIndex = 4
        chkPhishing.Text = "Enable Phishing Protection Filter"
        chkPhishing.UseVisualStyleBackColor = True
        ' 
        ' chkAllowPop
        ' 
        chkAllowPop.AutoSize = True
        chkAllowPop.Checked = True
        chkAllowPop.CheckState = CheckState.Checked
        chkAllowPop.Location = New Point(23, 128)
        chkAllowPop.Name = "chkAllowPop"
        chkAllowPop.Size = New Size(132, 17)
        chkAllowPop.TabIndex = 5
        chkAllowPop.Text = "Enable Pop-up Blocker"
        chkAllowPop.UseVisualStyleBackColor = True
        ' 
        ' grpJsSites
        ' 
        grpJsSites.Controls.Add(lbJsDisabled)
        grpJsSites.Controls.Add(txtJsDomain)
        grpJsSites.Controls.Add(btnAddJsDomain)
        grpJsSites.Controls.Add(btnRemoveJsDomain)
        grpJsSites.Location = New Point(23, 150)
        grpJsSites.Name = "grpJsSites"
        grpJsSites.Size = New Size(250, 245)
        grpJsSites.TabIndex = 6
        grpJsSites.TabStop = False
        grpJsSites.Text = "Disable JavaScript Per Site"
        ' 
        ' lbJsDisabled
        ' 
        lbJsDisabled.FormattingEnabled = True
        lbJsDisabled.ItemHeight = 13
        lbJsDisabled.Location = New Point(10, 20)
        lbJsDisabled.Name = "lbJsDisabled"
        lbJsDisabled.Size = New Size(230, 147)
        lbJsDisabled.TabIndex = 0
        ' 
        ' txtJsDomain
        ' 
        txtJsDomain.Location = New Point(10, 175)
        txtJsDomain.Name = "txtJsDomain"
        txtJsDomain.Size = New Size(230, 21)
        txtJsDomain.TabIndex = 1
        ' 
        ' btnAddJsDomain
        ' 
        btnAddJsDomain.Location = New Point(10, 204)
        btnAddJsDomain.Name = "btnAddJsDomain"
        btnAddJsDomain.Size = New Size(105, 25)
        btnAddJsDomain.TabIndex = 2
        btnAddJsDomain.Text = "Add Domain"
        btnAddJsDomain.UseVisualStyleBackColor = True
        ' 
        ' btnRemoveJsDomain
        ' 
        btnRemoveJsDomain.Location = New Point(135, 204)
        btnRemoveJsDomain.Name = "btnRemoveJsDomain"
        btnRemoveJsDomain.Size = New Size(105, 25)
        btnRemoveJsDomain.TabIndex = 3
        btnRemoveJsDomain.Text = "Remove"
        btnRemoveJsDomain.UseVisualStyleBackColor = True
        ' 
        ' grpBlocked
        ' 
        grpBlocked.Controls.Add(lbBlocked)
        grpBlocked.Controls.Add(txtBlock)
        grpBlocked.Controls.Add(btnAddBlock)
        grpBlocked.Controls.Add(btnRemoveBlock)
        grpBlocked.Location = New Point(296, 150)
        grpBlocked.Name = "grpBlocked"
        grpBlocked.Size = New Size(250, 245)
        grpBlocked.TabIndex = 7
        grpBlocked.TabStop = False
        grpBlocked.Text = "Blocked Sites"
        ' 
        ' lbBlocked
        ' 
        lbBlocked.FormattingEnabled = True
        lbBlocked.ItemHeight = 13
        lbBlocked.Location = New Point(10, 20)
        lbBlocked.Name = "lbBlocked"
        lbBlocked.Size = New Size(230, 147)
        lbBlocked.TabIndex = 0
        ' 
        ' txtBlock
        ' 
        txtBlock.Location = New Point(10, 175)
        txtBlock.Name = "txtBlock"
        txtBlock.Size = New Size(230, 21)
        txtBlock.TabIndex = 1
        ' 
        ' btnAddBlock
        ' 
        btnAddBlock.Location = New Point(10, 204)
        btnAddBlock.Name = "btnAddBlock"
        btnAddBlock.Size = New Size(105, 25)
        btnAddBlock.TabIndex = 2
        btnAddBlock.Text = "Add Site"
        btnAddBlock.UseVisualStyleBackColor = True
        ' 
        ' btnRemoveBlock
        ' 
        btnRemoveBlock.Location = New Point(135, 204)
        ' 
        ' tpPerformance
        ' 
        tpPerformance.Controls.Add(grpGraphics)
        tpPerformance.Controls.Add(grpTabs)
        tpPerformance.Controls.Add(grpPageLoading)
        tpPerformance.Location = New Point(4, 22)
        tpPerformance.Name = "tpPerformance"
        tpPerformance.Padding = New Padding(10)
        tpPerformance.Size = New Size(601, 452)
        tpPerformance.TabIndex = 2
        tpPerformance.Text = "Performance"
        tpPerformance.UseVisualStyleBackColor = True
        ' 
        ' grpGraphics
        ' 
        grpGraphics.Controls.Add(chkUseHardwareAcceleration)
        grpGraphics.Controls.Add(lblGraphicsDesc)
        grpGraphics.Controls.Add(lblGraphicsStatus)
        grpGraphics.Location = New Point(10, 10)
        grpGraphics.Name = "grpGraphics"
        grpGraphics.Size = New Size(578, 95)
        grpGraphics.TabIndex = 0
        grpGraphics.TabStop = False
        grpGraphics.Text = "🖥️ Graphics"
        ' 
        ' chkUseHardwareAcceleration
        ' 
        chkUseHardwareAcceleration.AutoSize = True
        chkUseHardwareAcceleration.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        chkUseHardwareAcceleration.Location = New Point(15, 20)
        chkUseHardwareAcceleration.Name = "chkUseHardwareAcceleration"
        chkUseHardwareAcceleration.Size = New Size(265, 17)
        chkUseHardwareAcceleration.TabIndex = 0
        chkUseHardwareAcceleration.Text = "Use graphics acceleration when available"
        ToolTip1.SetToolTip(chkUseHardwareAcceleration, "Improves browser performance by using the GPU for rendering when supported by your hardware.")
        chkUseHardwareAcceleration.UseVisualStyleBackColor = True
        ' 
        ' lblGraphicsDesc
        ' 
        lblGraphicsDesc.AutoSize = True
        lblGraphicsDesc.ForeColor = Color.DimGray
        lblGraphicsDesc.Location = New Point(34, 40)
        lblGraphicsDesc.Name = "lblGraphicsDesc"
        lblGraphicsDesc.Size = New Size(420, 13)
        lblGraphicsDesc.TabIndex = 1
        lblGraphicsDesc.Text = "Improves browser performance by using the GPU for rendering when supported by your hardware."
        ' 
        ' lblGraphicsStatus
        ' 
        lblGraphicsStatus.AutoSize = True
        lblGraphicsStatus.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblGraphicsStatus.ForeColor = Color.DarkGoldenrod
        lblGraphicsStatus.Location = New Point(34, 62)
        lblGraphicsStatus.Name = "lblGraphicsStatus"
        lblGraphicsStatus.Size = New Size(245, 13)
        lblGraphicsStatus.TabIndex = 2
        lblGraphicsStatus.Text = "⚠️ Restart required for this setting to take effect."
        ' 
        ' grpTabs
        ' 
        grpTabs.Controls.Add(chkFadeInactiveTabs)
        grpTabs.Controls.Add(lblTabsDesc)
        grpTabs.Controls.Add(lblTabsStatus)
        grpTabs.Location = New Point(10, 115)
        grpTabs.Name = "grpTabs"
        grpTabs.Size = New Size(578, 95)
        grpTabs.TabIndex = 1
        grpTabs.TabStop = False
        grpTabs.Text = "📑 Tabs"
        ' 
        ' chkFadeInactiveTabs
        ' 
        chkFadeInactiveTabs.AutoSize = True
        chkFadeInactiveTabs.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        chkFadeInactiveTabs.Location = New Point(15, 20)
        chkFadeInactiveTabs.Name = "chkFadeInactiveTabs"
        chkFadeInactiveTabs.Size = New Size(130, 17)
        chkFadeInactiveTabs.TabIndex = 0
        chkFadeInactiveTabs.Text = "Fade inactive tabs"
        ToolTip1.SetToolTip(chkFadeInactiveTabs, "Tabs will appear visually inactive when they are sleeping or saving memory and CPU resources.")
        chkFadeInactiveTabs.UseVisualStyleBackColor = True
        ' 
        ' lblTabsDesc
        ' 
        lblTabsDesc.AutoSize = True
        lblTabsDesc.ForeColor = Color.DimGray
        lblTabsDesc.Location = New Point(34, 40)
        lblTabsDesc.Name = "lblTabsDesc"
        lblTabsDesc.Size = New Size(410, 13)
        lblTabsDesc.TabIndex = 1
        lblTabsDesc.Text = "Tabs will appear visually inactive when they are sleeping or saving memory and CPU resources."
        ' 
        ' lblTabsStatus
        ' 
        lblTabsStatus.AutoSize = True
        lblTabsStatus.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblTabsStatus.ForeColor = Color.ForestGreen
        lblTabsStatus.Location = New Point(34, 62)
        lblTabsStatus.Name = "lblTabsStatus"
        lblTabsStatus.Size = New Size(125, 13)
        lblTabsStatus.TabIndex = 2
        lblTabsStatus.Text = "✓ Applied immediately"
        ' 
        ' grpPageLoading
        ' 
        grpPageLoading.Controls.Add(chkPreloadPages)
        grpPageLoading.Controls.Add(lblPageLoadingDesc)
        grpPageLoading.Controls.Add(lblPageLoadingInfo)
        grpPageLoading.Controls.Add(lblPageLoadingStatus)
        grpPageLoading.Location = New Point(10, 220)
        grpPageLoading.Name = "grpPageLoading"
        grpPageLoading.Size = New Size(578, 120)
        grpPageLoading.TabIndex = 2
        grpPageLoading.TabStop = False
        grpPageLoading.Text = "⚡ Page Loading"
        ' 
        ' chkPreloadPages
        ' 
        chkPreloadPages.AutoSize = True
        chkPreloadPages.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        chkPreloadPages.Location = New Point(15, 20)
        chkPreloadPages.Name = "chkPreloadPages"
        chkPreloadPages.Size = New Size(298, 17)
        chkPreloadPages.TabIndex = 0
        chkPreloadPages.Text = "Preload pages for faster browsing and searching"
        ToolTip1.SetToolTip(chkPreloadPages, "Uses cookies and predictive loading to improve browsing performance by preloading likely pages.")
        chkPreloadPages.UseVisualStyleBackColor = True
        ' 
        ' lblPageLoadingDesc
        ' 
        lblPageLoadingDesc.AutoSize = True
        lblPageLoadingDesc.ForeColor = Color.DimGray
        lblPageLoadingDesc.Location = New Point(34, 40)
        lblPageLoadingDesc.Name = "lblPageLoadingDesc"
        lblPageLoadingDesc.Size = New Size(420, 13)
        lblPageLoadingDesc.TabIndex = 1
        lblPageLoadingDesc.Text = "Uses cookies and predictive loading to improve browsing performance by preloading likely pages."
        ' 
        ' lblPageLoadingInfo
        ' 
        lblPageLoadingInfo.AutoSize = True
        lblPageLoadingInfo.ForeColor = Color.DimGray
        lblPageLoadingInfo.Location = New Point(34, 58)
        lblPageLoadingInfo.Name = "lblPageLoadingInfo"
        lblPageLoadingInfo.Size = New Size(365, 13)
        lblPageLoadingInfo.TabIndex = 2
        lblPageLoadingInfo.Text = "Uses cookies to remember your preferences, even if you don't visit those pages."
        ' 
        ' lblPageLoadingStatus
        ' 
        lblPageLoadingStatus.AutoSize = True
        lblPageLoadingStatus.Font = New Font("Tahoma", 8.25F, FontStyle.Bold, GraphicsUnit.Point, CByte(0))
        lblPageLoadingStatus.ForeColor = Color.ForestGreen
        lblPageLoadingStatus.Location = New Point(34, 85)
        lblPageLoadingStatus.Name = "lblPageLoadingStatus"
        lblPageLoadingStatus.Size = New Size(125, 13)
        lblPageLoadingStatus.TabIndex = 3
        lblPageLoadingStatus.Text = "✓ Applied immediately"
        tpAdvanced.Location = New Point(4, 22)
        tpAdvanced.Name = "tpAdvanced"
        tpAdvanced.Padding = New Padding(10)
        tpAdvanced.Size = New Size(601, 452)
        tpAdvanced.TabIndex = 3
        tpAdvanced.Text = "Advanced"
        tpAdvanced.UseVisualStyleBackColor = True
        ' 
        ' grpDns
        ' 
        grpDns.Controls.Add(lblCustomDns)
        grpDns.Controls.Add(txtCustomDns)
        grpDns.Location = New Point(13, 10)
        grpDns.Name = "grpDns"
        grpDns.Size = New Size(556, 68)
        grpDns.TabIndex = 0
        grpDns.TabStop = False
        grpDns.Text = "Custom DNS Server"
        ' 
        ' lblCustomDns
        ' 
        lblCustomDns.AutoSize = True
        lblCustomDns.Location = New Point(15, 30)
        lblCustomDns.Name = "lblCustomDns"
        lblCustomDns.Size = New Size(121, 13)
        lblCustomDns.TabIndex = 0
        lblCustomDns.Text = "DNS Server IP Address:"
        ' 
        ' txtCustomDns
        ' 
        txtCustomDns.Location = New Point(138, 27)
        txtCustomDns.Name = "txtCustomDns"
        txtCustomDns.Size = New Size(250, 21)
        txtCustomDns.TabIndex = 1
        ' 
        ' grpProxy
        ' 
        grpProxy.Controls.Add(chkEnableProxy)
        grpProxy.Controls.Add(lblProxyHost)
        grpProxy.Controls.Add(txtProxyHost)
        grpProxy.Controls.Add(lblProxyPort)
        grpProxy.Controls.Add(txtProxyPort)
        grpProxy.Controls.Add(btnConfigureProxy)
        grpProxy.Location = New Point(13, 95)
        grpProxy.Name = "grpProxy"
        grpProxy.Size = New Size(556, 148)
        grpProxy.TabIndex = 1
        grpProxy.TabStop = False
        grpProxy.Text = "Proxy Settings"
        ' 
        ' chkEnableProxy
        ' 
        chkEnableProxy.AutoSize = True
        chkEnableProxy.Location = New Point(15, 25)
        chkEnableProxy.Name = "chkEnableProxy"
        chkEnableProxy.Size = New Size(128, 17)
        chkEnableProxy.TabIndex = 0
        chkEnableProxy.Text = "Enable Custom Proxy"
        chkEnableProxy.UseVisualStyleBackColor = True
        ' 
        ' lblProxyHost
        ' 
        lblProxyHost.AutoSize = True
        lblProxyHost.Location = New Point(15, 60)
        lblProxyHost.Name = "lblProxyHost"
        lblProxyHost.Size = New Size(84, 13)
        lblProxyHost.TabIndex = 1
        lblProxyHost.Text = "Proxy Host / IP:"
        ' 
        ' txtProxyHost
        ' 
        txtProxyHost.Location = New Point(135, 57)
        txtProxyHost.Name = "txtProxyHost"
        txtProxyHost.Size = New Size(250, 21)
        txtProxyHost.TabIndex = 1
        ' 
        ' lblProxyPort
        ' 
        lblProxyPort.AutoSize = True
        lblProxyPort.Location = New Point(15, 92)
        lblProxyPort.Name = "lblProxyPort"
        lblProxyPort.Size = New Size(31, 13)
        lblProxyPort.TabIndex = 2
        lblProxyPort.Text = "Port:"
        ' 
        ' txtProxyPort
        ' 
        txtProxyPort.Location = New Point(135, 89)
        txtProxyPort.Name = "txtProxyPort"
        txtProxyPort.Size = New Size(80, 21)
        txtProxyPort.TabIndex = 3
        txtProxyPort.Text = "8080"
        ' 
        ' btnConfigureProxy
        ' 
        btnConfigureProxy.Location = New Point(235, 87)
        btnConfigureProxy.Name = "btnConfigureProxy"
        btnConfigureProxy.Size = New Size(270, 25)
        btnConfigureProxy.TabIndex = 4
        btnConfigureProxy.Text = "Open Proxy Config"
        btnConfigureProxy.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnOK)
        Panel1.Controls.Add(btnCancel)
        Panel1.Controls.Add(btnApply)
        Panel1.Dock = DockStyle.Bottom
        Panel1.Location = New Point(0, 478)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(609, 45)
        Panel1.TabIndex = 1
        ' 
        ' btnOK
        ' 
        btnOK.Location = New Point(295, 10)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(80, 26)
        btnOK.TabIndex = 0
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnCancel
        ' 
        btnCancel.Location = New Point(380, 10)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(80, 26)
        btnCancel.TabIndex = 1
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = True
        ' 
        ' btnApply
        ' 
        btnApply.Location = New Point(465, 10)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(80, 26)
        btnApply.TabIndex = 2
        btnApply.Text = "Apply"
        btnApply.UseVisualStyleBackColor = True
        ' 
        ' tpAdvanced
        ' 
        tpAdvanced.Controls.Add(grpDns)
        tpAdvanced.Controls.Add(grpProxy)
        tpAdvanced.Location = New Point(4, 22)
        tpAdvanced.Name = "tpAdvanced"
        tpAdvanced.Padding = New Padding(10)
        tpAdvanced.Size = New Size(601, 452)
        tpAdvanced.TabIndex = 3
        tpAdvanced.Text = "Advanced"
        tpAdvanced.UseVisualStyleBackColor = True
        ' 
        ' grpDns
        ' 
        grpDns.Controls.Add(lblCustomDns)
        grpDns.Controls.Add(txtCustomDns)
        grpDns.Location = New Point(13, 10)
        grpDns.Name = "grpDns"
        grpDns.Size = New Size(556, 68)
        grpDns.TabIndex = 0
        grpDns.TabStop = False
        grpDns.Text = "Custom DNS Server"
        ' 
        ' lblCustomDns
        ' 
        lblCustomDns.AutoSize = True
        lblCustomDns.Location = New Point(15, 30)
        lblCustomDns.Name = "lblCustomDns"
        lblCustomDns.Size = New Size(121, 13)
        lblCustomDns.TabIndex = 0
        lblCustomDns.Text = "DNS Server IP Address:"
        ' 
        ' txtCustomDns
        ' 
        txtCustomDns.Location = New Point(138, 27)
        txtCustomDns.Name = "txtCustomDns"
        txtCustomDns.Size = New Size(250, 21)
        txtCustomDns.TabIndex = 1
        ' 
        ' grpProxy
        ' 
        grpProxy.Controls.Add(chkEnableProxy)
        grpProxy.Controls.Add(lblProxyHost)
        grpProxy.Controls.Add(txtProxyHost)
        grpProxy.Controls.Add(lblProxyPort)
        grpProxy.Controls.Add(txtProxyPort)
        grpProxy.Controls.Add(btnConfigureProxy)
        grpProxy.Location = New Point(13, 95)
        grpProxy.Name = "grpProxy"
        grpProxy.Size = New Size(556, 148)
        grpProxy.TabIndex = 1
        grpProxy.TabStop = False
        grpProxy.Text = "Proxy Settings"
        ' 
        ' chkEnableProxy
        ' 
        chkEnableProxy.AutoSize = True
        chkEnableProxy.Location = New Point(15, 25)
        chkEnableProxy.Name = "chkEnableProxy"
        chkEnableProxy.Size = New Size(128, 17)
        chkEnableProxy.TabIndex = 0
        chkEnableProxy.Text = "Enable Custom Proxy"
        chkEnableProxy.UseVisualStyleBackColor = True
        ' 
        ' lblProxyHost
        ' 
        lblProxyHost.AutoSize = True
        lblProxyHost.Location = New Point(15, 60)
        lblProxyHost.Name = "lblProxyHost"
        lblProxyHost.Size = New Size(84, 13)
        lblProxyHost.TabIndex = 1
        lblProxyHost.Text = "Proxy Host / IP:"
        ' 
        ' txtProxyHost
        ' 
        txtProxyHost.Location = New Point(135, 57)
        txtProxyHost.Name = "txtProxyHost"
        txtProxyHost.Size = New Size(250, 21)
        txtProxyHost.TabIndex = 1
        ' 
        ' lblProxyPort
        ' 
        lblProxyPort.AutoSize = True
        lblProxyPort.Location = New Point(15, 92)
        lblProxyPort.Name = "lblProxyPort"
        lblProxyPort.Size = New Size(31, 13)
        lblProxyPort.TabIndex = 2
        lblProxyPort.Text = "Port:"
        ' 
        ' txtProxyPort
        ' 
        txtProxyPort.Location = New Point(135, 89)
        txtProxyPort.Name = "txtProxyPort"
        txtProxyPort.Size = New Size(80, 21)
        txtProxyPort.TabIndex = 3
        txtProxyPort.Text = "8080"
        ' 
        ' btnConfigureProxy
        ' 
        btnConfigureProxy.Location = New Point(235, 87)
        btnConfigureProxy.Name = "btnConfigureProxy"
        btnConfigureProxy.Size = New Size(270, 25)
        btnConfigureProxy.TabIndex = 4
        btnConfigureProxy.Text = "Open Proxy Config"
        btnConfigureProxy.UseVisualStyleBackColor = True
        ' 
        ' Panel1
        ' 
        Panel1.Controls.Add(btnOK)
        Panel1.Controls.Add(btnCancel)
        Panel1.Controls.Add(btnApply)
        Panel1.Dock = DockStyle.Bottom
        Panel1.Location = New Point(0, 478)
        Panel1.Name = "Panel1"
        Panel1.Size = New Size(609, 45)
        Panel1.TabIndex = 1
        ' 
        ' btnOK
        ' 
        btnOK.Location = New Point(295, 10)
        btnOK.Name = "btnOK"
        btnOK.Size = New Size(80, 26)
        btnOK.TabIndex = 0
        btnOK.Text = "OK"
        btnOK.UseVisualStyleBackColor = True
        ' 
        ' btnCancel
        ' 
        btnCancel.Location = New Point(380, 10)
        btnCancel.Name = "btnCancel"
        btnCancel.Size = New Size(80, 26)
        btnCancel.TabIndex = 1
        btnCancel.Text = "Cancel"
        btnCancel.UseVisualStyleBackColor = True
        ' 
        ' btnApply
        ' 
        btnApply.Location = New Point(465, 10)
        btnApply.Name = "btnApply"
        btnApply.Size = New Size(80, 26)
        btnApply.TabIndex = 2
        btnApply.Text = "Apply"
        btnApply.UseVisualStyleBackColor = True
        ' 
        ' Settings
        ' 
        AutoScaleDimensions = New SizeF(6.0F, 13.0F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(609, 523)
        Controls.Add(TabControl1)
        Controls.Add(Panel1)
        Font = New Font("Tahoma", 8.25F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        FormBorderStyle = FormBorderStyle.FixedSingle
        MaximizeBox = False
        MinimizeBox = False
        Name = "Settings"
        ShowInTaskbar = False
        StartPosition = FormStartPosition.CenterScreen
        Text = "Browser Settings"
        TabControl1.ResumeLayout(False)
        tpBrowser.ResumeLayout(False)
        tpBrowser.PerformLayout()
        grpStartupSection.ResumeLayout(False)
        grpStartupSection.PerformLayout()
        grpBackupRestore.ResumeLayout(False)
        grpBackupRestore.PerformLayout()
        grpGraphics.ResumeLayout(False)
        grpGraphics.PerformLayout()
        grpTabs.ResumeLayout(False)
        grpTabs.PerformLayout()
        grpPageLoading.ResumeLayout(False)
        grpPageLoading.PerformLayout()
        tpPrivacyTab.ResumeLayout(False)
        grpCookies.ResumeLayout(False)
        grpCookies.PerformLayout()
        grpCookieManager.ResumeLayout(False)
        grpCookieManager.PerformLayout()
        CType(dgvCookies, ComponentModel.ISupportInitialize).EndInit()
        tpPasswords.ResumeLayout(False)
        grpSavedPasswords.ResumeLayout(False)
        grpSavedPasswords.PerformLayout()
        grpPasswordPreferences.ResumeLayout(False)
        grpPasswordPreferences.PerformLayout()
        CType(dgvPasswords, ComponentModel.ISupportInitialize).EndInit()
        tpPrivacy.ResumeLayout(False)
        tpPrivacy.PerformLayout()
        grpPermissions.ResumeLayout(False)
        grpPermissions.PerformLayout()
        grpJsSites.ResumeLayout(False)
        grpJsSites.PerformLayout()
        grpBlocked.ResumeLayout(False)
        grpBlocked.PerformLayout()
        tpPerformance.ResumeLayout(False)
        grpGraphics.ResumeLayout(False)
        grpGraphics.PerformLayout()
        grpTabs.ResumeLayout(False)
        grpTabs.PerformLayout()
        grpPageLoading.ResumeLayout(False)
        grpPageLoading.PerformLayout()
        tpAdvanced.ResumeLayout(False)
        grpDns.ResumeLayout(False)
        grpDns.PerformLayout()
        grpProxy.ResumeLayout(False)
        grpProxy.PerformLayout()
        Panel1.ResumeLayout(False)
        ResumeLayout(False)

    End Sub

    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents tpBrowser As System.Windows.Forms.TabPage
    Friend WithEvents tpPrivacy As System.Windows.Forms.TabPage
    Friend WithEvents tpPerformance As System.Windows.Forms.TabPage
    Friend WithEvents tpAdvanced As System.Windows.Forms.TabPage
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnApply As System.Windows.Forms.Button

    ' Browser Settings Controls
    Friend WithEvents lblHomePage As System.Windows.Forms.Label
    Friend WithEvents txtHomePage As System.Windows.Forms.TextBox
    Friend WithEvents btnDefaultHomePage As System.Windows.Forms.Button
    Friend WithEvents grpStartup As System.Windows.Forms.GroupBox
    Friend WithEvents rbStartupHome As System.Windows.Forms.RadioButton
    Friend WithEvents rbStartupBlank As System.Windows.Forms.RadioButton
    Friend WithEvents rbStartupRestore As System.Windows.Forms.RadioButton
    Friend WithEvents rbStartupSpecific As System.Windows.Forms.RadioButton
    Friend WithEvents lblNewTab As System.Windows.Forms.Label
    Friend WithEvents cmbNewTab As System.Windows.Forms.ComboBox
    Friend WithEvents lblSearchEngine As System.Windows.Forms.Label
    Friend WithEvents cmbSearchEngine As System.Windows.Forms.ComboBox
    Friend WithEvents lblDownloads As System.Windows.Forms.Label
    Friend WithEvents txtDownloads As System.Windows.Forms.TextBox
    Friend WithEvents btnBrowseDownloads As System.Windows.Forms.Button
    Friend WithEvents lblFontSize As System.Windows.Forms.Label
    Friend WithEvents cmbFontSize As System.Windows.Forms.ComboBox
    Friend WithEvents chkFullScreen As System.Windows.Forms.CheckBox

    ' Privacy Controls
    Friend WithEvents grpPermissions As System.Windows.Forms.GroupBox
    Friend WithEvents chkPermCamera As System.Windows.Forms.CheckBox
    Friend WithEvents chkPermMic As System.Windows.Forms.CheckBox
    Friend WithEvents chkPermLocation As System.Windows.Forms.CheckBox
    Friend WithEvents chkPermNotifications As System.Windows.Forms.CheckBox
    Friend WithEvents chkIncognito As System.Windows.Forms.CheckBox
    Friend WithEvents chkHttpsOnly As System.Windows.Forms.CheckBox
    Friend WithEvents chkAdBlocker As System.Windows.Forms.CheckBox
    Friend WithEvents btnConfigureAdBlocker As System.Windows.Forms.Button
    Friend WithEvents chkPhishing As System.Windows.Forms.CheckBox
    Friend WithEvents chkAllowPop As System.Windows.Forms.CheckBox
    Friend WithEvents grpJsSites As System.Windows.Forms.GroupBox
    Friend WithEvents lbJsDisabled As System.Windows.Forms.ListBox
    Friend WithEvents txtJsDomain As System.Windows.Forms.TextBox
    Friend WithEvents btnAddJsDomain As System.Windows.Forms.Button
    Friend WithEvents btnRemoveJsDomain As System.Windows.Forms.Button
    Friend WithEvents grpBlocked As System.Windows.Forms.GroupBox
    Friend WithEvents lbBlocked As System.Windows.Forms.ListBox
    Friend WithEvents txtBlock As System.Windows.Forms.TextBox
    Friend WithEvents btnAddBlock As System.Windows.Forms.Button
    Friend WithEvents btnRemoveBlock As System.Windows.Forms.Button

    ' Performance Controls
    Friend WithEvents grpGraphics As System.Windows.Forms.GroupBox
    Friend WithEvents chkUseHardwareAcceleration As System.Windows.Forms.CheckBox
    Friend WithEvents lblGraphicsDesc As System.Windows.Forms.Label
    Friend WithEvents lblGraphicsStatus As System.Windows.Forms.Label
    Friend WithEvents grpTabs As System.Windows.Forms.GroupBox
    Friend WithEvents chkFadeInactiveTabs As System.Windows.Forms.CheckBox
    Friend WithEvents lblTabsDesc As System.Windows.Forms.Label
    Friend WithEvents lblTabsStatus As System.Windows.Forms.Label
    Friend WithEvents grpPageLoading As System.Windows.Forms.GroupBox
    Friend WithEvents chkPreloadPages As System.Windows.Forms.CheckBox
    Friend WithEvents lblPageLoadingDesc As System.Windows.Forms.Label
    Friend WithEvents lblPageLoadingInfo As System.Windows.Forms.Label
    Friend WithEvents lblPageLoadingStatus As System.Windows.Forms.Label

    ' Advanced Controls
    Friend WithEvents grpDns As System.Windows.Forms.GroupBox
    Friend WithEvents lblCustomDns As System.Windows.Forms.Label
    Friend WithEvents txtCustomDns As System.Windows.Forms.TextBox
    Friend WithEvents grpProxy As System.Windows.Forms.GroupBox
    Friend WithEvents chkEnableProxy As System.Windows.Forms.CheckBox
    Friend WithEvents lblProxyHost As System.Windows.Forms.Label
    Friend WithEvents txtProxyHost As System.Windows.Forms.TextBox
    Friend WithEvents lblProxyPort As System.Windows.Forms.Label
    Friend WithEvents txtProxyPort As System.Windows.Forms.TextBox
    Friend WithEvents btnConfigureProxy As System.Windows.Forms.Button

    ' Startup Section Controls
    Friend WithEvents grpStartupSection As System.Windows.Forms.GroupBox
    Friend WithEvents rbContinueWhereLeftOff As System.Windows.Forms.RadioButton
    Friend WithEvents lblContinueWhereLeftOffDesc As System.Windows.Forms.Label
    Friend WithEvents rbOpenNewTabPage As System.Windows.Forms.RadioButton
    Friend WithEvents lblOpenNewTabPageDesc As System.Windows.Forms.Label

    ' Backup & Restore Section Controls
    Friend WithEvents grpBackupRestore As System.Windows.Forms.GroupBox
    Friend WithEvents btnCreateBackup As System.Windows.Forms.Button
    Friend WithEvents btnRestoreBackup As System.Windows.Forms.Button
    Friend WithEvents btnResetSettings As System.Windows.Forms.Button
    Friend WithEvents lblLastBackup As System.Windows.Forms.Label
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip

    ' New Privacy Tab Controls
    Friend WithEvents tpPrivacyTab As System.Windows.Forms.TabPage
    Friend WithEvents grpCookies As System.Windows.Forms.GroupBox
    Friend WithEvents chkBlockThirdPartyCookies As System.Windows.Forms.CheckBox
    Friend WithEvents lblBlockThirdPartyDesc As System.Windows.Forms.Label
    Friend WithEvents grpCookieManager As System.Windows.Forms.GroupBox
    Friend WithEvents txtCookieSearch As System.Windows.Forms.TextBox
    Friend WithEvents btnRefreshCookies As System.Windows.Forms.Button
    Friend WithEvents btnDeleteSelectedCookie As System.Windows.Forms.Button
    Friend WithEvents btnDeleteAllCookies As System.Windows.Forms.Button
    Friend WithEvents btnExportCookies As System.Windows.Forms.Button
    Friend WithEvents dgvCookies As System.Windows.Forms.DataGridView
    Friend WithEvents colWebsite As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colCookieName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colDomain As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPath As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colExpires As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSecure As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colHttpOnly As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colSameSite As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents lblCookieCount As System.Windows.Forms.Label

    ' Password Manager Controls
    Friend WithEvents tpPasswords As System.Windows.Forms.TabPage
    Friend WithEvents grpSavedPasswords As System.Windows.Forms.GroupBox
    Friend WithEvents txtPasswordSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblPasswordCount As System.Windows.Forms.Label
    Friend WithEvents dgvPasswords As System.Windows.Forms.DataGridView
    Friend WithEvents colPassWebsite As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPassUsername As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPassPassword As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPassCreated As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colPassLastUsed As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnDeletePassword As System.Windows.Forms.Button
    Friend WithEvents btnDeleteAllPasswords As System.Windows.Forms.Button
    Friend WithEvents btnExportPasswords As System.Windows.Forms.Button
    Friend WithEvents btnImportPasswords As System.Windows.Forms.Button
    Friend WithEvents grpPasswordPreferences As System.Windows.Forms.GroupBox
    Friend WithEvents chkSavePasswords As System.Windows.Forms.CheckBox
    Friend WithEvents chkAutoFill As System.Windows.Forms.CheckBox
    Friend WithEvents chkRequireAuth As System.Windows.Forms.CheckBox

End Class
