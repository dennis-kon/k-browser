Imports System
Imports System.Collections.Generic

''' <summary>
''' Data transfer object representing application and privacy preferences persisted in Settings.json.
''' </summary>
Public Class PrivacySettingsModel
    ''' <summary>
    ''' Indicates whether third-party cookies should be blocked by WebView2.
    ''' </summary>
    Public Property BlockThirdPartyCookies As Boolean = False

    ''' <summary>
    ''' Stores user customized column widths for the Cookie Manager DataGridView.
    ''' </summary>
    Public Property ColumnWidths As Dictionary(Of String, Integer) = New Dictionary(Of String, Integer)()

    ''' <summary>
    ''' Configures startup mode: "RestoreSession" or "NewTab". Default is "NewTab".
    ''' </summary>
    Public Property StartupMode As String = "NewTab"

    ''' <summary>
    ''' Timestamp of the last successful backup. Formatted as ISO-8601 string.
    ''' </summary>
    Public Property LastBackup As Nullable(Of DateTime) = Nothing

    ''' <summary>
    ''' Performance configuration section (Graphics, Tabs, Page Loading).
    ''' </summary>
    Public Property Performance As PerformanceSettingsModel = New PerformanceSettingsModel()
End Class

''' <summary>
''' Alias type for application settings model.
''' </summary>
Public Class AppSettingsModel
    Inherits PrivacySettingsModel
End Class
