using System;
using Avalonia;

namespace Avalonia2.Desktop;

sealed class Program
{
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    public static AppBuilder BuildAvaloniaApp() =>
        AppBuilder.Configure<Avalonia2.App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
