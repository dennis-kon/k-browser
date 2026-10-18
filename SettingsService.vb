Imports System
Imports System.IO
Imports System.Text.Json
Imports System.Threading.Tasks

''' <summary>
''' Infrastructure service handling read and write operations for Settings.json.
''' Supports both async and synchronous operations to prevent deadlocks on UI events.
''' </summary>
Public Class SettingsService
    Private ReadOnly _settingsFilePath As String

    Private Shared ReadOnly ReadOptions As New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
    Private Shared ReadOnly WriteOptions As New JsonSerializerOptions With {.WriteIndented = True}

    ' Settings.json is read on every page load and for every new tab. Its text is kept here (per file) and
    ' only read from disk again when the file's time stamp or size changes; each caller still gets a fresh copy.
    Private Class CachedFile
        Public Json As String
        Public Stamp As DateTime
        Public Length As Long
    End Class
    Private Shared ReadOnly Cache As New Dictionary(Of String, CachedFile)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly CacheLock As New Object()

    Public Sub New()
        Dim appDataFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser")
        If Not Directory.Exists(appDataFolder) Then
            Directory.CreateDirectory(appDataFolder)
        End If
        _settingsFilePath = Path.Combine(appDataFolder, "Settings.json")
    End Sub

    Public Sub New(ByVal customPath As String)
        _settingsFilePath = customPath
    End Sub

    ''' <summary>
    ''' Gets the absolute file system path to Settings.json.
    ''' </summary>
    Public ReadOnly Property SettingsFilePath As String
        Get
            Return _settingsFilePath
        End Get
    End Property

    ''' <summary>
    ''' Synchronously loads privacy settings from Settings.json.
    ''' Safe for use in FormClosing and synchronous event handlers.
    ''' </summary>
    Public Function LoadSettings() As PrivacySettingsModel
        Try
            Dim info As New FileInfo(_settingsFilePath)
            If Not info.Exists Then Return New PrivacySettingsModel()

            Dim json As String = CachedJson(info)
            If json Is Nothing Then
                json = File.ReadAllText(_settingsFilePath)
                Remember(info, json)
            End If
            Return Parse(json)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SettingsService: Error reading Settings.json synchronously: " & ex.Message)
            Return New PrivacySettingsModel()
        End Try
    End Function

    ''' <summary>
    ''' Asynchronously loads privacy settings from Settings.json.
    ''' Returns default settings if the file does not exist or fails to parse.
    ''' </summary>
    Public Async Function LoadSettingsAsync() As Task(Of PrivacySettingsModel)
        Try
            Dim info As New FileInfo(_settingsFilePath)
            If Not info.Exists Then Return New PrivacySettingsModel()

            Dim json As String = CachedJson(info)
            If json Is Nothing Then
                Using reader As New StreamReader(New FileStream(_settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, True))
                    json = Await reader.ReadToEndAsync()
                End Using
                Remember(info, json)
            End If
            Return Parse(json)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SettingsService: Error reading Settings.json: " & ex.Message)
            Return New PrivacySettingsModel()
        End Try
    End Function

    ''' <summary>
    ''' Synchronously saves privacy settings to Settings.json with formatted indentation.
    ''' </summary>
    Public Sub SaveSettings(ByVal model As PrivacySettingsModel)
        If model Is Nothing Then Return

        Try
            Dim dir As String = Path.GetDirectoryName(_settingsFilePath)
            If Not String.IsNullOrEmpty(dir) AndAlso Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Dim json As String = JsonSerializer.Serialize(model, WriteOptions)
            File.WriteAllText(_settingsFilePath, json)
            Remember(New FileInfo(_settingsFilePath), json)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SettingsService: Error writing Settings.json synchronously: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Asynchronously saves privacy settings to Settings.json with formatted indentation.
    ''' </summary>
    Public Async Function SaveSettingsAsync(ByVal model As PrivacySettingsModel) As Task
        If model Is Nothing Then Throw New ArgumentNullException(NameOf(model))

        Try
            Dim dir As String = Path.GetDirectoryName(_settingsFilePath)
            If Not String.IsNullOrEmpty(dir) AndAlso Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Dim json As String = JsonSerializer.Serialize(model, WriteOptions)
            Using writer As New StreamWriter(New FileStream(_settingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, True))
                Await writer.WriteAsync(json)
            End Using
            Remember(New FileInfo(_settingsFilePath), json)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SettingsService: Error writing Settings.json: " & ex.Message)
            Throw New IOException("Failed to save settings to " & _settingsFilePath & ": " & ex.Message, ex)
        End Try
    End Function

    Private Shared Function Parse(ByVal json As String) As PrivacySettingsModel
        Return If(JsonSerializer.Deserialize(Of PrivacySettingsModel)(json, ReadOptions), New PrivacySettingsModel())
    End Function

    ''' <summary>The remembered text of the file, or Nothing when the file changed since (or was never read).</summary>
    Private Function CachedJson(ByVal info As FileInfo) As String
        SyncLock CacheLock
            Dim entry As CachedFile = Nothing
            If Cache.TryGetValue(_settingsFilePath, entry) AndAlso entry.Stamp = info.LastWriteTimeUtc AndAlso entry.Length = info.Length Then
                Return entry.Json
            End If
        End SyncLock
        Return Nothing
    End Function

    Private Sub Remember(ByVal info As FileInfo, ByVal json As String)
        info.Refresh()
        If Not info.Exists Then Return
        SyncLock CacheLock
            Cache(_settingsFilePath) = New CachedFile With {.Json = json, .Stamp = info.LastWriteTimeUtc, .Length = info.Length}
        End SyncLock
    End Sub
End Class
