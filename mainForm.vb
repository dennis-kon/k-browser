Imports System.Net
Public Class downman

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
        Dim urlText As String = Me.txtFileName.Text.Trim()
        If Not String.IsNullOrEmpty(urlText) AndAlso (urlText.StartsWith("http://", StringComparison.OrdinalIgnoreCase) OrElse urlText.StartsWith("https://", StringComparison.OrdinalIgnoreCase)) Then
            Dim urlUri As Uri = Nothing
            If Uri.TryCreate(urlText, UriKind.Absolute, urlUri) Then
                Me.SaveFileDialog1.FileName = System.IO.Path.GetFileName(urlUri.LocalPath)
            Else
                Me.SaveFileDialog1.FileName = "downloaded_file"
            End If

            If Me.SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
                Me.whereToSave = Me.SaveFileDialog1.FileName
                Me.SaveFileDialog1.FileName = ""
                Me.Label6.Text = "Save to: " & Me.whereToSave
                Me.txtFileName.Enabled = False
                Me.btnDownload.Enabled = False
                Me.btnCancel.Enabled = True
                Me.BackgroundWorker1.RunWorkerAsync()
            End If
        Else
            MessageBox.Show("Insert a valid http:// or https:// URL for download", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning)
        End If
    End Sub

    Private Sub BackgroundWorker1_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim theResponse As HttpWebResponse = Nothing
        Dim theRequest As HttpWebRequest = Nothing
        Try
            theRequest = CType(WebRequest.Create(Me.txtFileName.Text.Trim()), HttpWebRequest)
            theResponse = CType(theRequest.GetResponse(), HttpWebResponse)
        Catch ex As Exception
            MessageBox.Show("An error occurred while downloading this file." & ControlChars.CrLf & _
                            "1) The File doesn't exist" & ControlChars.CrLf & _
                            "2) Remote server error: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

            Dim cancelDelegate As New DownloadCompleteSafe(AddressOf DownloadComplete)
            Me.Invoke(cancelDelegate, True)
            Exit Sub
        End Try

        Dim length As Long = theResponse.ContentLength
        Dim safedelegate As New ChangeTextsSafe(AddressOf ChangeTexts)
        Me.Invoke(safedelegate, length, 0, 0, 0)

        Dim writeStream As New IO.FileStream(Me.whereToSave, IO.FileMode.Create)
        Dim nRead As Long = 0
        Dim bytesInSample As Long = 0
        Dim speedtimer As New Stopwatch
        Dim currentspeed As Double = -1

        speedtimer.Start()

        Do
            If BackgroundWorker1.CancellationPending Then
                Exit Do
            End If

            Dim readBytes(4095) As Byte
            Dim bytesread As Integer = theResponse.GetResponseStream().Read(readBytes, 0, 4096)
            If bytesread = 0 Then Exit Do

            nRead += bytesread
            bytesInSample += bytesread
            writeStream.Write(readBytes, 0, bytesread)

            If speedtimer.ElapsedMilliseconds >= 1000 Then
                currentspeed = (bytesInSample / (speedtimer.ElapsedMilliseconds / 1000.0))
                speedtimer.Restart()
                bytesInSample = 0
            End If

            Dim percent As Integer = 0
            If length > 0 Then
                percent = CInt(Math.Min(100L, Math.Max(0L, (nRead * 100L) \ length)))
            End If

            Me.Invoke(safedelegate, length, CInt(Math.Min(Integer.MaxValue, nRead)), percent, currentspeed)
        Loop

        theResponse.GetResponseStream().Close()
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

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub
End Class
