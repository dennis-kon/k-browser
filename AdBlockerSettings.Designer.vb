<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdBlockerSettings
    Inherits System.Windows.Forms.Form

    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    Private components As System.ComponentModel.IContainer

    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.chkEnableAdBlocker = New System.Windows.Forms.CheckBox()
        Me.chkShowBlockedCount = New System.Windows.Forms.CheckBox()
        Me.grpFilterLists = New System.Windows.Forms.GroupBox()
        Me.dgvFilterLists = New System.Windows.Forms.DataGridView()
        Me.colEnabled = New System.Windows.Forms.DataGridViewCheckBoxColumn()
        Me.colName = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUrl = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colLastUpdated = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.btnUpdateLists = New System.Windows.Forms.Button()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.PanelBottom = New System.Windows.Forms.Panel()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.grpFilterLists.SuspendLayout()
        CType(Me.dgvFilterLists, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.PanelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'chkEnableAdBlocker
        '
        Me.chkEnableAdBlocker.AutoSize = True
        Me.chkEnableAdBlocker.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkEnableAdBlocker.Location = New System.Drawing.Point(15, 15)
        Me.chkEnableAdBlocker.Name = "chkEnableAdBlocker"
        Me.chkEnableAdBlocker.Size = New System.Drawing.Size(190, 18)
        Me.chkEnableAdBlocker.TabIndex = 0
        Me.chkEnableAdBlocker.Text = "Enable Ad Blocker (EasyList)"
        Me.chkEnableAdBlocker.UseVisualStyleBackColor = True
        '
        'chkShowBlockedCount
        '
        Me.chkShowBlockedCount.AutoSize = True
        Me.chkShowBlockedCount.Location = New System.Drawing.Point(15, 42)
        Me.chkShowBlockedCount.Name = "chkShowBlockedCount"
        Me.chkShowBlockedCount.Size = New System.Drawing.Size(326, 17)
        Me.chkShowBlockedCount.TabIndex = 1
        Me.chkShowBlockedCount.Text = "Show number of ads blocked in icon on the navigation bar"
        Me.chkShowBlockedCount.UseVisualStyleBackColor = True
        '
        'grpFilterLists
        '
        Me.grpFilterLists.Controls.Add(Me.dgvFilterLists)
        Me.grpFilterLists.Controls.Add(Me.btnUpdateLists)
        Me.grpFilterLists.Controls.Add(Me.lblStatus)
        Me.grpFilterLists.Location = New System.Drawing.Point(12, 70)
        Me.grpFilterLists.Name = "grpFilterLists"
        Me.grpFilterLists.Size = New System.Drawing.Size(560, 280)
        Me.grpFilterLists.TabIndex = 2
        Me.grpFilterLists.TabStop = False
        Me.grpFilterLists.Text = "Filter Lists"
        '
        'dgvFilterLists
        '
        Me.dgvFilterLists.AllowUserToAddRows = False
        Me.dgvFilterLists.AllowUserToDeleteRows = False
        Me.dgvFilterLists.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvFilterLists.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvFilterLists.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEnabled, Me.colName, Me.colUrl, Me.colLastUpdated})
        Me.dgvFilterLists.Location = New System.Drawing.Point(12, 25)
        Me.dgvFilterLists.Name = "dgvFilterLists"
        Me.dgvFilterLists.RowHeadersVisible = False
        Me.dgvFilterLists.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvFilterLists.Size = New System.Drawing.Size(536, 195)
        Me.dgvFilterLists.TabIndex = 0
        '
        'colEnabled
        '
        Me.colEnabled.FillWeight = 40.0!
        Me.colEnabled.HeaderText = "Enabled"
        Me.colEnabled.Name = "colEnabled"
        '
        'colName
        '
        Me.colName.FillWeight = 90.0!
        Me.colName.HeaderText = "Filter List"
        Me.colName.Name = "colName"
        Me.colName.ReadOnly = True
        '
        'colUrl
        '
        Me.colUrl.FillWeight = 160.0!
        Me.colUrl.HeaderText = "URL"
        Me.colUrl.Name = "colUrl"
        Me.colUrl.ReadOnly = True
        '
        'colLastUpdated
        '
        Me.colLastUpdated.FillWeight = 90.0!
        Me.colLastUpdated.HeaderText = "Last Updated"
        Me.colLastUpdated.Name = "colLastUpdated"
        Me.colLastUpdated.ReadOnly = True
        '
        'btnUpdateLists
        '
        Me.btnUpdateLists.Location = New System.Drawing.Point(12, 232)
        Me.btnUpdateLists.Name = "btnUpdateLists"
        Me.btnUpdateLists.Size = New System.Drawing.Size(140, 28)
        Me.btnUpdateLists.TabIndex = 1
        Me.btnUpdateLists.Text = "Update All Lists"
        Me.btnUpdateLists.UseVisualStyleBackColor = True
        '
        'lblStatus
        '
        Me.lblStatus.AutoSize = True
        Me.lblStatus.Location = New System.Drawing.Point(165, 240)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(41, 13)
        Me.lblStatus.TabIndex = 2
        Me.lblStatus.Text = "Ready."
        '
        'PanelBottom
        '
        Me.PanelBottom.Controls.Add(Me.btnOK)
        Me.PanelBottom.Controls.Add(Me.btnCancel)
        Me.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelBottom.Location = New System.Drawing.Point(0, 360)
        Me.PanelBottom.Name = "PanelBottom"
        Me.PanelBottom.Size = New System.Drawing.Size(584, 45)
        Me.PanelBottom.TabIndex = 3
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(400, 10)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(80, 26)
        Me.btnOK.TabIndex = 0
        Me.btnOK.Text = "OK"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(490, 10)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(80, 26)
        Me.btnCancel.TabIndex = 1
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'AdBlockerSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(584, 405)
        Me.Controls.Add(Me.chkEnableAdBlocker)
        Me.Controls.Add(Me.chkShowBlockedCount)
        Me.Controls.Add(Me.grpFilterLists)
        Me.Controls.Add(Me.PanelBottom)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdBlockerSettings"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Ad Blocker Configuration"
        Me.grpFilterLists.ResumeLayout(False)
        Me.grpFilterLists.PerformLayout()
        CType(Me.dgvFilterLists, System.ComponentModel.ISupportInitialize).EndInit()
        Me.PanelBottom.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents chkEnableAdBlocker As System.Windows.Forms.CheckBox
    Friend WithEvents chkShowBlockedCount As System.Windows.Forms.CheckBox
    Friend WithEvents grpFilterLists As System.Windows.Forms.GroupBox
    Friend WithEvents dgvFilterLists As System.Windows.Forms.DataGridView
    Friend WithEvents colEnabled As System.Windows.Forms.DataGridViewCheckBoxColumn
    Friend WithEvents colName As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUrl As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colLastUpdated As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents btnUpdateLists As System.Windows.Forms.Button
    Friend WithEvents lblStatus As System.Windows.Forms.Label
    Friend WithEvents PanelBottom As System.Windows.Forms.Panel
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents btnCancel As System.Windows.Forms.Button

End Class
