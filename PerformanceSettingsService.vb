Imports System
Imports System.IO
Imports System.Threading.Tasks

''' <summary>
''' Dedicated service managing browser performance settings (Graphics, Tabs, Page Loading).
''' Responsibilities include loading, saving, and applying performance configurations.
''' Follows SOLID principles and Clean Architecture.
''' </summary>
Public Class PerformanceSettingsService
    Private ReadOnly _settingsService As SettingsService

    Public Sub New()
        _settingsService = New SettingsService()
    End Sub

    Public Sub New(ByVal settingsService As SettingsService)
        _settingsService = If(settingsService, New SettingsService())
    End Sub

    ''' <summary>
    ''' Asynchronously loads performance settings from Settings.json.
    ''' </summary>
    Public Async Function LoadPerformanceSettingsAsync() As Task(Of PerformanceSettingsModel)
        Try
            Dim settings As PrivacySettingsModel = Await _settingsService.LoadSettingsAsync()
            If settings IsNot Nothing AndAlso settings.Performance IsNot Nothing Then
                Return settings.Performance
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PerformanceSettingsService: Error loading performance settings: " & ex.Message)
        End Try
        Return New PerformanceSettingsModel()
    End Function

    ''' <summary>
    ''' Synchronously loads performance settings from Settings.json. Safe for UI event handlers.
    ''' </summary>
    Public Function LoadPerformanceSettings() As PerformanceSettingsModel
        Try
            Dim settings As PrivacySettingsModel = _settingsService.LoadSettings()
            If settings IsNot Nothing AndAlso settings.Performance IsNot Nothing Then
                Return settings.Performance
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PerformanceSettingsService: Error loading performance settings synchronously: " & ex.Message)
        End Try
        Return New PerformanceSettingsModel()
    End Function

    ''' <summary>
    ''' Asynchronously saves performance settings to Settings.json.
    ''' </summary>
    Public Async Function SavePerformanceSettingsAsync(ByVal performanceModel As PerformanceSettingsModel) As Task
        If performanceModel Is Nothing Then Throw New ArgumentNullException(NameOf(performanceModel))

        Try
            Dim settings As PrivacySettingsModel = Await _settingsService.LoadSettingsAsync()
            If settings Is Nothing Then settings = New PrivacySettingsModel()
            settings.Performance = performanceModel
            Await _settingsService.SaveSettingsAsync(settings)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PerformanceSettingsService: Error saving performance settings: " & ex.Message)
            Throw New IOException("Failed to save performance settings: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Synchronously saves performance settings to Settings.json. Safe for UI event handlers.
    ''' </summary>
    Public Sub SavePerformanceSettings(ByVal performanceModel As PerformanceSettingsModel)
        If performanceModel Is Nothing Then Return

        Try
            Dim settings As PrivacySettingsModel = _settingsService.LoadSettings()
            If settings Is Nothing Then settings = New PrivacySettingsModel()
            settings.Performance = performanceModel
            _settingsService.SaveSettings(settings)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("PerformanceSettingsService: Error saving performance settings synchronously: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Checks if changes between existing model and new model require an application restart.
    ''' </summary>
    Public Function RequiresRestart(ByVal currentModel As PerformanceSettingsModel, ByVal newModel As PerformanceSettingsModel) As Boolean
        If currentModel Is Nothing OrElse newModel Is Nothing Then Return False
        Return currentModel.UseHardwareAcceleration <> newModel.UseHardwareAcceleration
    End Function
End Class
