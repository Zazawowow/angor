---
name: avalonia-create-project-wizard
description: Continue building the Create New Project wizard in Avalonia2 — a 6-step form matching both AngorApp and Vue prototype references pixel-perfectly
---

## What I do

Continue implementing and refining the **Create New Project wizard** in the Avalonia2 desktop app. This wizard has 6 steps (Project Type, Project Profile, Project Images, Funding Configuration, Stages/Payouts, Review & Deploy) and must pixel-perfectly match two reference implementations.

## When to use me

Use this skill when:
- Working on any step of the Create Project wizard
- Fixing visual bugs in wizard steps (hover states, spacing, colors, layout)
- Adding missing wizard features (interstitial screens, advanced editors, etc.)
- Matching wizard UI to the Vue or AngorApp references

---

## Critical rules

1. **Always consult BOTH references** before making changes — the Vue prototype AND the AngorApp legacy Avalonia code
2. **Read the Vue source code first** from the local `angor-prototype` project — do NOT guess layouts or styles
3. **Pixel-perfect execution** — match references exactly for layout, spacing, colors, typography, and interaction states
4. **No SDK dependencies** — the ViewModel is visual-only with stub data
5. **Fix ALL project types** when fixing issues (Investment, Fund, Subscription) — never fix just one
6. **No bold page-level headings** in wizard steps — use only subtitle text at top
7. **No tiled patterns** on the stepper panel or globally in the shell content area (Step 3 banner is the exception)
8. **No hover effects** on type cards (Step 1) — neither selected nor unselected

---

## Reference sources

### Vue prototype (PRIMARY design reference)
- **Location**: `/Users/dorian/Projects/angor-prototype/angor-prototype/src/`
- **Wizard file**: `App.vue` (~12,275 lines — the entire wizard is in this single file)
- **Key sections in App.vue**: Search for `currentStep`, `totalSteps`, `projectForm`, `stage` to find relevant wizard code
- **Component files**: `src/components/atoms/` (Button, Input, Badge, etc.), `src/components/molecules/` (ProjectCard)
- **Global styles**: `src/style.css`
- **Web URL**: `angor.tx1138.com/app` (for visual verification — do NOT parse minified JS bundle)

### AngorApp legacy (secondary Avalonia reference)
- **Base path**: `/Users/dorian/Projects/angor/src/Angor/Avalonia/AngorApp/`
- **Wizard flows**: `UI/Flows/CreateProject/Wizard/`
  - `ProjectTypeView.axaml` — Step 1 type selection
  - `InvestmentProject/FundingConfigurationView.axaml` — Investment Step 4 (317 lines)
  - `FundProject/GoalView.axaml` — Fund Step 4 (155 lines)
  - `InvestmentProject/Stages/StagesView.axaml` — Stages container (27 lines)
  - `InvestmentProject/Stages/StagesSimpleEditor.axaml` — Simple stage editor (124 lines)
  - `InvestmentProject/Stages/StagesAdvancedEditor.axaml` — Manual per-stage editor
  - `InvestmentProject/Stages/IStagesViewModel.cs` — Stages interface (28 lines)

---

## Current implementation files

### Files being actively modified
| File | Lines | Purpose |
|------|-------|---------|
| `Avalonia2/UI/Sections/MyProjects/CreateProjectView.axaml` | ~1356 | Main wizard view — all 6 steps as `Panel IsVisible="{Binding IsStepN}"` blocks |
| `Avalonia2/UI/Sections/MyProjects/CreateProjectView.axaml.cs` | ~600 | Code-behind: type card styling, stepper, pattern/preset buttons, image picker |
| `Avalonia2/UI/Sections/MyProjects/CreateProjectViewModel.cs` | ~330 | ViewModel: stub fields, step navigation, `InvestStartDate`/`InvestEndDate` as `DateTimeOffset?` |

All paths relative to `/Users/dorian/Projects/angor/src/Angor/Avalonia/`

### Theme/resource files
| File | Key contents |
|------|-------------|
| `Avalonia2/UI/Themes/V2/Resources/Colors.axaml` | Accent=#2D5A3D, BitcoinAccent=#F7931A, SurfaceLow, SurfaceMedium, Surface, TextStrong, TextMuted, Stroke |
| `Avalonia2/UI/Themes/V2/Resources/Icons.axaml` | BitcoinSymbol (fill), NavIconTrendUp (stroke), RefreshArrows (stroke), Lightning, AngorLogo, CheckCircle, CloudUpload, Shield, Calendar, NavIconDocument |
| `Avalonia2/UI/Themes/V2/Resources/Tokens.axaml` | FontSizeSm=12, FontSizeBase=14, FontSizeLg=16, FontSizeXl=20 |
| `Avalonia2/UI/Shell/ShellView.axaml` | Shell layout (TiledPatternBrush overlay already removed) |

---

## Architecture notes

