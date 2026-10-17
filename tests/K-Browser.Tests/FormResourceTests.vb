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

    <Fact>
    Public Sub Form1_HasNoRemovedServices()
        Dim text = Source("Form1.vb") & Source("Form1.Designer.vb") & Source("Form1.resx")
        Dim removed = {"Formspring", "Keek", "WebCrawler", "Lycos", "Dogpile", "Badoo"}

        Assert.Empty(removed.Where(Function(s) text.IndexOf(s, StringComparison.OrdinalIgnoreCase) >= 0))
    End Sub

    ''' <summary>Brand menu bitmaps that Task 5 replaces with favicons.</summary>
    Friend Shared ReadOnly BrandImageKeys As String() = {
        "BingToolStripMenuItem.Image", "GoogleToolStripMenuItem1.Image", "YahooToolStripMenuItem.Image",
        "EBayToolStripMenuItem.Image", "MSNToolStripMenuItem.Image", "DuckDuckGoToolStripMenuItem.Image",
        "VimeoToolStripMenuItem.Image", "FacebookToolStripMenuItem.Image", "TwitterToolStripMenuItem.Image",
        "GoogleToolStripMenuItem.Image", "LinkedinToolStripMenuItem.Image", "InstagramToolStripMenuItem.Image",
        "VkToolStripMenuItem.Image", "PinterestToolStripMenuItem.Image"}

    <Fact>
    Public Sub Form1_OnlyBrandBitmapsRemain()
        Dim leftover = ResxDataNames("Form1.resx").Where(Function(n) n.EndsWith(".Image")).Except(BrandImageKeys).ToList()

        Assert.Empty(leftover)
    End Sub

    <Fact>
    Public Sub Form1Menus_HaveNoEmojiText()
        Dim designer = Source("Form1.Designer.vb")

        Assert.DoesNotContain("🌙", designer)
        Assert.DoesNotContain("🕶", designer)
    End Sub

    <Fact>
    Public Sub PrivateBrowserForm_HasNoGlyphText()
        Dim designer = Source("PrivateBrowserForm.Designer.vb")
        Dim glyphs = {"◀", "▶", "🔄", "🏠", "→", "⭐", "🕶"}

        Assert.Empty(glyphs.Where(Function(g) designer.Contains(g)))
    End Sub

End Class
