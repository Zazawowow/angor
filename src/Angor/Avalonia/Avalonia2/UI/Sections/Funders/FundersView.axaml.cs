using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;
using System.Reactive.Linq;

namespace Avalonia2.UI.Sections.Funders;

public partial class FundersView : UserControl
{
    public FundersView()
    {
        InitializeComponent();
        DataContext = new FundersViewModel();
        AddHandler(Button.ClickEvent, OnButtonClick, RoutingStrategies.Bubble);

        // Wire up filter tab click handlers (Borders use Tapped, not Button.Click)
        if (TabWaiting != null)
            TabWaiting.Tapped += (_, _) => SetFilterFromTab("waiting");
        if (TabApproved != null)
            TabApproved.Tapped += (_, _) => SetFilterFromTab("approved");
        if (TabRejected != null)
            TabRejected.Tapped += (_, _) => SetFilterFromTab("rejected");

        // Subscribe to visibility states once DataContext is set
        DataContextChanged += (_, _) => SubscribeToVisibility();
        SubscribeToVisibility();
    }

    private void SetFilterFromTab(string filter)
    {
        if (DataContext is FundersViewModel vm)
            vm.SetFilter(filter);
    }

    private void SubscribeToVisibility()
    {
        if (DataContext is not FundersViewModel vm) return;

        // Show/hide the empty filter panel and signatures list based on FilteredSignatures count
        vm.WhenAnyValue(x => x.FilteredSignatures)
          .Subscribe(filtered =>
          {
              var hasItems = filtered is { Count: > 0 };
              if (FilterEmptyPanel != null)
                  FilterEmptyPanel.IsVisible = !hasItems;
              if (SignaturesListPanel != null)
                  SignaturesListPanel.IsVisible = hasItems;
          });

        // Update tab visual states when CurrentFilter changes
        vm.WhenAnyValue(x => x.CurrentFilter)
          .Subscribe(filter => UpdateTabVisuals(filter));
    }

    /// <summary>
    /// Updates tab CSS classes to reflect the active filter.
    /// Active tab: FilterTabActive class (styles handle border + text color)
    /// Inactive tab: no FilterTabActive class
    /// </summary>
    private void UpdateTabVisuals(string activeFilter)
    {
        TabWaiting?.Classes.Set("FilterTabActive", activeFilter == "waiting");
        TabApproved?.Classes.Set("FilterTabActive", activeFilter == "approved");
        TabRejected?.Classes.Set("FilterTabActive", activeFilter == "rejected");
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn || DataContext is not FundersViewModel vm)
            return;

        switch (btn.Name)
        {
            case "ApproveAllButton":
                vm.ApproveAll();
                e.Handled = true;
                break;

            case "ApproveButton" when btn.Tag is int approveId:
                vm.ApproveSignature(approveId);
                e.Handled = true;
                break;

            case "RejectButton" when btn.Tag is int rejectId:
                vm.RejectSignature(rejectId);
                e.Handled = true;
                break;

            case "ChatButton":
                // Chat action — placeholder for now
                e.Handled = true;
                break;

            case "ExpandButton" when btn.Tag is int expandId:
                vm.ToggleExpanded(expandId);
                ToggleExpandedPanel(expandId, vm.IsExpanded(expandId));
                e.Handled = true;
                break;

            case "CopyNpubButton" when btn.Tag is string npub:
                CopyToClipboard(npub);
                e.Handled = true;
                break;
        }
    }

    /// <summary>
    /// Find the ExpandedPanel with matching Tag and toggle its visibility.
    /// Also rotate the chevron icon on the ExpandButton.
    /// </summary>
    private void ToggleExpandedPanel(int id, bool isExpanded)
    {
        // Walk the visual tree of SignaturesListPanel to find matching panels
        if (SignaturesListPanel == null) return;

        foreach (var container in SignaturesListPanel.GetLogicalDescendants())
        {
            // Find ExpandedPanel borders with matching Tag
            if (container is Border { Name: "ExpandedPanel" } panel && panel.Tag is int panelId && panelId == id)
            {
                panel.IsVisible = isExpanded;
            }

            // Rotate the expand button chevron
            if (container is Button { Name: "ExpandButton" } expandBtn && expandBtn.Tag is int btnId && btnId == id)
            {
                expandBtn.RenderTransform = isExpanded
                    ? new RotateTransform(180)
                    : new RotateTransform(0);
            }
        }
    }

    private async void CopyToClipboard(string text)
    {
        var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(text);
        }
    }
}
