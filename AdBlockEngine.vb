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

    ' Guards the per-tab blocked lists. The rules themselves need no lock: they are an immutable
    ' AdBlockRuleSet that is replaced as a whole when lists are reloaded.
    Private Shared ReadOnly LockObj As New Object()
    Private Shared _rules As AdBlockRuleSet = AdBlockRuleSet.Empty

    Private Shared ReadOnly FallbackDomains As String() = {"doubleclick.net", "adservice.google.com", "adnxs.com", "googlesyndication.com", "taboola.com", "outbrain.com", "adform.net", "scorecardresearch.com", "amazon-adsystem.com"}

    ' Keeps the blocked-items popup bounded on pages that run for hours; the count stays exact.
    Private Const MaxBlockedItemsPerTab As Integer = 500

    Public Shared Property TotalBlockedCount As Long = 0

    Private Shared ReadOnly TabBlockedItems As New Dictionary(Of Microsoft.Web.WebView2.WinForms.WebView2, List(Of BlockedTrackerItem))()
    Private Shared ReadOnly TabBlockedCounts As New Dictionary(Of Microsoft.Web.WebView2.WinForms.WebView2, Integer)()

    Private Shared ReadOnly Property Rules As AdBlockRuleSet
        Get
            Return System.Threading.Volatile.Read(_rules)
        End Get
    End Property

    Public Shared Sub RecordBlockedItem(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2, ByVal url As String)
        SyncLock LockObj
            TotalBlockedCount += 1
            If brws IsNot Nothing Then
                Dim items As List(Of BlockedTrackerItem) = Nothing
                If Not TabBlockedItems.TryGetValue(brws, items) Then
                    items = New List(Of BlockedTrackerItem)()
                    TabBlockedItems(brws) = items
                End If
                If items.Count >= MaxBlockedItemsPerTab Then items.RemoveAt(0)
                items.Add(New BlockedTrackerItem(url, DateTime.Now))

                Dim count As Integer = 0
                TabBlockedCounts.TryGetValue(brws, count)
                TabBlockedCounts(brws) = count + 1
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
            Dim count As Integer = 0
            If brws IsNot Nothing Then TabBlockedCounts.TryGetValue(brws, count)
            Return count
        End SyncLock
    End Function

    Public Shared Sub ClearTabBlockedItems(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2)
        SyncLock LockObj
            If brws IsNot Nothing AndAlso TabBlockedItems.ContainsKey(brws) Then
                TabBlockedItems(brws).Clear()
            End If
            If brws IsNot Nothing Then TabBlockedCounts.Remove(brws)
        End SyncLock
    End Sub

    Public Shared Sub RemoveTab(ByVal brws As Microsoft.Web.WebView2.WinForms.WebView2)
        SyncLock LockObj
            If brws IsNot Nothing Then
                TabBlockedItems.Remove(brws)
                TabBlockedCounts.Remove(brws)
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

        For Each site As String In My.Settings.AdBlockDisabledSites
            If String.IsNullOrWhiteSpace(site) Then Continue For
            Dim s As String = site.Trim()
            If host.Equals(s, StringComparison.OrdinalIgnoreCase) OrElse
               (host.Length > s.Length AndAlso host(host.Length - s.Length - 1) = "."c AndAlso host.EndsWith(s, StringComparison.OrdinalIgnoreCase)) Then
                Return True
            End If
        Next
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

    ''' <summary>File name a downloaded filter list is saved under.</summary>
    Public Shared Function ListFileName(ByVal listName As String) As String
        Return Regex.Replace(listName, "[^a-zA-Z0-9_]", "_") & ".txt"
    End Function

    ''' <summary>
    ''' Whether a downloaded list file should be loaded. FilterListsConfig holds "Name|1|updated;Name|0|updated";
    ''' a list switched off there is skipped. Files the config does not mention are loaded.
    ''' </summary>
    Public Shared Function IsListFileEnabled(ByVal filePath As String, ByVal config As String) As Boolean
        If String.IsNullOrWhiteSpace(config) Then Return True
        Dim fileName As String = System.IO.Path.GetFileName(filePath)
        For Each entry As String In config.Split(";"c)
            Dim parts As String() = entry.Split("|"c)
            If parts.Length >= 2 AndAlso String.Equals(ListFileName(parts(0)), fileName, StringComparison.OrdinalIgnoreCase) Then
                Return parts(1) <> "0"
            End If
        Next
        Return True
    End Function

    ''' <summary>
    ''' Reads the enabled filter lists into a new rule set and swaps it in. Lookups keep using the previous set
    ''' until the new one is complete. Safe to call from any thread; it does disk I/O, so keep it off the UI thread.
    ''' </summary>
    Public Shared Function LoadAllRules() As Integer
        Dim builder As New AdBlockRuleSetBuilder()
        Dim config As String = My.Settings.FilterListsConfig

        For Each filePath As String In System.IO.Directory.GetFiles(GetRulesDirectory(), "*.txt")
            If Not IsListFileEnabled(filePath, config) Then Continue For
            Try
                builder.AddLines(System.IO.File.ReadLines(filePath))
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error reading rule file " & filePath & ": " & ex.Message)
            End Try
        Next

        ' Add default fallback ad domains if no rules are downloaded yet
        If builder.RuleCount = 0 Then
            For Each d In FallbackDomains
                builder.AddDomain(d)
            Next
        End If

        Dim rules As AdBlockRuleSet = builder.Build()
        System.Threading.Volatile.Write(_rules, rules)
        Return rules.RuleCount
    End Function

    ''' <summary>Loads the rules on a background thread.</summary>
    Public Shared Function LoadAllRulesAsync() As Task(Of Integer)
        Return Task.Run(Function() LoadAllRules())
    End Function

    Private Shared _initialLoad As Task(Of Integer)

    ''' <summary>
    ''' Starts the first rule load once per run (on a background thread) and returns it, so the first page can
    ''' wait for the rules instead of loading unfiltered. Later calls return the same task.
    ''' </summary>
    Public Shared Function EnsureRulesLoadedAsync() As Task(Of Integer)
        SyncLock LockObj
            If _initialLoad Is Nothing Then _initialLoad = LoadAllRulesAsync()
            Return _initialLoad
        End SyncLock
    End Function

    Public Shared Function ShouldBlockDomainOnly(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) OrElse url = "about:blank" Then Return False
        Return Rules.ShouldBlockHost(url)
    End Function

    Public Shared Function ShouldBlockDocument(ByVal url As String) As Boolean
        Return ShouldBlockDomainOnly(url)
    End Function

    Public Shared Function ShouldBlock(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) OrElse url = "about:blank" Then Return False
        Return Rules.ShouldBlock(url)
    End Function

    Public Shared Async Function UpdateFilterListAsync(ByVal listName As String, ByVal url As String) As Task(Of Boolean)
        Try
            Using client As New System.Net.WebClient()
                client.Headers.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) K-Browser/1.0 AdBlocker")
                Dim rulesText As String = Await client.DownloadStringTaskAsync(url)

                If Not String.IsNullOrWhiteSpace(rulesText) Then
                    Dim fileName As String = ListFileName(listName)
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

