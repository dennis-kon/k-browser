<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PrivateBrowserForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        components = New System.ComponentModel.Container()
        pnlPrivateIndicator = New Panel()
        lblPrivateIcon = New Label()
        lblPrivateMode = New Label()
        lblPrivateDescription = New Label()
        tsNavigation = New ToolStrip()
        btnBack = New ToolStripButton()
        btnForward = New ToolStripButton()
        btnRefresh = New ToolStripButton()
        btnHome = New ToolStripButton()
        txtAddress = New ToolStripTextBox()
        btnGo = New ToolStripButton()
        btnBookmark = New ToolStripButton()
        lblPrivateBadge = New ToolStripLabel()
        wvPrivate = New Microsoft.Web.WebView2.WinForms.WebView2()
        ssStatus = New StatusStrip()
        tspProgress = New ToolStripProgressBar()
        tslStatus = New ToolStripStatusLabel()
        pnlPrivateIndicator.SuspendLayout()
        tsNavigation.SuspendLayout()
        CType(wvPrivate, System.ComponentModel.ISupportInitialize).BeginInit()
        ssStatus.SuspendLayout()
        SuspendLayout()
        '
        ' pnlPrivateIndicator
        '
        pnlPrivateIndicator.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(35, Byte), Integer))
        pnlPrivateIndicator.Controls.Add(lblPrivateDescription)
        pnlPrivateIndicator.Controls.Add(lblPrivateMode)
        pnlPrivateIndicator.Controls.Add(lblPrivateIcon)
        pnlPrivateIndicator.Dock = DockStyle.Top
        pnlPrivateIndicator.Location = New System.Drawing.Point(0, 0)
        pnlPrivateIndicator.Name = "pnlPrivateIndicator"
        pnlPrivateIndicator.Padding = New Padding(16, 10, 16, 10)
        pnlPrivateIndicator.Size = New System.Drawing.Size(1200, 68)
        pnlPrivateIndicator.TabIndex = 0
        '
        ' lblPrivateIcon
        '
        lblPrivateIcon.AutoSize = True
        lblPrivateIcon.Font = New System.Drawing.Font("Segoe UI", 20.0F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblPrivateIcon.ForeColor = System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(74, Byte), Integer))
        lblPrivateIcon.Location = New System.Drawing.Point(16, 12)
        lblPrivateIcon.Name = "lblPrivateIcon"
        lblPrivateIcon.Size = New System.Drawing.Size(50, 40)
        lblPrivateIcon.TabIndex = 0
        lblPrivateIcon.Text = "🕶"
        '
        ' lblPrivateMode
        '
        lblPrivateMode.AutoSize = True
        lblPrivateMode.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
        lblPrivateMode.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        lblPrivateMode.Location = New System.Drawing.Point(72, 10)
        lblPrivateMode.Name = "lblPrivateMode"
        lblPrivateMode.Size = New System.Drawing.Size(250, 22)
        lblPrivateMode.TabIndex = 1
        lblPrivateMode.Text = "You are browsing privately"
        '
        ' lblPrivateDescription
        '
        lblPrivateDescription.AutoSize = True
        lblPrivateDescription.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
        lblPrivateDescription.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(195, Byte), Integer))
        lblPrivateDescription.Location = New System.Drawing.Point(72, 36)
        lblPrivateDescription.Name = "lblPrivateDescription"
        lblPrivateDescription.Size = New System.Drawing.Size(450, 16)
        lblPrivateDescription.TabIndex = 2
        lblPrivateDescription.Text = "Your browsing history, cookies, and site data will not be saved after you close this window."
        '
        ' tsNavigation
        '
        tsNavigation.BackColor = System.Drawing.Color.FromArgb(CType(CType(37, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(50, Byte), Integer))
        tsNavigation.Font = New System.Drawing.Font("Segoe UI", 9.0F)
        tsNavigation.GripStyle = ToolStripGripStyle.Hidden
        tsNavigation.Items.AddRange(New ToolStripItem() {btnBack, btnForward, btnRefresh, btnHome, txtAddress, btnGo, btnBookmark, lblPrivateBadge})
        tsNavigation.Location = New System.Drawing.Point(0, 68)
        tsNavigation.Name = "tsNavigation"
        tsNavigation.Padding = New Padding(4, 2, 4, 2)
        tsNavigation.Size = New System.Drawing.Size(1200, 39)
        tsNavigation.TabIndex = 1
        '
        ' btnBack
        '
        btnBack.DisplayStyle = ToolStripItemDisplayStyle.Text
        btnBack.Font = New System.Drawing.Font("Segoe UI", 11.0F)
        btnBack.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnBack.Name = "btnBack"
        btnBack.Size = New System.Drawing.Size(30, 32)
        btnBack.Text = "◀"
        btnBack.ToolTipText = "Go Back"
        '
        ' btnForward
        '
        btnForward.DisplayStyle = ToolStripItemDisplayStyle.Text
        btnForward.Font = New System.Drawing.Font("Segoe UI", 11.0F)
        btnForward.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnForward.Name = "btnForward"
        btnForward.Size = New System.Drawing.Size(30, 32)
        btnForward.Text = "▶"
        btnForward.ToolTipText = "Go Forward"
        '
        ' btnRefresh
        '
        btnRefresh.DisplayStyle = ToolStripItemDisplayStyle.Text
        btnRefresh.Font = New System.Drawing.Font("Segoe UI", 11.0F)
        btnRefresh.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New System.Drawing.Size(30, 32)
        btnRefresh.Text = "🔄"
        btnRefresh.ToolTipText = "Refresh"
        '
        ' btnHome
        '
        btnHome.DisplayStyle = ToolStripItemDisplayStyle.Text
        btnHome.Font = New System.Drawing.Font("Segoe UI", 11.0F)
        btnHome.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        btnHome.Name = "btnHome"
        btnHome.Size = New System.Drawing.Size(30, 32)
        btnHome.Text = "🏠"
        btnHome.ToolTipText = "Home Page"
        '
        ' txtAddress
        '
        txtAddress.AutoCompleteMode = AutoCompleteMode.SuggestAppend
        txtAddress.AutoCompleteSource = AutoCompleteSource.AllUrl
        txtAddress.BackColor = System.Drawing.Color.FromArgb(CType(CType(24, Byte), Integer), CType(CType(30, Byte), Integer), CType(CType(44, Byte), Integer))
        txtAddress.BorderStyle = BorderStyle.FixedSingle
        txtAddress.Font = New System.Drawing.Font("Segoe UI", 9.75F)
        txtAddress.ForeColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(245, Byte), Integer))
        txtAddress.Name = "txtAddress"
        txtAddress.Size = New System.Drawing.Size(800, 32)
        '
        ' btnGo
        '
        btnGo.DisplayStyle = ToolStripItemDisplayStyle.Text
        btnGo.Font = New System.Drawing.Font("Segoe UI", 10.0F, System.Drawing.FontStyle.Bold)
        btnGo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(74, Byte), Integer))
        btnGo.Name = "btnGo"
        btnGo.Size = New System.Drawing.Size(30, 32)
        btnGo.Text = "→"
        btnGo.ToolTipText = "Navigate"
        '
        ' btnBookmark
        '
        btnBookmark.DisplayStyle = ToolStripItemDisplayStyle.Text
        btnBookmark.Font = New System.Drawing.Font("Segoe UI", 10.0F)
        btnBookmark.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(195, Byte), Integer))
        btnBookmark.Name = "btnBookmark"
        btnBookmark.Size = New System.Drawing.Size(30, 32)
        btnBookmark.Text = "⭐"
        btnBookmark.ToolTipText = "Bookmark (disabled in Private Mode)"
        '
        ' lblPrivateBadge
        '
        lblPrivateBadge.Alignment = ToolStripItemAlignment.Right
        lblPrivateBadge.Font = New System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold)
        lblPrivateBadge.ForeColor = System.Drawing.Color.FromArgb(CType(CType(161, Byte), Integer), CType(CType(197, Byte), Integer), CType(CType(74, Byte), Integer))
        lblPrivateBadge.Name = "lblPrivateBadge"
        lblPrivateBadge.Size = New System.Drawing.Size(90, 32)
        lblPrivateBadge.Text = "🕶 Private Mode"
        '
        ' wvPrivate
        '
        wvPrivate.AllowExternalDrop = True
        wvPrivate.CreationProperties = Nothing
        wvPrivate.DefaultBackgroundColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        wvPrivate.Dock = DockStyle.Fill
        wvPrivate.Location = New System.Drawing.Point(0, 107)
        wvPrivate.Name = "wvPrivate"
        wvPrivate.Size = New System.Drawing.Size(1200, 586)
        wvPrivate.TabIndex = 2
        wvPrivate.ZoomFactor = 1.0R
        '
        ' ssStatus
        '
        ssStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(25, Byte), Integer), CType(CType(35, Byte), Integer))
        ssStatus.Items.AddRange(New ToolStripItem() {tspProgress, tslStatus})
        ssStatus.Location = New System.Drawing.Point(0, 693)
        ssStatus.Name = "ssStatus"
        ssStatus.Size = New System.Drawing.Size(1200, 24)
        ssStatus.TabIndex = 3
        '
        ' tspProgress
        '
        tspProgress.Name = "tspProgress"
        tspProgress.Size = New System.Drawing.Size(100, 18)
        '
        ' tslStatus
        '
        tslStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(160, Byte), Integer), CType(CType(175, Byte), Integer), CType(CType(195, Byte), Integer))
        tslStatus.Name = "tslStatus"
        tslStatus.Size = New System.Drawing.Size(80, 19)
        tslStatus.Text = "Private Mode"
        '
        ' PrivateBrowserForm
        '
        AutoScaleDimensions = New System.Drawing.SizeF(7.0F, 15.0F)
        AutoScaleMode = AutoScaleMode.Font
        BackColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        ClientSize = New System.Drawing.Size(1200, 717)
        Controls.Add(wvPrivate)
        Controls.Add(ssStatus)
        Controls.Add(tsNavigation)
        Controls.Add(pnlPrivateIndicator)
        Name = "PrivateBrowserForm"
        StartPosition = FormStartPosition.CenterScreen
        Text = "K Browser - Private"
        pnlPrivateIndicator.ResumeLayout(False)
        pnlPrivateIndicator.PerformLayout()
        tsNavigation.ResumeLayout(False)
        tsNavigation.PerformLayout()
        CType(wvPrivate, System.ComponentModel.ISupportInitialize).EndInit()
        ssStatus.ResumeLayout(False)
        ssStatus.PerformLayout()
        ResumeLayout(False)
        PerformLayout()

    End Sub

    Friend WithEvents pnlPrivateIndicator As Panel
    Friend WithEvents lblPrivateIcon As Label
    Friend WithEvents lblPrivateMode As Label
    Friend WithEvents lblPrivateDescription As Label
    Friend WithEvents tsNavigation As ToolStrip
    Friend WithEvents btnBack As ToolStripButton
    Friend WithEvents btnForward As ToolStripButton
    Friend WithEvents btnRefresh As ToolStripButton
    Friend WithEvents btnHome As ToolStripButton
    Friend WithEvents txtAddress As ToolStripTextBox
    Friend WithEvents btnGo As ToolStripButton
    Friend WithEvents btnBookmark As ToolStripButton
    Friend WithEvents lblPrivateBadge As ToolStripLabel
    Friend WithEvents wvPrivate As Microsoft.Web.WebView2.WinForms.WebView2
    Friend WithEvents ssStatus As StatusStrip
    Friend WithEvents tspProgress As ToolStripProgressBar
    Friend WithEvents tslStatus As ToolStripStatusLabel

End Class
