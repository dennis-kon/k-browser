Public Class fb

    Private Sub fb_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        WebBrowser1.Navigate("http://www.facebook.com/sharer.php?u=")
    End Sub
End Class