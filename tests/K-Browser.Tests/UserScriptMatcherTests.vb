Imports System.Text.Json
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' User scripts are injected into every document the engine creates; the @match / @include header is
''' what keeps a script written for one site from running on all of them.
''' </summary>
Public Class UserScriptMatcherTests

    Private Shared Function Matches(pattern As String, url As String) As Boolean
        Return UserScriptMatcher.Matches(New String() {pattern}, url)
    End Function

    ' ── @match patterns ────────────────────────────────────────────────────

    <Theory>
    <InlineData("*://*/*", "https://a.com/x")>
    <InlineData("*://*/*", "http://a.com/")>
    <InlineData("*://*/*", "https://sub.a.com:8443/p?q=1#h")>
    <InlineData("<all_urls>", "https://a.com/x")>
    <InlineData("https://example.com/*", "https://example.com/")>
    <InlineData("https://example.com/*", "https://example.com/a/b?c=d#e")>
    <InlineData("https://example.com/*", "https://example.com:8443/")>
    <InlineData("https://example.com/*", "HTTPS://EXAMPLE.COM/")>
    <InlineData("*://*.example.com/*", "https://example.com/")>
    <InlineData("*://*.example.com/*", "http://a.b.example.com/x")>
    <InlineData("http://localhost:3000/*", "http://localhost:3000/app")>
    <InlineData("https://example.com/app/*", "https://example.com/app/page")>
    <InlineData("https://example.com/login", "https://example.com/login")>
    <InlineData("https://example.com/login", "https://example.com/login#top")>
    Public Sub Pattern_Matches(pattern As String, url As String)
        Assert.True(Matches(pattern, url))
    End Sub

    <Theory>
    <InlineData("*://*/*", "file:///C:/page.html")>
    <InlineData("*://*/*", "about:blank")>
    <InlineData("*://*/*", "ftp://a.com/x")>
    <InlineData("<all_urls>", "file:///C:/page.html")>
    <InlineData("https://example.com/*", "http://example.com/")>
    <InlineData("https://example.com/*", "https://example.com.evil.com/")>
    <InlineData("https://example.com/*", "https://evilexample.com/")>
    <InlineData("https://example.com/*", "https://sub.example.com/")>
    <InlineData("https://example.com/*", "https://example.com@evil.com/")>
    <InlineData("https://example.com/*", "https://evil.com/?u=https://example.com/")>
    <InlineData("*://*.example.com/*", "https://notexample.com/")>
    <InlineData("*://*.example.com/*", "https://example.com.evil.com/")>
    <InlineData("*://*.example.com/*", "https://evil.com@a.example.com/")>
    <InlineData("http://localhost:3000/*", "http://localhost:3001/app")>
    <InlineData("http://localhost:3000/*", "http://localhost/app")>
    <InlineData("https://example.com/app/*", "https://example.com/other")>
    <InlineData("https://example.com/login", "https://example.com/login/extra")>
    Public Sub Pattern_DoesNotMatch(pattern As String, url As String)
        Assert.False(Matches(pattern, url))
    End Sub

    ' ── @include globs ─────────────────────────────────────────────────────

    <Theory>
    <InlineData("*example.com*", "https://www.example.com/x")>
    <InlineData("http://example.com/*", "http://example.com/a")>
    Public Sub Glob_Matches(pattern As String, url As String)
        Assert.True(Matches(pattern, url))
    End Sub

    <Theory>
    <InlineData("http://example.com/*", "https://example.com/a")>
    <InlineData("*example.com/", "https://example.com/a")>
    Public Sub Glob_DoesNotMatch(pattern As String, url As String)
        Assert.False(Matches(pattern, url))
    End Sub

    ' ── Bad input fails closed ─────────────────────────────────────────────

    <Theory>
    <InlineData("")>
    <InlineData("   ")>
    <InlineData(Nothing)>
    <InlineData("/regex.*/")>
    Public Sub UnusablePattern_MatchesNothing(pattern As String)
        Assert.False(Matches(pattern, "https://example.com/"))
    End Sub

    <Fact>
    Public Sub NoPatterns_MatchesNothing()
        Assert.False(UserScriptMatcher.Matches(New String() {}, "https://example.com/"))
    End Sub

    <Fact>
    Public Sub AnyOneOfSeveralPatterns_IsEnough()
        Dim patterns = New String() {"https://a.com/*", "https://b.com/*"}
        Assert.True(UserScriptMatcher.Matches(patterns, "https://b.com/x"))
        Assert.False(UserScriptMatcher.Matches(patterns, "https://c.com/x"))
    End Sub

    ' ── The injected script ────────────────────────────────────────────────

    <Fact>
    Public Sub WrappedCode_ChecksThePageAddress_BeforeRunningTheScript()
        Dim code = UserScriptMatcher.BuildWrappedCode("Demo", "console.log('hi');", New String() {"https://example.com/*"})
        Dim guardAt = code.IndexOf("location.href", StringComparison.Ordinal)
        Dim bodyAt = code.IndexOf("console.log('hi');", StringComparison.Ordinal)
        Assert.True(guardAt >= 0, "no address check in the wrapper")
        Assert.True(bodyAt > guardAt, "the script body must come after the address check")
    End Sub

    <Fact>
    Public Sub WrappedCode_EmbedsTheRegexesAsJsonStrings()
        Dim patterns = New String() {"https://example.com/*"}
        Dim code = UserScriptMatcher.BuildWrappedCode("Demo", "x()", patterns)
        Dim regexSource As String = UserScriptMatcher.ToRegexSource(patterns(0))
        Assert.NotNull(regexSource)
        Assert.Contains(JsonSerializer.Serialize(regexSource), code)
    End Sub

    <Fact>
    Public Sub WrappedCode_NeverLetsTheScriptNameBreakOutOfItsString()
        Dim nasty As String = "x'); alert(1); //" & vbLf & """</script>"
        Dim code = UserScriptMatcher.BuildWrappedCode(nasty, "x()", New String() {"*://*/*"})
        Assert.Contains(JsonSerializer.Serialize(nasty), code)
        Assert.DoesNotContain(nasty, code)
    End Sub

    <Fact>
    Public Sub WrappedCode_WithNoUsablePattern_NeverRunsTheScript()
        Dim code = UserScriptMatcher.BuildWrappedCode("Demo", "x()", New String() {"/regex.*/"})
        Assert.Contains("return", code)
        Assert.True(code.IndexOf("return", StringComparison.Ordinal) < code.IndexOf("x()", StringComparison.Ordinal))
    End Sub


    ' ── Reading the script header ──────────────────────────────────────────

    Private Const SampleHeader As String =
        "// ==UserScript==" & vbCrLf &
        "// @name         My Helper" & vbCrLf &
        "// @namespace    https://example.org/ns" & vbCrLf &
        "// @match        https://example.com/*" & vbCrLf &
        "// @include       *://*.example.org/*" & vbTab & vbCrLf &
        "// @exclude       https://example.com/private/*" & vbCrLf &
        "// @grant        none" & vbCrLf &
        "// ==/UserScript==" & vbCrLf &
        "console.log(1);"

    <Fact>
    Public Sub Header_NameIsTheValue_NotTheDirective()
        Assert.Equal("My Helper", UserScriptMatcher.ParseHeader(SampleHeader).Name)
    End Sub

    <Fact>
    Public Sub Header_MatchAndIncludeValues_AreCollected_WithoutTheDirective()
        Dim header = UserScriptMatcher.ParseHeader(SampleHeader)
        Assert.Equal(New String() {"https://example.com/*", "*://*.example.org/*"}, header.Patterns)
    End Sub

    <Fact>
    Public Sub Header_ExcludeValues_AreCollected()
        Assert.Equal(New String() {"https://example.com/private/*"}, UserScriptMatcher.ParseHeader(SampleHeader).Excludes)
    End Sub

    <Fact>
    Public Sub Header_Namespace_IsNotMistakenForName()
        Dim text = "// ==UserScript==" & vbLf & "// @namespace  nope" & vbLf & "// ==/UserScript==" & vbLf
        Assert.Equal("", UserScriptMatcher.ParseHeader(text).Name)
    End Sub

    <Fact>
    Public Sub NoHeader_MeansNoPatterns_AndNoName()
        Dim header = UserScriptMatcher.ParseHeader("console.log(1);")
        Assert.Empty(header.Patterns)
        Assert.Empty(header.Excludes)
        Assert.Equal("", header.Name)
    End Sub

    <Fact>
    Public Sub DirectivesOutsideTheHeaderBlock_AreIgnored()
        Dim text = "// ==UserScript==" & vbLf & "// @match https://a.com/*" & vbLf & "// ==/UserScript==" & vbLf &
                   "// @match https://evil.com/*" & vbLf
        Assert.Equal(New String() {"https://a.com/*"}, UserScriptMatcher.ParseHeader(text).Patterns)
    End Sub

    ' ── Excludes ───────────────────────────────────────────────────────────

    <Fact>
    Public Sub Excluded_Pages_AreNotApplicable_EvenIfTheyMatch()
        Dim include = New String() {"https://example.com/*"}
        Dim exclude = New String() {"https://example.com/private/*"}
        Assert.True(UserScriptMatcher.IsApplicable(include, exclude, "https://example.com/public"))
        Assert.False(UserScriptMatcher.IsApplicable(include, exclude, "https://example.com/private/a"))
        Assert.False(UserScriptMatcher.IsApplicable(include, exclude, "https://other.com/"))
    End Sub

    <Fact>
    Public Sub WrappedCode_ChecksExcludes_BeforeRunningTheScript()
        Dim code = UserScriptMatcher.BuildWrappedCode("Demo", "x()", New String() {"https://example.com/*"}, New String() {"https://example.com/private/*"})
        Dim excludeSource As String = UserScriptMatcher.ToRegexSource("https://example.com/private/*")
        Assert.Contains(JsonSerializer.Serialize(excludeSource), code)
        Assert.True(code.IndexOf("excluded", StringComparison.Ordinal) < code.IndexOf("x()", StringComparison.Ordinal))
    End Sub

End Class
