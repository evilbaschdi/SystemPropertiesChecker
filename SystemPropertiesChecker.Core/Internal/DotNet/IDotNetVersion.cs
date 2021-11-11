using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that return a list of current installed .net versions
    /// </summary>
    public interface IDotNetVersion : IValue<List<string>>
    {
    }
}