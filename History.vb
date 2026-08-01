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
        If ListBox1.SelectedItem IsNot Nothing Then
            Form1.NavigateActiveTab(ListBox1.SelectedItem.ToString())
        End If
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            If ListBox1.SelectedItem IsNot Nothing Then
                Dim item As Integer = My.Settings.History.IndexOf(ListBox1.SelectedItem.ToString())
                If item >= 0 Then
                    My.Settings.History.RemoveAt(item)
                    ListBox1.Items.Remove(ListBox1.SelectedItem)
                    My.Settings.Save()
                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub OpenToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        If ListBox1.SelectedItem IsNot Nothing Then
            Form1.NavigateActiveTab(ListBox1.SelectedItem.ToString())
        End If
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
        Try
            My.Settings.History.Clear()
            My.Settings.Save()
        Catch ex As Exception
        End Try
    End Sub
End Class