using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
/// <summary>
///     Interface for classes that return the current installed version of .net core
/// </summary>
public interface IDotNetCoreVersion : IValue<string>
{
}