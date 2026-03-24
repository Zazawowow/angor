# CRITICAL-07: ManageProject — Confirm Claim funds is a stub

## Status: [x] COMPLETED

## Section: MyProjects

## Problem
`OnConfirmClaimClick()` in `ManageProjectModalsView.axaml.cs` (lines ~207-224) only does `Task.Delay(1500)` with comment "Simulate network delay (visual-only, no real backend)". The founder cannot actually claim released funds from stages.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/Modals/ManageProjectModalsView.axaml.cs` (lines 207-224)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/ManageProjectViewModel.cs`

## What needs to happen
1. Add a `ClaimFounderFundsAsync()` method to `ManageProjectViewModel` that calls the appropriate `IFounderAppService` method.
2. Wire `OnConfirmClaimClick` to call it instead of `Task.Delay`.
3. Handle success/failure with proper UI feedback.

## SDK availability
- `IFounderAppService` is injected into `ManageProjectViewModel`. Check if it has a `ClaimFunds` or similar method. `GetClaimableTransactions()` is already called — the claim/spend counterpart may need investigation.

## Acceptance criteria
- Founders can claim funds from released stages.
- Real transaction is broadcast.
- Success/failure shown to user.
