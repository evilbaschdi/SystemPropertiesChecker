using EvilBaschdi.Core.Wpf;
using EvilBaschdi.Core.Wpf.AppHelpers;
using EvilBaschdi.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.ViewModels;

namespace SystemPropertiesChecker;

/// <inheritdoc />
public class ConfigureDelegateForConfigureServices : IConfigureDelegateForConfigureServices
{
    /// <inheritdoc />
    public void RunFor(HostBuilderContext _, IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(_);

        ArgumentNullException.ThrowIfNull(serviceCollection);

        serviceCollection.AddCoreServices();

        serviceCollection.AddScoped<IScreenShot, ScreenShot>();
        serviceCollection.AddSingleton<IApplicationLayout, ApplicationLayout>();
        serviceCollection.AddSingleton<IApplicationStyle, ApplicationStyle>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddTransient(typeof(MainWindow));
    }
}