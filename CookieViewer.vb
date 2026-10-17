
Imports System.IO
Public Class CookieViewer
    Private SelNode As TreeNode

    Private Sub frmCookieViewer_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        IconService.SetIcon(btnDeleteALL, FluentIcon.Delete)
        ThemeManager.ApplyTheme(Me)
        BuildTreeImages()
        LoadCookies()
    End Sub

    ''' <summary>
    ''' Tree node icon, drawn in the tree's themed text colour (call after ApplyTheme).
    ''' </summary>
    Private Sub BuildTreeImages()
        Dim size = IconService.GetIcon(FluentIcon.Cookies, 16, tvCookies.ForeColor).Size
        ImageList1.Images.Clear()
        ImageList1.ColorDepth = ColorDepth.Depth32Bit
        ImageList1.ImageSize = size
        ImageList1.Images.Add("cookie.png", IconService.GetIcon(FluentIcon.Cookies, 16, tvCookies.ForeColor))
    End Sub

    Private Sub LoadCookies()
        tvCookies.Nodes.Clear()
        Dim cookiesDir As String = Environment.GetFolderPath(Environment.SpecialFolder.Cookies)
        If Not Directory.Exists(cookiesDir) Then Return

        Dim s As String
        Dim oNode As TreeNode
        Dim oInfo As FileInfo
        For Each s In Directory.GetFiles(cookiesDir)
            If s.EndsWith(".txt") Then
                oInfo = New FileInfo(s)
                oNode = New TreeNode
                oNode.Text = oInfo.Name
                oNode.Tag = s
                oNode.ImageIndex = 0
                oNode.SelectedImageIndex = 0
                tvCookies.Nodes.Add(oNode)
            End If
        Next
    End Sub

    Private Sub ViewCookie(ByVal oNode As TreeNode)
        Try
            rtbCookies.LoadFile(oNode.Tag, RichTextBoxStreamType.PlainText)
        Catch ex As Exception

        End Try
    End Sub

    Private Sub tvCookies_AfterSelect(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tvCookies.AfterSelect
        SelNode = e.Node
        btnDelete.Enabled = True
    End Sub

    Private Sub tvCookies_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles tvCookies.Click
        If Not IsNothing(SelNode) Then
            cmCookies.Enabled = True
        Else
            cmCookies.Enabled = False
        End If
    End Sub

    Private Sub tvCookies_NodeMouseClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvCookies.NodeMouseClick
        If e.Button = MouseButtons.Left Then
            ViewCookie(e.Node)
        Else
            tvCookies.SelectedNode = e.Node
        End If
    End Sub

    Private Sub mnuDeleteCookie_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mnuDeleteCookie.Click
        DeleteCookie()
    End Sub

    Private Sub DeleteCookie()
        Try
            If Not IsNothing(SelNode) Then
                If MessageBox.Show("Are you sure you want to delete cookie" & tvCookies.SelectedNode.Text & "?", "Confirm Delete", MessageBoxButtons.YesNoCancel) = DialogResult.Yes Then
                    File.Delete(SelNode.Tag)
                    tvCookies.SelectedNode.Remove()
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Private Sub btnDeleteALL_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDeleteALL.Click
        If MessageBox.Show("Are you sure you want to delete all cookies?", "Confirm Delete", MessageBoxButtons.YesNoCancel) = DialogResult.Yes Then
            Try
                Dim cookiesDir As String = Environment.GetFolderPath(Environment.SpecialFolder.Cookies)
                If Directory.Exists(cookiesDir) Then
                    Dim s As String
                    For Each s In Directory.GetFiles(cookiesDir)
                        If s.EndsWith(".txt") Then
                            File.Delete(s)
                        End If
                    Next
                End If
                tvCookies.Nodes.Clear()
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        DeleteCookie()
    End Sub
End Class