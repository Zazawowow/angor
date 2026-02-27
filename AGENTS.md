# Angor Avalonia2 Desktop App

## Project Overview
Angor is a Bitcoin investment platform. The Avalonia2 project is a standalone desktop UI prototype built with Avalonia UI, ReactiveUI, and the Zafiro design system. It currently contains **visual-only** implementations (all data is hardcoded/stubbed) — the backend team will wire SDK connections later.

## Source of Truth: Vue Prototype
**CRITICAL**: The Vue.js prototype at `/Users/dorian/Projects/angor-prototype/angor-prototype/` is the single source of truth for all UI design, layout, styling, colors, spacing, animations, responsiveness, and user flows.

**Before implementing or fixing ANY UI feature, ALWAYS:**
1. Read the corresponding Vue component(s) from the prototype
2. Extract exact CSS values (colors, spacing, shadows, gradients, border-radius, font sizes)
3. Note any animations/transitions (hover effects, modal fades, card transforms)
4. Check both light and dark mode styles
5. Reproduce the layout and visual hierarchy pixel-perfectly in Avalonia XAML

### Prototype Structure
```
/Users/dorian/Projects/angor-prototype/angor-prototype/
  src/
    components/
      atoms/          - Badge.vue, Button.vue, Input.vue, ProgressBar.vue, Spinner.vue
      molecules/      - ProjectCard.vue, PortfolioStats.stories.js, FilterPills.stories.js
      organisms/      - Header.stories.js, Sidebar.stories.js
      pages/          - *.stories.js (Storybook page stories)
      templates/      - DashboardLayout.stories.js
      Funds.vue       - Funds page (empty + populated states, wallet groups, modals)
      ManageFunds.vue - Project fund management detail page
      InvestPage.vue  - Investment flow page
      InvestmentDetail.vue - Investment detail page
      HubInvestPage.vue    - Hub variant of invest page
      HubInvestmentDetail.vue - Hub variant of investment detail
      HubProjectDetail.vue   - Hub project detail page
      HubProjectProfile.vue  - Hub project profile
      Settings.vue    - Settings page
      ProfileWelcome.vue - Welcome screen
    style.css         - Global CSS styles
    App.vue           - Main app shell
  tailwind.config.cjs - Tailwind theme tokens
  styles/             - Additional global styles
```

### Key Design Tokens (from prototype)
- **Brand green**: `#4B7C5A` (primary), `#3d6448` (hover/darker), `#2d5a3d` (gradient start)
- **Bitcoin orange**: `#F7931A`
- **Dark mode bg**: `#111` (surface), `#2A2A2A` (card), `#404040` (border), `#0A0A0A` (input bg)
- **Light mode bg**: `#ffffff` (card), `#f9fafb` (surface low), `#e5e7eb` (border)
- **Text light**: `#1a1a1a` (strong), `#666666` (muted), `#4a4a4a` (stat labels)
- **Text dark**: `#fafafa` (strong), `#a0a0a0` (muted), `#d0d0d0` (pills)
- **Card shadow**: `0 2px 8px rgba(0,0,0,0.1)` normal, `0 4px 16px rgba(0,0,0,0.15)` hover
- **Card border-radius**: `16px` (ProjectCard), `12px` (general cards), `8px` (buttons)
- **Card hover**: `translateY(-2px)` + enhanced shadow
- **Modal transition**: `modal-fade` (opacity 0→1 over 0.3s)
- **Progress bar fill**: `linear-gradient(90deg, #2d5a3d 0%, #4a8f5f 100%)`

