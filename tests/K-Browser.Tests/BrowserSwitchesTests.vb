Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' The browser engine's command-line switches. Chromium treats "--dns-prefetch-disable" as on whenever it
''' is present (any "=value" is ignored), and keeps only the last of repeated "--enable-features" switches.
''' </summary>
Public Class BrowserSwitchesTests

    Private Shared Function Args(preload As Boolean, Optional gpu As Boolean = True) As String
        Dim perf As New PerformanceSettingsModel With {.PreloadPages = preload, .UseHardwareAcceleration = gpu}
        Return If(BrowserInitializationService.CreateEnvironmentOptions(perf, False).AdditionalBrowserArguments, "")
    End Function

    <Fact>
    Public Sub PreloadOn_LeavesDnsPrefetchingOn()
        Assert.DoesNotContain("dns-prefetch-disable", Args(preload:=True))
    End Sub

    <Fact>
    Public Sub PreloadOff_TurnsDnsPrefetchingOff()
        Assert.Contains("--dns-prefetch-disable", Args(preload:=False))
        Assert.DoesNotContain("--dns-prefetch-disable=", Args(preload:=False))
    End Sub

    <Theory>
    <InlineData(True)>
    <InlineData(False)>
    Public Sub NoFeatureSwitchesThatCancelOrDoNothing(preload As Boolean)
        Dim a = Args(preload)
        Assert.DoesNotContain("PredictivePrefetching", a)
        Assert.DoesNotContain("site-per-process", a)
        Assert.DoesNotContain("SingleProcess", a)
    End Sub

    <Fact>
    Public Sub HardwareAccelerationOff_DisablesTheGpu()
        Assert.Contains("--disable-gpu", Args(preload:=True, gpu:=False))
        Assert.DoesNotContain("--disable-gpu", Args(preload:=True, gpu:=True))
    End Sub

End Class
