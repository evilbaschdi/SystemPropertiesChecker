using EvilBaschdi.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal;

/// <inheritdoc />
public class ConfigureServices : IConfigureServices
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
        IConfigureTerminalServices configureTerminalServices = new ConfigureTerminalServices();

        configureCoreServices.RunFor(serviceCollection);
        configureTerminalServices.RunFor(serviceCollection);
    }
}