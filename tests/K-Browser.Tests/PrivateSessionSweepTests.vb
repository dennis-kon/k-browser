Imports System.IO
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' A private session's profile folder is deleted when its window closes cleanly. After a crash it used to
''' stay in %TEMP% (cookies, cache, history) until the OS got round to it; a sweep at startup removes
''' folders no running session owns, without touching a session that is still open.
''' </summary>
Public Class PrivateSessionSweepTests
    Implements IDisposable

    Private ReadOnly _root As String = Path.Combine(Path.GetTempPath(), "kbrowser-sweep-tests-" & Guid.NewGuid().ToString("N"))

    Public Sub New()
        Directory.CreateDirectory(_root)
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            Directory.Delete(_root, True)
        Catch
        End Try
    End Sub

    Private Function MakeSession(id As String, Optional withLockFile As Boolean = True) As String
        Dim dir As String = Path.Combine(_root, id)
        Directory.CreateDirectory(Path.Combine(dir, "EBWebView"))
        File.WriteAllText(Path.Combine(dir, "EBWebView", "Cookies"), "secret")
        If withLockFile Then File.WriteAllText(Path.Combine(_root, id & ".lock"), "")
        Return dir
    End Function

    <Fact>
    Public Sub Session_WhoseLockIsNotHeld_IsRemoved_WithItsLockFile()
        Dim dir = MakeSession("crashed")

        Dim removed = PrivateSessionService.SweepStaleSessions(_root)

        Assert.Equal(1, removed)
        Assert.False(Directory.Exists(dir))
        Assert.False(File.Exists(Path.Combine(_root, "crashed.lock")))
    End Sub

    <Fact>
    Public Sub Session_WhoseLockIsHeld_IsKept()
        Dim dir = MakeSession("open")
        Using held As New FileStream(Path.Combine(_root, "open.lock"), FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            Dim removed = PrivateSessionService.SweepStaleSessions(_root)

            Assert.Equal(0, removed)
            Assert.True(Directory.Exists(dir))
        End Using
    End Sub

    <Fact>
    Public Sub Folder_WithNoLockFile_IsKept_WhileRecent()
        ' Created by an older version that has no lock files and may still be running.
        Dim dir = MakeSession("legacy-recent", withLockFile:=False)

        Assert.Equal(0, PrivateSessionService.SweepStaleSessions(_root))
        Assert.True(Directory.Exists(dir))
    End Sub

    <Fact>
    Public Sub Folder_WithNoLockFile_IsRemoved_OnceItIsOld()
        Dim dir = MakeSession("legacy-old", withLockFile:=False)
        Directory.SetLastWriteTimeUtc(dir, DateTime.UtcNow.AddDays(-2))

        Assert.Equal(1, PrivateSessionService.SweepStaleSessions(_root))
        Assert.False(Directory.Exists(dir))
    End Sub

    <Fact>
    Public Sub OnlyTheStaleSessions_AreRemoved()
        Dim stale = MakeSession("stale")
        Dim live = MakeSession("live")
        Using held As New FileStream(Path.Combine(_root, "live.lock"), FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            Assert.Equal(1, PrivateSessionService.SweepStaleSessions(_root))
        End Using
        Assert.False(Directory.Exists(stale))
        Assert.True(Directory.Exists(live))
    End Sub

    <Fact>
    Public Sub MissingRoot_IsNotAnError()
        Assert.Equal(0, PrivateSessionService.SweepStaleSessions(Path.Combine(_root, "does-not-exist")))
    End Sub

    <Fact>
    Public Sub LeftoverLockFile_WithNoFolder_IsCleanedUp()
        File.WriteAllText(Path.Combine(_root, "orphan.lock"), "")
        PrivateSessionService.SweepStaleSessions(_root)
        Assert.False(File.Exists(Path.Combine(_root, "orphan.lock")))
    End Sub

    <Fact>
    Public Sub ASessionThatHoldsItsLock_IsNotSweptByAnotherInstance()
        ' The real class: create the lock the way a running session does, then sweep from "another" instance.
        Using svc As New PrivateSessionService(_root)
            svc.AcquireSessionLock()
            Directory.CreateDirectory(svc.SessionFolder)
            File.WriteAllText(Path.Combine(svc.SessionFolder, "Cookies"), "secret")

            Assert.Equal(0, PrivateSessionService.SweepStaleSessions(_root))
            Assert.True(Directory.Exists(svc.SessionFolder))
        End Using
    End Sub

End Class
