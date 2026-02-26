using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using Avalonia.VisualTree;
using Avalonia2.UI.Sections.MyProjects.Deploy;

namespace Avalonia2.UI.Sections.FindProjects;

public partial class InvestPageView : UserControl
{
    // Vue ref: selected border = #4B7C5A, selected bg = #4B7C5A/20 (same in both themes)
    private static readonly SolidColorBrush SelectedBorderBrush = new(Color.Parse("#4B7C5A"));
    private static readonly SolidColorBrush SelectedBackground = new(Color.Parse("#194B7C5A")); // ~10% green
    // Quick amount selected: Vue bg #2d5a3d, border #2d5a3d, color white
    private static readonly SolidColorBrush QuickSelectedBg = new(Color.Parse("#2D5A3D"));
    private static readonly SolidColorBrush QuickSelectedBorder = new(Color.Parse("#2D5A3D"));

    public InvestPageView()
    {
        InitializeComponent();

        // Wire up button clicks
        AddHandler(Button.ClickEvent, OnButtonClick);
        // Wallet card clicks via PointerPressed on Border (no Button chrome)
        AddHandler(Border.PointerPressedEvent, OnBorderPressed, RoutingStrategies.Bubble);
    }

    private InvestPageViewModel? Vm => DataContext as InvestPageViewModel;

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn) return;

        switch (btn.Name)
        {
            // Wallet selector
            case "CloseWalletSelector":
                Vm?.CloseModal();
                break;
            case "PayWithWalletButton":
                Vm?.PayWithWallet();
                break;
            case "PayInvoiceInsteadButton":
                Vm?.ShowInvoice();
                break;

            // Invoice
            case "CloseInvoice":
                Vm?.CloseModal();
                break;
            case "CopyInvoiceButton":
                CopyToClipboard(Vm?.InvoiceString);
                break;

            // Success
            case "ViewInvestmentsButton":
                // Navigate back to project list
                NavigateBackToList();
                break;
        }
    }

    /// <summary>
    /// Handle clicks on Border elements — wallet cards, quick amounts, back button, submit, copy, QR.
    /// Walks up from clicked element to find the target Border by name.
    /// </summary>
    private void OnBorderPressed(object? sender, PointerPressedEventArgs e)
    {
        var source = e.Source as Control;
        Border? found = null;
        string? foundName = null;

        // Walk up to find a named border we care about
        while (source != null)
        {
            if (source is Border b && !string.IsNullOrEmpty(b.Name))
            {
                var name = b.Name;
                if (name == "WalletBorder" || name == "QuickAmountBorder" ||
                    name == "BackButton" || name == "SubmitButton" ||
                    name == "CopyProjectIdButton" || name == "QrCodePlaceholder" ||
                    name == "WalletBackdrop" || name == "InvoiceBackdrop" ||
                    name == "SuccessBackdrop")
                {
                    found = b;
                    foundName = name;
                    break;
                }
            }
            source = source.Parent as Control;
        }

        if (found == null || foundName == null) return;

        switch (foundName)
        {
            case "BackButton":
                NavigateBackToDetail();
                e.Handled = true;
                break;

            case "SubmitButton":
                Vm?.Submit();
                e.Handled = true;
                break;

            case "QuickAmountBorder":
                if (found.DataContext is QuickAmountOption option)
                {
                    Vm?.SelectQuickAmount(option.Amount);
                    UpdateQuickAmountSelection();
                    e.Handled = true;
                }
                break;

            case "WalletBorder":
                if (found.DataContext is WalletItem wallet)
                {
                    Vm?.SelectWallet(wallet);
                    UpdateWalletSelection();
                    e.Handled = true;
                }
                break;

            case "CopyProjectIdButton":
                CopyToClipboard(Vm?.ProjectId);
                e.Handled = true;
                break;

            case "QrCodePlaceholder":
                // Simulate payment received on QR click (Vue ref: clicking QR triggers payment)
                Vm?.PayViaInvoice();
                e.Handled = true;
                break;

            case "WalletBackdrop":
            case "InvoiceBackdrop":
                // Backdrop click = close modal
                if (e.Source is Border backdrop && (backdrop.Name == "WalletBackdrop" || backdrop.Name == "InvoiceBackdrop"))
                    Vm?.CloseModal();
                e.Handled = true;
                break;

            case "SuccessBackdrop":
                // Backdrop click on success = navigate back
                if (e.Source is Border successBg && successBg.Name == "SuccessBackdrop")
                    NavigateBackToList();
                e.Handled = true;
                break;
        }
    }

    /// <summary>Navigate back to project detail (go up one drill-down level).</summary>
    private void NavigateBackToDetail()
    {
        var findProjectsView = this.FindLogicalAncestorOfType<FindProjectsView>();
        if (findProjectsView?.DataContext is FindProjectsViewModel vm)
        {
            vm.CloseInvestPage();
        }
    }

    /// <summary>Navigate back to project list (from success modal).</summary>
    private void NavigateBackToList()
    {
        var findProjectsView = this.FindLogicalAncestorOfType<FindProjectsView>();
        if (findProjectsView?.DataContext is FindProjectsViewModel vm)
        {
            vm.CloseInvestPage();
            vm.CloseProjectDetail();
        }
    }

    /// <summary>Update wallet item borders to show selection state (identical to DeployFlowOverlay).</summary>
    private void UpdateWalletSelection()
    {
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

    /// <summary>Update quick amount borders to show selection state.
    /// Vue ref: selected bg #2d5a3d, border #2d5a3d, text white.</summary>
    private void UpdateQuickAmountSelection()
    {
        var unselectedBg = this.FindResource("StatsDetailInnerBackground") as IBrush
                           ?? new SolidColorBrush(Color.Parse("#FFFFFF"));
        var unselectedBorder = this.FindResource("StatsDetailInnerBorder") as IBrush
                               ?? new SolidColorBrush(Color.Parse("#40FDBA74"));

        var quickBorders = this.GetVisualDescendants()
            .OfType<Border>()
            .Where(b => b.Name == "QuickAmountBorder");

        foreach (var border in quickBorders)
        {
            var isSelected = border.DataContext is QuickAmountOption opt
                             && Vm?.SelectedQuickAmount != null
                             && Math.Abs(opt.Amount - Vm.SelectedQuickAmount.Value) < 0.0000001;

            border.Background = isSelected ? QuickSelectedBg : unselectedBg;
            border.BorderBrush = isSelected ? QuickSelectedBorder : unselectedBorder;

            // Update text colors for children
            var texts = border.GetVisualDescendants().OfType<TextBlock>().ToList();
            foreach (var tb in texts)
            {
                if (isSelected)
                    tb.Foreground = Brushes.White;
                else
                {
                    // Restore theme colors
                    var resourceKey = tb.FontWeight == Avalonia.Media.FontWeight.Bold
                        ? "StatsDetailValue"
                        : "StatsDetailLabel";
                    if (this.FindResource(resourceKey) is IBrush brush)
                        tb.Foreground = brush;
                }
            }
        }
    }

    private async void CopyToClipboard(string? text)
    {
        if (string.IsNullOrEmpty(text)) return;
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(text);
        }
    }
}
