# Fluent Icons Across K-Browser — Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Replace every old bitmap and emoji "icon" in K-Browser with Microsoft Fluent icons that follow the theme and are crisp at the launch DPI, and drop six outdated site links.

**Architecture:** A shared `IconService` loads the bundled Fluent "Resizable" font from an embedded resource, renders glyphs to cached, DPI-sized bitmaps, and remembers which UI element shows which icon so `ThemeManager.ApplyTheme` can re-colour them. A custom ToolStrip renderer stops WinForms from greying disabled icons. A `FaviconService` fetches and disk-caches real site favicons for the search/social menus. Forms assign icons in code; the old bitmaps are deleted from `.resx` and `Designer.vb` together.

**Tech Stack:** VB.NET, WinForms, .NET 8 (`net8.0-windows`, built with the installed .NET 9 SDK), GDI+ (`System.Drawing`), xUnit for tests.

**Spec:** `docs/superpowers/specs/2026-10-17-fluent-icons-design.md`

## Global Constraints

- Icon font: `FluentSystemIcons-Resizable.ttf` from `https://raw.githubusercontent.com/microsoft/fluentui-system-icons/a347af95478f989038cccbf085978865264c048a/fonts/FluentSystemIcons-Resizable.ttf`, MIT licence, embedded as a resource (not installed, not copied to output).
- Icon sizes (logical px, scaled by system DPI ÷ 96): toolbar 20, menus/inline 16, banner/picture-box 32.
- Default icon colour = the hosting element's `ForeColor`. Disabled colour = per-channel average of `ForeColor` and `BackColor`, alpha 255.
- Explicit colours only: `AdBlockPopup.lblShield` (220,50,50) disabled / (30,150,60) active; Settings warning `Color.DarkGoldenrod`, applied `Color.ForestGreen`.
- No PerMonitorV2 / `HighDpiMode` change. No layout redesign. No new hover effect. App icon (`favicon.ico`, `$this.Icon`) and the About box logo stay as they are.
- Favicons: fetched directly from `https://<domain>/favicon.ico` — never via a third-party favicon service; cached under `%LocalAppData%\K-Browser\FaviconCache`; never block the UI thread.
- Removed menu items (user decision): Formspring, Keek, WebCrawler, Lycos, Dogpile, Badoo.
- Whenever a bitmap leaves a `.resx`, its `resources.GetObject("…")` line leaves the matching `Designer.vb` in the same commit.
- Work on branch `feature/fluent-icons` (cut from `develop`). Commit messages end with `Co-Authored-By: Claude Opus 5.5 <noreply@anthropic.com>`.

## Review Focus

1. **Brand menus opened offline or on a slow network** — the menu opens instantly with globe icons, nothing freezes, and reopening later retries. Pinned in Task 5 (`GetFaviconAsync_NetworkFailure_ReturnsNothingAndRetriesNextCall`, `..._Timeout_ReturnsNothing`).
2. **Back/Forward disabled in dark mode** — icon is a muted version of the theme colour, not WinForms' muddy grey, and returns to full colour when re-enabled. Pinned in Task 2 (`Disabled_UsesBlendedColor`, `Reenabled_RestoresForeColor`).
3. **Dark mode toggled, then a dialog opened (or the main window re-themed)** — every icon is drawn in the new theme colour. Pinned in Task 2 (`ApplyTheme_RecolorsBoundIcons`).
4. **A site returns HTML, an empty body, or a broken image for favicon.ico** — globe stays, no exception, nothing written to the cache. Pinned in Task 5 (`GetFaviconAsync_NonImageResponse_ReturnsNothingAndWritesNoCacheFile`).
5. **A form opened after its bitmaps were removed** — no `MissingManifestResourceException`. Pinned in Task 1 (`EveryDesignerGetObjectHasResxEntry`), which every later task re-runs.

---

### Task 1: Test project, font, and `IconService` rendering

**Files:**
- Create: `tests/K-Browser.Tests/K-Browser.Tests.vbproj`, `tests/K-Browser.Tests/TestSupport.vb`, `tests/K-Browser.Tests/ResourceConsistencyTests.vb`, `tests/K-Browser.Tests/IconServiceRenderTests.vb`
- Create: `Assets/Fonts/FluentSystemIcons-Resizable.ttf`, `ThirdPartyNotices.txt`, `FluentIcon.vb`, `IconService.vb`
- Modify: `k-browser.vbproj`, `k-browser.sln`, `ApplicationEvents.vb`

**Interfaces:**
- Produces:
  - `Public Enum FluentIcon As Integer` (values = Resizable-font code points, below)
  - `IconService.Initialize()` — loads the font once; sets `ToolStripManager.Renderer` (renderer arrives in Task 2; in this task `Initialize` only loads the font)
  - `IconService.SystemDpi As Integer` (ReadOnly, captured once from the screen)
  - `IconService.GetIcon(icon As FluentIcon, logicalSize As Integer, color As Color, Optional filled As Boolean = False) As Bitmap` — at `SystemDpi`
  - `IconService.GetIcon(icon As FluentIcon, logicalSize As Integer, color As Color, dpi As Integer, filled As Boolean) As Bitmap`
  - Returned bitmaps are cached and shared — callers must never dispose them.
  - Tests: `TestSupport.RepoRoot As String`, `TestSupport.RunSta(action As Action)`

