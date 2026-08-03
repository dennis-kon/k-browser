# K-Browser Codebase Documentation

K-Browser is a lightweight, high-performance tabbed web browser application built using Visual Basic .NET (VB.NET) and Windows Forms. It combines standard browsing capabilities with advanced desktop utilities and a clean, SOLID-compliant modular architecture. Key features include Multi-Process WebView2 Architecture, Smart Tab Lifecycles, Greasemonkey UserScript Host, $O(K)$ Prefix Trie AdBlocker Engine, Site-Specific AdBlock Popup Controls, Native DWM Dark Mode, JSON Session State Persistence & Recovery, Compressed Backup & Restore System, and Advanced Cookie Management with RFC 4180 CSV Export.

---

## ✨ New Features & Implementations Overview

The codebase incorporates a modern, decoupled **Services Infrastructure & Data Layer** adhering to SOLID principles and Clean Architecture guidelines:

### 1. ⚙️ Modular Services & JSON Data Layer
* **[SettingsService.vb](SettingsService.vb):** Infrastructure service managing async and thread-safe synchronous I/O for `%LocalAppData%\K-Browser\Settings.json`. Prevents UI deadlocks during form initialization and shutdown.
* **[SettingsManager.vb](SettingsManager.vb):** Global event notification bus (`SettingsChanged`) allowing UI forms and background services to react instantly when user configuration is updated.
* **[PrivacySettingsService.vb](PrivacySettingsService.vb):** Coordinates privacy settings and applies Chromium tracking prevention levels (`Strict` vs `Balanced`) and third-party cookie blocking across active WebView2 instances.
* **[PerformanceSettingsService.vb](PerformanceSettingsService.vb):** Manages graphics and resource parameters (hardware acceleration, page preloading, DNS prefetching) and determines restart requirements (`RequiresRestart`).

### 2. 🔄 Session Persistence & Crash Recovery
* **[SessionService.vb](SessionService.vb):** Persists open tab URLs, page titles, and active index into `%LocalAppData%\K-Browser\Session.json`. Automatically restores sessions on browser startup if enabled in settings (`StartupMode = "RestoreSession"`), with graceful fallback handling for corrupted files.
* **[SessionModel.vb](SessionModel.vb) & [TabItemModel.vb](TabItemModel.vb):** Clean DTOs for tab state serialization.

### 3. 🚀 Browser Initialization & Command-Line Switch Engine
* **[BrowserInitializationService.vb](BrowserInitializationService.vb):** Builds `CoreWebView2EnvironmentOptions` and configures Chromium command-line switches (`--disable-gpu`, `--disable-gpu-compositing`, `--enable-features=PredictivePrefetching`, `--dns-prefetch-disable`, `--block-third-party-cookies`) dynamically based on performance and privacy profiles.

### 4. 📦 Backup, Restoration & Reset Engine
* **[BackupService.vb](BackupService.vb):** Creates compressed ZIP archives (`BrowserBackup_YYYY-MM-DD.zip`) packaging `Settings.json`, `Session.json`, bookmarks (`bookmarks.json`), history (`history.json`), and user preferences (`user_config.json`). Updates `LastBackup` ISO-8601 timestamps.
* **[RestoreService.vb](RestoreService.vb):** Validates ZIP archives before extraction and safely restores configuration, bookmarks, history, and preferences. Provides a non-destructive default reset (`ResetSettingsAsync`) that preserves user bookmarks and history while resetting configuration flags.

### 5. 🍪 Advanced Cookie Interop & Export Engine
* **[CookieService.vb](CookieService.vb):** Interop layer wrapping `CoreWebView2CookieManager` for async cookie fetching, domain filtering, single-cookie deletion, and complete store clearing (`DeleteAllCookies`).
* **[CookieExportService.vb](CookieExportService.vb):** Asynchronously exports cookie list records into RFC 4180 compliant CSV files with automatic field escaping (`EscapeCsvField`).
* **[CookieItem.vb](CookieItem.vb):** Strongly-typed cookie DTO supporting case-insensitive multi-field search (`MatchesSearch`).

