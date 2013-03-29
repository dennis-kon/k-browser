Imports System.Net
Public Class mainForm

    Dim whereToSave As String
    Delegate Sub ChangeTextsSafe(ByVal length As Long, ByVal position As Integer, ByVal percent As Integer, ByVal speed As Double)
    Delegate Sub DownloadCompleteSafe(ByVal cancelled As Boolean)

    Public Sub DownloadComplete(ByVal cancelled As Boolean)
        Me.txtFileName.Enabled = True
        Me.btnDownload.Enabled = True
        Me.btnCancel.Enabled = False

        If cancelled Then

            Me.Label4.Text = "Cancelled"

            MessageBox.Show("Download aborted", "Aborted", MessageBoxButtons.OK, MessageBoxIcon.Information)


        Else
            Me.Label4.Text = "Successfully downloaded"

            MessageBox.Show("Successfully downloaded", "Download Manager ", MessageBoxButtons.OK, MessageBoxIcon.Information)


        End If

        Me.ProgressBar1.Value = 0
        Me.Label5.Text = "Downloading: "
        Me.Label6.Text = "Save file to: "
        Me.Label3.Text = "File size: "
        Me.Label2.Text = "Downloading speed: "
        Me.Label4.Text = ""

    End Sub

    Public Sub ChangeTexts(ByVal length As Long, ByVal position As Integer, ByVal percent As Integer, ByVal speed As Double)

        Me.Label3.Text = "File Size: " & Math.Round((length / 1024), 2) & " KB"

        Me.Label5.Text = "Downloading: " & Me.txtFileName.Text

        Me.Label4.Text = "Downloaded " & Math.Round((position / 1024), 2) & " KB of " & Math.Round((length / 1024), 2) & "KB (" & Me.ProgressBar1.Value & "%)"

        If speed = -1 Then
            Me.Label2.Text = "Speed: calculating..."
        Else
            Me.Label2.Text = "Speed: " & Math.Round((speed / 1024), 2) & " KB/s"
        End If

        Me.ProgressBar1.Value = percent


    End Sub

    Private Sub btnDownload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDownload.Click

        If Me.txtFileName.Text <> "" AndAlso Me.txtFileName.Text.StartsWith("http://") Then


            Me.SaveFileDialog1.FileName = Me.txtFileName.Text.Split("/"c)(Me.txtFileName.Text.Split("/"c).Length - 1)

            If Me.SaveFileDialog1.ShowDialog = Windows.Forms.DialogResult.OK Then

                Me.whereToSave = Me.SaveFileDialog1.FileName

                Me.SaveFileDialog1.FileName = ""

                Me.Label6.Text = "Save to: " & Me.whereToSave

                Me.txtFileName.Enabled = False
                Me.btnDownload.Enabled = False
                Me.btnCancel.Enabled = True

                Me.BackgroundWorker1.RunWorkerAsync()

            End If

        Else

            MessageBox.Show("Insert a valid URL for download", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)

        End If

    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork


        Dim theResponse As HttpWebResponse
        Dim theRequest As HttpWebRequest
        Try

            theRequest = WebRequest.Create(Me.txtFileName.Text)
            theResponse = theRequest.GetResponse
        Catch ex As Exception

            MessageBox.Show("An error occurred while downloading this file. Possibe causes:" & ControlChars.CrLf & _
                            "1) The File doesn't exist" & ControlChars.CrLf & _
                            "2) Remote server error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Dim cancelDelegate As New DownloadCompleteSafe(AddressOf DownloadComplete)

            Me.Invoke(cancelDelegate, True)

            Exit Sub
        End Try
        Dim length As Long = theResponse.ContentLength

        Dim safedelegate As New ChangeTextsSafe(AddressOf ChangeTexts)
        Me.Invoke(safedelegate, length, 0, 0, 0)

        Dim writeStream As New IO.FileStream(Me.whereToSave, IO.FileMode.Create)


        Dim nRead As Integer


        Dim speedtimer As New Stopwatch
        Dim currentspeed As Double = -1
        Dim readings As Integer = 0

        Do

            If BackgroundWorker1.CancellationPending Then
                Exit Do
            End If

            speedtimer.Start()

            Dim readBytes(4095) As Byte
            Dim bytesread As Integer = theResponse.GetResponseStream.Read(readBytes, 0, 4096)

            nRead += bytesread
            Dim percent As Short = (nRead * 100) / length

            Me.Invoke(safedelegate, length, nRead, percent, currentspeed)

            If bytesread = 0 Then Exit Do

            writeStream.Write(readBytes, 0, bytesread)

            speedtimer.Stop()

            readings += 1
            If readings >= 5 Then
                currentspeed = 20480 / (speedtimer.ElapsedMilliseconds / 1000)
                speedtimer.Reset()
                readings = 0
            End If
        Loop


        theResponse.GetResponseStream.Close()
        writeStream.Close()

        If Me.BackgroundWorker1.CancellationPending Then

            IO.File.Delete(Me.whereToSave)

            Dim cancelDelegate As New DownloadCompleteSafe(AddressOf DownloadComplete)

            Me.Invoke(cancelDelegate, True)

            Exit Sub

        End If

        Dim completeDelegate As New DownloadCompleteSafe(AddressOf DownloadComplete)

        Me.Invoke(completeDelegate, False)

    End Sub

    Private Sub mainForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Label4.Text = ""
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.BackgroundWorker1.CancelAsync()
    End Sub
End Class
