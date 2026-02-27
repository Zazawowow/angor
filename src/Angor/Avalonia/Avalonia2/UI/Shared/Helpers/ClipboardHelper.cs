using Avalonia.Controls;

namespace Avalonia2.UI.Shared.Helpers;

/// <summary>
/// Centralized clipboard helper extracted from 4 duplicated CopyToClipboard methods
/// in InvestModalsView, InvestPageView, DeployFlowOverlay, and FundersView.
/// </summary>
public static class ClipboardHelper
{
    /// <summary>
    /// Copy text to the system clipboard via the TopLevel clipboard API.
    /// </summary>
    /// <param name="control">Any attached control (used to resolve TopLevel).</param>
    /// <param name="text">The text to copy. No-ops if null or empty.</param>
    public static async void CopyToClipboard(Control control, string? text)
    {
        if (string.IsNullOrEmpty(text)) return;
        var clipboard = TopLevel.GetTopLevel(control)?.Clipboard;
        if (clipboard != null)
        {
            await clipboard.SetTextAsync(text);
        }
    }
}
