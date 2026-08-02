<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AdBlockPopup
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
        Me.components = New System.ComponentModel.Container()
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblShield = New System.Windows.Forms.Label()
        Me.lblSiteDomain = New System.Windows.Forms.Label()
        Me.lblFullUrl = New System.Windows.Forms.Label()
        Me.chkDisableOnSite = New System.Windows.Forms.CheckBox()
        Me.lblStatusText = New System.Windows.Forms.Label()
        Me.pnlStats = New System.Windows.Forms.Panel()
        Me.pnlBoxPage = New System.Windows.Forms.Panel()
        Me.lblCountPage = New System.Windows.Forms.Label()
        Me.lblTitlePage = New System.Windows.Forms.Label()
        Me.pnlBoxTotal = New System.Windows.Forms.Panel()
        Me.lblCountTotal = New System.Windows.Forms.Label()
        Me.lblTitleTotal = New System.Windows.Forms.Label()
        Me.pnlGridContainer = New System.Windows.Forms.Panel()
        Me.lblGridHeader = New System.Windows.Forms.Label()
        Me.dgvBlockedTrackers = New System.Windows.Forms.DataGridView()
        Me.colIndex = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colUrl = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.colTime = New System.Windows.Forms.DataGridViewTextBoxColumn()
        Me.pnlFooter = New System.Windows.Forms.Panel()
        Me.btnAdBlockSettings = New System.Windows.Forms.Button()
        Me.btnReload = New System.Windows.Forms.Button()
        Me.btnClose = New System.Windows.Forms.Button()
        Me.TimerRefresh = New System.Windows.Forms.Timer(Me.components)
        Me.pnlHeader.SuspendLayout()
        Me.pnlStats.SuspendLayout()
        Me.pnlBoxPage.SuspendLayout()
        Me.pnlBoxTotal.SuspendLayout()
        Me.pnlGridContainer.SuspendLayout()
        CType(Me.dgvBlockedTrackers, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFooter.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.Controls.Add(Me.lblShield)
        Me.pnlHeader.Controls.Add(Me.lblSiteDomain)
        Me.pnlHeader.Controls.Add(Me.lblFullUrl)
        Me.pnlHeader.Controls.Add(Me.chkDisableOnSite)
        Me.pnlHeader.Controls.Add(Me.lblStatusText)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(564, 105)
        Me.pnlHeader.TabIndex = 0
        '
        'lblShield
        '
        Me.lblShield.AutoSize = True
        Me.lblShield.Font = New System.Drawing.Font("Segoe UI Emoji", 20.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShield.Location = New System.Drawing.Point(12, 12)
        Me.lblShield.Name = "lblShield"
        Me.lblShield.Size = New System.Drawing.Size(53, 36)
        Me.lblShield.TabIndex = 0
        Me.lblShield.Text = "🛡️"
        '
        'lblSiteDomain
        '
        Me.lblSiteDomain.AutoSize = True
        Me.lblSiteDomain.Font = New System.Drawing.Font("Segoe UI", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSiteDomain.Location = New System.Drawing.Point(62, 12)
        Me.lblSiteDomain.Name = "lblSiteDomain"
        Me.lblSiteDomain.Size = New System.Drawing.Size(102, 20)
        Me.lblSiteDomain.TabIndex = 1
        Me.lblSiteDomain.Text = "example.com"
        '
        'lblFullUrl
        '
        Me.lblFullUrl.AutoEllipsis = True
        Me.lblFullUrl.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblFullUrl.ForeColor = System.Drawing.Color.Gray
        Me.lblFullUrl.Location = New System.Drawing.Point(63, 34)
        Me.lblFullUrl.Name = "lblFullUrl"
        Me.lblFullUrl.Size = New System.Drawing.Size(490, 18)
        Me.lblFullUrl.TabIndex = 2
        Me.lblFullUrl.Text = "https://example.com/page"
        '
        'chkDisableOnSite
        '
        Me.chkDisableOnSite.AutoSize = True
        Me.chkDisableOnSite.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkDisableOnSite.Location = New System.Drawing.Point(61, 56)
        Me.chkDisableOnSite.Name = "chkDisableOnSite"
        Me.chkDisableOnSite.Size = New System.Drawing.Size(251, 19)
        Me.chkDisableOnSite.TabIndex = 3
        Me.chkDisableOnSite.Text = "Disable Ads & Trackers Blocker on this site"
        Me.chkDisableOnSite.UseVisualStyleBackColor = True
        '
        'lblStatusText
        '
        Me.lblStatusText.AutoSize = True
        Me.lblStatusText.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatusText.ForeColor = System.Drawing.Color.Green
        Me.lblStatusText.Location = New System.Drawing.Point(58, 80)
        Me.lblStatusText.Name = "lblStatusText"
        Me.lblStatusText.Size = New System.Drawing.Size(192, 15)
        Me.lblStatusText.TabIndex = 4
        Me.lblStatusText.Text = "Status: Active protection on this site"
        '
        'pnlStats
        '
        Me.pnlStats.Controls.Add(Me.pnlBoxPage)
        Me.pnlStats.Controls.Add(Me.pnlBoxTotal)
        Me.pnlStats.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlStats.Location = New System.Drawing.Point(0, 105)
        Me.pnlStats.Name = "pnlStats"
        Me.pnlStats.Size = New System.Drawing.Size(564, 75)
        Me.pnlStats.TabIndex = 1
        '
        'pnlBoxPage
        '
        Me.pnlBoxPage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBoxPage.Controls.Add(Me.lblCountPage)
        Me.pnlBoxPage.Controls.Add(Me.lblTitlePage)
        Me.pnlBoxPage.Location = New System.Drawing.Point(16, 6)
        Me.pnlBoxPage.Name = "pnlBoxPage"
        Me.pnlBoxPage.Size = New System.Drawing.Size(255, 62)
        Me.pnlBoxPage.TabIndex = 0
        '
        'lblCountPage
        '
        Me.lblCountPage.AutoSize = True
        Me.lblCountPage.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountPage.ForeColor = System.Drawing.Color.FromArgb(CType(CType(200, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(30, Byte), Integer))
        Me.lblCountPage.Location = New System.Drawing.Point(10, 4)
        Me.lblCountPage.Name = "lblCountPage"
        Me.lblCountPage.Size = New System.Drawing.Size(28, 32)
        Me.lblCountPage.TabIndex = 0
        Me.lblCountPage.Text = "0"
        '
        'lblTitlePage
        '
        Me.lblTitlePage.AutoSize = True
        Me.lblTitlePage.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitlePage.ForeColor = System.Drawing.Color.Gray
        Me.lblTitlePage.Location = New System.Drawing.Point(12, 38)
        Me.lblTitlePage.Name = "lblTitlePage"
        Me.lblTitlePage.Size = New System.Drawing.Size(122, 15)
        Me.lblTitlePage.TabIndex = 1
        Me.lblTitlePage.Text = "Blocked on this page"
        '
        'pnlBoxTotal
        '
        Me.pnlBoxTotal.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlBoxTotal.Controls.Add(Me.lblCountTotal)
        Me.pnlBoxTotal.Controls.Add(Me.lblTitleTotal)
        Me.pnlBoxTotal.Location = New System.Drawing.Point(288, 6)
        Me.pnlBoxTotal.Name = "pnlBoxTotal"
        Me.pnlBoxTotal.Size = New System.Drawing.Size(260, 62)
        Me.pnlBoxTotal.TabIndex = 1
        '
        'lblCountTotal
        '
        Me.lblCountTotal.AutoSize = True
        Me.lblCountTotal.Font = New System.Drawing.Font("Segoe UI", 18.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCountTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(120, Byte), Integer), CType(CType(200, Byte), Integer))
        Me.lblCountTotal.Location = New System.Drawing.Point(10, 4)
        Me.lblCountTotal.Name = "lblCountTotal"
        Me.lblCountTotal.Size = New System.Drawing.Size(28, 32)
        Me.lblCountTotal.TabIndex = 0
        Me.lblCountTotal.Text = "0"
        '
        'lblTitleTotal
        '
        Me.lblTitleTotal.AutoSize = True
        Me.lblTitleTotal.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitleTotal.ForeColor = System.Drawing.Color.Gray
        Me.lblTitleTotal.Location = New System.Drawing.Point(12, 38)
        Me.lblTitleTotal.Name = "lblTitleTotal"
        Me.lblTitleTotal.Size = New System.Drawing.Size(94, 15)
        Me.lblTitleTotal.TabIndex = 1
        Me.lblTitleTotal.Text = "Blocked in total"
        '
        'pnlGridContainer
        '
        Me.pnlGridContainer.Controls.Add(Me.lblGridHeader)
        Me.pnlGridContainer.Controls.Add(Me.dgvBlockedTrackers)
        Me.pnlGridContainer.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlGridContainer.Location = New System.Drawing.Point(0, 180)
        Me.pnlGridContainer.Name = "pnlGridContainer"
        Me.pnlGridContainer.Padding = New System.Windows.Forms.Padding(16, 25, 16, 8)
        Me.pnlGridContainer.Size = New System.Drawing.Size(564, 230)
        Me.pnlGridContainer.TabIndex = 2
        '
        'lblGridHeader
        '
        Me.lblGridHeader.AutoSize = True
        Me.lblGridHeader.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGridHeader.Location = New System.Drawing.Point(16, 6)
        Me.lblGridHeader.Name = "lblGridHeader"
        Me.lblGridHeader.Size = New System.Drawing.Size(209, 15)
        Me.lblGridHeader.TabIndex = 0
        Me.lblGridHeader.Text = "Blocked Trackers & Ads URLs on Page:"
        '
        'dgvBlockedTrackers
        '
        Me.dgvBlockedTrackers.AllowUserToAddRows = False
        Me.dgvBlockedTrackers.AllowUserToDeleteRows = False
        Me.dgvBlockedTrackers.BackgroundColor = System.Drawing.Color.White
        Me.dgvBlockedTrackers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvBlockedTrackers.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colIndex, Me.colUrl, Me.colTime})
        Me.dgvBlockedTrackers.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvBlockedTrackers.Location = New System.Drawing.Point(16, 25)
        Me.dgvBlockedTrackers.MultiSelect = False
        Me.dgvBlockedTrackers.Name = "dgvBlockedTrackers"
        Me.dgvBlockedTrackers.ReadOnly = True
        Me.dgvBlockedTrackers.RowHeadersVisible = False
        Me.dgvBlockedTrackers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvBlockedTrackers.Size = New System.Drawing.Size(532, 197)
        Me.dgvBlockedTrackers.TabIndex = 1
        '
        'colIndex
        '
        Me.colIndex.HeaderText = "#"
        Me.colIndex.Name = "colIndex"
        Me.colIndex.ReadOnly = True
        Me.colIndex.Width = 35
        '
        'colUrl
        '
        Me.colUrl.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
        Me.colUrl.HeaderText = "Blocked Tracker / Ad URL"
        Me.colUrl.Name = "colUrl"
        Me.colUrl.ReadOnly = True
        '
        'colTime
        '
        Me.colTime.HeaderText = "Time"
        Me.colTime.Name = "colTime"
        Me.colTime.ReadOnly = True
        Me.colTime.Width = 75
        '
        'pnlFooter
        '
        Me.pnlFooter.Controls.Add(Me.btnAdBlockSettings)
        Me.pnlFooter.Controls.Add(Me.btnReload)
        Me.pnlFooter.Controls.Add(Me.btnClose)
        Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.pnlFooter.Location = New System.Drawing.Point(0, 410)
        Me.pnlFooter.Name = "pnlFooter"
        Me.pnlFooter.Size = New System.Drawing.Size(564, 45)
        Me.pnlFooter.TabIndex = 3
        '
        'btnAdBlockSettings
        '
        Me.btnAdBlockSettings.Location = New System.Drawing.Point(16, 8)
        Me.btnAdBlockSettings.Name = "btnAdBlockSettings"
        Me.btnAdBlockSettings.Size = New System.Drawing.Size(160, 28)
        Me.btnAdBlockSettings.TabIndex = 0
        Me.btnAdBlockSettings.Text = "⚙️ AdBlocker Settings..."
        Me.btnAdBlockSettings.UseVisualStyleBackColor = True
        '
        'btnReload
        '
        Me.btnReload.Location = New System.Drawing.Point(185, 8)
        Me.btnReload.Name = "btnReload"
        Me.btnReload.Size = New System.Drawing.Size(120, 28)
        Me.btnReload.TabIndex = 1
        Me.btnReload.Text = "↻ Reload Page"
        Me.btnReload.UseVisualStyleBackColor = True
        '
        'btnClose
        '
        Me.btnClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnClose.Location = New System.Drawing.Point(448, 8)
        Me.btnClose.Name = "btnClose"
        Me.btnClose.Size = New System.Drawing.Size(100, 28)
        Me.btnClose.TabIndex = 2
        Me.btnClose.Text = "Close"
        Me.btnClose.UseVisualStyleBackColor = True
        '
        'TimerRefresh
        '
        Me.TimerRefresh.Interval = 1000
        '
        'AdBlockPopup
        '
        Me.ClientSize = New System.Drawing.Size(564, 455)
        Me.Controls.Add(Me.pnlGridContainer)
        Me.Controls.Add(Me.pnlFooter)
        Me.Controls.Add(Me.pnlStats)
        Me.Controls.Add(Me.pnlHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "AdBlockPopup"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Ads & Trackers Status - K-Browser"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.pnlStats.ResumeLayout(False)
        Me.pnlBoxPage.ResumeLayout(False)
        Me.pnlBoxPage.PerformLayout()
        Me.pnlBoxTotal.ResumeLayout(False)
        Me.pnlBoxTotal.PerformLayout()
        Me.pnlGridContainer.ResumeLayout(False)
        Me.pnlGridContainer.PerformLayout()
        CType(Me.dgvBlockedTrackers, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFooter.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblShield As System.Windows.Forms.Label
    Friend WithEvents lblSiteDomain As System.Windows.Forms.Label
    Friend WithEvents lblFullUrl As System.Windows.Forms.Label
    Friend WithEvents chkDisableOnSite As System.Windows.Forms.CheckBox
    Friend WithEvents lblStatusText As System.Windows.Forms.Label
    Friend WithEvents pnlStats As System.Windows.Forms.Panel
    Friend WithEvents pnlBoxPage As System.Windows.Forms.Panel
    Friend WithEvents lblCountPage As System.Windows.Forms.Label
    Friend WithEvents lblTitlePage As System.Windows.Forms.Label
    Friend WithEvents pnlBoxTotal As System.Windows.Forms.Panel
    Friend WithEvents lblCountTotal As System.Windows.Forms.Label
    Friend WithEvents lblTitleTotal As System.Windows.Forms.Label
    Friend WithEvents pnlGridContainer As System.Windows.Forms.Panel
    Friend WithEvents lblGridHeader As System.Windows.Forms.Label
    Friend WithEvents dgvBlockedTrackers As System.Windows.Forms.DataGridView
    Friend WithEvents colIndex As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colUrl As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents colTime As System.Windows.Forms.DataGridViewTextBoxColumn
    Friend WithEvents pnlFooter As System.Windows.Forms.Panel
    Friend WithEvents btnAdBlockSettings As System.Windows.Forms.Button
    Friend WithEvents btnReload As System.Windows.Forms.Button
    Friend WithEvents btnClose As System.Windows.Forms.Button
    Friend WithEvents TimerRefresh As System.Windows.Forms.Timer
End Class
