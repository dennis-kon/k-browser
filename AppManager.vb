Imports System.IO
Imports System.Net

Public Class AppManager


    Public Shared Function IsValidUrl(ByVal url As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(url, _
            "(http|ftp|https)://([\w-]+\.)+(/[\w- ./?%&=]*)?")
    End Function

    Public Shared Function IsValidIP(ByVal ipAddress As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(ipAddress, _
            "^(25[0-5]|2[0-4]\d|[0-1]?\d?\d)(\.(25[0-5]|2[0-4]\d|[0-1]?\d?\d)){3}$")
    End Function

    Public Shared Function IsValidEmail(ByVal email As String) As Boolean
        Return System.Text.RegularExpressions.Regex.IsMatch(email, _
            "^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$")
    End Function


    Public Shared Function FixURL(ByVal sURL As String) As String
        If Not sURL.ToLower().StartsWith("http://") _
        Then sURL = "http://" & sURL
        Return sURL
    End Function
   
End Class
