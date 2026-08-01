<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bookmarks
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Bookmarks))
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.lblCount = New System.Windows.Forms.Label()
        Me.txtSearch = New System.Windows.Forms.TextBox()
        Me.lblSearch = New System.Windows.Forms.Label()
        Me.lblTitle = New System.Windows.Forms.Label()
        Me.PanelBottom = New System.Windows.Forms.Panel()
        Me.btnOpenNewTab = New System.Windows.Forms.Button()
        Me.btnAddFolder = New System.Windows.Forms.Button()
        Me.btnAddBookmark = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.tvBookmarks = New System.Windows.Forms.TreeView()
        Me.cmsBookmarks = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmOpenActive = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmOpenNewTab = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCopyUrl = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmAddFolder = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmAddBookmark = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmEdit = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmDelete = New System.Windows.Forms.ToolStripMenuItem()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.pnlHeader.SuspendLayout()
        Me.PanelBottom.SuspendLayout()
        Me.cmsBookmarks.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.pnlHeader.Controls.Add(Me.lblCount)
        Me.pnlHeader.Controls.Add(Me.txtSearch)
        Me.pnlHeader.Controls.Add(Me.lblSearch)
        Me.pnlHeader.Controls.Add(Me.lblTitle)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(484, 75)
        Me.pnlHeader.TabIndex = 0
        '
        'lblCount
        '
        Me.lblCount.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCount.AutoSize = True
        Me.lblCount.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCount.ForeColor = System.Drawing.Color.Gray
        Me.lblCount.Location = New System.Drawing.Point(400, 14)
        Me.lblCount.Name = "lblCount"
        Me.lblCount.Size = New System.Drawing.Size(73, 15)
        Me.lblCount.TabIndex = 3
        Me.lblCount.Text = "0 bookmarks"
        '
        'txtSearch
        '
        Me.txtSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtSearch.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSearch.Location = New System.Drawing.Point(65, 38)
        Me.txtSearch.Name = "txtSearch"
        Me.txtSearch.Size = New System.Drawing.Size(407, 24)
        Me.txtSearch.TabIndex = 2
        '
        'lblSearch
        '
        Me.lblSearch.AutoSize = True
        Me.lblSearch.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblSearch.Location = New System.Drawing.Point(12, 42)
        Me.lblSearch.Name = "lblSearch"
        Me.lblSearch.Size = New System.Drawing.Size(45, 15)
        Me.lblSearch.TabIndex = 1
        Me.lblSearch.Text = "Filter:"
        '
        'lblTitle
        '
        Me.lblTitle.AutoSize = True
        Me.lblTitle.Font = New System.Drawing.Font("Segoe UI Semibold", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(28, Byte), Integer), CType(CType(36, Byte), Integer), CType(CType(52, Byte), Integer))
        Me.lblTitle.Location = New System.Drawing.Point(11, 10)
        Me.lblTitle.Name = "lblTitle"
        Me.lblTitle.Size = New System.Drawing.Size(157, 21)
        Me.lblTitle.TabIndex = 0
        Me.lblTitle.Text = "Bookmarks Manager"
        '
        'PanelBottom
        '
        Me.PanelBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
        Me.PanelBottom.Controls.Add(Me.btnOpenNewTab)
        Me.PanelBottom.Controls.Add(Me.btnAddFolder)
        Me.PanelBottom.Controls.Add(Me.btnAddBookmark)
        Me.PanelBottom.Controls.Add(Me.btnEdit)
        Me.PanelBottom.Controls.Add(Me.btnOpen)
        Me.PanelBottom.Controls.Add(Me.btnDelete)
        Me.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelBottom.Location = New System.Drawing.Point(0, 475)
        Me.PanelBottom.Name = "PanelBottom"
        Me.PanelBottom.Size = New System.Drawing.Size(484, 51)
        Me.PanelBottom.TabIndex = 2
        '
        'btnOpenNewTab
        '
        Me.btnOpenNewTab.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpenNewTab.Location = New System.Drawing.Point(82, 11)
        Me.btnOpenNewTab.Name = "btnOpenNewTab"
        Me.btnOpenNewTab.Size = New System.Drawing.Size(74, 28)
        Me.btnOpenNewTab.TabIndex = 5
        Me.btnOpenNewTab.Text = "New Tab"
        Me.btnOpenNewTab.UseVisualStyleBackColor = True
        '
        'btnAddFolder
        '
        Me.btnAddFolder.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddFolder.Location = New System.Drawing.Point(161, 11)
        Me.btnAddFolder.Name = "btnAddFolder"
        Me.btnAddFolder.Size = New System.Drawing.Size(84, 28)
        Me.btnAddFolder.TabIndex = 2
        Me.btnAddFolder.Text = "+ Folder"
        Me.btnAddFolder.UseVisualStyleBackColor = True
        '
        'btnAddBookmark
        '
        Me.btnAddBookmark.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnAddBookmark.Location = New System.Drawing.Point(250, 11)
        Me.btnAddBookmark.Name = "btnAddBookmark"
        Me.btnAddBookmark.Size = New System.Drawing.Size(86, 28)
        Me.btnAddBookmark.TabIndex = 3
        Me.btnAddBookmark.Text = "+ Website"
        Me.btnAddBookmark.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnEdit.Location = New System.Drawing.Point(341, 11)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(60, 28)
        Me.btnEdit.TabIndex = 4
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'btnOpen
        '
        Me.btnOpen.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnOpen.Location = New System.Drawing.Point(11, 11)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(66, 28)
        Me.btnOpen.TabIndex = 0
        Me.btnOpen.Text = "Open"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnDelete.Location = New System.Drawing.Point(406, 11)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(66, 28)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'tvBookmarks
        '
        Me.tvBookmarks.AllowDrop = True
        Me.tvBookmarks.ContextMenuStrip = Me.cmsBookmarks
        Me.tvBookmarks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvBookmarks.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvBookmarks.HideSelection = False
        Me.tvBookmarks.ImageIndex = 0
        Me.tvBookmarks.ImageList = Me.ImageList1
        Me.tvBookmarks.Location = New System.Drawing.Point(0, 75)
        Me.tvBookmarks.Name = "tvBookmarks"
        Me.tvBookmarks.SelectedImageIndex = 0
        Me.tvBookmarks.ShowNodeToolTips = True
        Me.tvBookmarks.Size = New System.Drawing.Size(484, 400)
        Me.tvBookmarks.TabIndex = 1
        '
        'cmsBookmarks
        '
        Me.cmsBookmarks.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmOpenActive, Me.tsmOpenNewTab, Me.tsmCopyUrl, Me.ToolStripSeparator1, Me.tsmAddFolder, Me.tsmAddBookmark, Me.tsmEdit, Me.tsmDelete})
        Me.cmsBookmarks.Name = "cmsBookmarks"
        Me.cmsBookmarks.Size = New System.Drawing.Size(176, 164)
        '
        'tsmOpenActive
        '
        Me.tsmOpenActive.Name = "tsmOpenActive"
        Me.tsmOpenActive.Size = New System.Drawing.Size(175, 22)
        Me.tsmOpenActive.Text = "Open in Active Tab"
        '
        'tsmOpenNewTab
        '
        Me.tsmOpenNewTab.Name = "tsmOpenNewTab"
        Me.tsmOpenNewTab.Size = New System.Drawing.Size(175, 22)
        Me.tsmOpenNewTab.Text = "Open in New Tab"
        '
        'tsmCopyUrl
        '
        Me.tsmCopyUrl.Name = "tsmCopyUrl"
        Me.tsmCopyUrl.Size = New System.Drawing.Size(175, 22)
        Me.tsmCopyUrl.Text = "Copy URL"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(172, 6)
        '
        'tsmAddFolder
        '
        Me.tsmAddFolder.Name = "tsmAddFolder"
        Me.tsmAddFolder.Size = New System.Drawing.Size(175, 22)
        Me.tsmAddFolder.Text = "+ Add Folder"
        '
        'tsmAddBookmark
        '
        Me.tsmAddBookmark.Name = "tsmAddBookmark"
        Me.tsmAddBookmark.Size = New System.Drawing.Size(175, 22)
        Me.tsmAddBookmark.Text = "+ Add Website"
        '
        'tsmEdit
        '
        Me.tsmEdit.Name = "tsmEdit"
        Me.tsmEdit.Size = New System.Drawing.Size(175, 22)
        Me.tsmEdit.Text = "Edit"
        '
        'tsmDelete
        '
        Me.tsmDelete.Name = "tsmDelete"
        Me.tsmDelete.Size = New System.Drawing.Size(175, 22)
        Me.tsmDelete.Text = "Delete"
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'Bookmarks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(484, 526)
        Me.Controls.Add(Me.tvBookmarks)
        Me.Controls.Add(Me.PanelBottom)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(420, 360)
        Me.Name = "Bookmarks"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bookmarks Manager"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.PanelBottom.ResumeLayout(False)
        Me.cmsBookmarks.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As System.Windows.Forms.Panel
    Friend WithEvents lblTitle As System.Windows.Forms.Label
    Friend WithEvents lblSearch As System.Windows.Forms.Label
    Friend WithEvents txtSearch As System.Windows.Forms.TextBox
    Friend WithEvents lblCount As System.Windows.Forms.Label
    Friend WithEvents PanelBottom As System.Windows.Forms.Panel
    Friend WithEvents btnAddFolder As System.Windows.Forms.Button
    Friend WithEvents btnAddBookmark As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnOpenNewTab As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents tvBookmarks As System.Windows.Forms.TreeView
    Friend WithEvents cmsBookmarks As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents tsmOpenActive As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmOpenNewTab As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmCopyUrl As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tsmAddFolder As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmAddBookmark As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmEdit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsmDelete As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList

End Class
