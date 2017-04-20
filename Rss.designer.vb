<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Rss
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Rss))
        Me.toolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.tsRssLocation = New System.Windows.Forms.ToolStripTextBox()
        Me.tsRssGo = New System.Windows.Forms.ToolStripButton()
        Me.tsCboFeeds = New System.Windows.Forms.ToolStripComboBox()
        Me.splitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.tvwRss = New System.Windows.Forms.TreeView()
        Me.webBrowser1 = New System.Windows.Forms.WebBrowser()
        Me.toolStrip1.SuspendLayout()
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.splitContainer1.Panel1.SuspendLayout()
        Me.splitContainer1.Panel2.SuspendLayout()
        Me.splitContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        'toolStrip1
        '
        Me.toolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsRssLocation, Me.tsRssGo, Me.tsCboFeeds})
        Me.toolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.toolStrip1.Name = "toolStrip1"
        Me.toolStrip1.Size = New System.Drawing.Size(762, 25)
        Me.toolStrip1.TabIndex = 2
        Me.toolStrip1.Text = "toolStrip1"
        '
        'tsRssLocation
        '
        Me.tsRssLocation.Name = "tsRssLocation"
        Me.tsRssLocation.Size = New System.Drawing.Size(400, 25)
        Me.tsRssLocation.Text = "http://rss.news.yahoo.com/rss/topstories"
        '
        'tsRssGo
        '
        Me.tsRssGo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsRssGo.Image = CType(resources.GetObject("tsRssGo.Image"), System.Drawing.Image)
        Me.tsRssGo.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsRssGo.Name = "tsRssGo"
        Me.tsRssGo.Size = New System.Drawing.Size(23, 22)
        Me.tsRssGo.Text = "rssGo"
        '
        'tsCboFeeds
        '
        Me.tsCboFeeds.Items.AddRange(New Object() {"http://rss.news.yahoo.com/rss/topstories", "http://feeds.reuters.com/news/artsculture", "http://rss.allrecipes.com/2/3.xml", "http://news.nationalgeographic.com/index.rss", "http://feeds.bbci.co.uk/news/science_and_environment/rss.xml?edition=int", "http://rss.allrecipes.com/2/7.xml", "http://feeds.abcnews.com/abcnews/politicsheadlines", "http://feeds.bbci.co.uk/news/business/rss.xml?edition=int", "http://www.nytimes.com/services/xml/rss/userland/Sports.xml", "http://feeds.gawker.com/lifehacker/full", "http://rss.nytimes.com/services/xml/rss/nyt/GlobalHome.xml", "http://rss.nytimes.com/services/xml/rss/nyt/MostShared.xml", "http://www.telegraph.co.uk/sport/othersports/basketball/rss", "http://www.telegraph.co.uk/sport/columnists/rss", "http://online.wsj.com/xml/rss/3_7031.xml", "http://images.apple.com/main/rss/hotnews/hotnews.rss", "http://www.nasa.gov/rss/breaking_news.rss", "http://news.cnet.com/2547-1009_3-0-10.xml", "http://searchenginewatch.com/sew.xml", "http://www.theregister.co.uk/headlines.rss", "http://www.ad.nl/cache/rss_digitaal.xml", "http://www.breedbandwinkel.nl/rss/nieuws.xml", "http://goarticles.com/feeds/category/Travel/58/recent.rss", "http://www.dhnet.be/rss", "http://info.abril.com.br/aberto/infonews/rssnews.xml", "http://www.ansa.it/main/notizie/awnplus/topnews/synd/ansait_awnplus_topnews_medsy" & _
                        "nd_Today_Idx.xml", "http://www.foxnews.com/about/rss/feedburner/foxnews/latest", "http://www.softpedia.com/backend.xml ", "http://rss.ew.com/web/ew/rss/todayslatest/index.xml", "http://mlb.mlb.com/partnerxml/gen/news/rss/mlb.xml", "http://bjsw.oxfordjournals.org/rss/mfc.xml", "http://feeds.feedburner.com/giveawayoftheday/feed", "http://feeds.feedburner.com/HowToGeek", "http://www.npr.org/rss/rss.php?id=1026", "http://feeds.feedburner.com/cnet/NnTv", "http://www.telegraph.co.uk/finance/rss", "http://www.telegraph.co.uk/news/uknews/rss", "http://feeds.reuters.com/reuters/technologyNews", "http://news.google.com/?output=rss", "http://rss.news.yahoo.com/rss/world", "http://rss.in.gr/feed/news/world/", "http://ws.kathimerini.gr/xml_files/news.xml", "http://rss.news.yahoo.com/rss/tech", "http://scitation.aip.org/rss/chaos1.xml", "http://www.washingtonpost.com/wp-dyn/rss/politics/elections/index.xml", "http://www.washingtonpost.com/wp-dyn/rss/nation/index.xml", "http://feeds.washingtonpost.com/wp-dyn/rss/nation/science/index_xml", "http://www.nytimes.com/services/xml/rss/nyt/JobMarket.xml", "http://www.rollingstone.com/siteServices/rss/allNews", "http://feeds.huffingtonpost.com/huffingtonpost/TheBlog", "http://www.nytimes.com/services/xml/rss/nyt/Business.xml", "http://feeds.sfgate.com/sfgate/rss/feeds/bayarea", "http://feeds.feedburner.com/cnet/rEwS", "http://bjsw.oxfordjournals.org/rss/current.xml", "http://bjsw.oxfordjournals.org/rss/ahead.xml", "http://bjsw.oxfordjournals.org/rss/mfr.xml", "http://feeds.feedburner.com/Mashable", "http://feeds.ogleearth.com/ogleearth", "http://i.rottentomatoes.com/syndication/rss/top_movies.xml", "http://i.rottentomatoes.com/syndication/rss/top_news.xml", "http://feeds.sfgate.com/sfgate/rss/feeds/business", "http://rss.slashdot.org/Slashdot/slashdot", "http://rss.slashdot.org/Slashdot/slashdotBookReviews,", "http://www.npr.org/rss/rss.php?id=1009", "http://feeds.sfgate.com/sfgate/rss/feeds/crime", "http://www.microsoft.com/windowsvista/rss.xml", "http://www.microsoft.com/atwork/community/rss.xml", "http://blogs.msdn.com/tiptalk/rss.xml", "http://msdn.microsoft.com/msdnmag/rss/newrss.aspx?issue=tue", "http://web.mit.edu/newsoffice/topic/space.feed"})
        Me.tsCboFeeds.Name = "tsCboFeeds"
        Me.tsCboFeeds.Size = New System.Drawing.Size(300, 25)
        '
        'splitContainer1
        '
        Me.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.splitContainer1.Location = New System.Drawing.Point(0, 25)
        Me.splitContainer1.Name = "splitContainer1"
        '
        'splitContainer1.Panel1
        '
        Me.splitContainer1.Panel1.AutoScroll = True
        Me.splitContainer1.Panel1.Controls.Add(Me.tvwRss)
        '
        'splitContainer1.Panel2
        '
        Me.splitContainer1.Panel2.Controls.Add(Me.webBrowser1)
        Me.splitContainer1.Size = New System.Drawing.Size(762, 398)
        Me.splitContainer1.SplitterDistance = 251
        Me.splitContainer1.TabIndex = 4
        '
        'tvwRss
        '
        Me.tvwRss.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tvwRss.Location = New System.Drawing.Point(0, 0)
        Me.tvwRss.Name = "tvwRss"
        Me.tvwRss.Size = New System.Drawing.Size(251, 398)
        Me.tvwRss.TabIndex = 0
        '
        'webBrowser1
        '
        Me.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.webBrowser1.Location = New System.Drawing.Point(0, 0)
        Me.webBrowser1.MinimumSize = New System.Drawing.Size(20, 20)
        Me.webBrowser1.Name = "webBrowser1"
        Me.webBrowser1.Size = New System.Drawing.Size(507, 398)
        Me.webBrowser1.TabIndex = 1
        '
        'frmRss
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(762, 423)
        Me.Controls.Add(Me.splitContainer1)
        Me.Controls.Add(Me.toolStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmRss"
        Me.Text = "RSS Feed Reader"
        Me.toolStrip1.ResumeLayout(False)
        Me.toolStrip1.PerformLayout()
        Me.splitContainer1.Panel1.ResumeLayout(False)
        Me.splitContainer1.Panel2.ResumeLayout(False)
        CType(Me.splitContainer1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.splitContainer1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents toolStrip1 As System.Windows.Forms.ToolStrip
    Private WithEvents tsRssLocation As System.Windows.Forms.ToolStripTextBox
    Private WithEvents tsRssGo As System.Windows.Forms.ToolStripButton
    Private WithEvents tsCboFeeds As System.Windows.Forms.ToolStripComboBox
    Private WithEvents splitContainer1 As System.Windows.Forms.SplitContainer
    Private WithEvents tvwRss As System.Windows.Forms.TreeView
    Private WithEvents webBrowser1 As System.Windows.Forms.WebBrowser

End Class
