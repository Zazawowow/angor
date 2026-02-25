# Design Extraction Guide

How to extract design values from the PWA (angor.tx1138.com) and add them to `docs/AVALONIA_UI_SPEC.md`.

---

## 1. Open the PWA and DevTools

1. Open [angor.tx1138.com/app.html](https://angor.tx1138.com/app.html) in Chrome/Edge.
2. Open DevTools (F12 or Cmd+Option+I).
3. Use the element picker (top-left of DevTools) to click the element you want to match.

---

## 2. Extract Computed Styles

In the **Computed** tab (or **Styles** tab), note:

| Property | Where to put in Avalonia |
|----------|--------------------------|
| `padding` | `Margin` or `Padding` on the control |
| `margin` | `Margin` on the control |
| `font-size` | `FontSize` or `TextBlock` class |
| `font-weight` | `FontWeight` |
| `color` | `Foreground` or `DynamicResource` (TextStrong, TextMuted, etc.) |
| `background-color` | Add to Colors.axaml if new |
| `background` (gradient) | `LinearGradientBrush` in Colors.axaml |
| `border` | `BorderBrush`, `BorderThickness` |
| `border-radius` | `CornerRadius` — use 4pt values (8, 12, 16) |
| `box-shadow` | `BoxShadow` — convert to Avalonia format `offsetX offsetY blur spread color` |

---

## 3. Convert CSS to Avalonia

### Box shadow

```css
box-shadow: 0 20px 40px #0009;
```

```xml
<BoxShadows>0 20 40 0 #99000000</BoxShadows>
<!-- Avalonia: offsetX offsetY blurRadius spreadRadius color -->
<!-- #0009 = rgba(0,0,0,0.36) → use hex with alpha: #99000000 for ~60% -->
```

### Gradient

```css
background: linear-gradient(135deg, #2a2a2a, #1a1a1a);
```

```xml
<LinearGradientBrush StartPoint="0%,0%" EndPoint="100%,100%">
    <GradientStop Offset="0" Color="#2A2A2A" />
    <GradientStop Offset="1" Color="#1A1A1A" />
</LinearGradientBrush>
```

### Padding / margin

Use 4pt multiples. Round 10px → 8 or 12; 18px → 16 or 20.

---

## 4. Add to AVALONIA_UI_SPEC.md

When you add a new component mapping:

1. **Component Map (Section 1):** Add row: `Web class` | `Avalonia file` | `Avalonia control`
2. **Tokens (Section 2):** Add any new colors/thicknesses to the table
3. **Extracted CSS (Section 8):** Paste the raw CSS for reference

---

## 5. Storybook CSS Location

The minified CSS is at:
`https://angor.tx1138.com/storybook/assets/preview.css`

Search for:
- `ultra-modern` — dark theme
- `ultra-modern-light` — light theme
- `invest-column`, `launch-column` — home cards
- `nav a`, `nav a.active` — sidebar
- `btn-primary`, `content-card` — buttons and cards

---

## 6. Screenshot + Annotation

For complex layouts:

1. Screenshot the web element.
2. Annotate with measurements (padding, gap, icon size).
3. Save in `docs/screenshots/` with a descriptive name.
4. Reference in AVALONIA_UI_SPEC.md: `See docs/screenshots/home-cards-annotated.png`
