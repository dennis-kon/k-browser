Imports System.IO
Imports System.Text.RegularExpressions
Imports Xunit

''' <summary>
''' App-wide guards: no emoji/arrow glyphs used as icons in UI strings, and no old bitmap icons in forms.
''' </summary>
Public Class AppWideIconTests

    Private Shared ReadOnly StringLiteral As New Regex("""(?:[^""]|"""")*""")

    Private Shared Function IsIconGlyph(text As String, index As Integer) As Boolean
        Dim c = AscW(text(index)) And &HFFFF
        Return Char.IsSurrogate(text(index)) OrElse
               (c >= &H2190 AndAlso c <= &H21FF) OrElse
               (c >= &H2300 AndAlso c <= &H23FF) OrElse
               (c >= &H25A0 AndAlso c <= &H25FF) OrElse
               (c >= &H2600 AndAlso c <= &H27BF) OrElse
               (c >= &H2B00 AndAlso c <= &H2BFF) OrElse
               c = &HFE0F
    End Function

    ''' <summary>
    ''' Glyphs that are deliberate text, not icons: the sleeping-tab marker in tab captions (a TabControl
    ''' caption can't show an image without adding an image list to the tab strip).
    ''' </summary>
    Private Shared ReadOnly AllowedGlyphStrings As String() = {"TabLifecycleManager.vb: ""💤 """}

    <Fact>
    Public Sub NoEmojiInUiStrings()
        Dim found As New List(Of String)
        For Each file In Directory.GetFiles(RepoRoot, "*.vb")
            Dim name = Path.GetFileName(file)
            If name.StartsWith("Form4", StringComparison.OrdinalIgnoreCase) Then Continue For
            Dim lines = IO.File.ReadAllLines(file)
            For i = 0 To lines.Length - 1
                If lines(i).TrimStart().StartsWith("'") Then Continue For
                For Each m As Match In StringLiteral.Matches(lines(i))
                    If Enumerable.Range(0, m.Value.Length).Any(Function(k) IsIconGlyph(m.Value, k)) AndAlso
                       Not AllowedGlyphStrings.Contains(name & ": " & m.Value) Then
                        found.Add($"{name}:{i + 1}: {m.Value}")
                    End If
                Next
            Next
        Next

        Assert.Empty(found)
    End Sub

    ''' <summary>Bitmaps that are artwork, not icons: the K-Browser logo in the About box.</summary>
    Private Shared ReadOnly AllowedBitmaps As String() = {"AboutBox.resx: PictureBox1.Image"}

    <Fact>
    Public Sub NoBitmapIconsRemainInResx()
        Dim found As New List(Of String)
        For Each resx In Directory.GetFiles(RepoRoot, "*.resx")
            Dim name = Path.GetFileName(resx)
            If name.StartsWith("Form4", StringComparison.OrdinalIgnoreCase) Then Continue For
            For Each key In FormResourceTests.ResxDataNames(name)
                If key.EndsWith(".Image") OrElse key.EndsWith(".BackgroundImage") OrElse key.EndsWith(".ImageStream") Then
                    found.Add(name & ": " & key)
                End If
            Next
        Next

        Assert.Empty(found.Except(AllowedBitmaps))
    End Sub

End Class
