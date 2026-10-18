''' <summary>
''' A finished, read-only set of ad-block rules. It is checked for every request a page makes, so it is
''' built once by <see cref="AdBlockRuleSetBuilder"/> (rules lower-cased up front) and never changed after,
''' which lets any thread read it without a lock. Reloading rules builds a new set and swaps it in.
''' </summary>
Public NotInheritable Class AdBlockRuleSet

    Public Shared ReadOnly Empty As New AdBlockRuleSet(New AdBlockTrie(), Array.Empty(Of String)(), Array.Empty(Of String)())

    Private ReadOnly _hosts As AdBlockTrie
    Private ReadOnly _paths As String()
    Private ReadOnly _allows As String()

    Friend Sub New(ByVal hosts As AdBlockTrie, ByVal paths As String(), ByVal allows As String())
        _hosts = hosts
        _paths = paths
        _allows = allows
    End Sub

    Public ReadOnly Property RuleCount As Integer
        Get
            Return _hosts.RuleCount + _paths.Length + _allows.Length
        End Get
    End Property

    ''' <summary>Whether the address's host (or a parent domain) is on a blocked-domain rule.</summary>
    Public Function ShouldBlockHost(ByVal url As String) As Boolean
        Dim host As String = HostOf(url)
        Return host IsNot Nothing AndAlso _hosts.IsHostBlocked(host)
    End Function

    ''' <summary>
    ''' Whether a request should be blocked: its host or path matches a block rule and no allow rule matches.
    ''' Allow rules are only looked at for the few requests that would be blocked.
    ''' </summary>
    Public Function ShouldBlock(ByVal url As String) As Boolean
        Dim host As String = HostOf(url)
        If host Is Nothing Then Return False

        Dim lowerUrl As String = url.ToLowerInvariant()
        If Not (_hosts.IsHostBlocked(host) OrElse ContainsAny(lowerUrl, _paths)) Then Return False
        Return Not ContainsAny(lowerUrl, _allows)
    End Function

    Private Shared Function ContainsAny(ByVal lowerUrl As String, ByVal parts As String()) As Boolean
        For Each part As String In parts
            If lowerUrl.Contains(part, StringComparison.Ordinal) Then Return True
        Next
        Return False
    End Function

    Private Shared Function HostOf(ByVal url As String) As String
        If String.IsNullOrWhiteSpace(url) Then Return Nothing
        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) OrElse String.IsNullOrEmpty(uri.Host) Then Return Nothing
        Return uri.Host
    End Function

End Class

