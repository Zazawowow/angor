# HIGH-15: ShellViewModel — Wallet type and subtitle hardcoded

## Status: [x] COMPLETED — Subtitle now uses wallet name from SDK metadata

## Section: Shell

## Problem
`ShellViewModel.cs` lines 396-398: When building `WalletSwitcherItem`, `WalletType` is hardcoded to `"bitcoin"` and `Subtitle` to `"Bitcoin • Angor Account"` instead of coming from wallet metadata.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Shell/ShellViewModel.cs` (lines 396-398)

## What needs to happen
1. Get wallet type from SDK metadata (if available).
2. Build subtitle from actual wallet metadata (name, type, seed group).

## SDK availability
- `IWalletAppService.GetMetadatas()` returns wallet metadata — check what fields are available.

## Acceptance criteria
- Wallet switcher shows real type and meaningful subtitle per wallet.
