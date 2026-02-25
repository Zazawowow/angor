# Avalonia UI Specification

**Reference:** [angor.tx1138.com/app.html](https://angor.tx1138.com/app.html) (PWA)  
**Storybook:** [angor.tx1138.com/storybook](https://angor.tx1138.com/storybook)  
**Purpose:** Single source of truth mapping web design → Avalonia implementation. Use this doc when applying UI changes.

**To add new mappings:** See [DESIGN_EXTRACTION_GUIDE.md](DESIGN_EXTRACTION_GUIDE.md).

---

## 1. Component Map (Web → Avalonia)

| Web (CSS class / element) | Avalonia file | Avalonia control |
|---------------------------|---------------|------------------|
| `.app-container` | `ShellView.axaml` | `UserControl` root |
| `.app-container::before` (crosshatch texture) | `Colors.axaml` → `AppTextureBrush` | `Border` overlay in ShellView |
| `aside` (sidebar) | `ShellView.axaml` | `SectionStrip` |
| `nav a` / `.nav-item` | `SectionStrip.axaml` | `SectionStripItem` |
| `nav a.active` | `SectionStrip.axaml` | `SectionStripItem:selected` |
| `main` (content area) | `ShellView.axaml` | `OverlayBorder` |
| `.invest-column` / `.launch-column` | `HomeSectionView.axaml` | `Grid` + `Border.Panel` + logo overlay |
| `.invest-column::before` (logo pattern) | `Colors.axaml` → `TiledLogoBrush` | `Border` with `CornerRadius="16"` |
| `.content-card` | `Border.axaml` | `Border.Panel` |
| `.btn-primary` | `Buttons.axaml` | `EnhancedButton` with `Classes="Primary"` |
| `.btn-secondary` | `Buttons.axaml` | `EnhancedButton` with `Classes="Secondary"` |
| `header` (top bar) | `ShellView.axaml` | `DockPanel` in row 0 |
| `header button` | `ShellView.axaml` | `ToggleButton`, `EnhancedButton` with `Classes="Widget"` |

---

## 2. Design Tokens (Web → Avalonia)

### 2.1 Colors

| Token | Web (ultra-modern dark) | Avalonia (Dark theme) | File |
|-------|-------------------------|------------------------|------|
| App background | `#0a0a0a` | `AppBackground` = `#0A0A0A` | Colors.axaml |
| App background (light) | `#faf9f6` | `AppBackground` = `#faf9f6` | Colors.axaml |
| Card surface | `#1a1a1a` / `#1a1a1ab3` | `Surface` / `CardSurface` | Colors.axaml |
| Card border | `rgba(255,255,255,.1)` | `Stroke` = `#19FFFFFF` | Colors.axaml |
| Primary button bg | `linear-gradient(135deg,#2a2a2a,#1a1a1a)` | `PrimaryButtonBackground` | Colors.axaml |
| Primary button border | `#3a3a3a` | `PrimaryButtonBorder` = `#3A3A3A` | Colors.axaml |
| Brand / accent | `#2d5a3d` | `Brand` = `#2D5A3D` | Colors.axaml |
| Text strong | `#fafafa` | `TextStrong` = `#FAFAFA` | Colors.axaml |
| Text muted | `#a0a0a0` / `gray` | `TextMuted` = `#A0A0A0` | Colors.axaml |
| Focus ring (inputs) | `#f7931a` | `BitcoinOrange` | Colors.axaml |

### 2.2 Spacing (4pt system)

All spacing must be multiples of 4. Use `Tokens.axaml`:

| Token | Value | Use |
|-------|-------|-----|
| `Spacing1` / `Thickness4` | 4 | Tight gaps |
| `Spacing2` / `Thickness8` | 8 | Small padding |
| `Spacing3` | 12 | Compact |
| `Spacing4` / `Thickness16` | 16 | Standard |
| `Spacing5` | 24 | Section gaps, card column gap |
| `Spacing6` / `Thickness32` | 32 | Card inner padding |
| `Spacing7` | 40 | Large section spacing |
| `Spacing8` | 48 | Page-level |

### 2.3 Border Radius

| Web | Avalonia | Use |
|-----|----------|-----|
| `8px` | `RadiusSm` = 6 (or 8) | Small elements |
| `10px` | — | Header buttons (use 10 or 12) |
| `12px` | `RadiusMd` = 12 | Buttons, nav items |
| `16px` | `RadiusLg` = 16 | Content cards, home cards |

### 2.4 Typography

Font family and weights match the Vue app (`app.css`, `dashboard.css`):

| Token | Value |
|-------|-------|
| Sans-serif | Roboto (matches `--angor-font-sans-serif`) |
| Monospace | Consolas, Monaco, Menlo, Liberation Mono, Courier New (matches `--angor-font-monospace`) |

| Web | Avalonia | Token / value |
|-----|----------|---------------|
| Title (h1/h2) | `TextBlock.Title` | 22px Bold (700) |
| Subtitle | `TextBlock.Subtitle` | 18px Light (300) |
| Body | `TextBlock.Regular` | 16px Normal (400) |
| Small / muted | `TextBlock.Size-XS` | 12px |
| Nav item | — | 14px Medium (500) |
| Nav item selected | — | 14px SemiBold (600) |
| Group header (INVESTOR / FOUNDER) | `SectionStripGroupHeader` | 10px Bold (700), Roboto, letter-spacing 1 |

Weight mapping (CSS → Avalonia): 300=Light, 400=Normal, 500=Medium, 600=SemiBold, 700=Bold.

---

## 3. Layout Patterns

### 3.1 App Shell

```
┌─ UserControl (AppBackground) ─────────────────────────────────────────────┐
│  ┌─ Grid (Margin 0,20,20,20) ──────────────────────────────────────────┐ │
│  │  Row 0: Logo | Header (title, wallet, theme, settings)              │ │
│  │  Row 1: SectionStrip (200px) | OverlayBorder (content)               │ │
│  │  Overlay: Border (AppTextureBrush, IsHitTestVisible=False)          │ │
│  └─────────────────────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────────────────────┘
```

### 3.2 Home Cards

```
Grid ColumnDefinitions="*,24,*"  ← equal columns, 24px gap
  Column 0: Fund Projects card
  Column 2: Get Funded card

Each card = Grid (stacked layers):
  Layer 1: Border.Panel (surface, shadow, border)
  Layer 2: Border (TiledLogoBrush, CornerRadius=16)
  Layer 3: StackPanel (Margin=40, Spacing=20)
    - PathIcon 80×80
    - Title
    - Subtitle
    - EnhancedButton
```

### 3.3 Sidebar (SectionStrip)

- `Padding="8,8,8,8"` on SectionStrip
- `ItemPadding="0"` (content padding via SectionStripItem)
- `SectionStripItem`: `Margin="2,2,2,2"`, `Padding="12,9,12,9"`, `CornerRadius="8"`
- `SectionStripGroupHeader`: `Margin="0,20,0,0"`, `FontSize="11"`, `FontWeight="SemiBold"`

---

## 4. Shadows (Web → Avalonia)

| Web | Avalonia | Token |
|-----|----------|-------|
| `0 20px 40px #0009` (cards) | `0 20 40 0 #99000000` | `ItemShadowBig` (dark) |
| `0 6px 20px #00000080` (nav active) | `0 1 6 0 #30000000` | `SectionShadow` |
| `0 4px 16px #0006` (buttons) | `0 2 8 0 #18000000` | `DefaultShadow` |
| `0 4px 16px #0000000a` (light cards) | `0 4 16 0 #20000000` | `ItemShadowBig` (light) |

---

## 5. Background Textures

### 5.1 App-wide crosshatch

**Web:** `.app-container::before`  
```css
background: repeating-linear-gradient(45deg, transparent 2px, rgba(255,255,255,.02) 4px),
            repeating-linear-gradient(-45deg, transparent 2px, rgba(0,0,0,.01) 4px);
```

**Avalonia:** `AppTextureBrush` (DrawingBrush) in Colors.axaml  
- 4×4px tile, white lines ~2% opacity, black lines ~1%  
- Applied as full-span `Border` overlay in ShellView

### 5.2 Card logo pattern

**Web:** `.invest-column::before`  
```css
background-image: url(logo.svg);
background-size: 80px;
opacity: .08;  /* dark theme */
opacity: .03;  /* light theme */
```

**Avalonia:** `TiledLogoBrush` (VisualBrush)  
- `DestinationRect="0,0,80,86"` (logo.svg aspect)
- Dark: `Opacity="0.08"` | Light: `Opacity="0.03"`
- Applied per-card as `Border` overlay with `CornerRadius="16"`

---

## 6. State Mappings

| Web state | Avalonia selector | Key properties |
|-----------|-------------------|----------------|
| `nav a` (default) | `SectionStripItem` | `Foreground=TextMuted`, `FontWeight=Medium` |
| `nav a:hover` | `SectionStripItem:pointerover` | `Background=SurfaceHover` |
| `nav a.active` | `SectionStripItem:selected` | `Background=SurfaceSelected`, `Foreground=Highlight`, `FontWeight=SemiBold`, `BoxShadow=SectionShadow` |
| `.btn-primary` | `EnhancedButton` + `Classes="Primary"` | `PrimaryButtonBackground`, `PrimaryButtonBorder` |
| `.btn-primary:hover` | `:pointerover` | `DefaultShadowHover`, `transform` (Avalonia: transition) |
| Card hover | — | Optional: `DefaultShadowHover` |

---

## 7. Avalonia File Index

### Core

| Path | Purpose |
|------|---------|
| `UI/Shell/ShellView.axaml` | App shell, header, sidebar, content frame |
| `UI/Themes/V2/Resources/Colors.axaml` | Colors, brushes, shadows, theme dictionaries |
| `UI/Themes/V2/Resources/Tokens.axaml` | Spacing, typography, radius, thickness |
| `UI/Themes/V2/Styles/SectionStrip.axaml` | Sidebar nav item and group header styles |
| `UI/Themes/V2/Styles/Border.axaml` | `Border.Panel` (cards) |
| `UI/Themes/V2/Styles/Buttons.axaml` | EnhancedButton, Primary, Secondary |
| `UI/Themes/V2/Styles/Typography.axaml` | Title, Subtitle, Size-*, Weight-* |

### Sections (sidebar nav → content)

| Section | Avalonia file | Web route / equivalent |
|---------|---------------|------------------------|
| Home | `Sections/Home/HomeSectionView.axaml` | `/` — Fund Projects + Get Funded cards |
| Funds | `Sections/Funds/FundsSectionView.axaml` | `/funds` — wallet accounts |
| Find Projects | `Sections/FindProjects/FindProjectsSectionView.axaml` | `/investor` — project list |
| Funded | (uses FindProjects or dedicated) | `/investor` funded tab |
| My Projects | `Sections/MyProjects/MyProjectsSectionView.axaml` | `/founder` — creator dashboard |
| Portfolio | `Sections/Portfolio/PortfolioSectionView.axaml` | Investor portfolio |
| Browse | `Sections/Browse/BrowseSectionView.axaml` | Browse/discover |
| Founder | `Sections/Founder/FounderSectionView.axaml` | Founder hub |
| Wallet | `Sections/Wallet/Main/WalletSectionView.axaml` | Wallet management |
| Settings | `Sections/Settings/SettingsSectionView.axaml` | App settings |

### Shared controls

| Control | Path | Use |
|---------|------|-----|
| ProjectCard | `Shared/Controls/ProjectCard.axaml` | Project list items |
| Badge | `Shared/Controls/Badge.axaml` | Status badges |
| EdgePanel | `Shared/Styles/EdgePanel.axaml` | Icon + label layout |
| OverlayBorder | `Themes/V2/Styles/OverlayBorder.axaml` | Card/panel with shadow |

---

## 8. Extracted Web CSS (Reference)

### invest-column / launch-column (home cards)

```css
background: #1a1a1ab3;
border: 1px solid rgba(255,255,255,.1);
box-shadow: 0 20px 40px #0009, 0 0 0 1px #ffffff0d;
/* ::before: logo.svg, 80px, opacity .08 (dark) / .03 (light) */
```

### nav a.active (selected sidebar item)

```css
background: linear-gradient(135deg, #2a2a2a, #1a1a1a);
border: 1px solid #3a3a3a;
box-shadow: 0 6px 20px #00000080, inset 0 1px #ffffff1a;
font-weight: 600;
```

### .btn-primary (dark)

```css
background: linear-gradient(135deg, #2a2a2a, #1a1a1a);
border: 1px solid #3a3a3a;
box-shadow: 0 4px 16px #0006, inset 0 1px #ffffff14;
color: #fafafa;
font-weight: 700;
border-radius: 12px;
```

### .content-card (dark)

```css
background: linear-gradient(135deg, #1a1a1a, #0f0f0f);
border: 1px solid #2a2a2a;
box-shadow: 0 20px 40px #0009, 0 0 0 1px #ffffff08;
border-radius: 16px;
```

---

## 9. Hot Reload (HotAvalonia)

**Package:** `HotAvalonia` (already in AngorApp.csproj)

XAML hot reload is enabled by default when running under the debugger. Edit any `.axaml` file and save — changes apply in real time without restarting the app.

- **Usage:** Run the app with F5 (debug). Edit XAML, save. Changes appear automatically.
- **Optional:** Add `MonoMod.RuntimeDetour` to hot reload embedded assets (icons, images).
- **`[AvaloniaHotReload]`:** Apply to parameterless methods to re-run logic on hot reload (e.g. `InitializeComponentState`).

---

## 10. Checklist for New UI Work

- [ ] Use 4pt spacing (Tokens.axaml)
- [ ] Use design tokens, not hardcoded values
- [ ] Map to web component in this spec before implementing
- [ ] Match shadow, radius, and border from web
- [ ] Test light and dark themes
- [ ] Ensure equal-width cards when side-by-side (Grid with `*` columns)
