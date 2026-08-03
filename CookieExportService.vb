Imports System
Imports System.Collections.Generic
Imports System.IO
Imports System.Text
Imports System.Threading.Tasks

''' <summary>
''' Application service responsible for exporting visible cookie table records to RFC 4180 compliant CSV files.
''' </summary>
Public Class CookieExportService

    ''' <summary>
    ''' Asynchronously exports a list of CookieItem records to a CSV file.
    ''' </summary>
    ''' <param name="cookies">Collection of cookies to export</param>
    ''' <param name="filePath">Target destination file path</param>
    Public Async Function ExportToCsvAsync(ByVal cookies As IEnumerable(Of CookieItem), ByVal filePath As String) As Task
        If String.IsNullOrWhiteSpace(filePath) Then Throw New ArgumentException("File path cannot be empty.", NameOf(filePath))

        Dim builder As New StringBuilder()

        ' Write Header Line
        builder.AppendLine("Website,Cookie Name,Domain,Path,Expires,Secure,HttpOnly,SameSite")

        If cookies IsNot Nothing Then
            For Each c As CookieItem In cookies
                Dim website As String = EscapeCsvField(c.Website)
                Dim name As String = EscapeCsvField(c.Name)
                Dim domain As String = EscapeCsvField(c.Domain)
                Dim pathVal As String = EscapeCsvField(c.Path)
                Dim expires As String = EscapeCsvField(c.DisplayExpires)
                Dim secure As String = EscapeCsvField(c.IsSecure.ToString())
                Dim httpOnly As String = EscapeCsvField(c.IsHttpOnly.ToString())
                Dim sameSite As String = EscapeCsvField(c.SameSite)

                builder.AppendLine($"{website},{name},{domain},{pathVal},{expires},{secure},{httpOnly},{sameSite}")
            Next
        End If

        Try
            Dim directoryPath As String = Path.GetDirectoryName(filePath)
            If Not String.IsNullOrEmpty(directoryPath) AndAlso Not Directory.Exists(directoryPath) Then
                Directory.CreateDirectory(directoryPath)
            End If

            Using writer As New StreamWriter(filePath, False, Encoding.UTF8)
                Await writer.WriteAsync(builder.ToString())
            End Using
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("CookieExportService: Failed to export cookies: " & ex.Message)
            Throw New IOException("Failed to save cookies to CSV: " & ex.Message, ex)
        End Try
    End Function

    ''' <summary>
    ''' Escapes fields for CSV output according to RFC 4180 rules.
    ''' </summary>
    Private Function EscapeCsvField(ByVal field As String) As String
        If String.IsNullOrEmpty(field) Then Return ""

        If field.Contains(",") OrElse field.Contains("""") OrElse field.Contains(vbCr) OrElse field.Contains(vbLf) Then
            Return """" & field.Replace("""", """""") & """"
        End If

        Return field
    End Function

End Class
