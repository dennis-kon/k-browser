'///////////////////////////////////////////
'Form to facilitate settings in the application.
'I am using built in vs 2005 settings for 
'most of the settings in the application
'other than search providers and feeds.
'///////////////////////////////////////////
Imports System.Enum
Imports System.Data.OleDb
Imports System.Net
Imports System.Runtime.InteropServices


Public Class Settings
    Dim host As String
    Dim port As Integer
    Dim counter As Integer
    Private Sub frmSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadBlockedSites()
        LoadPhishingSites()
        LoadPopUpSettings()
        LoadPopAllowed()

    End Sub

#Region " Popup Blocker Settings "



    Private Sub LoadPopUpSettings()
        chkAllowPop.Checked = My.Settings.PopUpBlockerEnabled
        chkPopSound.Checked = My.Settings.PopSound
        chkPopInfo.Checked = My.Settings.PopInfoBar
    End Sub

    Private Sub LoadPopAllowed()
        lbPop.Items.Clear()
        Dim li As ListItem
        Dim s As String
        For Each s In My.Settings.AllowedPopSites
            li = New ListItem
            li.Text = s
            lbPop.Items.Add(li)
        Next
    End Sub

    Private Sub chkAllowPop_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkAllowPop.CheckedChanged
        My.Settings.PopUpBlockerEnabled = chkAllowPop.Checked
    End Sub

    Private Sub chkPopSound_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPopSound.CheckedChanged
        My.Settings.PopSound = chkPopSound.Checked
    End Sub

    Private Sub chkPopInfo_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPopInfo.CheckedChanged
        My.Settings.PopInfoBar = chkPopInfo.Checked
    End Sub

    Private Sub btnPopAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopAdd.Click
        If txtPop.Text = "" Then
            'Do nothing
        Else
            My.Settings.AllowedPopSites.Add(AppManager.FixURL(txtPop.Text))
            txtPop.Text = String.Empty
            LoadPopAllowed()
        End If
    End Sub

    Private Sub btnPopRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopRemove.Click
        Dim li As ListItem
        If lbPop.SelectedItems.Count > 0 Then
            For Each li In lbPop.SelectedItems
                My.Settings.AllowedPopSites.Remove(li.Text)
            Next
            LoadPopAllowed()
        End If
    End Sub

    Private Sub btnPopRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPopRemoveAll.Click
        My.Settings.AllowedPopSites.Clear()
        lbPop.Items.Clear()
    End Sub

#End Region

#Region " Blocked Sites "

    Private Sub LoadBlockedSites()
        lbBlocked.Items.Clear()
        Dim s As String
        For Each s In My.Settings.BlockedSites
            lbBlocked.Items.Add(s)
        Next
    End Sub

    Private Sub btnAddBlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddBlock.Click
        If txtBlock.Text = "" Then
            'Do nothing
        Else
            My.Settings.BlockedSites.Add(AppManager.FixURL(txtBlock.Text))
            LoadBlockedSites()
            txtBlock.Text = String.Empty
        End If
    End Sub

    Private Sub btnBlockRemoveAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBlockRemoveAll.Click
        Dim s As String
        For Each s In My.Settings.BlockedSites
            My.Settings.BlockedSites.Remove(s)
        Next
        LoadBlockedSites()
    End Sub

    Private Sub btnRemoveBlock_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveBlock.Click
        If lbBlocked.SelectedIndices.Count > 0 Then
            My.Settings.BlockedSites.Remove(lbBlocked.SelectedItem)
            LoadBlockedSites()
        End If
    End Sub

#End Region

#Region " Integrations "

    Private Sub LoadAdditional()
        chkPhishing.Checked = My.Settings.UsePhishingFilter
    End Sub

#End Region



#Region " Phishing "
    Private Sub LoadPhishingSites()
        lbPhishing.Items.Clear()
        Dim s As String
        For Each s In My.Settings.PhishingSites
            lbPhishing.Items.Add(s)
        Next
    End Sub
    Private Sub btnAddPhish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddPhish.Click
        If txtPhishing.Text = "" Then
            'Do nothing
        Else
            My.Settings.PhishingSites.Add(AppManager.FixURL(txtPhishing.Text))
            LoadPhishingSites()
            txtPhishing.Text = String.Empty
        End If
    End Sub

    Private Sub btnRemovePhish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemovePhish.Click
        If lbPhishing.SelectedIndices.Count > 0 Then
            My.Settings.PhishingSites.Remove(lbPhishing.SelectedItem)
            LoadPhishingSites()
        End If
    End Sub

    Private Sub btnRemoveAllPhish_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveAllPhish.Click
        Dim s As String
        For Each s In My.Settings.PhishingSites
            My.Settings.PhishingSites.Remove(s)
        Next
        LoadPhishingSites()
    End Sub

#End Region

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        My.Settings.Save()
        Me.Close()
    End Sub



    Private Sub chkPhishing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkPhishing.CheckedChanged
        My.Settings.UsePhishingFilter = chkPhishing.Checked
        My.Settings.Save()
    End Sub

    Private Sub btnAddProvider_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub


    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
    End Sub


    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim hostname As IPHostEntry = Dns.GetHostEntry(TextBox1.Text.Trim())

        Dim ip As IPAddress() = hostname.AddressList

        TextBox2.Text = ip(0).ToString()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        counter = counter + 1
        TextBox3.Text = counter
        host = TextBox1.Text
        port = TextBox3.Text

        Dim hostadd As System.Net.IPAddress = System.Net.Dns.GetHostEntry(host).AddressList(0)
        Dim EPhost As New System.Net.IPEndPoint(hostadd, port)
        Dim s As New System.Net.Sockets.Socket(System.Net.Sockets.AddressFamily.InterNetwork, System.Net.Sockets.SocketType.Stream, System.Net.Sockets.ProtocolType.Tcp)
        Try
            s.Connect(EPhost)
        Catch
        End Try
        If Not s.Connected Then
            ListBox1.Items.Add("Port " + port.ToString + " is not open")
        Else
            ListBox1.Items.Add("Port " + port.ToString + " is open")
            ListBox2.Items.Add(port.ToString)
        End If
        Label3.Text = "Open Ports: " + ListBox2.Items.Count.ToString

    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        ListBox1.Items.Add("Scanning: " + TextBox1.Text)
        ListBox1.Items.Add("-------------------")
        Button2.Enabled = True
        Button1.Enabled = False
        Timer1.Enabled = True
        Timer1.Start()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Timer1.Stop()
        Timer1.Enabled = False
        Button1.Enabled = True
        Button2.Enabled = False
    End Sub


 
End Class
