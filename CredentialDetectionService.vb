Imports System.Runtime.CompilerServices
Imports System.Security.Cryptography
Imports System.Text
Imports System.Text.Json
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.Core
Imports Microsoft.Web.WebView2.WinForms

''' <summary>
''' Application-layer service for detecting login forms on web pages and triggering
''' credential capture/autofill via JavaScript injection into WebView2.
''' 
''' This service:
''' - Injects a login form detection script after DOMContentLoaded
''' - Captures credential submissions via WebView2 PostMessage
''' - Injects autofill scripts to pre-fill saved credentials
''' - Respects private mode (no detection/autofill in PrivateBrowserForm)
''' </summary>
Public Class CredentialDetectionService

    Private Const CaptureMessageType As String = "kbrowser_credential_capture"

    Private ReadOnly _passwordManager As PasswordManagerService
    Private ReadOnly _settingsService As SettingsService

    ' Any page can call window.chrome.webview.postMessage, so a capture message is only trusted if it
    ' carries the secret the host put in the script it injected into that very page.
    Private NotInheritable Class NonceBox
        Public Value As String
    End Class
    Private ReadOnly _nonces As New ConditionalWeakTable(Of CoreWebView2, NonceBox)()

    Private ReadOnly _promptGate As New CapturePromptGate(TimeSpan.FromSeconds(5))

    ''' <summary>
    ''' Raised when a login form submission is detected and credentials are captured.
    ''' </summary>
    Public Event CredentialsCaptured As EventHandler(Of CredentialCapturedEventArgs)

    ''' <summary>
    ''' Raised when a page with a login form is loaded and saved credentials exist for it.
    ''' </summary>
    Public Event LoginFormDetected As EventHandler(Of LoginFormDetectedEventArgs)

    ''' <summary>What one page scan found (see <see cref="ScanPageAsync"/>).</summary>
    Public Structure PageScanResult
        ''' <summary>Our login detector is the one running in the page, so its capture messages can be trusted.</summary>
        Public DetectorInstalled As Boolean
        Public HasPasswordField As Boolean
    End Structure

    ''' <summary>
    ''' The script for one page scan: installs the login detector and/or checks for a password field, and returns
    ''' {nonce, hasPasswordField}. The answer is a fresh object literal, so a page cannot intercept the nonce
    ''' through setters it put on Object.prototype. Returns Nothing when there is nothing to do.
    ''' </summary>
    Public Shared Function BuildPageScanScript(ByVal nonceLiteral As String, ByVal installDetector As Boolean, ByVal checkForPasswordField As Boolean) As String
        If Not installDetector AndAlso Not checkForPasswordField Then Return Nothing
        Dim detector As String = If(installDetector, GetLoginDetectionScript(nonceLiteral).Trim().TrimEnd(";"c), "null")
        Dim passwordCheck As String = If(checkForPasswordField, "document.querySelector('input[type=""password""]') !== null", "false")
        Return "(function() {" & vbLf &
               "    var installed = " & detector & ";" & vbLf &
               "    var hasPasswordField = " & passwordCheck & ";" & vbLf &
               "    return {nonce: installed, hasPasswordField: hasPasswordField};" & vbLf &
               "})();"
    End Function

    ''' <summary>
    ''' Reads a page scan's answer (the JSON ExecuteScriptAsync returns). The detector only counts as ours when
    ''' the answer carries exactly the nonce we sent; anything unexpected counts as nothing found.
    ''' </summary>
    Public Shared Function ParsePageScanResult(ByVal json As String, ByVal expectedNonce As String) As PageScanResult
        Dim result As New PageScanResult()
        If String.IsNullOrWhiteSpace(json) Then Return result
        Try
            Using doc As JsonDocument = JsonDocument.Parse(json)
                Dim root As JsonElement = doc.RootElement
                If root.ValueKind <> JsonValueKind.Object Then Return result

                Dim nonce As JsonElement
                If Not String.IsNullOrEmpty(expectedNonce) AndAlso root.TryGetProperty("nonce", nonce) AndAlso
                   nonce.ValueKind = JsonValueKind.String Then
                    result.DetectorInstalled = String.Equals(nonce.GetString(), expectedNonce, StringComparison.Ordinal)
                End If

                Dim hasField As JsonElement
                If root.TryGetProperty("hasPasswordField", hasField) AndAlso hasField.ValueKind = JsonValueKind.True Then
                    result.HasPasswordField = True
                End If
            End Using
        Catch ex As JsonException
            Return New PageScanResult()
        End Try
        Return result
    End Function

    Public Sub New()
        _passwordManager = New PasswordManagerService()
        _settingsService = New SettingsService()
    End Sub

    Public Sub New(ByVal passwordManager As PasswordManagerService)
        _passwordManager = If(passwordManager, New PasswordManagerService())
        _settingsService = New SettingsService()
    End Sub

    ''' <summary>
    ''' Runs once per page load (after DOMContentLoaded) and makes a single trip into the page: installs the
    ''' login form detector, which captures submissions (when saving passwords is on), and checks for a
    ''' password field (when autofill is on). If the page has one and passwords are saved for the site,
    ''' raises LoginFormDetected.
    ''' </summary>
    Public Async Function ScanPageAsync(ByVal webView As WebView2) As Task
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then Return

        Try
            Dim core As CoreWebView2 = webView.CoreWebView2

            ' A new document starts with no trusted nonce; messages from it are rejected until its
            ' script has been injected and has reported back that it is the one running.
            SetNonce(core, Nothing)

            Dim settings = _settingsService.LoadSettings()
            Dim savePasswords As Boolean = settings?.Passwords Is Nothing OrElse settings.Passwords.SavePasswords
            Dim offerAutofill As Boolean = settings?.Passwords Is Nothing OrElse settings.Passwords.OfferAutoFill

            Dim nonce As String = NewNonce()
            Dim script As String = BuildPageScanScript(ToJsStringLiteral(nonce), savePasswords, offerAutofill)
            If script Is Nothing Then Return

            Dim scan As PageScanResult = ParsePageScanResult(Await core.ExecuteScriptAsync(script), nonce)

            ' The detector answers with its nonce only if it was the one that installed itself. If the
            ' page got there first (set the guard flag), nothing is trusted and capture stays off.
            If savePasswords AndAlso scan.DetectorInstalled Then SetNonce(core, nonce)

            If offerAutofill AndAlso scan.HasPasswordField Then
                Dim url As String = core.Source
                ' Don't even offer to fill a page the password may not be filled into (plain HTTP).
                If Not IsAutofillTargetAllowed(url, url) Then Return
                Dim savedCredentials = Await _passwordManager.GetCredentialsForSiteAsync(url)

                If savedCredentials.Count > 0 Then
                    RaiseEvent LoginFormDetected(Me, New LoginFormDetectedEventArgs(url, savedCredentials, webView))
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Error scanning page for login forms: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' True when a password saved for <paramref name="expectedUrl"/> may be filled into the page now at
    ''' <paramref name="currentUrl"/>: same origin (host and port), and the page is served over HTTPS
    ''' (or is loopback). Plain-HTTP pages can be served by anyone on the network path.
    ''' </summary>
    Public Shared Function IsAutofillTargetAllowed(ByVal expectedUrl As String, ByVal currentUrl As String) As Boolean
        Dim expected As Uri = Nothing
        Dim current As Uri = Nothing
        If Not Uri.TryCreate(expectedUrl, UriKind.Absolute, expected) Then Return False
        If Not Uri.TryCreate(currentUrl, UriKind.Absolute, current) Then Return False

        Dim secure As Boolean = current.Scheme = Uri.UriSchemeHttps OrElse
                                (current.Scheme = Uri.UriSchemeHttp AndAlso current.IsLoopback)
        If Not secure Then Return False

        Dim expectedKey As String = PasswordManagerService.ExtractDomain(expectedUrl)
        Return expectedKey.Length > 0 AndAlso
               expectedKey.Equals(PasswordManagerService.ExtractDomain(currentUrl), StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>
    ''' Returns <paramref name="value"/> as a double-quoted JavaScript string literal that cannot be
    ''' broken out of (quotes, backslashes, line breaks, U+2028/9 and "&lt;/script" are all escaped).
    ''' </summary>
    Public Shared Function ToJsStringLiteral(ByVal value As String) As String
        Return System.Text.Json.JsonSerializer.Serialize(If(value, String.Empty))
    End Function

    ''' <summary>
    ''' Injects JavaScript to autofill a login form with the given credentials.
    ''' Dispatches 'input' and 'change' events so the site's JavaScript recognizes the fill.
    ''' The fill is refused unless the page currently loaded is still the origin the credentials were
    ''' saved for: the user confirms in a dialog first, and the tab may navigate while it is open.
    ''' </summary>
    ''' <param name="expectedUrl">URL of the page the saved credentials belong to.</param>
    Public Async Function InjectAutofillAsync(ByVal webView As WebView2, ByVal expectedUrl As String, ByVal username As String, ByVal password As String) As Task
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then Return
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then Return

        Try
            Dim currentUrl As String = webView.CoreWebView2.Source
            If Not IsAutofillTargetAllowed(expectedUrl, currentUrl) Then
                System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Autofill refused — page is no longer the origin the credentials belong to.")
                Return
            End If

            ' The script re-checks its own document's origin, closing the gap between the check above
            ' and the moment the script actually runs.
            Dim currentUri As New Uri(currentUrl)
            Dim origin As String = currentUri.Scheme & "://" & currentUri.Authority

            Dim script As String = GetAutofillScript(ToJsStringLiteral(origin), ToJsStringLiteral(username), ToJsStringLiteral(password))
            Await webView.CoreWebView2.ExecuteScriptAsync(script)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Error injecting autofill: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Handles WebMessageReceived events from the injected login detection script.
    ''' Parses the credential capture message and raises the CredentialsCaptured event.
    ''' </summary>
    Public Sub HandleWebMessage(ByVal sender As Object, ByVal e As CoreWebView2WebMessageReceivedEventArgs)
        Dim promptKey As String = Nothing
        Try
            Dim core As CoreWebView2 = TryCast(sender, CoreWebView2)
            If core Is Nothing Then Return

            Dim capture As CredentialCapturedEventArgs = ParseCaptureMessage(e.WebMessageAsJson, e.Source, GetNonce(core))
            If capture Is Nothing Then Return

            ' Only the top-level document may capture, not a frame embedded in it.
            If Not IsSameHost(core.Source, e.Source) Then Return

            ' Check if this site is blocked
            If _passwordManager.IsBlockedSite(capture.Url) Then Return

            ' The prompt is modal: never stack prompts, and give a site the user dismissed some peace.
            promptKey = PasswordManagerService.ExtractDomain(capture.Url)
            If Not _promptGate.TryEnter(promptKey) Then
                promptKey = Nothing
                Return
            End If

            RaiseEvent CredentialsCaptured(Me, capture)
        Catch ex As Exception
            If promptKey IsNot Nothing Then _promptGate.Release(promptKey, suppress:=False)
            System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Error processing web message: " & ex.Message)
        End Try
    End Sub

    ''' <summary>
    ''' Called by whoever showed the save prompt for a capture, once it is closed (success or not).
    ''' With <paramref name="suppress"/> the site is not prompted again this session.
    ''' </summary>
    Public Sub ReleaseCapturePrompt(ByVal url As String, ByVal suppress As Boolean)
        _promptGate.Release(PasswordManagerService.ExtractDomain(url), suppress)
    End Sub

    ''' <summary>
    ''' Validates a message received from a page. Returns the captured credentials, or Nothing if the
    ''' message is not one produced by the script this host injected into the sending page.
    ''' </summary>
    ''' <param name="webMessageJson">CoreWebView2WebMessageReceivedEventArgs.WebMessageAsJson. The injected
    ''' script posts an object; a string message (its JSON is a quoted string) is never a capture.</param>
    ''' <param name="sourceUrl">e.Source — set by the WebView2 runtime, not by page content.</param>
    ''' <param name="expectedNonce">The secret recorded when the script was injected into this document.</param>
    Public Shared Function ParseCaptureMessage(ByVal webMessageJson As String, ByVal sourceUrl As String, ByVal expectedNonce As String) As CredentialCapturedEventArgs
        If String.IsNullOrEmpty(webMessageJson) OrElse String.IsNullOrEmpty(expectedNonce) Then Return Nothing

        Try
            Using doc As JsonDocument = JsonDocument.Parse(webMessageJson)
                Dim root As JsonElement = doc.RootElement
                If root.ValueKind <> JsonValueKind.Object Then Return Nothing

                If GetStringProperty(root, "type") <> CaptureMessageType Then Return Nothing

                ' Any page can call window.chrome.webview.postMessage() directly. Without the nonce a page
                ' could raise the save prompt on its own, with credentials of its choosing.
                Dim nonce As String = GetStringProperty(root, "nonce")
                If nonce Is Nothing OrElse Not CryptographicOperations.FixedTimeEquals(
                        Encoding.UTF8.GetBytes(nonce), Encoding.UTF8.GetBytes(expectedNonce)) Then Return Nothing

                Dim username As String = GetStringProperty(root, "username")
                Dim password As String = GetStringProperty(root, "password")
                Dim url As String = GetStringProperty(root, "url")
                If String.IsNullOrWhiteSpace(username) OrElse String.IsNullOrWhiteSpace(password) Then Return Nothing

                ' The url inside the payload is page-supplied; the source is not. Reject a message that
                ' claims to be from another origin (e.g. to poison a banking site's saved credentials).
                If Not IsSameHost(url, sourceUrl) Then
                    System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Rejected credential capture — url/source origin mismatch.")
                    Return Nothing
                End If

                Return New CredentialCapturedEventArgs(url, username, password)
            End Using
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Private Shared Function GetStringProperty(ByVal root As JsonElement, ByVal name As String) As String
        Dim prop As JsonElement = Nothing
        If root.TryGetProperty(name, prop) AndAlso prop.ValueKind = JsonValueKind.String Then Return prop.GetString()
        Return Nothing
    End Function

    Private Shared Function NewNonce() As String
        Return Convert.ToHexString(RandomNumberGenerator.GetBytes(16)).ToLowerInvariant()
    End Function

    Private Function GetNonce(ByVal core As CoreWebView2) As String
        Dim box As NonceBox = Nothing
        Return If(_nonces.TryGetValue(core, box), box.Value, Nothing)
    End Function

    Private Sub SetNonce(ByVal core As CoreWebView2, ByVal nonce As String)
        _nonces.GetOrCreateValue(core).Value = nonce
    End Sub

    ''' <summary>
    ''' Compares the host of a page-claimed URL against the host of the WebView2-provided
    ''' source URL. Used to reject forged credential-capture messages from untrusted page content.
    ''' </summary>
    Private Shared Function IsSameHost(ByVal claimedUrl As String, ByVal actualSourceUrl As String) As Boolean
        Dim claimedHost As String = PasswordManagerService.ExtractDomain(claimedUrl)
        Dim actualHost As String = PasswordManagerService.ExtractDomain(actualSourceUrl)
        Return Not String.IsNullOrEmpty(claimedHost) AndAlso claimedHost.Equals(actualHost, StringComparison.OrdinalIgnoreCase)
    End Function

    ' ─────────────────────────────────────────────────
    ' JavaScript Generation
    ' ─────────────────────────────────────────────────

    ''' <summary>
    ''' Returns the JavaScript that detects login form submissions and posts
    ''' credentials back to the host via window.chrome.webview.postMessage().
    ''' The script evaluates to its nonce when it installed itself, and to null when a page had already
    ''' claimed the guard flag; the host only trusts messages once it has seen its own nonce come back.
    ''' </summary>
    ''' <param name="nonceLiteral">The per-document secret, already a JavaScript string literal.</param>
    Private Shared Function GetLoginDetectionScript(ByVal nonceLiteral As String) As String
        Return "
(function() {
    if (window.__kbrowser_credential_detection) return null;
    window.__kbrowser_credential_detection = true;
    var nonce = " & nonceLiteral & ";

    function findUsernameField(form) {
        var inputs = form.querySelectorAll('input[type=""text""], input[type=""email""], input[type=""tel""], input:not([type])');
        for (var i = 0; i < inputs.length; i++) {
            var inp = inputs[i];
            var name = (inp.name || inp.id || inp.autocomplete || '').toLowerCase();
            if (name.indexOf('user') >= 0 || name.indexOf('email') >= 0 || name.indexOf('login') >= 0 ||
                name.indexOf('account') >= 0 || name.indexOf('name') >= 0 || inp.type === 'email') {
                return inp;
            }
        }
        if (inputs.length > 0) return inputs[0];
        return null;
    }

    function handleFormSubmit(e) {
        var form = e.target;
        var passwordField = form.querySelector('input[type=""password""]');
        if (!passwordField || !passwordField.value) return;

        var usernameField = findUsernameField(form);
        var username = usernameField ? usernameField.value : '';

        if (username && passwordField.value) {
            // An object, not JSON.stringify(...): the host reads WebMessageAsJson and a string
            // message would arrive as a quoted string, not as the fields below.
            window.chrome.webview.postMessage({
                type: 'kbrowser_credential_capture',
                nonce: nonce,
                username: username,
                password: passwordField.value,
                url: window.location.href
            });
        }
    }

    document.addEventListener('submit', function(e) {
        if (e.target && e.target.tagName === 'FORM') {
            handleFormSubmit(e);
        }
    }, true);

    // Also watch for programmatic form submissions (e.g., SPA login buttons)
    var forms = document.querySelectorAll('form');
    forms.forEach(function(form) {
        if (form.querySelector('input[type=""password""]')) {
            form.addEventListener('submit', handleFormSubmit, true);
        }
    });
    return nonce;
})();
"
    End Function

    ''' <summary>
    ''' Returns JavaScript that fills username and password fields on the current page.
    ''' All three arguments must already be JavaScript string literals (see ToJsStringLiteral).
    ''' </summary>
    Private Shared Function GetAutofillScript(ByVal originLiteral As String, ByVal usernameLiteral As String, ByVal passwordLiteral As String) As String
        Return "
(function() {
    if (window.location.origin !== " & originLiteral & ") return;

    function triggerInputEvent(el) {
        el.dispatchEvent(new Event('input', {bubbles: true}));
        el.dispatchEvent(new Event('change', {bubbles: true}));
        el.dispatchEvent(new Event('blur', {bubbles: true}));
    }

    var passwordField = document.querySelector('input[type=""password""]');
    if (!passwordField) return;

    var form = passwordField.closest('form') || document;
    var inputs = form.querySelectorAll('input[type=""text""], input[type=""email""], input[type=""tel""], input:not([type])');
    var usernameField = null;

    for (var i = 0; i < inputs.length; i++) {
        var inp = inputs[i];
        var name = (inp.name || inp.id || inp.autocomplete || '').toLowerCase();
        if (name.indexOf('user') >= 0 || name.indexOf('email') >= 0 || name.indexOf('login') >= 0 ||
            name.indexOf('account') >= 0 || name.indexOf('name') >= 0 || inp.type === 'email') {
            usernameField = inp;
            break;
        }
    }
    if (!usernameField && inputs.length > 0) usernameField = inputs[0];

    if (usernameField) {
        var nativeSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
        nativeSetter.call(usernameField, " & usernameLiteral & ");
        triggerInputEvent(usernameField);
    }

    if (passwordField) {
        var nativeSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
        nativeSetter.call(passwordField, " & passwordLiteral & ");
        triggerInputEvent(passwordField);
    }
})();
"
    End Function

End Class

''' <summary>
''' Event args for when credentials are captured from a login form submission.
''' </summary>
Public Class CredentialCapturedEventArgs
    Inherits EventArgs

    Public ReadOnly Property Url As String
    Public ReadOnly Property Username As String
    Public ReadOnly Property Password As String

    Public Sub New(ByVal url As String, ByVal username As String, ByVal password As String)
        Me.Url = url
        Me.Username = username
        Me.Password = password
    End Sub
End Class

''' <summary>
''' Event args for when a login form is detected on a page with saved credentials.
''' </summary>
Public Class LoginFormDetectedEventArgs
    Inherits EventArgs

    Public ReadOnly Property Url As String
    Public ReadOnly Property SavedCredentials As List(Of CredentialEntry)
    Public ReadOnly Property WebView As WebView2

    Public Sub New(ByVal url As String, ByVal credentials As List(Of CredentialEntry), ByVal webView As WebView2)
        Me.Url = url
        Me.SavedCredentials = credentials
        Me.WebView = webView
    End Sub
End Class
