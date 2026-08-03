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
        grpPerfGeneral = New GroupBox()
        chkMemorySaver = New CheckBox()
        chkHardwareAccel = New CheckBox()
        grpDoH = New GroupBox()
        lblDoH = New Label()
        cmbDoH = New ComboBox()
        lblDoHCustom = New Label()
        txtDoHCustom = New TextBox()
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
        tpPrivacyTab.SuspendLayout()
        grpCookies.SuspendLayout()
        grpCookieManager.SuspendLayout()
        CType(dgvCookies, ComponentModel.ISupportInitialize).BeginInit()
        tpPrivacy.SuspendLayout()
        grpPermissions.SuspendLayout()
        grpJsSites.SuspendLayout()
        grpBlocked.SuspendLayout()
        tpPerformance.SuspendLayout()
        grpPerfGeneral.SuspendLayout()
        grpDoH.SuspendLayout()
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
        btnRemoveBlock.Name = "btnRemoveBlock"
        btnRemoveBlock.Size = New Size(105, 25)
        btnRemoveBlock.TabIndex = 3
        btnRemoveBlock.Text = "Remove"
        btnRemoveBlock.UseVisualStyleBackColor = True
        ' 
        ' tpPerformance
        ' 
        tpPerformance.Controls.Add(grpPerfGeneral)
        tpPerformance.Controls.Add(grpDoH)
        tpPerformance.Location = New Point(4, 22)
        tpPerformance.Name = "tpPerformance"
        tpPerformance.Padding = New Padding(10)
        tpPerformance.Size = New Size(601, 452)
        tpPerformance.TabIndex = 2
        tpPerformance.Text = "Performance"
        tpPerformance.UseVisualStyleBackColor = True
        ' 
        ' grpPerfGeneral
        ' 
        grpPerfGeneral.Controls.Add(chkMemorySaver)
        grpPerfGeneral.Controls.Add(chkHardwareAccel)
        grpPerfGeneral.Location = New Point(13, 10)
        grpPerfGeneral.Name = "grpPerfGeneral"
        grpPerfGeneral.Size = New Size(557, 94)
        grpPerfGeneral.TabIndex = 0
        grpPerfGeneral.TabStop = False
        grpPerfGeneral.Text = "Resource Management"
        ' 
        ' chkMemorySaver
        ' 
        chkMemorySaver.AutoSize = True
        chkMemorySaver.Checked = True
        chkMemorySaver.CheckState = CheckState.Checked
        chkMemorySaver.Location = New Point(15, 25)
        chkMemorySaver.Name = "chkMemorySaver"
        chkMemorySaver.Size = New Size(289, 17)
        chkMemorySaver.TabIndex = 0
        chkMemorySaver.Text = "Enable Memory Saver (sleep inactive background tabs)"
        chkMemorySaver.UseVisualStyleBackColor = True
        ' 
        ' chkHardwareAccel
        ' 
        chkHardwareAccel.AutoSize = True
        chkHardwareAccel.Checked = True
        chkHardwareAccel.CheckState = CheckState.Checked
        chkHardwareAccel.Location = New Point(15, 55)
        chkHardwareAccel.Name = "chkHardwareAccel"
        chkHardwareAccel.Size = New Size(244, 17)
        chkHardwareAccel.TabIndex = 1
        chkHardwareAccel.Text = "Enable Hardware Acceleration when available"
        chkHardwareAccel.UseVisualStyleBackColor = True
        ' 
        ' grpDoH
        ' 
        grpDoH.Controls.Add(lblDoH)
        grpDoH.Controls.Add(cmbDoH)
        grpDoH.Controls.Add(lblDoHCustom)
        grpDoH.Controls.Add(txtDoHCustom)
        grpDoH.Location = New Point(13, 115)
        grpDoH.Name = "grpDoH"
        grpDoH.Size = New Size(557, 114)
        grpDoH.TabIndex = 1
        grpDoH.TabStop = False
        grpDoH.Text = "DNS-over-HTTPS (DoH) Settings"
        ' 
        ' lblDoH
        ' 
        lblDoH.AutoSize = True
        lblDoH.Location = New Point(15, 30)
        lblDoH.Name = "lblDoH"
        lblDoH.Size = New Size(74, 13)
        lblDoH.TabIndex = 0
        lblDoH.Text = "DoH Provider:"
        ' 
        ' cmbDoH
        ' 
        cmbDoH.DropDownStyle = ComboBoxStyle.DropDownList
        cmbDoH.FormattingEnabled = True
        cmbDoH.Items.AddRange(New Object() {"Off (Default System DNS)", "Cloudflare (1.1.1.1)", "Google (8.8.8.8)", "Custom"})
        cmbDoH.Location = New Point(120, 27)
        cmbDoH.Name = "cmbDoH"
        cmbDoH.Size = New Size(250, 21)
        cmbDoH.TabIndex = 1
        ' 
        ' lblDoHCustom
        ' 
        lblDoHCustom.AutoSize = True
        lblDoHCustom.Location = New Point(15, 68)
        lblDoHCustom.Name = "lblDoHCustom"
        lblDoHCustom.Size = New Size(92, 13)
        lblDoHCustom.TabIndex = 2
        lblDoHCustom.Text = "Custom DoH URL:"
        ' 
        ' txtDoHCustom
        ' 
        txtDoHCustom.Location = New Point(120, 65)
        txtDoHCustom.Name = "txtDoHCustom"
        txtDoHCustom.Size = New Size(380, 21)
        txtDoHCustom.TabIndex = 3
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
chkHardwareAccel.TabIndex = 1
        chkHardwareAccel.Text = "Enable Hardware Acceleration when available"
        chkHardwareAccel.UseVisualStyleBackColor = True
        ' 
        ' grpDoH
        ' 
        grpDoH.Controls.Add(lblDoH)
        grpDoH.Controls.Add(cmbDoH)
        grpDoH.Controls.Add(lblDoHCustom)
        grpDoH.Controls.Add(txtDoHCustom)
        grpDoH.Location = New Point(13, 115)
        grpDoH.Name = "grpDoH"
        grpDoH.Size = New Size(557, 114)
        grpDoH.TabIndex = 1
        grpDoH.TabStop = False
        grpDoH.Text = "DNS-over-HTTPS (DoH) Settings"
        ' 
        ' lblDoH
        ' 
        lblDoH.AutoSize = True
        lblDoH.Location = New Point(15, 30)
        lblDoH.Name = "lblDoH"
        lblDoH.Size = New Size(74, 13)
        lblDoH.TabIndex = 0
        lblDoH.Text = "DoH Provider:"
        ' 
        ' cmbDoH
        ' 
        cmbDoH.DropDownStyle = ComboBoxStyle.DropDownList
        cmbDoH.FormattingEnabled = True
        cmbDoH.Items.AddRange(New Object() {"Off (Default System DNS)", "Cloudflare (1.1.1.1)", "Google (8.8.8.8)", "Custom"})
        cmbDoH.Location = New Point(120, 27)
        cmbDoH.Name = "cmbDoH"
        cmbDoH.Size = New Size(250, 21)
        cmbDoH.TabIndex = 1
        ' 
        ' lblDoHCustom
        ' 
        lblDoHCustom.AutoSize = True
        lblDoHCustom.Location = New Point(15, 68)
        lblDoHCustom.Name = "lblDoHCustom"
        lblDoHCustom.Size = New Size(92, 13)
        lblDoHCustom.TabIndex = 2
        lblDoHCustom.Text = "Custom DoH URL:"
        ' 
        ' txtDoHCustom
        ' 
        txtDoHCustom.Location = New Point(120, 65)
        txtDoHCustom.Name = "txtDoHCustom"
        txtDoHCustom.Size = New Size(380, 21)
        txtDoHCustom.TabIndex = 3
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
        tpPrivacyTab.ResumeLayout(False)
        grpCookies.ResumeLayout(False)
        grpCookies.PerformLayout()
        grpCookieManager.ResumeLayout(False)
        grpCookieManager.PerformLayout()
        CType(dgvCookies, ComponentModel.ISupportInitialize).EndInit()
        tpPrivacy.ResumeLayout(False)
        tpPrivacy.PerformLayout()
        grpPermissions.ResumeLayout(False)
        grpPermissions.PerformLayout()
        grpJsSites.ResumeLayout(False)
        grpJsSites.PerformLayout()
        grpBlocked.ResumeLayout(False)
        grpBlocked.PerformLayout()
        tpPerformance.ResumeLayout(False)
        grpPerfGeneral.ResumeLayout(False)
        grpPerfGeneral.PerformLayout()
        grpDoH.ResumeLayout(False)
        grpDoH.PerformLayout()
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
    Friend WithEvents grpPerfGeneral As System.Windows.Forms.GroupBox
    Friend WithEvents chkMemorySaver As System.Windows.Forms.CheckBox
    Friend WithEvents chkHardwareAccel As System.Windows.Forms.CheckBox
    Friend WithEvents grpDoH As System.Windows.Forms.GroupBox
    Friend WithEvents lblDoH As System.Windows.Forms.Label
    Friend WithEvents cmbDoH As System.Windows.Forms.ComboBox
    Friend WithEvents lblDoHCustom As System.Windows.Forms.Label
    Friend WithEvents txtDoHCustom As System.Windows.Forms.TextBox

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

End Class