## Avalonia2 Project Structure
```
src/Angor/Avalonia/Avalonia2/
  UI/
    Shell/          - MainWindow, ShellView (sidebar+content), ShellViewModel (navigation)
    Sections/       - Home, FindProjects, MyProjects, Funders, Funds, Portfolio, Settings
    Shared/
      Controls/     - ProjectCard, EmptyState, StatCard, WalletCard, ResponsiveGrid, etc.
      Styles/       - Shared .axaml style files
      Resources/    - Icons.axaml
    Themes/V2/
      Theme.axaml   - Master theme (includes FluentTheme + all custom styles)
      Resources/    - Colors.axaml, Tokens.axaml, Icons.axaml
      Styles/       - 22+ style .axaml files (Typography, Buttons, Pills, Layout, etc.)
      Controls/     - 7 control .axaml files (Wizard, CalendarPicker, etc.)
```

## Development Rules

### 1. Always Reference the Prototype
- Do NOT guess colors, spacing, or layouts — always read the Vue source first
- When a style value exists in the prototype, use it exactly
- Use `DynamicResource` for theme-aware values; define new resources in Colors.axaml/Tokens.axaml if needed

### 2. Code Style
- Use `[Reactive]` source generators for reactive properties (NOT manual RaisePropertyChanged)
- ViewModels inherit from `ReactiveObject`
- Keep Views (XAML + code-behind) in the same folder as their ViewModel
- NO SDK dependencies — all data is stubbed/hardcoded for now
- Comment Vue CSS references in XAML where non-obvious

### 3. Naming Conventions
- Sections: `{Name}View.axaml` + `{Name}ViewModel.cs`
- Shared controls: `{ControlName}.axaml` + `{ControlName}.axaml.cs`
- Theme styles: `{StyleCategory}.axaml` inside `Themes/V2/Styles/`
- Resources: Use `DynamicResource` with descriptive keys like `Surface`, `TextStrong`, `Stroke`, `Brand`

### 4. Dark/Light Theme Support
- ALL visual elements must support both themes via `DynamicResource`
- Colors defined as `ThemeVariant` dictionaries (Light/Dark) in Colors.axaml
- Never hardcode colors that differ between themes — use resources
- Exception: brand green `#4B7C5A` is the same in both themes

### 5. Animations in Avalonia
- Use `Avalonia.Animation` for transitions (Setter animations, Transitions property)
- Card hover: `RenderTransform` ScaleTransform or TranslateTransform with Transitions
- Modal fade: Opacity transition on overlay panels
- Progress bars: Width animation with easing
- **NEVER use `BrushTransition` on selection-toggled elements** — see Rule 9

### 6. Uniform 24px Section Spacing
All section-level spacing MUST use **24px** uniformly. This includes:
- **Page padding**: `ScrollableView ContentPadding="24"` (or `Margin="24"` on the outermost Grid for two-column layouts)
- **Top margin**: 24px from the top of the section to the first content element
- **Nav-to-content gap**: 24px between any fixed nav bar and the scrollable content below it
- **Between content cards**: `Spacing="24"` on the main vertical StackPanel (the gap between major card sections stacked vertically)
- **Side margins**: 24px left and right
- **Bottom margin**: 24px at the bottom of the scroll content
- **Grid column gap** (two-column layouts): 24px between the left panel and the right content column

**Exceptions** (internal spacing within cards, not section-level):
- Stat card grids use `gap: 16px` (achieved via 8px margin on each side of inner items)
- Card internal padding is `24px` (detail cards) or `20px` (stat cards) — per Vue prototype
- Inner element spacing (icon-to-text, label-to-value) follows the Vue prototype exactly

**For sticky nav bar views** (e.g., InvestmentDetailView): the nav bar uses `VerticalAlignment="Top"` + `ZIndex="100"`, and the scroll content has a spacer Panel whose height = nav top margin (24) + nav height (44) + gap below nav (24) = **92px**.

### 7. Build & Run
```bash
cd src/Angor/Avalonia
dotnet build Avalonia2.slnx
dotnet run --project Avalonia2.Desktop
```

### 8. Mandatory Dark/Light Mode State Verification for Interactive Elements
Every interactive element (buttons, cards, list items, tabs, plan selectors, wallet cards) MUST have ALL visual states explicitly defined and verified in BOTH light AND dark themes. Before implementing any clickable/selectable element:

