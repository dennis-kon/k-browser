Imports System.Runtime.CompilerServices
Imports Microsoft.Web.WebView2.Core

''' <summary>
''' Registers the ad blocker's request filter on a tab. Every request a filter covers is held until the
''' WebResourceRequested handler has run on the window's UI thread, so the filter is only attached while
''' the ad blocker is on, and it leaves out top-level pages, which the blocker always lets through.
''' </summary>
Public Class AdBlockRequestFilter

    ''' <summary>The kinds of request the ad blocker checks.</summary>
    Public Shared ReadOnly Property Contexts As IReadOnlyList(Of CoreWebView2WebResourceContext) = New CoreWebView2WebResourceContext() {
        CoreWebView2WebResourceContext.Stylesheet,
        CoreWebView2WebResourceContext.Image,
        CoreWebView2WebResourceContext.Media,
        CoreWebView2WebResourceContext.Font,
        CoreWebView2WebResourceContext.Script,
        CoreWebView2WebResourceContext.XmlHttpRequest,
        CoreWebView2WebResourceContext.Fetch,
        CoreWebView2WebResourceContext.TextTrack,
        CoreWebView2WebResourceContext.EventSource,
        CoreWebView2WebResourceContext.Websocket,
        CoreWebView2WebResourceContext.Manifest,
        CoreWebView2WebResourceContext.SignedExchange,
        CoreWebView2WebResourceContext.Ping,
        CoreWebView2WebResourceContext.CspViolationReport,
        CoreWebView2WebResourceContext.Other
    }

    Private Class Marker
    End Class

    ' Which tabs currently have the filters attached (entries go away with the tab).
    Private Shared ReadOnly Attached As New ConditionalWeakTable(Of CoreWebView2, Marker)()

    ''' <summary>Attaches or removes the filters on a tab to match whether the ad blocker is on.</summary>
    Public Shared Sub Apply(ByVal core As CoreWebView2, ByVal enabled As Boolean)
        If core Is Nothing Then Return
        Dim marker As Marker = Nothing
        Dim isAttached As Boolean = Attached.TryGetValue(core, marker)
        If enabled = isAttached Then Return

        Try
            For Each context In Contexts
                If enabled Then
                    core.AddWebResourceRequestedFilter("*", context)
                Else
                    core.RemoveWebResourceRequestedFilter("*", context)
                End If
            Next
            If enabled Then
                Attached.Add(core, New Marker())
            Else
                Attached.Remove(core)
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("AdBlockRequestFilter.Apply failed: " & ex.Message)
        End Try
    End Sub

End Class
