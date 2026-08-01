Imports System.Collections.Generic
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Threading.Tasks
Imports Microsoft.Web.WebView2.WinForms

Public Class UserScriptManager

    Public Class UserScript
        Public Property Name As String = ""
        Public Property Code As String = ""
        Public Property MatchPatterns As New List(Of String)()
    End Class

    Private Shared ReadOnly Scripts As New List(Of UserScript)()
    Private Shared UserScriptsDir As String = ""

    ''' <summary>
    ''' Initializes UserScript directory and loads all local .user.js scripts.
    ''' </summary>
    Public Shared Sub Initialize()
        Try
            UserScriptsDir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "UserScripts")
            If Not Directory.Exists(UserScriptsDir) Then
                Directory.CreateDirectory(UserScriptsDir)
                CreateSampleUserScript()
            End If

            ReloadUserScripts()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error initializing UserScriptManager: " & ex.Message)
        End Try
    End Sub

    Public Shared Sub ReloadUserScripts()
        Scripts.Clear()
        If Not Directory.Exists(UserScriptsDir) Then
            Return
        End If

        For Each filePath As String In Directory.GetFiles(UserScriptsDir, "*.user.js", SearchOption.AllDirectories)
            Try
                Dim scriptText As String = File.ReadAllText(filePath)
                Dim script As New UserScript With {.Code = scriptText, .Name = Path.GetFileNameWithoutExtension(filePath)}

                ' Parse Greasemonkey / Tampermonkey header metadata
                Dim headerMatch As Match = Regex.Match(scriptText, "//\s*==UserScript==([\s\S]*?)//\s*==/UserScript==", RegexOptions.IgnoreCase)
                If headerMatch.Success Then
                    Dim headerContent As String = headerMatch.Groups(1).Value
                    For Each line As String In headerContent.Split(New String() {vbCrLf, vbLf}, StringSplitOptions.RemoveEmptyEntries)
                        If line.Contains("@name") Then
                            Dim parts = line.Split(New Char() {" "c, vbTab}, StringSplitOptions.RemoveEmptyEntries)
                            If parts.Length >= 2 Then
                                script.Name = String.Join(" ", parts, 1, parts.Length - 1)
                            End If
                        ElseIf line.Contains("@match") OrElse line.Contains("@include") Then
                            Dim parts = line.Split(New Char() {" "c, vbTab}, StringSplitOptions.RemoveEmptyEntries)
                            If parts.Length >= 2 Then
                                script.MatchPatterns.Add(parts(1).Trim())
                            End If
                        End If
                    Next
                End If

                If script.MatchPatterns.Count = 0 Then
                    script.MatchPatterns.Add("*://*/*") ' Default fallback
                End If

                Scripts.Add(script)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error parsing UserScript " & filePath & ": " & ex.Message)
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Registers all loaded UserScripts to automatically execute when documents are created inside WebView2.
    ''' </summary>
    Public Shared Async Function RegisterScriptsForTabAsync(ByVal brws As WebView2) As Task
        If brws Is Nothing OrElse brws.CoreWebView2 Is Nothing Then
            Return
        End If

        For Each script In Scripts
            Try
                ' Add script to execute on document creation
                Dim wrappedCode As String = "(function() { try { " & script.Code & " } catch(e) { console.error('K-Browser UserScript [" & script.Name & "] error:', e); } })();"
                Await brws.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(wrappedCode)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error injecting UserScript [" & script.Name & "]: " & ex.Message)
            End Try
        Next
    End Function

    Private Shared Sub CreateSampleUserScript()
        Try
            Dim samplePath As String = Path.Combine(UserScriptsDir, "SampleCustomStyle.user.js")
            Dim sampleContent As String = "// ==UserScript==" & vbCrLf &
                                         "// @name         K-Browser Custom UserScript Helper" & vbCrLf &
                                         "// @match        *://*/*" & vbCrLf &
                                         "// ==/UserScript==" & vbCrLf & vbCrLf &
                                         "console.log('⚡ K-Browser UserScript Engine loaded successfully!');"
            File.WriteAllText(samplePath, sampleContent)
        Catch
        End Try
    End Sub

End Class
