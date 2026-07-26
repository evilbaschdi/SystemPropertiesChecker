using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPropertiesChecker.Terminal;

/// <inheritdoc />
public class InitializeServiceProvider : IInitializeServiceProvider
{
    /// <inheritdoc />
    public IServiceProvider ValueFor([NotNull] Action<IServiceCollection> serviceCollectionConfiguration)
    {
        ArgumentNullException.ThrowIfNull(serviceCollectionConfiguration);

        IServiceCollection serviceCollection = new ServiceCollection();
        serviceCollectionConfiguration(serviceCollection);
        var serviceProvider = serviceCollection.BuildServiceProvider();
        return serviceProvider;
    }
}