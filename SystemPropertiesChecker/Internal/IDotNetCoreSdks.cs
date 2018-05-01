using System.Collections.Generic;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that return a list of current installed .net core sdks
    /// </summary>
    public interface IDotNetCoreSdks : IValue<List<string>>
    {
    }
}