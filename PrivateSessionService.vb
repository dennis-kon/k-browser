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
    Private ReadOnly _rootFolder As String
    Private _lockStream As FileStream
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
        Me.New(DefaultRoot)
    End Sub

    ''' <summary>Creates a session whose profile folder lives under <paramref name="rootFolder"/>.</summary>
    Public Sub New(ByVal rootFolder As String)
        _sessionId = Guid.NewGuid().ToString("N")
        _rootFolder = rootFolder
        _sessionFolder = Path.Combine(rootFolder, _sessionId)
    End Sub

    ''' <summary>The folder that holds every private session's profile folder and lock file.</summary>
    Public Shared ReadOnly Property DefaultRoot As String
        Get
            Return Path.Combine(Path.GetTempPath(), "KBrowser", "PrivateSession")
        End Get
    End Property

    ''' <summary>
    ''' Takes an exclusive lock on "{id}.lock" next to the session folder for as long as the session lives.
    ''' The operating system drops the lock if the process dies, which is how
    ''' <see cref="SweepStaleSessions(String)"/> tells a crashed session from one that is still open.
    ''' </summary>
    Public Sub AcquireSessionLock()
        If _lockStream IsNot Nothing Then Return
        Directory.CreateDirectory(_rootFolder)
        _lockStream = New FileStream(Path.Combine(_rootFolder, _sessionId & ".lock"), FileMode.OpenOrCreate,
                                     FileAccess.ReadWrite, FileShare.None)
    End Sub

    ''' <summary>
    ''' Lets go of the session lock. The lock file itself is removed only with the session folder: a lock file
    ''' nobody holds is what tells the start-up sweep that a leftover folder may be deleted.
    ''' </summary>
    Private Sub ReleaseSessionLock(Optional ByVal deleteLockFile As Boolean = True)
        If _lockStream Is Nothing Then Return
        Try
            _lockStream.Dispose()
            If deleteLockFile Then File.Delete(Path.Combine(_rootFolder, _sessionId & ".lock"))
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PrivateSessionService: Could not release the session lock: " & ex.Message)
        Finally
            _lockStream = Nothing
        End Try
    End Sub

    ''' <summary>
    ''' Removes private-session folders left behind by a crash: those whose lock file is no longer held, and
    ''' folders from older versions (no lock file) untouched for more than a day. A session that is still open
    ''' holds its lock and is never touched. Returns how many session folders were removed.
    ''' </summary>
    Public Shared Function SweepStaleSessions(ByVal rootFolder As String) As Integer
        Return SweepStaleSessions(rootFolder, TimeSpan.FromHours(24))
    End Function

    Public Shared Function SweepStaleSessions(ByVal rootFolder As String, ByVal legacyMaxAge As TimeSpan) As Integer
        Dim removed As Integer = 0
        If String.IsNullOrEmpty(rootFolder) OrElse Not Directory.Exists(rootFolder) Then Return 0

        Try
            For Each folder As String In Directory.GetDirectories(rootFolder)
                Try
                    Dim lockPath As String = Path.Combine(rootFolder, Path.GetFileName(folder) & ".lock")
                    If File.Exists(lockPath) Then
                        Dim held As FileStream = TryTakeLock(lockPath)
                        If held Is Nothing Then Continue For ' a running session owns it
                        Try
                            Directory.Delete(folder, True)
                            removed += 1
                        Finally
                            held.Dispose()
                        End Try
                        File.Delete(lockPath)
                    ElseIf DateTime.UtcNow - Directory.GetLastWriteTimeUtc(folder) > legacyMaxAge Then
                        Directory.Delete(folder, True)
                        removed += 1
                    End If
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine("PrivateSessionService: Could not sweep a stale session: " & ex.Message)
                End Try
            Next

            ' Lock files whose session folder is already gone.
            For Each lockPath As String In Directory.GetFiles(rootFolder, "*.lock")
                Try
                    If Directory.Exists(Path.Combine(rootFolder, Path.GetFileNameWithoutExtension(lockPath))) Then Continue For
                    Dim held As FileStream = TryTakeLock(lockPath)
                    If held Is Nothing Then Continue For
                    held.Dispose()
                    File.Delete(lockPath)
                Catch ex As Exception
                    System.Diagnostics.Debug.WriteLine("PrivateSessionService: Could not remove an orphan lock: " & ex.Message)
                End Try
            Next
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PrivateSessionService: Sweep failed: " & ex.Message)
        End Try
        Return removed
    End Function

    ''' <summary>Opens the lock file exclusively, or returns Nothing when another session holds it.</summary>
    Private Shared Function TryTakeLock(ByVal lockPath As String) As FileStream
        Try
            Return New FileStream(lockPath, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
        Catch ex As IOException
            Return Nothing
        End Try
    End Function

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
            ' Claim the session first, so a sweep from another instance never sees the folder unowned.
            AcquireSessionLock()

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
    ''' Ends the private session in the only order that lets its profile folder be deleted:
    ''' clear the browsing data while the browser still runs, shut the browser down (its process keeps files in
    ''' the folder open until it has exited), then delete the folder. A step that fails does not stop the
    ''' ones after it. Returns True when the folder is gone; otherwise it is left, unlocked, for the
    ''' start-up sweep (<see cref="SweepStaleSessions(String)"/>) to remove.
    ''' </summary>
    Public Async Function EndSessionAsync(ByVal clearBrowsingData As Func(Of Task), ByVal shutDownBrowser As Func(Of Task)) As Task(Of Boolean)
        Await RunStepAsync(clearBrowsingData, "clearing browsing data")
        Await RunStepAsync(shutDownBrowser, "shutting down the browser")
        _environment = Nothing

        Dim deleted As Boolean = Await DeleteSessionFolderAsync()
        ReleaseSessionLock(deleteLockFile:=deleted)
        Return deleted
    End Function

    Private Shared Async Function RunStepAsync(ByVal [step] As Func(Of Task), ByVal what As String) As Task
        If [step] Is Nothing Then Return
        Try
            Await [step]()
        Catch ex As Exception
            ' Log technical error only — never log private URLs or session content
            System.Diagnostics.Debug.WriteLine("PrivateSessionService: Error " & what & ": " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Disposes the browser control (via <paramref name="closeBrowser"/>) and waits until the session's browser
    ''' process has exited, or until <paramref name="timeout"/> passes.
    ''' </summary>
    Public Shared Async Function ShutDownBrowserAsync(ByVal environment As CoreWebView2Environment, ByVal closeBrowser As Action,
                                                      ByVal timeout As TimeSpan) As Task
        If environment Is Nothing Then
            closeBrowser()
            Return
        End If

        Dim exited As New TaskCompletionSource(Of Boolean)(TaskCreationOptions.RunContinuationsAsynchronously)
        Dim onExit As EventHandler(Of CoreWebView2BrowserProcessExitedEventArgs) = Sub(s, e) exited.TrySetResult(True)
        AddHandler environment.BrowserProcessExited, onExit
        Try
            closeBrowser()
            Await Task.WhenAny(exited.Task, Task.Delay(timeout))
        Finally
            RemoveHandler environment.BrowserProcessExited, onExit
        End Try
    End Function

    ''' <summary>
    ''' Deletes the session folder, retrying briefly in case a browser helper process is still letting go of a
    ''' file. Returns True when the folder no longer exists.
    ''' </summary>
    Private Async Function DeleteSessionFolderAsync() As Task(Of Boolean)
        If String.IsNullOrEmpty(_sessionFolder) OrElse Not Directory.Exists(_sessionFolder) Then
            Return True
        End If

        Const MaxRetries As Integer = 3
        For attempt As Integer = 1 To MaxRetries
            Try
                Directory.Delete(_sessionFolder, recursive:=True)
                Return True
            Catch ex As IOException
                System.Diagnostics.Debug.WriteLine("PrivateSessionService: Folder deletion attempt " & attempt & " failed (IO): " & ex.Message)
            Catch ex As UnauthorizedAccessException
                System.Diagnostics.Debug.WriteLine("PrivateSessionService: Folder deletion attempt " & attempt & " failed (Access): " & ex.Message)
            End Try

            ' Delay outside the Catch block (VB.NET does not allow Await inside Catch)
            If attempt < MaxRetries Then Await Task.Delay(250 * attempt)
        Next
        Return Not Directory.Exists(_sessionFolder)
    End Function

    ''' <summary>
    ''' Disposes the private session service and releases the environment reference.
    ''' Note: Actual cleanup (ClearBrowsingDataAsync + folder deletion) should be called
    ''' via EndSessionAsync before disposal.
    ''' </summary>
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not _disposed Then
            If disposing Then
                _environment = Nothing
                ReleaseSessionLock()
            End If
            _disposed = True
        End If
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

End Class
