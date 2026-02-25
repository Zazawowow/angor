---
name: avalonia-new-control
description: Create a new shared Avalonia2 control (TemplatedControl or UserControl) with proper namespace, style registration, and design preview
---

## What I do

Create reusable UI controls in `src/Angor/Avalonia/Avalonia2/UI/Shared/Controls/` following Avalonia2 conventions:
- TemplatedControl (lookless, styled via Theme.axaml) for generic controls
- UserControl for composite controls with fixed layout
- Proper namespace (`Avalonia2.UI.Shared.Controls`)
- Design preview in the `.axaml` file
- Registration in `Theme.axaml` if it's a TemplatedControl with styles

## When to use me

Use this skill when:
- Creating a new reusable control (Badge, Card variant, custom input, etc.)
- Need to decide between TemplatedControl vs UserControl
- Need the proper file structure and registration steps

## Decision: TemplatedControl vs UserControl

| Use TemplatedControl when... | Use UserControl when... |
|------------------------------|------------------------|
| Control is lookless (restyled via themes) | Control has a fixed visual structure |
| Has StyledProperties for binding | Has a specific DataContext/ViewModel |
| Needs a ControlTemplate | Is a composed view of other controls |
| Examples: Badge, ProjectCard, PageContainer | Examples: SuccessView, CopyButton |

## TemplatedControl template

### .axaml (style definition)
```xml
<Styles xmlns="https://github.com/avaloniaui"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:controls="clr-namespace:Avalonia2.UI.Shared.Controls">

  <Design.PreviewWith>
    <controls:MyControl Width="300" SomeProperty="Preview value" />
  </Design.PreviewWith>

  <Style Selector="controls|MyControl">
    <Setter Property="Template">
      <ControlTemplate TargetType="controls:MyControl">
        <Border Background="{DynamicResource Surface}" CornerRadius="12" Padding="16">
          <ContentPresenter Content="{TemplateBinding SomeProperty}" />
        </Border>
      </ControlTemplate>
    </Setter>
  </Style>
</Styles>
```

### .axaml.cs (code-behind)
```csharp
using Avalonia;
using Avalonia.Controls.Primitives;

namespace Avalonia2.UI.Shared.Controls;

public class MyControl : TemplatedControl
{
    public static readonly StyledProperty<string?> SomePropertyProperty =
        AvaloniaProperty.Register<MyControl, string?>(nameof(SomeProperty));

    public string? SomeProperty
    {
        get => GetValue(SomePropertyProperty);
        set => SetValue(SomePropertyProperty, value);
    }
}
```

### Register in Theme.axaml
Add a `<StyleInclude>` line in `UI/Themes/V2/Theme.axaml`:
```xml
<StyleInclude Source="/UI/Shared/Controls/MyControl.axaml" />
```

## UserControl template

### .axaml
```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             mc:Ignorable="d"
             x:Class="Avalonia2.UI.Shared.Controls.MyControl">
    <Border Background="{DynamicResource Surface}" CornerRadius="12" Padding="16">
        <!-- content -->
    </Border>
</UserControl>
```

### .axaml.cs
```csharp
using Avalonia.Controls;

namespace Avalonia2.UI.Shared.Controls;

public partial class MyControl : UserControl
{
    public MyControl()
    {
        InitializeComponent();
    }
}
```

## Styling conventions

- Use `DynamicResource` for all theme-aware colors (`Surface`, `TextMuted`, `Brand`, etc.)
- Use design tokens for spacing: `Spacing1`–`Spacing8`, `Thickness4`–`Thickness32`
- All spacing multiples of 4 (4pt grid)
- CornerRadius values: 6 (sm), 8, 12 (md), 16 (lg)
- Use utility classes where possible: `Classes="flex gap-4 items-center"`
- Use `FlexPanel` from `Avalonia.Labs.Panels` for flexible layouts

## File locations

- Shared controls: `Avalonia2/UI/Shared/Controls/`
- Sub-grouped controls: `Avalonia2/UI/Shared/Controls/Common/`
- Section-specific controls: `Avalonia2/UI/Sections/<SectionName>/`

## Existing shared controls (reference)

| Control | Type | File |
|---------|------|------|
| PageContainer | TemplatedControl | `Shared/Controls/PageContainer.axaml` |
| ScrollableView | TemplatedControl | `Shared/Controls/ScrollableView.axaml` |
| Badge | TemplatedControl | `Shared/Controls/Badge.axaml` |
| Header | TemplatedControl | `Shared/Controls/Header.axaml` |
| ProjectCard | TemplatedControl | `Shared/Controls/ProjectCard.axaml` |
| List | TemplatedControl | `Shared/Controls/List.axaml` |
| IconLabel | TemplatedControl | `Shared/Controls/IconLabel.axaml` |
| Pane | TemplatedControl | `Shared/Controls/Pane.axaml` |
| NewCard | TemplatedControl | `Shared/Controls/NewCard.axaml` |
| SectionItem | TemplatedControl | `Shared/Controls/SectionItem.axaml` |
| ErrorSummary | TemplatedControl | `Shared/Controls/ErrorSummary.axaml` |
| CopyButton | UserControl | `Shared/Controls/Common/CopyButton.axaml` |
| SuccessView | UserControl | `Shared/Controls/Common/Success/SuccessView.axaml` |

## Rule: No SDK dependencies

Controls must not reference `Angor.Sdk`, `Angor.Data`, or `AngorApp.Model`. Use sample data for design previews.
