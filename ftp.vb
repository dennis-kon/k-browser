Imports System.Net
Imports System.IO

Public Class ftp
    Public Property UseSystemPasswordChar As Boolean
    Public Property PasswordChar As Char

    Private Sub listFTP(ByVal URL As String, ByVal bk As String, ByVal pw As String)
        Dim requ As FtpWebRequest = Nothing
        Dim resp As FtpWebResponse = Nothing
        Dim reader As StreamReader = Nothing
        Try
            requ = CType(WebRequest.Create(URL), WebRequest)
            requ.Credentials = New NetworkCredential(bk, pw)
            requ.Method = WebRequestMethods.Ftp.ListDirectory
            resp = CType(requ.GetResponse(), FtpWebResponse)
            reader = New StreamReader(resp.GetResponseStream())
            While (reader.Peek() > -1)
                ListBox1.Items.Add(reader.ReadLine())

            End While
            ToolStripStatusLabel1.Text = "list complete !"
        Catch ex As UriFormatException
            ToolStripStatusLabel1.Text = ex.Message
        Catch ex As WebException
            ToolStripStatusLabel2.Text = ex.Message
        Finally
            If reader IsNot Nothing Then reader.Close()
        End Try
    End Sub

    Private Sub downloadFTP(ByVal URL As String, ByVal bk As String, ByVal pw As String)
        Dim requ As FtpWebRequest = Nothing
        Dim resp As FtpWebResponse = Nothing
        Dim respStrm As Stream = Nothing
        Dim fileStrm As FileStream = Nothing
        Try
            requ = CType(WebRequest.Create(URL), FtpWebRequest)
            requ.Credentials = New NetworkCredential(bk, pw)
            requ.Method = WebRequestMethods.Ftp.DownloadFile
            resp = CType(requ.GetResponse(), FtpWebResponse)
            respStrm = resp.GetResponseStream()
            SaveFileDialog1.FileName = Path.GetFileName(requ.RequestUri.LocalPath)
            If (SaveFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
                fileStrm = File.Create(SaveFileDialog1.FileName)
                Dim buff(1024) As Byte
                Dim bytesRead As Integer = 0
                While (True)
                    bytesRead = respStrm.Read(buff, 0, buff.Length)
                    If (bytesRead = 0) Then Exit While
                    fileStrm.Write(buff, 0, bytesRead)
                End While
                ToolStripStatusLabel1.Text = "Download complete"
            End If
        Catch ex As UriFormatException
            ToolStripStatusLabel1.Text = ex.Message
        Catch ex As WebException
            ToolStripStatusLabel2.Text = ex.Message
        Catch ex As IOException
            ToolStripStatusLabel2.Text = ex.Message
        Finally
            If respStrm IsNot Nothing Then respStrm.Close()
            If fileStrm IsNot Nothing Then fileStrm.Close()
        End Try
    End Sub

    Private Sub uploadFTP(ByVal URl As String, ByVal filename As String, ByVal bk As String, ByVal pw As String)
        Dim requ As FtpWebRequest = Nothing
        Dim resp As FtpWebResponse = Nothing
        Dim requStrm As Stream = Nothing
        Dim fileStrm As FileStream = Nothing
        Try
            requ = CType(WebRequest.Create(URL), FtpWebRequest)
            requ.Credentials = New NetworkCredential(bk, pw)
            requ.Method = WebRequestMethods.Ftp.UploadFile
            requ.Timeout = System.Threading.Timeout.Infinite
            requ.Proxy = Nothing
            requStrm = requ.GetRequestStream()
            Dim buff(2048) As Byte
            Dim bytesRead As Integer = 0
            fileStrm = File.OpenRead(filename)
            Do While (True)
                bytesRead = fileStrm.Read(buff, 0, buff.Length)
                If (bytesRead = 0) Then Exit Do
                requStrm.Write(buff, 0, bytesRead)
            Loop
            requStrm.Close()
            resp = CType(requ.GetResponse(), FtpWebResponse)
            ToolStripStatusLabel1.Text = "Upload complete "
        Catch ex As UriFormatException
            ToolStripStatusLabel1.Text = ex.Message
        Catch ex As IOException
            ToolStripStatusLabel2.Text = ex.Message
        Catch ex As WebException
            ToolStripStatusLabel2.Text = ex.Message
        Finally
            If resp IsNot Nothing Then resp.Close()
            If fileStrm IsNot Nothing Then fileStrm.Close()
            If requStrm IsNot Nothing Then requStrm.Close()
        End Try
    End Sub

    Private Sub deleteFTP(ByVal URL As String, ByVal bk As String, ByVal pw As String)
        Dim requ As FtpWebRequest = Nothing
        Dim resp As FtpWebResponse = Nothing
        Try
            requ = CType(WebRequest.Create(URL), FtpWebRequest)
            requ.Credentials = New NetworkCredential(bk, pw)
            requ.Method = WebRequestMethods.Ftp.DeleteFile
            resp = CType(requ.GetResponse(), FtpWebResponse)
            ToolStripStatusLabel1.Text = "The File was deleted!"
        Catch ex As UriFormatException
            ToolStripStatusLabel1.Text = ex.Message
        Catch ex As WebException
            ToolStripStatusLabel2.Text = ex.Message
        Finally
            If resp IsNot Nothing Then resp.Close()
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        ListBox1.Items.Clear()
        listFTP(txt_server.Text, txt_user.Text, txt_pw.Text)
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        downloadFTP(TextBox4.text, txt_user.Text, txt_pw.Text)
    End Sub

    Private Sub ListBox1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListBox1.SelectedIndexChanged
        TextBox4.Text = txt_server.Text & "/" & ListBox1.SelectedItems(0).ToString()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        If (OpenFileDialog1.ShowDialog() = Windows.Forms.DialogResult.OK) Then
            uploadFTP(OpenFileDialog1.FileName, txt_server.Text + "/" + Path.GetFileName(OpenFileDialog1.FileName), txt_user.Text, txt_pw.Text)
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If (MessageBox.Show("Do you want realy to delete this file? ", " Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = Windows.Forms.DialogResult.Yes) Then
            deleteFTP(TextBox4.Text, txt_user.Text, txt_pw.Text)
        End If
    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        ListBox1.Items.Clear()
        listFTP(txt_server.Text, txt_user.Text, txt_pw.Text)
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button6.Click
        ListBox1.Items.Clear()
    End Sub

    
    Private Sub ftp_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        txt_pw.TextBox.PasswordChar = "*"c
    End Sub
End Class
