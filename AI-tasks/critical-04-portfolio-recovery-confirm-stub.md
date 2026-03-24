# CRITICAL-04: Portfolio RecoveryModalsView — Confirm Recovery is a stub

## Status: [x] COMPLETED

## Section: Portfolio

## Problem
`ProcessRecoveryConfirmAsync()` in `RecoveryModalsView.axaml.cs` (lines 95-104) only does `Task.Delay(2000)` and updates local UI state. It never calls `PortfolioViewModel.RecoverFundsAsync()` which exists and is fully implemented with real SDK calls (`IInvestmentAppService.BuildRecoveryTransaction` + `SubmitTransactionFromDraft`).

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/RecoveryModalsView.axaml.cs` (lines 95-104)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/PortfolioViewModel.cs` (`RecoverFundsAsync` method)

## What needs to happen
1. Replace `Task.Delay(2000)` with a call to `PortfolioViewModel.RecoverFundsAsync(investment, feeRate)`.
2. Get the `PortfolioViewModel` reference (parent VM or via DI).
3. Use the user's selected fee priority instead of hardcoded value.
4. Handle success/failure — show error if SDK call fails instead of blindly showing success.

## SDK availability
- `PortfolioViewModel.RecoverFundsAsync()` already calls `IInvestmentAppService.BuildRecoveryTransaction()` and `SubmitTransactionFromDraft()`. Fully implemented.

## Acceptance criteria
- Confirm Recovery actually submits a recovery transaction to the blockchain.
- Fee selection is respected.
- Errors are shown to the user.
