using Avalonia;
using Avalonia.Controls.Primitives;

namespace Avalonia2.UI.Shared.Controls;

/// <summary>
/// A wallet card row showing wallet name, balance, type label, and action buttons.
/// Vue: Wallet list items in Funds section — icon, name/type on left, balance + actions on right.
/// </summary>
public class WalletCard : TemplatedControl
{
    public static readonly StyledProperty<string?> WalletNameProperty =
        AvaloniaProperty.Register<WalletCard, string?>(nameof(WalletName));

    public static readonly StyledProperty<string?> BalanceProperty =
        AvaloniaProperty.Register<WalletCard, string?>(nameof(Balance));

    public static readonly StyledProperty<string?> WalletTypeProperty =
        AvaloniaProperty.Register<WalletCard, string?>(nameof(WalletType));

    public string? WalletName
    {
        get => GetValue(WalletNameProperty);
        set => SetValue(WalletNameProperty, value);
    }

    public string? Balance
    {
        get => GetValue(BalanceProperty);
        set => SetValue(BalanceProperty, value);
    }

    public string? WalletType
    {
        get => GetValue(WalletTypeProperty);
        set => SetValue(WalletTypeProperty, value);
    }
}
