namespace Avalonia2.UI.Sections.Settings;

/// <summary>
/// Settings ViewModel — visual layer only.
/// Vue Settings has 8 sections: Network, Theme (mobile-only, we include it for desktop),
/// Explorer, Indexer, Nostr Relays, Currency Display, Seed Backup, Danger Zone.
/// The backend team will wire up real settings persistence via SDK.
/// </summary>
public partial class SettingsViewModel : ReactiveObject
{
    [Reactive] private string networkType = "Mainnet";
    [Reactive] private bool isNetworkModalOpen;

    // Explorer — production mainnet default
    [Reactive] private string explorerUrl = "https://explorer.angor.io";

    // Indexer — production mainnet default
    [Reactive] private string indexerUrl = "https://indexer.angor.io";

    // Nostr Relays — production mainnet defaults
    [Reactive] private string relay1 = "wss://relay.angor.io";
    [Reactive] private string relay2 = "wss://relay2.angor.io";
    [Reactive] private string relay3 = "wss://relay.damus.io";

    // Currency Display
    [Reactive] private string currencyDisplay = "BTC";

    public bool IsDarkThemeEnabled
    {
        get => Application.Current?.ActualThemeVariant == Avalonia.Styling.ThemeVariant.Dark;
        set
        {
            if (Application.Current != null)
            {
                Application.Current.RequestedThemeVariant = value
                    ? Avalonia.Styling.ThemeVariant.Dark
                    : Avalonia.Styling.ThemeVariant.Light;
            }
            this.RaisePropertyChanged();
        }
    }

    public void OpenNetworkModal() => IsNetworkModalOpen = true;
    public void CloseNetworkModal() => IsNetworkModalOpen = false;

    public void SelectNetwork(string network)
    {
        NetworkType = network;
        IsNetworkModalOpen = false;
    }
}
