using SystemPropertiesChecker.Model;
using EvilBaschdi.Core.DotNetExtensions;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    ///     Interface for classes that provide values about the current windows version.
    /// </summary>
    public interface IWindowsVersionInformation : IValue<WindowsVersionInformationModel>
    {
    }
}