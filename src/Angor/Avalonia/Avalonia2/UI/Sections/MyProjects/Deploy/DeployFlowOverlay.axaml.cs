using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace Avalonia2.UI.Sections.MyProjects.Deploy;

public partial class DeployFlowOverlay : UserControl
{
    // Vue ref: selected border = #4B7C5A, selected bg = #4B7C5A/20 (same in both themes)
    private static readonly SolidColorBrush SelectedBorderBrush = new(Color.Parse("#4B7C5A"));
    private static readonly SolidColorBrush SelectedBackground = new(Color.Parse("#194B7C5A")); // ~10% green

    public DeployFlowOverlay()
    {
        InitializeComponent();
        AddHandler(Button.ClickEvent, OnButtonClick);
        // Handle wallet card clicks via PointerPressed on Border elements (no Button chrome)
        AddHandler(Border.PointerPressedEvent, OnWalletBorderPressed, RoutingStrategies.Bubble);
    }

    private DeployFlowViewModel? Vm => DataContext as DeployFlowViewModel;

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn) return;

        switch (btn.Name)
        {
            // Wallet selector
            case "CloseWalletSelector":
                // Vue ref: X button / backdrop click → showDeployWalletModal = false (returns to step 6)
                Vm?.Close();
                break;
            case "PayWithWalletButton":
                // Vue ref: payWithDeployWallet() → shows QR with "received" → success
                Vm?.PayWithWallet();
                break;
            case "PayInvoiceInsteadButton":
                // Vue ref: proceedToDeployInvoice() → QR code modal
                Vm?.ShowPayFee();
                break;

            // Pay fee
            case "ClosePayFee":
                // Vue ref: closeDeployModal() → resets all state, back to step 6
                Vm?.Close();
                break;
            case "BackToWalletsButton":
                Vm?.BackToWalletSelector();
                break;
            case "CopyInvoiceButton":
                CopyInvoiceToClipboard();
                break;

            // Success — Vue ref: no X button on success modal
            case "GoToMyProjectsButton":
                // Vue ref: goToMyProjects() — creates project, adds to list, closes wizard
                Vm?.GoToMyProjects();
                break;
            case "CompleteProfileButton":
                // Vue ref: completeProfile() opens /profile/{id} in new tab, does NOT close modal.
                // Desktop stub: just go to my projects (same as "Go to My Projects").
                Vm?.GoToMyProjects();
                break;
        }
    }

    /// <summary>
    /// Handle backdrop click — Vue ref: @click.self on success modal calls goToMyProjects(),
    /// on wallet modal closes it, on deploy modal calls closeDeployModal().
    /// </summary>
    protected override void OnPointerPressed(Avalonia.Input.PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        // Only react to clicks directly on the backdrop (not bubbled from children)
        if (e.Source is Border border && border.Name == "Backdrop")
        {
            // Vue: backdrop click on success modal = goToMyProjects()
            // Vue: backdrop click on wallet/deploy modal = close modal
            Vm?.GoToMyProjects();
        }
    }

    /// <summary>
    /// Handle clicks on WalletBorder elements (replaces Button to avoid FluentTheme hover chrome).
    /// Walks up from the clicked element to find the WalletBorder, then uses its DataContext.
    /// </summary>
    private void OnWalletBorderPressed(object? sender, PointerPressedEventArgs e)
    {
        // Walk up from the hit target to find a Border named "WalletBorder"
        var source = e.Source as Control;
        Border? walletBorder = null;
        while (source != null)
        {
            if (source is Border b && b.Name == "WalletBorder")
            {
                walletBorder = b;
                break;
            }
            source = source.Parent as Control;
        }

        if (walletBorder?.DataContext is WalletItem wallet)
        {
            Vm?.SelectWallet(wallet);
            UpdateWalletSelection();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Update wallet item borders to show selection state.
    /// Vue ref: unselected = theme border/bg, selected = border-[#4B7C5A] bg-[#4B7C5A]/20
    /// Finds all WalletBorder elements directly (no Button wrapper).
    /// </summary>
    private void UpdateWalletSelection()
    {
        // Resolve theme-aware unselected colors
        var unselectedBorder = this.FindResource("Border") as IBrush
                               ?? new SolidColorBrush(Color.Parse("#404040"));
        var unselectedBg = this.FindResource("DeployWalletItemBg") as IBrush
                           ?? new SolidColorBrush(Color.Parse("#1a1a1a"));

        var walletBorders = this.GetVisualDescendants()
            .OfType<Border>()
            .Where(b => b.Name == "WalletBorder");

        foreach (var border in walletBorders)
        {
            var isSelected = border.DataContext is WalletItem w && w.IsSelected;
            border.BorderBrush = isSelected ? SelectedBorderBrush : unselectedBorder;
            border.Background = isSelected ? SelectedBackground : unselectedBg;
        }
    }

    private async void CopyInvoiceToClipboard()
    {
        if (Vm == null) return;
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(Vm.InvoiceString);
        }
    }
}
