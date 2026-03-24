# HIGH-16: Constants — Fake Lightning invoice string

## Status: [ ] Not started

## Section: Shared

## Problem
`Constants.cs` line 27: `InvoiceString` is a fake Lightning invoice `lnbc100n1pjk4x0s...` used across invest and deploy flows. Should be generated dynamically.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Shared/Constants.cs` (line 27)
- Used in `InvestModalsView.axaml.cs` and `DeployFlowOverlay.axaml.cs`

## What needs to happen
1. Generate real invoices from the SDK or remove Lightning invoice UI if not supported yet.
2. If Lightning is not yet implemented, clearly mark the UI as "Coming Soon" rather than showing a fake invoice.

## Acceptance criteria
- No fake invoice string in production code. Either real or clearly marked as unavailable.
