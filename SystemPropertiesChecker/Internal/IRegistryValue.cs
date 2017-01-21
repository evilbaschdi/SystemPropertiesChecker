using EvilBaschdi.Core.DotNetExtensions;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public interface IRegistryValue : IValueFor<string, string>
    {
    }
}