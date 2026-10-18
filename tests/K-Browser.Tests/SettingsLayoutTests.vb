Imports System.Threading
Imports System.Windows.Forms
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' The Settings window's Performance and Browser tabs: Memory Saver and the new-tab choice are real controls,
''' and the Performance groups do not overlap.
''' </summary>
Public Class SettingsLayoutTests

    Private Shared Function C(f As Form, name As String) As Control
        Dim found = f.Controls.Find(name, True)
        Assert.True(found.Length = 1, "control not found: " & name)
        Return found(0)
    End Function

    Private Shared Sub OnSta(action As Action(Of Settings))
        Dim failure As Exception = Nothing
        Dim t As New Thread(Sub()
                                Try
                                    Using f As New Settings()
                                        action(f)
                                    End Using
                                Catch ex As Exception
                                    failure = ex
                                End Try
                            End Sub)
        t.SetApartmentState(ApartmentState.STA)
        t.Start()
        t.Join()
        If failure IsNot Nothing Then Throw New Exception("Settings form check failed", failure)
    End Sub

    <Fact>
    Public Sub MemorySaver_IsOnThePerformanceTab()
        OnSta(Sub(f)
                  Assert.Same(C(f, "grpTabs"), C(f, "chkMemorySaver").Parent)
                  Assert.True(C(f, "chkMemorySaver").Bottom <= C(f, "grpTabs").Height)
              End Sub)
    End Sub

    <Fact>
    Public Sub PerformanceGroups_DoNotOverlap()
        OnSta(Sub(f)
                  Assert.True(C(f, "grpGraphics").Bottom <= C(f, "grpTabs").Top)
                  Assert.True(C(f, "grpTabs").Bottom <= C(f, "grpPageLoading").Top)
                  Assert.True(C(f, "grpPageLoading").Bottom <= C(f, "tpPerformance").Height)
              End Sub)
    End Sub

    <Fact>
    Public Sub NewTabChoice_IsOnTheBrowserTab()
        OnSta(Sub(f)
                  Assert.Same(C(f, "tpBrowser"), C(f, "cmbNewTab").Parent)
                  Assert.Equal(2, DirectCast(C(f, "cmbNewTab"), ComboBox).Items.Count)
                  Assert.True(C(f, "cmbNewTab").Bottom <= C(f, "tpBrowser").Height)
              End Sub)
    End Sub

    <Fact>
    Public Sub PreloadPages_SaysItNeedsARestart()
        OnSta(Sub(f) Assert.Contains("Restart", C(f, "lblPageLoadingStatus").Text))
    End Sub

End Class
