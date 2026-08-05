Imports System.IO
Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' Infrastructure service providing DPAPI-based encryption and decryption
''' via ProtectedData.Protect/Unprotect with DataProtectionScope.CurrentUser.
''' 
''' This ensures credentials are encrypted with the current Windows user's key
''' and cannot be decrypted by other users or on other machines.
''' </summary>
Public Class SecureStorageService

    ' Static entropy adds a layer of application-specific salt to the DPAPI encryption.
    ' This prevents other applications running under the same user from trivially decrypting
    ' K Browser's stored credentials (they'd need to know this entropy value).
    Private Shared ReadOnly Entropy As Byte() = Encoding.UTF8.GetBytes("KBrowser.PasswordManager.v1.Entropy")

    ''' <summary>
    ''' Encrypts a plaintext string using DPAPI with CurrentUser scope.
    ''' Returns a Base64-encoded ciphertext string.
    ''' </summary>
    ''' <param name="plainText">The plaintext string to encrypt.</param>
    ''' <returns>Base64-encoded encrypted string, or empty string if input is empty.</returns>
    Public Function Encrypt(ByVal plainText As String) As String
        If String.IsNullOrEmpty(plainText) Then Return String.Empty

        Try
            Dim plainBytes As Byte() = Encoding.UTF8.GetBytes(plainText)
            Dim encryptedBytes As Byte() = ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser)
            Return Convert.ToBase64String(encryptedBytes)
        Catch ex As CryptographicException
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Encryption failed: " & ex.Message)
            Throw New InvalidOperationException("Failed to encrypt data. Windows Data Protection API is unavailable.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Decrypts a Base64-encoded DPAPI ciphertext string back to plaintext.
    ''' </summary>
    ''' <param name="cipherText">Base64-encoded encrypted string from Encrypt().</param>
    ''' <returns>The decrypted plaintext string, or empty string if input is empty.</returns>
    Public Function Decrypt(ByVal cipherText As String) As String
        If String.IsNullOrEmpty(cipherText) Then Return String.Empty

        Try
            Dim encryptedBytes As Byte() = Convert.FromBase64String(cipherText)
            Dim plainBytes As Byte() = ProtectedData.Unprotect(encryptedBytes, Entropy, DataProtectionScope.CurrentUser)
            Return Encoding.UTF8.GetString(plainBytes)
        Catch ex As CryptographicException
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Decryption failed: " & ex.Message)
            Throw New InvalidOperationException("Failed to decrypt data. The credential may be corrupted or was encrypted by a different user.", ex)
        Catch ex As FormatException
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Invalid Base64 ciphertext: " & ex.Message)
            Throw New InvalidOperationException("Invalid encrypted data format.", ex)
        End Try
    End Function

    ''' <summary>
    ''' Encrypts an entire byte array (e.g., serialized credential file) using DPAPI.
    ''' Used for whole-file encryption of the Passwords.dat storage file.
    ''' </summary>
    Public Function EncryptBytes(ByVal plainBytes As Byte()) As Byte()
        If plainBytes Is Nothing OrElse plainBytes.Length = 0 Then Return Array.Empty(Of Byte)()

        Try
            Return ProtectedData.Protect(plainBytes, Entropy, DataProtectionScope.CurrentUser)
        Catch ex As CryptographicException
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Byte encryption failed: " & ex.Message)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Decrypts an entire byte array that was encrypted with EncryptBytes().
    ''' </summary>
    Public Function DecryptBytes(ByVal encryptedBytes As Byte()) As Byte()
        If encryptedBytes Is Nothing OrElse encryptedBytes.Length = 0 Then Return Array.Empty(Of Byte)()

        Try
            Return ProtectedData.Unprotect(encryptedBytes, Entropy, DataProtectionScope.CurrentUser)
        Catch ex As CryptographicException
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Byte decryption failed: " & ex.Message)
            Throw
        End Try
    End Function

    ''' <summary>
    ''' Exports credentials to an encrypted .kpass file.
    ''' The entire JSON payload is DPAPI-encrypted before writing to disk.
    ''' </summary>
    ''' <param name="credentials">List of credential entries (with encrypted password fields).</param>
    ''' <param name="filePath">Target file path for the .kpass export.</param>
    Public Sub ExportToFile(ByVal credentials As List(Of CredentialEntry), ByVal filePath As String)
        If credentials Is Nothing OrElse credentials.Count = 0 Then
            Throw New ArgumentException("No credentials to export.")
        End If

        Try
            Dim options As New System.Text.Json.JsonSerializerOptions With {.WriteIndented = False}
            Dim json As String = System.Text.Json.JsonSerializer.Serialize(credentials, options)
            Dim jsonBytes As Byte() = Encoding.UTF8.GetBytes(json)
            Dim encryptedBytes As Byte() = EncryptBytes(jsonBytes)
            File.WriteAllBytes(filePath, encryptedBytes)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Export failed: " & ex.Message)
            Throw New IOException("Failed to export credentials: " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Imports credentials from an encrypted .kpass file.
    ''' </summary>
    ''' <param name="filePath">Path to the .kpass file to import.</param>
    ''' <returns>List of credential entries loaded from the file.</returns>
    Public Function ImportFromFile(ByVal filePath As String) As List(Of CredentialEntry)
        If Not File.Exists(filePath) Then
            Throw New FileNotFoundException("Import file not found.", filePath)
        End If

        Try
            Dim encryptedBytes As Byte() = File.ReadAllBytes(filePath)
            Dim jsonBytes As Byte() = DecryptBytes(encryptedBytes)
            Dim json As String = Encoding.UTF8.GetString(jsonBytes)
            Dim options As New System.Text.Json.JsonSerializerOptions With {.PropertyNameCaseInsensitive = True}
            Dim entries = System.Text.Json.JsonSerializer.Deserialize(Of List(Of CredentialEntry))(json, options)
            Return If(entries, New List(Of CredentialEntry)())
        Catch ex As CryptographicException
            Throw New InvalidOperationException("Cannot decrypt the import file. It may have been created by a different user or on a different machine.", ex)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("SecureStorageService: Import failed: " & ex.Message)
            Throw New IOException("Failed to import credentials: " & ex.Message, ex)
        End Try
    End Function

End Class
