namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
///     Interface for providing system properties retrieved via PowerShell.
/// </summary>
public interface ISystemPropertiesProvider
{
    /// <summary>
    ///     Gets the unified system properties data.
    /// </summary>
    SystemPropertiesData Data { get; }
}