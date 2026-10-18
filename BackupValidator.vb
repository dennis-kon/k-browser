Imports System.IO
Imports System.Text.Json
Imports System.Text.RegularExpressions

''' <summary>
''' Checks the contents of a backup ZIP before they are applied. A backup is a file the user is handed, so a
''' restore must not be able to switch off browser protections with malformed values, send downloads to a
''' network share, or start the browser on a script URL. Anything that does not validate is dropped and the
''' current setting is kept.
''' </summary>
Public Class BackupValidator

    Private Shared ReadOnly DangerousScheme As New Regex("^\s*(javascript|data|vbscript|blob|mailto|search-ms|ms-[a-z0-9\-]+)\s*:",
                                                         RegexOptions.IgnoreCase Or RegexOptions.CultureInvariant)
    Private Shared ReadOnly DriveRoot As New Regex("^[A-Za-z]:[\\/]", RegexOptions.CultureInvariant)

    Private Shared ReadOnly SearchEngines As String() = {"Google", "Bing", "DuckDuckGo", "Yahoo"}

    Private Shared ReadOnly BooleanKeys As String() = {
        "FullScreenOnStartup", "PermissionCamera", "PermissionMic", "PermissionLocation", "PermissionNotifications",
        "IncognitoEnabled", "HttpsOnlyMode", "AdBlockerEnabled", "MemorySaverEnabled", "HardwareAcceleration"}

    ''' <summary>
    ''' True for an address a tab or the start page may be opened on: http(s), file, or about:blank.
    ''' </summary>
    Public Shared Function IsAllowedStartUrl(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) Then Return False
        Dim text As String = url.Trim()
        If text.Equals("about:blank", StringComparison.OrdinalIgnoreCase) Then Return True

        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(text, UriKind.Absolute, uri) Then Return False
        Return uri.Scheme = Uri.UriSchemeHttp OrElse uri.Scheme = Uri.UriSchemeHttps OrElse uri.Scheme = Uri.UriSchemeFile
    End Function

    ''' <summary>
    ''' The home page may also be a bare host name ("example.com", "localhost:3000"), which the browser
    ''' turns into an http address.
    ''' </summary>
    Private Shared Function IsAllowedHomePage(ByVal value As String) As Boolean
        If String.IsNullOrWhiteSpace(value) Then Return False
        Dim text As String = value.Trim()
        If text.Contains("://") OrElse text.StartsWith("about:", StringComparison.OrdinalIgnoreCase) Then Return IsAllowedStartUrl(text)
        If DangerousScheme.IsMatch(text) Then Return False

        Dim uri As Uri = Nothing
        Return Uri.TryCreate("http://" & text, UriKind.Absolute, uri) AndAlso uri.Host.Length > 0
    End Function

    ''' <summary>
    ''' True for a local drive path ("C:\Users\me\Downloads"). Network shares and relative paths are refused:
    ''' a restored download folder on someone else's server would receive every file the user downloads.
    ''' </summary>
    Private Shared Function IsLocalAbsolutePath(ByVal value As String) As Boolean
        If String.IsNullOrWhiteSpace(value) Then Return False
        Dim text As String = value.Trim()
        If text.Length > 240 OrElse Not DriveRoot.IsMatch(text) Then Return False
        If text.IndexOfAny(Path.GetInvalidPathChars()) >= 0 OrElse text.IndexOfAny(New Char() {"?"c, "*"c}) >= 0 Then Return False
        Return True
    End Function

    ''' <summary>
    ''' Returns only the entries of a restored user_config.json that are valid, with values normalised so the
    ''' caller can parse them without risk. Unknown keys are dropped.
    ''' </summary>
    Public Shared Function SanitizeUserConfig(ByVal config As IDictionary(Of String, String)) As Dictionary(Of String, String)
        Dim clean As New Dictionary(Of String, String)(StringComparer.Ordinal)
        If config Is Nothing Then Return clean

        For Each pair As KeyValuePair(Of String, String) In config
            Dim key As String = pair.Key
            Dim value As String = pair.Value

            Select Case key
                Case "HomePageUrl"
                    If IsAllowedHomePage(value) Then clean(key) = value.Trim()

                Case "SearchEngine"
                    Dim engine As String = SearchEngines.FirstOrDefault(Function(e) e.Equals(value, StringComparison.OrdinalIgnoreCase))
                    If engine IsNot Nothing Then clean(key) = engine

                Case "DownloadsFolder"
                    If IsLocalAbsolutePath(value) Then clean(key) = value.Trim()

                Case "FontSize"
                    Dim size As Integer
                    If Integer.TryParse(value, System.Globalization.NumberStyles.None, System.Globalization.CultureInfo.InvariantCulture, size) AndAlso size >= 0 AndAlso size <= 3 Then
                        clean(key) = size.ToString(System.Globalization.CultureInfo.InvariantCulture)
                    End If

                Case Else
                    If BooleanKeys.Contains(key) Then
                        Dim flag As Boolean
                        If Boolean.TryParse(value, flag) Then clean(key) = If(flag, "True", "False")
                    End If
            End Select
        Next
        Return clean
    End Function

    ''' <summary>
    ''' Returns a copy of a restored session with only the tabs whose address is allowed, keeping the
    ''' selected tab selected when it survives and otherwise clamping the selection into range.
    ''' </summary>
    Public Shared Function FilterSession(ByVal session As SessionModel) As SessionModel
        Dim cleaned As New SessionModel()
        If session Is Nothing OrElse session.Tabs Is Nothing Then Return cleaned

        Dim selected As Integer = session.SelectedTab
        For index As Integer = 0 To session.Tabs.Count - 1
            Dim tab As TabItemModel = session.Tabs(index)
            If tab Is Nothing OrElse Not IsAllowedStartUrl(tab.Url) Then Continue For
            If index = selected Then cleaned.SelectedTab = cleaned.Tabs.Count
            cleaned.Tabs.Add(tab)
        Next

        cleaned.SelectedTab = Math.Max(0, Math.Min(cleaned.SelectedTab, cleaned.Tabs.Count - 1))
        Return cleaned
    End Function

    ''' <summary>True when the text is a Settings.json this browser could have written.</summary>
    Public Shared Function IsValidSettingsJson(ByVal json As String) As Boolean
        If String.IsNullOrWhiteSpace(json) Then Return False
        Try
            Dim model As PrivacySettingsModel = JsonSerializer.Deserialize(Of PrivacySettingsModel)(json, New JsonSerializerOptions With {.PropertyNameCaseInsensitive = True})
            If model Is Nothing Then Return False
            Return model.StartupMode = "NewTab" OrElse model.StartupMode = "RestoreSession"
        Catch ex As JsonException
            Return False
        End Try
    End Function

End Class
