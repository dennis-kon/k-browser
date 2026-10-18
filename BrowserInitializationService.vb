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
    Public Shared Function CreateEnvironmentOptions(ByVal perfSettings As PerformanceSettingsModel, ByVal blockThirdPartyCookies As Boolean,
                                                    Optional ByVal proxyEnabled As Boolean = False,
                                                    Optional ByVal proxyHost As String = "",
                                                    Optional ByVal proxyPort As Integer = 8080) As CoreWebView2EnvironmentOptions
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

        ' Custom proxy (Settings > Advanced). Only takes effect when the browser engine starts.
        Dim proxySwitch As String = BuildProxyArgument(proxyEnabled, proxyHost, proxyPort)
        If proxySwitch.Length > 0 Then switches.Append(proxySwitch).Append(" ")

        Dim finalSwitches As String = switches.ToString().Trim()
        If Not String.IsNullOrEmpty(finalSwitches) Then
            options.AdditionalBrowserArguments = finalSwitches
        End If

        Return options
    End Function

    ''' <summary>
    ''' Returns the "--proxy-server=host:port" switch for the custom proxy, or an empty string when the
    ''' proxy is off or its host/port is not valid. The switch ends up in a space-separated argument
    ''' string, so the host is checked strictly: it must not be able to carry another switch.
    ''' </summary>
    Public Shared Function BuildProxyArgument(ByVal enabled As Boolean, ByVal host As String, ByVal port As Integer) As String
        If Not enabled Then Return String.Empty
        If Not IsValidProxyHost(host) OrElse port < 1 OrElse port > 65535 Then Return String.Empty
        Return "--proxy-server=" & host.Trim() & ":" & port.ToString(System.Globalization.CultureInfo.InvariantCulture)
    End Function

    Private Shared ReadOnly HostnamePattern As New System.Text.RegularExpressions.Regex(
        "^(?!-)[A-Za-z0-9-]{1,63}(?<!-)(\.(?!-)[A-Za-z0-9-]{1,63}(?<!-))*$",
        System.Text.RegularExpressions.RegexOptions.CultureInvariant)

    ''' <summary>
    ''' True for a plain host name, IPv4 address or bracketed IPv6 address — no scheme, port, path or spaces.
    ''' </summary>
    Public Shared Function IsValidProxyHost(ByVal host As String) As Boolean
        If String.IsNullOrWhiteSpace(host) Then Return False
        Dim h As String = host.Trim()
        If h.Length > 255 Then Return False

        If h.StartsWith("[", StringComparison.Ordinal) AndAlso h.EndsWith("]", StringComparison.Ordinal) Then
            Dim address As System.Net.IPAddress = Nothing
            Return System.Net.IPAddress.TryParse(h.Substring(1, h.Length - 2), address) AndAlso
                   address.AddressFamily = System.Net.Sockets.AddressFamily.InterNetworkV6
        End If

        Return HostnamePattern.IsMatch(h)
    End Function

End Class
