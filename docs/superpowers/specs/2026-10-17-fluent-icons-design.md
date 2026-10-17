# Fluent Icons Across K-Browser — Design

Date: 2026-10-17
Status: Draft for review

## Intent

K-Browser's buttons and menus use old fixed bitmaps (embedded in `.resx` files) and, in newer
windows, emoji characters as stand-in icons. They look dated, don't follow the dark/light theme,
and blur on high-DPI screens. Replace them app-wide with Microsoft's **Fluent** icon style so
K-Browser looks like a native Windows 11 browser (the same visual language as Edge and Windows 11).

## Goals

- Every icon on a button, toolbar item, menu item, or icon-style label in the app uses a Fluent icon.
- Icons are drawn in the current theme's colours and re-coloured when the theme is applied.
- Icons are crisp at the display scaling in effect when the app starts (100%, 125%, 150%, 200%).
- Identical look on Windows 10 and Windows 11 (the installer allows `MinVersion=10.0`).
- One shared helper produces all icons; forms don't each reinvent rendering.

## Non-goals

- Redesigning toolbar or dialog layouts, button order, or sizes (beyond what an icon swap needs).
- Changing the app/window icon (`favicon.ico`, `$this.Icon` in each form).
- Fixing the broader issue that toggling dark mode only re-themes the main window. Icons follow
  whatever theme `ThemeManager` applies to their form — no more, no less.
- Replacing the K-Browser logo/brand artwork (e.g., the About box logo).

## Anti-goals (fails even if it "works")

- A mix of old bitmaps/emoji and new Fluent icons left behind in shipped UI.
- Icons that are dark-on-dark or light-on-light after toggling the theme.
- Blurry/upscaled icons at 150%+ scaling.
- Visible layout shifts or clipped icons on the existing 36×36 toolbar buttons.
- A runtime crash because a `Designer.vb` still calls `resources.GetObject("X.Image")` for a
  bitmap that was removed from its `.resx`.

## Decisions made

1. **Icon set: Microsoft Fluent UI System Icons** (github.com/microsoft/fluentui-system-icons,
   MIT licence) — chosen by the user ("native Windows 11 (Fluent)").
