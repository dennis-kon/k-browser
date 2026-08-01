Imports System.Collections.Generic
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

Public Class TabLifecycleManager

    Private Shared ReadOnly SuspendedTabs As New HashSet(Of WebView2)()

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
    ''' Suspends background tab JavaScript timers and rendering loop to conserve RAM and CPU.
    ''' </summary>
    Public Shared Async Sub SuspendTab(ByVal brws As WebView2)
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Try
                If Not SuspendedTabs.Contains(brws) Then
                    Dim success As Boolean = Await brws.CoreWebView2.TrySuspendAsync()
                    If success Then
                        SuspendedTabs.Add(brws)
                        System.Diagnostics.Debug.WriteLine("Tab suspended successfully to free memory.")
                    End If
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("TrySuspendAsync error: " & ex.Message)
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Resumes a suspended tab when activated by the user.
    ''' </summary>
    Public Shared Sub ResumeTab(ByVal brws As WebView2)
        If brws IsNot Nothing AndAlso brws.CoreWebView2 IsNot Nothing Then
            Try
                If SuspendedTabs.Contains(brws) Then
                    brws.CoreWebView2.Resume()
                    SuspendedTabs.Remove(brws)
                    System.Diagnostics.Debug.WriteLine("Tab resumed from suspension.")
                End If
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Resume tab error: " & ex.Message)
            End Try
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
