Imports System.IO
Imports System.Xml.Linq
Imports Xunit

''' <summary>
''' Source-level checks that forms no longer carry old bitmap or emoji icons.
''' </summary>
Public Class FormResourceTests

    Friend Shared Function ResxDataNames(resxFile As String) As HashSet(Of String)
        Return New HashSet(Of String)(
            XDocument.Load(Path.Combine(RepoRoot, resxFile)).Root.Elements("data").Select(Function(d) CStr(d.Attribute("name"))))
    End Function

    Friend Shared Function Source(file As String) As String
        Return IO.File.ReadAllText(Path.Combine(RepoRoot, file))
    End Function

    <Fact>
    Public Sub Form1Toolbar_HasNoBitmaps()
        Dim toolbarKeys = {"Back.Image", "ToolStripButton2.Image", "ToolStripButton3.Image", "ToolStripButton4.Image",
                           "ToolStripButton5.Image", "ToolStripButton6.Image", "ToolStripButton7.Image",
                           "ToolStripButton8.Image", "ToolStripButton9.Image"}

        Assert.Empty(toolbarKeys.Where(Function(k) ResxDataNames("Form1.resx").Contains(k)))
    End Sub

    <Fact>
    Public Sub Form1Toolbar_HasNoEmojiText()
        Dim lines = (Source("Form1.Designer.vb") & vbLf & Source("Form1.vb")).Split(ChrW(10))
        Dim offending = lines.Where(Function(l) (l.Contains("tsbAdBlockBadge.Text") OrElse l.Contains("btnPrivateMode.Text")) AndAlso
                                                (l.Contains("🛡") OrElse l.Contains("🕶"))).ToList()

        Assert.Empty(offending)
    End Sub

End Class
