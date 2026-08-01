# K-Browser Version Specification & Release History

**Application Title:** K-Browser  
**Executable / Assembly Name:** `K-Browser.exe`  
**Current Version:** `4.8.1.1` (`AssemblyVersion` / `AssemblyFileVersion`)  
**Git Release Tag:** `Version_4.8.1`  
**License:** GNU General Public License v3.0 (GPL-3.0)  
**Target Framework:** .NET Framework 4.8 / Windows Forms  
**Author:** Dennis Kon  

---

## 🏛️ Executive Solution Summary

**K-Browser** is a lightweight, secure, multi-tabbed web browser and desktop productivity platform built for Windows. It couples a modern rendering engine (**Microsoft WebView2** Chromium with fallback IE emulation registry control) with embedded privacy and utility tools, including a custom multi-tier AdBlocker Engine, Phishing Security Guard, RSS Feed Reader, FTP Client, Task Manager, Cookie Manager, and TCP Port Scanner.

---

## 📌 Current Version Breakdown: v5.0.0.0

### 1. 🌐 Web Rendering Engine Modernization
* **WebView2 Integration:** Uses `Microsoft.Web.WebView2.WinForms` as the primary web rendering engine, providing Chromium-speed performance, modern Web standard support, and DevTools integration (`CoreWebView2.OpenDevToolsWindow()`).
* **Registry Emulation Control:** Automatically configures Windows Registry entries under `HKCU\Software\Microsoft\Internet Explorer\Main\FeatureControl` (`FEATURE_BROWSER_EMULATION`, `FEATURE_GPU_RENDERING`, `FEATURE_NATIVE_DOCUMENT_MODE`, `FEATURE_SCRIPT_URL_MITIGATION`) for fallback compatibility across host executables (`K-Browser.exe`, `devenv.exe`).
* **User-Agent Customization:** Overrides standard browser user-agent strings to modern Chrome/Edge Windows 10 standard signatures.
* **Favicon Caching:** Built-in bounded favicon cache (`MAX_FAVICON_CACHE = 100`) preventing memory leaks during long browsing sessions.

### 2. 🛡️ AdBlocker & Privacy Engine (`AdBlockEngine.vb`, `AdBlockerSettings.vb`)
* **Triple-Tier Rule Engine:**
  1. **Whitelist / Allow Rules (`@@`):** Evaluated first to ensure user-whitelisted domains/URLs are never blocked.
  2. **Domain Anchors (`||`):** High-efficiency host-matching rules (`doubleclick.net`, `adservice.google.com`, `googlesyndication.com`, etc.) used for document and sub-resource blocking.
  3. **Ad Path Substrings:** Sub-resource filter matching for ad paths, scripts, banners, and telemetry endpoints.
