# CRITICAL-03: WalletDetailModal — Copy UTXO txid references undefined MockUtxos

## Status: [x] COMPLETED

## Section: Funds

## Problem
`WalletDetailModal.axaml.cs` line 105 references `MockUtxos[idx].Txid` which is an undefined property. This will crash at runtime when a user tries to copy a UTXO transaction ID. The real data is in `_utxos`.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/WalletDetailModal.axaml.cs` (line ~105)

## What needs to happen
1. Replace `MockUtxos[idx]` references with `_utxos[idx]`.
2. Extract the txid from the real UTXO object (check the `UtxoData` or similar type for the correct property name).
3. Add bounds checking to prevent index-out-of-range.

## SDK availability
- `_utxos` is already populated from `AccountBalanceInfo.AccountInfo.AllUtxos()` — real SDK data.

## Acceptance criteria
- Copy UTXO txid works without crashing.
- Copies the real transaction ID from the UTXO.
