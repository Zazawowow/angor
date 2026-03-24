# HIGH-12: Portfolio InvestmentViewModel — All recovery/penalty data hardcoded

## Status: [x] COMPLETED — PenaltyDuration, PenaltyDaysRemaining from SDK; RecoveryProjectId uses ProjectIdentifier; MinerFee/DestinationAddress now settable (empty default)

## Section: Portfolio

## Problem
`InvestmentViewModel` (inside `PortfolioViewModel.cs`) has multiple hardcoded properties that should come from SDK:

- `PenaltyDuration => "90 days"` — hardcoded
- `MinerFee => "0.00000645"` — hardcoded
- `DestinationAddress => "tb1q7fxzwjc97ft53sugz8v7szj5ul02er7wtykjwp"` — hardcoded testnet address
- `RecoveryProjectId => "angor1q3pthkftzh3ym4emg0ctcxhmz5u5m7lt5tk69je"` — hardcoded
- `PenaltyDaysRemaining => 67` — hardcoded

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/PortfolioViewModel.cs` (InvestmentViewModel class, lines ~351-357)

## What needs to happen
1. Load penalty duration from the project's stage configuration via SDK.
2. Get real fee estimation from SDK or calculate from network.
3. Use the investor's actual wallet address.
4. Use the real project identifier.
5. Calculate remaining days from the recovery timeline.

## SDK availability
- `IInvestmentAppService.GetRecoveryStatus()` is already called in `LoadRecoveryStatusAsync()` — check if it returns penalty/timing data that can populate these fields.

## Acceptance criteria
- All 5 properties return real data from SDK or calculated values.
