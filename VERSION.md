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

**K-Browser** is a lightweight, secure, multi-tabbed web browser and desktop productivity platform built for Windows. It couples a modern rendering engine (**Microsoft WebView2** Chromium with fallback IE emulation registry control) with embedded privacy and utility tools, including a custom Trie-based AdBlocker Engine, UserScript Injection Host, Smart Tab Lifecycle Manager, Multi-Process Architecture Manager, Dark Mode & Modern Styling Engine, Phishing Security Guard, RSS Feed Reader, FTP Client, Task Manager, Cookie Manager, and TCP Port Scanner.

---

## 📌 Current Version Breakdown: v5.1.0.0

### 1. ⚡ Smart Tab Suspending & Memory Saver (`TabLifecycleManager.vb`)
* **Background Tab Suspension:** Automatically suspends background inactive WebView2 tabs (`CoreWebView2.TrySuspendAsync()`) to pause background JavaScript timers, animation loops, and renderer activity, freeing RAM and CPU resources.
* **Instant Tab Resumption:** Intercepts tab selection changes (`TabControl1.SelectedIndexChanged`) to instantly resume (`CoreWebView2.Resume()`) the active tab.

### 2. 🧩 UserScript Injection Host (`UserScriptManager.vb`)
* **Greasemonkey / Tampermonkey Compatibility:** Parses `.user.js` script header metadata (`// ==UserScript==`, `@name`, `@match`, `@include`).
* **Document Creation Injection:** Registers user scripts in `%LocalAppData%\K-Browser\UserScripts\` to execute automatically on document creation via `CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync()`.

### 3. 🚀 Trie-Based AdBlock Engine (`AdBlockTrie.vb`, `AdBlockEngine.vb`)
* **$O(K)$ Prefix Tree Host Lookup:** Replaces $O(N)$ linear domain array scans with an $O(K)$ Prefix Tree (Trie) host lookup structure.
* **Microsecond Resource Interception:** Reduces ad rule evaluation overhead per network request to **< 0.05ms**, ensuring zero browsing lag on ad-heavy websites.

### 4. 📄 Redesigned Page Source Viewer (`Source.vb`, `Source.designer.vb`)
* **Monospace Code Editor:** Uses `Consolas` / `Courier New` 10pt with custom tab stop indentation.
* **Toolbar & Search:** Live search bar (`txtSearch`) with match highlighting and status (`lblMatchCount`), 1-click clipboard copy (`📋 Copy All`), word wrap toggle, file export (`💾 Save As...`), and print support.
* **Live Status Strip:** Displays real-time cursor line and column (`Ln X, Col Y`), total line count, and character stats.

### 5. 🌙 Theme System Engine (`ThemeManager.vb`)
* **Dynamic Dark/Light Modes:** `ThemeManager` controls global theme state (`IsDarkMode`, default Light Mode) with dynamic switching across all forms.
* **Native Windows DWM Dark Title Bar:** Uses `DwmSetWindowAttribute` to give Windows 10/11 title bars a native dark theme frame.
* **WebView2 Preferred Color Scheme:** Syncs `CoreWebView2.Profile.PreferredColorScheme` to `Dark` or `Light` for native `prefers-color-scheme: dark` web rendering.

---

## 📜 Version History & Release Matrix

| Version Tag | Release Date | Summary & Major Features Introduced | Key Architectural Changes |
| :--- | :--- | :--- | :--- |
| **v5.1.0.0** | *Current* | Smart Tab Suspending (`TabLifecycleManager`), UserScript Injection Host (`UserScriptManager`), Trie-based AdBlock Engine (`AdBlockTrie`), Redesigned Page Source Viewer (`Source.vb`). | Added `TabLifecycleManager.vb`, `UserScriptManager.vb`, `AdBlockTrie.vb`, and $O(K)$ Trie lookups. |
| **v5.0.0.0** | Previous | Dark Mode Engine (`ThemeManager`), Native DWM Dark Titlebar, PreferredColorScheme Sync, Multi-Process Architecture (`TabProcessManager`), Process Isolation & Crash Resilience, EasyList AdBlocker CSS protection, Date-filtered History. | Introduced `ThemeManager.vb`, `TabProcessManager.vb`, `WebView2_ProcessFailed`, `ShouldBlockDomainOnly()`, `GenericWebPaths`, and `"Title|URL|DateTime"` history. |
| **v4.8.1** | Legacy | Engine stabilization, Visual Studio project setup, URL validation, process termination safety, tab disposal cleanup. | Migrated core browser runtime towards WebView2 integration with IE emulation fallback. |
| **v4.8.0** | Legacy | Multi-tab UI overhaul, initial phishing filter dialog, enhanced `My.Settings` storage schemas. | TabControl event architecture, `Form1.Designer.vb` restructuring. |
| **v1.0.0 - v4.5** | Legacy | Initial browser releases, FTP client, Task Manager, RSS parser, Cookie Viewer. | Original Windows Forms codebase architecture by Dennis Kon. |
