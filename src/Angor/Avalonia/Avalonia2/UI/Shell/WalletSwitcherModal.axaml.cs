using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Avalonia2.UI.Shell;

/// <summary>
/// Shell-level wallet switcher modal.
/// DataContext = ShellViewModel (set by the ShellView when opening).
/// Implements IBackdropCloseable so clicking the backdrop closes the modal.
/// Vue: "Select Wallet" modal in App.vue — clicking a wallet selects it and closes.
/// </summary>
public partial class WalletSwitcherModal : UserControl, IBackdropCloseable
{
    public WalletSwitcherModal()
    {
        InitializeComponent();

        AddHandler(Button.ClickEvent, OnButtonClick);
        AddHandler(Border.PointerPressedEvent, OnBorderPressed, RoutingStrategies.Bubble);
    }

    private ShellViewModel? Vm => DataContext as ShellViewModel;

    /// <summary>
    /// Called by the shell when the backdrop is clicked — just close.
    /// </summary>
    public void OnBackdropCloseRequested()
    {
        // No special cleanup needed — just let the shell close the modal.
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn) return;

        switch (btn.Name)
        {
            case "CloseWalletSwitcher":
                Vm?.HideModal();
                break;
        }
    }

    private void OnBorderPressed(object? sender, PointerPressedEventArgs e)
    {
        var source = e.Source as Control;
        Border? found = null;

        while (source != null)
        {
            if (source is Border b && b.Name == "WalletBorder")
            {
                found = b;
                break;
            }
            source = source.Parent as Control;
        }

        if (found?.DataContext is WalletSwitcherItem wallet)
        {
            // Select the wallet
            Vm?.SelectSwitcherWallet(wallet);

            // Update visual states via CSS class toggling (Rule #9 compliant)
            UpdateWalletSelection();

            // Vue behavior: selecting a wallet immediately closes the modal
            Vm?.HideModal();

            e.Handled = true;
        }
    }

    /// <summary>
    /// Update wallet card visual states via CSS class toggling.
    /// Same pattern as WalletSelectionHelper but for WalletSwitcherItem.
    /// </summary>
    private void UpdateWalletSelection()
    {
        var walletBorders = this.GetVisualDescendants()
            .OfType<Border>()
            .Where(b => b.Name == "WalletBorder");

        foreach (var border in walletBorders)
        {
            var isSelected = border.DataContext is WalletSwitcherItem w && w.IsSelected;
            border.Classes.Set("WalletSelected", isSelected);
        }
    }
}
