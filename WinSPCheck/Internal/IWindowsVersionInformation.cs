using EvilBaschdi.Core.DotNetExtensions;
using WinSPCheck.Model;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Interface for classes that provide values about the current windows version.
    /// </summary>
    public interface IWindowsVersionInformation : IValue<WindowsVersionInformationModel>
    {
    }
}