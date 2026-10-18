''' <summary>
''' Hands an address to another browser's command line (Tools > Open page in Internet Explorer).
''' </summary>
Public Class ExternalBrowser

    ''' <summary>
    ''' Returns the address in a form that is safe as a program argument, or Nothing. Only absolute http(s)
    ''' addresses qualify, so the argument always starts with a scheme (never a "-flag"), and Uri escapes spaces
    ''' and quotes, so it cannot be split into several arguments.
    ''' </summary>
    Public Shared Function SafeUrlArgument(ByVal text As String) As String
        If String.IsNullOrWhiteSpace(text) Then Return Nothing

        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(text.Trim(), UriKind.Absolute, uri) Then Return Nothing
        If uri.Scheme <> Uri.UriSchemeHttp AndAlso uri.Scheme <> Uri.UriSchemeHttps Then Return Nothing
        Return uri.AbsoluteUri
    End Function

End Class
