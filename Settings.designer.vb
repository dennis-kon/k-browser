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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Settings))
        Me.cmSearch = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmSearchRemove = New System.Windows.Forms.ToolStripMenuItem()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.HelpProvider1 = New System.Windows.Forms.HelpProvider()
        Me.txtPop = New System.Windows.Forms.TextBox()
        Me.lbPop = New System.Windows.Forms.ListBox()
        Me.chkAllowPop = New System.Windows.Forms.CheckBox()
        Me.TabPage7 = New System.Windows.Forms.TabPage()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.chkPhishing = New System.Windows.Forms.CheckBox()
        Me.btnRemoveAllPhish = New System.Windows.Forms.Button()
        Me.btnRemovePhish = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.lbPhishing = New System.Windows.Forms.ListBox()
        Me.btnAddPhish = New System.Windows.Forms.Button()
        Me.txtPhishing = New System.Windows.Forms.TextBox()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.btnBlockRemoveAll = New System.Windows.Forms.Button()
        Me.btnRemoveBlock = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lbBlocked = New System.Windows.Forms.ListBox()
        Me.btnAddBlock = New System.Windows.Forms.Button()
        Me.txtBlock = New System.Windows.Forms.TextBox()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.btnPopRemoveAll = New System.Windows.Forms.Button()
        Me.btnPopRemove = New System.Windows.Forms.Button()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.btnPopAdd = New System.Windows.Forms.Button()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.chkPopInfo = New System.Windows.Forms.CheckBox()
        Me.chkPopSound = New System.Windows.Forms.CheckBox()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.TextBox3 = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Button2 = New System.Windows.Forms.Button()
        Me.Button3 = New System.Windows.Forms.Button()
        Me.ListBox2 = New System.Windows.Forms.ListBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.ListBox1 = New System.Windows.Forms.ListBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.TextBox2 = New System.Windows.Forms.TextBox()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmSearch.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.TabPage7.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.TabPage3.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.TabPage2.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmSearch
        '
        Me.cmSearch.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmSearchRemove})
        Me.cmSearch.Name = "cmSearch"
        Me.cmSearch.Size = New System.Drawing.Size(118, 26)
        '
        'cmSearchRemove
        '
        Me.cmSearchRemove.Name = "cmSearchRemove"
        Me.cmSearchRemove.Size = New System.Drawing.Size(117, 22)
        Me.cmSearchRemove.Text = "Remove"
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnOK)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel1.Location = New System.Drawing.Point(0, 401)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(443, 32)
        Me.Panel1.TabIndex = 1
        '
        'btnOK
        '
        Me.btnOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnOK.Location = New System.Drawing.Point(350, 2)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(88, 27)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'txtPop
        '
        Me.HelpProvider1.SetHelpString(Me.txtPop, "URL of site you want to add to allowed list (http://www.microsoft.com)")
        Me.txtPop.Location = New System.Drawing.Point(6, 31)
        Me.txtPop.Name = "txtPop"
        Me.HelpProvider1.SetShowHelp(Me.txtPop, True)
        Me.txtPop.Size = New System.Drawing.Size(283, 21)
        Me.txtPop.TabIndex = 0
        '
        'lbPop
        '
        Me.lbPop.FormattingEnabled = True
        Me.HelpProvider1.SetHelpString(Me.lbPop, "This is a list of sites where popups are allowed. You can use shift + click to al" & _
                "low a popup at anytime.")
        Me.lbPop.Location = New System.Drawing.Point(6, 74)
        Me.lbPop.Name = "lbPop"
        Me.HelpProvider1.SetShowHelp(Me.lbPop, True)
        Me.lbPop.Size = New System.Drawing.Size(283, 173)
        Me.lbPop.TabIndex = 2
        '
        'chkAllowPop
        '
        Me.chkAllowPop.AutoSize = True
        Me.HelpProvider1.SetHelpString(Me.chkAllowPop, "Enables or disables the popup blocker feature.")
        Me.chkAllowPop.Location = New System.Drawing.Point(11, 7)
        Me.chkAllowPop.Name = "chkAllowPop"
        Me.HelpProvider1.SetShowHelp(Me.chkAllowPop, True)
        Me.chkAllowPop.Size = New System.Drawing.Size(128, 17)
        Me.chkAllowPop.TabIndex = 7
        Me.chkAllowPop.Text = "Enable popup blocker"
        Me.chkAllowPop.UseVisualStyleBackColor = True
        '
        'TabPage7
        '
        Me.TabPage7.Controls.Add(Me.GroupBox6)
        Me.TabPage7.Location = New System.Drawing.Point(4, 22)
        Me.TabPage7.Name = "TabPage7"
        Me.TabPage7.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage7.Size = New System.Drawing.Size(435, 375)
        Me.TabPage7.TabIndex = 6
        Me.TabPage7.Text = "Phishing"
        Me.TabPage7.UseVisualStyleBackColor = True
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.chkPhishing)
        Me.GroupBox6.Controls.Add(Me.btnRemoveAllPhish)
        Me.GroupBox6.Controls.Add(Me.btnRemovePhish)
        Me.GroupBox6.Controls.Add(Me.Label11)
        Me.GroupBox6.Controls.Add(Me.Label12)
        Me.GroupBox6.Controls.Add(Me.lbPhishing)
        Me.GroupBox6.Controls.Add(Me.btnAddPhish)
        Me.GroupBox6.Controls.Add(Me.txtPhishing)
        Me.GroupBox6.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox6.Location = New System.Drawing.Point(3, 3)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(429, 369)
        Me.GroupBox6.TabIndex = 4
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Phishing Manager"
        '
        'chkPhishing
        '
        Me.chkPhishing.AutoSize = True
        Me.chkPhishing.Location = New System.Drawing.Point(9, 20)
        Me.chkPhishing.Name = "chkPhishing"
        Me.chkPhishing.Size = New System.Drawing.Size(127, 17)
        Me.chkPhishing.TabIndex = 7
        Me.chkPhishing.Text = "Enable Phishing Filter"
        Me.chkPhishing.UseVisualStyleBackColor = True
        '
        'btnRemoveAllPhish
        '
        Me.btnRemoveAllPhish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveAllPhish.Location = New System.Drawing.Point(347, 130)
        Me.btnRemoveAllPhish.Name = "btnRemoveAllPhish"
        Me.btnRemoveAllPhish.Size = New System.Drawing.Size(74, 23)
        Me.btnRemoveAllPhish.TabIndex = 6
        Me.btnRemoveAllPhish.Text = "Remove ALL"
        Me.btnRemoveAllPhish.UseVisualStyleBackColor = True
        '
        'btnRemovePhish
        '
        Me.btnRemovePhish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemovePhish.Location = New System.Drawing.Point(347, 101)
        Me.btnRemovePhish.Name = "btnRemovePhish"
        Me.btnRemovePhish.Size = New System.Drawing.Size(74, 23)
        Me.btnRemovePhish.TabIndex = 5
        Me.btnRemovePhish.Text = "Remove"
        Me.btnRemovePhish.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.Location = New System.Drawing.Point(5, 78)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(294, 16)
        Me.Label11.TabIndex = 4
        Me.Label11.Text = "Phishing Sites"
        '
        'Label12
        '
        Me.Label12.Location = New System.Drawing.Point(6, 39)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(363, 13)
        Me.Label12.TabIndex = 3
        Me.Label12.Text = "Address of phishing website"
        '
        'lbPhishing
        '
        Me.lbPhishing.FormattingEnabled = True
        Me.lbPhishing.Location = New System.Drawing.Point(6, 100)
        Me.lbPhishing.Name = "lbPhishing"
        Me.lbPhishing.Size = New System.Drawing.Size(292, 264)
        Me.lbPhishing.TabIndex = 2
        '
        'btnAddPhish
        '
        Me.btnAddPhish.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddPhish.Location = New System.Drawing.Point(346, 53)
        Me.btnAddPhish.Name = "btnAddPhish"
        Me.btnAddPhish.Size = New System.Drawing.Size(75, 23)
        Me.btnAddPhish.TabIndex = 1
        Me.btnAddPhish.Text = "Add"
        Me.btnAddPhish.UseVisualStyleBackColor = True
        '
        'txtPhishing
        '
        Me.txtPhishing.Location = New System.Drawing.Point(5, 54)
        Me.txtPhishing.Name = "txtPhishing"
        Me.txtPhishing.Size = New System.Drawing.Size(292, 21)
        Me.txtPhishing.TabIndex = 0
        '
        'TabPage3
        '
        Me.TabPage3.Controls.Add(Me.GroupBox2)
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Size = New System.Drawing.Size(435, 375)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Blocked Sites"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.btnBlockRemoveAll)
        Me.GroupBox2.Controls.Add(Me.btnRemoveBlock)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.lbBlocked)
        Me.GroupBox2.Controls.Add(Me.btnAddBlock)
        Me.GroupBox2.Controls.Add(Me.txtBlock)
        Me.GroupBox2.Dock = System.Windows.Forms.DockStyle.Fill
        Me.GroupBox2.Location = New System.Drawing.Point(0, 0)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(435, 375)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Blocked Sites Manager"
        '
        'btnBlockRemoveAll
        '
        Me.btnBlockRemoveAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnBlockRemoveAll.Location = New System.Drawing.Point(353, 111)
        Me.btnBlockRemoveAll.Name = "btnBlockRemoveAll"
        Me.btnBlockRemoveAll.Size = New System.Drawing.Size(74, 23)
        Me.btnBlockRemoveAll.TabIndex = 6
        Me.btnBlockRemoveAll.Text = "Remove ALL"
        Me.btnBlockRemoveAll.UseVisualStyleBackColor = True
        '
        'btnRemoveBlock
        '
        Me.btnRemoveBlock.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnRemoveBlock.Location = New System.Drawing.Point(353, 82)
        Me.btnRemoveBlock.Name = "btnRemoveBlock"
        Me.btnRemoveBlock.Size = New System.Drawing.Size(74, 23)
        Me.btnRemoveBlock.TabIndex = 5
        Me.btnRemoveBlock.Text = "Remove"
        Me.btnRemoveBlock.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 55)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(294, 16)
        Me.Label3.TabIndex = 4
        Me.Label3.Text = "Blocked Sites"
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(7, 16)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(363, 13)
        Me.Label4.TabIndex = 3
        Me.Label4.Text = "Address of website to block"
        '
        'lbBlocked
        '
        Me.lbBlocked.FormattingEnabled = True
        Me.lbBlocked.Location = New System.Drawing.Point(6, 74)
        Me.lbBlocked.Name = "lbBlocked"
        Me.lbBlocked.Size = New System.Drawing.Size(292, 290)
        Me.lbBlocked.TabIndex = 2
        '
        'btnAddBlock
        '
        Me.btnAddBlock.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAddBlock.Location = New System.Drawing.Point(353, 31)
        Me.btnAddBlock.Name = "btnAddBlock"
        Me.btnAddBlock.Size = New System.Drawing.Size(75, 23)
        Me.btnAddBlock.TabIndex = 1
        Me.btnAddBlock.Text = "Add"
        Me.btnAddBlock.UseVisualStyleBackColor = True
        '
        'txtBlock
        '
        Me.txtBlock.Location = New System.Drawing.Point(6, 31)
        Me.txtBlock.Name = "txtBlock"
        Me.txtBlock.Size = New System.Drawing.Size(292, 21)
        Me.txtBlock.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.chkAllowPop)
        Me.TabPage1.Controls.Add(Me.GroupBox4)
        Me.TabPage1.Controls.Add(Me.GroupBox3)
        Me.TabPage1.ImageIndex = 0
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(435, 375)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Popup Blocker"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.btnPopRemoveAll)
        Me.GroupBox4.Controls.Add(Me.btnPopRemove)
        Me.GroupBox4.Controls.Add(Me.Label5)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.lbPop)
        Me.GroupBox4.Controls.Add(Me.btnPopAdd)
        Me.GroupBox4.Controls.Add(Me.txtPop)
        Me.GroupBox4.Location = New System.Drawing.Point(5, 30)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(376, 262)
        Me.GroupBox4.TabIndex = 6
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Exceptions"
        '
        'btnPopRemoveAll
        '
        Me.btnPopRemoveAll.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPopRemoveAll.Location = New System.Drawing.Point(294, 122)
        Me.btnPopRemoveAll.Name = "btnPopRemoveAll"
        Me.btnPopRemoveAll.Size = New System.Drawing.Size(74, 23)
        Me.btnPopRemoveAll.TabIndex = 6
        Me.btnPopRemoveAll.Text = "Remove ALL"
        Me.btnPopRemoveAll.UseVisualStyleBackColor = True
        '
        'btnPopRemove
        '
        Me.btnPopRemove.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPopRemove.Location = New System.Drawing.Point(294, 93)
        Me.btnPopRemove.Name = "btnPopRemove"
        Me.btnPopRemove.Size = New System.Drawing.Size(74, 23)
        Me.btnPopRemove.TabIndex = 5
        Me.btnPopRemove.Text = "Remove"
        Me.btnPopRemove.UseVisualStyleBackColor = True
        '
        'Label5
        '
        Me.Label5.Location = New System.Drawing.Point(6, 55)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 16)
        Me.Label5.TabIndex = 4
        Me.Label5.Text = "Allowed Sites"
        '
        'Label6
        '
        Me.Label6.Location = New System.Drawing.Point(7, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(363, 13)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Address of website to allow"
        '
        'btnPopAdd
        '
        Me.btnPopAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnPopAdd.Location = New System.Drawing.Point(293, 51)
        Me.btnPopAdd.Name = "btnPopAdd"
        Me.btnPopAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnPopAdd.TabIndex = 1
        Me.btnPopAdd.Text = "Add"
        Me.btnPopAdd.UseVisualStyleBackColor = True
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.chkPopInfo)
        Me.GroupBox3.Controls.Add(Me.chkPopSound)
        Me.GroupBox3.Location = New System.Drawing.Point(5, 298)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(376, 70)
        Me.GroupBox3.TabIndex = 5
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Default action"
        '
        'chkPopInfo
        '
        Me.chkPopInfo.AutoSize = True
        Me.chkPopInfo.Location = New System.Drawing.Point(10, 43)
        Me.chkPopInfo.Name = "chkPopInfo"
        Me.chkPopInfo.Size = New System.Drawing.Size(218, 17)
        Me.chkPopInfo.TabIndex = 4
        Me.chkPopInfo.Text = "Show Info bar when a popup is blocked."
        Me.chkPopInfo.UseVisualStyleBackColor = True
        '
        'chkPopSound
        '
        Me.chkPopSound.AutoSize = True
        Me.chkPopSound.Location = New System.Drawing.Point(10, 20)
        Me.chkPopSound.Name = "chkPopSound"
        Me.chkPopSound.Size = New System.Drawing.Size(203, 17)
        Me.chkPopSound.TabIndex = 3
        Me.chkPopSound.Text = "Play Sound when a popup is blocked."
        Me.chkPopSound.UseVisualStyleBackColor = True
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.TabPage1)
        Me.TabControl1.Controls.Add(Me.TabPage3)
        Me.TabControl1.Controls.Add(Me.TabPage7)
        Me.TabControl1.Controls.Add(Me.TabPage2)
        Me.TabControl1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Multiline = True
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(443, 401)
        Me.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight
        Me.TabControl1.TabIndex = 0
        '
        'TabPage2
        '
        Me.TabPage2.Controls.Add(Me.Panel2)
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(435, 375)
        Me.TabPage2.TabIndex = 7
        Me.TabPage2.Text = "IP Fnder"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel2.Controls.Add(Me.Label9)
        Me.Panel2.Controls.Add(Me.TextBox3)
        Me.Panel2.Controls.Add(Me.Label8)
        Me.Panel2.Controls.Add(Me.Button2)
        Me.Panel2.Controls.Add(Me.Button3)
        Me.Panel2.Controls.Add(Me.ListBox2)
        Me.Panel2.Controls.Add(Me.Label7)
        Me.Panel2.Controls.Add(Me.ListBox1)
        Me.Panel2.Controls.Add(Me.Button1)
        Me.Panel2.Controls.Add(Me.TextBox2)
        Me.Panel2.Controls.Add(Me.TextBox1)
        Me.Panel2.Controls.Add(Me.Label2)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Location = New System.Drawing.Point(8, 6)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(402, 369)
        Me.Panel2.TabIndex = 0
        '
        'TextBox3
        '
        Me.TextBox3.Location = New System.Drawing.Point(76, 101)
        Me.TextBox3.Name = "TextBox3"
        Me.TextBox3.ReadOnly = True
        Me.TextBox3.Size = New System.Drawing.Size(71, 21)
        Me.TextBox3.TabIndex = 19
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(13, 106)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(31, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Port:"
        '
        'Button2
        '
        Me.Button2.Location = New System.Drawing.Point(322, 249)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(75, 23)
        Me.Button2.TabIndex = 17
        Me.Button2.Text = "Stop"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'Button3
        '
        Me.Button3.Location = New System.Drawing.Point(322, 194)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(75, 23)
        Me.Button3.TabIndex = 16
        Me.Button3.Text = "Start"
        Me.Button3.UseVisualStyleBackColor = True
        '
        'ListBox2
        '
        Me.ListBox2.FormattingEnabled = True
        Me.ListBox2.Location = New System.Drawing.Point(16, 294)
        Me.ListBox2.Name = "ListBox2"
        Me.ListBox2.Size = New System.Drawing.Size(268, 56)
        Me.ListBox2.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(10, 271)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(65, 13)
        Me.Label7.TabIndex = 14
        Me.Label7.Text = "Open Ports:"
        '
        'ListBox1
        '
        Me.ListBox1.FormattingEnabled = True
        Me.ListBox1.Location = New System.Drawing.Point(13, 164)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(268, 95)
        Me.ListBox1.TabIndex = 13
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(322, 28)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 23)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Check IP"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'TextBox2
        '
        Me.TextBox2.Location = New System.Drawing.Point(76, 67)
        Me.TextBox2.Name = "TextBox2"
        Me.TextBox2.Size = New System.Drawing.Size(208, 21)
        Me.TextBox2.TabIndex = 3
        '
        'TextBox1
        '
        Me.TextBox1.Location = New System.Drawing.Point(76, 31)
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(208, 21)
        Me.TextBox1.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(10, 70)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(60, 13)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Ip Adress :"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(66, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Host Name :"
        '
        'Timer1
        '
        Me.Timer1.Interval = 10
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(10, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(122, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "Ip Finder | Port Scanner"
        '
        'Settings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(443, 433)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.Panel1)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.HelpButton = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Settings"
        Me.HelpProvider1.SetShowHelp(Me, True)
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Settings"
        Me.cmSearch.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.TabPage7.ResumeLayout(False)
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.TabPage3.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.TabControl1.ResumeLayout(False)
        Me.TabPage2.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents cmSearch As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents cmSearchRemove As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents HelpProvider1 As System.Windows.Forms.HelpProvider
    Friend WithEvents TabPage7 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox6 As System.Windows.Forms.GroupBox
    Friend WithEvents chkPhishing As System.Windows.Forms.CheckBox
    Friend WithEvents btnRemoveAllPhish As System.Windows.Forms.Button
    Friend WithEvents btnRemovePhish As System.Windows.Forms.Button
    Friend WithEvents Label11 As System.Windows.Forms.Label
    Friend WithEvents Label12 As System.Windows.Forms.Label
    Friend WithEvents lbPhishing As System.Windows.Forms.ListBox
    Friend WithEvents btnAddPhish As System.Windows.Forms.Button
    Friend WithEvents txtPhishing As System.Windows.Forms.TextBox
    Friend WithEvents TabPage3 As System.Windows.Forms.TabPage
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents btnBlockRemoveAll As System.Windows.Forms.Button
    Friend WithEvents btnRemoveBlock As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lbBlocked As System.Windows.Forms.ListBox
    Friend WithEvents btnAddBlock As System.Windows.Forms.Button
    Friend WithEvents txtBlock As System.Windows.Forms.TextBox
    Friend WithEvents TabPage1 As System.Windows.Forms.TabPage
    Friend WithEvents chkAllowPop As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox4 As System.Windows.Forms.GroupBox
    Friend WithEvents btnPopRemoveAll As System.Windows.Forms.Button
    Friend WithEvents btnPopRemove As System.Windows.Forms.Button
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents lbPop As System.Windows.Forms.ListBox
    Friend WithEvents btnPopAdd As System.Windows.Forms.Button
    Friend WithEvents txtPop As System.Windows.Forms.TextBox
    Friend WithEvents GroupBox3 As System.Windows.Forms.GroupBox
    Friend WithEvents chkPopInfo As System.Windows.Forms.CheckBox
    Friend WithEvents chkPopSound As System.Windows.Forms.CheckBox
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents TabPage2 As System.Windows.Forms.TabPage
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents TextBox2 As System.Windows.Forms.TextBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Timer1 As System.Windows.Forms.Timer
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents ListBox2 As System.Windows.Forms.ListBox
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents TextBox3 As System.Windows.Forms.TextBox
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
End Class
