Public Class History

    Private Sub History_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadHistoryData()
    End Sub

    Private Sub LoadHistoryData(Optional ByVal filterText As String = "")
        Try
            ListBox1.Items.Clear()
            If My.Settings.History IsNot Nothing Then
                Dim count As Integer = 0
                For Each item As String In My.Settings.History
                    If Not String.IsNullOrWhiteSpace(item) Then
                        If String.IsNullOrWhiteSpace(filterText) OrElse item.ToLower().Contains(filterText.ToLower()) Then
                            ListBox1.Items.Add(item)
                            count += 1
                        End If
                    End If
                Next
                lblCount.Text = count & " item" & If(count = 1, "", "s")
            Else
                lblCount.Text = "0 items"
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading history: " & ex.Message)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.TextChanged
        LoadHistoryData(txtSearch.Text.Trim())
    End Sub

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles ListBox1.DoubleClick
        OpenSelectedInActiveTab()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, tsmOpenActive.Click
        OpenSelectedInActiveTab()
    End Sub

    Private Async Sub btnOpenNewTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenNewTab.Click, tsmOpenNewTab.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            Dim url As String = ListBox1.SelectedItem.ToString()
            Await Form1.CreateNewTab(url)
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click, tsmCopy.Click
        If ListBox1.SelectedItem IsNot Nothing Then
            Clipboard.SetText(ListBox1.SelectedItem.ToString())
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click, tsmDelete.Click
        Try
            If ListBox1.SelectedItem IsNot Nothing Then
                Dim selectedUrl As String = ListBox1.SelectedItem.ToString()
                If My.Settings.History IsNot Nothing AndAlso My.Settings.History.Contains(selectedUrl) Then
                    My.Settings.History.Remove(selectedUrl)
                    My.Settings.Save()
                End If
                LoadHistoryData(txtSearch.Text.Trim())
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error deleting history item: " & ex.Message)
        End Try
    End Sub

    Private Sub btnClearAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAll.Click, tsmClearAll.Click
        If MsgBox("Are you sure you want to clear all browsing history?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Clear History") = MsgBoxResult.Yes Then
            Try
                If My.Settings.History IsNot Nothing Then
                    My.Settings.History.Clear()
                    My.Settings.Save()
                End If
                LoadHistoryData()
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error clearing history: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub OpenSelectedInActiveTab()
        If ListBox1.SelectedItem IsNot Nothing Then
            Dim url As String = ListBox1.SelectedItem.ToString()
            Form1.NavigateActiveTab(url)
        End If
    End Sub

End Class