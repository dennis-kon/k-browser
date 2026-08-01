Imports System.Windows.Forms

Public Class AdBlockerSettings

    Private Class DefaultFilterList
        Public Property Name As String
        Public Property Url As String
        Public Property Enabled As Boolean = True
        Public Property LastUpdated As String = "Never"
    End Class

    Private Shared ReadOnly DefaultLists As New List(Of DefaultFilterList) From {
        New DefaultFilterList With {.Name = "EasyList", .Url = "https://easylist.to/easylist/easylist.txt"},
        New DefaultFilterList With {.Name = "EasyPrivacy", .Url = "https://easylist.to/easylist/easyprivacy.txt"},
        New DefaultFilterList With {.Name = "Fanboy's Annoyance", .Url = "https://secure.fanboy.co.nz/fanboy-annoyance.txt"}
    }

    Private Sub AdBlockerSettings_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        chkEnableAdBlocker.Checked = My.Settings.AdBlockerEnabled
        chkShowBlockedCount.Checked = My.Settings.ShowBlockedCount

        PopulateGrid()
    End Sub

    Private Sub PopulateGrid()
        dgvFilterLists.Rows.Clear()

        Dim savedConfig As String = My.Settings.FilterListsConfig
        Dim savedDict As New Dictionary(Of String, Tuple(Of Boolean, String))()

        If Not String.IsNullOrWhiteSpace(savedConfig) Then
            ' Format: Name|Enabled|LastUpdated;...
            Dim entries As String() = savedConfig.Split(";"c)
            For Each entry As String In entries
                Dim parts As String() = entry.Split("|"c)
                If parts.Length >= 3 Then
                    Dim name As String = parts(0)
                    Dim enabled As Boolean = (parts(1) = "1")
                    Dim lastUpdated As String = parts(2)
                    savedDict(name) = New Tuple(Of Boolean, String)(enabled, lastUpdated)
                End If
            Next
        End If

        For Each filterList In DefaultLists
            Dim isEnabled As Boolean = True
            Dim lastUpdated As String = "Never"

            If savedDict.ContainsKey(filterList.Name) Then
                isEnabled = savedDict(filterList.Name).Item1
                lastUpdated = savedDict(filterList.Name).Item2
            End If

            dgvFilterLists.Rows.Add(isEnabled, filterList.Name, filterList.Url, lastUpdated)
        Next
    End Sub

    Private Sub SaveConfig()
        My.Settings.AdBlockerEnabled = chkEnableAdBlocker.Checked
        My.Settings.ShowBlockedCount = chkShowBlockedCount.Checked

        Dim entries As New List(Of String)()
        For Each row As DataGridViewRow In dgvFilterLists.Rows
            If row.IsNewRow Then Continue For
            Dim enabled As Boolean = CBool(row.Cells(0).Value)
            Dim name As String = CStr(row.Cells(1).Value)
            Dim lastUpdated As String = CStr(row.Cells(3).Value)
            entries.Add(name & "|" & If(enabled, "1", "0") & "|" & lastUpdated)
        Next

        My.Settings.FilterListsConfig = String.Join(";", entries.ToArray())
        My.Settings.Save()
    End Sub

    Private Async Sub btnUpdateLists_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateLists.Click
        btnUpdateLists.Enabled = False
        lblStatus.Text = "Downloading filter lists..."

        Dim successCount As Integer = 0
        For Each row As DataGridViewRow In dgvFilterLists.Rows
            If row.IsNewRow Then Continue For
            Dim isEnabled As Boolean = CBool(row.Cells(0).Value)
            If isEnabled Then
                Dim name As String = CStr(row.Cells(1).Value)
                Dim url As String = CStr(row.Cells(2).Value)

                lblStatus.Text = "Downloading " & name & "..."
                Dim success As Boolean = Await AdBlockEngine.UpdateFilterListAsync(name, url)
                If success Then
                    row.Cells(3).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm")
                    successCount += 1
                End If
            End If
        Next

        Dim totalRules As Integer = AdBlockEngine.LoadAllRules()
        SaveConfig()

        lblStatus.Text = "Updated " & successCount & " list(s). Active rules: " & totalRules
        btnUpdateLists.Enabled = True
    End Sub

    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        SaveConfig()
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub

End Class

Namespace My
    Partial Friend Class MySettings
        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("True")> _
        Public Property ShowBlockedCount() As Boolean
            Get
                Return CBool(Me("ShowBlockedCount"))
            End Get
            Set(ByVal value As Boolean)
                Me("ShowBlockedCount") = value
            End Set
        End Property

        <Global.System.Configuration.UserScopedSettingAttribute(), Global.System.Configuration.DefaultSettingValueAttribute("")> _
        Public Property FilterListsConfig() As String
            Get
                Return CStr(Me("FilterListsConfig"))
            End Get
            Set(ByVal value As String)
                Me("FilterListsConfig") = value
            End Set
        End Property
    End Class
End Namespace
