Imports System.IO
Imports Advance_Webbrowser
Imports Xunit

''' <summary>
''' Cookie names, domains and paths are chosen by the websites. A value starting with = + - @ is run as a
''' formula when the CSV is opened in Excel, so such values must be neutralised on export.
''' </summary>
Public Class CookieCsvExportTests
    Implements IDisposable

    Private ReadOnly _file As String = Path.Combine(Path.GetTempPath(), "kbrowser-cookie-csv-" & Guid.NewGuid().ToString("N") & ".csv")

    Public Sub Dispose() Implements IDisposable.Dispose
        Try
            File.Delete(_file)
        Catch
        End Try
    End Sub

    Private Function ExportRow(cookie As CookieItem) As String
        Dim svc As New CookieExportService()
        svc.ExportToCsvAsync(New CookieItem() {cookie}, _file).GetAwaiter().GetResult()
        Dim lines = File.ReadAllLines(_file)
        Assert.Equal(2, lines.Length)
        Return lines(1)
    End Function

    Private Shared Function Cookie(Optional website As String = "example.com", Optional name As String = "sid",
                                   Optional domain As String = ".example.com", Optional path As String = "/") As CookieItem
        Return New CookieItem() With {.Website = website, .Name = name, .Domain = domain, .Path = path,
                                      .IsSecure = True, .IsHttpOnly = True, .SameSite = "Lax"}
    End Function

    <Fact>
    Public Sub OrdinaryCookie_IsExportedUnchanged()
        Assert.Equal("example.com,sid,.example.com,/,Session,True,True,Lax", ExportRow(Cookie()))
    End Sub

    <Theory>
    <InlineData("=1+1", "'=1+1")>
    <InlineData("+1", "'+1")>
    <InlineData("-2", "'-2")>
    <InlineData("@SUM(A1)", "'@SUM(A1)")>
    Public Sub NameStartingWithAFormulaCharacter_IsPrefixedWithAQuote(name As String, expected As String)
        Assert.Equal("example.com," & expected & ",.example.com,/,Session,True,True,Lax", ExportRow(Cookie(name:=name)))
    End Sub

    <Fact>
    Public Sub EveryTextColumn_IsNeutralised()
        Dim row = ExportRow(Cookie(website:="=a", name:="=b", domain:="=c", path:="=d"))
        Assert.Equal("'=a,'=b,'=c,'=d,Session,True,True,Lax", row)
    End Sub

    <Fact>
    Public Sub FormulaValue_ThatAlsoNeedsQuoting_IsStillValidCsv()
        Dim row = ExportRow(Cookie(name:="=HYPERLINK(""http://evil"",""x"")"))
        ' leading quote inside the CSV quoting, embedded quotes doubled
        Assert.StartsWith("example.com,""'=HYPERLINK(""""http://evil"""",""""x"""")"",", row)
    End Sub

    <Fact>
    Public Sub ValueThatStartsWithATabOrReturn_IsNeutralised()
        Assert.StartsWith("example.com,'" & vbTab & "x,", ExportRow(Cookie(name:=vbTab & "x")))
    End Sub

    <Fact>
    Public Sub ExistingQuotingRules_StillApply()
        Dim row = ExportRow(Cookie(name:="a,b"))
        Assert.Equal("example.com,""a,b"",.example.com,/,Session,True,True,Lax", row)
    End Sub

End Class
