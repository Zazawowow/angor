# HIGH-09: SendFundsModal — Hardcoded stub txid in success panel

## Status: [x] COMPLETED (resolved by CRITICAL-01)

## Section: Funds

## Problem
`SendFundsModal.axaml.cs` line 22 defines `StubTxid = "a1b2c3d4e5f6...7890abcd"` which is displayed after a "successful" send. The AXAML success panel also has hardcoded fee `"0.00001200 BTC"` and amount.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/SendFundsModal.axaml.cs` (line 22)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/SendFundsModal.axaml` (lines 303, 311)

## Resolution
Blocked by CRITICAL-01. Once real send is implemented, replace stub values with real transaction data.

## Acceptance criteria
- No hardcoded txid, fee, or amount values in the send success panel.
