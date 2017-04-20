Public Class History

    Private Sub History_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            ListBox1.Items.Clear()

            Dim c As Integer = My.Settings.History.Count
            Dim x As Integer
            For x = 0 To c - 1
                ListBox1.Items.Add(My.Settings.History.Item(x))
            Next


        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Form1.wb.Navigate(ListBox1.SelectedItem)

    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            Try
                Dim item As Integer = My.Settings.History.IndexOf(ListBox1.SelectedItem)
                My.Settings.History.RemoveAt(item)
                ListBox1.Items.Remove(ListBox1.SelectedItem)
                My.Settings.Save()
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Form1.wb.Navigate(ListBox1.SelectedItem)
    End Sub

    Private Sub DeleteToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            Try
                Dim item As Integer = My.Settings.History.IndexOf(ListBox1.SelectedItem)
                My.Settings.History.RemoveAt(item)
                ListBox1.Items.Remove(ListBox1.SelectedItem)
                My.Settings.Save()
            Catch ex As Exception

            End Try
        Catch ex As Exception

        End Try
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ListBox1.Items.Clear()
    End Sub
End Class