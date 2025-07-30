using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
/// <summary>
///     Interface for classes that return info regarding current installed version of .net core
/// </summary>
public interface IDotNetCoreInfo : IListOfKeyValuePairOf<string, string>;