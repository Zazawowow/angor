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

### 6. Build & Run
```bash
cd src/Angor/Avalonia
dotnet build Avalonia2.slnx
dotnet run --project Avalonia2.Desktop
```
