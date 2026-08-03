Imports System

''' <summary>
''' Strongly-typed domain model representing performance settings stored in Settings.json.
''' </summary>
Public Class PerformanceSettingsModel
    ''' <summary>
    ''' Enables GPU hardware acceleration for rendering when supported by hardware.
    ''' </summary>
    Public Property UseHardwareAcceleration As Boolean = True

    ''' <summary>
    ''' Visually dims or fades inactive/sleeping tab headers to reduce visual clutter and reflect resource saving.
    ''' </summary>
    Public Property FadeInactiveTabs As Boolean = True

    ''' <summary>
    ''' Enables predictive page preloading and DNS prefetching to improve page load speed.
    ''' </summary>
    Public Property PreloadPages As Boolean = True
End Class
