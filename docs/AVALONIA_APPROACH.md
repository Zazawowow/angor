# Avalonia Approach: Fast, Responsive, Web-Like UI Development

Research-backed recommendations for making Angor's Avalonia app faster to develop, more responsive, and easier to style—without leaving the Avalonia ecosystem.

**→ For pixel-perfect web→Avalonia mapping, see [AVALONIA_UI_SPEC.md](AVALONIA_UI_SPEC.md).**

---

## 1. Layout: FlexPanel (CSS-like Flexbox)

### Recommended: Avalonia.Labs.Panels

**Package:** `Avalonia.Labs.Panels` (NuGet)  
**Version:** 11.3.1 (Aug 2025)  
**Source:** [AvaloniaUI/Avalonia.Labs](https://github.com/AvaloniaUI/Avalonia.Labs) – official experimental controls

**Why this over alternatives:**
- **Alba.Avalonia.FlexPanel** – Deprecated, merged into Avalonia.Labs
- **Avalonia.Flexbox** (jp2masa) – Older, less maintained
- **Avalonia.Labs.Panels** – Official, actively maintained, used by NexusMods.App (2.1K stars)

**Features:**
- `FlexDirection` (Row, Column, RowReverse, ColumnReverse)
- `JustifyContent`, `AlignItems`, `AlignContent`
- `Gap`, `FlexWrap`
- Per-child: `FlexGrow`, `FlexShrink`, `FlexBasis`, `AlignSelf`
- Single panel replaces Grid + StackPanel + WrapPanel combos

**Limitations:** Not 100% CSS-spec complete; some edge cases. Set child alignment to Stretch when using non-default alignment.

**Usage:**
```xml
<FlexPanel FlexDirection="Row" JustifyContent="SpaceBetween" AlignItems="Center" Gap="12">
    <TextBlock Text="Label" />
    <Button Content="Action" />
</FlexPanel>
```

---

## 2. Styling: Utility Classes (Tailwind-like)

Create a central `Styles/Utilities.axaml` with reusable classes:

| Category | Classes | Purpose |
|----------|---------|---------|
| **Layout** | `.flex`, `.flex-col`, `.items-center`, `.justify-between`, `.gap-4`, `.gap-8` | FlexPanel + common layouts |
| **Spacing** | `.p-4`, `.px-6`, `.py-3`, `.m-2`, `.mt-4` | Padding, margin |
| **Typography** | `.text-sm`, `.text-base`, `.text-lg`, `.font-medium`, `.font-bold`, `.text-muted` | Font size, weight, color |
| **Sizing** | `.w-full`, `.min-w-200`, `.h-10` | Width, height |
| **Borders** | `.rounded`, `.rounded-lg`, `.border` | Corner radius, borders |

Apply via `Classes="flex gap-4 items-center"` instead of per-control setters.

---

## 3. Design Tokens

Centralize in `Colors.axaml` or `Tokens.axaml`:

```xml
<!-- Spacing scale (4px base) -->
<Thickness x:Key="Spacing1">4</Thickness>
<Thickness x:Key="Spacing2">8</Thickness>
<Thickness x:Key="Spacing3">12</Thickness>
<Thickness x:Key="Spacing4">16</Thickness>
<Thickness x:Key="Spacing6">24</Thickness>
<Thickness x:Key="Spacing8">32</Thickness>

<!-- Typography -->
<x:Double x:Key="FontSizeSm">12</x:Double>
<x:Double x:Key="FontSizeBase">14</x:Double>
<x:Double x:Key="FontSizeLg">16</x:Double>

<!-- Radius -->
<x:Double x:Key="RadiusSm">6</x:Double>
<x:Double x:Key="RadiusMd">8</x:Double>
<x:Double x:Key="RadiusLg">12</x:Double>
```

Reference everywhere: `{StaticResource Spacing4}` instead of hardcoded `16`.

---

## 4. Performance Optimizations

### Already in place ✓
- `AvaloniaUseCompiledBindingsByDefault="true"` – Reduces reflection overhead

### Recommended additions

| Optimization | Impact | Action |
|--------------|--------|--------|
| **Compiled bindings** | High | Already enabled |
| **Virtualization** | High for lists | Use `VirtualizingStackPanel` for long lists; avoid nested `ScrollViewer` in item templates |
| **ItemsRepeater** | High for grids | Use `Avalonia.Controls.ItemsRepeater` + `UniformGridLayout` for virtualized grids |
| **Flat visual tree** | Medium | Avoid deep nesting; combine elements where possible |
| **Async data loading** | High | Keep I/O off UI thread; use async/await |
| **StreamGeometry** | Medium | Prefer over PathGeometry for paths |
| **Image sizing** | Medium | Use appropriately sized images, not scaled-down large ones |
| **Fix binding errors** | High | Each error causes a perf dip; avoid RelativeSource.FindAncestor in DataTemplates |

### Native AOT (optional, advanced)

- **Benefit:** ~450ms → ~50ms cold start, smaller binaries, less memory
- **Requirement:** `PublishAot=true`, compiled bindings, trimmer config
- **Caveat:** Some third-party controls may not be AOT-compatible (Zafiro, etc.)

---

## 5. Developer Experience: Hot Reload

### HotAvalonia (recommended)

**Package:** `HotAvalonia` (3.0.2+)  
**Stars:** 417 on GitHub  
**License:** MIT

Real-time XAML updates as you edit—no full app restart for UI changes.

```bash
dotnet add package HotAvalonia
```

Integrates with your dev workflow; edit XAML and see changes immediately.

### Live.Avalonia (alternative)

Uses `dotnet watch build` + `ILiveView`. Add to `.csproj`:
```xml
<ItemGroup>
  <Watch Include="**/*.xaml" />
</Watch>
```
Note: Project is archived; HotAvalonia is more actively maintained.

---

## 6. Frameworks & Libraries

| Library | Purpose | Status |
|---------|---------|--------|
| **Zafiro.Avalonia** | Controls, SectionStrip, icons | ✓ In use |
| **Avalonia.Labs.Panels** | FlexPanel | Add for layout |
| **HotAvalonia** | XAML hot reload | Add for DX |
| **Avalonia Accelerate** | Dev Tools, TreeDataGrid, Parcel | Paid; TreeDataGrid for large grids |
| **Material.Avalonia** | Material Design theme | Alternative theme |
| **ItemsRepeater** | Virtualized grid layout | Consider for CardGrid, project lists |

---

## 7. Implementation Order

1. **Avalonia.Labs.Panels** – Add FlexPanel for layout
2. **Utilities.axaml** – Create utility classes for common patterns
3. **Design tokens** – Centralize spacing, typography, radius
4. **HotAvalonia** – Enable hot reload for faster iteration
5. **Virtualization audit** – Ensure long lists use VirtualizingStackPanel
6. **Binding audit** – Fix any binding errors (check trace output)

---

## 8. UI Component Frameworks (with Custom Styling)

Frameworks that provide pre-built components while allowing you to apply your own styles/branding.

### Comparison

| Framework | Components | Customization | Best For |
|-----------|------------|---------------|----------|
| **Material.Avalonia** | Buttons, cards, dialogs, text fields, chips, etc. | Primary/secondary colors, override any brush | Material Design look, strong theming |
| **FluentAvalonia** | Fluent v2 controls, NavigationView, InfoBar, etc. | Accent color, resource overrides | Windows 11–style UI |
| **Semi.Avalonia** | Themed native controls, extra variants | ControlTheme per control, Classes | Semi Design, flexible variants |
| **Built-in Fluent** | Standard Avalonia controls | ThemeDictionaries, Style overrides | Minimal deps, full control |
| **Zafiro** (current) | SectionStrip, dialogs, icons | Style overrides | Lightweight, composable |

### Material.Avalonia – Strong Theming

**Customization:**
- `CustomMaterialTheme` – Set `PrimaryColor` and `SecondaryColor` per light/dark
- Override any brush in `Application.Resources` (e.g. `PrimaryHueLightBrush`)
- [Brush names list](https://github.com/AvaloniaCommunity/Material.Avalonia/Brush-Names)
- Runtime theme changes via `CurrentTheme`

```xml
<CustomMaterialTheme>
  <CustomMaterialTheme.Palettes>
    <CustomMaterialThemeResources x:Key="Dark" PrimaryColor="#4B7C5A" SecondaryColor="#3a3a3a" />
    <CustomMaterialThemeResources x:Key="Light" PrimaryColor="#4B7C5A" SecondaryColor="#2d5a3d" />
  </CustomMaterialTheme.Palettes>
</CustomMaterialTheme>

<!-- Override specific brush -->
<Application.Resources>
  <SolidColorBrush x:Key="PrimaryHueLightBrush" Color="#4B7C5A" />
</Application.Resources>
```

### FluentAvalonia – Accent Override

**Customization:**
- `CustomAccentColor` – One color, auto-generates variants
- Override `SystemAccentColor`, `SystemAccentColorLight1`, etc.
- Standard Avalonia styles override any control

```xml
<fluent:FluentTheme CustomAccentColor="#4B7C5A" />

<!-- Or override resources -->
<App.Resources>
  <Color x:Key="SystemAccentColor">#4B7C5A</Color>
</App.Resources>
```

### Semi.Avalonia – Per-Control Themes

**Customization:**
- `Theme="{DynamicResource ProgressRing}"` – Swap control theme
- `Classes="Warning"` – Apply variant styles
- Design tokens for deeper customization

```xml
<Button Classes="Warning">Warning</Button>
<ProgressBar Theme="{DynamicResource ProgressRing}" Value="30" />
```

### Styling Override Pattern (All Frameworks)

Add styles **after** the framework theme in `Application.Styles`:

```xml
<Application.Styles>
  <FluentTheme />  <!-- or MaterialTheme, SemiTheme -->
  <StyleInclude Source="YourOverrides.axaml" />  <!-- Your custom styles last -->
</Application.Styles>
```

Your styles override framework defaults. Use `DynamicResource` for colors so they work with light/dark.

### Recommendation for Angor

**Option A: Stay with built-in Fluent + Zafiro** (current)
- You already have `Colors.axaml` with ThemeDictionaries
- Full control, no extra framework
- Add FlexPanel + utilities for layout

**Option B: Add Material.Avalonia**
- More components (cards, chips, dialogs)
- Strong theming: set `PrimaryColor="#4B7C5A"` for brand
- Override brushes for fine-grained control

**Option C: Add FluentAvalonia**
- Richer Fluent controls (NavigationView, etc.)
- `CustomAccentColor="#4B7C5A"` for brand
- Stays close to current Fluent base

---

## 9. Quick Reference

### FlexPanel (Avalonia.Labs.Panels) common patterns

```xml
<!-- Row, space between, centered -->
<FlexPanel FlexDirection="Row" JustifyContent="SpaceBetween" AlignItems="Center" Gap="12">

<!-- Column, centered -->
<FlexPanel FlexDirection="Column" AlignItems="Center" Gap="8">

<!-- Wrap with gap -->
<FlexPanel FlexWrap="Wrap" Gap="16">

<!-- Child grows to fill -->
<Button FlexPanel.Grow="1" Content="Fill" />
```

### Virtualization checklist

- [ ] Lists with 50+ items use VirtualizingStackPanel
- [ ] No ScrollViewer inside item template
- [ ] ItemsPresenter directly in ScrollViewer (no extra wrappers)
- [ ] Fixed or predictable item heights when possible
