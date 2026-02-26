using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;

namespace Avalonia2.UI.Sections.FindProjects;

public partial class ProjectDetailView : UserControl
{
    private bool _detailsExpanded = true;
    private bool _nostrExpanded = true;

    public ProjectDetailView()
    {
        InitializeComponent();

        // Back button — now a Button inside a Border wrapper
        var backBtn = this.FindControl<Button>("BackButton");
        if (backBtn != null)
            backBtn.Click += OnBackClick;

        // Invest button — navigate to InvestPage
        var investBtn = this.FindControl<Border>("InvestButton");
        if (investBtn != null)
            investBtn.PointerPressed += OnInvestPressed;

        // Collapsible sections
        var detailsHeader = this.FindControl<Border>("DetailsHeader");
        if (detailsHeader != null)
            detailsHeader.PointerPressed += OnDetailsHeaderPressed;

        var nostrHeader = this.FindControl<Border>("NostrHeader");
        if (nostrHeader != null)
            nostrHeader.PointerPressed += OnNostrHeaderPressed;

        // Set progress bar width after loaded
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        if (DataContext is ProjectItemViewModel vm)
        {
            var fill = this.FindControl<Border>("ProgressFill");
            if (fill?.Parent is Panel parent)
            {
                // Bind width as percentage of parent
                parent.PropertyChanged += (_, args) =>
                {
                    if (args.Property == BoundsProperty)
                        fill.Width = parent.Bounds.Width * (vm.Progress / 100.0);
                };
                // Initial
                if (parent.Bounds.Width > 0)
                    fill.Width = parent.Bounds.Width * (vm.Progress / 100.0);
            }
        }
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        // Walk up to find FindProjectsView and call CloseProjectDetail on its ViewModel
        var findProjectsView = this.FindLogicalAncestorOfType<FindProjectsView>();
        if (findProjectsView?.DataContext is FindProjectsViewModel vm)
        {
            vm.CloseProjectDetail();
        }
    }

    private void OnInvestPressed(object? sender, PointerPressedEventArgs e)
    {
        // Walk up to find FindProjectsView and call OpenInvestPage on its ViewModel
        var findProjectsView = this.FindLogicalAncestorOfType<FindProjectsView>();
        if (findProjectsView?.DataContext is FindProjectsViewModel vm)
        {
            vm.OpenInvestPage();
        }
    }

    private void OnDetailsHeaderPressed(object? sender, PointerPressedEventArgs e)
    {
        _detailsExpanded = !_detailsExpanded;
        var content = this.FindControl<StackPanel>("DetailsContent");
        if (content != null)
            content.IsVisible = _detailsExpanded;

        var chevron = this.FindControl<Path>("DetailsChevron");
        if (chevron?.RenderTransform is RotateTransform rt)
            rt.Angle = _detailsExpanded ? 0 : -90;
    }

    private void OnNostrHeaderPressed(object? sender, PointerPressedEventArgs e)
    {
        _nostrExpanded = !_nostrExpanded;
        var content = this.FindControl<StackPanel>("NostrContent");
        if (content != null)
            content.IsVisible = _nostrExpanded;

        var chevron = this.FindControl<Path>("NostrChevron");
        if (chevron?.RenderTransform is RotateTransform rt)
            rt.Angle = _nostrExpanded ? 0 : -90;
    }
}
