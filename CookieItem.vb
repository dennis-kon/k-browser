Imports System

''' <summary>
''' Represents a strongly-typed domain model for a browser cookie retrieved from WebView2.
''' </summary>
Public Class CookieItem
    Public Property Website As String = String.Empty
    Public Property Name As String = String.Empty
    Public Property Domain As String = String.Empty
    Public Property Path As String = String.Empty
    Public Property Expires As Nullable(Of DateTime) = Nothing
    Public Property IsSecure As Boolean = False
    Public Property IsHttpOnly As Boolean = False
    Public Property SameSite As String = "Unspecified"

    ''' <summary>
    ''' Formats the expiration date as a user-friendly display string.
    ''' </summary>
    Public ReadOnly Property DisplayExpires As String
        Get
            If Expires.HasValue Then
                Return Expires.Value.ToString("yyyy-MM-dd HH:mm:ss")
            End If
            Return "Session"
        End Get
    End Property

    ''' <summary>
    ''' Determines whether this cookie record matches the provided search query string.
    ''' </summary>
    Public Function MatchesSearch(ByVal searchQuery As String) As Boolean
        If String.IsNullOrWhiteSpace(searchQuery) Then Return True
        Dim query As String = searchQuery.Trim().ToLowerInvariant()

        Return (Not String.IsNullOrEmpty(Website) AndAlso Website.ToLowerInvariant().Contains(query)) OrElse
               (Not String.IsNullOrEmpty(Name) AndAlso Name.ToLowerInvariant().Contains(query)) OrElse
               (Not String.IsNullOrEmpty(Domain) AndAlso Domain.ToLowerInvariant().Contains(query)) OrElse
               (Not String.IsNullOrEmpty(Path) AndAlso Path.ToLowerInvariant().Contains(query))
    End Function
End Class
