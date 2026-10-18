Imports System.Text.Json
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' The host listens to messages from arbitrary web pages. Only a message produced by the script the
''' host itself injected (it carries that script's per-page secret) may open the "Save password?" prompt,
''' and the prompt must not be usable to lock up the browser.
''' </summary>
Public Class CredentialCaptureTests

    Private Const Nonce As String = "9f2c41d07b6e4a1c8d3e5f60a7b81c24"
    Private Const SiteUrl As String = "https://shop.example.com/login"

    Private Shared Function Message(Optional nonce As String = Nonce,
                                    Optional username As String = "alice",
                                    Optional password As String = "s3cret",
                                    Optional url As String = SiteUrl,
                                    Optional type As String = "kbrowser_credential_capture") As String
        Dim fields As New Dictionary(Of String, Object)
        fields("type") = type
        If nonce IsNot Nothing Then fields("nonce") = nonce
        If username IsNot Nothing Then fields("username") = username
        If password IsNot Nothing Then fields("password") = password
        If url IsNot Nothing Then fields("url") = url
        Return JsonSerializer.Serialize(fields)
    End Function

    ' ── Message validation ─────────────────────────────────────────────────

    <Fact>
    Public Sub ValidMessage_IsAccepted()
        Dim capture = CredentialDetectionService.ParseCaptureMessage(Message(), SiteUrl, Nonce)
        Assert.NotNull(capture)
        Assert.Equal("alice", capture.Username)
        Assert.Equal("s3cret", capture.Password)
        Assert.Equal(SiteUrl, capture.Url)
    End Sub

    <Fact>
    Public Sub MessageFromTheInjectedScript_ArrivesAsAJsonObject_NotAJsonString()
        ' WebMessageAsJson of postMessage("{...}") is a quoted string, which is never a capture.
        Dim asString As String = JsonSerializer.Serialize(Message())
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(asString, SiteUrl, Nonce))
    End Sub

    <Theory>
    <InlineData("wrong-nonce")>
    <InlineData("")>
    <InlineData(Nothing)>
    Public Sub Message_WithoutTheCorrectNonce_IsRejected(sentNonce As String)
        ' (VB is case-insensitive: a parameter named "nonce" would shadow the Nonce constant.)
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(nonce:=sentNonce), SiteUrl, Nonce))
    End Sub

    <Theory>
    <InlineData(Nothing)>
    <InlineData("")>
    Public Sub Message_IsRejected_WhenNoScriptWasInjected(expectedNonce As String)
        ' No nonce on record (injection skipped, or a page pre-empted it): fail closed.
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(nonce:=""), SiteUrl, expectedNonce))
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(), SiteUrl, expectedNonce))
    End Sub

    <Theory>
    <InlineData("https://bank.com/login")>
    <InlineData("https://shop.example.com.evil.com/login")>
    <InlineData("https://shop.example.com:8443/login")>
    Public Sub Message_ClaimingAnotherOrigin_IsRejected(claimedUrl As String)
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(url:=claimedUrl), SiteUrl, Nonce))
    End Sub

    <Fact>
    Public Sub Message_SentFromAnotherOrigin_IsRejected()
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(), "https://evil.com/frame", Nonce))
    End Sub

    <Theory>
    <InlineData(Nothing, "pw")>
    <InlineData("", "pw")>
    <InlineData("   ", "pw")>
    <InlineData("alice", Nothing)>
    <InlineData("alice", "")>
    <InlineData("alice", "   ")>
    Public Sub Message_WithoutUsernameOrPassword_IsRejected(username As String, password As String)
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(username:=username, password:=password), SiteUrl, Nonce))
    End Sub

    <Fact>
    Public Sub Message_OfTheWrongType_IsRejected()
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(Message(type:="something_else"), SiteUrl, Nonce))
    End Sub

    <Theory>
    <InlineData("")>
    <InlineData(Nothing)>
    <InlineData("not json")>
    <InlineData("123")>
    <InlineData("[]")>
    <InlineData("null")>
    <InlineData("{""type"":""kbrowser_credential_capture"",""nonce"":1,""username"":2,""password"":3,""url"":4}")>
    Public Sub Garbage_IsRejected_WithoutThrowing(json As String)
        Assert.Null(CredentialDetectionService.ParseCaptureMessage(json, SiteUrl, Nonce))
    End Sub

    ' ── Prompt gate ────────────────────────────────────────────────────────

    Private NotInheritable Class FakeClock
        Public Now As New DateTime(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc)
        Public Function Read() As DateTime
            Return Now
        End Function
    End Class

    Private Shared Function NewGate(clock As FakeClock) As CapturePromptGate
        Return New CapturePromptGate(TimeSpan.FromSeconds(5), AddressOf clock.Read)
    End Function

    <Fact>
    Public Sub Gate_AllowsOnePromptAtATime()
        Dim gate = NewGate(New FakeClock())
        Assert.True(gate.TryEnter("a.com"))
        Assert.False(gate.TryEnter("a.com"))
        Assert.False(gate.TryEnter("b.com"))
    End Sub

    <Fact>
    Public Sub Gate_RefusesAnotherPrompt_UntilTheCooldownPasses()
        Dim clock As New FakeClock()
        Dim gate = NewGate(clock)
        Assert.True(gate.TryEnter("a.com"))
        gate.Release("a.com", suppress:=False)

        clock.Now = clock.Now.AddSeconds(4)
        Assert.False(gate.TryEnter("a.com"))

        clock.Now = clock.Now.AddSeconds(2)
        Assert.True(gate.TryEnter("a.com"))
    End Sub

    <Fact>
    Public Sub Gate_MutesASite_TheUserDismissed_ForTheRestOfTheSession()
        Dim clock As New FakeClock()
        Dim gate = NewGate(clock)
        Assert.True(gate.TryEnter("spam.com"))
        gate.Release("spam.com", suppress:=True)

        clock.Now = clock.Now.AddHours(1)
        Assert.False(gate.TryEnter("spam.com"))
        Assert.True(gate.TryEnter("other.com"))
    End Sub

    <Fact>
    Public Sub Gate_KeysAreCaseInsensitive()
        Dim clock As New FakeClock()
        Dim gate = NewGate(clock)
        Assert.True(gate.TryEnter("Spam.com"))
        gate.Release("SPAM.com", suppress:=True)
        clock.Now = clock.Now.AddHours(1)
        Assert.False(gate.TryEnter("spam.COM"))
    End Sub

End Class
