Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' Blocked-site, phishing-site and per-site JavaScript lists used to be matched with
''' url.Contains(entry): "example.com" also matched "notexample.com" and any URL that merely
''' mentioned it in a query string, and a lookalike host slipped through just as easily.
''' </summary>
Public Class SiteFilterTests

    <Theory>
    <InlineData("example.com", "https://example.com/")>
    <InlineData("example.com", "https://sub.example.com/x")>
    <InlineData("example.com", "https://a.b.example.com/")>
    <InlineData("example.com", "http://example.com:8080/x")>
    <InlineData("EXAMPLE.com", "https://Example.COM/")>
    <InlineData("  example.com  ", "https://example.com/")>
    <InlineData("https://example.com", "http://example.com/")>
    <InlineData("example.com/", "https://example.com/anything")>
    Public Sub HostEntry_MatchesTheHostAndItsSubdomains(entry As String, url As String)
        Assert.True(SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("example.com", "https://notexample.com/")>
    <InlineData("example.com", "https://example.com.evil.com/")>
    <InlineData("example.com", "https://evil.com/?q=example.com")>
    <InlineData("example.com", "https://evil.com/example.com/page")>
    <InlineData("example.com", "https://evil.com#example.com")>
    <InlineData("example.com", "https://example.com@evil.com/")>
    <InlineData("sub.example.com", "https://example.com/")>
    Public Sub HostEntry_DoesNotMatchOtherHosts(entry As String, url As String)
        Assert.False(SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("example.com/login", "https://example.com/login")>
    <InlineData("example.com/login", "https://example.com/login?x=1")>
    <InlineData("example.com/login", "https://example.com/login/step2")>
    <InlineData("https://example.com/login", "http://example.com/login")>
    <InlineData("example.com/a?b=1", "https://example.com/a?b=1&c=2")>
    Public Sub PathEntry_MatchesThatPathOnThatHost(entry As String, url As String)
        Assert.True(SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("example.com/login", "https://example.com/other")>
    <InlineData("example.com/login", "https://example.com/")>
    <InlineData("example.com/login", "https://evil.com/example.com/login")>
    <InlineData("example.com/login", "https://evil.com/?next=example.com/login")>
    Public Sub PathEntry_DoesNotMatchElsewhere(entry As String, url As String)
        Assert.False(SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("facebook", "https://facebook.com/")>
    <InlineData("facebook", "https://m.facebook.com/x")>
    <InlineData("Face", "https://facebook.com/")>
    Public Sub KeywordEntry_MatchesWithinTheHostName(entry As String, url As String)
        Assert.True(SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("facebook", "https://google.com/search?q=facebook")>
    <InlineData("facebook", "https://example.com/facebook")>
    Public Sub KeywordEntry_DoesNotMatchTheRestOfTheUrl(entry As String, url As String)
        Assert.False(SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("localhost:3000", "http://localhost:3000/", True)>
    <InlineData("localhost:3000", "http://localhost:3001/", False)>
    <InlineData("localhost:3000", "http://localhost/", False)>
    <InlineData("example.com:8443", "https://example.com:8443/x", True)>
    <InlineData("example.com:8443", "https://example.com/x", False)>
    Public Sub EntryWithAPort_OnlyMatchesThatPort(entry As String, url As String, expected As Boolean)
        Assert.Equal(expected, SiteFilter.UrlMatchesEntry(url, entry))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData("   ")>
    <InlineData(Nothing)>
    Public Sub EmptyEntry_MatchesNothing(entry As String)
        Assert.False(SiteFilter.UrlMatchesEntry("https://example.com/", entry))
    End Sub

    <Fact>
    Public Sub UnparseableUrl_FallsBackToASubstringCheck()
        ' Fail closed for blocking: if we cannot tell what the host is, keep the old behaviour.
        Assert.True(SiteFilter.UrlMatchesEntry("not a url but evil.com inside", "evil.com"))
        Assert.False(SiteFilter.UrlMatchesEntry("not a url", "evil.com"))
    End Sub

    <Fact>
    Public Sub AnyEntry_OfAList_IsEnough()
        Dim entries = New String() {"a.com", "b.com"}
        Assert.True(SiteFilter.MatchesAny("https://b.com/", entries))
        Assert.False(SiteFilter.MatchesAny("https://c.com/", entries))
        Assert.False(SiteFilter.MatchesAny("https://c.com/", Nothing))
    End Sub

    ' ── Sub-frames ─────────────────────────────────────────────────────────

    <Fact>
    Public Sub Frame_OnABlockedSite_IsBlocked()
        Assert.True(SiteFilter.IsFrameBlocked("https://ads.evil.com/x", False, New String() {"evil.com"}, Nothing))
    End Sub

    <Fact>
    Public Sub Frame_OnAPhishingSite_IsBlocked()
        Assert.True(SiteFilter.IsFrameBlocked("https://phish.test/login", False, Nothing, New String() {"phish.test"}))
    End Sub

    <Fact>
    Public Sub PlainHttpFrame_IsBlocked_InHttpsOnlyMode_ButNotOtherwise()
        Assert.True(SiteFilter.IsFrameBlocked("http://embed.example.com/", True, Nothing, Nothing))
        Assert.False(SiteFilter.IsFrameBlocked("http://embed.example.com/", False, Nothing, Nothing))
        Assert.False(SiteFilter.IsFrameBlocked("https://embed.example.com/", True, Nothing, Nothing))
    End Sub

    <Theory>
    <InlineData("about:blank")>
    <InlineData("about:srcdoc")>
    <InlineData("blob:https://example.com/1234")>
    <InlineData("data:text/html,hi")>
    <InlineData("")>
    Public Sub NonWebFrames_AreNeverBlocked(url As String)
        Assert.False(SiteFilter.IsFrameBlocked(url, True, New String() {"example.com", "about"}, New String() {"example.com", "about"}))
    End Sub

End Class
