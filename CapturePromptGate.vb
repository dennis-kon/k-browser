''' <summary>
''' Limits how often a web page can bring up the "Save password?" prompt. The prompt is modal, so a
''' page that kept asking could otherwise lock the browser window.
'''
''' Rules: one prompt at a time; a short cooldown after each prompt closes; and a site the user
''' dismissed is muted for the rest of the session.
''' </summary>
Public Class CapturePromptGate

    Private ReadOnly _cooldown As TimeSpan
    Private ReadOnly _clock As Func(Of DateTime)
    Private ReadOnly _muted As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
    Private ReadOnly _lock As New Object()
    Private _open As Boolean
    Private _lastClosed As DateTime = DateTime.MinValue

    Public Sub New(ByVal cooldown As TimeSpan, Optional ByVal clock As Func(Of DateTime) = Nothing)
        _cooldown = cooldown
        _clock = If(clock, Function() DateTime.UtcNow)
    End Sub

    ''' <summary>
    ''' Claims the prompt for <paramref name="key"/> (the site). Returns False when another prompt is
    ''' open, the cooldown has not passed, or the site is muted. Pair every True with <see cref="Release"/>.
    ''' </summary>
    Public Function TryEnter(ByVal key As String) As Boolean
        SyncLock _lock
            If _open Then Return False
            If _muted.Contains(If(key, String.Empty)) Then Return False
            If _lastClosed <> DateTime.MinValue AndAlso _clock() - _lastClosed < _cooldown Then Return False
            _open = True
            Return True
        End SyncLock
    End Function

    ''' <summary>
    ''' Marks the prompt closed. With <paramref name="suppress"/> the site gets no further prompts this session.
    ''' </summary>
    Public Sub Release(ByVal key As String, ByVal suppress As Boolean)
        SyncLock _lock
            _open = False
            _lastClosed = _clock()
            If suppress Then _muted.Add(If(key, String.Empty))
        End SyncLock
    End Sub

End Class
