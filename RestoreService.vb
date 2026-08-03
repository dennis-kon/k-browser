Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.IO.Compression
Imports System.Text.Json
Imports System.Threading.Tasks

''' <summary>
''' Dedicated service managing validation and restoration of browser configuration from backup ZIP archives,
''' as well as resetting browser settings to default values without deleting user data.
''' </summary>
Public Class RestoreService
    Private ReadOnly _settingsService As SettingsService

    Public Sub New()
        _settingsService = New SettingsService()
    End Sub

    Public Sub New(ByVal settingsService As SettingsService)
        _settingsService = If(settingsService, New SettingsService())
    End Sub

    ''' <summary>
    ''' Validates whether the specified file is a valid backup ZIP archive containing expected entries.
    ''' </summary>
    Public Function ValidateBackupArchive(ByVal zipFilePath As String) As Boolean
        If String.IsNullOrWhiteSpace(zipFilePath) OrElse Not File.Exists(zipFilePath) Then
            Return False
        End If

        Try
            Using archive As ZipArchive = ZipFile.OpenRead(zipFilePath)
                ' Backup archive must contain at least one valid browser configuration file
                Return archive.Entries.Any(Function(e) e.Name.Equals("Settings.json", StringComparison.OrdinalIgnoreCase) OrElse
                                                       e.Name.Equals("user_config.json", StringComparison.OrdinalIgnoreCase) OrElse
                                                       e.Name.Equals("bookmarks.json", StringComparison.OrdinalIgnoreCase))
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("RestoreService: ZIP archive validation failed: " & ex.Message)
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Asynchronously restores browser settings, session, bookmarks, history, and preferences from a backup ZIP archive.
    ''' </summary>
    ''' <param name="zipFilePath">Path to the backup ZIP file</param>
    Public Async Function RestoreBackupAsync(ByVal zipFilePath As String) As Task
        If Not ValidateBackupArchive(zipFilePath) Then
            Throw New InvalidDataException("The selected file is not a valid or readable browser backup ZIP archive.")
        End If

        Dim tempDir As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "TempRestore_" & Guid.NewGuid().ToString("N"))
        Try
            If Directory.Exists(tempDir) Then Directory.Delete(tempDir, True)
            Directory.CreateDirectory(tempDir)

            ' Extract archive asynchronously
            Await Task.Run(Sub() ZipFile.ExtractToDirectory(zipFilePath, tempDir, True))

            ' 1. Restore Settings.json
            Dim restoredSettingsFile As String = Path.Combine(tempDir, "Settings.json")
            If File.Exists(restoredSettingsFile) Then
                File.Copy(restoredSettingsFile, _settingsService.SettingsFilePath, True)
            End If

            ' 2. Restore Session.json
            Dim restoredSessionFile As String = Path.Combine(tempDir, "Session.json")
            Dim sessionSvc As New SessionService()
            If File.Exists(restoredSessionFile) Then
                File.Copy(restoredSessionFile, sessionSvc.SessionFilePath, True)
            End If

            ' 3. Restore Bookmarks if included in backup
            Dim restoredBookmarksFile As String = Path.Combine(tempDir, "bookmarks.json")
            If File.Exists(restoredBookmarksFile) Then
                Dim json As String = File.ReadAllText(restoredBookmarksFile)
                Dim bList As List(Of String) = JsonSerializer.Deserialize(Of List(Of String))(json)
                If bList IsNot Nothing Then
                    If My.Settings.BookmarksTreeData Is Nothing Then
                        My.Settings.BookmarksTreeData = New System.Collections.Specialized.StringCollection()
                    End If
                    My.Settings.BookmarksTreeData.Clear()
                    For Each item In bList
                        My.Settings.BookmarksTreeData.Add(item)
                    Next
                End If
            End If

            ' 4. Restore History if included in backup
            Dim restoredHistoryFile As String = Path.Combine(tempDir, "history.json")
            If File.Exists(restoredHistoryFile) Then
                Dim json As String = File.ReadAllText(restoredHistoryFile)
                Dim hList As List(Of String) = JsonSerializer.Deserialize(Of List(Of String))(json)
                If hList IsNot Nothing Then
                    If My.Settings.History Is Nothing Then
                        My.Settings.History = New System.Collections.Specialized.StringCollection()
                    End If
                    My.Settings.History.Clear()
                    For Each item In hList
                        My.Settings.History.Add(item)
                    Next
                End If
            End If

            ' 5. Restore User Preferences if included in backup
            Dim restoredConfigFile As String = Path.Combine(tempDir, "user_config.json")
            If File.Exists(restoredConfigFile) Then
                Dim json As String = File.ReadAllText(restoredConfigFile)
                Dim cfg As Dictionary(Of String, String) = JsonSerializer.Deserialize(Of Dictionary(Of String, String))(json)
                If cfg IsNot Nothing Then
                    If cfg.ContainsKey("HomePageUrl") Then My.Settings.HomePageUrl = cfg("HomePageUrl")
                    If cfg.ContainsKey("SearchEngine") Then My.Settings.SearchEngine = cfg("SearchEngine")
                    If cfg.ContainsKey("DownloadsFolder") Then My.Settings.DownloadsFolder = cfg("DownloadsFolder")
                    If cfg.ContainsKey("FontSize") Then My.Settings.FontSize = CInt(cfg("FontSize"))
                    If cfg.ContainsKey("FullScreenOnStartup") Then My.Settings.FullScreenOnStartup = CBool(cfg("FullScreenOnStartup"))
                    If cfg.ContainsKey("PermissionCamera") Then My.Settings.PermissionCamera = CBool(cfg("PermissionCamera"))
                    If cfg.ContainsKey("PermissionMic") Then My.Settings.PermissionMic = CBool(cfg("PermissionMic"))
                    If cfg.ContainsKey("PermissionLocation") Then My.Settings.PermissionLocation = CBool(cfg("PermissionLocation"))
                    If cfg.ContainsKey("PermissionNotifications") Then My.Settings.PermissionNotifications = CBool(cfg("PermissionNotifications"))
                    If cfg.ContainsKey("IncognitoEnabled") Then My.Settings.IncognitoEnabled = CBool(cfg("IncognitoEnabled"))
                    If cfg.ContainsKey("HttpsOnlyMode") Then My.Settings.HttpsOnlyMode = CBool(cfg("HttpsOnlyMode"))
                    If cfg.ContainsKey("AdBlockerEnabled") Then My.Settings.AdBlockerEnabled = CBool(cfg("AdBlockerEnabled"))
                    If cfg.ContainsKey("MemorySaverEnabled") Then My.Settings.MemorySaverEnabled = CBool(cfg("MemorySaverEnabled"))
                    If cfg.ContainsKey("HardwareAcceleration") Then My.Settings.HardwareAcceleration = CBool(cfg("HardwareAcceleration"))
                End If
            End If

            My.Settings.Save()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("RestoreService: Restore process failed: " & ex.Message)
            Throw New Exception("Restore process failed: " & ex.Message, ex)
        Finally
            Try
                If Directory.Exists(tempDir) Then Directory.Delete(tempDir, True)
            Catch
            End Try
        End Try
    End Function

    ''' <summary>
    ''' Resets browser settings to default values.
    ''' Does NOT delete bookmarks, downloaded files, or user data.
    ''' </summary>
    Public Async Function ResetSettingsAsync() As Task
        Try
            ' Reset My.Settings defaults
            My.Settings.HomePageUrl = "https://www.google.com"
            My.Settings.StartupBehavior = 0
            My.Settings.NewTabPage = 0
            My.Settings.SearchEngine = "Google"
            My.Settings.DownloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads")
            My.Settings.FontSize = 1
            My.Settings.FullScreenOnStartup = False
            My.Settings.PermissionCamera = True
            My.Settings.PermissionMic = True
            My.Settings.PermissionLocation = True
            My.Settings.PermissionNotifications = True
            My.Settings.IncognitoEnabled = False
            My.Settings.HttpsOnlyMode = False
            My.Settings.AdBlockerEnabled = True
            My.Settings.MemorySaverEnabled = True
            My.Settings.HardwareAcceleration = True
            My.Settings.DnsOverHttpsProvider = 0
            My.Settings.CustomDnsServer = ""
            My.Settings.CustomProxyEnabled = False
            My.Settings.CustomProxyHost = ""
            My.Settings.CustomProxyPort = 8080
            My.Settings.Save()

            ' Reset Settings.json preferences to defaults
            Dim defaultSettings As New PrivacySettingsModel With {
                .BlockThirdPartyCookies = False,
                .StartupMode = "NewTab",
                .LastBackup = Nothing
            }
            Await _settingsService.SaveSettingsAsync(defaultSettings)

            SettingsManager.NotifySettingsChanged()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("RestoreService: Failed to reset settings to defaults: " & ex.Message)
            Throw New Exception("Failed to reset settings to default values: " & ex.Message, ex)
        End Try
    End Function

End Class
