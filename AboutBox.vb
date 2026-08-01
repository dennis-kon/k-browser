Public Class AboutBox

    Private Sub AboutBox_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        RichTextBox1.Text = "K-Browser is a modern, lightweight web browser for Microsoft Windows powered by the high-performance Microsoft Edge WebView2 (Chromium) engine. It is designed for intuitive navigation, enhanced privacy, and fast, secure browsing." & vbCrLf & vbCrLf &
                            "Key Features:" & vbCrLf &
                            "• Microsoft Edge WebView2 (Chromium) Core" & vbCrLf &
                            "• Built-in EasyList Ad Blocker & Badge Counter" & vbCrLf &
                            "• HTTPS-Only Mode & Security Policies" & vbCrLf &
                            "• Built-in Download Manager & FTP Client" & vbCrLf &
                            "• Hierarchical Bookmarks & Folder Management" & vbCrLf &
                            "• Anti-Phishing Protection & Site Controls" & vbCrLf &
                            "• Custom Zoom UI & Multi-Tab Navigation"
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        gp.ShowDialog()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form1.NavigateActiveTab("https://k-browser.com/")
    End Sub

End Class
