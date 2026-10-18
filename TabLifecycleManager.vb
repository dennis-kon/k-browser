Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Dedicated service managing tab lifecycle (sleeping background tabs, tab fading, memory saving).
''' Extensible architecture supporting Sleeping Tabs, Memory Saver, and Tab Hibernation.
''' </summary>
Public Class TabLifecycleManager

    Private Shared ReadOnly SuspendedTabs As New HashSet(Of WebView2)()
    Public Shared Property FadeInactiveTabsEnabled As Boolean = True

    ''' <summary>How long a tab stays in the background before Memory Saver puts it to sleep.</summary>
    Public Shared ReadOnly IdleBeforeSuspend As TimeSpan = TimeSpan.FromMinutes(5)

    ' When each background tab was last switched away from (active tabs are not in here).
    Private Shared ReadOnly BackgroundSince As New Dictionary(Of WebView2, DateTime)()

    ''' <summary>
    ''' Whether Memory Saver should put a tab to sleep now. Sleeping stops the page's timers, sockets and
    ''' notifications, so it only happens when switched on, after the tab has been in the background for
    ''' <see cref="IdleBeforeSuspend"/>, and never while the tab plays sound.
    ''' </summary>
    Public Shared Function ShouldSuspend(ByVal memorySaverEnabled As Boolean, ByVal backgroundSince As DateTime?, ByVal now As DateTime,
                                         ByVal playingAudio As Boolean, ByVal isSuspended As Boolean) As Boolean
        If Not memorySaverEnabled OrElse isSuspended OrElse playingAudio OrElse Not backgroundSince.HasValue Then Return False
        Return now - backgroundSince.Value >= IdleBeforeSuspend
    End Function

    ''' <summary>
    ''' Handles tab selection changes in Form1: resumes the active tab and notes when the others went to the
    ''' background. With Memory Saver on, background tabs also get a low memory target straight away (which
    ''' does not stop them); <see cref="SuspendIdleTabs"/> puts them to sleep later.
    ''' </summary>
    Public Shared Sub OnTabSelectionChanged(ByVal tabControl As TabControl, ByVal memorySaverEnabled As Boolean)
        If tabControl Is Nothing OrElse tabControl.SelectedTab Is Nothing Then
            Return
        End If

        Dim activeTab As TabPage = tabControl.SelectedTab
        For Each page As TabPage In tabControl.TabPages
            Dim brws As WebView2 = If(page.Controls.Count > 0, TryCast(page.Controls(0), WebView2), Nothing)
            If brws Is Nothing OrElse brws.CoreWebView2 Is Nothing Then Continue For

            If Object.ReferenceEquals(page, activeTab) Then
                ResumeTab(brws)
                BackgroundSince.Remove(brws)
                SetMemoryTarget(brws, CoreWebView2MemoryUsageTargetLevel.Normal)
            Else
                If Not BackgroundSince.ContainsKey(brws) Then BackgroundSince(brws) = DateTime.Now
                SetMemoryTarget(brws, If(memorySaverEnabled, CoreWebView2MemoryUsageTargetLevel.Low, CoreWebView2MemoryUsageTargetLevel.Normal))
            End If
        Next
    End Sub

    ''' <summary>Puts to sleep the background tabs that <see cref="ShouldSuspend"/> picks. Called from a timer.</summary>
    Public Shared Sub SuspendIdleTabs(ByVal tabControl As TabControl, ByVal memorySaverEnabled As Boolean)
        If tabControl Is Nothing OrElse Not memorySaverEnabled Then Return
        Dim now As DateTime = DateTime.Now
        For Each page As TabPage In tabControl.TabPages
            If Object.ReferenceEquals(page, tabControl.SelectedTab) OrElse page.Controls.Count = 0 Then Continue For
            Dim brws = TryCast(page.Controls(0), WebView2)
            If brws Is Nothing OrElse brws.CoreWebView2 Is Nothing Then Continue For

            Dim since As DateTime
            Dim idleSince As DateTime? = Nothing
            If BackgroundSince.TryGetValue(brws, since) Then idleSince = since
            Dim playing As Boolean = False
            Try
                playing = brws.CoreWebView2.IsDocumentPlayingAudio
            Catch
            End Try
            If ShouldSuspend(True, idleSince, now, playing, SuspendedTabs.Contains(brws)) Then SuspendTab(brws)
        Next
    End Sub

    Private Shared Sub SetMemoryTarget(ByVal brws As WebView2, ByVal level As CoreWebView2MemoryUsageTargetLevel)
        Try
            If brws.CoreWebView2.MemoryUsageTargetLevel <> level Then brws.CoreWebView2.MemoryUsageTargetLevel = level
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("MemoryUsageTargetLevel error: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Suspends background tab JavaScript timers and rendering loop to conserve RAM and CPU,
    ''' and applies visual fading to inactive tab headers when FadeInactiveTabs is enabled.
    ''' </summary>
    Public Shared Async Sub SuspendTab(ByVal brws As WebView2)
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Try
                If Not SuspendedTabs.Contains(brws) Then
                    Dim success As Boolean = Await brws.CoreWebView2.TrySuspendAsync()
                    If success Then
                        SuspendedTabs.Add(brws)
                        ApplyTabFadeState(brws, True)
                        System.Diagnostics.Debug.WriteLine("Tab suspended successfully to free memory.")
                    End If
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("TrySuspendAsync error: " & ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Resumes a suspended tab when activated by the user and restores normal tab header appearance.
    ''' </summary>
    Public Shared Sub ResumeTab(ByVal brws As WebView2)
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Try
                If SuspendedTabs.Contains(brws) Then
                    brws.CoreWebView2.Resume()
                    SuspendedTabs.Remove(brws)
                    ApplyTabFadeState(brws, False)
                    System.Diagnostics.Debug.WriteLine("Tab resumed from suspension.")
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Resume tab error: " & ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Applies visual fading state to a tab page header.
    ''' </summary>
    Private Shared Sub ApplyTabFadeState(ByVal brws As WebView2, ByVal isSuspended As Boolean)
        If brws IsNot Nothing AndAlso brws.Parent IsNot Nothing AndAlso TypeOf brws.Parent Is TabPage Then
            Dim page As TabPage = DirectCast(brws.Parent, TabPage)
            If FadeInactiveTabsEnabled AndAlso isSuspended Then
                If Not page.Text.StartsWith("💤 ") Then
                    page.Text = "💤 " & page.Text
                End If
            Else
                If page.Text.StartsWith("💤 ") Then
                    page.Text = page.Text.Substring(3)
                End If
            End If
        End If
    End Sub

    ''' <summary>
    ''' Cleans up tab reference from tracking collection on tab close.
    ''' </summary>
    Public Shared Sub OnTabClosed(ByVal brws As WebView2)
        If brws Is Nothing Then Return
        SuspendedTabs.Remove(brws)
        BackgroundSince.Remove(brws)
    End Sub

End Class
