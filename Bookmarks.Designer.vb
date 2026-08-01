<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Bookmarks
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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Bookmarks))
        Me.tvBookmarks = New System.Windows.Forms.TreeView()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PanelTop = New System.Windows.Forms.Panel()
        Me.btnAddFolder = New System.Windows.Forms.Button()
        Me.btnAddBookmark = New System.Windows.Forms.Button()
        Me.btnEdit = New System.Windows.Forms.Button()
        Me.PanelBottom = New System.Windows.Forms.Panel()
        Me.btnOpen = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.PanelTop.SuspendLayout()
        Me.PanelBottom.SuspendLayout()
        Me.SuspendLayout()
        '
        'tvBookmarks
        '
        Me.tvBookmarks.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvBookmarks.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tvBookmarks.HideSelection = False
        Me.tvBookmarks.ImageIndex = 0
        Me.tvBookmarks.ImageList = Me.ImageList1
        Me.tvBookmarks.Location = New System.Drawing.Point(0, 40)
        Me.tvBookmarks.Name = "tvBookmarks"
        Me.tvBookmarks.SelectedImageIndex = 0
        Me.tvBookmarks.Size = New System.Drawing.Size(340, 395)
        Me.tvBookmarks.TabIndex = 0
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'PanelTop
        '
        Me.PanelTop.Controls.Add(Me.btnAddFolder)
        Me.PanelTop.Controls.Add(Me.btnAddBookmark)
        Me.PanelTop.Controls.Add(Me.btnEdit)
        Me.PanelTop.Dock = System.Windows.Forms.DockStyle.Top
        Me.PanelTop.Location = New System.Drawing.Point(0, 0)
        Me.PanelTop.Name = "PanelTop"
        Me.PanelTop.Size = New System.Drawing.Size(340, 40)
        Me.PanelTop.TabIndex = 1
        '
        'btnAddFolder
        '
        Me.btnAddFolder.Location = New System.Drawing.Point(8, 7)
        Me.btnAddFolder.Name = "btnAddFolder"
        Me.btnAddFolder.Size = New System.Drawing.Size(95, 26)
        Me.btnAddFolder.TabIndex = 0
        Me.btnAddFolder.Text = "+ New Folder"
        Me.btnAddFolder.UseVisualStyleBackColor = True
        '
        'btnAddBookmark
        '
        Me.btnAddBookmark.Location = New System.Drawing.Point(109, 7)
        Me.btnAddBookmark.Name = "btnAddBookmark"
        Me.btnAddBookmark.Size = New System.Drawing.Size(100, 26)
        Me.btnAddBookmark.TabIndex = 1
        Me.btnAddBookmark.Text = "+ Add Website"
        Me.btnAddBookmark.UseVisualStyleBackColor = True
        '
        'btnEdit
        '
        Me.btnEdit.Location = New System.Drawing.Point(215, 7)
        Me.btnEdit.Name = "btnEdit"
        Me.btnEdit.Size = New System.Drawing.Size(75, 26)
        Me.btnEdit.TabIndex = 2
        Me.btnEdit.Text = "Edit"
        Me.btnEdit.UseVisualStyleBackColor = True
        '
        'PanelBottom
        '
        Me.PanelBottom.Controls.Add(Me.btnOpen)
        Me.PanelBottom.Controls.Add(Me.btnDelete)
        Me.PanelBottom.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.PanelBottom.Location = New System.Drawing.Point(0, 435)
        Me.PanelBottom.Name = "PanelBottom"
        Me.PanelBottom.Size = New System.Drawing.Size(340, 45)
        Me.PanelBottom.TabIndex = 2
        '
        'btnOpen
        '
        Me.btnOpen.Location = New System.Drawing.Point(12, 9)
        Me.btnOpen.Name = "btnOpen"
        Me.btnOpen.Size = New System.Drawing.Size(100, 28)
        Me.btnOpen.TabIndex = 0
        Me.btnOpen.Text = "Open Page"
        Me.btnOpen.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Location = New System.Drawing.Point(228, 9)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(100, 28)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'Bookmarks
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(340, 480)
        Me.Controls.Add(Me.tvBookmarks)
        Me.Controls.Add(Me.PanelTop)
        Me.Controls.Add(Me.PanelBottom)
        Me.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Bookmarks"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Bookmarks Manager"
        Me.PanelTop.ResumeLayout(False)
        Me.PanelBottom.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents tvBookmarks As System.Windows.Forms.TreeView
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PanelTop As System.Windows.Forms.Panel
    Friend WithEvents btnAddFolder As System.Windows.Forms.Button
    Friend WithEvents btnAddBookmark As System.Windows.Forms.Button
    Friend WithEvents btnEdit As System.Windows.Forms.Button
    Friend WithEvents PanelBottom As System.Windows.Forms.Panel
    Friend WithEvents btnOpen As System.Windows.Forms.Button
    Friend WithEvents btnDelete As System.Windows.Forms.Button

End Class
