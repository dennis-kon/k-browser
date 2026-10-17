Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Xml.Linq
Imports Xunit

Public Class ResourceConsistencyTests

    <Fact>
    Public Sub EveryDesignerGetObjectHasResxEntry()
        Dim missing As New List(Of String)
        Dim getObject As New Regex("resources\.GetObject\(""([^""]+)""\)")

        For Each designer In Directory.GetFiles(RepoRoot, "*.vb")
            Dim name = Path.GetFileName(designer)
            If Not name.EndsWith(".designer.vb", StringComparison.OrdinalIgnoreCase) Then Continue For
            If name.StartsWith("Form4", StringComparison.OrdinalIgnoreCase) Then Continue For

            Dim keys = getObject.Matches(File.ReadAllText(designer)).Select(Function(m) m.Groups(1).Value).ToList()
            If keys.Count = 0 Then Continue For

            Dim resx = Path.Combine(RepoRoot, name.Substring(0, name.Length - ".designer.vb".Length) & ".resx")
            Dim dataNames As New HashSet(Of String)
            If File.Exists(resx) Then
                For Each dataElement In XDocument.Load(resx).Root.Elements("data")
                    dataNames.Add(CStr(dataElement.Attribute("name")))
                Next
            End If

            For Each key In keys
                If Not dataNames.Contains(key) Then missing.Add(name & ": " & key)
            Next
        Next

        Assert.Empty(missing)
    End Sub

End Class
