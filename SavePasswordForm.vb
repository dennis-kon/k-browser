''' <summary>
''' Modal dialog shown after a login form submission is detected.
''' Offers the user three choices: Save, Never (for this site), or Not Now.
''' 
''' Usage:
'''   Using dlg As New SavePasswordForm("github.com", "user@example.com")
'''       dlg.ShowDialog(parentForm)
'''       Select Case dlg.UserChoice
'''           Case SavePasswordChoice.Save : ' Save the credential
'''           Case SavePasswordChoice.Never : ' Block this site
'''           Case SavePasswordChoice.NotNow : ' Dismiss, ask again next time
'''       End Select
'''   End Using
''' </summary>
Public Class SavePasswordForm

    ''' <summary>
    ''' The user's choice after the dialog closes.
    ''' </summary>
    Public Property UserChoice As SavePasswordChoice = SavePasswordChoice.NotNow

    ''' <summary>
    ''' Creates the Save Password dialog for a specific domain and username.
    ''' </summary>
    ''' <param name="domain">The website domain (e.g., "github.com").</param>
    ''' <param name="username">The detected username or email.</param>
    Public Sub New(ByVal domain As String, ByVal username As String)
        InitializeComponent()
        lblDomain.Text = domain
        lblUsername.Text = username

        Try
            If Application.OpenForms.Count > 0 AndAlso Application.OpenForms(0).Icon IsNot Nothing Then
                Me.Icon = Application.OpenForms(0).Icon
            End If
        Catch
            ' Silently fail if icon retrieval fails
        End Try
    End Sub

    Private Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnSave.Click
        UserChoice = SavePasswordChoice.Save
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnNever_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNever.Click
        UserChoice = SavePasswordChoice.Never
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

    Private Sub btnNotNow_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnNotNow.Click
        UserChoice = SavePasswordChoice.NotNow
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class

''' <summary>
''' Represents the user's choice in the Save Password dialog.
''' </summary>
Public Enum SavePasswordChoice
    ''' <summary>Save the credential securely.</summary>
    Save = 0
    ''' <summary>Never ask to save passwords for this site.</summary>
    Never = 1
    ''' <summary>Dismiss now, ask again next time.</summary>
    NotNow = 2
End Enum
