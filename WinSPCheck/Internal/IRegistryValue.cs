using EvilBaschdi.Core.DotNetExtensions;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public interface IRegistryValue : IValueFor<string, string>
    {
    }
}