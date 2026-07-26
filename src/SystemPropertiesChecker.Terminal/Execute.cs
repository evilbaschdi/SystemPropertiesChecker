using EvilBaschdi.Core;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Terminal.Internal;

namespace SystemPropertiesChecker.Terminal;

/// <inheritdoc />
public class Execute : IRun
{
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    ///     Constructor
    /// </summary>
    public Execute([NotNull] IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    /// <inheritdoc />
    public void Run()
    {
        //WINDOWS
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteWindowsTable writeWindowsTable = _serviceProvider?.GetService<IWriteWindowsTable>();
        writeWindowsTable?.Run();

        //HISTORY
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteHistoryTable writeHistoryTable = _serviceProvider?.GetService<IWriteHistoryTable>();
        writeHistoryTable?.Run();

        //.NET FRAMEWORK
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteDotNetTable writeDotNetTable = _serviceProvider?.GetService<IWriteDotNetTable>();
        writeDotNetTable?.Run();

        //.NET CORE
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteDotNetCoreTable writeDotNetCoreTable = _serviceProvider?.GetService<IWriteDotNetCoreTable>();
        writeDotNetCoreTable?.Run();

        //OTHER
        // ReSharper disable once SuggestVarOrType_SimpleTypes
        IWriteOtherTable writeOtherTable = _serviceProvider?.GetService<IWriteOtherTable>();
        writeOtherTable?.Run();
    }
}