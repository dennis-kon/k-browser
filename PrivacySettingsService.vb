Imports System
Imports System.Collections.Generic
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Application service coordinating privacy settings persistence and configuring WebView2 cookie & tracking policies.
''' </summary>
Public Class PrivacySettingsService
    Private ReadOnly _settingsService As SettingsService

    Public Sub New()
        _settingsService = New SettingsService()
    End Sub

    Public Sub New(ByVal settingsService As SettingsService)
        _settingsService = If(settingsService, New SettingsService())
    End Sub

    ''' <summary>
    ''' Loads privacy preferences from Settings.json.
    ''' </summary>
    Public Async Function LoadPrivacySettingsAsync() As Task(Of PrivacySettingsModel)
        Return Await _settingsService.LoadSettingsAsync()
    End Function

    ''' <summary>
    ''' Saves updated privacy preferences to Settings.json.
    ''' </summary>
    Public Async Function SavePrivacySettingsAsync(ByVal model As PrivacySettingsModel) As Task
        Await _settingsService.SaveSettingsAsync(model)
    End Function

    ''' <summary>
    ''' Appends or updates `--block-third-party-cookies` in CoreWebView2EnvironmentOptions.AdditionalBrowserArguments.
    ''' Used when initializing CoreWebView2Environment instances in TabProcessManager.
    ''' </summary>
    Public Sub ConfigureEnvironmentOptions(ByVal options As CoreWebView2EnvironmentOptions, ByVal blockThirdParty As Boolean)
        If options Is Nothing Then Return

        Dim currentArgs As String = If(options.AdditionalBrowserArguments, "")
        Const BlockFlag As String = "--block-third-party-cookies"

        If blockThirdParty Then
            If Not currentArgs.Contains(BlockFlag) Then
                options.AdditionalBrowserArguments = (currentArgs & " " & BlockFlag).Trim()
            End If
        Else
            If currentArgs.Contains(BlockFlag) Then
                options.AdditionalBrowserArguments = currentArgs.Replace(BlockFlag, "").Trim()
            End If
        End If
    End Sub

    ''' <summary>
    ''' Applies third-party cookie blocking and tracking prevention policies to an active WebView2 instance.
    ''' </summary>
    ''' <param name="webView">Target WebView2 control</param>
    ''' <param name="blockThirdParty">True to reject third-party cookies</param>
    Public Sub ApplyThirdPartyCookieBlocking(ByVal webView As WebView2, ByVal blockThirdParty As Boolean)
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then Return

        Try
            ' Profile.PreferredTrackingPreventionLevel allows setting Chromium's tracking prevention level.
            ' When third-party blocking is enabled, setting Strict tracking prevention ensures high cookie isolation.
            If webView.CoreWebView2.Profile IsNot Nothing Then
                webView.CoreWebView2.Profile.PreferredTrackingPreventionLevel = If(blockThirdParty,
                    CoreWebView2TrackingPreventionLevel.Strict,
                    CoreWebView2TrackingPreventionLevel.Balanced)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PrivacySettingsService: Error applying tracking prevention: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Applies privacy settings to all provided active WebView2 controls.
    ''' </summary>
    Public Sub ApplySettingsToActiveWebViews(ByVal webViews As IEnumerable(Of WebView2), ByVal settings As PrivacySettingsModel)
        If webViews Is Nothing OrElse settings Is Nothing Then Return

        For Each brws In webViews
            ApplyThirdPartyCookieBlocking(brws, settings.BlockThirdPartyCookies)
        Next
    End Sub
End Class
