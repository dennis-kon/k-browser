Public Class Phising

    Private Sub btnIgnore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIgnore.Click
        Me.Close()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Form1.wb.GoBack()
        Me.Close()
    End Sub

    Private Sub frmPhising_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        '
    End Sub
End Class