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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.tpBrowser = New System.Windows.Forms.TabPage()
        Me.lblHomePage = New System.Windows.Forms.Label()
        Me.txtHomePage = New System.Windows.Forms.TextBox()
        Me.btnDefaultHomePage = New System.Windows.Forms.Button()
        Me.grpStartup = New System.Windows.Forms.GroupBox()
        Me.rbStartupHome = New System.Windows.Forms.RadioButton()
        Me.rbStartupBlank = New System.Windows.Forms.RadioButton()
        Me.rbStartupRestore = New System.Windows.Forms.RadioButton()
        Me.rbStartupSpecific = New System.Windows.Forms.RadioButton()
        Me.lblNewTab = New System.Windows.Forms.Label()
        Me.cmbNewTab = New System.Windows.Forms.ComboBox()
        Me.lblSearchEngine = New System.Windows.Forms.Label()
        Me.cmbSearchEngine = New System.Windows.Forms.ComboBox()
        Me.lblDownloads = New System.Windows.Forms.Label()
        Me.txtDownloads = New System.Windows.Forms.TextBox()
        Me.btnBrowseDownloads = New System.Windows.Forms.Button()
        Me.lblFontSize = New System.Windows.Forms.Label()
        Me.cmbFontSize = New System.Windows.Forms.ComboBox()
        Me.chkFullScreen = New System.Windows.Forms.CheckBox()
        Me.tpPrivacy = New System.Windows.Forms.TabPage()
        Me.grpPermissions = New System.Windows.Forms.GroupBox()
        Me.chkPermCamera = New System.Windows.Forms.CheckBox()
        Me.chkPermMic = New System.Windows.Forms.CheckBox()
        Me.chkPermLocation = New System.Windows.Forms.CheckBox()
        Me.chkPermNotifications = New System.Windows.Forms.CheckBox()
        Me.chkIncognito = New System.Windows.Forms.CheckBox()
        Me.chkHttpsOnly = New System.Windows.Forms.CheckBox()
        Me.chkAdBlocker = New System.Windows.Forms.CheckBox()
        Me.btnConfigureAdBlocker = New System.Windows.Forms.Button()
        Me.chkPhishing = New System.Windows.Forms.CheckBox()
        Me.chkAllowPop = New System.Windows.Forms.CheckBox()
        Me.grpJsSites = New System.Windows.Forms.GroupBox()
        Me.lbJsDisabled = New System.Windows.Forms.ListBox()
        Me.txtJsDomain = New System.Windows.Forms.TextBox()
        Me.btnAddJsDomain = New System.Windows.Forms.Button()
        Me.btnRemoveJsDomain = New System.Windows.Forms.Button()
        Me.grpBlocked = New System.Windows.Forms.GroupBox()
        Me.lbBlocked = New System.Windows.Forms.ListBox()
        Me.txtBlock = New System.Windows.Forms.TextBox()
        Me.btnAddBlock = New System.Windows.Forms.Button()
        Me.btnRemoveBlock = New System.Windows.Forms.Button()
        Me.tpPerformance = New System.Windows.Forms.TabPage()
        Me.grpPerfGeneral = New System.Windows.Forms.GroupBox()
        Me.chkMemorySaver = New System.Windows.Forms.CheckBox()
        Me.chkHardwareAccel = New System.Windows.Forms.CheckBox()
        Me.grpDoH = New System.Windows.Forms.GroupBox()
        Me.lblDoH = New System.Windows.Forms.Label()
        Me.cmbDoH = New System.Windows.Forms.ComboBox()
        Me.lblDoHCustom = New System.Windows.Forms.Label()
        Me.txtDoHCustom = New System.Windows.Forms.TextBox()
        Me.tpAdvanced = New System.Windows.Forms.TabPage()
        Me.grpDns = New System.Windows.Forms.GroupBox()
        Me.lblCustomDns = New System.Windows.Forms.Label()
        Me.txtCustomDns = New System.Windows.Forms.TextBox()
        Me.grpProxy = New System.Windows.Forms.GroupBox()
        Me.chkEnableProxy = New System.Windows.Forms.CheckBox()
        Me.lblProxyHost = New System.Windows.Forms.Label()
        Me.txtProxyHost = New System.Windows.Forms.TextBox()
        Me.lblProxyPort = New System.Windows.Forms.Label()
        Me.txtProxyPort = New System.Windows.Forms.TextBox()
        Me.btnConfigureProxy = New System.Windows.Forms.Button()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnApply = New System.Windows.Forms.Button()
        Me.TabControl1.SuspendLayout()
        Me.tpBrowser.SuspendLayout()
        Me.grpStartup.SuspendLayout()
        Me.tpPrivacy.SuspendLayout()
        Me.grpPermissions.SuspendLayout()
        Me.grpJsSites.SuspendLayout()
        Me.grpBlocked.SuspendLayout()
        Me.tpPerformance.SuspendLayout()
        Me.grpPerfGeneral.SuspendLayout()
        Me.grpDoH.SuspendLayout()
        Me.tpAdvanced.SuspendLayout()
        Me.grpDns.SuspendLayout()
        Me.grpProxy.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.tpBrowser)
        Me.TabControl1.Controls.Add(Me.tpPrivacy)
        Me.TabControl1.Controls.Add(Me.tpPerformance)
        Me.TabControl1.Controls.Add(Me.tpAdvanced)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(560, 435)
        Me.TabControl1.TabIndex = 0
        '
        'tpBrowser
        '
        Me.tpBrowser.Controls.Add(Me.lblHomePage)
        Me.tpBrowser.Controls.Add(Me.txtHomePage)
        Me.tpBrowser.Controls.Add(Me.btnDefaultHomePage)
        Me.tpBrowser.Controls.Add(Me.grpStartup)
        Me.tpBrowser.Controls.Add(Me.lblNewTab)
        Me.tpBrowser.Controls.Add(Me.cmbNewTab)
        Me.tpBrowser.Controls.Add(Me.lblSearchEngine)
        Me.tpBrowser.Controls.Add(Me.cmbSearchEngine)
        Me.tpBrowser.Controls.Add(Me.lblDownloads)
        Me.tpBrowser.Controls.Add(Me.txtDownloads)
        Me.tpBrowser.Controls.Add(Me.btnBrowseDownloads)
        Me.tpBrowser.Controls.Add(Me.lblFontSize)
        Me.tpBrowser.Controls.Add(Me.cmbFontSize)
        Me.tpBrowser.Controls.Add(Me.chkFullScreen)
        Me.tpBrowser.Location = New System.Drawing.Point(4, 22)
        Me.tpBrowser.Name = "tpBrowser"
        Me.tpBrowser.Padding = New System.Windows.Forms.Padding(10)
        Me.tpBrowser.Size = New System.Drawing.Size(552, 409)
        Me.tpBrowser.TabIndex = 0
        Me.tpBrowser.Text = "Browser Settings"
        Me.tpBrowser.UseVisualStyleBackColor = True
        '
        'lblHomePage
        '
        Me.lblHomePage.AutoSize = True
        Me.lblHomePage.Location = New System.Drawing.Point(13, 15)
        Me.lblHomePage.Name = "lblHomePage"
        Me.lblHomePage.Size = New System.Drawing.Size(87, 13)
        Me.lblHomePage.TabIndex = 0
        Me.lblHomePage.Text = "Home Page URL:"
        '
        'txtHomePage
        '
        Me.txtHomePage.Location = New System.Drawing.Point(135, 12)
        Me.txtHomePage.Name = "txtHomePage"
        Me.txtHomePage.Size = New System.Drawing.Size(280, 21)
        Me.txtHomePage.TabIndex = 1
        '
        'btnDefaultHomePage
        '
        Me.btnDefaultHomePage.Location = New System.Drawing.Point(425, 10)
        Me.btnDefaultHomePage.Name = "btnDefaultHomePage"
        Me.btnDefaultHomePage.Size = New System.Drawing.Size(110, 25)
        Me.btnDefaultHomePage.TabIndex = 2
        Me.btnDefaultHomePage.Text = "Use Default"
        Me.btnDefaultHomePage.UseVisualStyleBackColor = True
        '
        'grpStartup
        '
        Me.grpStartup.Controls.Add(Me.rbStartupHome)
        Me.grpStartup.Controls.Add(Me.rbStartupBlank)
        Me.grpStartup.Controls.Add(Me.rbStartupRestore)
        Me.grpStartup.Controls.Add(Me.rbStartupSpecific)
        Me.grpStartup.Location = New System.Drawing.Point(16, 45)
        Me.grpStartup.Name = "grpStartup"
        Me.grpStartup.Size = New System.Drawing.Size(519, 75)
        Me.grpStartup.TabIndex = 3
        Me.grpStartup.TabStop = False
        Me.grpStartup.Text = "Startup Behavior"
        '
        'rbStartupHome
        '
        Me.rbStartupHome.AutoSize = True
        Me.rbStartupHome.Checked = True
        Me.rbStartupHome.Location = New System.Drawing.Point(15, 22)
        Me.rbStartupHome.Name = "rbStartupHome"
        Me.rbStartupHome.Size = New System.Drawing.Size(108, 17)
        Me.rbStartupHome.TabIndex = 0
        Me.rbStartupHome.TabStop = True
        Me.rbStartupHome.Text = "Open Home page"
        Me.rbStartupHome.UseVisualStyleBackColor = True
        '
        'rbStartupBlank
        '
        Me.rbStartupBlank.AutoSize = True
        Me.rbStartupBlank.Location = New System.Drawing.Point(150, 22)
        Me.rbStartupBlank.Name = "rbStartupBlank"
        Me.rbStartupBlank.Size = New System.Drawing.Size(106, 17)
        Me.rbStartupBlank.TabIndex = 1
        Me.rbStartupBlank.Text = "Open Blank page"
        Me.rbStartupBlank.UseVisualStyleBackColor = True
        '
        'rbStartupRestore
        '
        Me.rbStartupRestore.AutoSize = True
        Me.rbStartupRestore.Location = New System.Drawing.Point(15, 45)
        Me.rbStartupRestore.Name = "rbStartupRestore"
        Me.rbStartupRestore.Size = New System.Drawing.Size(145, 17)
        Me.rbStartupRestore.TabIndex = 2
        Me.rbStartupRestore.Text = "Restore previous session"
        Me.rbStartupRestore.UseVisualStyleBackColor = True
        '
        'rbStartupSpecific
        '
        Me.rbStartupSpecific.AutoSize = True
        Me.rbStartupSpecific.Location = New System.Drawing.Point(150, 45)
        Me.rbStartupSpecific.Name = "rbStartupSpecific"
        Me.rbStartupSpecific.Size = New System.Drawing.Size(116, 17)
        Me.rbStartupSpecific.TabIndex = 3
        Me.rbStartupSpecific.Text = "Open specific page"
        Me.rbStartupSpecific.UseVisualStyleBackColor = True
        '
        'lblNewTab
        '
        Me.lblNewTab.AutoSize = True
        Me.lblNewTab.Location = New System.Drawing.Point(13, 135)
        Me.lblNewTab.Name = "lblNewTab"
        Me.lblNewTab.Size = New System.Drawing.Size(80, 13)
        Me.lblNewTab.TabIndex = 4
        Me.lblNewTab.Text = "New Tab Page:"
        '
        'cmbNewTab
        '
        Me.cmbNewTab.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbNewTab.FormattingEnabled = True
        Me.cmbNewTab.Items.AddRange(New Object() {"Default Page", "Home Page", "Blank Page"})
        Me.cmbNewTab.Location = New System.Drawing.Point(135, 132)
        Me.cmbNewTab.Name = "cmbNewTab"
        Me.cmbNewTab.Size = New System.Drawing.Size(280, 21)
        Me.cmbNewTab.TabIndex = 4
        '
        'lblSearchEngine
        '
        Me.lblSearchEngine.AutoSize = True
        Me.lblSearchEngine.Location = New System.Drawing.Point(13, 170)
        Me.lblSearchEngine.Name = "lblSearchEngine"
        Me.lblSearchEngine.Size = New System.Drawing.Size(117, 13)
        Me.lblSearchEngine.TabIndex = 5
        Me.lblSearchEngine.Text = "Default Search Engine:"
        '
        'cmbSearchEngine
        '
        Me.cmbSearchEngine.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbSearchEngine.FormattingEnabled = True
        Me.cmbSearchEngine.Items.AddRange(New Object() {"Google", "Bing", "DuckDuckGo", "Yahoo"})
        Me.cmbSearchEngine.Location = New System.Drawing.Point(135, 167)
        Me.cmbSearchEngine.Name = "cmbSearchEngine"
        Me.cmbSearchEngine.Size = New System.Drawing.Size(280, 21)
        Me.cmbSearchEngine.TabIndex = 5
        '
        'lblDownloads
        '
        Me.lblDownloads.AutoSize = True
        Me.lblDownloads.Location = New System.Drawing.Point(13, 205)
        Me.lblDownloads.Name = "lblDownloads"
        Me.lblDownloads.Size = New System.Drawing.Size(96, 13)
        Me.lblDownloads.TabIndex = 6
        Me.lblDownloads.Text = "Downloads Folder:"
        '
        'txtDownloads
        '
        Me.txtDownloads.Location = New System.Drawing.Point(135, 202)
        Me.txtDownloads.Name = "txtDownloads"
        Me.txtDownloads.Size = New System.Drawing.Size(280, 21)
        Me.txtDownloads.TabIndex = 6
        '
        'btnBrowseDownloads
        '
        Me.btnBrowseDownloads.Location = New System.Drawing.Point(425, 200)
        Me.btnBrowseDownloads.Name = "btnBrowseDownloads"
        Me.btnBrowseDownloads.Size = New System.Drawing.Size(110, 25)
        Me.btnBrowseDownloads.TabIndex = 7
        Me.btnBrowseDownloads.Text = "Browse..."
        Me.btnBrowseDownloads.UseVisualStyleBackColor = True
        '
        'lblFontSize
        '
        Me.lblFontSize.AutoSize = True
        Me.lblFontSize.Location = New System.Drawing.Point(13, 240)
        Me.lblFontSize.Name = "lblFontSize"
        Me.lblFontSize.Size = New System.Drawing.Size(55, 13)
        Me.lblFontSize.TabIndex = 8
        Me.lblFontSize.Text = "Font Size:"
        '
        'cmbFontSize
        '
        Me.cmbFontSize.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbFontSize.FormattingEnabled = True
        Me.cmbFontSize.Items.AddRange(New Object() {"Small", "Medium", "Large", "Extra Large"})
        Me.cmbFontSize.Location = New System.Drawing.Point(135, 237)
        Me.cmbFontSize.Name = "cmbFontSize"
        Me.cmbFontSize.Size = New System.Drawing.Size(280, 21)
        Me.cmbFontSize.TabIndex = 8
        '
        'chkFullScreen
        '
        Me.chkFullScreen.AutoSize = True
        Me.chkFullScreen.Location = New System.Drawing.Point(135, 275)
        Me.chkFullScreen.Name = "chkFullScreen"
        Me.chkFullScreen.Size = New System.Drawing.Size(208, 17)
        Me.chkFullScreen.TabIndex = 9
        Me.chkFullScreen.Text = "Launch in Full-Screen mode by default"
        Me.chkFullScreen.UseVisualStyleBackColor = True
        '
        'tpPrivacy
        '
        Me.tpPrivacy.Controls.Add(Me.grpPermissions)
        Me.tpPrivacy.Controls.Add(Me.chkIncognito)
        Me.tpPrivacy.Controls.Add(Me.chkHttpsOnly)
        Me.tpPrivacy.Controls.Add(Me.chkAdBlocker)
        Me.tpPrivacy.Controls.Add(Me.btnConfigureAdBlocker)
        Me.tpPrivacy.Controls.Add(Me.chkPhishing)
        Me.tpPrivacy.Controls.Add(Me.chkAllowPop)
        Me.tpPrivacy.Controls.Add(Me.grpJsSites)
        Me.tpPrivacy.Controls.Add(Me.grpBlocked)
        Me.tpPrivacy.Location = New System.Drawing.Point(4, 22)
        Me.tpPrivacy.Name = "tpPrivacy"
        Me.tpPrivacy.Padding = New System.Windows.Forms.Padding(10)
        Me.tpPrivacy.Size = New System.Drawing.Size(552, 409)
        Me.tpPrivacy.TabIndex = 1
        Me.tpPrivacy.Text = "Privacy & Security"
        Me.tpPrivacy.UseVisualStyleBackColor = True
        '
        'grpPermissions
        '
        Me.grpPermissions.Controls.Add(Me.chkPermCamera)
        Me.grpPermissions.Controls.Add(Me.chkPermMic)
        Me.grpPermissions.Controls.Add(Me.chkPermLocation)
        Me.grpPermissions.Controls.Add(Me.chkPermNotifications)
        Me.grpPermissions.Location = New System.Drawing.Point(13, 10)
        Me.grpPermissions.Name = "grpPermissions"
        Me.grpPermissions.Size = New System.Drawing.Size(523, 65)
        Me.grpPermissions.TabIndex = 0
        Me.grpPermissions.TabStop = False
        Me.grpPermissions.Text = "Permission Manager"
        '
        'chkPermCamera
        '
        Me.chkPermCamera.AutoSize = True
        Me.chkPermCamera.Checked = True
        Me.chkPermCamera.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPermCamera.Location = New System.Drawing.Point(15, 22)
        Me.chkPermCamera.Name = "chkPermCamera"
        Me.chkPermCamera.Size = New System.Drawing.Size(91, 17)
        Me.chkPermCamera.TabIndex = 0
        Me.chkPermCamera.Text = "Allow Camera"
        Me.chkPermCamera.UseVisualStyleBackColor = True
        '
        'chkPermMic
        '
        Me.chkPermMic.AutoSize = True
        Me.chkPermMic.Checked = True
        Me.chkPermMic.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPermMic.Location = New System.Drawing.Point(130, 22)
        Me.chkPermMic.Name = "chkPermMic"
        Me.chkPermMic.Size = New System.Drawing.Size(109, 17)
        Me.chkPermMic.TabIndex = 1
        Me.chkPermMic.Text = "Allow Microphone"
        Me.chkPermMic.UseVisualStyleBackColor = True
        '
        'chkPermLocation
        '
        Me.chkPermLocation.AutoSize = True
        Me.chkPermLocation.Checked = True
        Me.chkPermLocation.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPermLocation.Location = New System.Drawing.Point(260, 22)
        Me.chkPermLocation.Name = "chkPermLocation"
        Me.chkPermLocation.Size = New System.Drawing.Size(94, 17)
        Me.chkPermLocation.TabIndex = 2
        Me.chkPermLocation.Text = "Allow Location"
        Me.chkPermLocation.UseVisualStyleBackColor = True
        '
        'chkPermNotifications
        '
        Me.chkPermNotifications.AutoSize = True
        Me.chkPermNotifications.Checked = True
        Me.chkPermNotifications.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPermNotifications.Location = New System.Drawing.Point(380, 22)
        Me.chkPermNotifications.Name = "chkPermNotifications"
        Me.chkPermNotifications.Size = New System.Drawing.Size(113, 17)
        Me.chkPermNotifications.TabIndex = 3
        Me.chkPermNotifications.Text = "Allow Notifications"
        Me.chkPermNotifications.UseVisualStyleBackColor = True
        '
        'chkIncognito
        '
        Me.chkIncognito.AutoSize = True
        Me.chkIncognito.Location = New System.Drawing.Point(13, 82)
        Me.chkIncognito.Name = "chkIncognito"
        Me.chkIncognito.Size = New System.Drawing.Size(197, 17)
        Me.chkIncognito.TabIndex = 1
        Me.chkIncognito.Text = "Incognito / Private Mode by Default"
        Me.chkIncognito.UseVisualStyleBackColor = True
        '
        'chkHttpsOnly
        '
        Me.chkHttpsOnly.AutoSize = True
        Me.chkHttpsOnly.Location = New System.Drawing.Point(260, 82)
        Me.chkHttpsOnly.Name = "chkHttpsOnly"
        Me.chkHttpsOnly.Size = New System.Drawing.Size(237, 17)
        Me.chkHttpsOnly.TabIndex = 2
        Me.chkHttpsOnly.Text = "HTTPS-Only Mode (Upgrade HTTP requests)"
        Me.chkHttpsOnly.UseVisualStyleBackColor = True
        '
        'chkAdBlocker
        '
        Me.chkAdBlocker.AutoSize = True
        Me.chkAdBlocker.Checked = True
        Me.chkAdBlocker.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAdBlocker.Location = New System.Drawing.Point(13, 105)
        Me.chkAdBlocker.Name = "chkAdBlocker"
        Me.chkAdBlocker.Size = New System.Drawing.Size(187, 17)
        Me.chkAdBlocker.TabIndex = 3
        Me.chkAdBlocker.Text = "Enable Ad Blocker (EasyList rules)"
        Me.chkAdBlocker.UseVisualStyleBackColor = True
        '
        'btnConfigureAdBlocker
        '
        Me.btnConfigureAdBlocker.Location = New System.Drawing.Point(210, 102)
        Me.btnConfigureAdBlocker.Name = "btnConfigureAdBlocker"
        Me.btnConfigureAdBlocker.Size = New System.Drawing.Size(42, 23)
        Me.btnConfigureAdBlocker.TabIndex = 8
        Me.btnConfigureAdBlocker.Text = "..."
        Me.btnConfigureAdBlocker.UseVisualStyleBackColor = True
        '
        'chkPhishing
        '
        Me.chkPhishing.AutoSize = True
        Me.chkPhishing.Checked = True
        Me.chkPhishing.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkPhishing.Location = New System.Drawing.Point(260, 105)
        Me.chkPhishing.Name = "chkPhishing"
        Me.chkPhishing.Size = New System.Drawing.Size(179, 17)
        Me.chkPhishing.TabIndex = 4
        Me.chkPhishing.Text = "Enable Phishing Protection Filter"
        Me.chkPhishing.UseVisualStyleBackColor = True
        '
        'chkAllowPop
        '
        Me.chkAllowPop.AutoSize = True
        Me.chkAllowPop.Checked = True
        Me.chkAllowPop.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkAllowPop.Location = New System.Drawing.Point(13, 128)
        Me.chkAllowPop.Name = "chkAllowPop"
        Me.chkAllowPop.Size = New System.Drawing.Size(132, 17)
        Me.chkAllowPop.TabIndex = 5
        Me.chkAllowPop.Text = "Enable Pop-up Blocker"
        Me.chkAllowPop.UseVisualStyleBackColor = True
        '
        'grpJsSites
        '
        Me.grpJsSites.Controls.Add(Me.lbJsDisabled)
        Me.grpJsSites.Controls.Add(Me.txtJsDomain)
        Me.grpJsSites.Controls.Add(Me.btnAddJsDomain)
        Me.grpJsSites.Controls.Add(Me.btnRemoveJsDomain)
        Me.grpJsSites.Location = New System.Drawing.Point(13, 150)
        Me.grpJsSites.Name = "grpJsSites"
        Me.grpJsSites.Size = New System.Drawing.Size(250, 245)
        Me.grpJsSites.TabIndex = 6
        Me.grpJsSites.TabStop = False
        Me.grpJsSites.Text = "Disable JavaScript Per Site"
        '
        'lbJsDisabled
        '
        Me.lbJsDisabled.FormattingEnabled = True
        Me.lbJsDisabled.Location = New System.Drawing.Point(10, 20)
        Me.lbJsDisabled.Name = "lbJsDisabled"
        Me.lbJsDisabled.Size = New System.Drawing.Size(230, 147)
        Me.lbJsDisabled.TabIndex = 0
        '
        'txtJsDomain
        '
        Me.txtJsDomain.Location = New System.Drawing.Point(10, 175)
        Me.txtJsDomain.Name = "txtJsDomain"
        Me.txtJsDomain.Size = New System.Drawing.Size(230, 21)
        Me.txtJsDomain.TabIndex = 1
        '
        'btnAddJsDomain
        '
        Me.btnAddJsDomain.Location = New System.Drawing.Point(10, 204)
        Me.btnAddJsDomain.Name = "btnAddJsDomain"
        Me.btnAddJsDomain.Size = New System.Drawing.Size(105, 25)
        Me.btnAddJsDomain.TabIndex = 2
        Me.btnAddJsDomain.Text = "Add Domain"
        Me.btnAddJsDomain.UseVisualStyleBackColor = True
        '
        'btnRemoveJsDomain
        '
        Me.btnRemoveJsDomain.Location = New System.Drawing.Point(135, 204)
        Me.btnRemoveJsDomain.Name = "btnRemoveJsDomain"
        Me.btnRemoveJsDomain.Size = New System.Drawing.Size(105, 25)
        Me.btnRemoveJsDomain.TabIndex = 3
        Me.btnRemoveJsDomain.Text = "Remove"
        Me.btnRemoveJsDomain.UseVisualStyleBackColor = True
        '
        'grpBlocked
        '
        Me.grpBlocked.Controls.Add(Me.lbBlocked)
        Me.grpBlocked.Controls.Add(Me.txtBlock)
        Me.grpBlocked.Controls.Add(Me.btnAddBlock)
        Me.grpBlocked.Controls.Add(Me.btnRemoveBlock)
        Me.grpBlocked.Location = New System.Drawing.Point(286, 150)
        Me.grpBlocked.Name = "grpBlocked"
        Me.grpBlocked.Size = New System.Drawing.Size(250, 245)
        Me.grpBlocked.TabIndex = 7
        Me.grpBlocked.TabStop = False
        Me.grpBlocked.Text = "Blocked Sites"
        '
        'lbBlocked
        '
        Me.lbBlocked.FormattingEnabled = True
        Me.lbBlocked.Location = New System.Drawing.Point(10, 20)
        Me.lbBlocked.Name = "lbBlocked"
        Me.lbBlocked.Size = New System.Drawing.Size(230, 147)
        Me.lbBlocked.TabIndex = 0
        '
        'txtBlock
        '
        Me.txtBlock.Location = New System.Drawing.Point(10, 175)
        Me.txtBlock.Name = "txtBlock"
        Me.txtBlock.Size = New System.Drawing.Size(230, 21)
        Me.txtBlock.TabIndex = 1
        '
        'btnAddBlock
        '
        Me.btnAddBlock.Location = New System.Drawing.Point(10, 204)
        Me.btnAddBlock.Name = "btnAddBlock"
        Me.btnAddBlock.Size = New System.Drawing.Size(105, 25)
        Me.btnAddBlock.TabIndex = 2
        Me.btnAddBlock.Text = "Add Site"
        Me.btnAddBlock.UseVisualStyleBackColor = True
        '
        'btnRemoveBlock
        '
        Me.btnRemoveBlock.Location = New System.Drawing.Point(135, 204)
        Me.btnRemoveBlock.Name = "btnRemoveBlock"
        Me.btnRemoveBlock.Size = New System.Drawing.Size(105, 25)
        Me.btnRemoveBlock.TabIndex = 3
        Me.btnRemoveBlock.Text = "Remove"
        Me.btnRemoveBlock.UseVisualStyleBackColor = True
        '
        'tpPerformance
        '
        Me.tpPerformance.Controls.Add(Me.grpPerfGeneral)
        Me.tpPerformance.Controls.Add(Me.grpDoH)
        Me.tpPerformance.Location = New System.Drawing.Point(4, 22)
        Me.tpPerformance.Name = "tpPerformance"
        Me.tpPerformance.Padding = New System.Windows.Forms.Padding(10)
        Me.tpPerformance.Size = New System.Drawing.Size(552, 409)
        Me.tpPerformance.TabIndex = 2
        Me.tpPerformance.Text = "Performance"
        Me.tpPerformance.UseVisualStyleBackColor = True
        '
        'grpPerfGeneral
        '
        Me.grpPerfGeneral.Controls.Add(Me.chkMemorySaver)
        Me.grpPerfGeneral.Controls.Add(Me.chkHardwareAccel)
        Me.grpPerfGeneral.Location = New System.Drawing.Point(13, 10)
        Me.grpPerfGeneral.Name = "grpPerfGeneral"
        Me.grpPerfGeneral.Size = New System.Drawing.Size(523, 90)
        Me.grpPerfGeneral.TabIndex = 0
        Me.grpPerfGeneral.TabStop = False
        Me.grpPerfGeneral.Text = "Resource Management"
        '
        'chkMemorySaver
        '
        Me.chkMemorySaver.AutoSize = True
        Me.chkMemorySaver.Checked = True
        Me.chkMemorySaver.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkMemorySaver.Location = New System.Drawing.Point(15, 25)
        Me.chkMemorySaver.Name = "chkMemorySaver"
        Me.chkMemorySaver.Size = New System.Drawing.Size(289, 17)
        Me.chkMemorySaver.TabIndex = 0
        Me.chkMemorySaver.Text = "Enable Memory Saver (sleep inactive background tabs)"
        Me.chkMemorySaver.UseVisualStyleBackColor = True
        '
        'chkHardwareAccel
        '
        Me.chkHardwareAccel.AutoSize = True
        Me.chkHardwareAccel.Checked = True
        Me.chkHardwareAccel.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkHardwareAccel.Location = New System.Drawing.Point(15, 55)
        Me.chkHardwareAccel.Name = "chkHardwareAccel"
        Me.chkHardwareAccel.Size = New System.Drawing.Size(244, 17)
        Me.chkHardwareAccel.TabIndex = 1
        Me.chkHardwareAccel.Text = "Enable Hardware Acceleration when available"
        Me.chkHardwareAccel.UseVisualStyleBackColor = True
        '
        'grpDoH
        '
        Me.grpDoH.Controls.Add(Me.lblDoH)
        Me.grpDoH.Controls.Add(Me.cmbDoH)
        Me.grpDoH.Controls.Add(Me.lblDoHCustom)
        Me.grpDoH.Controls.Add(Me.txtDoHCustom)
        Me.grpDoH.Location = New System.Drawing.Point(13, 115)
        Me.grpDoH.Name = "grpDoH"
        Me.grpDoH.Size = New System.Drawing.Size(523, 110)
        Me.grpDoH.TabIndex = 1
        Me.grpDoH.TabStop = False
        Me.grpDoH.Text = "DNS-over-HTTPS (DoH) Settings"
        '
        'lblDoH
        '
        Me.lblDoH.AutoSize = True
        Me.lblDoH.Location = New System.Drawing.Point(15, 30)
        Me.lblDoH.Name = "lblDoH"
        Me.lblDoH.Size = New System.Drawing.Size(74, 13)
        Me.lblDoH.TabIndex = 0
        Me.lblDoH.Text = "DoH Provider:"
        '
        'cmbDoH
        '
        Me.cmbDoH.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cmbDoH.FormattingEnabled = True
        Me.cmbDoH.Items.AddRange(New Object() {"Off (Default System DNS)", "Cloudflare (1.1.1.1)", "Google (8.8.8.8)", "Custom"})
        Me.cmbDoH.Location = New System.Drawing.Point(120, 27)
        Me.cmbDoH.Name = "cmbDoH"
        Me.cmbDoH.Size = New System.Drawing.Size(250, 21)
        Me.cmbDoH.TabIndex = 1
        '
        'lblDoHCustom
        '
        Me.lblDoHCustom.AutoSize = True
        Me.lblDoHCustom.Location = New System.Drawing.Point(15, 68)
        Me.lblDoHCustom.Name = "lblDoHCustom"
        Me.lblDoHCustom.Size = New System.Drawing.Size(92, 13)
        Me.lblDoHCustom.TabIndex = 2
        Me.lblDoHCustom.Text = "Custom DoH URL:"
        '
        'txtDoHCustom
        '
        Me.txtDoHCustom.Location = New System.Drawing.Point(120, 65)
        Me.txtDoHCustom.Name = "txtDoHCustom"
        Me.txtDoHCustom.Size = New System.Drawing.Size(380, 21)
        Me.txtDoHCustom.TabIndex = 3
        '
        'tpAdvanced
        '
        Me.tpAdvanced.Controls.Add(Me.grpDns)
        Me.tpAdvanced.Controls.Add(Me.grpProxy)
        Me.tpAdvanced.Location = New System.Drawing.Point(4, 22)
        Me.tpAdvanced.Name = "tpAdvanced"
        Me.tpAdvanced.Padding = New System.Windows.Forms.Padding(10)
        Me.tpAdvanced.Size = New System.Drawing.Size(552, 409)
        Me.tpAdvanced.TabIndex = 3
        Me.tpAdvanced.Text = "Advanced"
        Me.tpAdvanced.UseVisualStyleBackColor = True
        '
        'grpDns
        '
        Me.grpDns.Controls.Add(Me.lblCustomDns)
        Me.grpDns.Controls.Add(Me.txtCustomDns)
        Me.grpDns.Location = New System.Drawing.Point(13, 10)
        Me.grpDns.Name = "grpDns"
        Me.grpDns.Size = New System.Drawing.Size(523, 70)
        Me.grpDns.TabIndex = 0
        Me.grpDns.TabStop = False
        Me.grpDns.Text = "Custom DNS Server"
        '
        'lblCustomDns
        '
        Me.lblCustomDns.AutoSize = True
        Me.lblCustomDns.Location = New System.Drawing.Point(15, 30)
        Me.lblCustomDns.Name = "lblCustomDns"
        Me.lblCustomDns.Size = New System.Drawing.Size(121, 13)
        Me.lblCustomDns.TabIndex = 0
        Me.lblCustomDns.Text = "DNS Server IP Address:"
        '
        'txtCustomDns
        '
        Me.txtCustomDns.Location = New System.Drawing.Point(135, 27)
        Me.txtCustomDns.Name = "txtCustomDns"
        Me.txtCustomDns.Size = New System.Drawing.Size(250, 21)
        Me.txtCustomDns.TabIndex = 1
        '
        'grpProxy
        '
        Me.grpProxy.Controls.Add(Me.chkEnableProxy)
        Me.grpProxy.Controls.Add(Me.lblProxyHost)
        Me.grpProxy.Controls.Add(Me.txtProxyHost)
        Me.grpProxy.Controls.Add(Me.lblProxyPort)
        Me.grpProxy.Controls.Add(Me.txtProxyPort)
        Me.grpProxy.Controls.Add(Me.btnConfigureProxy)
        Me.grpProxy.Location = New System.Drawing.Point(13, 95)
        Me.grpProxy.Name = "grpProxy"
        Me.grpProxy.Size = New System.Drawing.Size(523, 150)
        Me.grpProxy.TabIndex = 1
        Me.grpProxy.TabStop = False
        Me.grpProxy.Text = "Proxy Settings"
        '
        'chkEnableProxy
        '
        Me.chkEnableProxy.AutoSize = True
        Me.chkEnableProxy.Location = New System.Drawing.Point(15, 25)
        Me.chkEnableProxy.Name = "chkEnableProxy"
        Me.chkEnableProxy.Size = New System.Drawing.Size(128, 17)
        Me.chkEnableProxy.TabIndex = 0
        Me.chkEnableProxy.Text = "Enable Custom Proxy"
        Me.chkEnableProxy.UseVisualStyleBackColor = True
        '
        'lblProxyHost
        '
        Me.lblProxyHost.AutoSize = True
        Me.lblProxyHost.Location = New System.Drawing.Point(15, 60)
        Me.lblProxyHost.Name = "lblProxyHost"
        Me.lblProxyHost.Size = New System.Drawing.Size(84, 13)
        Me.lblProxyHost.TabIndex = 1
        Me.lblProxyHost.Text = "Proxy Host / IP:"
        '
        'txtProxyHost
        '
        Me.txtProxyHost.Location = New System.Drawing.Point(135, 57)
        Me.txtProxyHost.Name = "txtProxyHost"
        Me.txtProxyHost.Size = New System.Drawing.Size(250, 21)
        Me.txtProxyHost.TabIndex = 1
        '
        'lblProxyPort
        '
        Me.lblProxyPort.AutoSize = True
        Me.lblProxyPort.Location = New System.Drawing.Point(15, 92)
        Me.lblProxyPort.Name = "lblProxyPort"
        Me.lblProxyPort.Size = New System.Drawing.Size(31, 13)
        Me.lblProxyPort.TabIndex = 2
        Me.lblProxyPort.Text = "Port:"
        '
        'txtProxyPort
        '
        Me.txtProxyPort.Location = New System.Drawing.Point(135, 89)
        Me.txtProxyPort.Name = "txtProxyPort"
        Me.txtProxyPort.Size = New System.Drawing.Size(80, 21)
        Me.txtProxyPort.TabIndex = 3
        Me.txtProxyPort.Text = "8080"
        '
        'btnConfigureProxy
        '
        Me.btnConfigureProxy.Location = New System.Drawing.Point(235, 87)
        Me.btnConfigureProxy.Name = "btnConfigureProxy"
        Me.btnConfigureProxy.Size = New System.Drawing.Size(270, 25)
        Me.btnConfigureProxy.TabIndex = 4
        Me.btnConfigureProxy.Text = "Open Proxy Config"
        Me.btnConfigureProxy.UseVisualStyleBackColor = True
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOK)
        Me.Panel1.Controls.Add(Me.btnCancel)
        Me.Panel1.Controls.Add(Me.btnApply)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 435)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(560, 45)
        Me.Panel1.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(295, 10)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 26)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(380, 10)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 26)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnApply
        '
        Me.btnApply.Location = New System.Drawing.Point(465, 10)
        Me.btnApply.Name = "btnApply"
        Me.btnApply.Size = New System.Drawing.Size(80, 26)
        Me.btnApply.TabIndex = 2
        Me.btnApply.Text = "Apply"
        Me.btnApply.UseVisualStyleBackColor = True
        '
        'Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(560, 480)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Settings"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Browser Settings"
        Me.TabControl1.ResumeLayout(False)
        Me.tpBrowser.ResumeLayout(False)
        Me.tpBrowser.PerformLayout()
        Me.grpStartup.ResumeLayout(False)
        Me.grpStartup.PerformLayout()
        Me.tpPrivacy.ResumeLayout(False)
        Me.tpPrivacy.PerformLayout()
        Me.grpPermissions.ResumeLayout(False)
        Me.grpPermissions.PerformLayout()
        Me.grpJsSites.ResumeLayout(False)
        Me.grpJsSites.PerformLayout()
        Me.grpBlocked.ResumeLayout(False)
        Me.grpBlocked.PerformLayout()
        Me.tpPerformance.ResumeLayout(False)
        Me.grpPerfGeneral.ResumeLayout(False)
        Me.grpPerfGeneral.PerformLayout()
        Me.grpDoH.ResumeLayout(False)
        Me.grpDoH.PerformLayout()
        Me.tpAdvanced.ResumeLayout(False)
        Me.grpDns.ResumeLayout(False)
        Me.grpDns.PerformLayout()
        Me.grpProxy.ResumeLayout(False)
        Me.grpProxy.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

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

End Class
