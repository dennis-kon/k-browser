Imports System

''' <summary>
''' Provides a centralized event manager to notify forms and background services when browser settings are modified.
''' </summary>
Public Class SettingsManager

    ''' <summary>
    ''' Fired whenever application settings are updated and saved via the Settings dialog.
    ''' </summary>
    Public Shared Event SettingsChanged As EventHandler

    ''' <summary>
    ''' Raises the SettingsChanged event to notify active listeners.
    ''' </summary>
    Public Shared Sub NotifySettingsChanged()
        RaiseEvent SettingsChanged(Nothing, EventArgs.Empty)
    End Sub

End Class
