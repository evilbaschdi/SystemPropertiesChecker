using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
/// <summary>
///     Interface for classes that provide a WindowsVersionInformationStack.
/// </summary>
public interface IWindowsVersionDictionary : IValue<Dictionary<string, string>>;