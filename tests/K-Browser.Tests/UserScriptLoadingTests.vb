Imports System.IO
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' User scripts are read once per run (off the UI thread) and each script's page wrapper is built once,
''' instead of re-reading the folder for every window and rebuilding every wrapper for every tab.
''' </summary>
Public Class UserScriptLoadingTests
    Implements IDisposable

    Private ReadOnly _dir As String = Path.Combine(Path.GetTempPath(), "kb-userscripts-" & Guid.NewGuid().ToString("N"))

    Public Sub New()
        Directory.CreateDirectory(_dir)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            Directory.Delete(_dir, True)
        Catch
        End Try
    End Sub

    <Fact>
    Public Sub Scripts_AreParsed_WithTheirWrapperBuiltOnce()
        File.WriteAllText(Path.Combine(_dir, "a.user.js"),
            "// ==UserScript==" & vbLf & "// @name Dark docs" & vbLf & "// @match https://docs.example.com/*" & vbLf &
            "// @exclude https://docs.example.com/private/*" & vbLf & "// ==/UserScript==" & vbLf & "document.body.style.background='#000';")

        Dim scripts = UserScriptManager.LoadScripts(_dir)

        Dim s = Assert.Single(scripts)
        Assert.Equal("Dark docs", s.Name)
        Assert.Equal({"https://docs.example.com/*"}, s.MatchPatterns.ToArray())
        Assert.Equal({"https://docs.example.com/private/*"}, s.Excludes.ToArray())
        Assert.Equal(UserScriptMatcher.BuildWrappedCode(s.Name, s.Code, s.MatchPatterns, s.Excludes), s.WrappedCode)
    End Sub

    <Fact>
    Public Sub ScriptWithoutPatterns_RunsEverywhere_AndIsNamedAfterItsFile()
        File.WriteAllText(Path.Combine(_dir, "plain.user.js"), "console.log(1);")
        Dim s = Assert.Single(UserScriptManager.LoadScripts(_dir))
        Assert.Equal("plain.user", s.Name)
        Assert.Equal({"*://*/*"}, s.MatchPatterns.ToArray())
    End Sub

    <Fact>
    Public Sub OnlyUserJsFiles_IncludingSubfolders_AreLoaded()
        File.WriteAllText(Path.Combine(_dir, "notes.txt"), "not a script")
        File.WriteAllText(Path.Combine(_dir, "other.js"), "not a user script")
        Directory.CreateDirectory(Path.Combine(_dir, "sub"))
        File.WriteAllText(Path.Combine(_dir, "sub", "b.user.js"), "1")
        Assert.Single(UserScriptManager.LoadScripts(_dir))
    End Sub

    <Fact>
    Public Sub MissingFolder_GivesNoScripts()
        Assert.Empty(UserScriptManager.LoadScripts(Path.Combine(_dir, "nope")))
    End Sub

End Class
