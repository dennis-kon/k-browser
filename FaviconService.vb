Imports System.Collections.Concurrent
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Net.Http
Imports System.Text.RegularExpressions

''' <summary>
''' Fetches site favicons straight from https://&lt;domain&gt;/favicon.ico (no third-party favicon
''' service) and caches them in memory and as PNG files on disk. Never throws: any failure yields
''' Nothing, and failures are not cached so a later call retries.
''' Returned bitmaps are shared from the cache: callers must never dispose them.
''' </summary>
Public Class FaviconService

    Private Shared ReadOnly HostPattern As New Regex("^(?=.{1,253}$)([a-z0-9-]{1,63}\.)+[a-z]{2,63}$", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)

    Private Shared ReadOnly _default As New Lazy(Of FaviconService)(
        Function()
            Dim http As New HttpClient() With {.Timeout = TimeSpan.FromSeconds(5)}
            http.DefaultRequestHeaders.UserAgent.ParseAdd("K-Browser")
            Dim dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "FaviconCache")
            Return New FaviconService(http, dir)
        End Function)

    Private ReadOnly _http As HttpClient
    Private ReadOnly _cacheDirectory As String
    Private ReadOnly _loads As New ConcurrentDictionary(Of String, Task(Of Bitmap))()

    Public Sub New(http As HttpClient, cacheDirectory As String)
        _http = http
        _cacheDirectory = cacheDirectory
    End Sub

    ''' <summary>
    ''' Shared instance: cache in %LocalAppData%\K-Browser\FaviconCache, 5 second network timeout.
    ''' </summary>
    Public Shared ReadOnly Property [Default] As FaviconService
        Get
            Return _default.Value
        End Get
    End Property

    Public Function GetFaviconAsync(domain As String) As Task(Of Bitmap)
        If domain Is Nothing OrElse Not HostPattern.IsMatch(domain) Then Return Task.FromResult(Of Bitmap)(Nothing)
        Return _loads.GetOrAdd(domain.ToLowerInvariant(), AddressOf LoadAsync)
    End Function

    Private Async Function LoadAsync(domain As String) As Task(Of Bitmap)
        ' Task.Run keeps disk and network work off the UI thread and guarantees this completes
        ' after GetOrAdd has stored it, so the TryRemove below always finds its own entry.
        Dim result = Await Task.Run(Function() LoadCoreAsync(domain)).ConfigureAwait(False)
        If result Is Nothing Then _loads.TryRemove(domain, Nothing)
        Return result
    End Function

    Private Async Function LoadCoreAsync(domain As String) As Task(Of Bitmap)
        Dim cacheFile = Path.Combine(_cacheDirectory, domain & ".png")
        Try
            If File.Exists(cacheFile) Then
                Dim cached = Decode(File.ReadAllBytes(cacheFile))
                If cached IsNot Nothing Then Return cached
            End If

            Using response = Await _http.GetAsync("https://" & domain & "/favicon.ico").ConfigureAwait(False)
                If Not response.IsSuccessStatusCode Then Return Nothing
                Dim icon = Decode(Await response.Content.ReadAsByteArrayAsync().ConfigureAwait(False))
                If icon Is Nothing Then Return Nothing

                Directory.CreateDirectory(_cacheDirectory)
                Dim tempFile = cacheFile & "." & Guid.NewGuid().ToString("N") & ".tmp"
                icon.Save(tempFile, ImageFormat.Png)
                File.Move(tempFile, cacheFile, overwrite:=True)
                Return icon
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("FaviconService: " & domain & ": " & ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Decodes PNG/ICO/GIF/BMP bytes into a standalone bitmap, or Nothing if they aren't an image.
    ''' </summary>
    Private Shared Function Decode(bytes As Byte()) As Bitmap
        If bytes Is Nothing OrElse bytes.Length = 0 Then Return Nothing
        Try
            Using ms As New MemoryStream(bytes), img = Image.FromStream(ms)
                ' Copy so the bitmap doesn't depend on the stream (GDI+ keeps reading it otherwise).
                Return New Bitmap(img)
            End Using
        Catch ex As ArgumentException
        End Try
        Try
            Using ms As New MemoryStream(bytes), ico As New Icon(ms)
                Return ico.ToBitmap()
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

End Class
