using System.Collections.ObjectModel;

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
    public ObservableCollection<WalletItemViewModel> Wallets { get; set; } = new();
}

/// <summary>
/// Funds ViewModel — visual layer only.
/// Populated with Vue reference wallet data (2 seed groups, 5 wallets total).
/// The backend team will wire up real wallet data from the SDK.
/// </summary>
public partial class FundsViewModel : ReactiveObject
{
    /// <summary>Sum of all wallet balances</summary>
    public string TotalBalance { get; } = "5.2787";

    /// <summary>Total invested amount</summary>
    public string TotalInvested { get; } = "0.0218";

    public ObservableCollection<SeedGroupViewModel> SeedGroups { get; } = new()
    {
        new SeedGroupViewModel
        {
            GroupName = "Angor Account",
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
        },
        new SeedGroupViewModel
        {
            GroupName = "Imported Account",
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
        }
    };
}
