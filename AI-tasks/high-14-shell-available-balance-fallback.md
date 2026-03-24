# HIGH-14: ShellViewModel — Available balance fallback "10.0000 BTC"

## Status: [x] COMPLETED

## Section: Shell

## Problem
`ShellViewModel.cs` line 235: `AvailableBalanceDisplay` falls back to `"10.0000 BTC"` when no wallet is selected. This fake balance could confuse users.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Shell/ShellViewModel.cs` (line 235)

## What needs to happen
1. Show `"0.0000 BTC"` or `"—"` when no wallet is selected.
2. Show real balance from selected wallet.

## Acceptance criteria
- No fake balance displayed. Zero or placeholder when no wallet selected.
