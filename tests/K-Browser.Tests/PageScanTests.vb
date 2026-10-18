Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' On every page load the password features make one round trip into the page: it installs the login
''' detector (when saving passwords is on) and reports whether there is a password field (when autofill is on).
''' </summary>
Public Class PageScanTests

    Private Const Lit As String = """n0nce"""

    <Fact>
    Public Sub BothFeaturesOff_NoScriptRuns()
        Assert.Null(CredentialDetectionService.BuildPageScanScript(Lit, installDetector:=False, checkForPasswordField:=False))
    End Sub

    <Fact>
    Public Sub BothFeaturesOn_OneScriptDoesBoth()
        Dim js = CredentialDetectionService.BuildPageScanScript(Lit, True, True)
        Assert.Contains("__kbrowser_credential_detection", js)
        Assert.Contains("input[type=""password""]", js)
        Assert.Contains(Lit, js)
    End Sub

    <Fact>
    Public Sub SavingOff_DetectorIsNotInstalled()
        Dim js = CredentialDetectionService.BuildPageScanScript(Lit, installDetector:=False, checkForPasswordField:=True)
        Assert.DoesNotContain("__kbrowser_credential_detection", js)
        Assert.DoesNotContain(Lit, js)
    End Sub

    <Fact>
    Public Sub Result_WithOurNonce_IsTrusted()
        Dim r = CredentialDetectionService.ParsePageScanResult("{""nonce"":""n0nce"",""hasPasswordField"":true}", "n0nce")
        Assert.True(r.DetectorInstalled)
        Assert.True(r.HasPasswordField)
    End Sub

    <Theory>
    <InlineData("{""nonce"":""other"",""hasPasswordField"":false}")>
    <InlineData("{""nonce"":null,""hasPasswordField"":false}")>
    <InlineData("{""hasPasswordField"":false}")>
    <InlineData("{""nonce"":42,""hasPasswordField"":false}")>
    Public Sub Result_WithoutOurNonce_IsNotTrusted(json As String)
        Assert.False(CredentialDetectionService.ParsePageScanResult(json, "n0nce").DetectorInstalled)
    End Sub

    <Theory>
    <InlineData("null")>
    <InlineData("")>
    <InlineData("not json")>
    <InlineData("""a string""")>
    <InlineData("{""nonce"":""n0nce"",""hasPasswordField"":""yes""}")>
    Public Sub OddResults_AreTreatedAsNothingFound(json As String)
        Dim r = CredentialDetectionService.ParsePageScanResult(json, "n0nce")
        Assert.False(r.HasPasswordField)
    End Sub

    <Fact>
    Public Sub NoExpectedNonce_NeverTrusts()
        Assert.False(CredentialDetectionService.ParsePageScanResult("{""nonce"":"""",""hasPasswordField"":true}", Nothing).DetectorInstalled)
    End Sub

End Class
