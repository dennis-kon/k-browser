Imports System.Net
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' The FTP tool sends a user name and password. It must ask the server for TLS first, and fall back to
''' plain FTP only when the user has agreed after a failure that suggests the server cannot do TLS.
''' </summary>
Public Class FtpSecurityTests

#Disable Warning SYSLIB0014 ' FtpWebRequest is the API this tool is built on.

    <Fact>
    Public Sub Request_UsesTls_WhenAsked()
        Dim request = FtpSecurity.CreateRequest("ftp://example.com/file.txt", WebRequestMethods.Ftp.DownloadFile, "me", "pw", useTls:=True)
        Assert.True(request.EnableSsl)
        Assert.Equal(WebRequestMethods.Ftp.DownloadFile, request.Method)
        Assert.Equal("me", DirectCast(request.Credentials, NetworkCredential).UserName)
    End Sub

    <Fact>
    Public Sub Request_IsPlain_OnlyWhenTheCallerAllowsIt()
        Dim request = FtpSecurity.CreateRequest("ftp://example.com/", WebRequestMethods.Ftp.ListDirectory, "me", "pw", useTls:=False)
        Assert.False(request.EnableSsl)
    End Sub

    <Theory>
    <InlineData("http://example.com/")>
    <InlineData("https://example.com/")>
    <InlineData("example.com")>
    <InlineData("")>
    Public Sub NonFtpAddress_IsRefused(url As String)
        Assert.ThrowsAny(Of Exception)(Sub() FtpSecurity.CreateRequest(url, WebRequestMethods.Ftp.ListDirectory, "me", "pw", True))
    End Sub

    <Theory>
    <InlineData(WebExceptionStatus.SecureChannelFailure, Nothing, True)>
    <InlineData(WebExceptionStatus.ServerProtocolViolation, Nothing, True)>
    <InlineData(WebExceptionStatus.ProtocolError, FtpStatusCode.CommandNotImplemented, True)>
    <InlineData(WebExceptionStatus.ProtocolError, FtpStatusCode.CommandSyntaxError, True)>
    <InlineData(WebExceptionStatus.ProtocolError, FtpStatusCode.ServiceNotAvailable, False)>
    <InlineData(WebExceptionStatus.ProtocolError, FtpStatusCode.NotLoggedIn, False)>
    <InlineData(WebExceptionStatus.ProtocolError, FtpStatusCode.ActionNotTakenFileUnavailable, False)>
    <InlineData(WebExceptionStatus.NameResolutionFailure, Nothing, False)>
    <InlineData(WebExceptionStatus.Timeout, Nothing, False)>
    Public Sub PlainFallback_IsOffered_OnlyWhenTheServerLooksTlsIncapable(status As WebExceptionStatus, ftpStatus As Object, expected As Boolean)
        Dim code As FtpStatusCode? = If(ftpStatus Is Nothing, CType(Nothing, FtpStatusCode?), CType(ftpStatus, FtpStatusCode))
        Assert.Equal(expected, FtpSecurity.ShouldOfferPlainFallback(status, code))
    End Sub

#Enable Warning SYSLIB0014

End Class
