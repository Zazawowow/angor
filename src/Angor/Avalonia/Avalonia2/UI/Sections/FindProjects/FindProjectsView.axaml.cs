namespace Avalonia2.UI.Sections.FindProjects;

public partial class FindProjectsView : UserControl
{
    public FindProjectsView()
    {
        InitializeComponent();
        DataContext = new FindProjectsViewModel();
    }
}
