using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using System.Reactive.Linq;

namespace Avalonia2.UI.Sections.Portfolio;

public partial class PortfolioView : UserControl
{
    public PortfolioView()
    {
        InitializeComponent();
        DataContext = new PortfolioViewModel();
        AddHandler(Button.ClickEvent, OnButtonClick, RoutingStrategies.Bubble);

        // Manage visibility of the portfolio list panel based on ViewModel state
        DataContextChanged += (_, _) => SubscribeToVisibility();
        SubscribeToVisibility();
    }

    private void SubscribeToVisibility()
    {
        if (DataContext is PortfolioViewModel vm)
        {
            // Portfolio list is visible when: HasInvestments AND SelectedInvestment == null
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
