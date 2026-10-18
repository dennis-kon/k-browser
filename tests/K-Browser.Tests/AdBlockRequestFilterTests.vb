Imports Advance_Webbrowser
Imports Microsoft.Web.WebView2.Core
Imports Xunit

''' <summary>
''' Every request covered by the request filter waits for the browser window's UI thread. The filter therefore
''' covers only the kinds of request the ad blocker can act on: never top-level pages, which it always lets through.
''' </summary>
Public Class AdBlockRequestFilterTests

    <Fact>
    Public Sub TopLevelPages_AreNotFiltered()
        Assert.DoesNotContain(CoreWebView2WebResourceContext.Document, AdBlockRequestFilter.Contexts)
        Assert.DoesNotContain(CoreWebView2WebResourceContext.All, AdBlockRequestFilter.Contexts)
    End Sub

    <Theory>
    <InlineData(CoreWebView2WebResourceContext.Script)>
    <InlineData(CoreWebView2WebResourceContext.Image)>
    <InlineData(CoreWebView2WebResourceContext.XmlHttpRequest)>
    <InlineData(CoreWebView2WebResourceContext.Fetch)>
    <InlineData(CoreWebView2WebResourceContext.Media)>
    <InlineData(CoreWebView2WebResourceContext.Stylesheet)>
    <InlineData(CoreWebView2WebResourceContext.Font)>
    <InlineData(CoreWebView2WebResourceContext.Ping)>
    <InlineData(CoreWebView2WebResourceContext.Other)>
    Public Sub AdRelatedRequests_AreFiltered(context As CoreWebView2WebResourceContext)
        Assert.Contains(context, AdBlockRequestFilter.Contexts)
    End Sub

    <Fact>
    Public Sub EachContext_IsListedOnce()
        Assert.Equal(AdBlockRequestFilter.Contexts.Distinct().Count(), AdBlockRequestFilter.Contexts.Count)
    End Sub

End Class