1. **Read the Vue prototype** to extract exact CSS for ALL states in BOTH themes:
   - Default/inactive state (light + dark)
   - Hover state (light + dark)
   - Selected/active state (light + dark)
   - Disabled state (light + dark, if applicable)

2. **Define `SolidColorBrush` resources** for each state in Colors.axaml with BOTH light and dark variants — never use hardcoded colors that differ between themes.

3. **Prefer `DynamicResource` bindings in XAML over code-behind color assignment.** When code-behind explicitly sets `Background` or `BorderBrush`, it permanently overrides the XAML `{DynamicResource}` binding. For unselected/default states, use `ClearValue(Border.BackgroundProperty)` to restore the XAML binding instead of setting a new brush.

4. **Never use `new SolidColorBrush(Color.Parse(...))` for theme-dependent values** in code-behind — always use `FindResource("ResourceKey") as IBrush` and verify it returns non-null. If the resource is a `Color` (not a `SolidColorBrush`), the `as IBrush` cast returns `null`.

5. **Test both themes** before considering any interactive element complete.

### 9. NEVER Use BrushTransition on Selection-Toggled Elements
`BrushTransition` (on `Background`, `BorderBrush`, `Foreground`, `Fill`, `Stroke`) causes a **stuck-tint bug** when combined with CSS class toggling and `DynamicResource`-backed brushes. The animation gets stuck at intermediate interpolated values when transitioning selected→unselected, leaving a visible residual tint on elements that should be fully neutral.

**The rule:** Any element whose visual state changes via `Classes.Set(...)` in code-behind (selection toggles, active tab switches, stepper state changes) must have **instant** state changes — no `BrushTransition`.

**Where BrushTransition IS safe:**
- `:pointerover` hover effects (hover-only, no selection toggling)
- `:focus` / `:pressed` pseudo-class transitions
- `TransformOperationsTransition` (RenderTransform), `BoxShadowsTransition`, `DoubleTransition` (Opacity, Width) — these are always safe
- Avalonia built-in `:selected` pseudo-class on `ListBoxItem` with opaque same-type brushes (use with caution)

**Where BrushTransition is BANNED:**
- Any `Border`, `TextBlock`, `Path`, or other element that uses `Classes.Set("ClassName", bool)` to toggle between selected/unselected states
- Any style selector involving a custom CSS class modifier (e.g., `.WalletSelected`, `.SubPlanSelected`, `.TypeCardSelected`, `.FilterTabActive`, `.StepCurrent`, `.StepCompleted`)
- Any element where both endpoints are `DynamicResource` brushes that resolve differently per theme

**The correct pattern for selection-toggled elements:**
```xml
<!-- XAML: Define both states with DynamicResource, NO Transitions -->
<Style Selector="Border.WalletCard">
    <Setter Property="Background" Value="{DynamicResource WalletCardBg}" />
    <Setter Property="BorderBrush" Value="{DynamicResource WalletCardBorder}" />
</Style>
<Style Selector="Border.WalletCard.WalletSelected">
    <Setter Property="Background" Value="{DynamicResource WalletSelectedBg}" />
    <Setter Property="BorderBrush" Value="{DynamicResource WalletSelectedBorder}" />
</Style>
```
```csharp
// Code-behind: ONLY toggle CSS classes — zero color logic
border.Classes.Set("WalletSelected", isSelected);
```

**Anti-patterns to avoid:**
- `ClearValue()` — causes a flash frame where the default value is briefly visible
- `this.FindResource()` in code-behind — returns wrong theme values in modal contexts
- `new SolidColorBrush(Color.Parse(...))` — permanently overrides `DynamicResource` bindings
- `Application.Current?.ActualThemeVariant == ThemeVariant.Dark` — doesn't react to live theme changes
- Mixing `LinearGradientBrush` and `SolidColorBrush` for the same property's default/selected states — `BrushTransition` cannot interpolate between different brush types
