# CRITICAL-05: Portfolio RecoveryModalsView — Claim Penalty is a stub

## Status: [x] COMPLETED

## Section: Portfolio

## Problem
`ProcessClaimPenaltyAsync()` in `RecoveryModalsView.axaml.cs` (lines 106-115) only does `Task.Delay(2000)` and updates `PenaltyState = "canRelease"`. No SDK call is made.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/RecoveryModalsView.axaml.cs` (lines 106-115)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/PortfolioViewModel.cs` (`ClaimEndOfProjectAsync` method)

## What needs to happen
1. Replace `Task.Delay(2000)` with a call to `PortfolioViewModel.ClaimEndOfProjectAsync(investment, feeRate)`.
2. Handle success/failure from SDK response.
3. Only transition `PenaltyState` on actual success.

## SDK availability
- `PortfolioViewModel.ClaimEndOfProjectAsync()` calls `IInvestmentAppService.BuildEndOfProjectClaim()` + `SubmitTransactionFromDraft()`. Fully implemented.

## Acceptance criteria
- Claim Penalty submits a real end-of-project claim transaction.
- State transitions only on success.
- Errors shown to user.
