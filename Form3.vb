Imports System.Runtime.InteropServices
Imports Microsoft.Win32
Public Class Form3
#Region "Using Proxy"
    <Runtime.InteropServices.DllImport("wininet.dll", SetLastError:=True)> _
    Private Shared Function InternetSetOption(ByVal hInternet As IntPtr, ByVal dwOption As Integer, ByVal lpBuffer As IntPtr, ByVal lpdwBufferLength As Integer) As Boolean
    End Function

    Public Structure Struct_INTERNET_PROXY_INFO
        Public dwAccessType As Integer
        Public proxy As IntPtr
        Public proxyBypass As IntPtr
    End Structure

    Private Sub UseProxy(ByVal strProxy As String)
        Const INTERNET_OPTION_PROXY As Integer = 38
        Const INTERNET_OPEN_TYPE_PROXY As Integer = 3
        Const INTERNET_OPEN_TYPE_DIRECT As Integer = 1

        Dim struct_IPI As Struct_INTERNET_PROXY_INFO

        If String.IsNullOrEmpty(strProxy) OrElse strProxy = ":" Then
            struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_DIRECT
            struct_IPI.proxy = IntPtr.Zero
            struct_IPI.proxyBypass = IntPtr.Zero
        Else
            struct_IPI.dwAccessType = INTERNET_OPEN_TYPE_PROXY
            struct_IPI.proxy = Marshal.StringToHGlobalAnsi(strProxy)
            struct_IPI.proxyBypass = Marshal.StringToHGlobalAnsi("local")
        End If

        Dim intptrStruct As IntPtr = Marshal.AllocCoTaskMem(Marshal.SizeOf(struct_IPI))

        Try
            Marshal.StructureToPtr(struct_IPI, intptrStruct, True)
            InternetSetOption(IntPtr.Zero, INTERNET_OPTION_PROXY, intptrStruct, Marshal.SizeOf(struct_IPI))
        Finally
            Marshal.FreeCoTaskMem(intptrStruct)
            If struct_IPI.proxy <> IntPtr.Zero Then Marshal.FreeHGlobal(struct_IPI.proxy)
            If struct_IPI.proxyBypass <> IntPtr.Zero Then Marshal.FreeHGlobal(struct_IPI.proxyBypass)
        End Try
    End Sub
#End Region

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If Not String.IsNullOrWhiteSpace(TextBox1.Text) AndAlso Not String.IsNullOrWhiteSpace(TextBox2.Text) Then
            UseProxy(TextBox1.Text.Trim() & ":" & TextBox2.Text.Trim())
            If Form1.wb IsNot Nothing Then
                Form1.wb.Navigate("http://ipchicken.com")
            End If
        End If
    End Sub

    Private Sub CheckBox1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox1.CheckedChanged
        Form1.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        TextBox1.Text = ""
        TextBox2.Text = ""
        UseProxy(String.Empty)
        If Form1.wb IsNot Nothing Then
            Form1.wb.Navigate("http://ipchicken.com")
        End If
        Try
            Using proxy As RegistryKey = Registry.CurrentUser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Internet Settings", True)
                If proxy IsNot Nothing Then
                    proxy.SetValue("ProxyEnable", 0)
                End If
            End Using
        Catch ex As Exception
        End Try
    End Sub
End Class
