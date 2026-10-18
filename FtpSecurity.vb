Imports System.Net

#Disable Warning SYSLIB0014 ' FtpWebRequest is the API the FTP tool is built on.

''' <summary>
''' Builds the requests for the FTP tool. FTP sends the user name, password and files in clear text, so a
''' request asks the server for TLS first; plain FTP is only used after the user has agreed to it.
''' </summary>
Public Class FtpSecurity

    ''' <summary>Creates an FTP request. <paramref name="useTls"/> turns on explicit FTPS (AUTH TLS).</summary>
    Public Shared Function CreateRequest(ByVal url As String, ByVal method As String, ByVal userName As String,
                                         ByVal password As String, ByVal useTls As Boolean) As FtpWebRequest
        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) OrElse uri.Scheme <> Uri.UriSchemeFtp Then
            Throw New UriFormatException("An ftp:// address is required.")
        End If

        Dim request As FtpWebRequest = DirectCast(WebRequest.Create(uri), FtpWebRequest)
        request.Credentials = New NetworkCredential(userName, password)
        request.Method = method
        request.EnableSsl = useTls
        Return request
    End Function

    ''' <summary>
    ''' Whether a failed TLS attempt looks like "this server cannot do TLS" (so offering plain FTP is
    ''' reasonable) rather than a wrong password, a missing file or an unreachable server (where it is not).
    ''' </summary>
    Public Shared Function ShouldOfferPlainFallback(ByVal status As WebExceptionStatus, ByVal ftpStatus As FtpStatusCode?) As Boolean
        Select Case status
            Case WebExceptionStatus.SecureChannelFailure, WebExceptionStatus.ServerProtocolViolation
                Return True
            Case WebExceptionStatus.ProtocolError
                Return ftpStatus.HasValue AndAlso
                       (ftpStatus.Value = FtpStatusCode.CommandSyntaxError OrElse
                        ftpStatus.Value = FtpStatusCode.CommandNotImplemented OrElse
                        ftpStatus.Value = FtpStatusCode.BadCommandSequence)
            Case Else
                Return False
        End Select
    End Function

End Class
