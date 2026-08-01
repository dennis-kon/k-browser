Public Class Bookmarks

    Private Sub Bookmarks_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            ListBox1.Items.Clear()

            Dim c As Integer = My.Settings.Bookmarks.Count ' make sure you put bookmarks insted of history
            Dim x As Integer
            For x = 0 To c - 1
                ListBox1.Items.Add(My.Settings.Bookmarks.Item(x)) ' make sure you put bookmarks insted of history
            Next


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            Form1.NavigateActiveTab(ListBox1.SelectedItem.ToString())
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            If ListBox1.SelectedItem IsNot Nothing Then
                Dim item As Integer = My.Settings.Bookmarks.IndexOf(ListBox1.SelectedItem.ToString())
                If item >= 0 Then
                    My.Settings.Bookmarks.RemoveAt(item)
                    ListBox1.Items.Remove(ListBox1.SelectedItem)
                    My.Settings.Save()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub
End Class