using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
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

    /// <inheritdoc />
    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
            configureCoreServices.RunFor(serviceCollection);
            serviceCollection.AddSingleton<MainWindowViewModel>();

            IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            desktop.MainWindow = new MainWindow
                                 {
                                     DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
                                 };
        }

        base.OnFrameworkInitializationCompleted();
    }
}