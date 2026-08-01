Public Class Source

    Private Sub CopyToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CopyToolStripMenuItem.Click
        RichTextBox1.Copy()
    End Sub

    Private Sub SelectAllToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectAllToolStripMenuItem.Click
        RichTextBox1.SelectAll()
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        If PrintDialog1.ShowDialog() = Windows.Forms.DialogResult.OK Then
            PrintDocument1.Print()
        End If
    End Sub

    Private Sub PrintDocument1_PrintPage(ByVal sender As System.Object, ByVal e As System.Drawing.Printing.PrintPageEventArgs) Handles PrintDocument1.PrintPage
        Static currentChar As Integer
        Static currentLine As Integer

        Dim textfont As Font = RichTextBox1.Font
        Dim h, w As Integer
        Dim left, top As Integer

        With PrintDocument1.DefaultPageSettings
            h = .PaperSize.Height - .Margins.Top - .Margins.Bottom
            w = .PaperSize.Width - .Margins.Left - .Margins.Right
            left = PrintDocument1.DefaultPageSettings.Margins.Left
            top = PrintDocument1.DefaultPageSettings.Margins.Top
        End With

        e.Graphics.DrawRectangle(Pens.Blue, New Rectangle(left, top, w, h))

        If PrintDocument1.DefaultPageSettings.Landscape Then
            Dim a As Integer
            a = h
            h = w
            w = a
        End If

        Dim lines As Integer = CInt(Math.Round(h / textfont.Height))
        Dim b As New Rectangle(left, top, w, h)

        If Not RichTextBox1.WordWrap Then
            Using format As New StringFormat(StringFormatFlags.NoWrap)
                format.Trimming = StringTrimming.EllipsisWord
                Dim i As Integer
                For i = currentLine To Math.Min(currentLine + lines, RichTextBox1.Lines.Length - 1)
                    e.Graphics.DrawString(RichTextBox1.Lines(i), textfont, Brushes.Black, New RectangleF(left, top + textfont.Height * (i - currentLine), w, textfont.Height), format)
                Next
            End Using

            currentLine += lines

            If currentLine >= RichTextBox1.Lines.Length Then
                e.HasMorePages = False
                currentLine = 0
            Else
                e.HasMorePages = True
            End If
            Exit Sub
        End If

        Using format As New StringFormat(StringFormatFlags.LineLimit)
            Dim line, chars As Integer
            e.Graphics.MeasureString(Mid(RichTextBox1.Text, currentChar + 1), textfont, New SizeF(w, h), format, chars, line)

            If currentChar + chars < RichTextBox1.Text.Length Then
                ' Logic for finding clean break if needed
            End If

            e.Graphics.DrawString(RichTextBox1.Text.Substring(currentChar, chars), textfont, Brushes.Black, b, format)
            currentChar = currentChar + chars

            If currentChar < RichTextBox1.Text.Length Then
                e.HasMorePages = True
            Else
                e.HasMorePages = False
                currentChar = 0
            End If
        End Using
    End Sub

    Private Sub CloseToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CloseToolStripMenuItem.Click
        Me.Close()
    End Sub

End Class