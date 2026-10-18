Imports System.Text.Json
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' Guards the two places a saved password can leak to the wrong page: which origin it may be
''' filled into, and how it is embedded in the script that fills it.
''' </summary>
Public Class AutofillSecurityTests

    ' ── Origin the password may be filled into ─────────────────────────────

    <Theory>
    <InlineData("https://bank.com/login", "https://bank.com/login")>
    <InlineData("https://bank.com/login", "https://bank.com/other/page?x=1")>
    <InlineData("https://BANK.com/login", "https://bank.com/")>
    <InlineData("http://localhost:3000/login", "http://localhost:3000/login")>
    <InlineData("http://127.0.0.1:8080/", "http://127.0.0.1:8080/app")>
    Public Sub Autofill_IsAllowed_OnTheSameSecureOrigin(expected As String, current As String)
        Assert.True(CredentialDetectionService.IsAutofillTargetAllowed(expected, current))
    End Sub

    <Fact>
    Public Sub Autofill_IsRefused_OnPlainHttp_EvenForTheSameHost()
        ' http://bank.com can be served by anyone on the network path.
        Assert.False(CredentialDetectionService.IsAutofillTargetAllowed("https://bank.com/login", "http://bank.com/login"))
        Assert.False(CredentialDetectionService.IsAutofillTargetAllowed("http://bank.com/login", "http://bank.com/login"))
    End Sub

    <Theory>
    <InlineData("https://bank.com/login", "https://evil.com/login")>
    <InlineData("https://bank.com/login", "https://bank.com.evil.com/login")>
    <InlineData("https://bank.com/login", "https://evilbank.com/login")>
    <InlineData("https://bank.com/login", "https://www.bank.com/login")>
    <InlineData("https://www.bank.com/login", "https://bank.com/login")>
    <InlineData("https://bank.com/login", "https://bank.com:8443/login")>
    <InlineData("https://bank.com/login", "https://bank.com@evil.com/login")>
    Public Sub Autofill_IsRefused_WhenThePageMovedToAnotherOrigin(expected As String, current As String)
        ' The user confirms the fill in a dialog; the tab may navigate while it is open.
        Assert.False(CredentialDetectionService.IsAutofillTargetAllowed(expected, current))
    End Sub

    <Theory>
    <InlineData("", "https://bank.com/")>
    <InlineData("https://bank.com/", "")>
    <InlineData("https://bank.com/", Nothing)>
    <InlineData("https://bank.com/", "about:blank")>
    <InlineData("https://bank.com/", "not a url")>
    <InlineData("https://bank.com/", "file:///C:/bank.com/login.html")>
    Public Sub Autofill_IsRefused_WhenEitherUrlIsMissingOrNotWeb(expected As String, current As String)
        Assert.False(CredentialDetectionService.IsAutofillTargetAllowed(expected, current))
    End Sub

    ' ── Origin key used to store and look up credentials ───────────────────

    <Theory>
    <InlineData("https://github.com/login", "github.com")>
    <InlineData("HTTPS://GitHub.com/Login", "github.com")>
    <InlineData("github.com", "github.com")>
    <InlineData("https://example.com:443/", "example.com")>
    <InlineData("http://example.com:80/", "example.com")>
    <InlineData("https://example.com:8443/x", "example.com:8443")>
    <InlineData("http://localhost:3000", "localhost:3000")>
    <InlineData("example.com:8443/path", "example.com:8443")>
    Public Sub ExtractDomain_KeepsNonDefaultPorts(url As String, expected As String)
        ' Same host on another port is a different origin and must not share saved passwords.
        Assert.Equal(expected, PasswordManagerService.ExtractDomain(url))
    End Sub

    ' ── Embedding a password in the fill script ────────────────────────────

    Public Shared ReadOnly NastyValues As TheoryData(Of String) = New TheoryData(Of String) From {
        "plain",
        "it's",
        "back\slash",
        "'; alert(1); //",
        "line1" & vbLf & "line2",
        "line1" & vbCr & vbLf & "line2",
        "sep" & ChrW(&H2028) & "arator" & ChrW(&H2029),
        "</script><script>alert(1)</script>",
        "quote""double",
        "unicode-" & ChrW(&H3A9) & ChrW(&HD83D) & ChrW(&HDE00),
        ""
    }

    <Theory>
    <MemberData(NameOf(NastyValues))>
    Public Sub JsStringLiteral_RoundTripsAnyValue(value As String)
        Dim literal As String = CredentialDetectionService.ToJsStringLiteral(value)
        Assert.Equal(value, JsonSerializer.Deserialize(Of String)(literal))
    End Sub

    <Theory>
    <MemberData(NameOf(NastyValues))>
    Public Sub JsStringLiteral_NeverContainsCharactersThatEndAJsString(value As String)
        Dim literal As String = CredentialDetectionService.ToJsStringLiteral(value)
        Assert.StartsWith("""", literal)
        Assert.EndsWith("""", literal)
        Assert.DoesNotContain(vbLf, literal)
        Assert.DoesNotContain(vbCr, literal)
        Assert.DoesNotContain(ChrW(&H2028).ToString(), literal)
        Assert.DoesNotContain(ChrW(&H2029).ToString(), literal)
        Assert.DoesNotContain("</", literal)
    End Sub

End Class
