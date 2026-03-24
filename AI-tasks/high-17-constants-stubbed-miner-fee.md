# HIGH-17: Constants — Stubbed miner fee

## Status: [ ] Not started

## Section: Shared

## Problem
`Constants.cs` line 15: `MinerFee = 0.00000391` is a hardcoded stub. Transaction fees should be estimated dynamically based on network conditions.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Shared/Constants.cs` (lines 15, 21)

## What needs to happen
1. Use SDK fee estimation or indexer API to get current fee rates.
2. Replace constant with dynamic calculation.
3. Alternatively, if fee estimation is not yet available in SDK, document this as a known limitation.

## Acceptance criteria
- Miner fee reflects actual network conditions or is clearly marked as estimated.
