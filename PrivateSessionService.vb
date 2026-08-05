Imports System.IO
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Application-layer service responsible for managing the lifecycle of a private browsing session.
''' Creates an isolated WebView2 environment with a temporary user data folder,
''' and ensures complete data cleanup when the session ends.
''' 
''' This service owns all private session logic — UI forms delegate to it,
''' keeping business rules out of the presentation layer.
''' </summary>
Public Class PrivateSessionService
    Implements IDisposable

    Private ReadOnly _sessionId As String
    Private ReadOnly _sessionFolder As String
    Private _environment As CoreWebView2Environment
    Private _disposed As Boolean

    ''' <summary>
    ''' Gets the unique identifier for this private session.
    ''' </summary>
    Public ReadOnly Property SessionId As String
        Get
            Return _sessionId
        End Get
    End Property

    ''' <summary>
    ''' Gets the temporary user data folder path for this private session.
    ''' Located under %TEMP%\KBrowser\PrivateSession\{GUID} to ensure
    ''' complete isolation from the normal browser profile at %LOCALAPPDATA%\K-Browser\UserData.
    ''' </summary>
    Public ReadOnly Property SessionFolder As String
        Get
            Return _sessionFolder
        End Get
    End Property

    ''' <summary>
    ''' Gets the isolated CoreWebView2Environment for this private session.
    ''' Returns Nothing if the environment has not been created yet.
    ''' </summary>
    Public ReadOnly Property Environment As CoreWebView2Environment
        Get
            Return _environment
        End Get
    End Property

    ''' <summary>
    ''' Indicates whether this service represents an active private session.
    ''' </summary>
    Public ReadOnly Property IsPrivateSession As Boolean
        Get
            Return _environment IsNot Nothing AndAlso Not _disposed
        End Get
    End Property

    ''' <summary>
    ''' Creates a new PrivateSessionService with a unique GUID-based session identifier
    ''' and a corresponding temporary folder path.
    ''' </summary>
    Public Sub New()
        _sessionId = Guid.NewGuid().ToString("N")
        _sessionFolder = Path.Combine(Path.GetTempPath(), "KBrowser", "PrivateSession", _sessionId)
    End Sub

    ''' <summary>
    ''' Creates the isolated CoreWebView2Environment for this private session.
    ''' The environment uses a temporary user data folder that is completely separate
    ''' from the normal browser profile. This ensures separate cookies, cache,
    ''' local storage, session storage, IndexedDB, and browser history.
    ''' </summary>
    ''' <returns>The created CoreWebView2Environment, or Nothing if creation fails.</returns>
    Public Async Function CreatePrivateEnvironmentAsync() As Task(Of CoreWebView2Environment)
        If _environment IsNot Nothing Then
            Return _environment
        End If

        Try
            ' Create the temporary session folder
            If Not Directory.Exists(_sessionFolder) Then
                Directory.CreateDirectory(_sessionFolder)
            End If

            ' Build environment options with privacy-hardened switches
            Dim options As New CoreWebView2EnvironmentOptions()
            Dim switches As New System.Text.StringBuilder()

            ' Block third-party cookies by default in private mode
            switches.Append("--block-third-party-cookies ")

            ' Enable site isolation for security
            switches.Append("--enable-features=IsolateOrigins,site-per-process --disable-features=SingleProcess ")

            ' Disable autofill and password saving in private mode
            switches.Append("--disable-features=AutofillServerCommunication ")

            Dim finalSwitches As String = switches.ToString().Trim()
            If Not String.IsNullOrEmpty(finalSwitches) Then
                options.AdditionalBrowserArguments = finalSwitches
            End If

            ' Create the isolated environment with the temporary user data folder
            ' This is the key to true private browsing — a completely separate WebView2 profile
            _environment = Await CoreWebView2Environment.CreateAsync(Nothing, _sessionFolder, options)

            Return _environment
        Catch ex As Exception
            ' Log technical error only — never log private session details
            System.Diagnostics.Debug.WriteLine("PrivateSessionService: Failed to create private environment: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Performs complete cleanup of the private session.
    ''' First clears all browsing data via the WebView2 API, then deletes
    ''' the temporary session folder and all its contents.
    ''' </summary>
    ''' <param name="webView">Optional WebView2 control to clear browsing data from before folder deletion.</param>
    Public Async Function CleanupSessionAsync(Optional ByVal webView As WebView2 = Nothing) As Task
        Try
            ' Step 1: Clear browsing data via WebView2 API if a WebView is available
            If webView IsNot Nothing AndAlso webView.CoreWebView2 IsNot Nothing Then
                Try
                    Await webView.CoreWebView2.Profile.ClearBrowsingDataAsync()
                Catch ex As Exception
                    ' Browsing data clear failed — continue with folder deletion
                    System.Diagnostics.Debug.WriteLine("PrivateSessionService: ClearBrowsingDataAsync failed: " & ex.Message)
                End Try
            End If

            ' Step 2: Delete the temporary session folder with retry logic
            Await DeleteSessionFolderAsync()
        Catch ex As Exception
            ' Log technical error only — never log any private URLs or session content
            System.Diagnostics.Debug.WriteLine("PrivateSessionService: Cleanup error: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Attempts to delete the temporary session folder with retry logic.
    ''' WebView2 processes may still hold file locks briefly after the environment is released,
    ''' so we retry up to 3 times with increasing delays.
    ''' </summary>
    Private Async Function DeleteSessionFolderAsync() As Task
        If String.IsNullOrEmpty(_sessionFolder) OrElse Not Directory.Exists(_sessionFolder) Then
            Return
        End If

        Const MaxRetries As Integer = 3
        For attempt As Integer = 1 To MaxRetries
            Dim shouldRetry As Boolean = False
            Try
                Directory.Delete(_sessionFolder, recursive:=True)
                System.Diagnostics.Debug.WriteLine("PrivateSessionService: Session folder deleted successfully.")
                Return
            Catch ex As IOException
                ' File locks from WebView2 processes — wait and retry
                System.Diagnostics.Debug.WriteLine("PrivateSessionService: Folder deletion attempt " & attempt & " failed (IO): " & ex.Message)
                shouldRetry = (attempt < MaxRetries)
            Catch ex As UnauthorizedAccessException
                ' Permission issue — wait and retry
                System.Diagnostics.Debug.WriteLine("PrivateSessionService: Folder deletion attempt " & attempt & " failed (Access): " & ex.Message)
                shouldRetry = (attempt < MaxRetries)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("PrivateSessionService: Folder deletion failed: " & ex.Message)
                Return
            End Try

            ' Delay outside the Catch block (VB.NET does not allow Await inside Catch)
            If shouldRetry Then
                Await Task.Delay(500 * attempt)
            End If
        Next

        ' If all retries failed, schedule a deferred cleanup attempt
        Try
            Task.Run(Async Function()
                         Await Task.Delay(5000)
                         Try
                             If Directory.Exists(_sessionFolder) Then
                                 Directory.Delete(_sessionFolder, recursive:=True)
                             End If
                         Catch
                             ' Final cleanup failed — folder will be cleaned by OS temp cleanup
                         End Try
                     End Function)
        Catch
            ' Silently ignore if we cannot schedule deferred cleanup
        End Try
    End Function

    ''' <summary>
    ''' Disposes the private session service and releases the environment reference.
    ''' Note: Actual cleanup (ClearBrowsingDataAsync + folder deletion) should be called
    ''' via CleanupSessionAsync before disposal.
    ''' </summary>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                _environment = Nothing
            End If
            _disposed = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

End Class
