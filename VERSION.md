# K-Browser Version Specification & Release History

**Application Title:** K-Browser  
**Executable / Assembly Name:** `K-Browser.exe`  
**Current Version:** `5.1.0.0` (`AssemblyVersion` / `AssemblyFileVersion`)  
**Git Release Tag:** `Version_5.1.0.0`  
**License:** GNU General Public License v3.0 (GPL-3.0)  
**Target Framework:** .NET Framework 4.8 / Windows Forms  
**Author:** Dennis Kon  

---

## 🏛️ Executive Solution Summary

**K-Browser** is a lightweight, secure, multi-tabbed web browser and desktop productivity platform built for Windows. It couples a modern rendering engine (**Microsoft WebView2** Chromium with fallback IE emulation registry control) with embedded privacy and utility tools, including a custom Trie-based AdBlocker Engine, UserScript Injection Host, Smart Tab Lifecycle Manager, Multi-Process Architecture Manager, Dark Mode & Modern Styling Engine, Modular Services Infrastructure, JSON Session State Manager, Compressed Backup & Restore Engine, Advanced Cookie Manager, Phishing Security Guard, RSS Feed Reader, FTP Client, Task Manager, and TCP Port Scanner.

---

## 📌 Current Version Breakdown: v5.1.0.0

### 1. ⚡ Smart Tab Suspending & Memory Saver ([TabLifecycleManager.vb](TabLifecycleManager.vb))
* **Background Tab Suspension:** Automatically suspends background inactive WebView2 tabs (`CoreWebView2.TrySuspendAsync()`) to pause background JavaScript timers, animation loops, and renderer activity, freeing RAM and CPU resources.
* **Instant Tab Resumption:** Intercepts tab selection changes (`TabControl1.SelectedIndexChanged`) to instantly resume (`CoreWebView2.Resume()`) the active tab.

