---
name: avalonia-fix-build
description: Diagnose and fix Avalonia2 build errors — missing packages, broken XAML references, deleted file includes, namespace mismatches, and csproj issues
---

## What I do

Fix build errors in the `src/Angor/Avalonia/Avalonia2/` project. This includes:

- Running `dotnet build` on `Avalonia2.slnx` and parsing errors
- Fixing `Theme.axaml` when it references deleted files (AmountControl, FeerateSelector, etc.)
- Adding missing NuGet packages to `Avalonia2.csproj` (check `Directory.Packages.props` for versions)
- Removing or replacing references to deleted converters/controls
- Fixing namespace mismatches (`AngorApp.*` → `Avalonia2.*`)
- Resolving transitive dependency issues (e.g. `Zafiro.Avalonia` for `SmartDockPanel`)

## When to use me

Use this skill when:
- The Avalonia2 project fails to build
- You've copied or deleted files and need to update references
- Build errors mention missing types, namespaces, or XAML resources

## Project structure

```
src/Angor/Avalonia/
├── Directory.Packages.props          # Central NuGet versions (Avalonia 11.3.12)
├── Directory.Build.props             # Shared build props
├── Avalonia2.slnx                    # Solution file (XML format, .NET 9)
├── Avalonia2/
│   ├── Avalonia2.csproj              # NO SDK project refs — visual layer only
│   ├── App.axaml / App.axaml.cs
│   ├── GlobalUsings.cs
│   ├── UI/
│   │   ├── Themes/V2/Theme.axaml     # Master style include — references all controls/styles
│   │   ├── Themes/V2/Resources/      # Colors.axaml, Tokens.axaml, Icons.axaml
│   │   ├── Themes/V2/Styles/         # Expander, Dialog, Typography, etc.
│   │   ├── Themes/V2/Controls/       # EnhancedButton, Frame, Wizard, etc.
│   │   ├── Shared/Controls/          # Reusable controls (PageContainer, Badge, etc.)
│   │   ├── Shared/Styles/            # Shared styles (Cards, Buttons, TextBlock, etc.)
│   │   ├── Shared/Resources/         # Icons.axaml
│   │   ├── Shell/                    # MainWindow, ShellView, ShellViewModel
│   │   └── Sections/                 # Section views (Browse, Home, etc.)
│   └── Assets/                       # Images, fonts
└── Avalonia2.Desktop/
    ├── Avalonia2.Desktop.csproj
    └── Program.cs
```

## Build command

```bash
dotnet build src/Angor/Avalonia/Avalonia2.slnx
```

## Common fixes

### 1. Theme.axaml references a deleted file
Remove the `<StyleInclude>` line from `UI/Themes/V2/Theme.axaml`.

### 2. Missing NuGet package
1. Check `Directory.Packages.props` — if the package exists there, just add `<PackageReference Include="PackageName"/>` to `Avalonia2.csproj` (no version needed, central management).
2. If not in `Directory.Packages.props`, add it there first with version, then to csproj.

### 3. Namespace mismatch (AngorApp → Avalonia2)
- In `.cs` files: `namespace AngorApp.UI.*` → `namespace Avalonia2.UI.*`
- In `.axaml` files: `clr-namespace:AngorApp.UI.*` → `clr-namespace:Avalonia2.UI.*`
- In `.axaml` files: `using:AngorApp.UI.*` → `using:Avalonia2.UI.*`
- In `.axaml` files: `x:Class="AngorApp.*"` → `x:Class="Avalonia2.*"`

### 4. Missing converter (e.g. AngorConverters.ThicknessWithoutTop)
Replace with inline padding values or create a minimal local converter in Avalonia2.

### 5. Transitive dependency not resolving
Add explicit `<PackageReference>` for the package (e.g. `Zafiro.Avalonia` for `SmartDockPanel`, `DialogControl`, etc.).

## Key packages available in Directory.Packages.props

| Package | Version | Provides |
|---------|---------|----------|
| Zafiro.Avalonia | 49.1.4 | SmartDockPanel, EdgePanel, SectionStrip |
| Zafiro.Avalonia.Dialogs | 49.1.4 | DialogControl, DialogViewContainer, IOption |
| AsyncImageLoader.Avalonia | 3.5.0 | AdvancedImage (lazy image loading) |
| Avalonia.Labs.Panels | 11.3.1 | FlexPanel |
| ReactiveUI.SourceGenerators | 2.5.1 | [Reactive] attribute |

## Rule: No SDK dependencies

Avalonia2 must NOT reference:
- `Angor.Sdk`
- `Angor.Data`
- `AngorApp.Model`
- Any project under `src/Angor/Shared/`

If a build error requires one of these, the offending file should be removed or stubbed.
