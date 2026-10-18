Imports System.Text.RegularExpressions

''' <summary>
''' Matching for the user's blocked-site, phishing-site and per-site JavaScript lists, and the rules applied
''' to sub-frames.
'''
''' Entries used to be matched with url.Contains(entry), so "example.com" also matched "notexample.com" and any
''' URL that merely mentioned it in a query string. An entry is now understood as:
'''   host        "example.com"        the host and its subdomains (any port)
'''   host:port   "localhost:3000"     that port only
'''   path        "example.com/login"  that path (and anything below it) on that host
'''   keyword     "facebook"           (no dot) a word inside the host name
''' Any scheme in the entry ("https://") is ignored.
''' </summary>
Public Class SiteFilter

    Private Shared ReadOnly SchemePrefix As New Regex("^[a-z][a-z0-9+.\-]*://", RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)
    Private Shared ReadOnly PortSuffix As New Regex(":[0-9]{1,5}$", RegexOptions.CultureInvariant)

    ''' <summary>True when <paramref name="url"/> is covered by the list entry <paramref name="entry"/>.</summary>
    Public Shared Function UrlMatchesEntry(ByVal url As String, ByVal entry As String) As Boolean
        If String.IsNullOrWhiteSpace(entry) OrElse String.IsNullOrEmpty(url) Then Return False

        Dim wanted As String = SchemePrefix.Replace(entry.Trim().ToLowerInvariant(), String.Empty).TrimEnd("/"c)
        If wanted.Length = 0 Then Return False

        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) OrElse uri.Host.Length = 0 Then
            ' Cannot tell what the host is: keep the old, broad behaviour so a block is never skipped.
            Return url.ToLowerInvariant().Contains(wanted)
        End If

        Dim split As Integer = wanted.IndexOfAny(New Char() {"/"c, "?"c})
        Dim hostPart As String = If(split < 0, wanted, wanted.Substring(0, split))
        Dim pathPart As String = If(split < 0, String.Empty, wanted.Substring(split))

        Dim host As String = uri.Host.ToLowerInvariant()
        Dim authority As String = If(uri.IsDefaultPort, host, host & ":" & uri.Port.ToString(System.Globalization.CultureInfo.InvariantCulture))

        Dim hostMatches As Boolean
        If PortSuffix.IsMatch(hostPart) Then
            hostMatches = authority = hostPart OrElse authority.EndsWith("." & hostPart, StringComparison.Ordinal)
        ElseIf hostPart.Contains(".") Then
            hostMatches = host = hostPart OrElse host.EndsWith("." & hostPart, StringComparison.Ordinal)
        Else
            hostMatches = host.Contains(hostPart)
        End If
        If Not hostMatches Then Return False

        Return pathPart.Length = 0 OrElse uri.PathAndQuery.ToLowerInvariant().StartsWith(pathPart, StringComparison.Ordinal)
    End Function

    ''' <summary>True when the URL is covered by at least one entry. A missing list matches nothing.</summary>
    Public Shared Function MatchesAny(ByVal url As String, ByVal entries As IEnumerable(Of String)) As Boolean
        If entries Is Nothing Then Return False
        For Each entry As String In entries
            If UrlMatchesEntry(url, entry) Then Return True
        Next
        Return False
    End Function

    ''' <summary>
    ''' Whether a navigation inside a sub-frame must be cancelled. The top-level checks only see the main
    ''' frame, so a blocked or phishing page could otherwise be embedded in an iframe. Only http(s) frames are
    ''' judged: about:blank, blob: and data: frames are everywhere and never a destination on their own.
    ''' </summary>
    Public Shared Function IsFrameBlocked(ByVal url As String, ByVal httpsOnly As Boolean,
                                          ByVal blockedSites As IEnumerable(Of String),
                                          ByVal phishingSites As IEnumerable(Of String)) As Boolean
        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) Then Return False
        If uri.Scheme <> Uri.UriSchemeHttp AndAlso uri.Scheme <> Uri.UriSchemeHttps Then Return False

        If httpsOnly AndAlso uri.Scheme = Uri.UriSchemeHttp Then Return True
        Return MatchesAny(url, blockedSites) OrElse MatchesAny(url, phishingSites)
    End Function

End Class
