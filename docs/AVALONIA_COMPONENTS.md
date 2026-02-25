# Avalonia Component Registry

Inventory of shared UI components and where to find them. Use these instead of building ad-hoc layouts.

## Layout

| Component | Location | Usage |
|-----------|----------|-------|
| PageContainer | `UI/Shared/Controls/PageContainer.axaml` | Section content wrapper; responsive margin |
| ScrollableView | `UI/Shared/Controls/ScrollableView.axaml` | Scrollable areas with content padding |
| Pane | `UI/Shared/Controls/Pane.axaml` | Pane/split layout |
| FlexPanel | `Avalonia.Labs.Panels` | Row/column layouts; use `Classes="flex flex-col gap-4"` |

## Cards

| Component | Location | Usage |
|-----------|----------|-------|
| CardGrid | `Styles.axaml` (Zafiro) | Project/card lists; `Classes="CardGrid Listing"` |
| Card themes | `Shared/Styles/Cards.axaml`, `Themes/V2/Styles/` | WideCard, Card variants |
| ProjectCard | `UI/Shared/Controls/ProjectCard.axaml` | Project item template |

## Buttons

| Component | Location | Usage |
|-----------|----------|-------|
| EnhancedButton | Zafiro + `Styles.axaml` | Primary button control |
| FindProjectsBtn | `Themes/V2/Styles/Buttons.axaml` | Home screen CTAs |

## Forms

| Component | Location | Usage |
|-----------|----------|-------|
| AmountControl | `UI/Shared/Controls/AmountControl.axaml` | Amount input with validation |
| PasswordView | `UI/Shared/Controls/Password/` | Password input |
| FeerateSelector | `UI/Shared/Controls/Feerate/FeerateSelector.axaml` | Fee rate picker |

## Feedback

| Component | Location | Usage |
|-----------|----------|-------|
| Badge | `UI/Shared/Controls/Badge.axaml` | Status badges |
| ErrorSummary | `UI/Shared/Controls/ErrorSummary.axaml` | Validation errors |
| SuccessView | `UI/Shared/Controls/Common/Success/SuccessView.axaml` | Success state |

## Navigation

| Component | Location | Usage |
|-----------|----------|-------|
| SectionStrip | `Themes/V2/Styles/SectionStrip.axaml` | Main nav strip |
| Frame | `Themes/V2/Controls/Frame.axaml` | Content frame |

## Design Tokens

| Resource | Location | Values |
|----------|----------|--------|
| Spacing1–8 | `Themes/V2/Resources/Tokens.axaml` | 4, 8, 12, 16, 24, 32, 40, 48 |
| FontSizeSm, Base, Lg, Xl | Tokens.axaml | 12, 14, 16, 20 |
| RadiusSm, Md, Lg | Tokens.axaml | 6, 8, 12 |

## Utility Classes

See `Themes/V2/Styles/Utilities.axaml`:

- **Layout**: `.flex`, `.flex-col`, `.items-center`, `.justify-between`, `.gap-2`–`.gap-8`
- **Spacing**: `.p-4`, `.px-6`, `.py-3`, `.m-2`, `.mt-4` (Border/Layoutable)
- **Typography**: `.text-sm`, `.text-base`, `.text-muted`, `.font-medium`, `.font-bold`
- **Borders**: `.rounded`, `.rounded-lg` (Border)
