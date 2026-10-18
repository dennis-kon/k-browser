Imports System.Collections.Specialized

''' <summary>
''' Records visited pages in My.Settings.History as "Title|URL|DateTime" entries (legacy entries may be
''' "Title|URL" or a bare URL). This runs on every page-title change, so it keeps one entry per address,
''' leaves the list untouched when nothing changed, and caps its size.
''' </summary>
Public Class HistoryRecorder

    Public Const DefaultMaxEntries As Integer = 5000

    ''' <summary>
    ''' Records a visit. Returns True when the list changed (and so needs saving).
    ''' </summary>
    Public Shared Function Record(ByVal history As StringCollection, ByVal title As String, ByVal url As String,
                                  ByVal timestamp As DateTime, Optional ByVal maxEntries As Integer = DefaultMaxEntries) As Boolean
        If history Is Nothing OrElse String.IsNullOrWhiteSpace(url) Then Return False
        If url.Equals("about:blank", StringComparison.OrdinalIgnoreCase) Then Return False

        ' A "|" in the title would shift the URL into the wrong field.
        Dim displayTitle As String = If(String.IsNullOrWhiteSpace(title), url, title).Replace("|"c, "¦"c)

        ' Recent pages are near the end, so search backwards.
        Dim existingIndex As Integer = -1
        For i As Integer = history.Count - 1 To 0 Step -1
            If String.Equals(UrlOf(history(i)), url, StringComparison.OrdinalIgnoreCase) Then
                existingIndex = i
                Exit For
            End If
        Next

        If existingIndex = history.Count - 1 AndAlso existingIndex >= 0 AndAlso
           String.Equals(TitleOf(history(existingIndex)), displayTitle, StringComparison.Ordinal) Then
            Return False
        End If

        If existingIndex >= 0 Then history.RemoveAt(existingIndex)
        history.Add(displayTitle & "|" & url & "|" & timestamp.ToString("o"))

        Dim excess As Integer = history.Count - Math.Max(1, maxEntries)
        For i As Integer = 1 To excess
            history.RemoveAt(0)
        Next
        Return True
    End Function

    ''' <summary>The URL part of an entry, or the entry itself for a legacy URL-only entry.</summary>
    Public Shared Function UrlOf(ByVal entry As String) As String
        If entry Is Nothing Then Return Nothing
        Dim first As Integer = entry.IndexOf("|"c)
        If first < 0 Then Return entry
        Dim second As Integer = entry.IndexOf("|"c, first + 1)
        Return If(second < 0, entry.Substring(first + 1), entry.Substring(first + 1, second - first - 1))
    End Function

    Private Shared Function TitleOf(ByVal entry As String) As String
        Dim first As Integer = entry.IndexOf("|"c)
        Return If(first < 0, entry, entry.Substring(0, first))
    End Function

End Class

''' <summary>
''' Batches saves: the first Request starts a timer and one save happens when it fires, however many requests
''' arrive meanwhile. Flush saves a pending change immediately (for example when the window closes).
''' Request must be called on the UI thread, where the timer ticks.
''' </summary>
Public Class DeferredSave

    Private ReadOnly _save As Action
    Private ReadOnly _delayMs As Integer
    Private _timer As System.Windows.Forms.Timer

    Public Sub New(ByVal save As Action, ByVal delayMs As Integer)
        _save = save
        _delayMs = Math.Max(1, delayMs)
    End Sub

    Public ReadOnly Property IsPending As Boolean

    Public Sub Request()
        If _IsPending Then Return
        _IsPending = True
        If _timer Is Nothing Then
            _timer = New System.Windows.Forms.Timer() With {.Interval = _delayMs}
            AddHandler _timer.Tick, Sub() Flush()
        End If
        _timer.Start()
    End Sub

    Public Sub Flush()
        _timer?.Stop()
        If Not _IsPending Then Return
        _IsPending = False
        _save()
    End Sub

End Class
