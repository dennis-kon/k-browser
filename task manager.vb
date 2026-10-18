Public Class task_manager

    Private Sub task_manager_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        IconService.SetIcon(ToolStripButton1, FluentIcon.DismissCircle)
        ThemeManager.ApplyTheme(Me)
        RefreshProcessList()
        Timer1.Start()
    End Sub

    ' The window is shown with ShowDialog on its default instance, which is hidden rather than disposed on close:
    ' without this the refresh timer would keep listing every process for the rest of the session.
    Private Sub task_manager_FormClosed(ByVal sender As Object, ByVal e As FormClosedEventArgs) Handles Me.FormClosed
        Timer1.Stop()
    End Sub

    Private Sub RefreshProcessList()
        Dim ownId As Integer = Environment.ProcessId
        ListView1.BeginUpdate()
        Try
            ListView1.Items.Clear()
            For Each p As Process In Process.GetProcesses()
                Using p
                    Dim displayName As String = p.ProcessName
                    If p.ProcessName.Equals("msedgewebview2", StringComparison.OrdinalIgnoreCase) Then
                        displayName = "WebView2 Process (" & p.ProcessName & ")"
                    ElseIf p.ProcessName.Equals("K-Browser", StringComparison.OrdinalIgnoreCase) Then
                        displayName = "K-Browser Main Process"
                    End If
                    Dim item As New ListViewItem(displayName)

                    Dim memSize As String = "N/A"
                    Try
                        memSize = FormatNumber(Math.Round(p.PrivateMemorySize64 / 1024), 0)
                    Catch ex As Exception
                    End Try
                    item.SubItems.Add(memSize)

                    ' Checking whether a process responds means messaging each of its windows (with a timeout),
                    ' so it is only done for this browser's own process.
                    Dim responding As String = "N/A"
                    If p.Id = ownId Then
                        Try
                            responding = p.Responding.ToString()
                        Catch ex As Exception
                        End Try
                    End If
                    item.SubItems.Add(responding)

                    Dim startTime As String = "N/A"
                    Try
                        startTime = p.StartTime.ToString().Trim()
                    Catch ex As Exception
                    End Try
                    item.SubItems.Add(startTime)

                    item.SubItems.Add(p.Id.ToString())
                    ListView1.Items.Add(item)
                End Using
            Next
        Finally
            ListView1.EndUpdate()
        End Try
        ToolStripStatusLabel1.Text = "Processes: " & ListView1.Items.Count
    End Sub

    Private Sub KillSelectedProcesses()
        If ListView1.SelectedItems.Count = 0 Then Return
        If MessageBox.Show("Are you sure you want to terminate the selected process(es)?", "Confirm Termination", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) <> DialogResult.Yes Then
            Return
        End If
        For Each item As ListViewItem In ListView1.SelectedItems
            If item.SubItems.Count > 4 Then
                Dim pidText As String = item.SubItems(4).Text
                Dim pid As Integer
                If pidText <> "N/A" AndAlso Integer.TryParse(pidText, pid) Then
                    Try
                        Using target As Process = System.Diagnostics.Process.GetProcessById(pid)
                            target.Kill()
                        End Using
                    Catch ex As Exception
                        MessageBox.Show("Could not terminate process " & pid & ": " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End If
        Next
        RefreshProcessList()
    End Sub

    Private Sub ToolStripButton1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToolStripButton1.Click
        KillSelectedProcesses()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ' Disable Timer during refresh to avoid overlapping loads if list is slow
        Timer1.Stop()
        Try
            ' Remember selected process index to restore it if possible
            Dim selectedIdx As Integer = -1
            If ListView1.SelectedIndices.Count > 0 Then
                selectedIdx = ListView1.SelectedIndices(0)
            End If

            RefreshProcessList()

            If selectedIdx >= 0 AndAlso selectedIdx < ListView1.Items.Count Then
                ListView1.Items(selectedIdx).Selected = True
            End If
        Finally
            If Me.Visible Then Timer1.Start()
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        KillSelectedProcesses()
    End Sub
End Class
