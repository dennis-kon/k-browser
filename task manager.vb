Public Class task_manager

    Private Sub task_manager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ListView1.Items.Clear()
        For Each p As Process In Process.GetProcesses()
            Dim item As New ListViewItem(p.ProcessName)
            
            Dim memSize As String = "N/A"
            Try
                memSize = FormatNumber(Math.Round(p.PrivateMemorySize64 / 1024), 0)
            Catch ex As Exception
            End Try
            item.SubItems.Add(memSize)
            
            Dim responding As String = "N/A"
            Try
                responding = p.Responding.ToString()
            Catch ex As Exception
            End Try
            item.SubItems.Add(responding)
            
            Dim startTime As String = "N/A"
            Try
                startTime = p.StartTime.ToString().Trim()
            Catch ex As Exception
            End Try
            item.SubItems.Add(startTime)
            
            Dim pid As String = "N/A"
            Try
                pid = p.Id.ToString()
            Catch ex As Exception
            End Try
            item.SubItems.Add(pid)
            
            ListView1.Items.Add(item)
        Next
        ToolStripStatusLabel1.Text = "Processes: " & ListView1.Items.Count
    End Sub

    Private Sub KillSelectedProcesses()
        For Each item As ListViewItem In ListView1.SelectedItems
            If item.SubItems.Count > 4 Then
                Dim pidText As String = item.SubItems(4).Text
                Dim pid As Integer
                If pidText <> "N/A" AndAlso Integer.TryParse(pidText, pid) Then
                    Try
                        System.Diagnostics.Process.GetProcessById(pid).Kill()
                    Catch ex As Exception
                        MessageBox.Show("Could not terminate process " & pid & ": " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        Next
        task_manager_Load(Nothing, Nothing)
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        KillSelectedProcesses()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' Disable Timer during refresh to avoid overlapping loads if list is slow
        Timer1.Enabled = False
        Try
            ' Remember selected process index to restore it if possible
            Dim selectedIdx As Integer = -1
            If ListView1.SelectedIndices.Count > 0 Then
                selectedIdx = ListView1.SelectedIndices(0)
            End If
            
            task_manager_Load(Nothing, Nothing)
            
            If selectedIdx >= 0 AndAlso selectedIdx < ListView1.Items.Count Then
                ListView1.Items(selectedIdx).Selected = True
            End If
        Finally
            Timer1.Enabled = True
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        KillSelectedProcesses()
    End Sub
End Class