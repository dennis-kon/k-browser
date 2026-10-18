Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' The custom-proxy setting is passed to the browser engine inside a space-separated argument string,
''' so the host text must never be able to smuggle in another switch.
''' </summary>
Public Class ProxyArgumentTests

    <Theory>
    <InlineData("proxy.corp.local", 3128, "--proxy-server=proxy.corp.local:3128")>
    <InlineData("10.0.0.5", 8080, "--proxy-server=10.0.0.5:8080")>
    <InlineData("localhost", 1, "--proxy-server=localhost:1")>
    <InlineData("Proxy-1.Example.com", 65535, "--proxy-server=Proxy-1.Example.com:65535")>
    <InlineData("[::1]", 8080, "--proxy-server=[::1]:8080")>
    <InlineData("  proxy.local  ", 8080, "--proxy-server=proxy.local:8080")>
    Public Sub EnabledProxy_BecomesAProxyServerSwitch(host As String, port As Integer, expected As String)
        Assert.Equal(expected, BrowserInitializationService.BuildProxyArgument(True, host, port))
    End Sub

    <Fact>
    Public Sub DisabledProxy_AddsNothing()
        Assert.Equal("", BrowserInitializationService.BuildProxyArgument(False, "proxy.local", 8080))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData("   ")>
    <InlineData(Nothing)>
    <InlineData("bad host")>
    <InlineData("proxy.local --disable-web-security")>
    <InlineData("proxy.local" & vbTab & "--no-sandbox")>
    <InlineData("proxy.local" & vbLf & "--no-sandbox")>
    <InlineData("a;b")>
    <InlineData("a""b")>
    <InlineData("a'b")>
    <InlineData("a=b")>
    <InlineData("a,b")>
    <InlineData("http://proxy.local")>
    <InlineData("proxy.local:8080")>
    <InlineData("proxy.local/path")>
    <InlineData("-proxy.local")>
    <InlineData("--no-sandbox")>
    <InlineData("proxy..local")>
    <InlineData("[not-ipv6]")>
    Public Sub UnsafeOrMalformedHost_IsRefused(host As String)
        Assert.False(BrowserInitializationService.IsValidProxyHost(host))
        Assert.Equal("", BrowserInitializationService.BuildProxyArgument(True, host, 8080))
    End Sub

    <Theory>
    <InlineData(0)>
    <InlineData(-1)>
    <InlineData(65536)>
    Public Sub OutOfRangePort_IsRefused(port As Integer)
        Assert.Equal("", BrowserInitializationService.BuildProxyArgument(True, "proxy.local", port))
    End Sub

    <Fact>
    Public Sub EnvironmentOptions_IncludeTheProxy_AlongsideTheOtherSwitches()
        Dim options = BrowserInitializationService.CreateEnvironmentOptions(
            New PerformanceSettingsModel(), blockThirdPartyCookies:=True,
            proxyEnabled:=True, proxyHost:="proxy.local", proxyPort:=3128)
        Assert.Contains("--proxy-server=proxy.local:3128", options.AdditionalBrowserArguments)
        Assert.Contains("--block-third-party-cookies", options.AdditionalBrowserArguments)
    End Sub

    <Fact>
    Public Sub EnvironmentOptions_WithoutAProxy_HaveNoProxySwitch()
        Dim options = BrowserInitializationService.CreateEnvironmentOptions(
            New PerformanceSettingsModel(), blockThirdPartyCookies:=False)
        Assert.DoesNotContain("--proxy-server", If(options.AdditionalBrowserArguments, ""))
    End Sub

End Class
