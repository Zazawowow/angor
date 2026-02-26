using System.Globalization;
using System.Reactive.Linq;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Styling;
using ReactiveUI;

namespace Avalonia2.UI.Shell;

/// <summary>
/// Converts a StreamGeometry resource key (e.g. "NavIconHome") to the actual StreamGeometry instance.
/// Used in the nav item DataTemplate to bind icon paths.
/// </summary>
public class NavIconConverter : IValueConverter
{
    public static readonly NavIconConverter Instance = new();

    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string key && Application.Current!.TryFindResource(key, out var resource) && resource is StreamGeometry geometry)
        {
            return geometry;
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}

public partial class ShellView : UserControl
{
    private static readonly BlurEffect ModalBlur = new() { Radius = 8 };

    /// <summary>
    /// The current modal content control that has been added as a direct child
    /// of the ModalOverlay Panel. We track it so we can remove it on close.
    /// </summary>
    private Control? _currentModalChild;

    public ShellView()
    {
        InitializeComponent();
        var vm = new ShellViewModel();
        DataContext = vm;

        var modalOverlay = this.FindControl<Panel>("ModalOverlay")!;
        var shellContent = this.FindControl<Grid>("ShellContent")!;

        // React to ModalContent changes — manage the visual tree directly.
        // This replaces the XAML ContentPresenter binding which suffered from
        // an intermittent race: IsVisible and Content changing in the same
        // binding batch could cause Avalonia to skip the measure/arrange pass
        // for the new content, leaving the modal card invisible (especially
        // on second open, and more often in light mode).
        vm.WhenAnyValue(x => x.ModalContent)
            .Subscribe(content =>
            {
                Avalonia.Threading.Dispatcher.UIThread.Post(() =>
                {
                    // Remove previous modal child from the panel
                    if (_currentModalChild != null)
                    {
                        modalOverlay.Children.Remove(_currentModalChild);
                        _currentModalChild = null;
                    }

                    if (content is Control control)
                    {
                        // Add the new control as a direct child of the Panel
                        // (after the backdrop Border, so it renders on top)
                        control.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch;
                        control.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Stretch;
                        modalOverlay.Children.Add(control);
                        _currentModalChild = control;

                        // Make the overlay visible AFTER the content is in the tree
                        modalOverlay.IsVisible = true;
                        shellContent.Effect = ModalBlur;

                        // Force a layout pass to guarantee the content is measured
                        modalOverlay.InvalidateMeasure();
                        modalOverlay.InvalidateArrange();
                    }
                    else
                    {
                        // No content — hide the overlay
                        modalOverlay.IsVisible = false;
                        shellContent.Effect = null;
                    }
                });
            });

        // Shell-level backdrop click-to-close
        var backdrop = this.FindControl<Border>("ShellModalBackdrop");
        if (backdrop != null)
        {
            backdrop.PointerPressed += OnBackdropPressed;
        }
    }

    /// <summary>
    /// Backdrop click — close the modal. Individual modal content views handle
    /// their own close logic via OnBackdropCloseRequested if they need custom behavior.
    /// </summary>
    private void OnBackdropPressed(object? sender, PointerPressedEventArgs e)
    {
        if (DataContext is ShellViewModel vm && vm.IsModalOpen)
        {
            // Notify the modal content that a backdrop close was requested.
            // The content can handle cleanup (e.g., resetting VM state) via IBackdropCloseable.
            if (vm.ModalContent is IBackdropCloseable closeable)
            {
                closeable.OnBackdropCloseRequested();
            }
            vm.HideModal();
            e.Handled = true;
        }
    }

    /// <summary>
    /// Navigates to the Settings section when the header gear icon is clicked.
    /// </summary>
    private void OnSettingsClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (DataContext is ShellViewModel vm)
        {
            vm.NavigateToSettings();
        }
    }

    /// <summary>
    /// Called when the ListBox prepares a container for an item.
    /// Applies the NavGroupHeaderItem theme to group header entries
    /// so they are non-selectable and visually distinct.
    /// </summary>
    public void OnNavContainerPreparing(object? sender, ContainerPreparedEventArgs e)
    {
        if (e.Container is ListBoxItem item && item.DataContext is NavGroupHeader)
        {
            if (this.TryFindResource("NavGroupHeaderItem", out var theme) && theme is ControlTheme ct)
            {
                item.Theme = ct;
            }
        }
    }
}

/// <summary>
/// Interface for modal content views that need custom behavior when the backdrop is clicked.
/// </summary>
public interface IBackdropCloseable
{
    void OnBackdropCloseRequested();
}
