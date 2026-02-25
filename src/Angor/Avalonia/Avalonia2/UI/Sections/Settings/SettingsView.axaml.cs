using Avalonia.Input;
using Avalonia.Interactivity;

namespace Avalonia2.UI.Sections.Settings;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
        DataContext = new SettingsViewModel();
    }

    private SettingsViewModel? Vm => DataContext as SettingsViewModel;

    private void OnChangeNetworkClick(object? sender, RoutedEventArgs e) =>
        Vm?.OpenNetworkModal();

    private void OnModalBackdropPressed(object? sender, PointerPressedEventArgs e) =>
        Vm?.CloseNetworkModal();

    private void OnModalContentPressed(object? sender, PointerPressedEventArgs e) =>
        e.Handled = true; // Prevent backdrop close when clicking modal content

    private void OnSelectMainnet(object? sender, RoutedEventArgs e) =>
        Vm?.SelectNetwork("Mainnet");

    private void OnSelectTestnet(object? sender, RoutedEventArgs e) =>
        Vm?.SelectNetwork("Testnet");

    private void OnSelectSignet(object? sender, RoutedEventArgs e) =>
        Vm?.SelectNetwork("Signet");
}
