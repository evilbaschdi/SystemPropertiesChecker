using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Terminal.Internal;

namespace SystemPropertiesChecker.Terminal;

/// <inheritdoc />
public class ConfigureTerminalServices : IConfigureTerminalServices
{
    /// <inheritdoc />
    public void RunFor(IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IWriteWindowsTable, WriteWindowsTable>();
        services.AddScoped<IWriteHistoryTable, WriteHistoryTable>();
        services.AddScoped<IWriteDotNetTable, WriteDotNetTable>();
        services.AddScoped<IWriteDotNetCoreTable, WriteDotNetCoreTable>();
        services.AddScoped<IWriteOtherTable, WriteOtherTable>();
    }
}