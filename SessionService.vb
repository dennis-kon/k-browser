Imports System
Imports System.IO
Imports System.Text.Json
Imports System.Threading.Tasks

''' <summary>
''' Dedicated service managing browser session persistence in Session.json.
''' Responsibilities include saving, restoring, deleting, and gracefully handling corrupted session files.
''' </summary>
Public Class SessionService
    Private ReadOnly _sessionFilePath As String

    Public Sub New()
        Dim appDataFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser")
        If Not Directory.Exists(appDataFolder) Then
            Directory.CreateDirectory(appDataFolder)
        End If
        _sessionFilePath = Path.Combine(appDataFolder, "Session.json")
    End Sub

    Public Sub New(ByVal customPath As String)
        _sessionFilePath = customPath
    End Sub

    ''' <summary>
    ''' Gets the absolute file path to Session.json.
    ''' </summary>
    Public ReadOnly Property SessionFilePath As String
        Get
            Return _sessionFilePath
        End Get
    End Property

    ''' <summary>
    ''' Synchronously loads the saved browser session from Session.json.
    ''' Safe for use during FormClosing and UI thread events to prevent deadlock.
    ''' </summary>
    Public Function LoadSession() As SessionModel
        Try
            If Not File.Exists(_sessionFilePath) Then Return New SessionModel()
            Dim json As String = File.ReadAllText(_sessionFilePath)
            Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True
            }
            Dim session As SessionModel = JsonSerializer.Deserialize(Of SessionModel)(json, options)
            If session IsNot Nothing AndAlso session.Tabs IsNot Nothing Then
                Return session
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SessionService: Failed to read or parse Session.json (corrupted): " & ex.Message)
        End Try

        Return New SessionModel()
    End Function

    ''' <summary>
    ''' Asynchronously loads the saved browser session from Session.json.
    ''' Returns an empty SessionModel if the file is missing or corrupted.
    ''' </summary>
    Public Async Function LoadSessionAsync() As Task(Of SessionModel)
        Try
            If Not File.Exists(_sessionFilePath) Then
                Return New SessionModel()
            End If

            Using stream As New FileStream(_sessionFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, True)
                Dim options As New JsonSerializerOptions With {
                    .PropertyNameCaseInsensitive = True
                }
                Dim session As SessionModel = Await JsonSerializer.DeserializeAsync(Of SessionModel)(stream, options)
                If session IsNot Nothing AndAlso session.Tabs IsNot Nothing Then
                    Return session
                End If
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SessionService: Failed to read or parse Session.json (corrupted): " & ex.Message)
        End Try

        ' Fallback to clean empty session if corrupted or missing
        Return New SessionModel()
    End Function

    ''' <summary>
    ''' Synchronously saves the active browser session to Session.json.
    ''' Safe for use during FormClosing and UI thread events to prevent deadlock.
    ''' </summary>
    Public Sub SaveSession(ByVal session As SessionModel)
        If session Is Nothing Then Return

        Try
            Dim dir As String = Path.GetDirectoryName(_sessionFilePath)
            If Not String.IsNullOrEmpty(dir) AndAlso Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Dim options As New JsonSerializerOptions With {
                .WriteIndented = True
            }
            Dim json As String = JsonSerializer.Serialize(session, options)
            File.WriteAllText(_sessionFilePath, json)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SessionService: Failed to save Session.json synchronously: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Asynchronously saves the active browser session to Session.json.
    ''' </summary>
    Public Async Function SaveSessionAsync(ByVal session As SessionModel) As Task
        If session Is Nothing Then Throw New ArgumentNullException(NameOf(session))

        Try
            Dim dir As String = Path.GetDirectoryName(_sessionFilePath)
            If Not String.IsNullOrEmpty(dir) AndAlso Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Using stream As New FileStream(_sessionFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, True)
                Dim options As New JsonSerializerOptions With {
                    .WriteIndented = True
                }
                Await JsonSerializer.SerializeAsync(stream, session, options)
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SessionService: Failed to save Session.json: " & ex.Message)
            Throw New IOException("Failed to save session state to " & _sessionFilePath & ": " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Asynchronously deletes the saved Session.json file.
    ''' </summary>
    Public Async Function DeleteSessionAsync() As Task
        Try
            If File.Exists(_sessionFilePath) Then
                Await Task.Run(Sub() File.Delete(_sessionFilePath))
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SessionService: Error deleting Session.json: " & ex.Message)
        End Try
    End Function
End Class
