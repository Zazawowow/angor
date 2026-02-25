using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;

namespace Avalonia2.UI.Sections.Portfolio;

public partial class InvestmentDetailView : UserControl
{
    public InvestmentDetailView()
    {
        InitializeComponent();
        AddHandler(Button.ClickEvent, OnButtonClick, RoutingStrategies.Bubble);
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is Button btn && btn.Name == "BackButton")
        {
            // Navigate back: find parent PortfolioView and call CloseInvestmentDetail
            var portfolioView = this.FindLogicalAncestorOfType<PortfolioView>();
            if (portfolioView?.DataContext is PortfolioViewModel vm)
            {
                vm.CloseInvestmentDetail();
            }
        }
    }
}
