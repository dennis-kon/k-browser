Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' Memory Saver puts a background tab to sleep, which stops its timers, sockets and notifications. It must
''' only do that when switched on, after the tab has been in the background for a while, and never to a tab
''' that is playing sound.
''' </summary>
Public Class MemorySaverTests

    Private Shared ReadOnly Now As New DateTime(2026, 10, 18, 12, 0, 0)

    <Fact>
    Public Sub TabInTheBackgroundLongEnough_IsPutToSleep()
        Assert.True(TabLifecycleManager.ShouldSuspend(True, Now - TabLifecycleManager.IdleBeforeSuspend, Now, False, False))
    End Sub

    <Fact>
    Public Sub TabJustSwitchedAwayFrom_IsLeftRunning()
        Assert.False(TabLifecycleManager.ShouldSuspend(True, Now.AddSeconds(-30), Now, False, False))
    End Sub

    <Fact>
    Public Sub MemorySaverOff_NothingSleeps()
        Assert.False(TabLifecycleManager.ShouldSuspend(False, Now.AddHours(-3), Now, False, False))
    End Sub

    <Fact>
    Public Sub TabPlayingSound_KeepsRunning()
        Assert.False(TabLifecycleManager.ShouldSuspend(True, Now.AddHours(-3), Now, True, False))
    End Sub

    <Fact>
    Public Sub ActiveTab_NeverSleeps()
        Assert.False(TabLifecycleManager.ShouldSuspend(True, Nothing, Now, False, False))
    End Sub

    <Fact>
    Public Sub AlreadySleepingTab_IsNotSuspendedAgain()
        Assert.False(TabLifecycleManager.ShouldSuspend(True, Now.AddHours(-3), Now, False, True))
    End Sub

    <Fact>
    Public Sub WaitIsSomeMinutes_NotInstant()
        Assert.True(TabLifecycleManager.IdleBeforeSuspend >= TimeSpan.FromMinutes(1))
    End Sub

End Class
