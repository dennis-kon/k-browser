﻿Public Class Form4

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        OpenFileDialog1.ShowDialog()
        AxWindowsMediaPlayer1.URL = OpenFileDialog1.FileName
    End Sub
End Class