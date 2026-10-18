Imports System.IO
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' Settings.json is read on every page load (password detection) and for every new tab. It is kept in memory
''' and only read from disk again when the file changes, and every caller still gets its own copy.
''' </summary>
Public Class SettingsServiceCacheTests
    Implements IDisposable

    Private ReadOnly _dir As String = Path.Combine(Path.GetTempPath(), "kb-settings-" & Guid.NewGuid().ToString("N"))
    Private ReadOnly _file As String

    Public Sub New()
        Directory.CreateDirectory(_dir)
        _file = Path.Combine(_dir, "Settings.json")
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            Directory.Delete(_dir, True)
        Catch
        End Try
    End Sub

    Private Function Saved(startupMode As String) As SettingsService
        Dim svc As New SettingsService(_file)
        svc.SaveSettings(New PrivacySettingsModel With {.StartupMode = startupMode})
        Return svc
    End Function

    <Fact>
    Public Sub UnchangedFile_IsNotReadFromDiskAgain()
        Dim svc = Saved("RestoreSession")
        Assert.Equal("RestoreSession", svc.LoadSettings().StartupMode)

        ' While another program holds the file open exclusively, a disk read would fail.
        Using New FileStream(_file, FileMode.Open, FileAccess.ReadWrite, FileShare.None)
            Assert.Equal("RestoreSession", svc.LoadSettings().StartupMode)
            Assert.Equal("RestoreSession", New SettingsService(_file).LoadSettingsAsync().GetAwaiter().GetResult().StartupMode)
        End Using
    End Sub

    <Fact>
    Public Sub FileChangedOnDisk_IsReadAgain()
        Dim svc = Saved("RestoreSession")
        svc.LoadSettings()

        File.WriteAllText(_file, "{""StartupMode"":""NewTab""}")
        File.SetLastWriteTimeUtc(_file, DateTime.UtcNow.AddMinutes(1))

        Assert.Equal("NewTab", svc.LoadSettings().StartupMode)
    End Sub

    <Fact>
    Public Sub EachCaller_GetsItsOwnCopy()
        Dim svc = Saved("RestoreSession")
        Dim first = svc.LoadSettings()
        first.StartupMode = "changed but not saved"
        Assert.Equal("RestoreSession", svc.LoadSettings().StartupMode)
    End Sub

    <Fact>
    Public Sub AsyncSave_IsSeenByTheNextLoad()
        Dim svc = Saved("RestoreSession")
        svc.LoadSettings()
        svc.SaveSettingsAsync(New PrivacySettingsModel With {.StartupMode = "NewTab"}).GetAwaiter().GetResult()
        Assert.Equal("NewTab", svc.LoadSettings().StartupMode)
    End Sub

    <Fact>
    Public Sub MissingFile_GivesDefaults()
        Dim svc As New SettingsService(Path.Combine(_dir, "none.json"))
        Assert.Equal("NewTab", svc.LoadSettings().StartupMode)
    End Sub

End Class