### 6. 🛡️ Interactive AdBlock Popup UI
* **[AdBlockPopup.vb](AdBlockPopup.vb):** A site-specific popup dialog launched from the browser toolbar allowing users to view current domain status (`Status: ACTIVE` / `Status: DISABLED`), toggle ad blocking per domain (`chkDisableOnSite`), inspect live blocked network request URLs with timestamps, and monitor total application-wide blocked ad counts.

---

## 🏛️ Application Architecture & Component Diagram

The following diagram illustrates how the main form, sub-forms, services, data persistence models, ad block engine, and WebView2 instances interact within the application:

```mermaid
graph TD
    Form1[Form1 - Main Browser Window] --> CreateNewTab[CreateNewTab Helper]
    Form1 --> ThemeManager[ThemeManager - Dark Mode & Fluent Styling]
    Form1 --> TabProcessManager[TabProcessManager - Process Isolation]
    Form1 --> TabLifecycleManager[TabLifecycleManager - Tab Suspension & Memory Saver]
    Form1 --> UserScriptManager[UserScriptManager - Greasemonkey Script Host]
    Form1 --> SessionService[SessionService - Session.json Persistence]
    Form1 --> AppManager[AppManager - URL/Regex Utils]
    
    Form1 --> Source[Source Form - Modern Code Viewer]
    Form1 --> Bookmarks[Bookmarks Form]
    Form1 --> History[History Form]
    Form1 --> Settings[Settings Form - Central Configuration]
    Form1 --> AdBlockerSettings[AdBlockerSettings Form]
    Form1 --> AdBlockPopup[AdBlockPopup - Site Controls & Live Log]
    Form1 --> CookieViewer[CookieViewer Form]
    Form1 --> Rss[Rss Form]
    Form1 --> Ftp[ftp Form]
    Form1 --> TaskManager[task_manager Form]
    
    %% Services & Persistence Layer
    TabProcessManager --> InitService[BrowserInitializationService - Switch Builder]
    Settings --> SettingsService[SettingsService - Settings.json]
    Settings --> PerfService[PerformanceSettingsService]
    Settings --> PrivacyService[PrivacySettingsService]
    Settings --> BackupService[BackupService - ZIP Exporter]
    Settings --> RestoreService[RestoreService - ZIP Importer]
    SettingsService --> SettingsManager[SettingsManager - Event Bus]
    
    CookieViewer --> CookieService[CookieService - CoreWebView2CookieManager Interop]
    CookieViewer --> CookieExportService[CookieExportService - RFC 4180 CSV Export]
    
    %% Interceptors, Guards & WebView2
    ThemeManager -- Native DWM Dark Titlebar --> Form1
    ThemeManager -- PreferredColorScheme --> WebView2[CoreWebView2 Profile]
    Form1 -- Resource Request Guard --> AdBlockEngine[AdBlockEngine - EasyList Filter & AdBlockTrie]
    AdBlockEngine --> AdBlockTrie[AdBlockTrie - O(K) Host Trie]
    AdBlockPopup --> AdBlockEngine
    Form1 -- Navigating Guard --> BlockedSites{Blocked Sites List}
    Form1 -- Navigating Guard --> Phising[Phising Warning Dialog]
    Form1 -- Process Failure Guard --> ProcessFailed[WebView2 ProcessFailed Handler]
```

---

## 📂 Codebase Component Registry

