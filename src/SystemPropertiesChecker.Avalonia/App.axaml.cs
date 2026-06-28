using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.Lifetime;
using SystemPropertiesChecker.Avalonia.ViewModels;
using SystemPropertiesChecker.Avalonia.Views;

namespace SystemPropertiesChecker.Avalonia;

/// <inheritdoc />
/// <inheritdoc />
public class App : ApplicationWithSplash
{
    /// <inheritdoc />
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    /// <inheritdoc />
    protected override void PreMainWindowCreation()
    {
        ApplicationServices.AppName = Current?.Name;
    }

    /// <inheritdoc />
    protected override Window CreateMainWindow() => new MainWindow
                                                    {
                                                        DataContext = ApplicationServices.GetRequiredService<MainWindowViewModel>()
                                                    };
}