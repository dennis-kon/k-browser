Imports System

''' <summary>
''' Strongly-typed model representing an individual browser tab within a saved session.
''' </summary>
Public Class TabItemModel
    ''' <summary>
    ''' Target URL of the tab.
    ''' </summary>
    Public Property Url As String = String.Empty

    ''' <summary>
    ''' Indicates whether the tab is pinned.
    ''' </summary>
    Public Property Pinned As Boolean = False
End Class
