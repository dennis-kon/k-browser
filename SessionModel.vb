Imports System
Imports System.Collections.Generic

''' <summary>
''' Strongly-typed model representing the complete browser session state stored in Session.json.
''' </summary>
Public Class SessionModel
    ''' <summary>
    ''' List of open tabs in order.
    ''' </summary>
    Public Property Tabs As List(Of TabItemModel) = New List(Of TabItemModel)()

    ''' <summary>
    ''' Index of the active/selected tab.
    ''' </summary>
    Public Property SelectedTab As Integer = 0
End Class