- [ ] **Step 1: Branch and commit the spec**

```bash
git checkout develop && git checkout -b feature/fluent-icons
git add docs/superpowers/specs/2026-10-17-fluent-icons-design.md docs/superpowers/plans/2026-10-17-fluent-icons.md
git commit -m "docs: add Fluent icons design and plan"
```

- [ ] **Step 2: Create the test project and wire it into the build**

`K-Browser.Tests.vbproj`: SDK `Microsoft.NET.Sdk`, `net8.0-windows`, `UseWindowsForms` true, `IsPackable` false, `RootNamespace` `KBrowser.Tests`; packages `Microsoft.NET.Test.Sdk` 17.11.1, `xunit` 2.9.2, `xunit.runner.visualstudio` 2.8.2; `ProjectReference` to `..\..\k-browser.vbproj`.
In `k-browser.vbproj` add `<Compile Remove="tests\**" />`, `<EmbeddedResource Remove="tests\**" />`, `<None Remove="tests\**" />` — **without these the SDK globbing compiles the test files into the browser.** Run `dotnet sln k-browser.sln add tests/K-Browser.Tests/K-Browser.Tests.vbproj`.
`TestSupport.RepoRoot` walks up from `AppContext.BaseDirectory` until a directory containing `k-browser.sln`. `TestSupport.RunSta` runs the action on a new STA thread, joins, and rethrows any exception — use it for every test that creates WinForms objects.

- [ ] **Step 3: Write the resource guard test (expected to pass now — it protects every later task)**

```vb
<Fact> Public Sub EveryDesignerGetObjectHasResxEntry()
    ' For each *.Designer.vb / *.designer.vb in RepoRoot (top level, excluding Form4*),
    ' collect keys from  resources\.GetObject\("([^"]+)"\)  and assert each key exists as
    ' <data name="KEY"> in the sibling .resx (same base name, case-insensitive).
    Assert.Empty(missing)   ' missing = list of "File.Designer.vb: KEY"
End Sub
```

- [ ] **Step 4: Run it**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj --filter EveryDesignerGetObjectHasResxEntry`
Expected: PASS (if it fails, the repo already has a broken reference — report it before going on).

- [ ] **Step 5: Write the failing render tests** in `IconServiceRenderTests.vb`

```vb
<Fact> Sub GetIcon_SizeScalesWithDpi()
    Assert.Equal(16, IconService.GetIcon(FluentIcon.Home, 16, Color.Black, 96, False).Width)
    Assert.Equal(24, IconService.GetIcon(FluentIcon.Home, 16, Color.Black, 144, False).Width)
    Assert.Equal(25, IconService.GetIcon(FluentIcon.Home, 20, Color.Black, 120, False).Height)
End Sub
<Fact> Sub GetIcon_DrawsInRequestedColor()
    ' color (161,197,74); every pixel with A > 200 has R,G,B each within ±12; at least one such pixel
End Sub
<Fact> Sub GetIcon_GlyphCentredAndFillsBox()
    ' Home, 20px, dpi 96: bounding box of pixels with A > 0 is ≥ 10px wide and tall;
    ' its centre is within 1.5px of (10,10)
End Sub
<Fact> Sub GetIcon_AllEnumValuesRenderDistinctGlyphs()
    ' render every [Enum].GetValues(GetType(FluentIcon)) at 20px black, dpi 96;
    ' hash the pixel bytes; assert hash count = value count (a wrong code point draws the same
    ' "missing glyph" box as another wrong one)
End Sub
<Fact> Sub GetIcon_IsCached()
    ' same args → Assert.Same; different colour → Assert.NotSame
End Sub
<Fact> Sub GetIcon_FilledStarDiffersFromRegular()   ' pixel bytes differ
<Fact> Sub GetIcon_FilledWithoutVariant_Throws()    ' Assert.Throws(Of ArgumentException) for Home filled
```

- [ ] **Step 6: Run to see them fail**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj`
Expected: build FAIL — `IconService` / `FluentIcon` not defined.

- [ ] **Step 7: Add the font, licence, and enum**

Download the TTF (URL in Global Constraints) to `Assets/Fonts/`. In `k-browser.vbproj`: `<EmbeddedResource Include="Assets\Fonts\FluentSystemIcons-Resizable.ttf" LogicalName="KBrowser.Fonts.FluentSystemIcons-Resizable.ttf" />`. Put the MIT licence text from the Fluent repo's `LICENSE` file (same commit) in `ThirdPartyNotices.txt`, headed "Fluent UI System Icons — https://github.com/microsoft/fluentui-system-icons".

`FluentIcon.vb` — regular `_20_` glyph code points from the font's `FluentSystemIcons-Resizable.json` at that commit:

