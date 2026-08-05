''' <summary>
''' Password Details dialog opened when double-clicking a row in the Passwords grid.
''' Shows website, username, masked password, dates, and provides show/hide/delete actions.
''' 
''' The show password button requires Windows authentication (via CredUI) when
''' RequireWindowsAuthentication is enabled in settings.
''' </summary>
Public Class PasswordDetailForm

    Private ReadOnly _passwordManager As PasswordManagerService
    Private ReadOnly _authService As WindowsAuthenticationService
    Private ReadOnly _settingsService As SettingsService
    Private ReadOnly _encryptedPassword As String
    Private _isPasswordVisible As Boolean = False

    ''' <summary>
    ''' Set to True if the user deleted this credential.
    ''' </summary>
    Public Property WasDeleted As Boolean = False

    ''' <summary>
    ''' Creates the Password Detail dialog for a specific credential entry.
    ''' </summary>
    ''' <param name="credential">The credential entry (with encrypted password).</param>
    Public Sub New(ByVal credential As CredentialEntry)
        InitializeComponent()

        _passwordManager = New PasswordManagerService()
        _authService = New WindowsAuthenticationService()
        _settingsService = New SettingsService()
        _encryptedPassword = credential.Password

        ' Populate display fields
        lblWebsiteValue.Text = credential.Website
        lblUsernameValue.Text = credential.Username
        txtPasswordValue.Text = "••••••••••••"
        txtPasswordValue.UseSystemPasswordChar = False  ' We manually control masking
        lblCreatedValue.Text = credential.CreatedDate.ToString("dd MMM yyyy HH:mm")
        lblLastUsedValue.Text = credential.LastUsed.ToString("dd MMM yyyy HH:mm")

        Try
            If Application.OpenForms.Count > 0 AndAlso Application.OpenForms(0).Icon IsNot Nothing Then
                Me.Icon = Application.OpenForms(0).Icon
            End If
        Catch
            ' Silently fail if icon retrieval fails
        End Try
    End Sub

    Private Sub btnTogglePassword_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnTogglePassword.Click
        If _isPasswordVisible Then
            ' Hide the password
            HidePassword()
        Else
            ' Show the password — may require Windows authentication
            ShowPassword()
        End If
    End Sub

    Private Sub ShowPassword()
        Try
            ' Check if Windows authentication is required
            Dim settings = _settingsService.LoadSettings()
            Dim requireAuth As Boolean = True
            If settings?.Passwords IsNot Nothing Then
                requireAuth = settings.Passwords.RequireWindowsAuthentication
            End If

            If requireAuth Then
                Dim authenticated As Boolean = _authService.Authenticate(Me.Handle)
                If Not authenticated Then
                    MessageBox.Show("Authentication failed. Cannot reveal password.",
                                    "Authentication Required",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    Return
                End If
            End If

            ' Decrypt and display the password
            Dim plainPassword As String = _passwordManager.DecryptPassword(_encryptedPassword)
            txtPasswordValue.Text = plainPassword
            btnTogglePassword.Text = "🙈 Hide Password"
            _isPasswordVisible = True
        Catch ex As Exception
            MessageBox.Show("Failed to reveal password: " & ex.Message,
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub HidePassword()
        txtPasswordValue.Text = "••••••••••••"
        btnTogglePassword.Text = "👁 Show Password"
        _isPasswordVisible = False
    End Sub

    Private Async Sub btnDelete_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnDelete.Click
        Dim confirmResult As DialogResult = MessageBox.Show(
            "Delete saved credentials for " & lblWebsiteValue.Text & "?" & vbCrLf & vbCrLf &
            "This action cannot be undone.",
            "Delete Password",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Warning)

        If confirmResult = DialogResult.Yes Then
            Try
                Dim deleted As Boolean = Await _passwordManager.DeleteCredentialAsync(lblWebsiteValue.Text, lblUsernameValue.Text)
                If deleted Then
                    WasDeleted = True
                    MessageBox.Show("Password deleted successfully.",
                                    "Password Deleted",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MessageBox.Show("Could not find the credential to delete.",
                                    "Delete Failed",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            Catch ex As Exception
                MessageBox.Show("Error deleting password: " & ex.Message,
                                "Delete Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

    ''' <summary>
    ''' Ensure password is hidden when form loses focus or closes.
    ''' </summary>
    Private Sub PasswordDetailForm_FormClosing(ByVal sender As Object, ByVal e As FormClosingEventArgs) Handles Me.FormClosing
        If _isPasswordVisible Then
            HidePassword()
        End If
    End Sub

End Class
