---
name: avalonia-new-section
description: Scaffold a new Avalonia2 section with view, viewmodel, and shell registration following project conventions
---

## What I do

Create a new section in `src/Angor/Avalonia/Avalonia2/UI/Sections/<SectionName>/` with:
1. A `<SectionName>SectionView.axaml` — the section's main view
2. A `<SectionName>SectionView.axaml.cs` — code-behind
3. A `<SectionName>SectionViewModel.cs` — viewmodel with sample/stub data
4. Registration in `ShellViewModel.cs` so the section appears in navigation

## When to use me

Use this skill when:
- Creating a new section stub (e.g. Home, Browse, Settings, Portfolio)
- Adding a section to the navigation sidebar
- Need the standard section file structure

## Section view template (.axaml)

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:controls="clr-namespace:Avalonia2.UI.Shared.Controls"
             xmlns:local="clr-namespace:Avalonia2.UI.Sections.SECTIONNAME"
             mc:Ignorable="d" d:DesignWidth="800" d:DesignHeight="600"
             x:Class="Avalonia2.UI.Sections.SECTIONNAME.SECTIONNAMESectionView"
             x:DataType="local:SECTIONNAMESectionViewModel">

    <!-- Use ScrollableView for scrollable sections, PageContainer for non-scrollable -->
    <controls:ScrollableView ContentPadding="24">
        <StackPanel Spacing="24">
            <TextBlock Classes="Size-XL Weight-Bold" Text="SECTIONNAME" />
            <TextBlock Classes="Text-Muted" Text="Section content goes here" />
        </StackPanel>
    </controls:ScrollableView>
</UserControl>
```

## Section view code-behind template (.axaml.cs)

```csharp
using Avalonia.ReactiveUI;

namespace Avalonia2.UI.Sections.SECTIONNAME;

public partial class SECTIONNAMESectionView : ReactiveUserControl<SECTIONNAMESectionViewModel>
{
    public SECTIONNAMESectionView()
    {
        InitializeComponent();
    }
}
```

## Section viewmodel template (.cs)

```csharp
namespace Avalonia2.UI.Sections.SECTIONNAME;

public partial class SECTIONNAMESectionViewModel : ReactiveObject
{
    // Add [Reactive] properties for sample data
    // Example:
    // [Reactive] private string title = "SECTIONNAME";
}
```

## Shell registration

In `UI/Shell/ShellViewModel.cs`:

1. The `SectionNames` collection lists nav items:
```csharp
SectionNames = new ObservableCollection<string>
{
    "Home",
    "Funds",
    "Find Projects",
    "Portfolio",
    "My Projects",
    "Funders",
    "Settings",
};
```

2. The `CurrentSectionContent` property returns the active view. Update it to instantiate the real section view:
```csharp
public object? CurrentSectionContent => SelectedSectionName switch
{
    "Home" => new HomeSectionView { DataContext = new HomeSectionViewModel() },
    "Settings" => new SettingsSectionView { DataContext = new SettingsSectionViewModel() },
    // ... other sections
    _ => new TextBlock { Text = $"{SelectedSectionName} — coming soon" },
};
```

## Layout conventions

- **Scrollable sections** (Home, Find Projects, wizard steps): Use `<controls:ScrollableView ContentPadding="24">`
- **Non-scrollable sections** (My Projects, Portfolio): Use `<controls:PageContainer>`
- **Section content padding**: Always 24px (from `SectionPadding` token)
- **All spacing multiples of 4** (4pt grid system)
- **Use DynamicResource** for theme-aware colors
- **Use utility classes** before inline styles: `Classes="flex gap-4 items-center"`

## Known sections

| Name | Nav Label | Notes |
|------|-----------|-------|
| Home | Home | Two-column invest/launch layout |
| Funds | Funds | Wallet funds overview |
| FindProjects | Find Projects | Browse/search projects |
| Portfolio | Portfolio | User's invested projects |
| MyProjects | My Projects | User's created projects |
| Funders | Funders | People funding your projects |
| Settings | Settings | App settings |
| Browse | Browse | Alternative project browser |
| Wallet | Wallet | Wallet management (create/import) |
| Founder | Founder | Project creation flow |

## File location

All section files go in:
```
src/Angor/Avalonia/Avalonia2/UI/Sections/<SectionName>/
```

## Rule: No SDK dependencies

Section viewmodels use sample/stub data only. No references to `Angor.Sdk`, `Angor.Data`, or `AngorApp.Model`. The backend team will later replace stubs with real data bindings.