```vb
Public Enum FluentIcon As Integer
    ArrowClockwise = 57535 : ArrowDownload = 57565 : ArrowEnter = 57571 : ArrowLeft = 57621
    ArrowRight = 57665 : ArrowSync = 57713 : ArrowUpload = 57777 : Bookmark = 57970
    Box = 58022 : Broom = 58090 : Checkmark = 58464 : Cookies = 58837 : Copy = 58839
    Cut = 58903 : DataUsage = 58941 : Delete = 58989 : Dismiss = 59091 : DismissCircle = 59093
    Document = 59113 : Eye = 59523 : EyeOff = 59529 : Globe = 59803 : GlobeDesktop = 59813
    History = 59950 : Home = 59954 : Incognito = 60038 : Info = 60040 : Key = 60071
    LockClosed = 60332 : Open = 60660 : Password = 60755 : People = 60789 : Person = 60851
    Print = 61147 : Rss = 61329 : Save = 61337 : Screenshot = 61393 : Search = 61399
    Send = 61431 : Server = 61445 : Settings = 61455 : Shield = 61497 : SignOut = 61581
    Star = 61789 : TabAdd = 61895 : TextWrap = 62523 : Video = 62715 : Warning = 62817
    WeatherMoon = 62845 : WindowNew = 62955 : ZoomIn = 62991
End Enum
```
(Write one member per line; the colons are only to keep this plan short.) Filled variants live in a private dictionary inside `IconService`; the only entry needed is `Star → 61788`. Add an entry only when a task uses that filled icon.

- [ ] **Step 8: Implement `IconService` rendering** in `IconService.vb` (`Public NotInheritable Class`, Shared members like `ThemeManager`)

