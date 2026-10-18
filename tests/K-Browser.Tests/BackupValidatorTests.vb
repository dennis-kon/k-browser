Imports System.Text.Json
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' A backup ZIP is just a file the user is handed. Restoring one must not be able to switch off the
''' browser's protections with malformed values, point downloads at a network share, or start the
''' browser on a script URL.
''' </summary>
Public Class BackupValidatorTests

    Private Shared Function Config(ParamArray pairs() As String) As Dictionary(Of String, String)
        Dim d As New Dictionary(Of String, String)()
        For i As Integer = 0 To pairs.Length - 2 Step 2
            d(pairs(i)) = pairs(i + 1)
        Next
        Return d
    End Function

    ' ── URLs ───────────────────────────────────────────────────────────────

    <Theory>
    <InlineData("https://www.google.com")>
    <InlineData("http://example.com/start")>
    <InlineData("file:///C:/Users/me/start.html")>
    <InlineData("about:blank")>
    Public Sub Url_AllowedForStartPagesAndSessions(url As String)
        Assert.True(BackupValidator.IsAllowedStartUrl(url))
    End Sub

    <Theory>
    <InlineData("javascript:alert(1)")>
    <InlineData("JaVaScRiPt:alert(1)")>
    <InlineData("data:text/html,<script>alert(1)</script>")>
    <InlineData("vbscript:msgbox(1)")>
    <InlineData("ftp://example.com/")>
    <InlineData("about:config")>
    <InlineData("ms-msdt:/id PCWDiagnostic")>
    <InlineData("")>
    <InlineData(Nothing)>
    Public Sub Url_RefusedForStartPagesAndSessions(url As String)
        Assert.False(BackupValidator.IsAllowedStartUrl(url))
    End Sub

    <Theory>
    <InlineData("example.com")>
    <InlineData("www.example.com/path")>
    <InlineData("localhost:3000")>
    Public Sub HomePage_MayBeABareHostName(value As String)
        Dim cfg = BackupValidator.SanitizeUserConfig(Config("HomePageUrl", value))
        Assert.Equal(value, cfg("HomePageUrl"))
    End Sub

    <Fact>
    Public Sub HomePage_WithADangerousScheme_IsDropped()
        Assert.False(BackupValidator.SanitizeUserConfig(Config("HomePageUrl", "javascript:alert(1)")).ContainsKey("HomePageUrl"))
        Assert.False(BackupValidator.SanitizeUserConfig(Config("HomePageUrl", "data:text/html,x")).ContainsKey("HomePageUrl"))
    End Sub

    ' ── Settings values ────────────────────────────────────────────────────

    <Theory>
    <InlineData("google", "Google")>
    <InlineData("BING", "Bing")>
    <InlineData("DuckDuckGo", "DuckDuckGo")>
    <InlineData("yahoo", "Yahoo")>
    Public Sub SearchEngine_IsNormalised(value As String, expected As String)
        Assert.Equal(expected, BackupValidator.SanitizeUserConfig(Config("SearchEngine", value))("SearchEngine"))
    End Sub

    <Theory>
    <InlineData("evil")>
    <InlineData("https://evil.com/?q=")>
    <InlineData("")>
    Public Sub SearchEngine_Unknown_IsDropped(value As String)
        Assert.False(BackupValidator.SanitizeUserConfig(Config("SearchEngine", value)).ContainsKey("SearchEngine"))
    End Sub

    <Theory>
    <InlineData("HttpsOnlyMode", "true", "True")>
    <InlineData("HttpsOnlyMode", "False", "False")>
    <InlineData("AdBlockerEnabled", "TRUE", "True")>
    <InlineData("PermissionCamera", "false", "False")>
    <InlineData("FullScreenOnStartup", "True", "True")>
    Public Sub Booleans_AreNormalised(key As String, value As String, expected As String)
        Assert.Equal(expected, BackupValidator.SanitizeUserConfig(Config(key, value))(key))
    End Sub

    <Theory>
    <InlineData("HttpsOnlyMode", "yes")>
    <InlineData("HttpsOnlyMode", "1")>
    <InlineData("AdBlockerEnabled", "")>
    <InlineData("PermissionMic", "maybe")>
    Public Sub Booleans_ThatAreNotBooleans_AreDropped(key As String, value As String)
        Assert.False(BackupValidator.SanitizeUserConfig(Config(key, value)).ContainsKey(key))
    End Sub

    <Theory>
    <InlineData("0", True)>
    <InlineData("3", True)>
    <InlineData("4", False)>
    <InlineData("-1", False)>
    <InlineData("big", False)>
    Public Sub FontSize_MustBeInRange(value As String, kept As Boolean)
        Assert.Equal(kept, BackupValidator.SanitizeUserConfig(Config("FontSize", value)).ContainsKey("FontSize"))
    End Sub

    <Theory>
    <InlineData("C:\Users\me\Downloads", True)>
    <InlineData("D:\files", True)>
    <InlineData("\\evil-server\share", False)>
    <InlineData("//evil-server/share", False)>
    <InlineData("relative\path", False)>
    <InlineData("C:\bad|name", False)>
    <InlineData("", False)>
    Public Sub DownloadsFolder_MustBeALocalAbsolutePath(value As String, kept As Boolean)
        Assert.Equal(kept, BackupValidator.SanitizeUserConfig(Config("DownloadsFolder", value)).ContainsKey("DownloadsFolder"))
    End Sub

    <Fact>
    Public Sub UnknownKeys_AreDropped()
        Dim cfg = BackupValidator.SanitizeUserConfig(Config("SomethingNew", "x", "HttpsOnlyMode", "true"))
        Assert.False(cfg.ContainsKey("SomethingNew"))
        Assert.True(cfg.ContainsKey("HttpsOnlyMode"))
    End Sub

    <Fact>
    Public Sub NullConfig_GivesAnEmptyResult()
        Assert.Empty(BackupValidator.SanitizeUserConfig(Nothing))
    End Sub

    ' ── Session.json ───────────────────────────────────────────────────────

    <Fact>
    Public Sub Session_KeepsOnlyTabsWithAllowedUrls()
        Dim session As New SessionModel()
        session.Tabs.Add(New TabItemModel With {.Url = "https://a.com/"})
        session.Tabs.Add(New TabItemModel With {.Url = "javascript:alert(1)"})
        session.Tabs.Add(New TabItemModel With {.Url = "https://b.com/"})
        session.SelectedTab = 2

        Dim cleaned = BackupValidator.FilterSession(session)

        Assert.Equal(New String() {"https://a.com/", "https://b.com/"}, cleaned.Tabs.Select(Function(t) t.Url).ToArray())
        Assert.InRange(cleaned.SelectedTab, 0, cleaned.Tabs.Count - 1)
    End Sub

    <Fact>
    Public Sub Session_SelectedTab_IsClampedIntoRange()
        Dim session As New SessionModel()
        session.Tabs.Add(New TabItemModel With {.Url = "https://a.com/"})
        session.SelectedTab = 99
        Assert.Equal(0, BackupValidator.FilterSession(session).SelectedTab)
    End Sub

    ' ── Settings.json ──────────────────────────────────────────────────────

    <Fact>
    Public Sub SettingsJson_OfTheRightShape_IsAccepted()
        Assert.True(BackupValidator.IsValidSettingsJson(JsonSerializer.Serialize(New PrivacySettingsModel())))
    End Sub

    <Theory>
    <InlineData("not json")>
    <InlineData("[]")>
    <InlineData("")>
    <InlineData("{""StartupMode"":""run-this""}")>
    <InlineData("{""BlockThirdPartyCookies"":""maybe""}")>
    Public Sub SettingsJson_OfTheWrongShape_IsRefused(json As String)
        Assert.False(BackupValidator.IsValidSettingsJson(json))
    End Sub

End Class
