''' <summary>
''' Domain model representing a single saved credential entry.
''' Pure data carrier with no framework or infrastructure dependencies.
''' </summary>
Public Class CredentialEntry

    ''' <summary>
    ''' The website domain this credential belongs to (e.g., "github.com").
    ''' </summary>
    Public Property Website As String = String.Empty

    ''' <summary>
    ''' The username or email address used for login.
    ''' </summary>
    Public Property Username As String = String.Empty

    ''' <summary>
    ''' The password, stored as a Base64-encoded DPAPI ciphertext when persisted.
    ''' In memory after decryption, this holds the plaintext password transiently.
    ''' </summary>
    Public Property Password As String = String.Empty

    ''' <summary>
    ''' When this credential was first saved.
    ''' </summary>
    Public Property CreatedDate As DateTime = DateTime.Now

    ''' <summary>
    ''' When this credential was last used for autofill or viewed.
    ''' </summary>
    Public Property LastUsed As DateTime = DateTime.Now

End Class