### 2. 🧩 UserScript Injection Host ([UserScriptManager.vb](UserScriptManager.vb))
* **Greasemonkey / Tampermonkey Compatibility:** Parses `.user.js` script header metadata (`// ==UserScript==`, `@name`, `@match`, `@include`).
* **Document Creation Injection:** Registers user scripts in `%LocalAppData%\K-Browser\UserScripts\` to execute automatically on document creation via `CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync()`.

### 3. 🚀 Trie-Based AdBlock Engine ([AdBlockTrie.vb](AdBlockTrie.vb), [AdBlockEngine.vb](AdBlockEngine.vb))
* **$O(K)$ Prefix Tree Host Lookup:** Replaces $O(N)$ linear domain array scans with an $O(K)$ Prefix Tree (Trie) host lookup structure.
* **Microsecond Resource Interception:** Reduces ad rule evaluation overhead per network request to **< 0.05ms**, ensuring zero browsing lag on ad-heavy websites.

### 4. 🛡️ Site-Specific AdBlock Popup & Real-Time Audit Log ([AdBlockPopup.vb](AdBlockPopup.vb))
* **Toolbar Control Dialog:** Displays domain blocking status (`Status: ACTIVE` / `Status: DISABLED`), live blocked network request table with timestamps, site exemption toggle (`chkDisableOnSite`), and total application-wide blocked counters.

### 5. ⚙️ Decoupled Infrastructure & Services Layer ([SettingsService.vb](SettingsService.vb), [PrivacySettingsService.vb](PrivacySettingsService.vb), [PerformanceSettingsService.vb](PerformanceSettingsService.vb), [SettingsManager.vb](SettingsManager.vb))
* **JSON Preferences Storage:** Persists application settings to `%LocalAppData%\K-Browser\Settings.json` with async and thread-safe synchronous I/O.
* **Global Event Bus:** Fires `SettingsChanged` event via `SettingsManager` to notify UI forms and background services dynamically.
* **Tracking Prevention & Privacy Isolation:** Applies Chromium tracking prevention levels (`Strict` vs `Balanced`) and third-party cookie blocking across active WebView2 instances.
* **Performance Configuration:** Controls hardware acceleration (`--disable-gpu`), page preloading (`PredictivePrefetching`), and DNS prefetching, detecting required application restarts.

### 6. 🔄 Session Persistence & Crash Recovery ([SessionService.vb](SessionService.vb), [SessionModel.vb](SessionModel.vb), [TabItemModel.vb](TabItemModel.vb))
* **Automatic Session Saving:** Persists open tab titles, URLs, and active tab index into `%LocalAppData%\K-Browser\Session.json`.
* **Graceful Startup Restoration:** Restores tab session automatically upon browser launch when `StartupMode = "RestoreSession"`, with corrupted file fallback.

### 7. 🚀 Browser Initialization Switch Builder ([BrowserInitializationService.vb](BrowserInitializationService.vb))
* **Dynamic Chromium Arguments:** Constructs `CoreWebView2EnvironmentOptions` with tailored command-line switches based on graphics, preloading, and third-party cookie blocking flags.

### 8. 📦 Backup, Restoration & Settings Reset System ([BackupService.vb](BackupService.vb), [RestoreService.vb](RestoreService.vb))
* **ZIP Backup Generation:** Packages `Settings.json`, `Session.json`, bookmarks (`bookmarks.json`), history (`history.json`), and user preferences (`user_config.json`) into `BrowserBackup_YYYY-MM-DD.zip`.
* **Archive Validation & Restoration:** Validates ZIP structure before restoration and provides a safe default reset (`ResetSettingsAsync`) preserving user bookmarks/history.

### 9. 🍪 Advanced Cookie Interop & CSV Export Engine ([CookieService.vb](CookieService.vb), [CookieExportService.vb](CookieExportService.vb), [CookieItem.vb](CookieItem.vb))
* **WebView2 Cookie Interop:** Full async querying, search filtering, single-cookie deletion, and complete store clearing (`DeleteAllCookies`).
* **RFC 4180 CSV Export:** Asynchronously exports cookie records into formatted CSV files with automatic field escaping (`EscapeCsvField`).

### 10. 📄 Redesigned Page Source Viewer ([Source.vb](Source.vb))
* **Monospace Code Editor:** Uses `Consolas` / `Courier New` 10pt with custom tab stop indentation.
* **Toolbar & Search:** Live search bar (`txtSearch`) with match highlighting and status (`lblMatchCount`), 1-click clipboard copy (`📋 Copy All`), word wrap toggle, file export (`💾 Save As...`), and print support.
* **Live Status Strip:** Displays real-time cursor line and column (`Ln X, Col Y`), total line count, and character stats.

### 11. 🌙 Theme System Engine ([ThemeManager.vb](ThemeManager.vb))
* **Dynamic Dark/Light Modes:** `ThemeManager` controls global theme state (`IsDarkMode`, default Light Mode) with dynamic switching across all forms.
* **Native Windows DWM Dark Title Bar:** Uses `DwmSetWindowAttribute` to give Windows 10/11 title bars a native dark theme frame.
* **WebView2 Preferred Color Scheme:** Syncs `CoreWebView2.Profile.PreferredColorScheme` to `Dark` or `Light` for native `prefers-color-scheme: dark` web rendering.

---

## 📜 Version History & Release Matrix

| Version Tag | Release Date | Summary & Major Features Introduced | Key Architectural Changes |
| :--- | :--- | :--- | :--- |
| **v5.1.0.0** | *Current* | Smart Tab Suspending (`TabLifecycleManager`), UserScript Host (`UserScriptManager`), Trie AdBlock Engine (`AdBlockTrie`), Services Layer (`Settings`, `Session`, `Backup/Restore`, `Cookie` Services), Site-Specific AdBlock Popup (`AdBlockPopup`), Redesigned Page Source Viewer (`Source.vb`). | Introduced SOLID Services Layer, `Settings.json` & `Session.json` persistence, `BackupService.vb`, `RestoreService.vb`, `CookieService.vb`, `BrowserInitializationService.vb`, and `AdBlockPopup.vb`. |
| **v5.0.0.0** | Previous | Dark Mode Engine (`ThemeManager`), Native DWM Dark Titlebar, PreferredColorScheme Sync, Multi-Process Architecture (`TabProcessManager`), Process Isolation & Crash Resilience, EasyList AdBlocker CSS protection, Date-filtered History. | Introduced `ThemeManager.vb`, `TabProcessManager.vb`, `WebView2_ProcessFailed`, `ShouldBlockDomainOnly()`, `GenericWebPaths`, and `"Title|URL|DateTime"` history. |
| **v4.8.1** | Legacy | Engine stabilization, Visual Studio project setup, URL validation, process termination safety, tab disposal cleanup. | Migrated core browser runtime towards WebView2 integration with IE emulation fallback. |
| **v4.8.0** | Legacy | Multi-tab UI overhaul, initial phishing filter dialog, enhanced `My.Settings` storage schemas. | TabControl event architecture, `Form1.Designer.vb` restructuring. |
| **v1.0.0 - v4.5** | Legacy | Initial browser releases, FTP client, Task Manager, RSS parser, Cookie Viewer. | Original Windows Forms codebase architecture by Dennis Kon. |
