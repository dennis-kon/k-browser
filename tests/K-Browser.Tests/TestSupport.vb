Imports System.IO
Imports System.Runtime.ExceptionServices
Imports System.Threading

Public Module TestSupport

    ''' <summary>
    ''' The repository root: the nearest ancestor of the test output folder that contains k-browser.sln.
    ''' </summary>
    Public ReadOnly Property RepoRoot As String
        Get
            Dim dir = New DirectoryInfo(AppContext.BaseDirectory)
            While dir IsNot Nothing
                If File.Exists(Path.Combine(dir.FullName, "k-browser.sln")) Then Return dir.FullName
                dir = dir.Parent
            End While
            Throw New InvalidOperationException("k-browser.sln not found above " & AppContext.BaseDirectory)
        End Get
    End Property

    ''' <summary>
    ''' Runs the action on a fresh STA thread (WinForms controls expect one) and rethrows its exception.
    ''' </summary>
    Public Sub RunSta(action As Action)
        Dim captured As ExceptionDispatchInfo = Nothing
        Dim t As New Thread(Sub()
                                Try
                                    action()
                                Catch ex As Exception
                                    captured = ExceptionDispatchInfo.Capture(ex)
                                End Try
                            End Sub)
        t.SetApartmentState(ApartmentState.STA)
        t.Start()
        t.Join()
        captured?.Throw()
    End Sub

End Module