2. **Ship the font with the app rather than rely on the OS** *(Claude's call)*. The built-in
   Segoe Fluent Icons font exists only on Windows 11; Windows 10 has the older Segoe MDL2 Assets.
   Bundling the font gives the same result on both. Use **`FluentSystemIcons-Resizable.ttf`**
   (~1.6 MB) from commit `a347af95478f989038cccbf085978865264c048a` of the Fluent repo: one file
   holding both regular and filled glyphs, designed for drawing at any size, with every code
   point in the Basic Multilingual Plane (the size-specific Regular/Filled fonts put some glyphs
   above U+FFFF, which GDI+ handles poorly) *(Claude's call, made while planning)*.
3. **Scope: the whole app** — chosen by the user. Main browser window (`Form1`), the private
   browsing window (`PrivateBrowserForm`), and all smaller dialogs.
4. **Search-engine and social-network menu items show each site's real favicon**, not a library
   logo *(Claude's call, raised with the user without objection)*. Fluent has no brand logos.
5. **Emoji used as icons are replaced too**, not only bitmaps — they're the same problem.
6. **Drop six outdated site links** from the search/social menus — decided by the user:
   Formspring, Keek, WebCrawler, Lycos, Dogpile, Badoo.

## Inventory (what gets replaced)

### Main window — `Form1`
- **Toolbar (9 bitmap buttons, 36×36, `ImageScaling=None`):** `Back`, `ToolStripButton2`
  (Forward), `ToolStripButton3` (Refresh), `ToolStripButton4` (Home), `ToolStripButton7`
  (New tab), `ToolStripButton6` (Close tab), `ToolStripButton5` (Go), `ToolStripButton8` (RSS),
  `ToolStripButton9` (Add Bookmark).
- **Toolbar emoji buttons:** `tsbAdBlockBadge` (`"🛡️ 0"`), `btnPrivateMode` (`"🕶 Private"`).
  Keep their text (counter / "Private"), add an icon beside it (`DisplayStyle=ImageAndText`).
- **Menu items with bitmaps (~45 generic):** New tab, Close tab, New window, Save file, Print,
  Exit, Cut, Copy, Zoom, Image, History, Back, Forward, Reload, Stop, Cookies, View, Settings,
  Download Manager, FTP Client, Task Manager, Set Home Page, Media, Cookie Viewer, Send a Link,
  Internet Settings, Proxy Settings, Open Page In…, About, Check for Updates, plus the unnamed
  `ToolStripMenuItem3/5` and `DefualtToolStripMenuItem`. Full list: every `*.Image` entry in
  `Form1.resx`.
- **Menu items with emoji:** `DarkModeToolStripMenuItem` (`🌙`), `PrivateWindowToolStripMenuItem` (`🕶`).
- **Brand menu items (favicon, see below):** Bing, Google (×2), Yahoo, eBay, MSN, DuckDuckGo,
  Vimeo, Facebook, Twitter, LinkedIn, Instagram, VK, Pinterest. Parent menus
  (`LinksToolStripMenuItem`, `SocialNetworksToolStripMenuItem`) get generic Fluent icons.
- **Remove these menu items entirely** — decided by the user: `FormspringToolStripMenuItem`,
  `KeekToolStripMenuItem` (defunct services), `WebCrawlerToolStripMenuItem`,
  `LycosToolStripMenuItem`, `DogpileToolStripMenuItem`, `BadooToolStripMenuItem` (rarely used).
  Remove each item's declaration and setup in `Form1.Designer.vb`, its `.Image` entry in
  `Form1.resx`, its entry in the parent menu's `DropDownItems.AddRange`, and its `Click`
  handler in `Form1.vb`.

### Private window — `PrivateBrowserForm`
- Toolbar text-glyph buttons → Fluent icons (`DisplayStyle=Image`, keep `Text` as tooltip):
  `btnBack` `◀`, `btnForward` `▶`, `btnRefresh` `🔄`, `btnHome` `🏠`, `btnGo` `→`, `btnBookmark` `⭐`.
- `lblPrivateIcon` (`🕶`, large banner glyph) and `lblPrivateBadge` (`"🕶 Private Mode"`).
- Must look consistent with `Form1`'s toolbar (same glyphs, same size).

### Smaller dialogs
| Form | Item(s) |
|---|---|
| `CookieViewer` | `btnDeleteALL.Image`, `ImageList1` (tree/list images) |
| `Rss` | `tsRssGo.Image` |
| `ftp` | `ToolStripLabel1/2/3.Image` |
| `task manager` | `ToolStripButton1.Image` |
| `Bookmarks`, `History`, `mainForm` (Download Manager) | `PictureBox1.Image` — replace if it's a generic glyph-style image; leave if it's brand artwork |
| `AboutBox` | `PictureBox1.Image` — K-Browser logo, **leave as is** |
| `AdBlockPopup` | `lblShield` `🛡️`, `btnAdBlockSettings` `⚙️`, `btnReload` `↻` |
| `PasswordDetailForm` | `lblIcon` `🔐`, `btnTogglePassword` `👁` (eye / eye-off toggles with state) |
| `SavePasswordForm` | `lblIcon` `🔑` |
| `Settings` | `btnCreateBackup` `📦`, `btnRestoreBackup` `🔄`, `btnResetSettings` `⚠️`; status labels `✓`/`⚠️` |
| `Source` | `lblSearch` `🔍`, `btnCopy` `📋`, `btnSave` `💾`, `btnToggleWrap` `📄`, `btnPrint` `🖨️` |

`Settings` group-box titles (`🖥️ Graphics`, `📑 Tabs`, `⚡ Page Loading`): a `GroupBox` caption
can't hold an image, so **strip the emoji and keep the text** *(Claude's call)*.

Runtime-set strings that are status text rather than icons (e.g. `"⚠️ Tab Crashed"` tab caption
in `Form1.vb`, `"✓ Copied!"` in `Source.vb`) — strip the emoji, keep the words *(Claude's call)*.

The builder should grep once more for emoji/arrow glyphs in `*.Designer.vb`/`*.designer.vb`
and `*.vb` (including runtime assignments such as `AdBlockPopup.vb` lines 55/59) to catch
anything this list missed.

## How it works

### `IconService` (new, e.g. `IconService.vb`)
- Loads the bundled Fluent font(s) once into a `PrivateFontCollection`.
- API shape (names are the builder's call):
  `GetIcon(glyph As FluentIcon, sizePx As Integer, color As Color, Optional filled As Boolean = False) As Bitmap`
  where `FluentIcon` is an enum (or constants) mapping readable names → code points, covering
  only the icons the app uses.
- Renders the glyph centred on a transparent bitmap sized for the current DPI
  (logical size × `DeviceDpi / 96`). Caches by (glyph, size, colour, filled, dpi).
- Convenience helpers to apply an icon to a `ToolStripItem`, `Button`, `Label` so forms don't
  repeat boilerplate.

### Theming
- **Not every form is themed by `ThemeManager`** *(found in review)*. `PrivateBrowserForm`,
  `PasswordDetailForm` and `SavePasswordForm` are hard-coded dark in their designers and never
  call `ApplyTheme`; `mainForm` (Download Manager) is hard-coded light. So the icon colour rule is
  *(Claude's call)*:
  - Default icon colour = the hosting control's effective `ForeColor` (which `ThemeManager`
    sets on themed forms and the designer sets on fixed-palette forms). This keeps icons legible
    on both kinds of form.
  - **Keep today's deliberate colours.** On the fixed-palette forms the accent glyphs
    (`lblPrivateIcon`, `lblPrivateBadge`, private `btnGo`, both password-form `lblIcon`s,
    `btnTogglePassword` — lime (161,197,74); private `btnBookmark` — sub-text) already carry the
    colour in their `ForeColor`, so the default rule keeps it. Two places need an **explicit**
    colour because `ThemeManager` overwrites label colours: `AdBlockPopup.lblShield` uses the
    status colour (220,50,50) when disabled / (30,150,60) when active, and the Settings status
    labels use `Color.DarkGoldenrod` (warning) / `Color.ForestGreen` (applied). `IconService`
    therefore accepts an optional explicit colour.
  - Disabled items use the sub-text colour (`DarkSubText` / `LightSubText`, or the form's
    equivalent) rather than WinForms' auto-greyed bitmap, which looks muddy on dark backgrounds.
- When `ThemeManager.ApplyTheme(form)` runs, the theme-derived icons on that form are
  re-rendered in the new colours; fixed-palette forms and explicitly-coloured icons are unaffected. How the form's icon assignments are remembered (a registry keyed by item, an
  interface each form implements, `Tag` data, etc.) is the builder's call; the requirement is
  that toggling dark mode in the main window leaves no stale-coloured icon in any form that
  `ThemeManager` re-themes, and that forms opened later come up correctly coloured.
- No accent-colour hover effect in this change (keeps scope to icons; the existing renderer's
  hover look stays).

### Sizes and DPI
- **DPI scope** *(Claude's call, found in review)*: the app sets no `HighDpiMode`, so it runs
  System-aware. Icons are rendered for the DPI in effect at launch and are crisp there. Switching
  the app to PerMonitorV2 (crisp when dragged to a monitor with different scaling) would re-scale
  every form's layout and is **out of scope** for this change.
- Toolbar buttons: 20 px logical glyph on the existing 36×36 buttons; 16 px for menu items and
  inline button/label icons; 32 px for the large banner glyphs (`lblPrivateIcon`, password-form
  icons, dialog picture boxes). All drawn from the Resizable font's `_20_` glyph designs.
- Menu strips: set `ImageScalingSize` to the DPI-scaled 16 px so menus don't stretch the bitmap.

### Favicons for brand menu items
- Fetch `https://<site-domain>/favicon.ico` directly with `HttpClient` (no third-party favicon
  service — this is a privacy-focused browser). Follow redirects; accept ICO or PNG.
- Cache to disk under the app's data folder so it's fetched once, not every launch; in-memory
  cache on top. `Form1` already declares an unused `faviconCache` — reuse or remove it.
- Fetch lazily (when the menu first opens) or in the background after start-up — never block
  start-up or the UI thread.
- Show a Fluent `Globe` icon until the favicon loads, and keep it if the fetch fails.

### Removing the old images
- Delete each replaced `*.Image` / `ImageStream` entry from the form's `.resx` **and** the matching
  `resources.GetObject(...)` line in its `Designer.vb` in the same change.
- Icons are assigned in code (e.g., in each form's `Load`), not stored in the designer.

### Packaging
- Embed the `.ttf` file(s) as `EmbeddedResource` and load via `AddMemoryFont`
  *(Claude's call — avoids touching `installer.iss` / ClickOnce file lists)*. Keep the font
  memory pinned for the app's lifetime.
- Add the Fluent UI System Icons MIT licence text to the repo (e.g., `ThirdPartyNotices.txt`).

## Suggested icon mapping (builder confirms names exist in the font's mapping JSON)

| Use | Fluent icon |
|---|---|
| Back / Forward | `ArrowLeft` / `ArrowRight` |
| Refresh / Reload | `ArrowClockwise` |
| Stop | `Dismiss` |
| Home | `Home` |
| New tab / Close tab | `TabAdd` (or `Add`) / `TabDismiss` (or `Dismiss`) |
| New window | `WindowNew` |
| Go | `ArrowEnter` (distinct from Forward) |
| RSS | `Rss` |
| Add bookmark | `Star` (regular); `Star` filled when the current page is already bookmarked, if that state is readily available |
| Ad blocker | `Shield` / `ShieldCheckmark` |
| Private | `Glasses` (or closest incognito-style glyph) |
| Dark mode | `WeatherMoon` |
| Settings | `Settings` |
| Downloads | `ArrowDownload` |
| History | `History` |
| Cookies | `Cookies` |
| Print / Save | `Print` / `Save` |
| Cut / Copy | `Cut` / `Copy` |
| Zoom | `ZoomIn` |
| Search / Find | `Search` |
| Password / key / lock | `Password` / `Key` / `LockClosed` |
| Show/hide password | `Eye` / `EyeOff` |
| Warning / OK | `Warning` / `Checkmark` |
| Exit | `SignOut` (or `Dismiss`) |
| About | `Info` |
| Fallback for brand items | `Globe` |

## Verification

- `dotnet build` succeeds with no new warnings.
- Run the app and visually check, in **both** light and dark mode, after toggling at runtime:
  main toolbar, every main menu, the private window, and each dialog in the inventory.
- Check at 100% and 150% Windows display scaling: icons are sharp and centred, not clipped.
- Open the Links and Social menus offline: globe placeholders appear, no hang or exception.
  Online: favicons appear and are cached on the next launch.
- Grep confirms no remaining `resources.GetObject("...Image")` calls for removed bitmaps, and no
  emoji used as icons in UI text (outside deliberate prose like the About text).

## Left to the builder

- Exact `IconService` API, enum/constant naming, and how forms register icons for re-theming.
- Exact Fluent glyph choices where the mapping above offers alternatives.
- Whether brand favicons load lazily on menu open or in the background after start-up.
- How the Fluent font's code points are extracted (mapping JSON shipped with the font release).
