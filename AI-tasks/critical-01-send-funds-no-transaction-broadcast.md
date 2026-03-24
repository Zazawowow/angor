# CRITICAL-01: SendFundsModal — No actual transaction broadcast

## Status: [x] COMPLETED

## Section: Funds

## Problem
The Send button in `SendFundsModal` validates the form then shows a hardcoded stub txid (`"a1b2c3d4e5f6...7890abcd"`). No actual Bitcoin transaction is built or broadcast via the SDK.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/SendFundsModal.axaml.cs` (line 22 — `StubTxid` constant)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/SendFundsModal.axaml` (lines 303, 311 — hardcoded fee/amount in success panel)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/FundsViewModel.cs`

## What needs to happen
1. Add a `SendTransactionAsync(walletId, destinationAddress, amount, feeRate)` method to `FundsViewModel` that calls `IWalletAppService` to build and broadcast a transaction.
2. Wire `BtnSend` click to call this method instead of just showing the success panel.
3. On success, display the real txid returned from the SDK.
4. On failure, show an error message instead of the success panel.
5. Remove `StubTxid` constant and hardcoded fee/amount from the success panel AXAML.

## SDK availability
- `IWalletAppService` is already injected into `FundsViewModel`. Check if it has a `Send` or `BuildTransaction` method. If not, this may need SDK-side work first.

## Acceptance criteria
- Clicking Send with valid inputs builds and broadcasts a real transaction.
- Real txid is displayed on success.
- Errors are shown to the user.
- No hardcoded txid, fee, or amount in the success panel.
