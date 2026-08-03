Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Infrastructure service facilitating interop with WebView2 CoreWebView2CookieManager APIs.
''' Encapsulates retrieval, deletion, and searching of browser cookies cleanly.
''' </summary>
Public Class CookieService

    ''' <summary>
    ''' Asynchronously retrieves all stored cookies from the specified WebView2 control instance.
    ''' Uses CoreWebView2.CookieManager.GetCookiesAsync(Nothing) to query all profile cookies.
    ''' </summary>
    ''' <param name="webView">Active WebView2 instance</param>
    ''' <returns>List of strongly-typed CookieItem objects</returns>
    Public Async Function GetCookiesAsync(ByVal webView As WebView2) As Task(Of List(Of CookieItem))
        Dim result As New List(Of CookieItem)()

        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then
            Throw New InvalidOperationException("WebView2 engine is not initialized. Please open a web page first to load cookies.")
        End If

        Try
            ' CoreWebView2.CookieManager provides access to WebView2's cookie store.
            ' Passing Nothing (or empty string) to GetCookiesAsync fetches all cookies regardless of domain.
            Dim cookieManager As CoreWebView2CookieManager = webView.CoreWebView2.CookieManager
            Dim coreCookies As IReadOnlyList(Of CoreWebView2Cookie) = Await cookieManager.GetCookiesAsync(Nothing)

            If coreCookies IsNot Nothing Then
                For Each c As CoreWebView2Cookie In coreCookies
                    Dim item As New CookieItem With {
                        .Name = c.Name,
                        .Domain = c.Domain,
                        .Path = c.Path,
                        .IsSecure = c.IsSecure,
                        .IsHttpOnly = c.IsHttpOnly,
                        .SameSite = c.SameSite.ToString(),
                        .Website = If(c.Domain.StartsWith("."), c.Domain.Substring(1), c.Domain)
                    }

                    ' In WebView2 SDK, CoreWebView2Cookie.Expires returns a System.DateTime (Date)
                    If Not c.IsSession AndAlso c.Expires <> Date.MinValue Then
                        item.Expires = c.Expires
                    Else
                        item.Expires = Nothing
                    End If

                    result.Add(item)
                Next
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CookieService: GetCookiesAsync failed: " & ex.Message)
            Throw New Exception("Failed to retrieve cookies from WebView2: " & ex.Message, ex)
        End Try

        Return result
    End Function

    ''' <summary>
    ''' Asynchronously deletes a single specific cookie matching the given domain, name, and path.
    ''' Uses CoreWebView2CookieManager.DeleteCookie(coreCookie).
    ''' </summary>
    Public Async Function DeleteCookieAsync(ByVal webView As WebView2, ByVal cookieItem As CookieItem) As Task(Of Boolean)
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then
            Throw New InvalidOperationException("WebView2 engine is not initialized.")
        End If
        If cookieItem Is Nothing Then Return False

        Try
            Dim cookieManager As CoreWebView2CookieManager = webView.CoreWebView2.CookieManager
            Dim coreCookies As IReadOnlyList(Of CoreWebView2Cookie) = Await cookieManager.GetCookiesAsync(Nothing)

            If coreCookies IsNot Nothing Then
                For Each c As CoreWebView2Cookie In coreCookies
                    If String.Equals(c.Name, cookieItem.Name, StringComparison.OrdinalIgnoreCase) AndAlso
                       String.Equals(c.Domain, cookieItem.Domain, StringComparison.OrdinalIgnoreCase) AndAlso
                       String.Equals(c.Path, cookieItem.Path, StringComparison.OrdinalIgnoreCase) Then
                        ' DeleteCookie removes the matching cookie from the browser's persistent store.
                        cookieManager.DeleteCookie(c)
                        Return True
                    End If
                Next
            End If

            Return False
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CookieService: DeleteCookieAsync failed: " & ex.Message)
            Throw New Exception("Failed to delete cookie '" & cookieItem.Name & "': " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Asynchronously deletes all stored cookies from WebView2 profile context.
    ''' Uses CoreWebView2CookieManager.DeleteAllCookies().
    ''' </summary>
    Public Sub DeleteAllCookies(ByVal webView As WebView2)
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then
            Throw New InvalidOperationException("WebView2 engine is not initialized.")
        End If

        Try
            ' DeleteAllCookies clears every cookie stored in the profile partition.
            webView.CoreWebView2.CookieManager.DeleteAllCookies()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CookieService: DeleteAllCookies failed: " & ex.Message)
            Throw New Exception("Failed to clear all stored cookies: " & ex.Message, ex)
        End Try
    End Sub

    ''' <summary>
    ''' Filters a collection of CookieItems according to a user search query.
    ''' </summary>
    Public Function FilterCookies(ByVal cookies As IEnumerable(Of CookieItem), ByVal searchQuery As String) As List(Of CookieItem)
        If cookies Is Nothing Then Return New List(Of CookieItem)()
        If String.IsNullOrWhiteSpace(searchQuery) Then Return cookies.ToList()

        Return cookies.Where(Function(c) c.MatchesSearch(searchQuery)).ToList()
    End Function

End Class
