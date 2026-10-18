Imports System.Collections.Specialized
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' History is recorded on every page-title change. Recording must stay cheap, keep one entry per address,
''' stay within a size cap, and not touch the list at all when nothing changed.
''' </summary>
Public Class HistoryRecorderTests

    Private Shared ReadOnly T0 As New DateTime(2026, 10, 18, 12, 0, 0)

    Private Shared Function Entries(ParamArray items As String()) As StringCollection
        Dim c As New StringCollection()
        c.AddRange(items)
        Return c
    End Function

    <Fact>
    Public Sub NewPage_IsAddedAtTheEnd_AsTitleUrlTime()
        Dim h = Entries()
        Assert.True(HistoryRecorder.Record(h, "Example", "https://example.com/", T0))
        Assert.Equal(1, h.Count)
        Assert.Equal("Example|https://example.com/|" & T0.ToString("o"), h(0))
    End Sub

    <Fact>
    Public Sub RevisitedPage_MovesToTheEnd_WithoutADuplicate()
        Dim h = Entries("A|https://a.com/|x", "B|https://b.com/|x", "C|https://c.com/|x")
        Assert.True(HistoryRecorder.Record(h, "A again", "https://a.com/", T0))
        Assert.Equal(3, h.Count)
        Assert.StartsWith("B|", h(0))
        Assert.StartsWith("A again|https://a.com/|", h(2))
    End Sub

    <Fact>
    Public Sub LegacyUrlOnlyEntry_IsReplaced()
        Dim h = Entries("https://a.com/", "B|https://b.com/|x")
        HistoryRecorder.Record(h, "A", "https://a.com/", T0)
        Assert.Equal(2, h.Count)
        Assert.DoesNotContain("https://a.com/", h.Cast(Of String)())
    End Sub

    <Fact>
    Public Sub SameTitleForTheNewestEntry_ChangesNothing()
        Dim h = Entries("B|https://b.com/|x", "A|https://a.com/|old")
        Assert.False(HistoryRecorder.Record(h, "A", "https://a.com/", T0))
        Assert.Equal("A|https://a.com/|old", h(1))
    End Sub

    <Fact>
    Public Sub TitleWithABar_StillKeepsTheUrlReadable()
        Dim h = Entries()
        HistoryRecorder.Record(h, "News | Site", "https://news.com/", T0)
        Assert.Equal("https://news.com/", HistoryRecorder.UrlOf(h(0)))
    End Sub

    <Fact>
    Public Sub EmptyTitle_UsesTheUrl()
        Dim h = Entries()
        HistoryRecorder.Record(h, "", "https://a.com/", T0)
        Assert.StartsWith("https://a.com/|https://a.com/|", h(0))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData("about:blank")>
    <InlineData("ABOUT:BLANK")>
    <InlineData(Nothing)>
    Public Sub BlankAddresses_AreNotRecorded(url As String)
        Dim h = Entries()
        Assert.False(HistoryRecorder.Record(h, "x", url, T0))
        Assert.Equal(0, h.Count)
    End Sub

    <Fact>
    Public Sub OverTheCap_TheOldestEntriesAreDropped()
        Dim h = Entries("1|https://1.com/|x", "2|https://2.com/|x", "3|https://3.com/|x")
        HistoryRecorder.Record(h, "4", "https://4.com/", T0, maxEntries:=3)
        Assert.Equal(3, h.Count)
        Assert.StartsWith("2|", h(0))
        Assert.StartsWith("4|", h(2))
    End Sub

    <Fact>
    Public Sub UrlMatch_IgnoresCase()
        Dim h = Entries("A|HTTPS://A.COM/|x")
        HistoryRecorder.Record(h, "A2", "https://a.com/", T0)
        Assert.Equal(1, h.Count)
    End Sub

    <Fact>
    Public Sub DeferredSave_RunsOnceForManyRequests_OnlyWhenFlushed()
        Dim saves As Integer = 0
        Dim saver As New DeferredSave(Sub() saves += 1, 5000)
        saver.Request()
        saver.Request()
        saver.Request()
        Assert.Equal(0, saves)
        Assert.True(saver.IsPending)
        saver.Flush()
        Assert.Equal(1, saves)
        Assert.False(saver.IsPending)
        saver.Flush()
        Assert.Equal(1, saves)
    End Sub

End Class
