namespace Avalonia2.UI.Sections.Funders;

public partial class FundersView : UserControl
{
    public FundersView()
    {
        InitializeComponent();
        DataContext = new FundersViewModel();
    }
}
