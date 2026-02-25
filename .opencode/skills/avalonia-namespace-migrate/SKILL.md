---
name: avalonia-namespace-migrate
description: Bulk-rename AngorApp namespaces to Avalonia2 across .cs and .axaml files in the Avalonia2 project
---

## What I do

Migrate all `AngorApp.*` namespace references to `Avalonia2.*` in files under `src/Angor/Avalonia/Avalonia2/`. This is needed because controls/views were copied from the legacy AngorApp project and still carry old namespaces.

## When to use me

Use this skill when:
- Files copied from AngorApp need namespace updates
- Build errors show `AngorApp.*` type not found
- XAML files reference `clr-namespace:AngorApp.*` or `using:AngorApp.*`

## Migration rules

### C# files (.cs)

| Pattern | Replace with |
|---------|-------------|
| `namespace AngorApp.UI.Shared.Controls` | `namespace Avalonia2.UI.Shared.Controls` |
| `namespace AngorApp.UI.Shared.Controls.Common` | `namespace Avalonia2.UI.Shared.Controls.Common` |
| `namespace AngorApp.UI.Shared.Controls.Common.Success` | `namespace Avalonia2.UI.Shared.Controls.Common.Success` |
| `namespace AngorApp.UI.Sections.*` | `namespace Avalonia2.UI.Sections.*` |
| `namespace AngorApp.UI.Shell` | `namespace Avalonia2.UI.Shell` |
| `namespace AngorApp` | `namespace Avalonia2` |
| `using AngorApp.*` | `using Avalonia2.*` |

General rule: Replace `AngorApp` with `Avalonia2` in all namespace declarations and using statements.

### AXAML files (.axaml)

| Pattern | Replace with |
|---------|-------------|
| `clr-namespace:AngorApp.UI.Shared.Controls` | `clr-namespace:Avalonia2.UI.Shared.Controls` |
| `clr-namespace:AngorApp.UI.Sections.*` | `clr-namespace:Avalonia2.UI.Sections.*` |
| `clr-namespace:AngorApp.UI.Shell` | `clr-namespace:Avalonia2.UI.Shell` |
| `clr-namespace:AngorApp` | `clr-namespace:Avalonia2` |
| `using:AngorApp.UI.*` | `using:Avalonia2.UI.*` |
| `x:Class="AngorApp.*"` | `x:Class="Avalonia2.*"` |

### Remove references to non-existent AngorApp sections

Some AXAML files reference AngorApp section namespaces that don't exist in Avalonia2 (e.g. `AngorApp.UI.Sections.Founder.ProjectDetails`). These xmlns declarations should be removed entirely, along with any elements that use them. If a namespace is only used in `Design.PreviewWith`, remove the preview too.

## Procedure

1. **Search** for all `AngorApp` occurrences in `src/Angor/Avalonia/Avalonia2/`:
   ```bash
   grep -rn "AngorApp" src/Angor/Avalonia/Avalonia2/ --include="*.cs" --include="*.axaml"
   ```

2. **For each .cs file**: Replace the namespace declaration. Usually it's the only occurrence.

3. **For each .axaml file**: 
   - Update `xmlns:*` declarations
   - Update `x:Class` attributes
   - Remove xmlns declarations that reference non-existent AngorApp paths
   - Remove elements that depend on removed xmlns prefixes

4. **Build** to verify: `dotnet build src/Angor/Avalonia/Avalonia2.slnx`

## Known files needing migration (as of last check)

### .cs files (17 files)
All controls under `UI/Shared/Controls/` still use `AngorApp.*` namespaces.

### .axaml files (16 files)
All controls under `UI/Shared/Controls/` plus:
- `UI/Shared/Styles/Automation.axaml` — `xmlns:f="clr-namespace:AngorApp.UI.Shared.Controls"`
- `UI/Themes/V2/Styles/Expander.axaml` — `xmlns:controls="using:AngorApp.UI.Shared.Controls"`
- `UI/Themes/V2/Controls/ErrorSummary.axaml` — `xmlns:controls="clr-namespace:AngorApp.UI.Shared.Controls"`

### Special case: ProjectCard.axaml
Has xmlns for `AngorApp.UI.Sections.Founder.ProjectDetails.MainView` and `AngorApp.UI.Sections.Founder.ProjectDetails` — these sections don't exist in Avalonia2. Remove these xmlns declarations and simplify the Design.PreviewWith.

## Root namespace

The `Avalonia2.csproj` has `<RootNamespace>Avalonia2</RootNamespace>`, so:
- `UI/Shared/Controls/Badge.axaml.cs` → `namespace Avalonia2.UI.Shared.Controls;`
- `UI/Shell/ShellView.axaml.cs` → `namespace Avalonia2.UI.Shell;`
