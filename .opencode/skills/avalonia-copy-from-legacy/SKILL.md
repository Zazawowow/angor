---
name: avalonia-copy-from-legacy
description: Safely copy controls, views, or styles from the legacy AngorApp project to Avalonia2, stripping all SDK dependencies
---

## What I do

Copy files from `src/Angor/Avalonia/AngorApp/` to `src/Angor/Avalonia/Avalonia2/`, then clean them:
1. Identify SDK dependencies in the source files
2. Copy files to the correct Avalonia2 location
3. Strip all `Angor.Sdk`, `Angor.Data`, `AngorApp.Model` references
4. Rename namespaces from `AngorApp.*` to `Avalonia2.*`
5. Remove or stub SDK-dependent code
6. Register in Theme.axaml if needed
7. Build to verify

## When to use me

Use this skill when:
- Copying a control, view, or style from the existing AngorApp
- Need to assess whether a file is safe to copy (clean vs SDK-dependent)
- Migrating theme files, shared controls, or section views

## Source and target paths

| Source (AngorApp) | Target (Avalonia2) |
|-------------------|-------------------|
| `AngorApp/UI/Themes/V2/` | `Avalonia2/UI/Themes/V2/` |
| `AngorApp/UI/Shared/Controls/` | `Avalonia2/UI/Shared/Controls/` |
| `AngorApp/UI/Shared/Styles/` | `Avalonia2/UI/Shared/Styles/` |
| `AngorApp/UI/Shared/Resources/` | `Avalonia2/UI/Shared/Resources/` |
| `AngorApp/UI/Sections/<Name>/` | `Avalonia2/UI/Sections/<Name>/` |
| `AngorApp/Assets/` | `Avalonia2/Assets/` |

## Pre-copy assessment

Before copying, check the file for SDK dependencies. Search for these patterns:

### Banned references (file is NOT clean if it contains any):

```
Angor.Sdk
Angor.Data
AngorApp.Model
UIServices
IAmountUI
IBlossomService
IWallet
ISection
IHasWallet
INetworkConfiguration
ISendTransactionService
```

### Common SDK-dependent files (DO NOT copy):

| File | Why |
|------|-----|
| `AmountControl.axaml` + `.cs` | References `IAmountUI`, `AngorApp.Model` |
| `AngorConverters.cs` | References `Angor.Data`, `AngorApp.Model` |
| `DesignTimePreset.cs` | References `AngorApp.Model`, `IWallet` |
| `ImagePicker/` (entire dir) | References `IBlossomService`, `UIServices` |
| `Feerate/FeerateSelector.axaml` | References `FeerateSelectionViewModel`, SDK types |
| `Password/` (entire dir) | References `AngorApp.Model` |
| `Controller.cs` | References `AngorApp.Model` |
| `FeerateSelection*.cs` | References SDK types |
| `QRGenerator.cs` | References `AngorApp.Model` |

### Clean files (safe to copy, only need namespace rename):

All pure XAML files (no code-behind SDK refs):
- Theme files (`UI/Themes/V2/Resources/*.axaml`, `UI/Themes/V2/Styles/*.axaml`, `UI/Themes/V2/Controls/*.axaml`)
- Shared styles (`UI/Shared/Styles/*.axaml`)
- Simple controls: `Badge`, `Header`, `PageContainer`, `ScrollableView`, `List`, `IconLabel`, `Pane`, `NewCard`, `SectionItem`, `ErrorSummary`, `ProjectCard`, `CopyButton`, `SuccessView`

## Copy procedure

1. **Assess**: Read the source file(s) and check for banned references
2. **Copy**: Copy to the target location
3. **Namespace rename**: 
   - `.cs`: `namespace AngorApp.*` → `namespace Avalonia2.*`
   - `.axaml`: `clr-namespace:AngorApp.*` → `clr-namespace:Avalonia2.*`
   - `.axaml`: `using:AngorApp.*` → `using:Avalonia2.*`
   - `.axaml`: `x:Class="AngorApp.*"` → `x:Class="Avalonia2.*"`
4. **Strip SDK deps**: Remove any remaining SDK references:
   - Delete `using Angor.*` statements
   - Replace SDK types with stubs or remove
   - Remove SDK-dependent properties/methods
5. **Register**: Add `<StyleInclude>` to Theme.axaml if it's a styled control
6. **Build**: `dotnet build src/Angor/Avalonia/Avalonia2.slnx`

## Handling partially-clean files

Some files are mostly clean but have small SDK dependencies:

### Pattern: Remove SDK-only xmlns
```xml
<!-- Before (has SDK namespace) -->
<UserControl xmlns:model="clr-namespace:AngorApp.Model;assembly=AngorApp.Model"
             xmlns:controls="clr-namespace:AngorApp.UI.Shared.Controls">

<!-- After (SDK namespace removed, controls namespace updated) -->
<UserControl xmlns:controls="clr-namespace:Avalonia2.UI.Shared.Controls">
```

### Pattern: Replace SDK converter with inline value
```xml
<!-- Before -->
<ContentPresenter Padding="{TemplateBinding Padding, Converter={x:Static controls:AngorConverters.ThicknessWithoutTop}}" />

<!-- After — use explicit padding -->
<ContentPresenter Padding="{TemplateBinding Padding}" />
```

### Pattern: Replace SDK design-time data
```xml
<!-- Before -->
<d:DataType="model:IProject" />

<!-- After — use local stub type or remove -->
<d:DataType="local:ProjectStub" />
```

## Files already copied to Avalonia2

These have already been copied and should not be re-copied:
- All V2 Theme files (33 files)
- All Shared/Styles (13 files)
- All Shared/Resources (1 file)
- All Assets (24 files)
- Clean Shared/Controls (PageContainer, ScrollableView, Badge, Header, etc.)
- Shell files (created as stubs, not copied)

## Known issues with copied files

These files were copied but still need fixes:
- All `.cs` files still have `AngorApp.*` namespaces (17 files)
- All `.axaml` files still have `AngorApp.*` namespace references (16 files)
- `ProjectCard.axaml` references non-existent `AngorApp.UI.Sections.Founder.*` namespaces
- `Expander.axaml` references deleted `AngorConverters.ThicknessWithoutTop`
- `DialogControl.axaml` needs `Zafiro.Avalonia.Dialogs` package

## Rule: Never copy SDK-dependent code

If a file requires `Angor.Sdk`, `Angor.Data`, or `AngorApp.Model` to compile, either:
1. Don't copy it — create a clean stub in Avalonia2 instead
2. Copy it and fully remove/stub the SDK-dependent parts
Never add SDK project references to Avalonia2.csproj.
