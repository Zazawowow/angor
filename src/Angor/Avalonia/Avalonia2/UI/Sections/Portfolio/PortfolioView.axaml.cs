using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia2.UI.Shell;
using System.Reactive.Linq;

namespace Avalonia2.UI.Sections.Portfolio;

public partial class PortfolioView : UserControl
{
    public PortfolioView()
    {
        InitializeComponent();
        // Use the shared singleton PortfolioViewModel so investments added
        // during the invest flow persist across sidebar navigations.
        DataContext = SharedViewModels.Portfolio;
        AddHandler(Button.ClickEvent, OnButtonClick, RoutingStrategies.Bubble);

        // When navigating back to Funded, clear any open detail view
        // so the user sees the list (not a stale detail screen from last time).
        SharedViewModels.Portfolio.CloseInvestmentDetail();

        // Manage visibility of the portfolio list panel based on ViewModel state
        DataContextChanged += (_, _) => SubscribeToVisibility();
        SubscribeToVisibility();
    }

    private void SubscribeToVisibility()
    {
        if (DataContext is PortfolioViewModel vm)
        {
            // Portfolio list is visible when: HasInvestments AND no detail selected.
            // HasInvestments is handled by XAML binding; here we also hide when
            // SelectedInvestment is set (drill-down to detail view).
            vm.WhenAnyValue(
                x => x.HasInvestments,
                x => x.SelectedInvestment,
                (hasInvestments, selected) => hasInvestments && selected == null)
              .Subscribe(visible =>
              {
                  if (PortfolioListPanel != null)
                      PortfolioListPanel.IsVisible = visible;
              });
        }
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is Button btn && btn.Name == "ManageButton" &&
            btn.Tag is InvestmentViewModel investment &&
            DataContext is PortfolioViewModel vm)
        {
            vm.OpenInvestmentDetail(investment);
        }
    }
}