| Component File | Type | Description |
| :--- | :--- | :--- |
| [Form1.vb](Form1.vb) | Form | **Main Form**. Coordinates tab creation using `TabProcessManager`, tab memory suspending using `TabLifecycleManager`, session saving/restoring using `SessionService`, UserScript injection using `UserScriptManager`, theme state using `ThemeManager`, navigation, WebView2 event handlers, `ProcessFailed` crash recovery, and popup windows. |
| [BrowserInitializationService.vb](BrowserInitializationService.vb) | Class (Service) | **Browser Initialization Options Builder**. Constructs `CoreWebView2EnvironmentOptions` and configures Chromium command-line switches (`--disable-gpu`, `--enable-features=PredictivePrefetching`, `--dns-prefetch-disable`, `--block-third-party-cookies`). |
| [PerformanceSettingsService.vb](PerformanceSettingsService.vb) | Class (Service) | **Performance Settings Manager**. Handles loading, saving, and applying graphics acceleration, tab saver, and page preloading rules. Detects restart requirements via `RequiresRestart()`. |
| [PrivacySettingsService.vb](PrivacySettingsService.vb) | Class (Service) | **Privacy & Security Settings Service**. Coordinates privacy options, configuring Chromium tracking prevention levels (`Strict`/`Balanced`) and third-party cookie isolation on active WebView2 controls. |
| [SettingsService.vb](SettingsService.vb) | Class (Service) | **Settings I/O Infrastructure Service**. Handles asynchronous and thread-safe synchronous JSON serialization/deserialization for `%LocalAppData%\K-Browser\Settings.json`. |
| [SettingsManager.vb](SettingsManager.vb) | Class (Service) | **Global Settings Event Bus**. Exposes `SettingsChanged` event to instantly notify UI forms and background workers when user options are modified. |
| [SessionService.vb](SessionService.vb) | Class (Service) | **Session State Persistence Service**. Saves open tab titles, URLs, and active index to `%LocalAppData%\K-Browser\Session.json`, and handles session restoration on browser startup with corrupted file recovery. |
| [BackupService.vb](BackupService.vb) | Class (Service) | **Backup ZIP Engine**. Packages `Settings.json`, `Session.json`, bookmarks (`bookmarks.json`), history (`history.json`), and user options into compressed ZIP archives (`BrowserBackup_YYYY-MM-DD.zip`). |
| [RestoreService.vb](RestoreService.vb) | Class (Service) | **Restore & Reset Service**. Validates and extracts backup ZIP archives, restoring user preferences, and provides default setting resets without erasing user bookmarks/history. |
| [CookieService.vb](CookieService.vb) | Class (Service) | **WebView2 Cookie Interop Service**. Interacts with `CoreWebView2CookieManager` for async cookie querying, search filtering, individual cookie removal, and bulk cookie deletion. |
| [CookieExportService.vb](CookieExportService.vb) | Class (Service) | **CSV Export Engine**. Asynchronously exports cookie list records into RFC 4180 compliant CSV files with automatic field escaping. |
| [TabLifecycleManager.vb](TabLifecycleManager.vb) | Class | **Smart Tab Lifecycle & Memory Manager**. Suspends inactive background WebView2 tabs (`CoreWebView2.TrySuspendAsync()`) to free RAM and CPU timers, and instantly resumes active tabs (`CoreWebView2.Resume()`) upon selection. |
| [UserScriptManager.vb](UserScriptManager.vb) | Class | **UserScript Injection Host**. Loads `.user.js` Greasemonkey/Tampermonkey scripts from `%LocalAppData%\K-Browser\UserScripts\`, parses `@match` rules, and registers scripts to execute on document creation (`CoreWebView2.AddScriptToExecuteOnDocumentCreatedAsync`). |
| [AdBlockTrie.vb](AdBlockTrie.vb) | Class | **Prefix Tree (Trie) Ad Matching Engine**. Provides $O(K)$ host-anchored ad rule lookups, accelerating ad evaluation per resource request. |
| [AdBlockEngine.vb](AdBlockEngine.vb) | Class | **AdBlocker Engine**. Integrated with `AdBlockTrie` for fast host matching. Features stylesheet/font protection (`ShouldBlockDomainOnly`), domain-option filtering (`$domain=`), site exclusion list, generic path safeguards, and async list updater. |
| [AdBlockPopup.vb](AdBlockPopup.vb) | Form | **Site-Specific AdBlock Popup UI**. Toolbar dialog displaying active domain blocking status (`ACTIVE`/`DISABLED`), live blocked resource request table with timestamps, site exemption toggle, and total blocked counters. |
| [Source.vb](Source.vb) | Form | **Redesigned Page Source Viewer**. Modern code viewer featuring Consolas/Courier monospace code editor, live search bar with match highlighting, 1-click clipboard copy with visual feedback, Word Wrap toggle, file export ("Save As..."), line/column tracking, character stats, and ThemeManager styling. |
| [ThemeManager.vb](ThemeManager.vb) | Class | **Theme System & Modern Styling Engine**. Handles global theme state (`IsDarkMode`), applies native Windows 10/11 DWM dark window title bar handles (`DwmSetWindowAttribute`), recursively styles WinForms controls, and syncs `CoreWebView2.Profile.PreferredColorScheme` to dark/light. |
| [TabProcessManager.vb](TabProcessManager.vb) | Class | **Multi-Process Architecture Manager**. Configures `CoreWebView2Environment` with Chromium site isolation switches (`--enable-features=IsolateOrigins,site-per-process`), integrates `BrowserInitializationService`, provides custom isolated partition environments, and maps WebView2 process roles. |
| [AdBlockerSettings.vb](AdBlockerSettings.vb) | Form | **AdBlocker Configuration**. UI for toggling filter list subscriptions (EasyList, EasyPrivacy, Fanboy's Annoyance), triggering list updates, and managing options. |
| [Settings.vb](Settings.vb) | Form | **Configuration Panel**. Allows users to manage blocked/allowed site lists, toggle permissions (Geolocation, Notifications), configure backup/restore operations, performance preferences, privacy switches, and TCP port scanner. |
| [AppManager.vb](AppManager.vb) | Class | **Utilities**. Contains regex validators for URLs, IP addresses, and emails, along with URL scheme normalization (`FixURL`). |
| [History.vb](History.vb) | Form | **History Manager**. Lists visited sites with formatted `"domain — Page Title"` text, parses ISO 8601 timestamps (`"Title|URL|DateTime"`), and provides Date Range filtering (*All Time, Today, Yesterday, Last 7 Days, Last 30 Days*). |
| [Bookmarks.vb](Bookmarks.vb) | Form | **Bookmark Tree Manager**. Displays hierarchical bookmarks in a TreeView formatted as `"domain — Page Title"` while serializing clean titles (`BookmarkNodeData.Title`). Supports Drag-and-Drop tree reordering. |
| [Phising.vb](Phising.vb) | Form | **Phishing Alert**. Decoupled warning dialog shown if the navigation guard intercepts a phishing URL. |
| [CookieViewer.vb](CookieViewer.vb) | Form | **Cookie Manager**. Utility integrating `CookieService` and `CookieExportService` to search, view, delete, and export cookies to CSV. |
| [ftp.vb](ftp.vb) | Form | **FTP Client**. Client to list, upload, download, and delete files on remote FTP servers. |
| [task manager.vb](task%20manager.vb) | Form | **Task Manager**. Lists active machine processes with memory usage and process termination capabilities, with distinct visual tags for WebView2 sub-processes (`msedgewebview2.exe`). |
| [Rss.vb](Rss.vb) | Form | **RSS Feed Reader**. Parses XML RSS feeds and presents headlines/links in an interactive tree structure. |
| [AboutBox.vb](AboutBox.vb) | Form | **About Box**. Application metadata and version info dialog. |
| [PrivacySettingsModel.vb](PrivacySettingsModel.vb) | Model / DTO | **Privacy & Application Settings Model**. DTO representing JSON configuration structure persisted in `Settings.json`. |
| [PerformanceSettingsModel.vb](PerformanceSettingsModel.vb) | Model / DTO | **Performance Settings Model**. DTO holding hardware acceleration, memory saver, and preloading flags. |
| [SessionModel.vb](SessionModel.vb) | Model / DTO | **Session State Model**. Root serialization model for open tab sessions in `Session.json`. |
| [TabItemModel.vb](TabItemModel.vb) | Model / DTO | **Tab State Unit Model**. Serializable representation of individual tab URL and title. |
| [CookieItem.vb](CookieItem.vb) | Model / DTO | **Cookie Data Model**. Strongly-typed representation of WebView2 cookie attributes with search matching capability. |
| [ListItem.vb](ListItem.vb) | Model / DTO | **Key-Value Pair Container**. Display model for UI dropdowns and list bindings. |
