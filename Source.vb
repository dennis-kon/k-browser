Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Public Class Source

    Private _currentSearchIndex As Integer = 0
    Private _lastSearchQuery As String = ""

    Public Sub SetSourceData(ByVal htmlText As String, Optional ByVal pageUrl As String = "", Optional ByVal pageTitle As String = "")
        RichTextBox1.Text = If(htmlText, "")

        If Not String.IsNullOrWhiteSpace(pageTitle) OrElse Not String.IsNullOrWhiteSpace(pageUrl) Then
            Me.Text = $"Page Source: {pageTitle} ({pageUrl}) — K-Browser"
        Else
            Me.Text = "Page Source Viewer — K-Browser"
        End If

        UpdateStats()
    End Sub

    Private Sub Source_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ThemeManager.ApplyTheme(Me)

        ' Apply Monospace font for sharp code readability
        Try
            RichTextBox1.Font = New Font("Consolas", 10.0F, FontStyle.Regular)
        Catch
            RichTextBox1.Font = New Font("Courier New", 10.0F, FontStyle.Regular)
        End Try

        ' Set custom tab stops for code indentation
        RichTextBox1.SelectionTabs = New Integer() {16, 32, 48, 64, 80}
        UpdateStats()
    End Sub

    Private Sub UpdateStats()
        Dim lineCount As Integer = RichTextBox1.Lines.Length
        Dim charCount As Integer = RichTextBox1.TextLength
        lblStats.Text = $"{lineCount:N0} lines  |  {charCount:N0} characters"
        UpdateLineCol()
    End Sub

    Private Sub RichTextBox1_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RichTextBox1.SelectionChanged
        UpdateLineCol()
    End Sub

    Private Sub UpdateLineCol()
        If RichTextBox1.TextLength = 0 Then
            lblLineCol.Text = "Ln 1, Col 1    "
            Return
        End If

        Dim charIndex As Integer = RichTextBox1.SelectionStart
        Dim lineIndex As Integer = RichTextBox1.GetLineFromCharIndex(charIndex)
        Dim lineStartCharIndex As Integer = RichTextBox1.GetFirstCharIndexFromLine(lineIndex)
        Dim columnIndex As Integer = charIndex - lineStartCharIndex + 1

        lblLineCol.Text = $"Ln {lineIndex + 1}, Col {columnIndex}    "
    End Sub

    ' ── Search & Navigation ───────────────────────────────────────────────────

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtSearch.TextChanged
        _currentSearchIndex = 0
        PerformSearch(forward:=True)
    End Sub

    Private Sub btnFindNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindNext.Click
        PerformSearch(forward:=True)
    End Sub

    Private Sub btnFindPrev_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindPrev.Click
        PerformSearch(forward:=False)
    End Sub

    Private Sub PerformSearch(ByVal forward As Boolean)
        Dim query As String = txtSearch.Text
        If String.IsNullOrEmpty(query) Then
            lblMatchCount.Text = "0 matches"
            lblMatchCount.ForeColor = Color.Gray
            Return
        End If

        Dim fullText As String = RichTextBox1.Text
        Dim startIndex As Integer = RichTextBox1.SelectionStart

        If query <> _lastSearchQuery Then
            _lastSearchQuery = query
            startIndex = 0
        ElseIf forward Then
            startIndex += RichTextBox1.SelectionLength
        End If

        Dim options As RichTextBoxFinds = RichTextBoxFinds.None
        Dim matchPos As Integer = -1

        If forward Then
            matchPos = RichTextBox1.Find(query, startIndex, options)
            If matchPos = -1 AndAlso startIndex > 0 Then
                ' Wrap search from start
                matchPos = RichTextBox1.Find(query, 0, options)
            End If
        Else
            Dim searchEnd As Integer = Math.Max(0, startIndex - 1)
            matchPos = RichTextBox1.Find(query, 0, searchEnd, RichTextBoxFinds.Reverse)
            If matchPos = -1 AndAlso searchEnd < fullText.Length Then
                ' Wrap search from end
                matchPos = RichTextBox1.Find(query, 0, fullText.Length, RichTextBoxFinds.Reverse)
            End If
        End If

        If matchPos >= 0 Then
            RichTextBox1.Select(matchPos, query.Length)
            RichTextBox1.ScrollToCaret()
            lblMatchCount.Text = "Match found"
            lblMatchCount.ForeColor = If(ThemeManager.IsDarkMode, Color.LightGreen, Color.DarkGreen)
        Else
            lblMatchCount.Text = "No matches"
            lblMatchCount.ForeColor = Color.OrangeRed
        End If
    End Sub

    ' ── Actions Toolbar ────────────────────────────────────────────────────────

    Private Async Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click, CopyToolStripMenuItem.Click
        If RichTextBox1.SelectionLength > 0 Then
            RichTextBox1.Copy()
        Else
            Clipboard.SetText(RichTextBox1.Text)
        End If

        btnCopy.Text = "✓ Copied!"
        Await System.Threading.Tasks.Task.Delay(1500)
        btnCopy.Text = "📋 Copy All"
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If SaveFileDialog1.ShowDialog() = DialogResult.OK Then
            Try
                File.WriteAllText(SaveFileDialog1.FileName, RichTextBox1.Text)
                MessageBox.Show("Page source saved successfully!", "Save Source", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Error saving source file: " & ex.Message, "Save Source Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnToggleWrap_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnToggleWrap.Click
        RichTextBox1.WordWrap = Not RichTextBox1.WordWrap
        btnToggleWrap.Checked = RichTextBox1.WordWrap
        btnToggleWrap.Text = If(RichTextBox1.WordWrap, "📄 Word Wrap (On)", "📄 Word Wrap (Off)")
    End Sub

    Private Sub btnPrint_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrint.Click, PrintToolStripMenuItem.Click
        If PrintDialog1.ShowDialog() = DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        RichTextBox1.SelectAll()
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        Me.Close()
    End Sub

    ' ── Printing Support ───────────────────────────────────────────────────────

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Static currentLine As Integer = 0
        Dim textFont As Font = RichTextBox1.Font

        Dim pageHeight As Integer = e.MarginBounds.Height
        Dim pageWidth As Integer = e.MarginBounds.Width
        Dim leftMargin As Integer = e.MarginBounds.Left
        Dim topMargin As Integer = e.MarginBounds.Top

        Dim visibleLines As Integer = CInt(Math.Floor(pageHeight / textFont.Height))

        Using format As New StringFormat(StringFormatFlags.NoWrap)
            Dim i As Integer
            For i = currentLine To Math.Min(currentLine + visibleLines - 1, RichTextBox1.Lines.Length - 1)
                Dim lineText As String = RichTextBox1.Lines(i)
                Dim yPos As Single = topMargin + ((i - currentLine) * textFont.Height)
                e.Graphics.DrawString(lineText, textFont, Brushes.Black, New RectangleF(leftMargin, yPos, pageWidth, textFont.Height), format)
            Next
            currentLine += visibleLines
        End Using

        If currentLine < RichTextBox1.Lines.Length Then
            e.HasMorePages = True
        Else
            e.HasMorePages = False
            currentLine = 0
        End If
    End Sub

End Class