<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SavePasswordForm
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
        lblMessage = New Label()
        lblDomain = New Label()
        lblUsername = New Label()
        pnlButtons = New Panel()
        btnSave = New Button()
        btnNever = New Button()
        btnNotNow = New Button()
        pnlHeader.SuspendLayout()
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
        pnlHeader.Size = New System.Drawing.Size(380, 50)
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
        lblIcon.Text = "🔑"
        '
        ' lblTitle
        '
        lblTitle.AutoSize = True
        lblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0F, System.Drawing.FontStyle.Bold)
        lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        lblTitle.Location = New System.Drawing.Point(54, 14)
        lblTitle.Name = "lblTitle"
        lblTitle.Size = New System.Drawing.Size(130, 20)
        lblTitle.TabIndex = 1
        lblTitle.Text = "Save Password?"
        '
        ' lblMessage
        '
        lblMessage.AutoSize = True
        lblMessage.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        lblMessage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(195, Byte), Integer))
        lblMessage.Location = New System.Drawing.Point(18, 65)
        lblMessage.Name = "lblMessage"
        lblMessage.Size = New System.Drawing.Size(300, 16)
        lblMessage.TabIndex = 1
        lblMessage.Text = "Would you like K Browser to save the password for:"
        '
        ' lblDomain
        '
        lblDomain.AutoSize = True
        lblDomain.Font = New System.Drawing.Font("Segoe UI Semibold", 11.0F, System.Drawing.FontStyle.Bold)
        lblDomain.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        lblDomain.Location = New System.Drawing.Point(18, 90)
        lblDomain.Name = "lblDomain"
        lblDomain.Size = New System.Drawing.Size(100, 20)
        lblDomain.TabIndex = 2
        lblDomain.Text = "example.com"
        '
        ' lblUsername
        '
        lblUsername.AutoSize = True
        lblUsername.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        lblUsername.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(195, Byte), Integer))
        lblUsername.Location = New System.Drawing.Point(18, 116)
        lblUsername.Name = "lblUsername"
        lblUsername.Size = New System.Drawing.Size(100, 16)
        lblUsername.TabIndex = 3
        lblUsername.Text = "user@example.com"
        '
        ' pnlButtons
        '
        pnlButtons.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(47, Byte), Integer), CType(CType(67, Byte), Integer))
        pnlButtons.Controls.Add(btnSave)
        pnlButtons.Controls.Add(btnNever)
        pnlButtons.Controls.Add(btnNotNow)
        pnlButtons.Dock = DockStyle.Bottom
        pnlButtons.Location = New System.Drawing.Point(0, 148)
        pnlButtons.Name = "pnlButtons"
        pnlButtons.Padding = New Padding(10, 8, 10, 8)
        pnlButtons.Size = New System.Drawing.Size(380, 52)
        pnlButtons.TabIndex = 4
        '
        ' btnSave
        '
        btnSave.BackColor = System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(74, Byte), Integer))
        btnSave.FlatAppearance.BorderSize = 0
        btnSave.FlatStyle = FlatStyle.Flat
        btnSave.Font = New System.Drawing.Font("Segoe UI", 9.0F, System.Drawing.FontStyle.Bold)
        btnSave.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        btnSave.Location = New System.Drawing.Point(12, 10)
        btnSave.Name = "btnSave"
        btnSave.Size = New System.Drawing.Size(100, 32)
        btnSave.TabIndex = 0
        btnSave.Text = "Save"
        btnSave.UseVisualStyleBackColor = False
        '
        ' btnNever
        '
        btnNever.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(85, Byte), Integer))
        btnNever.FlatStyle = FlatStyle.Flat
        btnNever.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        btnNever.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnNever.Location = New System.Drawing.Point(140, 10)
        btnNever.Name = "btnNever"
        btnNever.Size = New System.Drawing.Size(100, 32)
        btnNever.TabIndex = 1
        btnNever.Text = "Never"
        '
        ' btnNotNow
        '
        btnNotNow.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(62, Byte), Integer), CType(CType(85, Byte), Integer))
        btnNotNow.FlatStyle = FlatStyle.Flat
        btnNotNow.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        btnNotNow.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnNotNow.Location = New System.Drawing.Point(268, 10)
        btnNotNow.Name = "btnNotNow"
        btnNotNow.Size = New System.Drawing.Size(100, 32)
        btnNotNow.TabIndex = 2
        btnNotNow.Text = "Not Now"
        '
        ' SavePasswordForm
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        ClientSize = New System.Drawing.Size(380, 200)
        Controls.Add(lblUsername)
        Controls.Add(lblDomain)
        Controls.Add(lblMessage)
        Controls.Add(pnlButtons)
        Controls.Add(pnlHeader)
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        Name = "SavePasswordForm"
        StartPosition = FormStartPosition.CenterParent
        Text = "Save Password?"
        pnlHeader.ResumeLayout(False)
        pnlHeader.PerformLayout()
        pnlButtons.ResumeLayout(False)
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents lblIcon As Label
    Friend WithEvents lblTitle As Label
    Friend WithEvents lblMessage As Label
    Friend WithEvents lblDomain As Label
    Friend WithEvents lblUsername As Label
    Friend WithEvents pnlButtons As Panel
    Friend WithEvents btnSave As Button
    Friend WithEvents btnNever As Button
    Friend WithEvents btnNotNow As Button

End Class
