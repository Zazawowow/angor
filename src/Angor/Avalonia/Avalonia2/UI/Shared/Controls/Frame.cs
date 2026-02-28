using Avalonia.Controls.Primitives;
using Avalonia.Media;

namespace Avalonia2.UI.Shared.Controls;

/// <summary>
/// A content control with Header, Footer, and Back button support.
/// Replaces Zafiro's Frame with identical API surface.
/// Template parts: BackButton, Header (ContentPresenter), Footer (ContentPresenter), Content.
/// </summary>
public class Frame : HeaderedContentControl
{
    public static readonly StyledProperty<object?> FooterProperty =
        AvaloniaProperty.Register<Frame, object?>(nameof(Footer));

    public static readonly StyledProperty<IBrush?> FooterBackgroundProperty =
        AvaloniaProperty.Register<Frame, IBrush?>(nameof(FooterBackground));

    public static readonly StyledProperty<Thickness> FooterPaddingProperty =
        AvaloniaProperty.Register<Frame, Thickness>(nameof(FooterPadding));

    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    public IBrush? FooterBackground
    {
        get => GetValue(FooterBackgroundProperty);
        set => SetValue(FooterBackgroundProperty, value);
    }

    public Thickness FooterPadding
    {
        get => GetValue(FooterPaddingProperty);
        set => SetValue(FooterPaddingProperty, value);
    }
}