- The wizard is a **single monolithic view** — NOT split into separate UserControls per step
- Steps are panels toggled via `IsVisible="{Binding IsStepN}"` bindings
- The Avalonia2 app is **simplified** — it does NOT have: `HeaderedContainer`, `EnhancedButton`, `OverlayBorder`, `IImagePicker`, `ScrollableView`, `EdgePanel`, `ResourceKeyConverter`, `BalancedWrapGrid`, `ButtonizedListBoxItem`, `ButtonizedAccentListBoxItem`, or the Blossom upload service
- `TiledLogoBrush` is defined in Colors.axaml — used intentionally in Step 3 banner area and HomeView (2 places)
- There is **no Subscription-specific config** in the AngorApp reference — we designed it ourselves

---

## Step-by-step status

### Step 1 — Project Type (DONE)
- Three type cards: Investment, Fund, Subscription
- Selected card: accent 2px border, transparent bg, accent title, solid accent icon bg with white icon, accent radio with white checkmark
- Unselected card: SurfaceLow 2px border, Surface bg, SurfaceLow icon bg with colored icon, SurfaceLow radio (empty)
- CornerRadius=20, BorderThickness=2
- **Known issue**: hover states still showing — need to strip `:pointerover` from Button template or switch to clickable Border/Panel

### Step 2 — Project Profile (DONE)
- Container cards: "What's the project called?", "Tell us about the project", "What's the project website?"
- Card pattern: Border with CornerRadius=16, Stroke border, Surface bg, SurfaceMedium header with bold question, content padding 24

### Step 3 — Project Images (DONE)
- Unified profile card preview (NOT card-with-header pattern)
- Clickable banner area with TiledLogoBrush pattern + camera icon
- Avatar overlap with click-to-upload
- Name/description preview below
- File picker prototype working

### Step 4 — Funding Configuration (DONE)
- **Investment**: Bitcoin accent target amount card (TextBox + BTC suffix + 0.25/0.5/1/2.5 presets), Fundraising Window card (CalendarDatePicker start/end + 1/2/3 month presets), Penalty card (Slider 30-180 days)
- **Fund**: Bitcoin accent Goal card (same amount presets), Blue Threshold card (Shield icon, threshold BTC input)
- **Subscription**: Bitcoin accent Subscription Price card (Sats presets 1k/5k/10k/50k)

### Step 5 — Stages/Payouts (NEEDS REWRITE)
Current version has basic payout pattern + payout day + stages table. Needs to match Vue reference:
- **Missing interstitial screen**: centered card with clipboard/shield icon, "Set Funding Release Schedule" title, orange info box about staged funding, green "Ok, Let's go" button — shown BEFORE the form
- **Form should match Vue**: "How long will the project take?" card (TextBox + "Months" dropdown + 3/6/12/18/24 presets), "How would you like payments split?" card (Weekly/Monthly/Bi-Monthly/Quarterly buttons), green "Generate Fund Release Schedule" button, outlined "Advanced Editor" button
- After generating: stages table replaces the form

### Step 6 — Review & Deploy (DONE — basic)
- Container cards for Profile, Funding Config, Stages, Ready to deploy

---

## Card pattern reference

### Container card (Steps 2, 4, 5, 6)
```xml
<Border CornerRadius="16" BorderThickness="1"
        BorderBrush="{DynamicResource Stroke}"
        Background="{DynamicResource Surface}">
    <StackPanel>
        <!-- Header -->
        <Border Background="{DynamicResource SurfaceMedium}"
                CornerRadius="16,16,0,0" Padding="24,16">
            <TextBlock FontWeight="Bold" Text="Question text here" />
        </Border>
        <!-- Content -->
        <StackPanel Margin="24" Spacing="16">
            <!-- form fields -->
        </StackPanel>
    </StackPanel>
</Border>
```

### Type card (Step 1)
- CornerRadius=20, BorderThickness=2
- Selected: accent border, transparent bg, accent title, solid accent icon circle, accent radio with checkmark
- Unselected: SurfaceLow border, Surface bg, SurfaceLow icon circle, SurfaceLow radio (empty)
- Icon rendering: Investment=stroke, Fund=fill, Subscription=stroke (tracked in `IconUsesStroke` array)

---

## Icon availability

Available in `Icons.axaml`:
- `BitcoinSymbol` (fill), `NavIconTrendUp` (stroke), `RefreshArrows` (stroke)
- `Lightning`, `AngorLogo`, `CheckCircle`, `CloudUpload`, `Shield`, `Calendar`, `NavIconDocument`

---

## Known remaining work

1. **Type card hover bug** — unselected and selected cards show visual change on hover. Fix: use custom ControlTheme stripping `:pointerover`, or replace Button with clickable Border + PointerPressed handler
2. **Step 5 interstitial** — add sub-step with info screen before stages form
3. **Step 5 complete rewrite** — match Vue reference exactly (project length card, frequency card, generate button, advanced editor toggle)
4. **Efficient workflow** — always read Vue CSS/template from `angor-prototype` before coding, don't guess
