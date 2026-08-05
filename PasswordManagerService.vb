Imports System.IO
Imports System.Text
Imports System.Text.Json
Imports System.Threading.Tasks

''' <summary>
''' Application-layer service for managing saved credentials.
''' Handles CRUD operations, search, blocked sites, and coordinates with SecureStorageService
''' for all encryption/decryption.
''' 
''' Storage:
'''   Credentials: %LOCALAPPDATA%\K-Browser\Passwords.dat (DPAPI-encrypted JSON)
'''   Blocked sites: %LOCALAPPDATA%\K-Browser\PasswordSaveExceptions.json
''' </summary>
Public Class PasswordManagerService

    Private ReadOnly _storageService As SecureStorageService
    Private ReadOnly _credentialFilePath As String
    Private ReadOnly _exceptionsFilePath As String
    Private _cachedCredentials As List(Of CredentialEntry) = Nothing

    Public Sub New()
        _storageService = New SecureStorageService()
        Dim appDataFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser")
        If Not Directory.Exists(appDataFolder) Then
            Directory.CreateDirectory(appDataFolder)
        End If
        _credentialFilePath = Path.Combine(appDataFolder, "Passwords.dat")
        _exceptionsFilePath = Path.Combine(appDataFolder, "PasswordSaveExceptions.json")
    End Sub

    ''' <summary>
    ''' Saves a new credential or updates an existing one for the same website+username.
    ''' The password is DPAPI-encrypted before storage.
    ''' </summary>
    Public Async Function SaveCredentialAsync(ByVal website As String, ByVal username As String, ByVal password As String) As Task
        If String.IsNullOrWhiteSpace(website) OrElse String.IsNullOrWhiteSpace(username) Then Return

        Dim credentials As List(Of CredentialEntry) = Await LoadCredentialsInternalAsync()
        Dim domain As String = ExtractDomain(website)

        ' Check if credential already exists for this domain + username
        Dim existing As CredentialEntry = credentials.Find(
            Function(c) c.Website.Equals(domain, StringComparison.OrdinalIgnoreCase) AndAlso
                        c.Username.Equals(username, StringComparison.OrdinalIgnoreCase))

        Dim encryptedPassword As String = _storageService.Encrypt(password)

        If existing IsNot Nothing Then
            ' Update existing credential
            existing.Password = encryptedPassword
            existing.LastUsed = DateTime.Now
        Else
            ' Add new credential
            credentials.Add(New CredentialEntry() With {
                .Website = domain,
                .Username = username,
                .Password = encryptedPassword,
                .CreatedDate = DateTime.Now,
                .LastUsed = DateTime.Now
            })
        End If

        Await SaveCredentialsInternalAsync(credentials)
        _cachedCredentials = credentials
    End Function

    ''' <summary>
    ''' Retrieves all saved credentials with passwords still encrypted.
    ''' Callers must use DecryptPassword() to reveal individual passwords on demand.
    ''' </summary>
    Public Async Function GetAllCredentialsAsync() As Task(Of List(Of CredentialEntry))
        If _cachedCredentials IsNot Nothing Then Return New List(Of CredentialEntry)(_cachedCredentials)
        Dim credentials = Await LoadCredentialsInternalAsync()
        _cachedCredentials = credentials
        Return New List(Of CredentialEntry)(credentials)
    End Function

    ''' <summary>
    ''' Returns credentials matching the given domain, with passwords decrypted.
    ''' Used for autofill — requires the actual password to fill login forms.
    ''' </summary>
    Public Async Function GetCredentialsForSiteAsync(ByVal url As String) As Task(Of List(Of CredentialEntry))
        Dim domain As String = ExtractDomain(url)
        Dim allCredentials = Await GetAllCredentialsAsync()

        Dim matches As New List(Of CredentialEntry)()
        For Each cred In allCredentials
            If cred.Website.Equals(domain, StringComparison.OrdinalIgnoreCase) Then
                Dim decrypted As New CredentialEntry() With {
                    .Website = cred.Website,
                    .Username = cred.Username,
                    .Password = DecryptPassword(cred.Password),
                    .CreatedDate = cred.CreatedDate,
                    .LastUsed = cred.LastUsed
                }
                matches.Add(decrypted)
            End If
        Next

        Return matches
    End Function

    ''' <summary>
    ''' Decrypts a single password field. Used when the user requests to view a password.
    ''' </summary>
    Public Function DecryptPassword(ByVal encryptedPassword As String) As String
        If String.IsNullOrEmpty(encryptedPassword) Then Return String.Empty
        Try
            Return _storageService.Decrypt(encryptedPassword)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PasswordManagerService: Failed to decrypt password: " & ex.Message)
            Return "[decryption failed]"
        End Try
    End Function

    ''' <summary>
    ''' Deletes a credential matching the given website and username.
    ''' </summary>
    Public Async Function DeleteCredentialAsync(ByVal website As String, ByVal username As String) As Task(Of Boolean)
        Dim credentials = Await LoadCredentialsInternalAsync()
        Dim removed As Integer = credentials.RemoveAll(
            Function(c) c.Website.Equals(website, StringComparison.OrdinalIgnoreCase) AndAlso
                        c.Username.Equals(username, StringComparison.OrdinalIgnoreCase))

        If removed > 0 Then
            Await SaveCredentialsInternalAsync(credentials)
            _cachedCredentials = credentials
            Return True
        End If
        Return False
    End Function

    ''' <summary>
    ''' Deletes all saved credentials.
    ''' </summary>
    Public Async Function DeleteAllCredentialsAsync() As Task
        Await SaveCredentialsInternalAsync(New List(Of CredentialEntry)())
        _cachedCredentials = New List(Of CredentialEntry)()
    End Function

    ''' <summary>
    ''' Updates the LastUsed timestamp for a credential (called after autofill).
    ''' </summary>
    Public Async Function UpdateLastUsedAsync(ByVal website As String, ByVal username As String) As Task
        Dim credentials = Await LoadCredentialsInternalAsync()
        Dim entry = credentials.Find(
            Function(c) c.Website.Equals(website, StringComparison.OrdinalIgnoreCase) AndAlso
                        c.Username.Equals(username, StringComparison.OrdinalIgnoreCase))

        If entry IsNot Nothing Then
            entry.LastUsed = DateTime.Now
            Await SaveCredentialsInternalAsync(credentials)
            _cachedCredentials = credentials
        End If
    End Function

    ''' <summary>
    ''' Searches credentials by website or username (case-insensitive partial match).
    ''' </summary>
    Public Function SearchCredentials(ByVal allCredentials As List(Of CredentialEntry), ByVal query As String) As List(Of CredentialEntry)
        If String.IsNullOrWhiteSpace(query) Then Return New List(Of CredentialEntry)(allCredentials)

        Dim lowerQuery As String = query.ToLowerInvariant()
        Return allCredentials.FindAll(
            Function(c) c.Website.ToLowerInvariant().Contains(lowerQuery) OrElse
                        c.Username.ToLowerInvariant().Contains(lowerQuery))
    End Function

    ' ─────────────────────────────────────────────────
    ' Blocked Sites Management
    ' ─────────────────────────────────────────────────

    ''' <summary>
    ''' Checks if a website is in the password save exceptions (blocked) list.
    ''' </summary>
    Public Function IsBlockedSite(ByVal url As String) As Boolean
        Dim domain As String = ExtractDomain(url)
        Dim blockedSites As List(Of String) = LoadBlockedSites()
        Return blockedSites.Exists(Function(s) s.Equals(domain, StringComparison.OrdinalIgnoreCase))
    End Function

    ''' <summary>
    ''' Adds a website domain to the blocked list ("Never save" for this site).
    ''' </summary>
    Public Sub AddBlockedSite(ByVal url As String)
        Dim domain As String = ExtractDomain(url)
        Dim blockedSites As List(Of String) = LoadBlockedSites()
        If Not blockedSites.Exists(Function(s) s.Equals(domain, StringComparison.OrdinalIgnoreCase)) Then
            blockedSites.Add(domain)
            SaveBlockedSites(blockedSites)
        End If
    End Sub

    ''' <summary>
    ''' Removes a website domain from the blocked list.
    ''' </summary>
    Public Sub RemoveBlockedSite(ByVal domain As String)
        Dim blockedSites As List(Of String) = LoadBlockedSites()
        blockedSites.RemoveAll(Function(s) s.Equals(domain, StringComparison.OrdinalIgnoreCase))
        SaveBlockedSites(blockedSites)
    End Sub

    ''' <summary>
    ''' Gets all blocked site domains.
    ''' </summary>
    Public Function GetBlockedSites() As List(Of String)
        Return LoadBlockedSites()
    End Function

    ' ─────────────────────────────────────────────────
    ' Export / Import
    ' ─────────────────────────────────────────────────

    ''' <summary>
    ''' Exports all credentials to an encrypted .kpass file.
    ''' </summary>
    Public Async Function ExportCredentialsAsync(ByVal filePath As String) As Task
        Dim credentials = Await LoadCredentialsInternalAsync()
        _storageService.ExportToFile(credentials, filePath)
    End Function

    ''' <summary>
    ''' Imports credentials from an encrypted .kpass file, merging with existing.
    ''' </summary>
    Public Async Function ImportCredentialsAsync(ByVal filePath As String) As Task(Of Integer)
        Dim imported As List(Of CredentialEntry) = _storageService.ImportFromFile(filePath)
        If imported Is Nothing OrElse imported.Count = 0 Then Return 0

        Dim existing = Await LoadCredentialsInternalAsync()
        Dim addedCount As Integer = 0

        For Each entry In imported
            Dim duplicate = existing.Find(
                Function(c) c.Website.Equals(entry.Website, StringComparison.OrdinalIgnoreCase) AndAlso
                            c.Username.Equals(entry.Username, StringComparison.OrdinalIgnoreCase))
            If duplicate Is Nothing Then
                existing.Add(entry)
                addedCount += 1
            End If
        Next

        Await SaveCredentialsInternalAsync(existing)
        _cachedCredentials = existing
        Return addedCount
    End Function

    ''' <summary>
    ''' Invalidates the in-memory cache, forcing a reload from disk on next access.
    ''' </summary>
    Public Sub InvalidateCache()
        _cachedCredentials = Nothing
    End Sub

    ' ─────────────────────────────────────────────────
    ' Internal File I/O
    ' ─────────────────────────────────────────────────

    ''' <summary>
    ''' Loads credentials from the DPAPI-encrypted Passwords.dat file.
    ''' </summary>
    Private Async Function LoadCredentialsInternalAsync() As Task(Of List(Of CredentialEntry))
        Try
            If Not File.Exists(_credentialFilePath) Then
                Return New List(Of CredentialEntry)()
            End If

            Dim encryptedBytes As Byte() = Await Task.Run(Function() File.ReadAllBytes(_credentialFilePath))
            If encryptedBytes.Length = 0 Then Return New List(Of CredentialEntry)()

            Dim jsonBytes As Byte() = _storageService.DecryptBytes(encryptedBytes)
            Dim json As String = Encoding.UTF8.GetString(jsonBytes)

            Dim options As New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
            Dim credentials = JsonSerializer.Deserialize(Of List(Of CredentialEntry))(json, options)
            Return If(credentials, New List(Of CredentialEntry)())
        Catch ex As Exception
            ' Corrupted file or decryption failure — log and return empty
            System.Diagnostics.Debug.WriteLine("PasswordManagerService: Error loading credentials: " & ex.Message)
            Return New List(Of CredentialEntry)()
        End Try
    End Function

    ''' <summary>
    ''' Saves credentials to the DPAPI-encrypted Passwords.dat file.
    ''' </summary>
    Private Async Function SaveCredentialsInternalAsync(ByVal credentials As List(Of CredentialEntry)) As Task
        Try
            Dim options As New JsonSerializerOptions With {.WriteIndented = False}
            Dim json As String = JsonSerializer.Serialize(credentials, options)
            Dim jsonBytes As Byte() = Encoding.UTF8.GetBytes(json)
            Dim encryptedBytes As Byte() = _storageService.EncryptBytes(jsonBytes)
            Await Task.Run(Sub() File.WriteAllBytes(_credentialFilePath, encryptedBytes))
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PasswordManagerService: Error saving credentials: " & ex.Message)
            Throw New IOException("Failed to save credentials securely.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Extracts the domain from a URL (e.g., "https://github.com/login" → "github.com").
    ''' </summary>
    Public Shared Function ExtractDomain(ByVal url As String) As String
        If String.IsNullOrWhiteSpace(url) Then Return String.Empty

        Try
            ' Ensure the URL has a scheme for Uri parsing
            Dim urlToParse As String = url.Trim()
            If Not urlToParse.Contains("://") Then
                urlToParse = "https://" & urlToParse
            End If

            Dim uri As New Uri(urlToParse)
            Return uri.Host.ToLowerInvariant()
        Catch
            ' If URL parsing fails, return the input cleaned up
            Return url.Trim().ToLowerInvariant().Replace("https://", "").Replace("http://", "").Split("/"c)(0)
        End Try
    End Function

    Private Function LoadBlockedSites() As List(Of String)
        Try
            If Not File.Exists(_exceptionsFilePath) Then Return New List(Of String)()

            Dim json As String = File.ReadAllText(_exceptionsFilePath)
            Dim options As New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
            Dim model = JsonSerializer.Deserialize(Of BlockedSitesModel)(json, options)
            Return If(model?.BlockedSites, New List(Of String)())
        Catch
            Return New List(Of String)()
        End Try
    End Function

    Private Sub SaveBlockedSites(ByVal sites As List(Of String))
        Try
            Dim model As New BlockedSitesModel() With {.BlockedSites = sites}
            Dim options As New JsonSerializerOptions With {.WriteIndented = True}
            Dim json As String = JsonSerializer.Serialize(model, options)
            File.WriteAllText(_exceptionsFilePath, json)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PasswordManagerService: Error saving blocked sites: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Internal model for PasswordSaveExceptions.json serialization.
    ''' </summary>
    Private Class BlockedSitesModel
        Public Property BlockedSites As List(Of String) = New List(Of String)()
    End Class

End Class
