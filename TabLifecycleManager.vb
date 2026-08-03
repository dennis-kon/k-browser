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

    ''' <summary>
    ''' Handles tab selection change events in Form1 to resume the active tab and suspend idle background tabs.
    ''' </summary>
    Public Shared Sub OnTabSelectionChanged(ByVal tabControl As TabControl)
        If tabControl Is Nothing OrElse tabControl.SelectedTab Is Nothing Then
            Return
        End If

        ' 1. Resume active tab if suspended
        Dim activeTab As TabPage = tabControl.SelectedTab
        If activeTab.Controls.Count > 0 Then
            Dim activeBrws = TryCast(activeTab.Controls(0), WebView2)
            If activeBrws IsNot Nothing AndAlso activeBrws.CoreWebView2 IsNot Nothing Then
                ResumeTab(activeBrws)
            End If
        End If

        ' 2. Suspend background tabs
        For Each page As TabPage In tabControl.TabPages
            If Not Object.ReferenceEquals(page, activeTab) AndAlso page.Controls.Count > 0 Then
                Dim bgBrws = TryCast(page.Controls(0), WebView2)
                If bgBrws IsNot Nothing AndAlso bgBrws.CoreWebView2 IsNot Nothing Then
                    SuspendTab(bgBrws)
                End If
            End If
        Next
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
        If brws IsNot Nothing AndAlso SuspendedTabs.Contains(brws) Then
            SuspendedTabs.Remove(brws)
        End If
    End Sub

End Class
