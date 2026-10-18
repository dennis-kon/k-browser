Imports System.Security.Principal
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' LogonUser succeeds for ANY valid Windows account. Revealing saved passwords must additionally
''' require that the account that just authenticated is the one running the browser.
''' </summary>
Public Class WindowsAuthenticationTests

    <Fact>
    Public Sub CurrentUsersSid_IsTheCurrentUser()
        Dim sid As SecurityIdentifier = WindowsIdentity.GetCurrent().User
        Assert.True(WindowsAuthenticationService.IsCurrentUser(sid))
    End Sub

    <Fact>
    Public Sub AnotherAccountsSid_IsNotTheCurrentUser()
        Dim system As New SecurityIdentifier(WellKnownSidType.LocalSystemSid, Nothing)
        Dim guests As New SecurityIdentifier("S-1-5-32-546")
        Assert.False(WindowsAuthenticationService.IsCurrentUser(system))
        Assert.False(WindowsAuthenticationService.IsCurrentUser(guests))
    End Sub

    <Fact>
    Public Sub NoSid_IsNotTheCurrentUser()
        Assert.False(WindowsAuthenticationService.IsCurrentUser(Nothing))
    End Sub

    <Fact>
    Public Sub ATokenForTheCurrentUser_IsAccepted()
        Using identity As WindowsIdentity = WindowsIdentity.GetCurrent()
            Assert.True(WindowsAuthenticationService.TokenBelongsToCurrentUser(identity.Token))
        End Using
    End Sub

    <Fact>
    Public Sub ANullToken_IsRefused()
        Assert.False(WindowsAuthenticationService.TokenBelongsToCurrentUser(IntPtr.Zero))
    End Sub

End Class
