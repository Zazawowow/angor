using System.Collections.ObjectModel;
using System.Reactive.Linq;
using Avalonia2.UI.Shell;
using ReactiveUI;

namespace Avalonia2.UI.Sections.Funds;

/// <summary>
/// Individual wallet item within a seed group.
/// Data from Vue reference JS bundle.
/// </summary>
public class WalletItemViewModel
{
    public string Name { get; set; } = "";
    public string Balance { get; set; } = "0.00000000 BTC";
    public string WalletType { get; set; } = "On-Chain";
    public string Label { get; set; } = "";
    /// <summary>bitcoin, lightning, liquid</summary>
    public string IconType { get; set; } = "bitcoin";
}

/// <summary>
/// Seed group (account) containing multiple wallets.
/// Vue reference: "Angor Account" and "Imported Account".
/// </summary>
public class SeedGroupViewModel
{
    public string GroupName { get; set; } = "";
    public string GroupBalance { get; set; } = "0.00000000";
    public ObservableCollection<WalletItemViewModel> Wallets { get; set; } = new();
}

/// <summary>
/// Funds ViewModel — visual layer only.
/// Populated with Vue reference wallet data (2 seed groups, 5 wallets total).
/// The backend team will wire up real wallet data from the SDK.
///
/// Follows the canonical empty state toggle pattern:
///   - HasWallets is [Reactive] and toggled by ShowPopulatedApp
///   - LoadSampleData() / ClearToEmpty() mirror PortfolioViewModel
/// </summary>
public partial class FundsViewModel : ReactiveObject
{
    /// <summary>True when wallets exist and populated state should show.</summary>
    [Reactive] private bool hasWallets;

    /// <summary>Sum of all wallet balances</summary>
    public string TotalBalance { get; private set; } = "0.0000";

    /// <summary>Total invested amount</summary>
    public string TotalInvested { get; private set; } = "0.0000";

    /// <summary>Bitcoin on-chain balance for stat card</summary>
    public string BitcoinBalance { get; private set; } = "0.0000";

    /// <summary>Liquid balance for stat card</summary>
    public string LiquidBalance { get; private set; } = "0.0000";

    public ObservableCollection<SeedGroupViewModel> SeedGroups { get; } = new();

    public FundsViewModel()
    {
        // Load sample data if prototype toggle says populated
        if (SharedViewModels.Prototype.ShowPopulatedApp)
            LoadSampleData();

        // React to prototype toggle changes (populated ↔ empty)
        SharedViewModels.Prototype.WhenAnyValue(x => x.ShowPopulatedApp)
            .Skip(1) // skip initial value (already handled above)
            .Subscribe(showPopulated =>
            {
                if (showPopulated)
                    LoadSampleData();
                else
                    ClearToEmpty();
            });
    }

    /// <summary>
    /// Populate with sample wallet data for visual testing of the populated state.
    /// Called when ShowPopulatedApp toggle is enabled or at construction.
    /// Vue reference: 2 seed groups, 5 wallets total.
    /// </summary>
    public void LoadSampleData()
    {
        SeedGroups.Clear();

        SeedGroups.Add(new SeedGroupViewModel
        {
            GroupName = "Angor Account",
            GroupBalance = "4.2344",
            Wallets = new ObservableCollection<WalletItemViewModel>
            {
                new()
                {
                    Name = "Bitcoin Wallet",
                    Balance = "2.54320000 BTC",
                    WalletType = "On-Chain",
                    Label = "Investments",
                    IconType = "bitcoin"
                },
                new()
                {
                    Name = "Lightning Wallet",
                    Balance = "0.12340000 BTC",
                    WalletType = "Lightning",
                    Label = "Projects",
                    IconType = "lightning"
                },
                new()
                {
                    Name = "Liquid Wallet",
                    Balance = "1.56780000 BTC",
                    WalletType = "Liquid",
                    Label = "Trading",
                    IconType = "liquid"
                },
            }
        });

        SeedGroups.Add(new SeedGroupViewModel
        {
            GroupName = "Imported Account",
            GroupBalance = "1.0443",
            Wallets = new ObservableCollection<WalletItemViewModel>
            {
                new()
                {
                    Name = "Bitcoin Wallet 2",
                    Balance = "0.98760000 BTC",
                    WalletType = "On-Chain",
                    Label = "Savings",
                    IconType = "bitcoin"
                },
                new()
                {
                    Name = "Lightning Wallet 2",
                    Balance = "0.05670000 BTC",
                    WalletType = "Lightning",
                    Label = "Daily",
                    IconType = "lightning"
                },
            }
        });

        TotalBalance = "5.2787";
        TotalInvested = "0.0218";
        BitcoinBalance = "3.5308";
        LiquidBalance = "1.5678";
        HasWallets = true;
    }

    /// <summary>
    /// Clear all wallet data and show empty state.
    /// Called when ShowPopulatedApp toggle is disabled.
    /// </summary>
    public void ClearToEmpty()
    {
        SeedGroups.Clear();
        TotalBalance = "0.0000";
        TotalInvested = "0.0000";
        BitcoinBalance = "0.0000";
        LiquidBalance = "0.0000";
        HasWallets = false;
    }

    /// <summary>
    /// Add a new wallet group (called from the create wallet modal).
    /// Creates a seed group with a single Bitcoin wallet.
    /// Vue: finishGenerateWallet() / submitSeedImport() adds a new group.
    /// </summary>
    public void AddWalletGroup(string groupName, string walletType)
    {
        var group = new SeedGroupViewModel
        {
            GroupName = groupName,
            GroupBalance = "0.0000",
            Wallets = new ObservableCollection<WalletItemViewModel>
            {
                new()
                {
                    Name = $"Bitcoin Wallet",
                    Balance = "0.00000000 BTC",
                    WalletType = "On-Chain",
                    Label = "",
                    IconType = "bitcoin"
                }
            }
        };

        SeedGroups.Add(group);
        HasWallets = true;
    }
}