''' <summary>
''' Parses Adblock Plus style filter lists into an <see cref="AdBlockRuleSet"/>. Only network rules that
''' are safe to apply everywhere are kept: blocked domains (||host^), ad-like path fragments and allow (@@)
''' rules. Element hiding, site-specific ($domain=) and stylesheet/font rules are skipped.
''' </summary>
Public Class AdBlockRuleSetBuilder

    ' Reserved generic keywords that MUST NEVER be treated as ad blocking rules
    Private Shared ReadOnly ReservedGenericKeywords As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        "http", "https", "file", "about", "html", "htm", "json", "xml", "text", "css", "javascript",
        "com", "org", "net", "edu", "gov", "mil", "int", "biz", "info", "name", "pro", "co", "io", "me", "tv", "cc", "us", "uk", "ca", "de", "fr", "gr", "eu", "au", "jp", "cn", "in", "ru", "br", "app", "dev", "site", "online", "store", "tech", "xyz", "website", "link", "click", "top",
        "co.uk", "com.ar", "co.th", "net.au", "org.uk", "com.br", "com.au", "co.jp", "co.kr", "co.nz", "com.mx", "com.tw",
        "www", "www1", "www2", "api", "cdn", "static", "assets", "media", "images", "img", "js", "style", "styles", "stylesheet", "css",
        "index", "main", "home", "default", "page", "document", "script", "scripts", "font", "fonts", "vendor", "app", "k-browser", "k-browser.com",
        "google", "gstatic", "googleapis", "github", "microsoft", "cloudflare", "amazonaws", "search", "intl", "about"
    }

    Private Shared ReadOnly GenericWebPaths As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase) From {
        "css", "style", "styles", "stylesheet", "assets", "static", "images", "image", "img", "fonts", "font",
        "js", "javascript", "script", "scripts", "media", "public", "dist", "build", "vendor", "node_modules",
        "includes", "content", "main", "common", "bundle", "theme", "themes", "core", "lib", "libs", "component",
        "components", "template", "templates", "min", "app", "site", "page", "pages", "about", "intl", "search",
        "en", "en_us", "code", "json", "xml", "html", "htm", "svg", "png", "jpg", "jpeg", "gif", "webp", "woff", "woff2"
    }

    Private ReadOnly _hosts As New AdBlockTrie()
    Private ReadOnly _paths As New HashSet(Of String)(StringComparer.Ordinal)
    Private ReadOnly _allows As New HashSet(Of String)(StringComparer.Ordinal)

    Public ReadOnly Property RuleCount As Integer
        Get
            Return _hosts.RuleCount + _paths.Count + _allows.Count
        End Get
    End Property

    Public Sub AddDomain(ByVal domain As String)
        _hosts.AddDomain(domain)
    End Sub

    ''' <summary>Adds the rules found in the given filter-list lines. Returns how many were kept.</summary>
    Public Function AddLines(ByVal lines As IEnumerable(Of String)) As Integer
        Dim before As Integer = RuleCount
        For Each rawLine As String In lines
            AddLine(rawLine)
        Next
        Return RuleCount - before
    End Function

    ''' <summary>Finishes the set. The builder must not be used afterwards.</summary>
    Public Function Build() As AdBlockRuleSet
        Return New AdBlockRuleSet(_hosts, _paths.ToArray(), _allows.ToArray())
    End Function

    Private Sub AddLine(ByVal rawLine As String)
        If String.IsNullOrWhiteSpace(rawLine) Then Return
        Dim line As String = rawLine.Trim()

        ' 1. Skip comments, metadata, and CSS element hiding rules
        If line.StartsWith("!") OrElse line.StartsWith("[") OrElse line.Contains("##") OrElse line.Contains("#@#") OrElse line.Contains("#?#") Then
            Return
        End If

        ' 2. Handle rule options starting with $ (e.g. ||example.com/ad^$third-party)
        Dim dollarIdx As Integer = line.IndexOf("$"c)
        If dollarIdx > 0 Then
            Dim options As String = line.Substring(dollarIdx + 1).ToLowerInvariant()
            line = line.Substring(0, dollarIdx).Trim()

            ' Skip domain-restricted rules or stylesheet/font options from becoming global path blocks
            If options.Contains("domain=") OrElse options.Contains("stylesheet") OrElse options.Contains("font") OrElse options.Contains("css") Then
                Return
            End If
        End If

        If String.IsNullOrWhiteSpace(line) Then Return

        ' 3. Whitelist / Allow rules starting with @@
        If line.StartsWith("@@") Then
            Dim allowPattern As String = line.Substring(2).Trim()
            If allowPattern.StartsWith("||") Then allowPattern = allowPattern.Substring(2)
            allowPattern = allowPattern.Replace("^", "").Replace("*", "").Trim()
            If allowPattern.Length >= 3 AndAlso Not ReservedGenericKeywords.Contains(allowPattern) Then
                _allows.Add(allowPattern.ToLowerInvariant())
            End If
            Return
        End If

        ' 4. Domain anchor rules starting with || (e.g. ||doubleclick.net^)
        If line.StartsWith("||") Then
            Dim domain As String = line.Substring(2)
            Dim caretIdx As Integer = domain.IndexOf("^"c)
            If caretIdx >= 0 Then domain = domain.Substring(0, caretIdx)
            Dim slashIdx As Integer = domain.IndexOf("/"c)
            If slashIdx >= 0 Then domain = domain.Substring(0, slashIdx)
            Dim questionIdx As Integer = domain.IndexOf("?"c)
            If questionIdx >= 0 Then domain = domain.Substring(0, questionIdx)
            Dim colonIdx As Integer = domain.IndexOf(":"c)
            If colonIdx >= 0 Then domain = domain.Substring(0, colonIdx)
            domain = domain.Replace("*", "").Trim().ToLowerInvariant()

            If domain.Length >= 4 AndAlso domain.Contains(".") AndAlso Not ReservedGenericKeywords.Contains(domain) Then
                Dim parts As String() = domain.Split("."c)
                If parts.Length >= 2 AndAlso parts(0).Length >= 2 Then
                    _hosts.AddDomain(domain)
                End If
            End If
            Return
        End If

        ' 5. Path / URL Substring rules (e.g. /ad_banner/, /adserver/, &ad_box=)
        Dim cleanPath As String = line.Replace("^", "").Replace("*", "").Trim()
        Dim trimmedPath As String = cleanPath.Trim("/"c, "\"c, "."c, " "c, "?"c, "&"c, "="c)

        If cleanPath.Length >= 5 AndAlso Not ReservedGenericKeywords.Contains(cleanPath) AndAlso Not ReservedGenericKeywords.Contains(trimmedPath) AndAlso Not GenericWebPaths.Contains(trimmedPath) Then
            If IsAdKeywordPattern(cleanPath) Then
                _paths.Add(cleanPath.ToLowerInvariant())
            End If
        End If
    End Sub

    Private Shared Function IsAdKeywordPattern(ByVal path As String) As Boolean
        Dim p As String = path.ToLowerInvariant()
        Return p.Contains("ad_") OrElse p.Contains("_ad") OrElse p.Contains("/ad/") OrElse p.Contains("/ads/") OrElse
               p.Contains("adserver") OrElse p.Contains("adservice") OrElse p.Contains("adsystem") OrElse
               p.Contains("doubleclick") OrElse p.Contains("pagead") OrElse p.Contains("popunder") OrElse
               p.Contains("adbanner") OrElse p.Contains("telemetry") OrElse p.Contains("tracker") OrElse
               p.Contains("analytics") OrElse p.Contains("sponsor") OrElse p.Contains("syndication") OrElse
               p.Contains("taboola") OrElse p.Contains("outbrain") OrElse p.Contains("adform") OrElse
               p.Contains("scorecard") OrElse p.Contains("adnxs") OrElse p.Contains("adking") OrElse
               p.Contains("adbox") OrElse p.Contains("adcontainer") OrElse p.Contains("adframe") OrElse
               p.Contains("adslot") OrElse p.Contains("adwrapper") OrElse p.Contains("advert") OrElse
               p.Contains("affiliate")
    End Function

End Class
