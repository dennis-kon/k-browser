Imports System.IO
Imports System.Threading.Tasks
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' Closing a private window must delete its profile folder. The browser process keeps files in that folder
''' open until it has shut down, so the folder can only go once the browser is gone. If it still cannot be
''' deleted, the next start-up sweep must be able to remove it.
''' </summary>
Public Class PrivateSessionEndTests
    Implements IDisposable

    Private ReadOnly _root As String = Path.Combine(Path.GetTempPath(), "kb-private-end-" & Guid.NewGuid().ToString("N"))

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            Directory.Delete(_root, True)
        Catch
        End Try
    End Sub

    ''' <summary>A session whose folder holds a file the "browser" keeps open, as the real browser process does.</summary>
    Private Function StartSession(ByRef browserFile As FileStream) As PrivateSessionService
        Dim svc As New PrivateSessionService(_root)
        svc.AcquireSessionLock()
        Directory.CreateDirectory(svc.SessionFolder)
        browserFile = New FileStream(Path.Combine(svc.SessionFolder, "Cookies"), FileMode.Create, FileAccess.ReadWrite, FileShare.None)
        Return svc
    End Function

    <Fact>
    Public Sub Folder_IsDeleted_AfterTheBrowserHasShutDown()
        Dim browserFile As FileStream = Nothing
        Using svc = StartSession(browserFile)
            Dim cleared As Boolean = False
            Dim deleted = svc.EndSessionAsync(
                Function()
                    cleared = True
                    Return Task.CompletedTask
                End Function,
                Function()
                    Assert.True(cleared, "browsing data must be cleared while the browser still runs")
                    browserFile.Dispose()
                    Return Task.CompletedTask
                End Function).GetAwaiter().GetResult()

            Assert.True(deleted)
            Assert.False(Directory.Exists(svc.SessionFolder))
            Assert.Empty(Directory.GetFiles(_root, "*.lock"))
        End Using
    End Sub

    <Fact>
    Public Sub FolderThatCannotBeDeleted_IsLeftForTheNextStartupSweep()
        Dim browserFile As FileStream = Nothing
        Using svc = StartSession(browserFile)
            Dim deleted = svc.EndSessionAsync(Function() Task.CompletedTask, Function() Task.CompletedTask).GetAwaiter().GetResult()
            Assert.False(deleted)

            browserFile.Dispose() ' the browser finally lets go
            Assert.Equal(1, PrivateSessionService.SweepStaleSessions(_root, TimeSpan.FromDays(365)))
            Assert.False(Directory.Exists(svc.SessionFolder))
        End Using
    End Sub

    <Fact>
    Public Sub FailingSteps_DoNotStopTheFolderBeingDeleted()
        Dim browserFile As FileStream = Nothing
        Using svc = StartSession(browserFile)
            Dim deleted = svc.EndSessionAsync(
                Function() Task.FromException(New InvalidOperationException("clear failed")),
                Function()
                    browserFile.Dispose()
                    Return Task.CompletedTask
                End Function).GetAwaiter().GetResult()
            Assert.True(deleted)
        End Using
    End Sub

End Class
