namespace Avalonia2.UI.Sections.Funds;

public partial class FundsView : UserControl
{
    public FundsView()
    {
        InitializeComponent();
        DataContext = new FundsViewModel();
    }
}
