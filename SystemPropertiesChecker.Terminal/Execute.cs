using EvilBaschdi.Core;
using EvilBaschdi.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemPropertiesChecker.Terminal.Internal;

namespace SystemPropertiesChecker.Terminal;

/// <inheritdoc />
public class Execute : IRun
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public Execute()
    {
        IHostInstance hostInstance = new HostInstance();
        IValueFor<Action<HostBuilderContext, IServiceCollection>, IServiceProvider> initServiceProviderByHostBuilder = new InitServiceProviderByHostBuilder(hostInstance);

        ServiceProvider = initServiceProviderByHostBuilder.ValueFor(new ConfigureServices().RunFor);
    }

    /// <summary>
    ///     ServiceProvider for DependencyInjection
    /// </summary>
    private static IServiceProvider? ServiceProvider { get; set; }

    /// <inheritdoc />
    public void Run()
    {
        //WINDOWS
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteWindowsTable? writeWindowsTable = ServiceProvider?.GetService<IWriteWindowsTable>();
        writeWindowsTable?.Run();

        //HISTORY
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteHistoryTable? writeHistoryTable = ServiceProvider?.GetService<IWriteHistoryTable>();
        writeHistoryTable?.Run();

        //.NET FRAMEWORK
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteDotNetTable? writeDotNetTable = ServiceProvider?.GetService<IWriteDotNetTable>();
        writeDotNetTable?.Run();

        //.NET CORE
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteDotNetCoreTable? writeDotNetCoreTable = ServiceProvider?.GetService<IWriteDotNetCoreTable>();
        writeDotNetCoreTable?.Run();

        //OTHER
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteOtherTable? writeOtherTable = ServiceProvider?.GetService<IWriteOtherTable>();
        writeOtherTable?.Run();
    }
}