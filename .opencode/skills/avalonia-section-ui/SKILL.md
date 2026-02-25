---
name: avalonia-section-ui
description: Build out an Avalonia2 section's UI with design parity against the Vue.js prototype, using components, tokens, and utility classes
---

## What I do

Implement the actual UI for an Avalonia2 section view, matching the Vue.js prototype design:
- Translate Vue/Tailwind layouts to Avalonia XAML
- Use the correct shared controls (PageContainer, ScrollableView, FlexPanel, etc.)
- Apply design tokens and utility classes
- Create sample/stub data in the viewmodel
- Match colors, spacing, typography, and layout from the web app

## When to use me

Use this skill when:
- Building out a section's real UI (beyond the initial stub)
- Translating a Vue component to Avalonia
- Need the web→Avalonia component map
- Need to match specific design patterns from the prototype

## Design reference

- **Vue prototype**: `angor-prototype/angor-prototype/src/`
- **Web app**: `angor.tx1138.com/app.html`
- **Storybook**: `angor.tx1138.com/storybook`
- **Spec doc**: `docs/AVALONIA_UI_SPEC.md`

## Web → Avalonia component map

| Web (CSS/HTML) | Avalonia | File |
|----------------|----------|------|
| `.app-container` | `ShellView` root | `UI/Shell/ShellView.axaml` |
| `aside` (sidebar) | `ListBox.Nav` | `UI/Shell/ShellView.axaml` |
| `main` (content) | `Border.Panel` | `UI/Shell/ShellView.axaml` |
| `.content-card` | `Border` with `Theme="{StaticResource CardOverlayBorder}"` | `Shared/Styles/Cards.axaml` |
| `.btn-primary` | `EnhancedButton Classes="Primary"` | `V2/Styles/Buttons.axaml` |
| `.btn-secondary` | `EnhancedButton Classes="Secondary"` | `V2/Styles/Buttons.axaml` |
| `div.flex` | `FlexPanel Classes="flex"` | `Avalonia.Labs.Panels` |
| `div.grid` | `Grid` or `UniformGrid` | Built-in |
| `div.flex.flex-col` | `FlexPanel Classes="flex flex-col"` | `Avalonia.Labs.Panels` |
| `gap-*` | `Classes="gap-4"` or `Spacing="16"` | `V2/Styles/Utilities.axaml` |
| `p-*` | `Classes="p-4"` or `Padding="16"` | `V2/Styles/Utilities.axaml` |
| `rounded-2xl` | `CornerRadius="16"` | — |
| `text-sm` | `Classes="Size-S"` | `V2/Styles/Typography.axaml` |
| `font-bold` | `Classes="Weight-Bold"` | `V2/Styles/Typography.axaml` |
| `text-gray-500` | `Classes="Text-Muted"` | `V2/Styles/Typography.axaml` |

## Tailwind spacing → Avalonia

| Tailwind | px | Avalonia |
|----------|-----|---------|
| p-1 / gap-1 | 4 | Padding="4" / Spacing="4" |
| p-2 / gap-2 | 8 | Padding="8" / Spacing="8" |
| p-3 / gap-3 | 12 | Padding="12" / Spacing="12" |
| p-4 / gap-4 | 16 | Padding="16" / Spacing="16" |
| p-5 | 20 | Padding="20" |
| p-6 / gap-6 | 24 | Padding="24" / Spacing="24" |
| p-8 / gap-8 | 32 | Padding="32" / Spacing="32" |
| p-10 | 40 | Padding="40" |
| p-12 | 48 | Padding="48" |

## Layout patterns

### Two-column card layout (Home section)
```xml
<Grid ColumnDefinitions="* *" ColumnSpacing="24">
    <Grid Grid.Column="0" MinHeight="480">
        <Border Classes="Panel" CornerRadius="16" />
        <StackPanel Spacing="20" VerticalAlignment="Center" Margin="40">
            <!-- Card content -->
        </StackPanel>
    </Grid>
    <Grid Grid.Column="1" MinHeight="480">
        <!-- Second card -->
    </Grid>
</Grid>
```

### Card list (Find Projects, Portfolio)
```xml
<ItemsControl ItemsSource="{Binding Projects}">
    <ItemsControl.ItemsPanel>
        <ItemsPanelTemplate>
            <WrapPanel Classes="CardGrid Listing" />
        </ItemsPanelTemplate>
    </ItemsControl.ItemsPanel>
    <ItemsControl.ItemTemplate>
        <DataTemplate>
            <controls:ProjectCard ProjectName="{Binding Name}" 
                                  ShortDescription="{Binding Description}" />
        </DataTemplate>
    </ItemsControl.ItemTemplate>
</ItemsControl>
```

### Settings list
```xml
<StackPanel Spacing="8">
    <controls:SectionItem Content="General" />
    <controls:SectionItem Content="Network" />
    <controls:SectionItem Content="Appearance" />
</StackPanel>
```

### Frame with header and footer (wizard steps)
```xml
<Frame HeaderPadding="24,16,24,16" FooterPadding="24,24,24,24">
    <Frame.Header>
        <TextBlock Classes="Size-XL Weight-Bold" Text="Step Title" />
    </Frame.Header>
    
    <controls:ScrollableView ContentPadding="24">
        <!-- Step content -->
    </controls:ScrollableView>
    
    <Frame.Footer>
        <StackPanel Orientation="Horizontal" Spacing="16" HorizontalAlignment="Right">
            <EnhancedButton Classes="Secondary" Content="Back" Padding="24,12" />
            <EnhancedButton Classes="Primary" Content="Next" Padding="24,12" />
        </StackPanel>
    </Frame.Footer>
</Frame>
```

## Sample data patterns

ViewModels should provide realistic stub data:

```csharp
public partial class FindProjectsSectionViewModel : ReactiveObject
{
    public ObservableCollection<ProjectStub> Projects { get; } = new()
    {
        new("Angor Hub", "Decentralized project funding platform", 
            new Uri("https://example.com/banner.jpg")),
        new("Bitcoin Wallet", "Simple and secure Bitcoin wallet",
            new Uri("https://example.com/banner2.jpg")),
    };
}

public record ProjectStub(string Name, string Description, Uri? Banner);
```

## Utility classes available

From `V2/Styles/Utilities.axaml`:

- **Layout**: `flex`, `flex-col`, `items-center`, `items-start`, `justify-between`, `justify-center`
- **Gap**: `gap-1` (4), `gap-2` (8), `gap-3` (12), `gap-4` (16), `gap-6` (24), `gap-8` (32)
- **Padding**: `p-2` (8), `p-4` (16), `p-6` (24), `p-8` (32)
- **Margin**: `m-2` (8), `m-4` (16), `mt-4`, `mb-4`, `mx-4`, `my-4`
- **Typography**: `text-sm`, `text-base`, `text-lg`, `text-xl`
- **Font weight**: `font-medium`, `font-bold`, `font-semibold`
- **Text color**: `text-muted`, `text-strong`
- **Border**: `rounded`, `rounded-lg`, `rounded-xl`

## Rules

- All spacing must be multiples of 4
- Use `DynamicResource` for theme-aware colors
- Use utility classes before inline styles
- Use `FlexPanel` over `StackPanel` for complex flex layouts
- Section padding is always 24 (via ScrollableView or PageContainer)
- No SDK dependencies — sample data only
- Match the Vue prototype as closely as possible
