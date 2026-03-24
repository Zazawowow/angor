# HIGH-11: CreateWalletModal — BIP-39 test seed displayed as placeholder

## Status: [x] COMPLETED

## Section: Funds

## Problem
`CreateWalletModal.axaml` line 241 shows hardcoded test mnemonic `"abandon ability able about above absent absorb abstract absurd abuse access accident"` as the seed phrase display. This should show the real generated seed from `IWalletAppService.GenerateRandomSeedwords()`.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/CreateWalletModal.axaml` (line 241)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/CreateWalletModal.axaml.cs`

## What needs to happen
1. When "Generate" is clicked, call `GenerateRandomSeedwords()` and display the real seed in `SeedPhraseDisplay`.
2. Remove hardcoded placeholder from AXAML.

## SDK availability
- `FundsViewModel.CreateWalletAsync()` already calls `GenerateRandomSeedwords()` — but the generated words need to be displayed to the user BEFORE wallet creation so they can back them up.

## Acceptance criteria
- User sees real generated seed words, not test mnemonic.
