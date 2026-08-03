Imports System.IO
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Class TabProcessManager

    Private Shared ReadOnly LockObj As New Object()
    Private Shared SharedEnvironment As CoreWebView2Environment

    ''' <summary>
    ''' Gets or creates the primary shared CoreWebView2Environment configured with Chromium site isolation switches.
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

            Dim options As New CoreWebView2EnvironmentOptions()
            ' Enforce Chromium process isolation per origin and disable single-process flags
            options.AdditionalBrowserArguments = "--enable-features=IsolateOrigins,site-per-process --disable-features=SingleProcess"

            ' Automatically load privacy preference from Settings.json on startup
            Dim settingsSvc As New SettingsService()
            Dim privacySvc As New PrivacySettingsService(settingsSvc)
            Dim privacyModel As PrivacySettingsModel = Await settingsSvc.LoadSettingsAsync()
            privacySvc.ConfigureEnvironmentOptions(options, privacyModel.BlockThirdPartyCookies)

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

            Dim options As New CoreWebView2EnvironmentOptions()
            options.AdditionalBrowserArguments = "--enable-features=IsolateOrigins,site-per-process --disable-features=SingleProcess"

            Dim settingsSvc As New SettingsService()
            Dim privacySvc As New PrivacySettingsService(settingsSvc)
            Dim privacyModel As PrivacySettingsModel = Await settingsSvc.LoadSettingsAsync()
            privacySvc.ConfigureEnvironmentOptions(options, privacyModel.BlockThirdPartyCookies)

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
