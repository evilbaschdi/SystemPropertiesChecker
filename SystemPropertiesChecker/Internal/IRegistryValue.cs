using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public interface IRegistryValue : IValueFor<string, string>
    {
    }
}