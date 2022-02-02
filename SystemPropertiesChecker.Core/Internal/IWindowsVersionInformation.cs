using EvilBaschdi.Core;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
/// <summary>
///     Interface for classes that provide values about the current windows version.
/// </summary>
public interface IWindowsVersionInformation : IValue<WindowsVersionInformationModel>
{
}