- Font: read the embedded stream into `Marshal.AllocCoTaskMem` memory, `PrivateFontCollection.AddMemoryFont`, and **never free that memory** (GDI+ reads it for the font's lifetime). Load once, thread-safe (`Lazy(Of FontFamily)`).
- Draw with GDI+ `Graphics.DrawString` (not `TextRenderer` — GDI can't see a `PrivateFontCollection` font) on a transparent `Format32bppPArgb` bitmap of `Round(logicalSize * dpi / 96)` px, font size = that pixel size in `GraphicsUnit.Pixel`, `TextRenderingHint.AntiAlias` (ClearType fringes on a transparent background), `StringFormat.GenericTypographic` centred both ways, glyph string `ChrW(codePoint)`.
- Cache key `(icon, pixelSize, color.ToArgb(), filled)` in a `ConcurrentDictionary`.
- `SystemDpi`: `Using g = Graphics.FromHwnd(IntPtr.Zero)` → `CInt(g.DpiX)`, captured once.
- Call `IconService.Initialize()` first thing in `MyApplication_Startup` in `ApplicationEvents.vb` (add the handler if it isn't there).

- [ ] **Step 9: Run tests**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj`
Expected: all PASS. Also `dotnet build k-browser.sln` succeeds with no new warnings.

- [ ] **Step 10: Commit**

```bash
git add tests/ Assets/ ThirdPartyNotices.txt FluentIcon.vb IconService.vb k-browser.vbproj k-browser.sln ApplicationEvents.vb
git commit -m "feat(icons): add Fluent icon font and IconService rendering with tests"
```

---

### Task 2: Icon bindings, theme re-colouring, disabled state

**Files:**
- Modify: `IconService.vb`, `ThemeManager.vb` (`ApplyTheme`, before `form.Refresh()`)
- Create: `IconToolStripRenderer.vb`, `tests/K-Browser.Tests/IconBindingTests.vb`

**Interfaces:**
- Consumes: `IconService.GetIcon`, `FluentIcon`, `TestSupport.RunSta`
- Produces:
  - `IconService.SetIcon(item As ToolStripItem, icon As FluentIcon, Optional logicalSize As Integer = 16, Optional color As Color? = Nothing, Optional filled As Boolean = False)` — sets `Image`, sets `ImageScaling = None`
  - `IconService.SetIcon(target As Control, icon As FluentIcon, Optional logicalSize As Integer = 16, Optional color As Color? = Nothing, Optional filled As Boolean = False)` — supports `Button` (and when its `Text` isn't empty, sets `TextImageRelation = ImageBeforeText`), `Label`, `PictureBox`; any other control → `ArgumentException`
  - `IconService.ClearIcon(target As Object)` — forgets the binding, leaves `Image` alone
  - `IconService.IsBound(target As Object) As Boolean`
  - `IconService.RefreshIcons(root As Control)` — re-renders every bound element under `root`: child controls recursively, `ToolStrip.Items` and nested `DropDownItems` recursively, and any control's `ContextMenuStrip`
  - `IconToolStripRenderer` (inherits `ToolStripProfessionalRenderer`); `IconService.Initialize()` now also sets `ToolStripManager.Renderer = New IconToolStripRenderer()`

- [ ] **Step 1: Write the failing tests** (all inside `TestSupport.RunSta`)

```vb
<Fact> Sub SetIcon_ToolStripButton_UsesForeColor()
    ' btn.ForeColor = Color.Red → opaque pixels ≈ red (±12); btn.ImageScaling = None
<Fact> Sub RefreshIcons_RecolorsAfterForeColorChange()
    ' Form > MenuStrip > ToolStripMenuItem(parent) > DropDownItems(child bound);
    ' set child.ForeColor = Blue; RefreshIcons(form) → child pixels ≈ blue
<Fact> Sub RefreshIcons_KeepsExplicitColor()
    ' SetIcon(lbl, Shield, 32, Color.FromArgb(30,150,60)); lbl.ForeColor = White; Refresh → still green
<Fact> Sub Disabled_UsesBlendedColor()
    ' ForeColor White, BackColor Black, bound, Enabled = False → fully opaque pixels ≈ (128,128,128) ±3
<Fact> Sub Reenabled_RestoresForeColor()
<Fact> Sub SetIcon_SupportsButtonLabelPictureBox_AndRejectsTextBox()
    ' Button with Text → TextImageRelation = ImageBeforeText; TextBox → Throws ArgumentException
<Fact> Sub ClearIcon_StopsRefresh()
    ' bind, ClearIcon, set Image = custom bitmap, RefreshIcons → Assert.Same(custom, item.Image)
<Fact> Sub ApplyTheme_RecolorsBoundIcons()
    ' Form with ToolStrip + bound button; ThemeManager.IsDarkMode = True; ApplyTheme(form)
    ' → pixels ≈ ThemeManager.DarkText; set False, ApplyTheme → ≈ LightText. Restore IsDarkMode after.
```

- [ ] **Step 2: Run to see them fail**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj --filter FullyQualifiedName~IconBindingTests`
Expected: build FAIL — `SetIcon` not defined.

- [ ] **Step 3: Implement bindings**

Store `IconBinding(Icon, LogicalSize, ExplicitColor As Color?, Filled)` in a `ConditionalWeakTable(Of Object, IconBinding)` so disposed forms don't leak. Colour = explicit, otherwise host `ForeColor`; when the host is disabled use the blend from Global Constraints. On the first bind of a host, subscribe to its `EnabledChanged` and re-render. `ThemeManager.ApplyTheme` calls `IconService.RefreshIcons(form)` just before `form.Refresh()`.

- [ ] **Step 4: Implement `IconToolStripRenderer`**

Override `OnRenderItemImage`: when `Not e.Item.Enabled AndAlso IconService.IsBound(e.Item) AndAlso e.Image IsNot Nothing`, draw `e.Image` into `e.ImageRectangle` as is (it's already the muted bitmap); otherwise `MyBase.OnRenderItemImage(e)`. Strips with `RenderMode = System` (CookieViewer) or `Professional` (`ContextMenuStrip1`) keep their own renderer; that's fine.

- [ ] **Step 5: Run tests**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj`
Expected: all PASS.

- [ ] **Step 6: Commit**

```bash
git add IconService.vb IconToolStripRenderer.vb ThemeManager.vb tests/
git commit -m "feat(icons): bind icons to UI elements, recolour on theme and disabled state"
```

---

### Task 3: Main window toolbar

**Files:**
- Modify: `Form1.vb` (new `Private Sub ApplyIcons()` called in the load path before `ThemeManager.ApplyTheme(Me)` at ~line 124; ad-block badge text at ~lines 182, 206, 904, 922), `Form1.Designer.vb`, `Form1.resx`
- Test: `tests/K-Browser.Tests/FormResourceTests.vb`

**Interfaces:**
- Consumes: `IconService.SetIcon(ToolStripItem, …)`, `FluentIcon`

| Control | Icon (20 px) | Other changes |
|---|---|---|
| `Back` / `ToolStripButton2` | `ArrowLeft` / `ArrowRight` | |
| `ToolStripButton3` / `ToolStripButton4` | `ArrowClockwise` / `Home` | |
| `ToolStripButton7` / `ToolStripButton6` | `TabAdd` / `Dismiss` | |
| `ToolStripButton5` (Go) / `ToolStripButton8` (RSS) | `ArrowEnter` / `Rss` | |
| `ToolStripButton9` (Add Bookmark) | `Star` | |
| `tsbAdBlockBadge` | `Shield` | `DisplayStyle = ImageAndText`, `AutoSize = True`, text = count only (`AdBlockEngine.TotalBlockedCount.ToString()` at all four sites) |
| `btnPrivateMode` | `Incognito` | `DisplayStyle = ImageAndText`, `AutoSize = True`, text `"Private"` |

- [ ] **Step 1: Write the failing test**

```vb
<Fact> Sub Form1Toolbar_HasNoBitmaps()
    ' Form1.resx has no <data name> among: Back.Image, ToolStripButton2..9.Image (the 9 toolbar keys)
<Fact> Sub Form1Toolbar_HasNoEmojiText()
    ' in Form1.Designer.vb and Form1.vb, no line containing "tsbAdBlockBadge.Text" or
    ' "btnPrivateMode.Text" contains "🛡" or "🕶" (the menu item's 🕶 is Task 4's)
```

- [ ] **Step 2: Run to see it fail**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj --filter FullyQualifiedName~FormResourceTests`
Expected: FAIL listing the 9 keys.

- [ ] **Step 3: Implement** — `ApplyIcons()` per the table; delete the 9 `.Image` lines in `Form1.Designer.vb` and their `<data>` entries in `Form1.resx`; designer `Text` of the two emoji buttons as in the table.

- [ ] **Step 4: Run tests and build**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj` then `dotnet build k-browser.sln`
Expected: all PASS (including `EveryDesignerGetObjectHasResxEntry`).

- [ ] **Step 5: Look at it**

Run: `dotnet run --project k-browser.vbproj`. Check in light mode, then toggle View → Dark Mode: all 11 toolbar icons are centred in their 36×36 buttons, in the text colour; Back/Forward look muted (not grey-boxed) when there's no history; the badge shows shield + count; Private shows icon + "Private".

- [ ] **Step 6: Commit**

```bash
git add Form1.vb Form1.Designer.vb Form1.resx tests/
git commit -m "feat(icons): Fluent icons on the main toolbar"
```

---

### Task 4: Main window menus and removing outdated links

**Files:**
- Modify: `Form1.vb` (`ApplyIcons`; delete click handlers of removed items where present — Dogpile, WebCrawler, Lycos at ~lines 663–680), `Form1.Designer.vb`, `Form1.resx`
- Test: `tests/K-Browser.Tests/FormResourceTests.vb`

**Interfaces:**
- Consumes: `IconService.SetIcon(ToolStripItem, …)` at 16 px

| Menu item | Icon | | Menu item | Icon |
|---|---|---|---|---|
| `ToolStripMenuItem5`, `OpePageInNToolStripMenuItem` | `Open` | | `HistoryToolStripMenuItem` | `History` |
| `NewTabToolStripMenuItem1` | `TabAdd` | | `BackToolStripMenuItem` / `ForwardToolStripMenuItem` | `ArrowLeft` / `ArrowRight` |
| `CloseTabToolStripMenuItem` | `Dismiss` | | `ReloadToolStripMenuItem1` / `StopToolStripMenuItem1` | `ArrowClockwise` / `Dismiss` |
| `NewWindowToolStripMenuItem` | `WindowNew` | | `ToolStripMenuItem3` (Home Page) | `Home` |
| `PrivateWindowToolStripMenuItem` (text → "New Private Window") | `Incognito` | | `CookieToolStripMenuItem` | `Cookies` |
| `DefualtToolStripMenuItem` | `Document` | | `CookieViewerToolStripMenuItem` (Clear All Recent Data) | `Broom` |
| `SaveFileToolStripMenuItem` / `PrintToolStripMenuItem` | `Save` / `Print` | | `ViewToolStripMenuItem1` (bookmarks) | `Bookmark` |
| `ExitToolStripMenuItem` | `SignOut` | | `DarkModeToolStripMenuItem` (text → "Dark Mode") | `WeatherMoon` |
| `CutToolStripMenuItem` / `CopyToolStripMenuItem` | `Cut` / `Copy` | | `LinksToolStripMenuItem` / `SocialNetworksToolStripMenuItem` | `Search` / `People` |
| `ZooomToolStripMenuItem` | `ZoomIn` | | `SettingsToolStripMenuItem` / `DownloadManagerToolStripMenuItem` | `Settings` / `ArrowDownload` |
| `ImageToolStripMenuItem` (Take a Screenshot) | `Screenshot` | | `FtpClientToolStripMenuItem` / `TaskToolStripMenuItem` | `Server` / `DataUsage` |
| `SetHomePageToolStripMenuItem` (Rss Reader) | `Rss` | | `MediaToolStripMenuItem` / `SendALinkToolStripMenuItem` | `Video` / `Send` |
| `InternetSettingsToolStripMenuItem` | `GlobeDesktop` | | `ProxySettingsToolStripMenuItem` | `Globe` |
| `AboutToolStripMenuItem` | `Info` | | `CheckForUpdatesToolStripMenuItem` | `ArrowSync` |

Only items that have a bitmap or emoji today get an icon; don't add icons to other menu items. Brand items (Bing … Pinterest) are Task 5.

- [ ] **Step 1: Write the failing tests**

```vb
<Fact> Sub Form1_HasNoRemovedServices()
    ' Form1.vb, Form1.Designer.vb, Form1.resx contain none of:
    ' "Formspring", "Keek", "WebCrawler", "Lycos", "Dogpile", "Badoo" (case-insensitive)
<Fact> Sub Form1_OnlyBrandBitmapsRemain()
    ' every <data name="*.Image"> left in Form1.resx is one of the 14 brand keys owned by Task 5:
    ' Bing, GoogleToolStripMenuItem1, Yahoo, EBay, MSN, DuckDuckGo, Vimeo, Facebook, Twitter,
    ' GoogleToolStripMenuItem, Linkedin, Instagram, Vk, Pinterest (each "...ToolStripMenuItem.Image")
```

- [ ] **Step 2: Run to see them fail**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj --filter FullyQualifiedName~FormResourceTests`
Expected: FAIL.

- [ ] **Step 3: Implement** — icons per the table in `ApplyIcons()`; remove the six items (declaration, `New`, setup block, `AddRange` entries at ~lines 726/810, `.resx` entry, click handler); remove the replaced bitmaps from designer + resx.

- [ ] **Step 4: Run tests and build** — same commands as Task 3 Step 4. Expected: PASS.

- [ ] **Step 5: Look at it** — open every top-level menu in both themes: icons are 16 px, aligned, theme-coloured; Back/Forward/Stop menu items look muted when disabled; the six links are gone.

- [ ] **Step 6: Commit**

```bash
git add Form1.vb Form1.Designer.vb Form1.resx tests/
git commit -m "feat(icons): Fluent icons on main menus; remove six outdated site links"
```

---

### Task 5: Favicons for search-engine and social menus

**Files:**
- Create: `FaviconService.vb`, `tests/K-Browser.Tests/FaviconServiceTests.vb`
- Modify: `Form1.vb`, `Form1.Designer.vb`, `Form1.resx`

**Interfaces:**
- Consumes: `IconService.SetIcon`, `IconService.ClearIcon`, `IconService.SystemDpi`
- Produces:
  - `Public Class FaviconService` — `Sub New(http As HttpClient, cacheDirectory As String)`
  - `Shared ReadOnly Property [Default] As FaviconService` — cache dir `%LocalAppData%\K-Browser\FaviconCache`, `HttpClient` with 5 s timeout, user agent `K-Browser`
  - `Function GetFaviconAsync(domain As String) As Task(Of Bitmap)` — returns `Nothing` on any failure, never throws
  - `IconService.ScaleImage(img As Image, logicalSize As Integer) As Bitmap` (add to `IconService`) — high-quality resize to the DPI-scaled square

- [ ] **Step 1: Write the failing tests** (fake `HttpMessageHandler` counting requests; temp cache dir per test)

```vb
<Fact> Async Function GetFaviconAsync_Png_ReturnsImageAndCachesToDisk() As Task
    ' handler returns a 32×32 PNG for https://example.com/favicon.ico
    ' → result.Width = 32; File.Exists(cacheDir\example.com.png); a second call (fresh instance, same dir) makes 0 requests
<Fact> Async Function GetFaviconAsync_Ico_ReturnsImage() As Task     ' body is a real .ico (build with Icon.Save)
<Fact> Async Function GetFaviconAsync_SameInstance_FetchesOnce() As Task   ' two calls → 1 request
<Fact> Async Function GetFaviconAsync_NonImageResponse_ReturnsNothingAndWritesNoCacheFile() As Task
    ' cases: 200 with "<html>…", 200 empty body, 404 → Nothing, no file
<Fact> Async Function GetFaviconAsync_NetworkFailure_ReturnsNothingAndRetriesNextCall() As Task
    ' handler throws HttpRequestException → Nothing; second call → handler hit again (failures not cached)
<Fact> Async Function GetFaviconAsync_Timeout_ReturnsNothing() As Task      ' handler throws TaskCanceledException
<Fact> Async Function GetFaviconAsync_RejectsNonHostname() As Task
    ' "..\evil", "a/b", "", Nothing, "exa mple.com" → Nothing and 0 requests
```

- [ ] **Step 2: Run to see them fail**

Run: `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj --filter FullyQualifiedName~FaviconServiceTests`
Expected: build FAIL — `FaviconService` not defined.

- [ ] **Step 3: Implement `FaviconService`**

Validate the domain against `^(?=.{1,253}$)([a-z0-9-]{1,63}\.)+[a-z]{2,63}$` (case-insensitive) before touching disk or network. Order: in-memory `ConcurrentDictionary(Of String, Task(Of Bitmap))` (drop the entry when the result is `Nothing`, so failures retry) → disk `<domain>.png` → GET `https://<domain>/favicon.ico` (redirects followed). Decode with `New Bitmap(stream)`; if that throws, try `New Icon(stream).ToBitmap()`; anything else → `Nothing`. Save to disk as PNG only after a successful decode.

- [ ] **Step 4: Wire the brand menus in `Form1.vb`**

Map item → domain: Bing `bing.com`, `GoogleToolStripMenuItem1` `google.com`, Yahoo `yahoo.com`, eBay `ebay.com`, MSN `msn.com`, DuckDuckGo `duckduckgo.com`, Vimeo `vimeo.com`, AccuWeather `accuweather.com`, Facebook `facebook.com`, Twitter `x.com`, `GoogleToolStripMenuItem` (Google+) `google.com`, LinkedIn `linkedin.com`, Instagram `instagram.com`, VK `vk.com`, Pinterest `pinterest.com`. Check each against the URL its click handler opens and use that host if it differs.
In `ApplyIcons()`, give each brand item `SetIcon(item, FluentIcon.Globe)`. Handle `DropDownOpening` on `LinksToolStripMenuItem` and `SocialNetworksToolStripMenuItem` with an `Async Sub` that, for each item still on the globe, awaits `FaviconService.Default.GetFaviconAsync(domain)`; on a non-`Nothing` result `IconService.ClearIcon(item)` then `item.Image = IconService.ScaleImage(result, 16)`. Remove the 14 brand bitmaps from designer + resx. Delete the unused `faviconCache` field and `MAX_FAVICON_CACHE` constant (`Form1.vb` lines 9–10) if nothing else references them.

- [ ] **Step 5: Run tests and build** — Expected: all PASS; `Form1_OnlyBrandBitmapsRemain` now holds because no `.Image` keys remain in `Form1.resx`.

- [ ] **Step 6: Look at it** — with Wi-Fi off, open both menus: globes, no delay. Wi-Fi on, reopen: favicons appear; restart the app and reopen: favicons appear immediately from `%LocalAppData%\K-Browser\FaviconCache`.

- [ ] **Step 7: Commit**

```bash
git add FaviconService.vb IconService.vb Form1.vb Form1.Designer.vb Form1.resx tests/
git commit -m "feat(icons): real site favicons for search and social menus"
```

---

### Task 6: Private browsing window

**Files:**
- Modify: `PrivateBrowserForm.vb` (load path: add `ApplyIcons()`), `PrivateBrowserForm.Designer.vb`
- Test: `tests/K-Browser.Tests/FormResourceTests.vb`

**Interfaces:**
- Consumes: `IconService.SetIcon`

| Control | Icon | Other changes |
|---|---|---|
| `btnBack` / `btnForward` | `ArrowLeft` / `ArrowRight` (20) | `DisplayStyle = Image`; `Text`/`ToolTipText` = "Back" / "Forward" |
| `btnRefresh` / `btnHome` | `ArrowClockwise` / `Home` (20) | same pattern: "Refresh" / "Home" |
| `btnGo` / `btnBookmark` | `ArrowEnter` / `Star` (20) | "Go" / "Bookmark"; keep their designer `ForeColor` (lime / sub-text) — that's the icon colour |
| `lblPrivateIcon` (Label) | `Incognito` (32) | `Text = ""`, image centred |
| `lblPrivateBadge` (ToolStripLabel) | `Incognito` (16) | `DisplayStyle = ImageAndText`, text "Private Mode" |

This form never calls `ThemeManager.ApplyTheme` — don't add that; icons render once from the designer colours. If its toolbar sets its own `Renderer`/`RenderMode` (so `IconToolStripRenderer` isn't used), set `Renderer = New IconToolStripRenderer()` on that strip so disabled Back/Forward aren't greyed.

- [ ] **Step 1: Write the failing test**

```vb
<Fact> Sub PrivateBrowserForm_HasNoGlyphText()
    ' PrivateBrowserForm.Designer.vb contains none of "◀", "▶", "🔄", "🏠", "→", "⭐", "🕶"
```

- [ ] **Step 2: Run to see it fail** — `--filter PrivateBrowserForm_HasNoGlyphText`, Expected: FAIL.
- [ ] **Step 3: Implement** per the table.
- [ ] **Step 4: Run tests and build** — Expected: PASS.
- [ ] **Step 5: Look at it** — File → New Private Window: the toolbar matches the main window's icons and size; the banner and badge show the incognito icon in lime; Back/Forward look muted when disabled.
- [ ] **Step 6: Commit**

```bash
git add PrivateBrowserForm.vb PrivateBrowserForm.Designer.vb tests/
git commit -m "feat(icons): Fluent icons in the private browsing window"
```

---

### Task 7: Themed dialogs

**Files:**
- Modify (each form's `.vb`, `Designer.vb`, `.resx` as needed): `CookieViewer`, `Rss`, `ftp`, `task manager`, `Bookmarks`, `History`, `AdBlockPopup`, `Settings`, `Source`
- Test: `tests/K-Browser.Tests/FormResourceTests.vb`

**Interfaces:**
- Consumes: `IconService.SetIcon`, `IconService.GetIcon`

Each form assigns icons in its `Load` handler (next to its existing `ThemeManager.ApplyTheme(Me)` call; order doesn't matter because `ApplyTheme` refreshes bound icons).

| Form | Control → icon (size) | Notes |
|---|---|---|
| `CookieViewer` | `btnDeleteALL` → `Delete` (16); `ImageList1` → one `Cookies` image | After `ApplyTheme`, rebuild `ImageList1` in code: `ColorDepth = Depth32Bit`, `ImageSize` = DPI-scaled 16, add `GetIcon(Cookies, 16, tvCookies.ForeColor)` under key `"cookie.png"`. Remove `ImageList1.ImageStream` from designer + resx. |
| `Rss` | `tsRssGo` → `ArrowEnter` (16) | |
| `ftp` | `ToolStripLabel1/2/3` → `Server` / `Person` / `Password` (16) | |
| `task manager` | `ToolStripButton1` (End Process) → `DismissCircle` (16) | |
| `Bookmarks`, `History` | `PictureBox1` → `Bookmark` / `History` (32) | Open the current image first; if it's K-Browser brand artwork, leave it and drop it from this row. |
| `AdBlockPopup` | `lblShield` → `Shield` (32), `Text = ""`; `btnAdBlockSettings` → `Settings` (16), text "AdBlocker Settings..."; `btnReload` → `ArrowClockwise` (16), text "Reload Page" | In `UpdateStatusUI`, replace both `lblShield.Text = "🛡️"` with `SetIcon(lblShield, Shield, 32, <status colour>)` using (220,50,50) / (30,150,60) |
| `Settings` | `btnCreateBackup` → `Box`; `btnRestoreBackup` → `ArrowUpload`; `btnResetSettings` → `Warning` (all 16) | Strip emoji from those buttons' text and from group titles `grpGraphics`, `grpTabs`, `grpPageLoading`. |
| `Settings` status | new `PictureBox` `picGraphicsStatus` → `Warning` (16, `Color.DarkGoldenrod`); `picTabsStatus`, `picPageLoadingStatus` → `Checkmark` (16, `Color.ForestGreen`) | A WinForms `Label` can't put an image beside its text, so add a 16×16 `PictureBox` (`SizeMode = CenterImage`) directly left of each status label; move the label right by 20 px and shrink it by 20 px. Strip `⚠️`/`✓` from the label text. |
| `Source` | `lblSearch` → `Search`; `btnCopy` → `Copy`; `btnSave` → `Save`; `btnToggleWrap` → `TextWrap`; `btnPrint` → `Print` (16) | Strip emoji from their text; `"✓ Copied!"` in `Source.vb` → `"Copied!"`. |

- [ ] **Step 1: Write the failing tests**

```vb
<Fact> Sub ThemedDialogs_HaveNoBitmapIcons()
    ' none of these keys remain in their resx: CookieViewer btnDeleteALL.Image, ImageList1.ImageStream;
    ' Rss tsRssGo.Image; ftp ToolStripLabel1/2/3.Image; task manager ToolStripButton1.Image
<Fact> Sub ThemedDialogs_HaveNoEmojiText()
    ' AdBlockPopup.Designer.vb, AdBlockPopup.vb, Settings.designer.vb, Source.designer.vb, Source.vb
    ' contain none of "🛡", "⚙", "↻", "📦", "🔄", "⚠", "🖥", "📑", "⚡", "✓", "🔍", "📋", "💾", "📄", "🖨"
```

- [ ] **Step 2: Run to see them fail** — Expected: FAIL.
- [ ] **Step 3: Implement** per the table.
- [ ] **Step 4: Run tests and build** — Expected: all PASS.
- [ ] **Step 5: Look at it** — open each dialog in light and dark mode: icons are theme-coloured and aligned; button icons sit left of the text; the ad-block shield turns red/green with site status; the Settings status icons are gold/green.
- [ ] **Step 6: Commit**

```bash
git add CookieViewer* Rss* ftp* "task manager"* Bookmarks* History* AdBlockPopup* Settings* Source* tests/
git commit -m "feat(icons): Fluent icons in themed dialogs"
```

---

### Task 8: Fixed-palette dialogs

**Files:**
- Modify: `PasswordDetailForm.vb/.Designer.vb`, `SavePasswordForm.vb/.Designer.vb`, `mainForm.vb/.designer.vb/.resx`
- Test: `tests/K-Browser.Tests/FormResourceTests.vb`

**Interfaces:**
- Consumes: `IconService.SetIcon`

| Form | Control → icon | Notes |
|---|---|---|
| `PasswordDetailForm` | `lblIcon` → `LockClosed` (32), `Text = ""` | colour comes from its lime `ForeColor` |
| `PasswordDetailForm` | `btnTogglePassword` → `Eye` (16) + "Show Password"; when showing → `EyeOff` + "Hide Password" | replace the `"🙈 Hide Password"` assignment (~line 82) and its "Show" counterpart |
| `SavePasswordForm` | `lblIcon` → `Key` (32), `Text = ""` | |
| `mainForm` (Download Manager) | `PictureBox1` → `ArrowDownload` (32) | same brand-artwork check as Task 7; remove `PictureBox1.Image` from designer + resx if replaced |

These forms don't call `ThemeManager.ApplyTheme`; don't add it.

- [ ] **Step 1: Write the failing test**

```vb
<Fact> Sub FixedPaletteDialogs_HaveNoEmojiText()
    ' PasswordDetailForm.Designer.vb/.vb and SavePasswordForm.Designer.vb contain none of "🔐", "👁", "🙈", "🔑"
```

- [ ] **Step 2: Run to see it fail** — Expected: FAIL.
- [ ] **Step 3: Implement** per the table.
- [ ] **Step 4: Run tests and build** — Expected: PASS.
- [ ] **Step 5: Look at it** — open a saved password's details (toggle show/hide), trigger the save-password prompt, open the Download Manager: icons are legible on each form's own background.
- [ ] **Step 6: Commit**

```bash
git add PasswordDetailForm* SavePasswordForm* mainForm* tests/
git commit -m "feat(icons): Fluent icons in password and download dialogs"
```

---

### Task 9: App-wide sweep and final verification

**Files:**
- Modify: `Form1.vb` (`"⚠️ Tab Crashed"` → `"Tab Crashed"`, ~line 812), plus anything the new tests flag
- Test: `tests/K-Browser.Tests/AppWideIconTests.vb`

- [ ] **Step 1: Write the app-wide guard tests**

```vb
<Fact> Sub NoEmojiInUiStrings()
    ' every top-level *.vb in RepoRoot (excluding Form4*): skip lines whose trimmed text starts with "'";
    ' in each string literal "...", no char in U+2190–21FF, U+2300–23FF, U+25A0–25FF, U+2600–27BF,
    ' U+2B00–2BFF, U+FE0F, and no surrogate pair. Report "File:line: literal".
<Fact> Sub NoBitmapIconsRemainInResx()
    ' across all top-level *.resx: no <data name> ending ".Image", ".BackgroundImage" or ".ImageStream",
    ' except AboutBox.resx PictureBox1.Image and any PictureBox the implementer kept as brand artwork
    ' in Tasks 7–8 (list them explicitly in the allowlist with a comment saying why).
```

- [ ] **Step 2: Run** — `dotnet test tests/K-Browser.Tests/K-Browser.Tests.vbproj`. Fix each reported string (strip the emoji, keep the words) or bitmap until both pass. Expected after fixes: all PASS.

- [ ] **Step 3: Full manual pass** — `dotnet run --project k-browser.vbproj` at 100% display scaling, then sign out/in (or restart) at 150%:
  - main toolbar, every main menu, the private window, and every dialog from Tasks 7–8, in light mode and after toggling dark mode at runtime;
  - icons sharp, centred, not clipped; disabled items muted; no emoji-as-icon left anywhere;
  - search/social menus offline (globes) and online (favicons, cached on next launch).

- [ ] **Step 4: Commit**

```bash
git status   # stage only the files you changed for this task, by name
git commit -m "feat(icons): app-wide guards against leftover emoji and bitmap icons"
```
