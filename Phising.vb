Public Class Phising

    Private Sub btnIgnore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIgnore.Click
        Me.DialogResult = Windows.Forms.DialogResult.Ignore
        Me.Close()
    End Sub

    Private Sub btnBack_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBack.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub frmPhising_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ThemeManager.ApplyTheme(Me)
    End Sub
End Class