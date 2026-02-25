---
name: avalonia-theme-style
description: Create or modify V2 theme styles and controls in Avalonia2, and register them in Theme.axaml
---

## What I do

Work with the V2 theme system in `src/Angor/Avalonia/Avalonia2/UI/Themes/V2/`:
- Create new theme styles (in `Styles/`)
- Create new theme controls (in `Controls/`)
- Edit existing styles for design parity with the Vue prototype
- Register new files in `Theme.axaml`
- Manage design tokens (Colors, Tokens, Icons)

## When to use me

Use this skill when:
- Adding a new styled control theme (e.g. a custom ListBox, ComboBox variant)
- Modifying colors, tokens, or typography
- Creating effects, transitions, or visual states
- Updating Theme.axaml to add/remove style includes

## Theme directory structure

```
UI/Themes/V2/
├── Theme.axaml              # Master style include file — App.axaml references this
├── Resources/
│   ├── Colors.axaml         # Theme-aware color definitions (Light/Dark variants)
│   ├── Tokens.axaml         # Spacing, font sizes, corner radius, shadows
│   └── Icons.axaml          # SVG path data for icons
├── Styles/
│   ├── Typography.axaml     # Font sizes, weights, text classes
│   ├── Buttons.axaml        # Button variants (Primary, Secondary, Widget, etc.)
│   ├── Layout.axaml         # Layout utilities
│   ├── Utilities.axaml      # Tailwind-like utility classes (flex, gap, p, m)
│   ├── ColorVariants.axaml  # Color modifier classes
│   ├── Pills.axaml          # Pill/tag styles
│   ├── Boxes.axaml          # Box container styles
│   ├── Border.axaml         # Border variants
│   ├── OverlayBorder.axaml  # Overlay border effects
│   ├── HeaderedContainer.axaml
│   ├── EdgePanel.axaml
│   ├── Effects.axaml        # Shadows, blur, transitions
│   ├── Icons.axaml          # Icon sizing/styling
│   ├── SectionStrip.axaml   # Navigation strip
│   ├── ToggleButtons.axaml
│   ├── CheckBox.axaml
│   ├── ComboBox.axaml
│   ├── ProgressBar.axaml
│   ├── ProgressRing.axaml
│   ├── Expander.axaml
│   ├── DialogControl.axaml
│   └── DialogViewContainer.axaml
└── Controls/
    ├── EnhancedButton.axaml # Extended button with icon support
    ├── Frame.axaml          # Content frame with header/footer
    ├── Wizard.axaml         # Step wizard control
    ├── CalendarPicker.axaml
    ├── NumericUpDown.axaml
    ├── ListBox.axaml
    └── ErrorSummary.axaml   # Error summary resource dictionary
```

## Theme.axaml registration

Every `.axaml` style/control file must be included in `Theme.axaml`:

```xml
<!-- In UI/Themes/V2/Theme.axaml -->

<!-- For Styles/ files -->
<StyleInclude Source="Styles/MyNewStyle.axaml" />

<!-- For Controls/ files -->
<StyleInclude Source="Controls/MyNewControl.axaml" />

<!-- For Shared/Controls/ files (absolute path from project root) -->
<StyleInclude Source="/UI/Shared/Controls/MyControl.axaml" />

<!-- For Shared/Styles/ files -->
<StyleInclude Source="/UI/Shared/Styles/MyStyle.axaml" />
```

Resources go in the `<Styles.Resources>` section:
```xml
<Styles.Resources>
    <ResourceDictionary>
        <ResourceDictionary.MergedDictionaries>
            <ResourceInclude Source="Resources/Colors.axaml" />
            <ResourceInclude Source="Resources/Tokens.axaml" />
            <ResourceInclude Source="Resources/Icons.axaml" />
        </ResourceDictionary.MergedDictionaries>
    </ResourceDictionary>
</Styles.Resources>
```

## Design tokens reference

### Colors (from Colors.axaml)

| Token | Light | Dark | Use |
|-------|-------|------|-----|
| AppBackground | #faf9f6 | #0A0A0A | App background |
| Surface | #FFFFFF | #1A1A1A | Card/panel background |
| SurfaceLow | #F5F5F5 | #2A2A2A | Subtle backgrounds |
| TextStrong | #0a0a0a | #FAFAFA | Primary text |
| TextMuted | #6b7280 | #A0A0A0 | Secondary text |
| Brand | #2D5A3D | #5FAF78 | Accent/brand color |
| Stroke | #E8E8E8 | #19FFFFFF | Borders |
| BitcoinOrange | #F7931A | #F7931A | Focus rings, BTC |

### Spacing tokens (from Tokens.axaml)

| Token | Value |
|-------|-------|
| Spacing1 / Thickness4 | 4 |
| Spacing2 / Thickness8 | 8 |
| Spacing3 | 12 |
| Spacing4 / Thickness16 | 16 |
| Spacing5 | 24 |
| Spacing6 / Thickness32 | 32 |
| Spacing7 | 40 |
| Spacing8 | 48 |

### Typography (from Typography.axaml)

| Class | Size |
|-------|------|
| Size-S | 12 |
| Size-M | 14 (base) |
| Size-L | 16 |
| Size-XL | 20 |
| Size-2XL | 24 |

### Corner radius

| Token | Value |
|-------|-------|
| RadiusSm | 6 |
| RadiusMd | 8 |
| RadiusLg | 12 |
| (cards) | 16 |

## Style file template

```xml
<Styles xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">

    <Design.PreviewWith>
        <Border Padding="20" Background="{DynamicResource AppBackground}">
            <!-- Preview content -->
        </Border>
    </Design.PreviewWith>

    <Style Selector="TargetControl.MyClass">
        <Setter Property="Background" Value="{DynamicResource Surface}" />
        <Setter Property="CornerRadius" Value="12" />
        <Setter Property="Padding" Value="16" />
    </Style>
</Styles>
```

## Rules

- All theme files are pure XAML — no C# code-behind, no SDK dependencies
- Use `DynamicResource` for colors (supports theme switching)
- Use `StaticResource` for tokens that don't change with theme (spacing, fonts)
- All spacing values must be multiples of 4
- Vue.js prototype is the visual source of truth
