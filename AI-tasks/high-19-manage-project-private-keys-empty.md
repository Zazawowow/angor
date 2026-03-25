# HIGH-19: ManageProjectViewModel — Private keys all empty strings

## Status: [x] COMPLETED

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
- `src/Angor/Avalonia/Avalonia2/UI/Sections/MyProjects/ManageProjectViewModel.cs`
- `src/Angor/Avalonia/Avalonia2/Composition/CompositionRoot.cs`

## Solution
1. Added `IProjectService`, `ISeedwordsProvider`, and `IDerivationOperations` as constructor dependencies.
2. `LoadProjectKeysAsync()` loads public keys (FounderKey, RecoveryKey, NostrNpub) from the Project entity via `IProjectService.GetAsync()`.
3. Private keys (NostrNsec, NostrHex) are derived from wallet seed words using `IDerivationOperations.DeriveProjectNostrPrivateKeyAsync()`.
4. Nostr public key is converted to npub format using `NostrConverter.ToNpub()`.
5. Nostr private key is converted to nsec format using `NostrConverter.ToNsec()`.
6. NIP-05 remains empty as it's Nostr profile metadata not stored in the project entity.
7. Updated CompositionRoot.cs factory delegate to provide the new dependencies.

## Acceptance criteria
- Private keys modal shows real project keys for the founder.
