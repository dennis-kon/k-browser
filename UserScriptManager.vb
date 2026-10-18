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
        Public Property Excludes As New List(Of String)()
        ''' <summary>The code injected into pages: the script wrapped in its @match / @exclude check.</summary>
        Public Property WrappedCode As String = ""
    End Class

    Private Shared ReadOnly LoadLock As New Object()
    Private Shared _loading As Task(Of IReadOnlyList(Of UserScript))

    Public Shared ReadOnly Property UserScriptsDir As String
        Get
            Return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "K-Browser", "UserScripts")
        End Get
    End Property

    ''' <summary>
    ''' Starts reading the user scripts once per run, on a background thread, and returns that load.
    ''' Later calls (other windows, every new tab) get the same result without touching the disk again.
    ''' </summary>
    Public Shared Function EnsureLoadedAsync() As Task(Of IReadOnlyList(Of UserScript))
        SyncLock LoadLock
            If _loading Is Nothing Then
                _loading = Task.Run(Function()
                                        Try
                                            If Not Directory.Exists(UserScriptsDir) Then
                                                Directory.CreateDirectory(UserScriptsDir)
                                                CreateSampleUserScript()
                                            End If
                                        Catch ex As Exception
                                            System.Diagnostics.Debug.WriteLine("Error initializing UserScriptManager: " & ex.Message)
                                        End Try
                                        Return LoadScripts(UserScriptsDir)
                                    End Function)
            End If
            Return _loading
        End SyncLock
    End Function

    ''' <summary>
    ''' Reads every *.user.js file under <paramref name="folder"/>, parses its Greasemonkey / Tampermonkey header
    ''' and builds the code that will be injected into pages. A file that cannot be read is skipped.
    ''' </summary>
    Public Shared Function LoadScripts(ByVal folder As String) As IReadOnlyList(Of UserScript)
        Dim scripts As New List(Of UserScript)()
        If String.IsNullOrEmpty(folder) OrElse Not Directory.Exists(folder) Then Return scripts

        For Each filePath As String In Directory.GetFiles(folder, "*.user.js", SearchOption.AllDirectories)
            Try
                Dim scriptText As String = File.ReadAllText(filePath)
                Dim script As New UserScript With {.Code = scriptText, .Name = Path.GetFileNameWithoutExtension(filePath)}

                Dim header As UserScriptHeader = UserScriptMatcher.ParseHeader(scriptText)
                If header.Name.Length > 0 Then script.Name = header.Name
                script.MatchPatterns.AddRange(header.Patterns)
                script.Excludes.AddRange(header.Excludes)

                If script.MatchPatterns.Count = 0 Then
                    script.MatchPatterns.Add("*://*/*") ' Default fallback
                End If

                ' The engine injects this into EVERY document, so the wrapper itself checks the page
                ' address against the script's @match / @include patterns.
                script.WrappedCode = UserScriptMatcher.BuildWrappedCode(script.Name, script.Code, script.MatchPatterns, script.Excludes)
                scripts.Add(script)
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error parsing UserScript " & filePath & ": " & ex.Message)
            End Try
        Next
        Return scripts
    End Function

    ''' <summary>
    ''' Registers all loaded UserScripts to automatically execute when documents are created inside WebView2.
    ''' Waits for the first load if it is still running, so the tab's first page gets the scripts too.
    ''' </summary>
    Public Shared Async Function RegisterScriptsForTabAsync(ByVal brws As WebView2) As Task
        If brws Is Nothing OrElse brws.CoreWebView2 Is Nothing Then
            Return
        End If

        Dim scripts As IReadOnlyList(Of UserScript)
        Try
            scripts = Await EnsureLoadedAsync()
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading UserScripts: " & ex.Message)
            Return
        End Try
        If brws.CoreWebView2 Is Nothing Then Return

        For Each script In scripts
            Try
                Await brws.CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync(script.WrappedCode)
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
                                         "console.log('K-Browser UserScript Engine loaded successfully!');"
            File.WriteAllText(samplePath, sampleContent)
        Catch
        End Try
    End Sub

End Class
