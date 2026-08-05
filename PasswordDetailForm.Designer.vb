<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PasswordDetailForm
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        pnlHeader = New Panel()
        lblIcon = New Label()
        lblTitle = New Label()
        pnlContent = New Panel()
        lblWebsiteCaption = New Label()
        lblWebsiteValue = New Label()
        lblUsernameCaption = New Label()
        lblUsernameValue = New Label()
        lblPasswordCaption = New Label()
        txtPasswordValue = New TextBox()
        btnTogglePassword = New Button()
        lblCreatedCaption = New Label()
        lblCreatedValue = New Label()
        lblLastUsedCaption = New Label()
        lblLastUsedValue = New Label()
        pnlButtons = New Panel()
        btnDelete = New Button()
        btnClose = New Button()
        pnlHeader.SuspendLayout()
        pnlContent.SuspendLayout()
        pnlButtons.SuspendLayout()
        SuspendLayout()
        '
        ' pnlHeader
        '
        pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(67, Byte), Integer))
        pnlHeader.Controls.Add(lblIcon)
        pnlHeader.Controls.Add(lblTitle)
        pnlHeader.Dock = DockStyle.Top
        pnlHeader.Location = New System.Drawing.Point(0, 0)
        pnlHeader.Name = "pnlHeader"
        pnlHeader.Padding = New Padding(16, 10, 16, 10)
        pnlHeader.Size = New System.Drawing.Size(420, 50)
        pnlHeader.TabIndex = 0
        '
        ' lblIcon
        '
        lblIcon.AutoSize = True
        lblIcon.Font = New System.Drawing.Font("Segoe UI", 16.0F)
        lblIcon.ForeColor = System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(74, Byte), Integer))
        lblIcon.Location = New System.Drawing.Point(14, 8)
        lblIcon.Name = "lblIcon"
        lblIcon.Size = New System.Drawing.Size(36, 30)
        lblIcon.TabIndex = 0
        lblIcon.Text = "🔐"
        '
        ' lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0F, System.Drawing.FontStyle.Bold)
        lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        lblTitle.Location = New System.Drawing.Point(54, 14)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New System.Drawing.Size(140, 20)
        lblTitle.TabIndex = 1
        lblTitle.Text = "Password Details"
        '
        ' pnlContent
        '
        pnlContent.BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        pnlContent.Controls.Add(lblWebsiteCaption)
        pnlContent.Controls.Add(lblWebsiteValue)
        pnlContent.Controls.Add(lblUsernameCaption)
        pnlContent.Controls.Add(lblUsernameValue)
        pnlContent.Controls.Add(lblPasswordCaption)
        pnlContent.Controls.Add(txtPasswordValue)
        pnlContent.Controls.Add(btnTogglePassword)
        pnlContent.Controls.Add(lblCreatedCaption)
        pnlContent.Controls.Add(lblCreatedValue)
        pnlContent.Controls.Add(lblLastUsedCaption)
        pnlContent.Controls.Add(lblLastUsedValue)
        pnlContent.Dock = DockStyle.Fill
        pnlContent.Location = New System.Drawing.Point(0, 50)
        pnlContent.Name = "pnlContent"
        pnlContent.Padding = New Padding(20, 14, 20, 10)
        pnlContent.Size = New System.Drawing.Size(420, 245)
        pnlContent.TabIndex = 1
        '
        ' lblWebsiteCaption
        '
        lblWebsiteCaption.AutoSize = True
        lblWebsiteCaption.Font = New System.Drawing.Font("Segoe UI", 8.25F)
        lblWebsiteCaption.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer))
        lblWebsiteCaption.Location = New System.Drawing.Point(20, 16)
        lblWebsiteCaption.Name = "lblWebsiteCaption"
        lblWebsiteCaption.Size = New System.Drawing.Size(50, 14)
        lblWebsiteCaption.TabIndex = 0
        lblWebsiteCaption.Text = "Website"
        '
        ' lblWebsiteValue
        '
        lblWebsiteValue.AutoSize = True
        lblWebsiteValue.Font = New System.Drawing.Font("Segoe UI", 10.0F)
        lblWebsiteValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        lblWebsiteValue.Location = New System.Drawing.Point(20, 32)
        lblWebsiteValue.Name = "lblWebsiteValue"
        lblWebsiteValue.Size = New System.Drawing.Size(100, 19)
        lblWebsiteValue.TabIndex = 1
        lblWebsiteValue.Text = "github.com"
        '
        ' lblUsernameCaption
        '
        lblUsernameCaption.AutoSize = True
        lblUsernameCaption.Font = New System.Drawing.Font("Segoe UI", 8.25F)
        lblUsernameCaption.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer))
        lblUsernameCaption.Location = New System.Drawing.Point(20, 60)
        lblUsernameCaption.Name = "lblUsernameCaption"
        lblUsernameCaption.Size = New System.Drawing.Size(60, 14)
        lblUsernameCaption.TabIndex = 2
        lblUsernameCaption.Text = "Username"
        '
        ' lblUsernameValue
        '
        lblUsernameValue.AutoSize = True
        lblUsernameValue.Font = New System.Drawing.Font("Segoe UI", 10.0F)
        lblUsernameValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        lblUsernameValue.Location = New System.Drawing.Point(20, 76)
        lblUsernameValue.Name = "lblUsernameValue"
        lblUsernameValue.Size = New System.Drawing.Size(150, 19)
        lblUsernameValue.TabIndex = 3
        lblUsernameValue.Text = "user@example.com"
        '
        ' lblPasswordCaption
        '
        lblPasswordCaption.AutoSize = True
        lblPasswordCaption.Font = New System.Drawing.Font("Segoe UI", 8.25F)
        lblPasswordCaption.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer))
        lblPasswordCaption.Location = New System.Drawing.Point(20, 106)
        lblPasswordCaption.Name = "lblPasswordCaption"
        lblPasswordCaption.Size = New System.Drawing.Size(55, 14)
        lblPasswordCaption.TabIndex = 4
        lblPasswordCaption.Text = "Password"
        '
        ' txtPasswordValue
        '
        txtPasswordValue.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(67, Byte), Integer))
        txtPasswordValue.BorderStyle = BorderStyle.FixedSingle
        txtPasswordValue.Font = New System.Drawing.Font("Segoe UI", 10.0F)
        txtPasswordValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        txtPasswordValue.Location = New System.Drawing.Point(20, 122)
        txtPasswordValue.Name = "txtPasswordValue"
        txtPasswordValue.ReadOnly = True
        txtPasswordValue.Size = New System.Drawing.Size(240, 25)
        txtPasswordValue.TabIndex = 5
        txtPasswordValue.Text = "••••••••••••"
        txtPasswordValue.UseSystemPasswordChar = True
        '
        ' btnTogglePassword
        '
        btnTogglePassword.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(85, Byte), Integer))
        btnTogglePassword.FlatStyle = FlatStyle.Flat
        btnTogglePassword.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        btnTogglePassword.ForeColor = System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(74, Byte), Integer))
        btnTogglePassword.Location = New System.Drawing.Point(268, 121)
        btnTogglePassword.Name = "btnTogglePassword"
        btnTogglePassword.Size = New System.Drawing.Size(130, 27)
        btnTogglePassword.TabIndex = 6
        btnTogglePassword.Text = "👁 Show Password"
        '
        ' lblCreatedCaption
        '
        lblCreatedCaption.AutoSize = True
        lblCreatedCaption.Font = New System.Drawing.Font("Segoe UI", 8.25F)
        lblCreatedCaption.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer))
        lblCreatedCaption.Location = New System.Drawing.Point(20, 160)
        lblCreatedCaption.Name = "lblCreatedCaption"
        lblCreatedCaption.Size = New System.Drawing.Size(75, 14)
        lblCreatedCaption.TabIndex = 7
        lblCreatedCaption.Text = "Created Date"
        '
        ' lblCreatedValue
        '
        lblCreatedValue.AutoSize = True
        lblCreatedValue.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        lblCreatedValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(225, Byte), Integer))
        lblCreatedValue.Location = New System.Drawing.Point(20, 176)
        lblCreatedValue.Name = "lblCreatedValue"
        lblCreatedValue.Size = New System.Drawing.Size(80, 16)
        lblCreatedValue.TabIndex = 8
        lblCreatedValue.Text = "01 Jan 2025"
        '
        ' lblLastUsedCaption
        '
        lblLastUsedCaption.AutoSize = True
        lblLastUsedCaption.Font = New System.Drawing.Font("Segoe UI", 8.25F)
        lblLastUsedCaption.ForeColor = System.Drawing.Color.FromArgb(CType(CType(120, Byte), Integer), CType(CType(140, Byte), Integer), CType(CType(165, Byte), Integer))
        lblLastUsedCaption.Location = New System.Drawing.Point(220, 160)
        lblLastUsedCaption.Name = "lblLastUsedCaption"
        lblLastUsedCaption.Size = New System.Drawing.Size(55, 14)
        lblLastUsedCaption.TabIndex = 9
        lblLastUsedCaption.Text = "Last Used"
        '
        ' lblLastUsedValue
        '
        lblLastUsedValue.AutoSize = True
        lblLastUsedValue.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        lblLastUsedValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(210, Byte), Integer), CType(CType(225, Byte), Integer))
        lblLastUsedValue.Location = New System.Drawing.Point(220, 176)
        lblLastUsedValue.Name = "lblLastUsedValue"
        lblLastUsedValue.Size = New System.Drawing.Size(80, 16)
        lblLastUsedValue.TabIndex = 10
        lblLastUsedValue.Text = "01 Jan 2025"
        '
        ' pnlButtons
        '
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(67, Byte), Integer))
        pnlButtons.Controls.Add(btnDelete)
        pnlButtons.Controls.Add(btnClose)
        pnlButtons.Dock = DockStyle.Bottom
        pnlButtons.Location = New System.Drawing.Point(0, 295)
        pnlButtons.Name = "pnlButtons"
        pnlButtons.Padding = New Padding(10, 8, 10, 8)
        pnlButtons.Size = New System.Drawing.Size(420, 52)
        pnlButtons.TabIndex = 2
        '
        ' btnDelete
        '
        btnDelete.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
        btnDelete.FlatAppearance.BorderSize = 0
        btnDelete.FlatStyle = FlatStyle.Flat
        btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        btnDelete.ForeColor = System.Drawing.Color.White
        btnDelete.Location = New System.Drawing.Point(12, 10)
        btnDelete.Name = "btnDelete"
        btnDelete.Size = New System.Drawing.Size(130, 32)
        btnDelete.TabIndex = 0
        btnDelete.Text = "Delete Password"
        btnDelete.UseVisualStyleBackColor = False
        '
        ' btnClose
        '
        btnClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(85, Byte), Integer))
        btnClose.FlatStyle = FlatStyle.Flat
        btnClose.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        btnClose.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnClose.Location = New System.Drawing.Point(310, 10)
        btnClose.Name = "btnClose"
        btnClose.Size = New System.Drawing.Size(95, 32)
        btnClose.TabIndex = 1
        btnClose.Text = "Close"
        '
        ' PasswordDetailForm
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        ClientSize = New System.Drawing.Size(420, 347)
        Controls.Add(pnlContent)
        Controls.Add(pnlButtons)
        Controls.Add(pnlHeader)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "PasswordDetailForm"
        StartPosition = FormStartPosition.CenterParent
        Text = "Password Details"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlContent.ResumeLayout(False)
        pnlContent.PerformLayout()
        pnlButtons.ResumeLayout(False)
        ResumeLayout(False)
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblIcon As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents pnlContent As Panel
    Friend WithEvents lblWebsiteCaption As Label
    Friend WithEvents lblWebsiteValue As Label
    Friend WithEvents lblUsernameCaption As Label
    Friend WithEvents lblUsernameValue As Label
    Friend WithEvents lblPasswordCaption As Label
    Friend WithEvents txtPasswordValue As TextBox
    Friend WithEvents btnTogglePassword As Button
    Friend WithEvents lblCreatedCaption As Label
    Friend WithEvents lblCreatedValue As Label
    Friend WithEvents lblLastUsedCaption As Label
    Friend WithEvents lblLastUsedValue As Label
    Friend WithEvents pnlButtons As Panel
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnClose As Button

End Class
