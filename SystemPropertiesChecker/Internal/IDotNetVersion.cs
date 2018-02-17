using System.Collections.Generic;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    ///     Interface for classes that return a list of current installed dot net versions.
    /// </summary>
    public interface IDotNetVersion : IValue<List<string>>
    {
    }
}