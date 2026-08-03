Imports System.IO
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Primary manager for WebView2 environment creation and process configuration.
''' Connects performance options (Graphics Acceleration, Page Preloading) and Privacy options.
''' </summary>
Public Class TabProcessManager

    Private Shared ReadOnly LockObj As New Object()
    Private Shared SharedEnvironment As CoreWebView2Environment

    ''' <summary>
    ''' Gets or creates the primary shared CoreWebView2Environment configured with Chromium site isolation,
    ''' GPU acceleration policies, preloading options, and privacy switches.
    ''' </summary>
    Public Shared Async Function GetSharedEnvironmentAsync() As Task(Of CoreWebView2Environment)
        If SharedEnvironment IsNot Nothing Then
            Return SharedEnvironment
        End If

        Try
            Dim userDataDir As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "UserData")
            If Not Directory.Exists(userDataDir) Then
                Directory.CreateDirectory(userDataDir)
            End If

            ' Automatically load settings from Settings.json on startup
            Dim settingsSvc As New SettingsService()
            Dim settingsModel As PrivacySettingsModel = Await settingsSvc.LoadSettingsAsync()

            Dim perfSettings As PerformanceSettingsModel = If(settingsModel.Performance, New PerformanceSettingsModel())
            Dim options As CoreWebView2EnvironmentOptions = BrowserInitializationService.CreateEnvironmentOptions(perfSettings, settingsModel.BlockThirdPartyCookies)

            ' Append process isolation switches
            Dim curArgs As String = If(options.AdditionalBrowserArguments, "")
            options.AdditionalBrowserArguments = (curArgs & " --enable-features=IsolateOrigins,site-per-process --disable-features=SingleProcess").Trim()

            ' Configure live TabLifecycleManager options
            TabLifecycleManager.FadeInactiveTabsEnabled = perfSettings.FadeInactiveTabs

            SharedEnvironment = Await CoreWebView2Environment.CreateAsync(Nothing, userDataDir, options)
            Return SharedEnvironment
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error initializing TabProcessManager shared environment: " & ex.Message)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Creates a custom isolated environment for private or isolated partition tabs.
    ''' </summary>
    Public Shared Async Function CreateIsolatedEnvironmentAsync(ByVal partitionId As String) As Task(Of CoreWebView2Environment)
        Dim env As CoreWebView2Environment = Nothing
        Try
            Dim safeId As String = System.Text.RegularExpressions.Regex.Replace(partitionId, "[^a-zA-Z0-9_]", "_")
            Dim userDataDir As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "Partitions", safeId)
            If Not Directory.Exists(userDataDir) Then
                Directory.CreateDirectory(userDataDir)
            End If

            Dim settingsSvc As New SettingsService()
            Dim settingsModel As PrivacySettingsModel = Await settingsSvc.LoadSettingsAsync()
            Dim perfSettings As PerformanceSettingsModel = If(settingsModel.Performance, New PerformanceSettingsModel())

            Dim options As CoreWebView2EnvironmentOptions = BrowserInitializationService.CreateEnvironmentOptions(perfSettings, settingsModel.BlockThirdPartyCookies)
            Dim curArgs As String = If(options.AdditionalBrowserArguments, "")
            options.AdditionalBrowserArguments = (curArgs & " --enable-features=IsolateOrigins,site-per-process --disable-features=SingleProcess").Trim()

            env = Await CoreWebView2Environment.CreateAsync(Nothing, userDataDir, options)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error creating isolated environment: " & ex.Message)
        End Try

        If env IsNot Nothing Then
            Return env
        End If

        Return Await GetSharedEnvironmentAsync()
    End Function

    ''' <summary>
    ''' Retrieves human-readable process role description for WebView2 process kinds.
    ''' </summary>
    Public Shared Function GetProcessKindName(ByVal kind As CoreWebView2ProcessKind) As String
        Select Case kind.ToString().ToLowerInvariant()
            Case "browser"
                Return "Browser Main"
            Case "renderer", "render"
                Return "Tab Renderer"
            Case "gpu"
                Return "GPU Accelerator"
            Case "utility"
                Return "Utility Service"
            Case Else
                Return "Sub-process (" & kind.ToString() & ")"
        End Select
    End Function

End Class
