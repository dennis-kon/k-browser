Public Class History

    ''' <summary>
    ''' Maps the display text (page title) shown in the ListBox to the raw
    ''' history entry stored in My.Settings.History ("Title|URL|DateTime" or legacy formats).
    ''' </summary>
    Private ReadOnly _entryMap As New Dictionary(Of String, String)()

    Private Sub History_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        cboDateFilter.SelectedIndex = 0 ' "All Time"
        LoadHistoryData()
    End Sub

    ' ── helpers ──────────────────────────────────────────────────────────────

    ''' <summary>
    ''' Extracts the URL portion from a history entry.
    ''' Supports "Title|URL|DateTime", "Title|URL", and legacy URL-only formats.
    ''' </summary>
    Private Shared Function GetUrlFromEntry(entry As String) As String
        Dim parts() As String = entry.Split("|"c)
        If parts.Length >= 2 Then Return parts(1)
        Return entry ' legacy URL-only entry
    End Function

    ''' <summary>
    ''' Extracts the display title from a history entry.
    ''' </summary>
    Private Shared Function GetTitleFromEntry(entry As String) As String
        Dim parts() As String = entry.Split("|"c)
        If parts.Length >= 1 AndAlso Not String.IsNullOrWhiteSpace(parts(0)) Then
            Return parts(0)
        End If
        Return entry ' legacy URL-only entry
    End Function

    ''' <summary>
    ''' Extracts the DateTime from a history entry.
    ''' Returns Nothing if the entry has no timestamp (legacy format).
    ''' </summary>
    Private Shared Function GetDateFromEntry(entry As String) As DateTime?
        Dim parts() As String = entry.Split("|"c)
        If parts.Length >= 3 Then
            Dim dt As DateTime
            If DateTime.TryParse(parts(2), dt) Then
                Return dt
            End If
        End If
        Return Nothing ' no timestamp available
    End Function

    ''' <summary>
    ''' Extracts the domain from a URL (e.g., "https://www.google.com/search?q=test" → "google.com").
    ''' Returns the URL itself if parsing fails.
    ''' </summary>
    Private Shared Function GetDomainFromUrl(url As String) As String
        Try
            Dim uri As New Uri(url)
            Dim host As String = uri.Host
            ' Strip "www." prefix for cleaner display
            If host.StartsWith("www.", StringComparison.OrdinalIgnoreCase) Then
                host = host.Substring(4)
            End If
            Return host
        Catch
            Return url
        End Try
    End Function

    ''' <summary>
    ''' Formats the display text as "domain — title".
    ''' If the title matches the domain or URL, shows just the domain.
    ''' </summary>
    Private Shared Function FormatDisplayText(title As String, url As String) As String
        Dim domain As String = GetDomainFromUrl(url)
        If String.IsNullOrWhiteSpace(title) OrElse
           title.Equals(url, StringComparison.OrdinalIgnoreCase) OrElse
           title.Equals(domain, StringComparison.OrdinalIgnoreCase) Then
            Return domain
        End If
        Return domain & " — " & title
    End Function

    ''' <summary>
    ''' Resolves the URL for the currently selected ListBox item.
    ''' Returns Nothing if no item is selected.
    ''' </summary>
    Private Function GetSelectedUrl() As String
        If ListBox1.SelectedItem Is Nothing Then Return Nothing
        Dim displayText As String = ListBox1.SelectedItem.ToString()
        If _entryMap.ContainsKey(displayText) Then
            Return GetUrlFromEntry(_entryMap(displayText))
        End If
        Return displayText ' fallback
    End Function

    ''' <summary>
    ''' Returns True if the entry date falls within the currently selected date range.
    ''' Entries without a timestamp are included only when "All Time" is selected.
    ''' </summary>
    Private Function MatchesDateFilter(entryDate As DateTime?) As Boolean
        Dim filterIndex As Integer = If(cboDateFilter.SelectedIndex >= 0, cboDateFilter.SelectedIndex, 0)

        ' "All Time" — show everything
        If filterIndex = 0 Then Return True

        ' If no date is stored, we can't filter — hide from date-specific views
        If Not entryDate.HasValue Then Return False

        Dim today As DateTime = DateTime.Today
        Select Case filterIndex
            Case 1 ' Today
                Return entryDate.Value.Date = today
            Case 2 ' Yesterday
                Return entryDate.Value.Date = today.AddDays(-1)
            Case 3 ' Last 7 Days
                Return entryDate.Value.Date >= today.AddDays(-6)
            Case 4 ' Last 30 Days
                Return entryDate.Value.Date >= today.AddDays(-29)
            Case Else
                Return True
        End Select
    End Function

    ' ── data loading ─────────────────────────────────────────────────────────

    Private Sub LoadHistoryData(Optional ByVal filterText As String = "")
        Try
            ListBox1.Items.Clear()
            _entryMap.Clear()

            If My.Settings.History IsNot Nothing Then
                Dim count As Integer = 0
                For Each item As String In My.Settings.History
                    If Not String.IsNullOrWhiteSpace(item) Then
                        Dim title As String = GetTitleFromEntry(item)
                        Dim url As String = GetUrlFromEntry(item)
                        Dim entryDate As DateTime? = GetDateFromEntry(item)

                        ' Apply date filter
                        If Not MatchesDateFilter(entryDate) Then Continue For

                        ' Apply text search filter against both title and URL
                        If Not String.IsNullOrWhiteSpace(filterText) AndAlso
                           Not title.ToLower().Contains(filterText.ToLower()) AndAlso
                           Not url.ToLower().Contains(filterText.ToLower()) Then
                            Continue For
                        End If

                        ' Format as "domain — title" for display
                        Dim displayText As String = FormatDisplayText(title, url)
                        ' Ensure unique display text if entries collide
                        If _entryMap.ContainsKey(displayText) Then
                            displayText = displayText & "  (" & url & ")"
                        End If

                        _entryMap(displayText) = item
                        ListBox1.Items.Add(displayText)
                        count += 1
                    End If
                Next
                lblCount.Text = count & " item" & If(count = 1, "", "s")
            Else
                lblCount.Text = "0 items"
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error loading history: " & ex.Message)
        End Try
    End Sub

    Private Sub txtSearch_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtSearch.TextChanged
        LoadHistoryData(txtSearch.Text.Trim())
    End Sub

    Private Sub cboDateFilter_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles cboDateFilter.SelectedIndexChanged
        LoadHistoryData(txtSearch.Text.Trim())
    End Sub

    ' ── actions ──────────────────────────────────────────────────────────────

    Private Sub ListBox1_DoubleClick(ByVal sender As Object, ByVal e As EventArgs) Handles ListBox1.DoubleClick
        OpenSelectedInActiveTab()
    End Sub

    Private Sub btnOpen_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpen.Click, tsmOpenActive.Click
        OpenSelectedInActiveTab()
    End Sub

    Private Async Sub btnOpenNewTab_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenNewTab.Click, tsmOpenNewTab.Click
        Dim url As String = GetSelectedUrl()
        If url IsNot Nothing Then
            Await Form1.CreateNewTab(url)
        End If
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click, tsmCopy.Click
        Dim url As String = GetSelectedUrl()
        If url IsNot Nothing Then
            Clipboard.SetText(url)
        End If
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click, tsmDelete.Click
        Try
            If ListBox1.SelectedItem IsNot Nothing Then
                Dim displayText As String = ListBox1.SelectedItem.ToString()
                If _entryMap.ContainsKey(displayText) Then
                    Dim rawEntry As String = _entryMap(displayText)
                    If My.Settings.History IsNot Nothing AndAlso My.Settings.History.Contains(rawEntry) Then
                        My.Settings.History.Remove(rawEntry)
                        My.Settings.Save()
                    End If
                End If
                LoadHistoryData(txtSearch.Text.Trim())
            End If
        Catch ex As Exception
            System.Diagnostics.Debug.WriteLine("Error deleting history item: " & ex.Message)
        End Try
    End Sub

    Private Sub btnClearAll_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearAll.Click, tsmClearAll.Click
        If MsgBox("Are you sure you want to clear all browsing history?", MsgBoxStyle.YesNo Or MsgBoxStyle.Question, "Clear History") = MsgBoxResult.Yes Then
            Try
                If My.Settings.History IsNot Nothing Then
                    My.Settings.History.Clear()
                    My.Settings.Save()
                End If
                LoadHistoryData()
            Catch ex As Exception
                System.Diagnostics.Debug.WriteLine("Error clearing history: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub OpenSelectedInActiveTab()
        Dim url As String = GetSelectedUrl()
        If url IsNot Nothing Then
            Form1.NavigateActiveTab(url)
        End If
    End Sub

End Class