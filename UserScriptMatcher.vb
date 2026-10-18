Imports System.Text
Imports System.Text.Json
Imports System.Text.RegularExpressions

''' <summary>
''' Decides which pages a user script (@match / @include header lines) applies to, and wraps the script so
''' that it only runs there. The engine injects every registered script into every document it creates, so
''' without this check a script written for one site runs on all of them, including banking sites.
'''
''' Each pattern becomes one regular expression that behaves the same in .NET (used by the tests and
''' <see cref="Matches"/>) and in the injected JavaScript (tested against location.href).
''' </summary>
Public Class UserScriptMatcher

    ' scheme://host/path  — the browser-extension "match pattern" shape used by @match.
    Private Shared ReadOnly MatchPatternShape As New Regex("^(\*|https?)://([^/]+)(/.*)$",
                                                           RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)
    Private Shared ReadOnly PortSuffix As New Regex(":([0-9]{1,5})$", RegexOptions.CultureInvariant)
    Private Shared ReadOnly HeaderBlock As New Regex("//[ \t]*==UserScript==([\s\S]*?)//[ \t]*==/UserScript==",
                                                     RegexOptions.CultureInvariant Or RegexOptions.IgnoreCase)
    Private Shared ReadOnly DirectiveLine As New Regex("^\s*//\s*@([A-Za-z][A-Za-z0-9\-]*)[ \t]+(\S.*?)\s*$", RegexOptions.CultureInvariant)

    ''' <summary>
    ''' Converts an @match / @include value to a regular expression source string, or Nothing when the
    ''' pattern is empty or not supported (such patterns match nothing).
    ''' </summary>
    Public Shared Function ToRegexSource(ByVal pattern As String) As String
        If String.IsNullOrWhiteSpace(pattern) Then Return Nothing
        Dim p As String = pattern.Trim()

        If p.Equals("<all_urls>", StringComparison.OrdinalIgnoreCase) Then Return "^https?://[^/@:]+(:[0-9]+)?/.*(#.*)?$"

        ' /regex/ literals are not supported; no URL starts with "/" either.
        If p.StartsWith("/", StringComparison.Ordinal) Then Return Nothing

        Dim m As Match = MatchPatternShape.Match(p)
        If m.Success Then Return FromMatchPattern(m.Groups(1).Value, m.Groups(2).Value, m.Groups(3).Value)

        Return "^" & EscapeWithWildcards(p) & "$"
    End Function

    Private Shared Function FromMatchPattern(ByVal scheme As String, ByVal hostAndPort As String, ByVal path As String) As String
        Dim schemeSource As String = If(scheme = "*", "https?", scheme.ToLowerInvariant())

        Dim host As String = hostAndPort.ToLowerInvariant()
        Dim portSource As String = "(:[0-9]+)?"
        Dim portMatch As Match = PortSuffix.Match(host)
        If portMatch.Success Then
            host = host.Substring(0, portMatch.Index)
            portSource = ":" & portMatch.Groups(1).Value
        End If

        Dim hostSource As String
        If host = "*" Then
            hostSource = "[^/@:]+"
        ElseIf host.StartsWith("*.", StringComparison.Ordinal) Then
            Dim rest As String = host.Substring(2)
            If rest.Contains("*") Then Return Nothing
            hostSource = "([^/@:]+\.)?" & Escape(rest)
        Else
            If host.Contains("*") OrElse host.Length = 0 Then Return Nothing
            hostSource = Escape(host)
        End If

        Return "^" & schemeSource & "://" & hostSource & portSource & EscapeWithWildcards(path) & "(#.*)?$"
    End Function

    ''' <summary>Escapes regex metacharacters; letters, digits, underscore and non-ASCII pass through.</summary>
    Private Shared Function Escape(ByVal text As String) As String
        Dim sb As New StringBuilder(text.Length + 8)
        For Each c As Char In text
            If Char.IsLetterOrDigit(c) OrElse c = "_"c OrElse AscW(c) > 127 Then
                sb.Append(c)
            Else
                sb.Append("\"c).Append(c)
            End If
        Next
        Return sb.ToString()
    End Function

    Private Shared Function EscapeWithWildcards(ByVal text As String) As String
        Return String.Join(".*", text.Split("*"c).Select(AddressOf Escape))
    End Function

    Private Shared Function ToRegexSources(ByVal patterns As IEnumerable(Of String)) As List(Of String)
        Dim sources As New List(Of String)()
        If patterns IsNot Nothing Then
            For Each pattern As String In patterns
                Dim source As String = ToRegexSource(pattern)
                If source IsNot Nothing Then sources.Add(source)
            Next
        End If
        Return sources
    End Function

    ''' <summary>
    ''' True when <paramref name="url"/> matches at least one pattern. No usable pattern matches nothing.
    ''' </summary>
    Public Shared Function Matches(ByVal patterns As IEnumerable(Of String), ByVal url As String) As Boolean
        If patterns Is Nothing OrElse String.IsNullOrWhiteSpace(url) Then Return False

        Dim uri As Uri = Nothing
        If Not Uri.TryCreate(url, UriKind.Absolute, uri) Then Return False
        ' The browser reports location.href in this normalised form (lower-case scheme and host).
        Dim href As String = uri.AbsoluteUri

        For Each pattern As String In patterns
            Dim source As String = ToRegexSource(pattern)
            If source Is Nothing Then Continue For
            Try
                If Regex.IsMatch(href, source, RegexOptions.CultureInvariant, TimeSpan.FromMilliseconds(100)) Then Return True
            Catch ex As RegexMatchTimeoutException
                ' A pattern that cannot be evaluated in time does not match.
            End Try
        Next
        Return False
    End Function

    ''' <summary>
    ''' Reads the // ==UserScript== block of a script. Directives outside that block are ignored.
    ''' </summary>
    Public Shared Function ParseHeader(ByVal scriptText As String) As UserScriptHeader
        Dim header As New UserScriptHeader()
        If String.IsNullOrEmpty(scriptText) Then Return header

        Dim block As Match = HeaderBlock.Match(scriptText)
        If Not block.Success Then Return header

        Dim patterns As New List(Of String)()
        Dim excludes As New List(Of String)()
        For Each line As String In block.Groups(1).Value.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
            Dim directive As Match = DirectiveLine.Match(line)
            If Not directive.Success Then Continue For

            Dim value As String = directive.Groups(2).Value
            Select Case directive.Groups(1).Value.ToLowerInvariant()
                Case "name"
                    If header.Name.Length = 0 Then header.Name = value
                Case "match", "include"
                    patterns.Add(value)
                Case "exclude", "exclude-match"
                    excludes.Add(value)
            End Select
        Next
        header.Patterns = patterns.ToArray()
        header.Excludes = excludes.ToArray()
        Return header
    End Function

    ''' <summary>
    ''' True when the page matches an include pattern and none of the exclude patterns.
    ''' </summary>
    Public Shared Function IsApplicable(ByVal includes As IEnumerable(Of String), ByVal excludes As IEnumerable(Of String), ByVal url As String) As Boolean
        Return Matches(includes, url) AndAlso Not Matches(excludes, url)
    End Function

    ''' <summary>
    ''' Wraps a user script so it runs only on pages matching <paramref name="patterns"/>. The wrapper tests
    ''' location.href itself, because the engine injects the script into every document. The script name is
    ''' embedded as a JSON string so it cannot break out of the wrapper.
    ''' </summary>
    Public Shared Function BuildWrappedCode(ByVal name As String, ByVal code As String, ByVal patterns As IEnumerable(Of String),
                                            Optional ByVal excludes As IEnumerable(Of String) = Nothing) As String
        Dim sources As List(Of String) = ToRegexSources(patterns)
        Dim excludeSources As List(Of String) = ToRegexSources(excludes)

        Dim sb As New StringBuilder()
        sb.AppendLine("(function() {")
        sb.AppendLine("  var patterns = " & JsonSerializer.Serialize(sources) & ";")
        sb.AppendLine("  var excludes = " & JsonSerializer.Serialize(excludeSources) & ";")
        sb.AppendLine("  function any(list) {")
        sb.AppendLine("    for (var i = 0; i < list.length; i++) {")
        sb.AppendLine("      try { if (new RegExp(list[i]).test(location.href)) return true; } catch (e) { }")
        sb.AppendLine("    }")
        sb.AppendLine("    return false;")
        sb.AppendLine("  }")
        sb.AppendLine("  var matched = any(patterns);")
        sb.AppendLine("  var excluded = any(excludes);")
        sb.AppendLine("  if (!matched || excluded) return;")
        sb.AppendLine("  try {")
        sb.AppendLine(code)
        ' On its own line: a script that ends in a // comment must not swallow the closing brace.
        sb.AppendLine("  } catch (e) { console.error('K-Browser UserScript [' + " & JsonSerializer.Serialize(If(name, String.Empty)) & " + '] error:', e); }")
        sb.Append("})();")
        Return sb.ToString()
    End Function

End Class

''' <summary>The parts of a // ==UserScript== block this browser acts on.</summary>
Public Class UserScriptHeader
    Public Property Name As String = ""
    Public Property Patterns As String() = Array.Empty(Of String)()
    Public Property Excludes As String() = Array.Empty(Of String)()
End Class
