# CRITICAL-08: ManageProject — Confirm Release funds to investors is a stub

## Status: [x] COMPLETED

## Section: MyProjects

## Problem
`OnConfirmReleaseClick()` in `ManageProjectModalsView.axaml.cs` (~line 270) only does `Task.Delay(1500)` with comment "Simulate network delay". `ManageProjectViewModel.ReleaseFundsToInvestorsAsync()` exists and calls real SDK (`IFounderAppService.GetReleasableTransactions()` + `ReleaseFunds()`) but is never called from the modal.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/Modals/ManageProjectModalsView.axaml.cs` (~line 270)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/ManageProjectViewModel.cs` (`ReleaseFundsToInvestorsAsync` method, lines 278-314)

## What needs to happen
1. Wire `OnConfirmReleaseClick` to call `ManageProjectViewModel.ReleaseFundsToInvestorsAsync()`.
2. Handle success/failure with UI feedback.
3. Remove `Task.Delay(1500)` simulation.

## SDK availability
- `ManageProjectViewModel.ReleaseFundsToInvestorsAsync()` already calls `IFounderAppService.GetReleasableTransactions()` + `ReleaseFunds()`. Fully implemented.

## Acceptance criteria
- Confirm Release actually releases funds back to investors via SDK.
- Success/failure shown to user.
