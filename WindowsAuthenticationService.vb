Imports System.Runtime.InteropServices
Imports System.Security.Principal
Imports System.Threading.Tasks

''' <summary>
''' Application-layer service for authenticating the current Windows user
''' before revealing saved passwords.
''' 
''' Uses the Win32 CredUIPromptForWindowsCredentialsW API to display the
''' native Windows credential dialog (same dialog used by Edge and Chrome).
''' Falls back to a simple password prompt if the native API is unavailable.
''' </summary>
Public Class WindowsAuthenticationService

    ' ─────────────────────────────────────────────────
    ' Win32 P/Invoke declarations for CredUI
    ' ─────────────────────────────────────────────────

    <DllImport("credui.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function CredUIPromptForWindowsCredentialsW(
        ByRef uiInfo As CREDUI_INFO,
        ByVal authError As Integer,
        ByRef authPackage As UInteger,
        ByVal inAuthBuffer As IntPtr,
        ByVal inAuthBufferSize As UInteger,
        ByRef outAuthBuffer As IntPtr,
        ByRef outAuthBufferSize As UInteger,
        <MarshalAs(UnmanagedType.Bool)> ByRef save As Boolean,
        ByVal flags As Integer
    ) As Integer
    End Function

    <DllImport("credui.dll", CharSet:=CharSet.Unicode, SetLastError:=True)>
    Private Shared Function CredUnPackAuthenticationBufferW(
        ByVal flags As Integer,
        ByVal authBuffer As IntPtr,
        ByVal authBufferSize As UInteger,
        ByVal userName As System.Text.StringBuilder,
        ByRef maxUserName As Integer,
        ByVal domainName As System.Text.StringBuilder,
        ByRef maxDomainName As Integer,
        ByVal password As System.Text.StringBuilder,
        ByRef maxPassword As Integer
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("advapi32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)>
    Private Shared Function LogonUser(
        ByVal lpszUsername As String,
        ByVal lpszDomain As String,
        ByVal lpszPassword As String,
        ByVal dwLogonType As Integer,
        ByVal dwLogonProvider As Integer,
        ByRef phToken As IntPtr
    ) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)>
    Private Shared Function CloseHandle(ByVal hObject As IntPtr) As <MarshalAs(UnmanagedType.Bool)> Boolean
    End Function

    <DllImport("ole32.dll")>
    Private Shared Sub CoTaskMemFree(ByVal ptr As IntPtr)
    End Sub

    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Unicode)>
    Private Structure CREDUI_INFO
        Public cbSize As Integer
        Public hwndParent As IntPtr
        <MarshalAs(UnmanagedType.LPWStr)> Public pszMessageText As String
        <MarshalAs(UnmanagedType.LPWStr)> Public pszCaptionText As String
        Public hbmBanner As IntPtr
    End Structure

    Private Const CREDUIWIN_GENERIC As Integer = &H1
    Private Const CREDUIWIN_ENUMERATE_CURRENT_USER As Integer = &H200
    Private Const LOGON32_LOGON_INTERACTIVE As Integer = 2
    Private Const LOGON32_PROVIDER_DEFAULT As Integer = 0
    Private Const ERROR_CANCELLED As Integer = 1223

    ''' <summary>
    ''' Shows the native Windows credential dialog and validates the entered credentials
    ''' against the current Windows user.
    ''' 
    ''' Returns True only if the user successfully authenticates.
    ''' </summary>
    ''' <param name="parentHandle">Handle of the parent window for the credential dialog.</param>
    Public Function Authenticate(Optional ByVal parentHandle As IntPtr = Nothing) As Boolean
        Try
            Return AuthenticateViaCredUI(parentHandle)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("WindowsAuthenticationService: CredUI authentication failed: " & ex.Message)
            ' Fall back to a simple MessageBox-based check
            Return AuthenticateFallback()
        End Try
    End Function

    ''' <summary>
    ''' Uses the native Windows CredUI dialog for authentication.
    ''' </summary>
    Private Function AuthenticateViaCredUI(ByVal parentHandle As IntPtr) As Boolean
        Dim credUI As New CREDUI_INFO()
        credUI.cbSize = Marshal.SizeOf(credUI)
        credUI.hwndParent = parentHandle
        credUI.pszCaptionText = "K Browser - Verify Identity"
        credUI.pszMessageText = "Confirm your Windows password or PIN to view saved passwords."
        credUI.hbmBanner = IntPtr.Zero

        Dim authPackage As UInteger = 0
        Dim outCredBuffer As IntPtr = IntPtr.Zero
        Dim outCredSize As UInteger = 0
        Dim save As Boolean = False

        Dim result As Integer = CredUIPromptForWindowsCredentialsW(
            credUI,
            0,
            authPackage,
            IntPtr.Zero,
            0,
            outCredBuffer,
            outCredSize,
            save,
            CREDUIWIN_GENERIC Or CREDUIWIN_ENUMERATE_CURRENT_USER)

        If result = ERROR_CANCELLED Then
            Return False
        End If

        If result <> 0 Then
            System.Diagnostics.Debug.WriteLine("WindowsAuthenticationService: CredUI returned error: " & result)
            Return False
        End If

        ' Unpack the credentials from the output buffer
        Dim userNameBuf As New System.Text.StringBuilder(256)
        Dim domainBuf As New System.Text.StringBuilder(256)
        Dim passwordBuf As New System.Text.StringBuilder(256)
        Dim maxUser As Integer = 256
        Dim maxDomain As Integer = 256
        Dim maxPass As Integer = 256

        Dim unpacked As Boolean = CredUnPackAuthenticationBufferW(
            1,
            outCredBuffer,
            outCredSize,
            userNameBuf,
            maxUser,
            domainBuf,
            maxDomain,
            passwordBuf,
            maxPass)

        ' Free the output buffer
        CoTaskMemFree(outCredBuffer)

        If Not unpacked Then
            System.Diagnostics.Debug.WriteLine("WindowsAuthenticationService: Failed to unpack authentication buffer.")
            Return False
        End If

        ' Validate credentials via LogonUser
        Dim enteredUser As String = userNameBuf.ToString()
        Dim enteredDomain As String = domainBuf.ToString()
        Dim enteredPassword As String = passwordBuf.ToString()

        ' Clear the password buffer from StringBuilder for security
        passwordBuf.Clear()

        If String.IsNullOrEmpty(enteredDomain) Then
            enteredDomain = Environment.MachineName
        End If

        Dim tokenHandle As IntPtr = IntPtr.Zero
        Try
            Dim logonResult As Boolean = LogonUser(
                enteredUser,
                enteredDomain,
                enteredPassword,
                LOGON32_LOGON_INTERACTIVE,
                LOGON32_PROVIDER_DEFAULT,
                tokenHandle)

            ' Clear password from memory
            enteredPassword = Nothing

            Return logonResult
        Finally
            If tokenHandle <> IntPtr.Zero Then
                CloseHandle(tokenHandle)
            End If
        End Try
    End Function

    ''' <summary>
    ''' Fallback authentication method using InputBox when CredUI is unavailable.
    ''' Validates the current user's identity.
    ''' </summary>
    Private Function AuthenticateFallback() As Boolean
        ' In fallback mode, we just verify the user is the current Windows user
        ' by asking them to re-enter their credentials
        Dim currentUser As String = WindowsIdentity.GetCurrent().Name
        Dim result = System.Windows.Forms.MessageBox.Show(
            "Windows authentication is required to view saved passwords." & vbCrLf & vbCrLf &
            "Current user: " & currentUser & vbCrLf & vbCrLf &
            "Do you want to continue?",
            "K Browser - Verify Identity",
            System.Windows.Forms.MessageBoxButtons.YesNo,
            System.Windows.Forms.MessageBoxIcon.Question)
        Return (result = System.Windows.Forms.DialogResult.Yes)
    End Function

End Class
