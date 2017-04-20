Imports System.Xml
Imports System.Xml.XPath



Public Class Rss

    ' Current Path to RSS Feed
    Private mRssUrl As String

    Private Sub Form1_Load(ByVal sender As System.Object, _
                           ByVal e As System.EventArgs) Handles MyBase.Load

        ' Clear path to RSS Feed
        mRssUrl = String.Empty

    End Sub




    Private Sub tsRssGo_Click(ByVal sender As System.Object, _
                              ByVal e As System.EventArgs) Handles tsRssGo.Click

        Try

            ' set the file path member var
            mRssUrl = Me.tsRssLocation.Text

            ' Clear the treeview.
            tvwRss.Nodes.Clear()

            ' set the wait cursor
            Me.Cursor = Cursors.WaitCursor

            ' create a new xml doc
            Dim doc As New XmlDocument()

            Try

                ' load the xml doc
                doc.Load(mRssUrl)

                ' return the cursor
                Me.Cursor = Cursors.Default

            Catch ex1 As Exception

                ' return the cursor
                Me.Cursor = Cursors.Default

                ' tell a story
                MessageBox.Show(ex1.Message)
                Return

            End Try



            ' get an xpath navigator   
            Dim navigator As XPathNavigator = doc.CreateNavigator()

            Try

                ' look for the path to the rss item titles navigate
                ' through the nodes to get all titles
                Dim nodes As XPathNodeIterator = navigator.Select("/rss/channel/item/title")

                While nodes.MoveNext

                    ' clean up the text for display
                    Dim node As XPathNavigator = nodes.Current
                    Dim tmp As String = node.Value.Trim()
                    tmp = tmp.Replace(ControlChars.CrLf, "")
                    tmp = tmp.Replace(ControlChars.Lf, "")
                    tmp = tmp.Replace(ControlChars.Cr, "")
                    tmp = tmp.Replace(ControlChars.FormFeed, "")
                    tmp = tmp.Replace(ControlChars.NewLine, "")

                    ' add a new treeview node for this
                    ' news item title
                    tvwRss.Nodes.Add(tmp)

                End While


                ' set a position counter
                Dim position As Integer = 0

                ' Get the links from the RSS feed
                Dim nodesLink As XPathNodeIterator = navigator.Select("/rss/channel/item/link")

                While nodesLink.MoveNext

                    ' clean up the link
                    Dim node As XPathNavigator = nodesLink.Current
                    Dim tmp As String = node.Value.Trim()
                    tmp = tmp.Replace(ControlChars.CrLf, "")
                    tmp = tmp.Replace(ControlChars.Lf, "")
                    tmp = tmp.Replace(ControlChars.Cr, "")
                    tmp = tmp.Replace(ControlChars.FormFeed, "")
                    tmp = tmp.Replace(ControlChars.NewLine, "")


                    ' use the position counter
                    ' to add a link child node
                    ' to each news item title
                    tvwRss.Nodes(position).Nodes.Add(tmp)

                    ' increment the position counter
                    position += 1

                End While


            Catch ex As Exception

                MessageBox.Show(ex.Message, "RSS Feed Load Error")

            End Try


            ' restore the cursor
            Me.Cursor = Cursors.Default

        Catch ex2 As Exception

            ' snitch
            MessageBox.Show(ex2.ToString(), "RSS Feed Initialization Failure")

        End Try

    End Sub


    ''' <summary>
    ''' Upon selection of a node, open the link
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tvwRss_AfterSelect(ByVal sender As System.Object, _
                                   ByVal e As System.Windows.Forms.TreeViewEventArgs) _
                                   Handles tvwRss.AfterSelect

        Try
            ' get the first four characters from the link for a
            ' quick test
            Dim tmp As String = tvwRss.SelectedNode.Text.Substring(0, 4)

            ' test the link text and then
            ' navigate the browser to that link
            If tmp = "http" Then
                webBrowser1.Navigate(tvwRss.SelectedNode.Text)
                webBrowser1.ScriptErrorsSuppressed = True
            End If

        Catch
            ' skip it if it isn't a link
        End Try

    End Sub


    ''' <summary>
    ''' Load the selected canned RSS feed into the 
    ''' treeview using the existing methods
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub tsCboFeeds_SelectedIndexChanged(ByVal sender _
        As System.Object, ByVal e As System.EventArgs) _
        Handles tsCboFeeds.SelectedIndexChanged

        ' load the text for the selected feed into
        ' the RSS location text box and then use
        ' the existing button click event to launch
        ' the RSS feed into the treeview
        tsRssLocation.Text = tsCboFeeds.SelectedItem.ToString()
        tsRssGo_Click(Me, New EventArgs())

    End Sub


    Private Sub tsCboFeeds_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsCboFeeds.Click

    End Sub
End Class
