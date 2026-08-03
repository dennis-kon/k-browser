Imports System
Imports Microsoft.Web.WebView2.Core

''' <summary>
''' Infrastructure service responsible for constructing CoreWebView2EnvironmentOptions
''' and configuring Chromium command-line switches based on performance and browser settings.
''' Follows SOLID principles and Clean Architecture.
''' </summary>
Public Class BrowserInitializationService

    ''' <summary>
    ''' Constructs CoreWebView2EnvironmentOptions configured with browser arguments
    ''' for Hardware Acceleration, Preloading, and Privacy settings.
    ''' </summary>
    Public Shared Function CreateEnvironmentOptions(ByVal perfSettings As PerformanceSettingsModel, ByVal blockThirdPartyCookies As Boolean) As CoreWebView2EnvironmentOptions
        Dim options As New CoreWebView2EnvironmentOptions()
        Dim switches As New System.Text.StringBuilder()

        If perfSettings IsNot Nothing Then
            ' Section 1 - Graphics: Disable GPU acceleration when hardware acceleration is disabled by user
            If Not perfSettings.UseHardwareAcceleration Then
                switches.Append("--disable-gpu --disable-gpu-compositing ")
                System.Diagnostics.Debug.WriteLine("BrowserInitializationService: Disabling GPU acceleration (--disable-gpu).")
            End If

            ' Section 3 - Page Loading: Enable predictive preloading and DNS prefetching when enabled
            If perfSettings.PreloadPages Then
                switches.Append("--enable-features=PredictivePrefetching --dns-prefetch-disable=false ")
                System.Diagnostics.Debug.WriteLine("BrowserInitializationService: Enabling predictive preloading and DNS prefetching.")
            Else
                switches.Append("--dns-prefetch-disable=true ")
            End If
        End If

        ' Third-party cookie blocking switch
        If blockThirdPartyCookies Then
            switches.Append("--block-third-party-cookies ")
        End If

        Dim finalSwitches As String = switches.ToString().Trim()
        If Not String.IsNullOrEmpty(finalSwitches) Then
            options.AdditionalBrowserArguments = finalSwitches
        End If

        Return options
    End Function

End Class
