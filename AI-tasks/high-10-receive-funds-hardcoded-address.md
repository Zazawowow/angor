# HIGH-10: ReceiveFundsModal — Hardcoded placeholder address

## Status: [x] COMPLETED (resolved by CRITICAL-02)

## Section: Funds

## Problem
`ReceiveFundsModal.axaml` line 120 shows hardcoded `bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh`.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/ReceiveFundsModal.axaml` (line 120)

## Resolution
Blocked by CRITICAL-02. Same fix.

## Acceptance criteria
- Address comes from SDK, not hardcoded.
