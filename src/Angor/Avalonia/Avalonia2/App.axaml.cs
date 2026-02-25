using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia2.UI.Shell;
using Zafiro.Avalonia.Icons;

namespace Avalonia2;

public partial class App : Application
{
    public override void Initialize()
    {
        IconControlProviderRegistry.Register(new ProjektankerIconControlProvider(), asDefault: true);
        IconControlProviderRegistry.Register(new SvgIconControlProvider());

        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
        }

        base.OnFrameworkInitializationCompleted();
    }
}
