Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Compression
Imports System.Text.Json
Imports System.Threading.Tasks

''' <summary>
''' Dedicated service managing the creation of compressed ZIP backups of browser data.
''' Bundles Settings.json, Session.json, Bookmarks, History, and user preferences.
''' </summary>
Public Class BackupService
    Private ReadOnly _settingsService As SettingsService

    Public Sub New()
        _settingsService = New SettingsService()
    End Sub

    Public Sub New(ByVal settingsService As SettingsService)
        _settingsService = If(settingsService, New SettingsService())
    End Sub

    ''' <summary>
    ''' Generates the standard default backup file name: BrowserBackup_YYYY-MM-DD.zip.
    ''' </summary>
    Public Shared Function GetDefaultBackupFileName() As String
        Return $"BrowserBackup_{DateTime.Now:yyyy-MM-dd}.zip"
    End Function

    ''' <summary>
    ''' Asynchronously creates a compressed ZIP backup at the specified destination path.
    ''' </summary>
    ''' <param name="zipDestinationPath">Target file path for the backup archive</param>
    ''' <returns>The timestamp of the completed backup</returns>
    Public Async Function CreateBackupAsync(ByVal zipDestinationPath As String) As Task(Of DateTime)
        If String.IsNullOrWhiteSpace(zipDestinationPath) Then
            Throw New ArgumentException("Backup destination file path cannot be empty.", NameOf(zipDestinationPath))
        End If

        Dim backupTime As DateTime = DateTime.Now

        Try
            ' Temporary workspace directory inside %LocalAppData%\K-Browser\TempBackup
            Dim tempDir As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "TempBackup_" & Guid.NewGuid().ToString("N"))
            If Directory.Exists(tempDir) Then Directory.Delete(tempDir, True)
            Directory.CreateDirectory(tempDir)

            ' 1. Copy Settings.json if present
            Dim settingsFile As String = _settingsService.SettingsFilePath
            If File.Exists(settingsFile) Then
                File.Copy(settingsFile, Path.Combine(tempDir, "Settings.json"), True)
            End If

            ' 2. Copy Session.json if present
            Dim sessionSvc As New SessionService()
            If File.Exists(sessionSvc.SessionFilePath) Then
                File.Copy(sessionSvc.SessionFilePath, Path.Combine(tempDir, "Session.json"), True)
            End If

            ' 3. Export Bookmarks to bookmarks.json
            Dim bookmarksList As New List(Of String)()
            If My.Settings.BookmarksTreeData IsNot Nothing Then
                For Each b As String In My.Settings.BookmarksTreeData
                    If Not String.IsNullOrWhiteSpace(b) Then bookmarksList.Add(b)
                Next
            End If
            Dim jsonOptions As New JsonSerializerOptions With {.WriteIndented = True}
            Dim bookmarksJson As String = JsonSerializer.Serialize(bookmarksList, jsonOptions)
            File.WriteAllText(Path.Combine(tempDir, "bookmarks.json"), bookmarksJson)

            ' 4. Export History to history.json
            Dim historyList As New List(Of String)()
            If My.Settings.History IsNot Nothing Then
                For Each h As String In My.Settings.History
                    If Not String.IsNullOrWhiteSpace(h) Then historyList.Add(h)
                Next
            End If
            Dim historyJson As String = JsonSerializer.Serialize(historyList, jsonOptions)
            File.WriteAllText(Path.Combine(tempDir, "history.json"), historyJson)

            ' 5. Export User Configuration / Preferences to user_config.json
            Dim userConfig As New Dictionary(Of String, String) From {
                {"HomePageUrl", My.Settings.HomePageUrl},
                {"StartupBehavior", My.Settings.StartupBehavior.ToString()},
                {"NewTabPage", My.Settings.NewTabPage.ToString()},
                {"SearchEngine", My.Settings.SearchEngine},
                {"DownloadsFolder", My.Settings.DownloadsFolder},
                {"FontSize", My.Settings.FontSize.ToString()},
                {"FullScreenOnStartup", My.Settings.FullScreenOnStartup.ToString()},
                {"PermissionCamera", My.Settings.PermissionCamera.ToString()},
                {"PermissionMic", My.Settings.PermissionMic.ToString()},
                {"PermissionLocation", My.Settings.PermissionLocation.ToString()},
                {"PermissionNotifications", My.Settings.PermissionNotifications.ToString()},
                {"IncognitoEnabled", My.Settings.IncognitoEnabled.ToString()},
                {"HttpsOnlyMode", My.Settings.HttpsOnlyMode.ToString()},
                {"AdBlockerEnabled", My.Settings.AdBlockerEnabled.ToString()},
                {"MemorySaverEnabled", My.Settings.MemorySaverEnabled.ToString()},
                {"HardwareAcceleration", My.Settings.HardwareAcceleration.ToString()}
            }
            Dim userConfigJson As String = JsonSerializer.Serialize(userConfig, jsonOptions)
            File.WriteAllText(Path.Combine(tempDir, "user_config.json"), userConfigJson)

            ' Create destination directory if needed
            Dim destDir As String = Path.GetDirectoryName(zipDestinationPath)
            If Not String.IsNullOrEmpty(destDir) AndAlso Not Directory.Exists(destDir) Then
                Directory.CreateDirectory(destDir)
            End If

            If File.Exists(zipDestinationPath) Then
                File.Delete(zipDestinationPath)
            End If

            ' Create ZIP file archive asynchronously
            Await Task.Run(Sub() ZipFile.CreateFromDirectory(tempDir, zipDestinationPath, CompressionLevel.Optimal, False))

            ' Clean up temporary folder
            Try
                Directory.Delete(tempDir, True)
            Catch
            End Try

            ' Update LastBackup timestamp in Settings.json
            Dim settingsModel As PrivacySettingsModel = Await _settingsService.LoadSettingsAsync()
            settingsModel.LastBackup = backupTime
            Await _settingsService.SaveSettingsAsync(settingsModel)

            Return backupTime
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("BackupService: Failed to create backup ZIP archive: " & ex.Message)
            Throw New IOException("Backup operation failed: " & ex.Message, ex)
        End Try
    End Function
End Class
