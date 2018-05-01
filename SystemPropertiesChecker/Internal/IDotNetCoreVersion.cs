using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that return the current installed version of .net core
    /// </summary>
    public interface IDotNetCoreVersion : IValue<string>
    {
    }
}