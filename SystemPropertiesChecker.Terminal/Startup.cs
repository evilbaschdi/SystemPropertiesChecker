using EvilBaschdi.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal;

/// <inheritdoc />
public class Startup : IConfigureServiceCollection
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection serviceCollection)
    {
        ArgumentNullException.ThrowIfNull(serviceCollection);
        IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
        IConfigureTerminalServices configureTerminalServices = new ConfigureTerminalServices();

        configureCoreServices.RunFor(serviceCollection);
        configureTerminalServices.RunFor(serviceCollection);
    }
}