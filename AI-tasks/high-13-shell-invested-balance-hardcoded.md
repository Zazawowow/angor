# HIGH-13: ShellViewModel — Invested balance always "0.0000 BTC"

## Status: [x] COMPLETED

## Section: Shell

## Problem
`ShellViewModel.cs` line 229: `InvestedBalanceDisplay` always returns `"0.0000 BTC"`. Should aggregate actual invested amounts from the Portfolio or SDK.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Shell/ShellViewModel.cs` (line 229)

## What needs to happen
1. Query `PortfolioViewModel` (already injected) for total invested amount.
2. Display real aggregated invested balance.

## SDK availability
- `PortfolioViewModel` already has `TotalInvested` property populated from `IInvestmentAppService.GetInvestments()`.

## Acceptance criteria
- Header shows real total invested balance.
