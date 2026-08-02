Imports System.IO
Imports System.Net
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks

Public Class BlockedTrackerItem
    Public Property Url As String
    Public Property Timestamp As DateTime

    Public Sub New(ByVal itemUrl As String, ByVal itemTime As DateTime)
        Me.Url = itemUrl
        Me.Timestamp = itemTime
    End Sub
End Class

Public Class AdBlockEngine

    Private Shared ReadOnly LockObj As New Object()
    Private Shared ReadOnly AllowRules As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly DomainAnchors As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private Shared ReadOnly HostTrie As New AdBlockTrie()
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

    Private Shared ReadOnly TabBlockedItems As New Dictionary(Of Microsoft.Web.WebView2.WinForms.WebView2, List(Of BlockedTrackerItem))()

    Public Shared Sub RecordBlockedItem(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2, ByVal url As String)
        SyncLock LockObj
            TotalBlockedCount += 1
            If brws IsNot Nothing Then
                If Not TabBlockedItems.ContainsKey(brws) Then
                    TabBlockedItems(brws) = New List(Of BlockedTrackerItem)()
                End If
                TabBlockedItems(brws).Add(New BlockedTrackerItem(url, DateTime.Now))
            End If
        End SyncLock
    End Sub

    Public Shared Function GetPageBlockedItems(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2) As List(Of BlockedTrackerItem)
        SyncLock LockObj
            If brws IsNot Nothing AndAlso TabBlockedItems.ContainsKey(brws) Then
                Return New List(Of BlockedTrackerItem)(TabBlockedItems(brws))
            End If
            Return New List(Of BlockedTrackerItem)()
        End SyncLock
    End Function

    Public Shared Function GetPageBlockedCount(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2) As Integer
        SyncLock LockObj
            If brws IsNot Nothing AndAlso TabBlockedItems.ContainsKey(brws) Then
                Return TabBlockedItems(brws).Count
            End If
            Return 0
        End SyncLock
    End Function

    Public Shared Sub ClearTabBlockedItems(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2)
        SyncLock LockObj
            If brws IsNot Nothing AndAlso TabBlockedItems.ContainsKey(brws) Then
                TabBlockedItems(brws).Clear()
            End If
        End SyncLock
    End Sub

    Public Shared Sub RemoveTab(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2)
        SyncLock LockObj
            If brws IsNot Nothing AndAlso TabBlockedItems.ContainsKey(brws) Then
                TabBlockedItems.Remove(brws)
            End If
        End SyncLock
    End Sub

    Public Shared Function GetDomain(ByVal url As String) As String
        If String.IsNullOrWhiteSpace(url) OrElse url = "about:blank" Then Return ""
        Try
            Dim uri As New Uri(url)
            Return uri.Host.ToLowerInvariant()
        Catch
            Return url.ToLowerInvariant()
        End Try
    End Function

    Public Shared Function IsSiteDisabled(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) OrElse url = "about:blank" Then Return False
        If My.Settings.AdBlockDisabledSites Is Nothing Then Return False

        Dim host As String = GetDomain(url)
        If String.IsNullOrEmpty(host) Then Return False

        SyncLock LockObj
            For Each site As String In My.Settings.AdBlockDisabledSites
                If Not String.IsNullOrWhiteSpace(site) Then
                    Dim s As String = site.Trim().ToLowerInvariant()
                    If host.Equals(s, StringComparison.OrdinalIgnoreCase) OrElse host.EndsWith("." & s, StringComparison.OrdinalIgnoreCase) Then
                        Return True
                    End If
                End If
            Next
        End SyncLock
        Return False
    End Function

    Public Shared Sub SetSiteDisabled(ByVal url As String, ByVal disabled As Boolean)
        Dim host As String = GetDomain(url)
        If String.IsNullOrEmpty(host) Then Return

        If My.Settings.AdBlockDisabledSites Is Nothing Then
            My.Settings.AdBlockDisabledSites = New System.Collections.Specialized.StringCollection()
        End If

        SyncLock LockObj
            Dim existingEntry As String = Nothing
            For Each site As String In My.Settings.AdBlockDisabledSites
                If Not String.IsNullOrWhiteSpace(site) AndAlso (host.Equals(site.Trim(), StringComparison.OrdinalIgnoreCase) OrElse site.Trim().Equals(host, StringComparison.OrdinalIgnoreCase)) Then
                    existingEntry = site
                    Exit For
                End If
            Next

            If disabled Then
                If existingEntry Is Nothing Then
                    My.Settings.AdBlockDisabledSites.Add(host)
                End If
            Else
                If existingEntry IsNot Nothing Then
                    My.Settings.AdBlockDisabledSites.Remove(existingEntry)
                End If
            End If
            My.Settings.Save()
        End SyncLock
    End Sub

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
            HostTrie.Clear()
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
                Dim defaults = New String() {"doubleclick.net", "adservice.google.com", "adnxs.com", "googlesyndication.com", "taboola.com", "outbrain.com", "adform.net", "scorecardresearch.com", "amazon-adsystem.com"}
                For Each d In defaults
                    DomainAnchors.Add(d)
                    HostTrie.AddDomain(d)
                Next
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
                            HostTrie.AddDomain(domain)
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
                If HostTrie.IsHostBlocked(host) Then
                    Return True
                End If
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

            ' 2. Domain Anchors (O(K) Host Trie matching)
            Try
                Dim uri As New Uri(url)
                Dim host As String = uri.Host.ToLowerInvariant()
                If HostTrie.IsHostBlocked(host) Then
                    Return True
                End If
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

    Public Shared Async Function CheckAndAutoUpdateListsAsync() As Task
        If Not My.Settings.AdBlockerEnabled Then Return

        Dim scheduleOption As Integer = My.Settings.AdBlockUpdateSchedule
        If scheduleOption <= 0 Then Return

        Dim lastCheckStr As String = My.Settings.AdBlockLastUpdateCheck
        Dim lastCheckTime As DateTime = DateTime.MinValue
        If Not String.IsNullOrWhiteSpace(lastCheckStr) Then
            DateTime.TryParse(lastCheckStr, lastCheckTime)
        End If

        Dim shouldUpdate As Boolean = False
        Dim timeSinceLastCheck As TimeSpan = DateTime.Now - lastCheckTime

        Select Case scheduleOption
            Case 1 ' Every 24 Hours
                If timeSinceLastCheck.TotalHours >= 24 Then shouldUpdate = True
            Case 2 ' Every Week (7 Days)
                If timeSinceLastCheck.TotalDays >= 7 Then shouldUpdate = True
            Case 3 ' Every Month (30 Days)
                If timeSinceLastCheck.TotalDays >= 30 Then shouldUpdate = True
        End Select

        If Not shouldUpdate Then Return

        Try
            Dim savedConfig As String = My.Settings.FilterListsConfig
            Dim filterUrls As New Dictionary(Of String, String) From {
                {"EasyList", "https://easylist.to/easylist/easylist.txt"},
                {"EasyPrivacy", "https://easylist.to/easylist/easyprivacy.txt"},
                {"Fanboy's Annoyance", "https://secure.fanboy.co.nz/fanboy-annoyance.txt"}
            }

            Dim enabledLists As New List(Of String)()
            If Not String.IsNullOrWhiteSpace(savedConfig) Then
                Dim entries As String() = savedConfig.Split(";"c)
                For Each entry As String In entries
                    Dim parts As String() = entry.Split("|"c)
                    If parts.Length >= 2 AndAlso parts(1) = "1" Then
                        enabledLists.Add(parts(0))
                    End If
                Next
            Else
                enabledLists.AddRange(filterUrls.Keys)
            End If

            Dim successAny As Boolean = False
            For Each listName As String In enabledLists
                If filterUrls.ContainsKey(listName) Then
                    Dim success As Boolean = Await UpdateFilterListAsync(listName, filterUrls(listName))
                    If success Then successAny = True
                End If
            Next

            If successAny Then
                LoadAllRules()
            End If

            My.Settings.AdBlockLastUpdateCheck = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
            My.Settings.Save()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Background auto-update failed: " & ex.Message)
        End Try
    End Function

End Class

