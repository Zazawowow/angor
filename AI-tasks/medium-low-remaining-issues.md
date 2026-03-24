# Medium & Low Priority Issues

---

## MEDIUM: Orphaned/Unwired Buttons

### M-26: Portfolio Refresh button — no handler
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/PortfolioView.axaml` (line ~93)
- **Problem:** Refresh button has no Click handler. Does nothing when clicked.
- **Fix:** Add Click handler that calls `PortfolioViewModel.LoadInvestmentsFromSdkAsync()`.
- [ ] Not started

### M-27: Settings Refresh Indexer button — no handler
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Settings/SettingsView.axaml`
- **Problem:** Button defined in AXAML but no Click handler wired in code-behind.
- **Fix:** Add handler calling `SettingsViewModel.RefreshIndexerStatusAsync()`.
- [ ] Not started

### M-28: Settings Refresh Relay button — no handler
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Settings/SettingsView.axaml`
- **Problem:** Same as M-27 but for Relay refresh.
- **Fix:** Add handler calling `SettingsViewModel.RefreshRelayStatusAsync()`.
- [ ] Not started

### M-29: Settings Download Backup button — no handler
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Settings/SettingsView.axaml`
- **Problem:** Button exists but Click handler missing in code-behind.
- **Fix:** Implement backup export functionality or disable button with "Coming Soon" state.
- [ ] Not started

### M-30: Funders Chat button — placeholder
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Funders/FundersView.axaml.cs` (line ~120)
- **Problem:** Handler has comment "Chat action — placeholder for now". No implementation.
- **Fix:** Implement Nostr DM or disable with "Coming Soon".
- [ ] Not started

### M-31: Settings Currency selector — disabled
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Settings/SettingsView.axaml` (line ~562)
- **Problem:** ComboBox has `IsEnabled="False"`, frozen at BTC.
- **Fix:** Intentional for now (BTC-only app). Document or add fiat conversion later.
- [ ] Not started

---

## MEDIUM: Fee Selection UI Not Connected

### M-32: SendFundsModal fee buttons — UI toggle only
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/SendFundsModal.axaml.cs`
- **Problem:** Fee buttons (Low/Medium/High) toggle CSS classes but perform no fee calculation.
- **Fix:** Blocked by CRITICAL-01. Once real send is implemented, wire fee selection to SDK fee estimation.
- [ ] Not started

### M-33: Portfolio recovery fee selection — ignored by SDK
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Portfolio/RecoveryModalsView.axaml.cs`
- **Problem:** User selects Priority/Standard/Economy fee but SDK calls always use hardcoded `DomainFeerate(20)` (20 sat/vB).
- **Fix:** Map fee priority selection to actual fee rates and pass to SDK Build*Transaction methods.
- [x] COMPLETED — fee priority now maps to 50/20/5 sat/vB and is passed to SDK calls

---

## LOW: Missing Functionality / Design Gaps

### L-34: CreateWalletModal Download Seed — no file save
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/CreateWalletModal.axaml.cs`
- **Problem:** "Download" button sets `_seedDownloaded = true` but doesn't open a file save dialog.
- **Fix:** Implement file save dialog using Avalonia's `StorageProvider.SaveFilePickerAsync()`.
- [ ] Not started

### L-35: SendFundsModal — no Bitcoin address format validation
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Funds/SendFundsModal.axaml.cs`
- **Problem:** Address validation is a basic null/empty check. No format validation (bech32, base58, etc.).
- **Fix:** Add Bitcoin address format validation (SDK may have address parsing utilities).
- [ ] Not started

### L-36: Funders Reject signature — not persisted to SDK
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Funders/FundersViewModel.cs`
- **Problem:** Rejecting a signature only updates local state, not persisted via SDK.
- **Fix:** Check if `IFounderAppService` has a reject method. If not, may need SDK-side work.
- [ ] Not started

### L-37: HomeViewModel — empty class
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/Home/HomeViewModel.cs`
- **Problem:** Completely empty class with no properties or SDK calls. Home page is static.
- **Fix:** Low priority — Home is a landing page. Could add portfolio summary stats later.
- [ ] Not started

### L-38: CreateProjectViewModel — no SDK validation of project params
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/CreateProjectViewModel.cs`
- **Problem:** Documented as "Visual-layer only". No server-side validation of project parameters.
- **Fix:** Add validation against SDK constraints before deployment (e.g., min/max amounts, stage limits).
- [ ] Not started

### L-39: WalletSwitcherModal OnBackdropCloseRequested — empty no-op
- **File:** `src/Angor/Avalonia/Avalonia2/UI/Shell/WalletSwitcherModal.axaml.cs`
- **Problem:** `OnBackdropCloseRequested()` is empty. Should close the modal when backdrop is tapped.
- **Fix:** Add `Vm?.HideModal()` call.
- [ ] Not started
