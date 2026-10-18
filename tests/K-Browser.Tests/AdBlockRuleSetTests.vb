Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' The ad blocker checks every image, script and request a page makes, so the rule set is built once
''' (rules lower-cased up front) and then only read. These tests pin down what it blocks.
''' </summary>
Public Class AdBlockRuleSetTests

    Private Shared Function Rules(ParamArray lines As String()) As AdBlockRuleSet
        Dim b As New AdBlockRuleSetBuilder()
        b.AddLines(lines)
        Return b.Build()
    End Function

    <Fact>
    Public Sub DomainAnchor_BlocksTheHostAndItsSubdomains()
        Dim r = Rules("||doubleclick.net^")
        Assert.True(r.ShouldBlock("https://doubleclick.net/x.js"))
        Assert.True(r.ShouldBlock("https://ad.g.doubleclick.net/x.js"))
        Assert.False(r.ShouldBlock("https://notdoubleclick.net/x.js"))
        Assert.False(r.ShouldBlock("https://example.com/doubleclick.net"))
    End Sub

    <Fact>
    Public Sub HostOnlyCheck_IgnoresPathRules()
        Dim r = Rules("||doubleclick.net^", "/adserver/")
        Assert.True(r.ShouldBlockHost("https://doubleclick.net/"))
        Assert.False(r.ShouldBlockHost("https://example.com/adserver/x.js"))
    End Sub

    <Fact>
    Public Sub PathRule_MatchesRegardlessOfCase()
        Dim r = Rules("/AdServer/")
        Assert.True(r.ShouldBlock("https://example.com/adserver/banner.js"))
        Assert.True(r.ShouldBlock("HTTPS://EXAMPLE.COM/ADSERVER/BANNER.JS"))
        Assert.False(r.ShouldBlock("https://example.com/server/banner.js"))
    End Sub

    <Fact>
    Public Sub AllowRule_WinsOverABlockRule()
        Dim r = Rules("||adnxs.com^", "@@||adnxs.com/safe")
        Assert.True(r.ShouldBlock("https://adnxs.com/track.js"))
        Assert.False(r.ShouldBlock("https://adnxs.com/safe/lib.js"))
        Assert.False(r.ShouldBlock("https://ADNXS.COM/SAFE/lib.js"))
    End Sub

    <Fact>
    Public Sub CommentsElementHidingAndSiteSpecificRules_AreIgnored()
        Dim r = Rules("! comment", "[Adblock Plus 2.0]", "example.com##.ad", "||tracker-one.com^$domain=a.com", "/ad_banner/$stylesheet")
        Assert.Equal(0, r.RuleCount)
        Assert.False(r.ShouldBlock("https://tracker-one.com/x"))
        Assert.False(r.ShouldBlock("https://site.com/ad_banner/x.css"))
    End Sub

    <Fact>
    Public Sub GenericWords_NeverBecomeRules()
        Dim r = Rules("||google.com^", "/static/", "@@cdn")
        Assert.True(r.ShouldBlock("https://google.com/")) ' a real domain anchor is still honoured
        Assert.False(r.ShouldBlock("https://site.com/static/app.js"))
    End Sub

    <Fact>
    Public Sub EmptySet_BlocksNothing()
        Assert.False(AdBlockRuleSet.Empty.ShouldBlock("https://doubleclick.net/"))
        Assert.False(AdBlockRuleSet.Empty.ShouldBlockHost("https://doubleclick.net/"))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData("about:blank")>
    <InlineData("not a url")>
    <InlineData(Nothing)>
    Public Sub OddAddresses_AreNotBlockedAndDoNotThrow(url As String)
        Dim r = Rules("||doubleclick.net^")
        Assert.False(r.ShouldBlock(url))
        Assert.False(r.ShouldBlockHost(url))
    End Sub

    <Fact>
    Public Sub RuleCount_CountsEachKind()
        Dim r = Rules("||doubleclick.net^", "/adserver/", "@@||adnxs.com/safe")
        Assert.Equal(3, r.RuleCount)
    End Sub

    <Theory>
    <InlineData("", "EasyList.txt", True)>
    <InlineData(Nothing, "EasyList.txt", True)>
    <InlineData("EasyList|0|Never;EasyPrivacy|1|x", "EasyList.txt", False)>
    <InlineData("EasyList|0|Never;EasyPrivacy|1|x", "EasyPrivacy.txt", True)>
    <InlineData("Fanboy's Annoyance|0|x", "Fanboy_s_Annoyance.txt", False)>
    <InlineData("EasyList|0|Never", "MyOwnList.txt", True)>
    <InlineData("EasyList|0|Never", "C:\rules\EASYLIST.txt", False)>
    Public Sub TurnedOffLists_AreNotLoaded(config As String, file As String, expected As Boolean)
        Assert.Equal(expected, AdBlockEngine.IsListFileEnabled(file, config))
    End Sub

    <Fact>
    Public Sub Builder_CanAddFallbackDomains()
        Dim b As New AdBlockRuleSetBuilder()
        b.AddDomain("taboola.com")
        Assert.True(b.Build().ShouldBlock("https://cdn.taboola.com/x.js"))
    End Sub

End Class
