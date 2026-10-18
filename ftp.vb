Imports System.IO
Imports System.Net

Public Class ftp
    Public Property UseSystemPasswordChar As Boolean
    Public Property PasswordChar As Char

    ' FTP sends the password and files in clear text. Requests ask for TLS first; plain FTP is used
    ' only after the user has agreed to it for the server currently typed in.
    Private _allowPlainFtp As Boolean = False

    Private Sub txt_server_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txt_server.TextChanged
        _allowPlainFtp = False
    End Sub

    ''' <summary>
    ''' After a failed TLS attempt that looks like "this server cannot do TLS", asks whether to retry without
    ''' encryption. Returns True (and allows plain FTP for this server) only if the user says yes.
    ''' </summary>
    Private Function OfferPlainFtp(ByVal ex As WebException) As Boolean
        If _allowPlainFtp Then Return False

        Dim ftpStatus As FtpStatusCode? = Nothing
        Dim response As FtpWebResponse = TryCast(ex.Response, FtpWebResponse)
        If response IsNot Nothing Then ftpStatus = response.StatusCode
        If Not FtpSecurity.ShouldOfferPlainFallback(ex.Status, ftpStatus) Then Return False

        Dim answer As DialogResult = MessageBox.Show(
            "An encrypted (TLS) connection to this server could not be made:" & vbCrLf & ex.Message & vbCrLf & vbCrLf &
            "Connect WITHOUT encryption instead?" & vbCrLf &
            "Your user name, password and files would be sent in clear text.",
            "FTP - No Encryption", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2)
        If answer <> DialogResult.Yes Then Return False

        _allowPlainFtp = True
        Return True
    End Function

    Private Sub listFTP(ByVal URL As String, ByVal bk As String, ByVal pw As String)
        Dim requ As FtpWebRequest = Nothing
        Dim resp As FtpWebResponse = Nothing
        Dim reader As StreamReader = Nothing
        Try
            requ = FtpSecurity.CreateRequest(URL, WebRequestMethods.Ftp.ListDirectory, bk, pw, Not _allowPlainFtp)
            resp = CType(requ.GetResponse(), FtpWebResponse)
            reader = New StreamReader(resp.GetResponseStream())
            While (reader.Peek() > -1)
                ListBox1.Items.Add(reader.ReadLine())

            End While
            ToolStripStatusLabel1.Text = "list complete !"
        Catch ex As UriFormatException
            ToolStripStatusLabel1.Text = ex.Message
        Catch ex As WebException
            If OfferPlainFtp(ex) Then
                listFTP(URL, bk, pw)
                Return
            End If
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
            requ = FtpSecurity.CreateRequest(URL, WebRequestMethods.Ftp.DownloadFile, bk, pw, Not _allowPlainFtp)
            resp = CType(requ.GetResponse(), FtpWebResponse)
            respStrm = resp.GetResponseStream()
            SaveFileDialog1.FileName = Path.GetFileName(requ.RequestUri.LocalPath)
            If (SaveFileDialog1.ShowDialog() = DialogResult.OK) Then
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
            If OfferPlainFtp(ex) Then
                downloadFTP(URL, bk, pw)
                Return
            End If
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
            requ = FtpSecurity.CreateRequest(URL, WebRequestMethods.Ftp.UploadFile, bk, pw, Not _allowPlainFtp)
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
            If OfferPlainFtp(ex) Then
                uploadFTP(URL, filename, bk, pw)
                Return
            End If
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
            requ = FtpSecurity.CreateRequest(URL, WebRequestMethods.Ftp.DeleteFile, bk, pw, Not _allowPlainFtp)
            resp = CType(requ.GetResponse(), FtpWebResponse)
            ToolStripStatusLabel1.Text = "The File was deleted!"
        Catch ex As UriFormatException
            ToolStripStatusLabel1.Text = ex.Message
        Catch ex As WebException
            If OfferPlainFtp(ex) Then
                deleteFTP(URL, bk, pw)
                Return
            End If
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
        If (OpenFileDialog1.ShowDialog() = DialogResult.OK) Then
            uploadFTP(txt_server.Text + "/" + Path.GetFileName(OpenFileDialog1.FileName), OpenFileDialog1.FileName, txt_user.Text, txt_pw.Text)
        End If
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        If (MessageBox.Show("Do you want realy to delete this file? ", " Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes) Then
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
        IconService.SetIcon(ToolStripLabel1, FluentIcon.Server)
        IconService.SetIcon(ToolStripLabel2, FluentIcon.Person)
        IconService.SetIcon(ToolStripLabel3, FluentIcon.Password)
        ThemeManager.ApplyTheme(Me)
        txt_pw.TextBox.PasswordChar = "*"c
    End Sub
End Class
