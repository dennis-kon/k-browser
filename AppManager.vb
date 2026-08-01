Public Class AppManager

    Public Shared Function IsValidUrl(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) Then Return False
        Return System.Text.RegularExpressions.Regex.IsMatch(url.Trim(),
            "^(https?|ftp|file)://[^\s/$.?#].[^\s]*$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    End Function

    Public Shared Function IsValidIP(ByVal ipAddress As String) As Boolean
        If String.IsNullOrWhiteSpace(ipAddress) Then Return False
        Return System.Text.RegularExpressions.Regex.IsMatch(ipAddress.Trim(),
            "^(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}$")
    End Function

    Public Shared Function IsValidEmail(ByVal email As String) As Boolean
        If String.IsNullOrWhiteSpace(email) Then Return False
        Return System.Text.RegularExpressions.Regex.IsMatch(email.Trim(),
            "^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")
    End Function

    Public Shared Function FixURL(ByVal sURL As String) As String
        If String.IsNullOrWhiteSpace(sURL) Then Return sURL
        Dim trimmed As String = sURL.Trim()
        Dim lowered As String = trimmed.ToLowerInvariant()
        If lowered.StartsWith("http://") OrElse lowered.StartsWith("https://") OrElse lowered.StartsWith("file://") OrElse lowered.StartsWith("about:") OrElse lowered.StartsWith("ftp://") Then
            Return trimmed
        End If
        Return "http://" & trimmed
    End Function

    Public Shared Function ResolveUrlOrSearch(ByVal input As String, Optional ByVal engine As String = "Google") As String
        If String.IsNullOrWhiteSpace(input) Then Return "about:blank"
        Dim trimmed As String = input.Trim()
        Dim lowered As String = trimmed.ToLowerInvariant()

        If lowered.StartsWith("http://") OrElse lowered.StartsWith("https://") OrElse lowered.StartsWith("file://") OrElse lowered.StartsWith("about:") OrElse lowered.StartsWith("ftp://") Then
            Return trimmed
        End If

        If trimmed.Contains(" ") OrElse (Not trimmed.Contains(".") AndAlso Not trimmed.Contains(":")) Then
            Dim query As String = Uri.EscapeDataString(trimmed)
            Select Case If(engine, "Google").ToLowerInvariant()
                Case "bing"
                    Return "https://www.bing.com/search?q=" & query
                Case "duckduckgo"
                    Return "https://duckduckgo.com/?q=" & query
                Case "yahoo"
                    Return "https://search.yahoo.com/search?p=" & query
                Case Else
                    Return "https://www.google.com/search?q=" & query
            End Select
        End If

        Return FixURL(trimmed)
    End Function

End Class
