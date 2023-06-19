using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Avalonia.DepencencyInjection;
using SystemPropertiesChecker.Avalonia.ViewModels;
using SystemPropertiesChecker.Avalonia.Views;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Avalonia;

/// <inheritdoc />
public class App : Application
{
    /// <inheritdoc />
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    ///     ServiceProvider for DependencyInjection
    /// </summary>
    // ReSharper disable once MemberCanBePrivate.Global
    public static IServiceProvider ServiceProvider { get; set; }

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        IServiceCollection serviceCollection = new ServiceCollection();
        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        IConfigureWindowsAndViewModels configureWindowsAndViewModels = new ConfigureWindowsAndViewModels();

        configureCoreServices.RunFor(serviceCollection);
        configureWindowsAndViewModels.RunFor(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = new MainWindow
            {
                DataContext = ServiceProvider.GetRequiredService<MainWindowViewModel>()
            };

            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }
}