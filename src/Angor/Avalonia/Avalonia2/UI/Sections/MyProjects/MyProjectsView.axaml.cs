using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.VisualTree;
using Avalonia2.UI.Shell;
using System.Reactive.Linq;

namespace Avalonia2.UI.Sections.MyProjects;

public partial class MyProjectsView : UserControl
{
    public MyProjectsView()
    {
        InitializeComponent();
        var vm = new MyProjectsViewModel();
        DataContext = vm;

        // Load sample projects for development/visual testing of populated state
        vm.LoadSampleProjects();

        AddHandler(Button.ClickEvent, OnButtonClick, RoutingStrategies.Bubble);

        // Manage panel visibility based on ViewModel state
        SubscribeToVisibility(vm);

        // Check if we should auto-open the wizard (from Home "Launch a Project" button)
        AttachedToVisualTree += OnAttachedToVisualTree;
    }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        var shell = this.FindAncestorOfType<ShellView>();
        if (shell?.DataContext is ShellViewModel shellVm && shellVm.PendingLaunchWizard)
        {
            shellVm.PendingLaunchWizard = false;
            if (DataContext is MyProjectsViewModel vm)
                OpenCreateWizard(vm);
        }
    }

    private void SubscribeToVisibility(MyProjectsViewModel vm)
    {
        // ShowCreateWizard drives the wizard panel
        vm.WhenAnyValue(x => x.ShowCreateWizard)
            .Subscribe(showWizard =>
            {
                if (CreateWizardPanel != null) CreateWizardPanel.IsVisible = showWizard;
                UpdateListVisibility(vm);
                // Update shell title
                var shell = this.FindAncestorOfType<ShellView>();
                if (shell?.DataContext is ShellViewModel shellVm)
                    shellVm.SectionTitleOverride = showWizard ? "Create New Project" : null;
            });

        // HasProjects drives empty state vs project list
        vm.WhenAnyValue(x => x.HasProjects)
            .Subscribe(_ => UpdateListVisibility(vm));
    }

    private void UpdateListVisibility(MyProjectsViewModel vm)
    {
        var showWizard = vm.ShowCreateWizard;
        if (EmptyStatePanel != null)
            EmptyStatePanel.IsVisible = !showWizard && !vm.HasProjects;
        if (ProjectListPanel != null)
            ProjectListPanel.IsVisible = !showWizard && vm.HasProjects;
    }

    private void OnButtonClick(object? sender, RoutedEventArgs e)
    {
        if (e.Source is not Button btn) return;
        if (DataContext is not MyProjectsViewModel vm) return;

        switch (btn.Name)
        {
            case "LaunchFromListButton":
                OpenCreateWizard(vm);
                break;
        }

        // EmptyState button doesn't have a Name — check by content
        if (btn.Content is Avalonia.Controls.StackPanel sp)
        {
            foreach (var child in sp.Children)
            {
                if (child is TextBlock tb && tb.Text == "Launch a Project")
                {
                    OpenCreateWizard(vm);
                    return;
                }
            }
        }
        // Also check direct TextBlock content inside button from EmptyState
        if (btn.Content is string s && s == "Launch a Project")
        {
            OpenCreateWizard(vm);
        }
    }

    private void OpenCreateWizard(MyProjectsViewModel vm)
    {
        vm.LaunchCreateWizard();

        // Wire the wizard's deploy callback
        // Vue ref: goToMyProjects() creates project, adds to list, closes wizard, navigates to my-projects.
        // Both "Go to My Projects" button AND backdrop click on success modal trigger this.
        if (CreateWizardView?.DataContext is CreateProjectViewModel wizardVm)
        {
            wizardVm.OnProjectDeployed = () =>
            {
                vm.OnProjectDeployed(wizardVm);
                vm.CloseCreateWizard(); // Close wizard → shows my-projects list with new project at top
            };
        }
    }
}
