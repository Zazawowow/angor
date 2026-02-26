using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia2.UI.Shared.Controls;
using System.Reactive.Linq;

namespace Avalonia2.UI.Sections.FindProjects;

public partial class FindProjectsView : UserControl
{
    public FindProjectsView()
    {
        InitializeComponent();
        DataContext = new FindProjectsViewModel();

        // Listen for taps on ProjectCard elements to open project detail
        AddHandler(InputElement.TappedEvent, OnCardTapped, RoutingStrategies.Bubble);

        // Manage visibility of the project list panel based on ViewModel state
        DataContextChanged += (_, _) => SubscribeToVisibility();
        SubscribeToVisibility();
    }

    private void SubscribeToVisibility()
    {
        if (DataContext is FindProjectsViewModel vm)
        {
            // Manage 3 mutually exclusive panels:
            // - ProjectListPanel: visible when no selection at all
            // - ProjectDetailPanel: visible when project selected but invest page not open
            // - InvestPagePanel: visible when invest page is open
            vm.WhenAnyValue(x => x.SelectedProject, x => x.InvestPageViewModel)
              .Subscribe(tuple =>
              {
                  var hasProject = tuple.Item1 != null;
                  var hasInvest = tuple.Item2 != null;

                  if (ProjectListPanel != null)
                      ProjectListPanel.IsVisible = !hasProject && !hasInvest;

                  var detailPanel = this.FindControl<Panel>("ProjectDetailPanel");
                  if (detailPanel != null)
                      detailPanel.IsVisible = hasProject && !hasInvest;

                  var investPanel = this.FindControl<Panel>("InvestPagePanel");
                  if (investPanel != null)
                      investPanel.IsVisible = hasInvest;
              });
        }
    }

    private void OnCardTapped(object? sender, TappedEventArgs e)
    {
        // Walk up from tapped element to find the ProjectCard control
        var element = e.Source as Control;
        while (element != null && element is not ProjectCard)
            element = element.Parent as Control;

        if (element is ProjectCard card
            && card.DataContext is ProjectItemViewModel project
            && DataContext is FindProjectsViewModel vm)
        {
            vm.OpenProjectDetail(project);
            e.Handled = true;
        }
    }
}
