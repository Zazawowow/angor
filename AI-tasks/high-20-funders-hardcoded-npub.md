# HIGH-20: FundersViewModel — Hardcoded sample npub fallback

## Status: [ ] Not started

## Section: Funders

## Problem
`FundersViewModel.cs` line 30: Uses a hardcoded npub `"npub1aunjpz36t2vwtqxyph2jc30c4feng4gv5yhhw6yckgzxa0rn52tq7tsnm7"` as a fallback when investor npub is not available from SDK.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funders/FundersViewModel.cs` (line 30)

## What needs to happen
1. Use the real investor npub from the SDK `Investment` object.
2. If npub is genuinely unavailable, show "Unknown" or empty rather than a fake npub.

## Acceptance criteria
- No fake npub displayed. Real data or clear placeholder.
