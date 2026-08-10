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

    Private ReadOnly _passwordManager As PasswordManagerService
    Private ReadOnly _settingsService As SettingsService

    ''' <summary>
    ''' Raised when a login form submission is detected and credentials are captured.
    ''' </summary>
    Public Event CredentialsCaptured As EventHandler(Of CredentialCapturedEventArgs)

    ''' <summary>
    ''' Raised when a page with a login form is loaded and saved credentials exist for it.
    ''' </summary>
    Public Event LoginFormDetected As EventHandler(Of LoginFormDetectedEventArgs)

    Public Sub New()
        _passwordManager = New PasswordManagerService()
        _settingsService = New SettingsService()
    End Sub

    Public Sub New(ByVal passwordManager As PasswordManagerService)
        _passwordManager = If(passwordManager, New PasswordManagerService())
        _settingsService = New SettingsService()
    End Sub

    ''' <summary>
    ''' Injects the login form detection JavaScript into the WebView2 control.
    ''' This script monitors for forms with password fields and captures submissions.
    ''' Must be called after DOMContentLoaded.
    ''' </summary>
    Public Async Function InjectLoginDetectionScriptAsync(ByVal webView As WebView2) As Task
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then Return

        Try
            ' Check if password saving is enabled in settings
            Dim settings = _settingsService.LoadSettings()
            If settings?.Passwords IsNot Nothing AndAlso Not settings.Passwords.SavePasswords Then Return

            Dim script As String = GetLoginDetectionScript()
            Await webView.CoreWebView2.ExecuteScriptAsync(script)
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Error injecting login detection script: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Checks if the current page has a login form and raises LoginFormDetected if
    ''' saved credentials exist for the domain.
    ''' </summary>
    Public Async Function CheckForAutofillAsync(ByVal webView As WebView2) As Task
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then Return

        Try
            Dim settings = _settingsService.LoadSettings()
            If settings?.Passwords IsNot Nothing AndAlso Not settings.Passwords.OfferAutoFill Then Return

            ' Check if the page has a password input field
            Dim hasLoginForm As String = Await webView.CoreWebView2.ExecuteScriptAsync(
                "(function(){return document.querySelector('input[type=""password""]') !== null;})()")

            If hasLoginForm = "true" Then
                Dim url As String = webView.CoreWebView2.Source
                Dim savedCredentials = Await _passwordManager.GetCredentialsForSiteAsync(url)

                If savedCredentials.Count > 0 Then
                    RaiseEvent LoginFormDetected(Me, New LoginFormDetectedEventArgs(url, savedCredentials, webView))
                End If
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Error checking for autofill: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    ''' Injects JavaScript to autofill a login form with the given credentials.
    ''' Dispatches 'input' and 'change' events so the site's JavaScript recognizes the fill.
    ''' </summary>
    Public Async Function InjectAutofillAsync(ByVal webView As WebView2, ByVal username As String, ByVal password As String) As Task
        If webView Is Nothing OrElse webView.CoreWebView2 Is Nothing Then Return
        If String.IsNullOrEmpty(username) OrElse String.IsNullOrEmpty(password) Then Return

        Try
            ' Escape backslashes BEFORE single quotes for safe JS injection.
            ' Escaping quotes first would double the backslash just inserted,
            ' un-escaping the quote and breaking out of the string literal.
            Dim safeUser As String = username.Replace("\", "\\").Replace("'", "\'")
            Dim safePass As String = password.Replace("\", "\\").Replace("'", "\'")

            Dim script As String = GetAutofillScript(safeUser, safePass)
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
        Try
            Dim message As String = e.WebMessageAsJson
            If String.IsNullOrEmpty(message) Then Return

            ' Parse the JSON message
            Dim doc = System.Text.Json.JsonDocument.Parse(message)
            Dim root = doc.RootElement

            Dim typeProp As System.Text.Json.JsonElement = Nothing
            If Not root.TryGetProperty("type", typeProp) Then Return
            If typeProp.GetString() <> "kbrowser_credential_capture" Then Return

            Dim usernameProp As System.Text.Json.JsonElement = Nothing
            Dim passwordProp As System.Text.Json.JsonElement = Nothing
            Dim urlProp As System.Text.Json.JsonElement = Nothing

            If Not root.TryGetProperty("username", usernameProp) Then Return
            If Not root.TryGetProperty("password", passwordProp) Then Return
            If Not root.TryGetProperty("url", urlProp) Then Return

            Dim capturedUsername As String = usernameProp.GetString()
            Dim capturedPassword As String = passwordProp.GetString()
            Dim capturedUrl As String = urlProp.GetString()

            If String.IsNullOrWhiteSpace(capturedUsername) OrElse String.IsNullOrWhiteSpace(capturedPassword) Then Return

            ' Security: e.Source is supplied by the WebView2 runtime itself and cannot be
            ' spoofed by page content, unlike the "url" field inside the JSON payload.
            ' Any page can call window.chrome.webview.postMessage() directly (not just our
            ' injected script), so without this check a malicious site could forge a capture
            ' claiming to be a different (e.g. banking) domain and poison that domain's saved
            ' credentials. Reject the message unless the claimed url's host actually matches
            ' the document that sent it.
            If Not IsSameHost(capturedUrl, e.Source) Then
                System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Rejected credential capture — url/source host mismatch.")
                Return
            End If

            ' Check if this site is blocked
            If _passwordManager.IsBlockedSite(capturedUrl) Then Return

            RaiseEvent CredentialsCaptured(Me, New CredentialCapturedEventArgs(capturedUrl, capturedUsername, capturedPassword))
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CredentialDetectionService: Error processing web message: " & ex.Message)
        End Try
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
    ''' </summary>
    Private Shared Function GetLoginDetectionScript() As String
        Return "
(function() {
    if (window.__kbrowser_credential_detection) return;
    window.__kbrowser_credential_detection = true;

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
            window.chrome.webview.postMessage(JSON.stringify({
                type: 'kbrowser_credential_capture',
                username: username,
                password: passwordField.value,
                url: window.location.href
            }));
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
})();
"
    End Function

    ''' <summary>
    ''' Returns JavaScript that fills username and password fields on the current page.
    ''' </summary>
    Private Shared Function GetAutofillScript(ByVal username As String, ByVal password As String) As String
        Return "
(function() {
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
        nativeSetter.call(usernameField, '" & username & "');
        triggerInputEvent(usernameField);
    }

    if (passwordField) {
        var nativeSetter = Object.getOwnPropertyDescriptor(window.HTMLInputElement.prototype, 'value').set;
        nativeSetter.call(passwordField, '" & password & "');
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