* **Reserved Generic Keyword Protection:** Guard list preventing over-blocking of standard top-level domains (`.com`, `.org`, `.io`), web protocols (`http`, `https`), and asset paths (`/assets/`, `/js/`, `/css/`).
* **Online Filter List Subscriptions:** Asynchronous downloading (`UpdateFilterListAsync`) and local disk caching of standard blocklists (**EasyList**, **EasyPrivacy**, **Fanboy's Annoyance**).
* **Live Counter:** Global thread-safe blocked request counter (`TotalBlockedCount`) surfaced in the UI.

### 3. 🔍 Browsing History & Filtering (`History.vb`)
* **Date-Range Filtering:** Added `DateTimePicker` range controls to filter visited URLs by specific start and end dates.
* **Live Query Search:** Instant substring search across `My.Settings.History` records.
* **Single-Click Navigation & Clearing:** Quick navigation back to historical URLs and one-click history purge.

### 4. 🔖 Bookmarks Management (`Bookmarks.vb`)
* **Enhanced DataGrid Display:** Improved layout, category organization, and URL launching from the Bookmarks form.
* **Search & Edit Capabilities:** Live filtering of saved bookmarks and persistent storage in `My.Settings.Bookmarks`.

### 5. 🔐 Security & Navigation Guards (`Settings.vb`, `Phising.vb`)
* **Phishing URL Guard:** Intercepts navigating events against known phishing domain lists (`My.Settings.PhishingSites`).
* **Modal Alert Dialog:** Shows a warnings dialog (`Phising.vb`) offering immediate navigation cancellation or session-level URL ignoring (`IgnoredUrls` cache).
* **Pop-Up & Domain Blocklist:** Configurable domain blocker with exception whitelisting (`My.Settings.AllowedPopSites`).
* **TCP Port Scanner:** Embedded multi-threaded network diagnostic utility inside `Settings.vb` to scan host ports for active listeners.

### 6. 🛠️ Desktop Utility Suite
* **FTP Client (`ftp.vb`):** Full-featured client for FTP server connections, directory browsing, file uploads, file downloads, and remote file deletion.
* **Task Manager (`task manager.vb`):** Live Windows process list with memory usage display (KB/MB) and process kill capability (`Process.Kill()`).
* **RSS Reader (`Rss.vb`):** XML parser converting RSS feeds into interactive hierarchical TreeView headline lists.
* **Cookie Viewer (`CookieViewer.vb`):** Inspects local browser cookies and enables selective or bulk deletion.

---

## 📜 Version History & Release Matrix

| Version Tag | Release Date | Summary & Major Features Introduced | Key Architectural Changes |
| :--- | :--- | :--- | :--- |
| **v4.8.1.1** | *Current* | EasyList AdBlocker engine, Date Range History Filter, Bookmark UI overhaul, Reserved keyword security guards. | Introduced `AdBlockEngine.vb`, async filter downloader, and reserved keyword safety checks. |
| **v4.8.1** | *Previous Tag* | Engine stabilization, Visual Studio project setup, URL validation, process termination safety, tab disposal cleanup. | Migrated core browser runtime towards WebView2 integration with IE emulation fallback. |
| **v4.8.0** | 2023 / 2024 | Multi-tab UI overhaul, initial phishing filter dialog, enhanced `My.Settings` storage schemas. | TabControl event architecture, `Form1.Designer.vb` restructuring. |
| **v4.5.0** | Legacy | Integrated Task Manager form, RSS XML parsing engine, and Cookie Viewer utility. | Extended WinForms desktop utility suite beyond standard web browser boundaries. |
| **v4.0.0** | Legacy | Added built-in FTP client, print preview, page zoom control, and TCP port scanner in settings. | Added `ftp.vb` asynchronous socket operations and network utilities. |
| **v1.0.0 - v3.x** | 2008 - 2013 | Initial tabbed browser built on IE WebBrowser control, bookmark management, basic history. | Original Windows Forms codebase architecture by Dennis Kon. |

---

## 📦 Component & Dependency Matrix

| Component / Library | Version | Scope / Purpose |
| :--- | :--- | :--- |
| **Visual Basic .NET** | 14.0+ (VB 2015/2019/2022) | Primary Application Source Language |
| **.NET Framework** | 4.8 / 4.7.2+ | Runtime Host Environment |
| **Microsoft.Web.WebView2** | Latest NuGet | Modern Chromium Web Rendering Engine |
| **AxWMPLib / WMPLib** | COM Interop | Windows Media Player Multimedia Support (`Form3`, `Form12`) |
| **System.Windows.Forms** | .NET Standard | Desktop User Interface Framework |
| **My.Settings** | Local App Data | Application Settings & User Preferences Persistence |

---

## 📐 Semantic Versioning Policy (`MAJOR.MINOR.PATCH.BUILD`)

K-Browser follows [Semantic Versioning (SemVer 2.0.0)](https://semver.org/) extended with a 4th Build segment for assembly compatibility:

$$\text{Version Format: } \mathbf{MAJOR.MINOR.PATCH.BUILD}$$

1. **MAJOR (X.0.0.0):** Incremented for major architecture overhauls (e.g., full migration from .NET Framework to .NET 8, complete redesign of UI framework).
2. **MINOR (4.X.0.0):** Incremented when significant new features or utility tools are added (e.g., AdBlocker engine, WebView2 engine upgrade, new built-in tools).
3. **PATCH (4.8.X.0):** Incremented for bug fixes, performance optimizations, security guard patches, and refactoring without breaking existing features.
4. **BUILD (4.8.1.X):** Incremented for incremental build releases, minor hotfixes, and maintenance deployments.

---

## 🔮 Future Architecture Roadmap

### 🎯 v4.9.0 (Planned Short-Term Release)
* **SQLite Persistence Migration:** Replace `My.Settings` StringCollections for History and Bookmarks with an embedded SQLite or LiteDB database for faster search indexing and lower memory footprint.
* **Background Worker Rule Parsing:** Offload large blocklist file parsing to background worker threads to keep the UI thread 100% responsive during ad list updates.
* **User-Customizable Search Engines:** Allow custom OpenSearch definitions and arbitrary search engine URLs.

### 🚀 v5.0.0 (Planned Major Milestone)
* **.NET 8 / WinForms Modernization:** Upgrade project file format to SDK-style `.vbproj` targeting modern .NET 8 / 9.
* **Multi-Process Architecture:** Isolate tab webviews into separate OS processes for crash resilience and enhanced memory isolation.
* **Dark Mode & Modern Styling:** Complete visual overhaul adopting dark theme support and modern fluent design UI controls.
