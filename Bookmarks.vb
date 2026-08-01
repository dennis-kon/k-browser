Imports System.Windows.Forms

Public Class Bookmarks

    Public Class BookmarkNodeData
        Public Property IsFolder As Boolean
        Public Property Url As String = ""
    End Class

    Private Sub Bookmarks_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadTreeFromSettings()
    End Sub

    Public Sub LoadTreeFromSettings()
        tvBookmarks.Nodes.Clear()

        If My.Settings.BookmarksTreeData Is Nothing Then
            My.Settings.BookmarksTreeData = New System.Collections.Specialized.StringCollection()
        End If

        ' Migrate legacy flat Bookmarks collection if BookmarksTreeData is empty
        If My.Settings.BookmarksTreeData.Count = 0 AndAlso My.Settings.Bookmarks IsNot Nothing AndAlso My.Settings.Bookmarks.Count > 0 Then
            For Each legacyUrl As String In My.Settings.Bookmarks
                If Not String.IsNullOrWhiteSpace(legacyUrl) Then
                    My.Settings.BookmarksTreeData.Add("URL:0:" & legacyUrl & ":" & legacyUrl)
                End If
            Next
            My.Settings.Save()
        End If

        Dim stack As New System.Collections.Generic.Dictionary(Of Integer, TreeNode)()

        For Each line As String In My.Settings.BookmarksTreeData
            If String.IsNullOrWhiteSpace(line) Then Continue For
            Dim parts As String() = line.Split(":"c)
            If parts.Length < 3 Then Continue For

            Dim typeStr As String = parts(0)
            Dim depth As Integer = 0
            Integer.TryParse(parts(1), depth)

            If typeStr = "FOLDER" Then
                Dim folderName As String = parts(2)
                Dim folderNode As New TreeNode(folderName)
                folderNode.Tag = New BookmarkNodeData With {.IsFolder = True}

                If depth = 0 OrElse Not stack.ContainsKey(depth - 1) Then
                    tvBookmarks.Nodes.Add(folderNode)
                Else
                    stack(depth - 1).Nodes.Add(folderNode)
                End If
                stack(depth) = folderNode

            ElseIf typeStr = "URL" AndAlso parts.Length >= 4 Then
                Dim title As String = parts(2)
                Dim url As String = parts(3)
                Dim urlNode As New TreeNode(title)
                urlNode.Tag = New BookmarkNodeData With {.IsFolder = False, .Url = url}

                If depth = 0 OrElse Not stack.ContainsKey(depth - 1) Then
                    tvBookmarks.Nodes.Add(urlNode)
                Else
                    stack(depth - 1).Nodes.Add(urlNode)
                End If
            End If
        Next

        tvBookmarks.ExpandAll()
    End Sub

    Public Sub SaveTreeToSettings()
        If My.Settings.BookmarksTreeData Is Nothing Then
            My.Settings.BookmarksTreeData = New System.Collections.Specialized.StringCollection()
        End If
        My.Settings.BookmarksTreeData.Clear()

        For Each rootNode As TreeNode In tvBookmarks.Nodes
            SerializeNode(rootNode, 0)
        Next

        My.Settings.Save()
    End Sub

    Private Sub SerializeNode(ByVal node As TreeNode, ByVal depth As Integer)
        Dim data = TryCast(node.Tag, BookmarkNodeData)
        If data Is Nothing Then Return

        If data.IsFolder Then
            My.Settings.BookmarksTreeData.Add("FOLDER:" & depth.ToString() & ":" & node.Text)
            For Each child As TreeNode In node.Nodes
                SerializeNode(child, depth + 1)
            Next
        Else
            My.Settings.BookmarksTreeData.Add("URL:" & depth.ToString() & ":" & node.Text & ":" & data.Url)
        End If
    End Sub

    Private Sub btnAddFolder_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddFolder.Click
        Dim folderName As String = InputBox("Enter folder name:", "New Folder", "New Folder")
        If String.IsNullOrWhiteSpace(folderName) Then Return

        Dim folderNode As New TreeNode(folderName)
        folderNode.Tag = New BookmarkNodeData With {.IsFolder = True}

        Dim selectedNode = tvBookmarks.SelectedNode
        If selectedNode IsNot Nothing Then
            Dim data = TryCast(selectedNode.Tag, BookmarkNodeData)
            If data IsNot Nothing AndAlso data.IsFolder Then
                selectedNode.Nodes.Add(folderNode)
                selectedNode.Expand()
            ElseIf selectedNode.Parent IsNot Nothing Then
                selectedNode.Parent.Nodes.Add(folderNode)
            Else
                tvBookmarks.Nodes.Add(folderNode)
            End If
        Else
            tvBookmarks.Nodes.Add(folderNode)
        End If

        SaveTreeToSettings()
    End Sub

    Private Sub btnAddBookmark_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBookmark.Click
        Dim title As String = InputBox("Enter website name:", "Add Website Bookmark", "New Bookmark")
        If String.IsNullOrWhiteSpace(title) Then Return

        Dim url As String = InputBox("Enter website URL:", "Add Website Bookmark", "https://")
        If String.IsNullOrWhiteSpace(url) Then Return

        Dim urlNode As New TreeNode(title)
        urlNode.Tag = New BookmarkNodeData With {.IsFolder = False, .Url = AppManager.FixURL(url)}

        Dim selectedNode = tvBookmarks.SelectedNode
        If selectedNode IsNot Nothing Then
            Dim data = TryCast(selectedNode.Tag, BookmarkNodeData)
            If data IsNot Nothing AndAlso data.IsFolder Then
                selectedNode.Nodes.Add(urlNode)
                selectedNode.Expand()
            ElseIf selectedNode.Parent IsNot Nothing Then
                selectedNode.Parent.Nodes.Add(urlNode)
            Else
                tvBookmarks.Nodes.Add(urlNode)
            End If
        Else
            tvBookmarks.Nodes.Add(urlNode)
        End If

        SaveTreeToSettings()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click
        OpenSelectedBookmark()
    End Sub

    Private Sub tvBookmarks_NodeMouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles tvBookmarks.NodeMouseDoubleClick
        OpenSelectedBookmark()
    End Sub

    Private Sub OpenSelectedBookmark()
        Dim selectedNode = tvBookmarks.SelectedNode
        If selectedNode IsNot Nothing Then
            Dim data = TryCast(selectedNode.Tag, BookmarkNodeData)
            If data IsNot Nothing AndAlso Not data.IsFolder AndAlso Not String.IsNullOrWhiteSpace(data.Url) Then
                Form1.NavigateActiveTab(data.Url)
            End If
        End If
    End Sub

    Private Sub btnEdit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnEdit.Click
        Dim selectedNode = tvBookmarks.SelectedNode
        If selectedNode Is Nothing Then Return
        Dim data = TryCast(selectedNode.Tag, BookmarkNodeData)
        If data Is Nothing Then Return

        If data.IsFolder Then
            Dim newName As String = InputBox("Edit Folder Name:", "Edit Folder", selectedNode.Text)
            If Not String.IsNullOrWhiteSpace(newName) Then
                selectedNode.Text = newName
                SaveTreeToSettings()
            End If
        Else
            Dim newTitle As String = InputBox("Edit Bookmark Title:", "Edit Bookmark", selectedNode.Text)
            If String.IsNullOrWhiteSpace(newTitle) Then Return
            Dim newUrl As String = InputBox("Edit Bookmark URL:", "Edit Bookmark", data.Url)
            If String.IsNullOrWhiteSpace(newUrl) Then Return

            selectedNode.Text = newTitle
            data.Url = AppManager.FixURL(newUrl)
            SaveTreeToSettings()
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        Dim selectedNode = tvBookmarks.SelectedNode
        If selectedNode IsNot Nothing Then
            If MessageBox.Show("Are you sure you want to delete '" & selectedNode.Text & "'?", "Delete Bookmark", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                selectedNode.Remove()
                SaveTreeToSettings()
            End If
        End If
    End Sub

End Class

Namespace My
    Partial Friend Class MySettings
        <Global.System.Configuration.UserScopedSettingAttribute()> _
        Public Property BookmarksTreeData() As Global.System.Collections.Specialized.StringCollection
            Get
                Return CType(Me("BookmarksTreeData"), Global.System.Collections.Specialized.StringCollection)
            End Get
            Set(ByVal value As Global.System.Collections.Specialized.StringCollection)
                Me("BookmarksTreeData") = value
            End Set
        End Property
    End Class
End Namespace