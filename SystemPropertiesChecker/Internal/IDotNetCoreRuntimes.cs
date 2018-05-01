using System.Collections.Generic;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that return a list of current installed .net core runtimes
    /// </summary>
    public interface IDotNetCoreRuntimes : IValue<List<string>>
    {
    }
}