Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Public Class AdBlockEngine

    Private Shared ReadOnly LockObj As New Object()
    Private Shared ReadOnly AllowRules As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly DomainAnchors As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly AdPathSubstrings As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)

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

    Public Shared Property TotalBlockedCount As Long = 0

    Private Shared Function GetRulesDirectory() As String
        Dim dirPath As String = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "K-Browser", "AdBlockRules")
        If Not System.IO.Directory.Exists(dirPath) Then
            System.IO.Directory.CreateDirectory(dirPath)
        End If
        Return dirPath
    End Function

    Public Shared Function LoadAllRules() As Integer
        SyncLock LockObj
            AllowRules.Clear()
            DomainAnchors.Clear()
            AdPathSubstrings.Clear()
        End SyncLock

        Dim totalRulesCount As Integer = 0
        Dim dirPath As String = GetRulesDirectory()
        Dim ruleFiles As String() = System.IO.Directory.GetFiles(dirPath, "*.txt")

        For Each filePath As String In ruleFiles
            Try
                Dim lines As String() = System.IO.File.ReadAllLines(filePath)
                totalRulesCount += ParseRulesLines(lines)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error reading rule file " & filePath & ": " & ex.Message)
            End Try
        Next

        ' Add default fallback ad domains if no rules are downloaded yet
        If totalRulesCount = 0 Then
            SyncLock LockObj
                DomainAnchors.Add("doubleclick.net")
                DomainAnchors.Add("adservice.google.com")
                DomainAnchors.Add("adnxs.com")
                DomainAnchors.Add("googlesyndication.com")
                DomainAnchors.Add("taboola.com")
                DomainAnchors.Add("outbrain.com")
                DomainAnchors.Add("adform.net")
                DomainAnchors.Add("scorecardresearch.com")
                DomainAnchors.Add("amazon-adsystem.com")
            End SyncLock
            totalRulesCount = 9
        End If

        Return totalRulesCount
    End Function

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

    Public Shared Function ParseRulesLines(ByVal lines As String()) As Integer
        Dim count As Integer = 0

        For Each rawLine As String In lines
            If String.IsNullOrWhiteSpace(rawLine) Then Continue For
            Dim line As String = rawLine.Trim()

            ' 1. Skip comments, metadata, and CSS element hiding rules
            If line.StartsWith("!") OrElse line.StartsWith("[") OrElse line.Contains("##") OrElse line.Contains("#@#") OrElse line.Contains("#?#") Then
                Continue For
            End If

            ' 2. Handle rule options starting with $ (e.g. ||example.com/ad^$third-party)
            Dim dollarIdx As Integer = line.IndexOf("$"c)
            If dollarIdx > 0 Then
                Dim options As String = line.Substring(dollarIdx + 1).ToLowerInvariant()
                line = line.Substring(0, dollarIdx).Trim()

                ' Skip domain-restricted rules or stylesheet/font options from becoming global path blocks
                If options.Contains("domain=") OrElse options.Contains("stylesheet") OrElse options.Contains("font") OrElse options.Contains("css") Then
                    Continue For
                End If
            End If

            If String.IsNullOrWhiteSpace(line) Then Continue For

            SyncLock LockObj
                ' 3. Whitelist / Allow rules starting with @@
                If line.StartsWith("@@") Then
                    Dim allowPattern As String = line.Substring(2).Trim()
                    If allowPattern.StartsWith("||") Then allowPattern = allowPattern.Substring(2)
                    allowPattern = allowPattern.Replace("^", "").Replace("*", "").Trim()
                    If allowPattern.Length >= 3 AndAlso Not ReservedGenericKeywords.Contains(allowPattern) Then
                        AllowRules.Add(allowPattern)
                        count += 1
                    End If
                    Continue For
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
                            DomainAnchors.Add(domain)
                            count += 1
                        End If
                    End If
                    Continue For
                End If

                ' 5. Path / URL Substring rules (e.g. /ad_banner/, /adserver/, &ad_box=)
                Dim cleanPath As String = line.Replace("^", "").Replace("*", "").Trim()
                Dim trimmedPath As String = cleanPath.Trim("/"c, "\"c, "."c, " "c, "?"c, "&"c, "="c)

                If cleanPath.Length >= 5 AndAlso Not ReservedGenericKeywords.Contains(cleanPath) AndAlso Not ReservedGenericKeywords.Contains(trimmedPath) AndAlso Not GenericWebPaths.Contains(trimmedPath) Then
                    If IsAdKeywordPattern(cleanPath) Then
                        AdPathSubstrings.Add(cleanPath)
                        count += 1
                    End If
                End If
            End SyncLock
        Next

        Return count
    End Function

    Public Shared Function ShouldBlockDomainOnly(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) OrElse url = "about:blank" Then Return False
        SyncLock LockObj
            Try
                Dim uri As New Uri(url)
                Dim host As String = uri.Host.ToLowerInvariant()
                For Each anchor As String In DomainAnchors
                    If host = anchor OrElse host.EndsWith("." & anchor) Then
                        Return True
                    End If
                Next
            Catch
            End Try
        End SyncLock
        Return False
    End Function

    Public Shared Function ShouldBlockDocument(ByVal url As String) As Boolean
        Return ShouldBlockDomainOnly(url)
    End Function

    Public Shared Function ShouldBlock(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) OrElse url = "about:blank" Then Return False
        Dim lowerUrl As String = url.ToLowerInvariant()

        SyncLock LockObj
            ' 1. Whitelist / Allow rules check
            For Each allow As String In AllowRules
                If lowerUrl.Contains(allow.ToLowerInvariant()) Then
                    Return False
                End If
            Next

            ' 2. Domain Anchors (Host matching)
            Try
                Dim uri As New Uri(url)
                Dim host As String = uri.Host.ToLowerInvariant()
                For Each anchor As String In DomainAnchors
                    If host = anchor OrElse host.EndsWith("." & anchor) Then
                        Return True
                    End If
                Next
            Catch
            End Try

            ' 3. Ad Path Substrings (for sub-resources like images, scripts, frames)
            For Each substring As String In AdPathSubstrings
                If lowerUrl.Contains(substring.ToLowerInvariant()) Then
                    Return True
                End If
            Next
        End SyncLock

        Return False
    End Function

    Public Shared Async Function UpdateFilterListAsync(ByVal listName As String, ByVal url As String) As Task(Of Boolean)
        Try
            Using client As New System.Net.WebClient()
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) K-Browser/1.0 AdBlocker")
                Dim rulesText As String = Await client.DownloadStringTaskAsync(url)

                If Not String.IsNullOrWhiteSpace(rulesText) Then
                    Dim fileName As String = Regex.Replace(listName, "[^a-zA-Z0-9_]", "_") & ".txt"
                    Dim filePath As String = System.IO.Path.Combine(GetRulesDirectory(), fileName)
                    System.IO.File.WriteAllText(filePath, rulesText)
                    Return True
                End If
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Failed to update list " & listName & ": " & ex.Message)
        End Try
        Return False
    End Function

End Class

