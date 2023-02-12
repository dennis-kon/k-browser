Imports System.Windows.Forms
Imports System.Net
Imports System.IO
Imports System.Xml
Imports System.Drawing
Imports System.Text
Imports Microsoft.Win32
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data


Public Class AboutBox

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Close()
    End Sub

    Private Sub LinkLabel2_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        gp.ShowDialog()
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Form1.wb.Navigate("https://www.k-browser.com")
    End Sub

    Private Sub Label2_Click(sender As Object, e As EventArgs) Handles Label2.Click

    End Sub
End Class
 