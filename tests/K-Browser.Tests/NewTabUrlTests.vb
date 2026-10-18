Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' A new tab opens the home page (or a blank page when the user chose that). It must never open a search
''' for the application's own folder, which is what happened when the address was a local file path.
''' </summary>
Public Class NewTabUrlTests

    <Theory>
    <InlineData(0)>
    <InlineData(1)>
    <InlineData(7)>
    Public Sub NewTab_OpensTheHomePage(newTabPage As Integer)
        Assert.Equal("https://example.com/start", AppManager.NewTabUrl(newTabPage, "https://example.com/start"))
    End Sub

    <Fact>
    Public Sub NewTab_BlankPageSetting_OpensAboutBlank()
        Assert.Equal("about:blank", AppManager.NewTabUrl(2, "https://example.com/start"))
    End Sub

    <Fact>
    Public Sub HomePage_WithoutScheme_GetsOne()
        Assert.Equal("http://example.com", AppManager.NewTabUrl(0, "example.com"))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData("   ")>
    <InlineData(Nothing)>
    Public Sub NoHomePageSet_FallsBackToAWebPage(home As String)
        Assert.Equal("https://www.google.com", AppManager.NewTabUrl(0, home))
        Assert.Equal("https://www.google.com", AppManager.HomePageOrDefault(home))
    End Sub

    <Theory>
    <InlineData("C:\Program Files\K-Browser\homepage\index.html", "file:///C:/Program%20Files/K-Browser/homepage/index.html")>
    <InlineData("C:\Users\me\page.html", "file:///C:/Users/me/page.html")>
    Public Sub LocalFilePath_OpensTheFile_NotASearch(path As String, expected As String)
        Assert.Equal(expected, AppManager.ResolveUrlOrSearch(path, "Google"))
    End Sub

End Class
