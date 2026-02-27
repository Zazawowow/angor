using Avalonia.Controls;
using Avalonia.VisualTree;
using Avalonia2.UI.Sections.MyProjects.Deploy;

namespace Avalonia2.UI.Shared.Helpers;

/// <summary>
/// Centralized wallet selection visual update helper.
/// Extracted from duplicated UpdateWalletSelection methods in InvestModalsView and DeployFlowOverlay.
/// Both views use identical CSS class toggling: "WalletSelected" on Border elements named "WalletBorder".
/// </summary>
public static class WalletSelectionHelper
{
    /// <summary>
    /// Update wallet card visual states via CSS class toggling.
    /// The "WalletCard" base style sets DynamicResource bg/border for unselected state.
    /// The "WalletSelected" modifier class overrides with selected-state DynamicResource values.
    /// </summary>
    /// <param name="root">The visual root to search for WalletBorder elements.</param>
    public static void UpdateWalletSelection(Control root)
    {
        var walletBorders = root.GetVisualDescendants()
            .OfType<Border>()
            .Where(b => b.Name == "WalletBorder");

        foreach (var border in walletBorders)
        {
            var isSelected = border.DataContext is WalletItem w && w.IsSelected;
            border.Classes.Set("WalletSelected", isSelected);
        }
    }
}
