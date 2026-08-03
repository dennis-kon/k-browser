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
            If Not File.Exists(_settingsFilePath) Then Return New PrivacySettingsModel()
            Dim json As String = File.ReadAllText(_settingsFilePath)
            Dim options As New JsonSerializerOptions With {
                .PropertyNameCaseInsensitive = True
            }
            Dim result As PrivacySettingsModel = JsonSerializer.Deserialize(Of PrivacySettingsModel)(json, options)
            Return If(result, New PrivacySettingsModel())
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
            If Not File.Exists(_settingsFilePath) Then
                Return New PrivacySettingsModel()
            End If

            Using stream As FileStream = New FileStream(_settingsFilePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, True)
                Dim options As New JsonSerializerOptions With {
                    .PropertyNameCaseInsensitive = True
                }
                Dim result As PrivacySettingsModel = Await JsonSerializer.DeserializeAsync(Of PrivacySettingsModel)(stream, options)
                Return If(result, New PrivacySettingsModel())
            End Using
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

            Dim options As New JsonSerializerOptions With {
                .WriteIndented = True
            }
            Dim json As String = JsonSerializer.Serialize(model, options)
            File.WriteAllText(_settingsFilePath, json)
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

            Using stream As FileStream = New FileStream(_settingsFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 4096, True)
                Dim options As New JsonSerializerOptions With {
                    .WriteIndented = True
                }
                Await JsonSerializer.SerializeAsync(stream, model, options)
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SettingsService: Error writing Settings.json: " & ex.Message)
            Throw New IOException("Failed to save settings to " & _settingsFilePath & ": " & ex.Message, ex)
        End Try
    End Function
End Class
