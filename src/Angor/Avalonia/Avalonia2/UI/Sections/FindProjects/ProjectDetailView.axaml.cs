using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using Avalonia.Media;

namespace Avalonia2.UI.Sections.FindProjects;

public partial class ProjectDetailView : UserControl
{
    private bool _detailsExpanded = true;
    private bool _nostrExpanded = true;
    private bool _navCtaVisible;

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

        // Nav CTA button — same action as InvestButton
        var navCta = this.FindControl<Border>("NavCtaButton");
        if (navCta != null)
            navCta.PointerPressed += OnInvestPressed;

        // Scroll detection for nav CTA fade
        var scroller = this.FindControl<ScrollViewer>("ContentScroller");
        if (scroller != null)
            scroller.ScrollChanged += OnScrollChanged;

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

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        var investBtn = this.FindControl<Border>("InvestButton");
        var navCta = this.FindControl<Border>("NavCtaButton");
        var scroller = this.FindControl<ScrollViewer>("ContentScroller");

        if (investBtn == null || navCta == null || scroller == null) return;

        // Check if the InvestButton is scrolled out of the visible area.
        // TranslatePoint gives the position of InvestButton's top-left relative to the scroller viewport.
        var point = investBtn.TranslatePoint(new Point(0, investBtn.Bounds.Height), scroller);

        // If the bottom of the InvestButton is above the top of the viewport, show the nav CTA
        bool shouldShow = point.HasValue && point.Value.Y < 0;

        if (shouldShow != _navCtaVisible)
        {
            _navCtaVisible = shouldShow;
            navCta.Opacity = shouldShow ? 1 : 0;
            navCta.IsHitTestVisible = shouldShow;
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

        var chevron = this.FindControl<Control>("DetailsChevron");
        chevron?.Classes.Set("ChevronExpanded", _detailsExpanded);
    }

    private void OnNostrHeaderPressed(object? sender, PointerPressedEventArgs e)
    {
        _nostrExpanded = !_nostrExpanded;
        var content = this.FindControl<StackPanel>("NostrContent");
        if (content != null)
            content.IsVisible = _nostrExpanded;

        var chevron = this.FindControl<Control>("NostrChevron");
        chevron?.Classes.Set("ChevronExpanded", _nostrExpanded);
    }
}
