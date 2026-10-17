''' <summary>
''' Fluent UI System Icons used by K-Browser.
''' </summary>
Public Enum FluentIcon
    ArrowClockwise
    ArrowDownload
    ArrowEnter
    ArrowLeft
    ArrowRight
    ArrowSync
    ArrowUpload
    Bookmark
    Box
    Broom
    Checkmark
    Cookies
    Copy
    Cut
    DataUsage
    Delete
    Dismiss
    DismissCircle
    Document
    Eye
    EyeOff
    Globe
    GlobeDesktop
    History
    Home
    Incognito
    Info
    Key
    LockClosed
    Open
    Password
    People
    Person
    Print
    Rss
    Save
    Screenshot
    Search
    Send
    Server
    Settings
    Shield
    SignOut
    Star
    TabAdd
    TextWrap
    Video
    Warning
    WeatherMoon
    WindowNew
    ZoomIn
End Enum

''' <summary>
''' Code points of each icon's size-specific regular designs in Assets/Fonts/FluentSystemIcons-Regular.ttf,
''' generated from FluentSystemIcons-Regular.json at fluentui-system-icons commit
''' a347af95478f989038cccbf085978865264c048a. Each entry is (design size in px, code point), ascending by size.
''' </summary>
Friend Module FluentIconGlyphs

    Public ReadOnly Designs As New Dictionary(Of FluentIcon, (Size As Integer, CodePoint As Integer)()) From {
        {FluentIcon.ArrowClockwise, {(12, 57513), (16, 57514), (20, 61757), (24, 61758), (28, 57515), (32, 57516), (48, 57517)}},
        {FluentIcon.ArrowDownload, {(16, 61775), (20, 61776), (24, 61777), (28, 983936), (32, 983937), (48, 61778)}},
        {FluentIcon.ArrowEnter, {(16, 983391), (20, 57535)}},
        {FluentIcon.ArrowLeft, {(12, 57559), (16, 61828), (20, 61787), (24, 61788), (28, 61789), (32, 61833), (48, 61854)}},
        {FluentIcon.ArrowRight, {(12, 57578), (16, 57579), (20, 61825), (24, 61826), (28, 61827), (32, 61858), (48, 61863)}},
        {FluentIcon.ArrowSync, {(12, 61839), (16, 57616), (20, 61840), (24, 61841)}},
        {FluentIcon.ArrowUpload, {(16, 57649), (20, 61860), (24, 61861), (32, 984480)}},
        {FluentIcon.Bookmark, {(12, 985911), (16, 61940), (20, 61941), (24, 61942), (28, 61943), (32, 57745)}},
        {FluentIcon.Box, {(16, 57786), (20, 57787), (24, 57788), (28, 985322), (32, 985323), (48, 985324)}},
        {FluentIcon.Broom, {(16, 61950), (20, 61953), (24, 61954), (28, 57860), (32, 984105)}},
        {FluentIcon.Checkmark, {(12, 62099), (16, 58117), (20, 62100), (24, 62101), (28, 62102), (32, 63700), (48, 58118)}},
        {FluentIcon.Cookies, {(16, 984301), (20, 62248), (24, 62249), (28, 984302), (32, 984303), (48, 984304)}},
        {FluentIcon.Copy, {(16, 62250), (20, 62251), (24, 62252), (28, 985045), (32, 983046), (48, 986223)}},
        {FluentIcon.Cut, {(16, 983948), (20, 62266), (24, 62267), (28, 985527), (32, 985528), (48, 985529)}},
        {FluentIcon.DataUsage, {(16, 984420), (20, 58463), (24, 62281), (28, 984921), (32, 984922), (48, 984923)}},
        {FluentIcon.Delete, {(12, 61093), (16, 58491), (20, 62284), (24, 62285), (28, 62286), (32, 61094), (48, 62287)}},
        {FluentIcon.Dismiss, {(12, 62311), (16, 62312), (20, 62313), (24, 62314), (28, 62315), (32, 62451), (48, 62464)}},
        {FluentIcon.DismissCircle, {(12, 58561), (16, 62316), (20, 62317), (24, 62318), (28, 58562), (32, 58563), (48, 62319)}},
        {FluentIcon.Document, {(16, 58583), (20, 62328), (24, 62329), (28, 62330), (32, 58584), (48, 58585)}},
        {FluentIcon.Eye, {(12, 58864), (16, 58865), (20, 58866), (24, 58867), (28, 61384), (32, 61385), (48, 61386)}},
        {FluentIcon.EyeOff, {(16, 58868), (20, 58869), (24, 58870), (32, 984787)}},
        {FluentIcon.Globe, {(12, 983435), (16, 59057), (20, 62554), (24, 62555), (28, 984333), (32, 59058), (48, 983662)}},
        {FluentIcon.GlobeDesktop, {(20, 59062), (24, 62558)}},
        {FluentIcon.History, {(16, 59141), (20, 62590), (24, 62591), (28, 59142), (32, 59143), (48, 59144)}},
        {FluentIcon.Home, {(12, 59150), (16, 59151), (20, 62592), (24, 62593), (28, 62594), (32, 59152), (48, 59153)}},
        {FluentIcon.Incognito, {(12, 986246), (16, 986212), (20, 59189), (24, 62625), (28, 986213), (32, 985494), (48, 985495)}},
        {FluentIcon.Info, {(12, 59190), (16, 62626), (20, 62627), (24, 62628), (28, 62629), (32, 983129), (48, 983130)}},
        {FluentIcon.Key, {(16, 59200), (20, 62646), (24, 62647), (32, 59201)}},
        {FluentIcon.LockClosed, {(12, 59277), (16, 59278), (20, 59279), (24, 59280), (28, 983583), (32, 59281), (48, 983584)}},
        {FluentIcon.Open, {(12, 983749), (16, 62849), (20, 62850), (24, 62851), (28, 59534), (32, 63079), (48, 59535)}},
        {FluentIcon.Password, {(16, 59592), (20, 59593), (24, 62878), (32, 984494), (48, 984495)}},
        {FluentIcon.People, {(12, 59627), (16, 62887), (20, 62888), (24, 62889), (28, 62890), (32, 59628), (48, 59629)}},
        {FluentIcon.Person, {(12, 62907), (16, 62908), (20, 62909), (24, 62910), (28, 62911), (32, 59675), (48, 62912)}},
        {FluentIcon.Print, {(16, 63226), (20, 63018), (24, 63019), (28, 59852), (32, 59853), (48, 63020)}},
        {FluentIcon.Rss, {(16, 983958), (20, 59965), (24, 59966)}},
        {FluentIcon.Save, {(16, 59971), (20, 63103), (24, 63104), (28, 59972), (32, 984441)}},
        {FluentIcon.Screenshot, {(16, 984082), (20, 63117), (24, 63118), (28, 984156)}},
        {FluentIcon.Search, {(12, 60027), (16, 60028), (20, 63119), (24, 63120), (28, 63121), (32, 60029), (48, 60030)}},
        {FluentIcon.Send, {(16, 60046), (20, 63129), (24, 63130), (28, 63131), (32, 983192), (48, 983193)}},
        {FluentIcon.Server, {(12, 983904), (16, 61916), (20, 63337), (24, 63340)}},
        {FluentIcon.Settings, {(16, 63144), (20, 63145), (24, 63146), (28, 63147), (32, 60052), (48, 60053)}},
        {FluentIcon.Shield, {(12, 61983), (16, 60099), (20, 63166), (24, 63167), (28, 60100), (32, 61216), (48, 60101)}},
        {FluentIcon.SignOut, {(20, 60154), (24, 63194)}},
        {FluentIcon.Star, {(12, 63245), (16, 63246), (20, 63247), (24, 63248), (28, 63249), (32, 984158), (48, 60319)}},
        {FluentIcon.TabAdd, {(20, 60389), (24, 60390), (32, 984777)}},
        {FluentIcon.TextWrap, {(16, 61224), (20, 60777), (24, 63502)}},
        {FluentIcon.Video, {(16, 63563), (20, 63564), (24, 63565), (28, 63566), (32, 60912), (48, 60916)}},
        {FluentIcon.Warning, {(12, 60987), (16, 63592), (20, 63593), (24, 63594), (28, 60988), (32, 983626), (48, 984015)}},
        {FluentIcon.WeatherMoon, {(12, 986197), (16, 60996), (20, 63613), (24, 63614), (28, 60997), (32, 985064), (48, 63615)}},
        {FluentIcon.WindowNew, {(16, 61050), (20, 63677), (24, 61051), (28, 985971), (32, 985972), (48, 985973)}},
        {FluentIcon.ZoomIn, {(16, 61070), (20, 63684), (24, 63685), (28, 985738), (32, 985585)}}
    }

End Module
