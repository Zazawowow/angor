using System.Windows.Input;
using Avalonia.Input;

namespace Avalonia2.UI.Shared.Controls.Common;

public class CopyButton : ContentControl
{
    public ICommand CopyCommand { get; }

    public CopyButton()
    {
        CopyCommand = ReactiveCommand.Create<object?>(async parameter =>
        {
            var text = parameter?.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                var clipboard = TopLevel.GetTopLevel(this)?.Clipboard;
                if (clipboard != null)
                    await clipboard.SetTextAsync(text);
            }
        });
    }
}
