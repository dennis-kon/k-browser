Public Class tw
    Private Sub tw_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WebBrowser1.Navigate("http://twitter.com/share?=")
    End Sub
End Class