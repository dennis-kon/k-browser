Imports System.Drawing
Imports System.Drawing.Drawing2D

Public Class AppManager

    Public Shared Function IsValidUrl(ByVal url As String) As Boolean
        If String.IsNullOrWhiteSpace(url) Then Return False
        Return System.Text.RegularExpressions.Regex.IsMatch(url.Trim(),
            "^(https?|ftp|file)://[^\s/$.?#].[^\s]*$",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase)
    End Function

    Public Shared Function IsValidIP(ByVal ipAddress As String) As Boolean
        If String.IsNullOrWhiteSpace(ipAddress) Then Return False
        Return System.Text.RegularExpressions.Regex.IsMatch(ipAddress.Trim(),
            "^(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}$")
    End Function

    Public Shared Function IsValidEmail(ByVal email As String) As Boolean
        If String.IsNullOrWhiteSpace(email) Then Return False
        Return System.Text.RegularExpressions.Regex.IsMatch(email.Trim(),
            "^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")
    End Function

    Public Shared Function FixURL(ByVal sURL As String) As String
        If String.IsNullOrWhiteSpace(sURL) Then Return sURL
        Dim trimmed As String = sURL.Trim()
        Dim lowered As String = trimmed.ToLowerInvariant()
        If lowered.StartsWith("http://") OrElse lowered.StartsWith("https://") OrElse lowered.StartsWith("file://") OrElse lowered.StartsWith("about:") OrElse lowered.StartsWith("ftp://") Then
            Return trimmed
        End If
        Return "http://" & trimmed
    End Function

    Public Shared Function ResolveUrlOrSearch(ByVal input As String, Optional ByVal engine As String = "Google") As String
        If String.IsNullOrWhiteSpace(input) Then Return "about:blank"
        Dim trimmed As String = input.Trim()
        Dim lowered As String = trimmed.ToLowerInvariant()

        If lowered.StartsWith("http://") OrElse lowered.StartsWith("https://") OrElse lowered.StartsWith("file://") OrElse lowered.StartsWith("about:") OrElse lowered.StartsWith("ftp://") Then
            Return trimmed
        End If

        If trimmed.Contains(" ") OrElse (Not trimmed.Contains(".") AndAlso Not trimmed.Contains(":")) Then
            Dim query As String = Uri.EscapeDataString(trimmed)
            Select Case If(engine, "Google").ToLowerInvariant()
                Case "bing"
                    Return "https://www.bing.com/search?q=" & query
                Case "duckduckgo"
                    Return "https://duckduckgo.com/?q=" & query
                Case "yahoo"
                    Return "https://search.yahoo.com/search?p=" & query
                Case Else
                    Return "https://www.google.com/search?q=" & query
            End Select
        End If

        Return FixURL(trimmed)
    End Function

    ''' <summary>
    ''' Dynamically generates a high-DPI modern System.Drawing.Bitmap bookmark ribbon icon.
    ''' </summary>
    ''' <param name="size">Target width and height of the square bitmap (e.g. 16, 24, 32).</param>
    ''' <param name="isBookmarked">If true, fills with vibrant active bookmark color (Amber Gold); otherwise Cornflower Blue.</param>
    ''' <param name="accentColor">Optional custom color override.</param>
    ''' <returns>A new 32bpp transparent System.Drawing.Bitmap icon.</returns>
    Public Shared Function CreateBookmarkIcon(Optional ByVal size As Integer = 16, Optional ByVal isBookmarked As Boolean = False, Optional ByVal accentColor As Color = Nothing) As Bitmap
        If size < 8 Then size = 16
        Dim bmp As New Bitmap(size, size, System.Drawing.Imaging.PixelFormat.Format32bppArgb)

        Using g As Graphics = Graphics.FromImage(bmp)
            g.SmoothingMode = SmoothingMode.AntiAlias
            g.PixelOffsetMode = PixelOffsetMode.HighQuality
            g.Clear(Color.Transparent)

            Dim fillColor As Color
            Dim borderColor As Color

            If accentColor <> Color.Empty Then
                fillColor = accentColor
                borderColor = Color.FromArgb(Math.Max(0, CInt(accentColor.R) - 40), Math.Max(0, CInt(accentColor.G) - 40), Math.Max(0, CInt(accentColor.B) - 40))
            ElseIf isBookmarked Then
                fillColor = Color.FromArgb(245, 158, 11)  ' Amber Gold (#F59E0B)
                borderColor = Color.FromArgb(217, 119, 6)   ' Dark Amber (#D97706)
            Else
                fillColor = Color.FromArgb(100, 149, 237) ' Cornflower Blue (#6495ED)
                borderColor = Color.FromArgb(65, 105, 225)  ' Royal Blue (#4169E1)
            End If

            Dim margin As Single = CSng(size) * 0.12F
            Dim left As Single = margin + 1.0F
            Dim top As Single = margin
            Dim width As Single = CSng(size) - (margin * 2.0F) - 2.0F
            Dim height As Single = CSng(size) - (margin * 2.0F)
            Dim right As Single = left + width
            Dim bottom As Single = top + height
            Dim notchDepth As Single = height * 0.28F

            Dim points As PointF() = {
                New PointF(left, top),
                New PointF(right, top),
                New PointF(right, bottom),
                New PointF(left + (width / 2.0F), bottom - notchDepth),
                New PointF(left, bottom)
            }

            Using path As New GraphicsPath()
                path.AddPolygon(points)

                Using fillBrush As New SolidBrush(fillColor)
                    g.FillPath(fillBrush, path)
                End Using

                Using pen As New Pen(borderColor, 1.2F)
                    pen.LineJoin = LineJoin.Round
                    g.DrawPath(pen, path)
                End Using
            End Using
        End Using

        Return bmp
    End Function

End Class
