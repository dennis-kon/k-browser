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
        CType(Form1.TabControl1.SelectedTab.Controls.Item(0), WebBrowser).Navigate(ListBox1.SelectedItem)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Dim item As Integer = My.Settings.Bookmarks.IndexOf(ListBox1.SelectedItem)
            My.Settings.Bookmarks.RemoveAt(item)
            ListBox1.Items.Remove(ListBox1.SelectedItem)
            My.Settings.Save()
        Catch ex As Exception

        End Try

    End Sub
End Class
