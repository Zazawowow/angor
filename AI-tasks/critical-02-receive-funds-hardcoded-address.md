# CRITICAL-02: ReceiveFundsModal — Hardcoded address, no QR code

## Status: [x] COMPLETED

## Section: Funds

## Problem
The Receive modal shows a hardcoded address `bc1qxy2kgdygjrsqtzq2n0yrf2493p83kkfjhx0wlh` instead of calling `IWalletAppService.GetNextReceiveAddress()`. The QR code is a placeholder icon.

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/ReceiveFundsModal.axaml` (line 120 — hardcoded address, line 83 — QR stub)
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/ReceiveFundsModal.axaml.cs`
- `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/FundsViewModel.cs`

## What needs to happen
1. When the Receive modal opens, call `_walletAppService.GetNextReceiveAddress(walletId)` to get a fresh address.
2. Display the real address in the text field.
3. Generate a real QR code from the address (evaluate QR library options — e.g., QRCoder, ZXing.Net, SkiaSharp-based).
4. Copy button should copy the real address.

## SDK availability
- `IWalletAppService.GetNextReceiveAddress(walletId)` is already used in `InvestPageViewModel` — confirmed working.

## Acceptance criteria
- Modal shows a real receive address from the SDK for the selected wallet.
- QR code renders the address.
- Copy copies the real address.
