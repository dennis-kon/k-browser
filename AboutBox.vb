Public Class AboutBox

    Private Sub AboutBox_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ThemeManager.ApplyTheme(Me)
        RichTextBox1.Text = "K-Browser is a lightweight, secure, multi-tabbed web browser and desktop productivity platform built for Windows. It couples a modern rendering engine (Microsoft WebView2 Chromium with fallback IE emulation registry control) with embedded privacy and utility tools, including a custom Trie-based AdBlocker Engine, UserScript Injection Host, Smart Tab Lifecycle Manager, Multi-Process Architecture Manager, Dark Mode & Modern Styling Engine, Modular Services Infrastructure, JSON Session State Manager, Compressed Backup & Restore Engine, Advanced Cookie Manager, Phishing Security Guard, RSS Feed Reader, FTP Client, Task Manager, and TCP Port Scanner. K-Browser is designed to be a fast, secure, and privacy-focused web browser that provides users with a seamless browsing experience while also offering advanced features for power users and developers."
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
