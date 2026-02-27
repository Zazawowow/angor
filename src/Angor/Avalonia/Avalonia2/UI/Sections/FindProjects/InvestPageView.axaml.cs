using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.VisualTree;
using Avalonia2.UI.Shared.Helpers;
using Avalonia2.UI.Shell;
using ReactiveUI;

namespace Avalonia2.UI.Sections.FindProjects;

public partial class InvestPageView : UserControl
{
    private IDisposable? _screenSubscription;

    public InvestPageView()
    {
        InitializeComponent();

        // Wire up button clicks
        AddHandler(Button.ClickEvent, OnButtonClick);
        // Quick amount + submit + subscription plan border clicks
        AddHandler(Border.PointerPressedEvent, OnBorderPressed, RoutingStrategies.Bubble);
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        // Reset scroll to top when navigating to a new invest page
        var scroller = this.FindControl<ScrollViewer>("ContentScroller");
        scroller?.ScrollToHome();

        _screenSubscription?.Dispose();
        _screenSubscription = null;

        if (DataContext is InvestPageViewModel vm)
        {
            // Watch for screen changes to show/hide shell-level modal
            _screenSubscription = vm.WhenAnyValue(x => x.CurrentScreen)
                .Subscribe(screen =>
                {
                    if (screen != InvestScreen.InvestForm)
                    {
                        ShowShellModal(vm);
                    }
                });

            // If subscription, apply initial plan selection styling after layout
            if (vm.IsSubscription)
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                    UpdateSubscriptionPlanSelection(), Avalonia.Threading.DispatcherPriority.Loaded);
            }
        }
    }

    protected override void OnDetachedFromLogicalTree(LogicalTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromLogicalTree(e);
        _screenSubscription?.Dispose();
        _screenSubscription = null;
    }

    private InvestPageViewModel? Vm => DataContext as InvestPageViewModel;

    private ShellViewModel? GetShellVm()
    {
        var shellView = this.FindAncestorOfType<ShellView>();
        return shellView?.DataContext as ShellViewModel;
    }

    /// <summary>
    /// Create InvestModalsView and push it to the shell-level modal overlay.
    /// </summary>
    private void ShowShellModal(InvestPageViewModel vm)
    {
        var shellVm = GetShellVm();
        if (shellVm == null || shellVm.IsModalOpen) return;

        var modalsView = new InvestModalsView
        {
            DataContext = vm,
            OnNavigateBackToList = () =>
            {
                // Add the invested project to the Portfolio section
                AddInvestmentToPortfolio(vm);
                // Navigate to the Funded section to show the new investment
                var shell = GetShellVm();
                shell?.NavigateToFunded();
            }
        };

        shellVm.ShowModal(modalsView);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn) return;

        switch (btn.Name)
        {
            // Back button (standardized: Button inside Border)
            case "BackButton":
                NavigateBackToDetail();
                break;
        }
    }

    /// <summary>
    /// Handle clicks on Border elements — quick amounts, submit button, copy project ID,
    /// and subscription plan buttons.
    /// </summary>
    private void OnBorderPressed(object? sender, PointerPressedEventArgs e)
    {
        var source = e.Source as Control;
        Border? found = null;
        string? foundName = null;

        while (source != null)
        {
            if (source is Border b && !string.IsNullOrEmpty(b.Name))
            {
                var name = b.Name;
                if (name == "QuickAmountBorder" || name == "SubmitButton" ||
                    name == "CopyProjectIdButton" || name == "SubPlanBorder")
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

            case "SubPlanBorder":
                if (found.DataContext is SubscriptionPlanOption plan)
                {
                    Vm?.SelectSubscriptionPlan(plan.PatternId);
                    UpdateSubscriptionPlanSelection();
                    e.Handled = true;
                }
                break;

            case "CopyProjectIdButton":
                ClipboardHelper.CopyToClipboard(this, Vm?.ProjectId);
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

    /// <summary>
    /// Add the invested project to the shared Portfolio ViewModel
    /// so it appears in the "Funded" section.
    /// </summary>
    private static void AddInvestmentToPortfolio(InvestPageViewModel investVm)
    {
        SharedViewModels.Portfolio.AddInvestmentFromProject(
            investVm.Project,
            investVm.FormattedAmount);
    }

    /// <summary>Update quick amount borders via CSS class toggling.
    /// The "QuickAmountBtn" base style sets DynamicResource bg/border for unselected state.
    /// The "QuickAmountSelected" modifier class overrides with brand green + white text.
    /// BrushTransition provides smooth 150ms animation on bg, border, and text foreground.
    /// No ClearValue() — eliminates flash.</summary>
    private void UpdateQuickAmountSelection()
    {
        var quickBorders = this.GetVisualDescendants()
            .OfType<Border>()
            .Where(b => b.Name == "QuickAmountBorder");

        foreach (var border in quickBorders)
        {
            var isSelected = border.DataContext is QuickAmountOption opt
                             && Vm?.SelectedQuickAmount != null
                             && Math.Abs(opt.Amount - Vm.SelectedQuickAmount.Value) < 0.0000001;
            border.Classes.Set("QuickAmountSelected", isSelected);
        }
    }

    /// <summary>Update subscription plan borders via CSS class toggling.
    /// The "SubPlanBtn" base style sets DynamicResource bg/border for unselected state.
    /// The "SubPlanSelected" modifier class overrides with selected-state DynamicResource values.
    /// BrushTransition on the Border provides smooth 200ms animation.
    /// No FindResource() or ClearValue() — eliminates flash and wrong-theme bugs.</summary>
    private void UpdateSubscriptionPlanSelection()
    {
        var planBorders = this.GetVisualDescendants()
            .OfType<Border>()
            .Where(b => b.Name == "SubPlanBorder");

        foreach (var border in planBorders)
        {
            var isSelected = border.DataContext is SubscriptionPlanOption plan
                             && plan.PatternId == Vm?.SelectedSubscriptionPattern;
            border.Classes.Set("SubPlanSelected", isSelected);
        }
    }
}
