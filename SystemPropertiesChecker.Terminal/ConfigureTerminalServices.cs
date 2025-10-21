using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Terminal.Internal;

namespace SystemPropertiesChecker.Terminal;

/// <summary />
public static class ConfigureTerminalServices
{
    /// <summary />
    public static void AddTerminalServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IWriteWindowsTable, WriteWindowsTable>();
        services.AddScoped<IWriteHistoryTable, WriteHistoryTable>();
        services.AddScoped<IWriteDotNetTable, WriteDotNetTable>();
        services.AddScoped<IWriteDotNetCoreTable, WriteDotNetCoreTable>();
        services.AddScoped<IWriteOtherTable, WriteOtherTable>();
    }
}