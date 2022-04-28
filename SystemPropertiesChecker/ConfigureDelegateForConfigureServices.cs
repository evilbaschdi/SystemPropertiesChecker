using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
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
        if (_ == null)
        {
            throw new ArgumentNullException(nameof(_));
        }

        if (serviceCollection == null)
        {
            throw new ArgumentNullException(nameof(serviceCollection));
        }

        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        configureCoreServices.RunFor(serviceCollection);
        serviceCollection.AddScoped<IScreenShot, ScreenShot>();
        serviceCollection.AddScoped<IRoundCorners, RoundCorners>();
        serviceCollection.AddSingleton<MainWindowViewModel>();
        serviceCollection.AddTransient(typeof(MainWindow));
    }
}