# CRITICAL-06: Portfolio RecoveryModalsView — Confirm Release is a stub

## Status: [x] COMPLETED

## Section: Portfolio

## Problem
`ProcessReleaseConfirmAsync()` in `RecoveryModalsView.axaml.cs` (lines 117-127) only does `Task.Delay(2000)` and updates `PenaltyState = "released"`. It never calls `PortfolioViewModel.ReleaseFundsAsync()` which is fully implemented.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/RecoveryModalsView.axaml.cs` (lines 117-127)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/PortfolioViewModel.cs` (`ReleaseFundsAsync` method)

## What needs to happen
1. Replace `Task.Delay(2000)` with a call to `PortfolioViewModel.ReleaseFundsAsync(investment, feeRate)`.
2. Use user's selected fee priority.
3. Handle success/failure.

## SDK availability
- `PortfolioViewModel.ReleaseFundsAsync()` calls `IInvestmentAppService.BuildReleaseTransaction()` + `SubmitTransactionFromDraft()`. Fully implemented.

## Acceptance criteria
- Confirm Release submits a real release transaction.
- Fee selection respected.
- Error handling present.
