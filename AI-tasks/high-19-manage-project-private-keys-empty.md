# HIGH-19: ManageProjectViewModel — Private keys all empty strings

## Status: [ ] Not started

## Section: MyProjects

## Problem
`ManageProjectViewModel.cs` lines 116-121: All key display properties are empty strings:
- `FounderKey = ""`
- `RecoveryKey = ""`
- `NostrNpub = ""`
- `Nip05 = ""`
- `NostrNsec = ""`
- `NostrHex = ""`

Comment says: "SDK doesn't expose these directly"

## Files
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/ManageProjectViewModel.cs` (lines 115-121)

## What needs to happen
1. Investigate what key data `IFounderAppService.CreateProjectKeys()` returns during deployment.
2. Store keys at deployment time and retrieve them for display.
3. Or add SDK methods to derive/retrieve these from the wallet.

## SDK availability
- `IFounderAppService` is injected. Keys may need to be persisted during project creation and retrieved later.

## Acceptance criteria
- Private keys modal shows real project keys for the founder.
