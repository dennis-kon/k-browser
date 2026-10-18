Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' "Open page in Internet Explorer" hands the address bar text to another program's command line.
''' Only a plain http(s) address may go through, and it can never start with a dash.
''' </summary>
Public Class ExternalBrowserTests

    <Theory>
    <InlineData("https://example.com/", "https://example.com/")>
    <InlineData("http://example.com/a b", "http://example.com/a%20b")>
    <InlineData("  https://example.com/x  ", "https://example.com/x")>
    <InlineData("HTTPS://EXAMPLE.COM/", "https://example.com/")>
    Public Sub HttpAddress_IsPassedOn_Normalised(text As String, expected As String)
        Assert.Equal(expected, ExternalBrowser.SafeUrlArgument(text))
    End Sub

    <Theory>
    <InlineData("--remote-debugging-port=9222")>
    <InlineData("-private")>
    <InlineData("/?")>
    <InlineData("javascript:alert(1)")>
    <InlineData("file:///C:/Windows/System32/calc.exe")>
    <InlineData("ftp://example.com/")>
    <InlineData("example.com")>
    <InlineData("")>
    <InlineData(Nothing)>
    Public Sub Anything_ElseIsRefused(text As String)
        Assert.Null(ExternalBrowser.SafeUrlArgument(text))
    End Sub

    <Fact>
    Public Sub QuotesAndSpaces_CanNotSurviveIntoTheArgument()
        Dim arg = ExternalBrowser.SafeUrlArgument("https://example.com/a"" -x ""b c")
        If arg IsNot Nothing Then
            Assert.DoesNotContain("""", arg)
            Assert.DoesNotContain(" ", arg)
        End If
    End Sub

End Class
