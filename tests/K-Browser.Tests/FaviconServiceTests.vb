Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net
Imports System.Net.Http
Imports System.Threading
Imports Advance_Webbrowser
Imports Xunit

Public Class FaviconServiceTests
    Implements IDisposable

    ''' <summary>Answers every request with the configured response (or exception) and counts requests.</summary>
    Private NotInheritable Class FakeHandler
        Inherits HttpMessageHandler

        Public Respond As Func(Of HttpRequestMessage, HttpResponseMessage)
        Public Requests As New List(Of Uri)

        Protected Overrides Function SendAsync(request As HttpRequestMessage, cancellationToken As CancellationToken) As Task(Of HttpResponseMessage)
            SyncLock Requests
                Requests.Add(request.RequestUri)
            End SyncLock
            Return Task.FromResult(Respond(request))
        End Function
    End Class

    Private ReadOnly _cacheDir As String = Path.Combine(Path.GetTempPath(), "kbrowser-favicon-tests-" & Guid.NewGuid().ToString("N"))
    Private ReadOnly _handler As New FakeHandler()

    Private Function NewService() As FaviconService
        Return New FaviconService(New HttpClient(_handler), _cacheDir)
    End Function

    Private Shared Function Ok(bytes As Byte()) As HttpResponseMessage
        Return New HttpResponseMessage(HttpStatusCode.OK) With {.Content = New ByteArrayContent(bytes)}
    End Function

    Private Shared Function PngBytes(size As Integer) As Byte()
        Using bmp As New Bitmap(size, size), g = Graphics.FromImage(bmp), ms As New MemoryStream()
            g.Clear(Color.OrangeRed)
            bmp.Save(ms, ImageFormat.Png)
            Return ms.ToArray()
        End Using
    End Function

    Private Shared Function IcoBytes() As Byte()
        Using bmp As New Bitmap(16, 16), g = Graphics.FromImage(bmp), ms As New MemoryStream()
            g.Clear(Color.SteelBlue)
            Using ico = Icon.FromHandle(bmp.GetHicon())
                ico.Save(ms)
            End Using
            Return ms.ToArray()
        End Using
    End Function

    <Fact>
    Public Async Function GetFaviconAsync_Png_ReturnsImageAndCachesToDisk() As Task
        _handler.Respond = Function(r) Ok(PngBytes(32))

        Dim result = Await NewService().GetFaviconAsync("example.com")

        Assert.NotNull(result)
        Assert.Equal(32, result.Width)
        Assert.Equal(New Uri("https://example.com/favicon.ico"), _handler.Requests.Single())
        Assert.True(File.Exists(Path.Combine(_cacheDir, "example.com.png")))

        Dim fromDisk = Await NewService().GetFaviconAsync("example.com")
        Assert.NotNull(fromDisk)
        Assert.Single(_handler.Requests)
    End Function

    <Fact>
    Public Async Function GetFaviconAsync_Ico_ReturnsImage() As Task
        _handler.Respond = Function(r) Ok(IcoBytes())

        Dim result = Await NewService().GetFaviconAsync("example.org")

        Assert.NotNull(result)
        Assert.Equal(16, result.Width)
    End Function

    <Fact>
    Public Async Function GetFaviconAsync_SameInstance_FetchesOnce() As Task
        _handler.Respond = Function(r) Ok(PngBytes(16))
        Dim service = NewService()

        Await service.GetFaviconAsync("example.net")
        Await service.GetFaviconAsync("EXAMPLE.net")

        Assert.Single(_handler.Requests)
    End Function

    <Theory>
    <InlineData("html")>
    <InlineData("empty")>
    <InlineData("404")>
    Public Async Function GetFaviconAsync_NonImageResponse_ReturnsNothingAndWritesNoCacheFile(kind As String) As Task
        _handler.Respond = Function(r)
                               Select Case kind
                                   Case "html" : Return Ok(System.Text.Encoding.UTF8.GetBytes("<html><body>Not here</body></html>"))
                                   Case "empty" : Return Ok(Array.Empty(Of Byte)())
                                   Case Else : Return New HttpResponseMessage(HttpStatusCode.NotFound)
                               End Select
                           End Function

        Dim result = Await NewService().GetFaviconAsync("example.com")

        Assert.Null(result)
        Assert.False(File.Exists(Path.Combine(_cacheDir, "example.com.png")))
    End Function

    <Fact>
    Public Async Function GetFaviconAsync_NetworkFailure_ReturnsNothingAndRetriesNextCall() As Task
        _handler.Respond = Function(r)
                                   Throw New HttpRequestException("offline")
                               End Function
        Dim service = NewService()

        Assert.Null(Await service.GetFaviconAsync("example.com"))
        Assert.Null(Await service.GetFaviconAsync("example.com"))

        Assert.Equal(2, _handler.Requests.Count)
    End Function

    <Fact>
    Public Async Function GetFaviconAsync_Timeout_ReturnsNothing() As Task
        _handler.Respond = Function(r)
                                   Throw New TaskCanceledException("timed out")
                               End Function

        Assert.Null(Await NewService().GetFaviconAsync("example.com"))
    End Function

    <Theory>
    <InlineData("..\evil")>
    <InlineData("a/b")>
    <InlineData("")>
    <InlineData(Nothing)>
    <InlineData("exa mple.com")>
    <InlineData("localhost")>
    Public Async Function GetFaviconAsync_RejectsNonHostname(domain As String) As Task
        _handler.Respond = Function(r) Ok(PngBytes(16))

        Assert.Null(Await NewService().GetFaviconAsync(domain))
        Assert.Empty(_handler.Requests)
    End Function

    <Fact>
    Public Sub ScaleImage_ResizesToDpiScaledSquare()
        Using source As New Bitmap(64, 32)
            Dim scaled = IconService.ScaleImage(source, 16)
            Dim expected = CInt(Math.Round(16 * IconService.SystemDpi / 96.0))

            Assert.Equal(expected, scaled.Width)
            Assert.Equal(expected, scaled.Height)
        End Using
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            If Directory.Exists(_cacheDir) Then Directory.Delete(_cacheDir, True)
        Catch
        End Try
    End Sub

End Class
