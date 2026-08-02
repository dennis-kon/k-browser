Imports System.Drawing
Imports System.Windows.Forms
Imports Microsoft.Web.WebView2.WinForms

Public Class AdBlockPopup

    Private m_brws As WebView2
    Private m_url As String
    Private m_lastCount As Integer = -1

    Public Sub New(ByVal brws As WebView2, ByVal currentUrl As String)
        InitializeComponent()
        m_brws = brws
        m_url = currentUrl
    End Sub

    Private Sub AdBlockPopup_Load(ByVal sender As Object, ByVal e As EventArgs) Handles MyBase.Load
        ThemeManager.ApplyTheme(Me)

        Try
            If Form1.ActiveForm IsNot Nothing AndAlso Form1.ActiveForm.Icon IsNot Nothing Then
                Me.Icon = Form1.ActiveForm.Icon
            ElseIf Application.OpenForms.Count > 0 AndAlso Application.OpenForms(0).Icon IsNot Nothing Then
                Me.Icon = Application.OpenForms(0).Icon
            End If
        Catch
        End Try

        Dim domain As String = AdBlockEngine.GetDomain(m_url)
        If String.IsNullOrWhiteSpace(domain) Then
            lblSiteDomain.Text = "Internal / Blank Page"
            lblFullUrl.Text = If(String.IsNullOrWhiteSpace(m_url), "about:blank", m_url)
            chkDisableOnSite.Enabled = False
            chkDisableOnSite.Checked = False
            lblStatusText.Text = "Status: Not applicable on internal pages"
            lblStatusText.ForeColor = Color.Gray
        Else
            lblSiteDomain.Text = domain
            lblFullUrl.Text = m_url
            chkDisableOnSite.Enabled = True

            Dim isDisabled As Boolean = AdBlockEngine.IsSiteDisabled(m_url)
            chkDisableOnSite.Checked = isDisabled
            UpdateStatusUI(isDisabled)
        End If

        RefreshData()
        TimerRefresh.Start()
    End Sub

    Private Sub UpdateStatusUI(ByVal isDisabled As Boolean)
        If isDisabled Then
            lblStatusText.Text = "Status: DISABLED on this site (Ads & trackers allowed)"
            lblStatusText.ForeColor = Color.FromArgb(220, 50, 50)
            lblShield.Text = "🛡️"
        Else
            lblStatusText.Text = "Status: ACTIVE on this site (Ads & trackers blocked)"
            lblStatusText.ForeColor = Color.FromArgb(30, 150, 60)
            lblShield.Text = "🛡️"
        End If
    End Sub

    Private Sub chkDisableOnSite_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles chkDisableOnSite.CheckedChanged
        If String.IsNullOrWhiteSpace(m_url) OrElse m_url = "about:blank" Then Return

        Dim isDisabled As Boolean = chkDisableOnSite.Checked
        AdBlockEngine.SetSiteDisabled(m_url, isDisabled)
        UpdateStatusUI(isDisabled)
    End Sub

    Private Sub RefreshData()
        Dim pageItems = AdBlockEngine.GetPageBlockedItems(m_brws)
        Dim pageCount As Integer = pageItems.Count
        Dim totalCount As Long = AdBlockEngine.TotalBlockedCount

        lblCountPage.Text = pageCount.ToString("N0")
        lblCountTotal.Text = totalCount.ToString("N0")

        If pageCount <> m_lastCount Then
            m_lastCount = pageCount
            dgvBlockedTrackers.Rows.Clear()

            For i As Integer = 0 To pageItems.Count - 1
                Dim item = pageItems(i)
                Dim rowIdx As Integer = dgvBlockedTrackers.Rows.Add((i + 1).ToString(), item.Url, item.Timestamp.ToString("HH:mm:ss"))
            Next
        End If
    End Sub

    Private Sub TimerRefresh_Tick(ByVal sender As Object, ByVal e As EventArgs) Handles TimerRefresh.Tick
        RefreshData()
    End Sub

    Private Sub btnAdBlockSettings_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnAdBlockSettings.Click
        AdBlockerSettings.ShowDialog()
        RefreshData()
    End Sub

    Private Sub btnReload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnReload.Click
        If m_brws IsNot Nothing AndAlso m_brws.CoreWebView2 IsNot Nothing Then
            Try
                m_brws.CoreWebView2.Reload()
            Catch
            End Try
        End If
        Me.Close()
    End Sub

    Private Sub btnClose_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnClose.Click
        Me.Close()
    End Sub

End Class
