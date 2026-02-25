# Frontend Strategy: Web-Like UI Development

## Current State

Angor has **two parallel desktop implementations**:

| Approach | Tech Stack | Styling | Distribution |
|----------|------------|---------|--------------|
| **Avalonia** (current) | XAML, Grid/StackPanel | AXAML styles, manual conversion from CSS | Windows .exe, Linux .deb/.AppImage, macOS .dmg, Android .apk |
| **Tauri + Blazor** (available) | Razor, HTML, CSS | Standard web: Flexbox, Grid, Tailwind, CSS variables | Configured but not in release workflow |

The **Blazor Client** (`src/Angor/Client`) is the web app at angor.io and already uses:
- HTML/CSS with Bootstrap utilities
- Flexbox, Grid, standard web layout
- CSS variables and scoped styles
- The "Vue" reference design you've been matching

## Recommendation: Use Tauri + Blazor for Desktop

**Switch desktop distribution from Avalonia to Tauri.** The Blazor app is already built with web-style UI and is wired to Tauri.

### Benefits

1. **Single UI codebase** for web + desktop
2. **Web-native tooling**: Flexbox, CSS Grid, Tailwind, CSS variables
3. **Faster iteration**: Change CSS, see results immediately
4. **No XAML/CSS conversion**: Write CSS once, use everywhere
5. **Smaller binaries**: Tauri uses the system webview instead of embedding a full UI stack

### What Changes

| Before | After |
|--------|-------|
| Develop in Avalonia (XAML) | Develop in Blazor (Razor + CSS) |
| Release Avalonia installers | Release Tauri installers |
| Two UI implementations | One UI implementation |

### What Stays

- **Android**: Keep Avalonia for Android (Tauri has no Android support)
- **Shared logic**: Angor.Sdk, Angor.Shared, etc. remain unchanged
- **Web app**: Blazor app continues to serve angor.io

### Migration Path

1. **Add Tauri to release workflow** – Build Tauri artifacts (dmg, msi, appimage) alongside or instead of Avalonia desktop
2. **Validate Tauri build** – Run `npm run tauri:build` and test installers
3. **Deprecate Avalonia desktop** – Keep Avalonia project only for Android
4. **Focus development on Blazor** – All new UI work happens in `src/Angor/Client`

### Quick Start: Run Tauri Desktop

```bash
# Start Blazor dev server + Tauri window
npm run tauri:dev
```

This runs the Blazor app in a native window. Styling uses your existing CSS.

---

## Alternative: Improve Avalonia (If You Must Keep It)

If you need to keep Avalonia for desktop (e.g. Android parity, specific features):

### 1. Add FlexPanel for Web-Like Layout

```xml
<!-- NuGet: Alba.Avalonia.FlexPanel -->
<FlexPanel FlexDirection="Row" JustifyContent="SpaceBetween" AlignItems="Center" Gap="12">
    <TextBlock Text="Label" />
    <Button Content="Action" />
</FlexPanel>
```

### 2. Create a Tailwind-Like Utility System

Define reusable style classes in a central AXAML file:

```xml
<!-- e.g. Styles/Utilities.axaml -->
<Style Selector=".flex">
  <Setter Property="(FlexPanel.FlexDirection)" Value="Row" />
</Style>
<Style Selector=".gap-4">
  <Setter Property="(FlexPanel.Gap)" Value="16" />
</Style>
```

### 3. Use Design Tokens

Centralize colors, spacing, and typography in `Colors.axaml` and reference them everywhere instead of hardcoding values.

---

## Summary

| Option | Effort | Result |
|--------|--------|--------|
| **Tauri + Blazor** | Low – infrastructure exists | Web-like UI, single codebase, easier styling |
| **Avalonia + FlexPanel** | Medium | Better layout, still XAML styling |
| **Avalonia + utilities** | High | More consistent, still not web-like |

**Recommended:** Use Tauri + Blazor for desktop. The Blazor app already provides the web-like development experience you want.
