using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
public abstract class RegistryValueFor : IRegistryValueFor
{
    private readonly ISystemPropertiesProvider _systemPropertiesProvider;
    private readonly string _subKey;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="subKey"></param>
    /// <param name="systemPropertiesProvider"></param>
    protected RegistryValueFor([NotNull] string subKey, [NotNull] ISystemPropertiesProvider systemPropertiesProvider)
    {
        _subKey = subKey ?? throw new ArgumentNullException(nameof(subKey));
        _systemPropertiesProvider = systemPropertiesProvider ?? throw new ArgumentNullException(nameof(systemPropertiesProvider));
    }

    /// <inheritdoc />
    public string ValueFor([NotNull] string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        if (!OperatingSystem.IsWindows())
        {
            return string.Empty;
        }

        if (_subKey.Equals(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", StringComparison.OrdinalIgnoreCase))
        {
            var dict = _systemPropertiesProvider.Data.RegistryWindowsNtCurrentVersion;
            if (dict != null && dict.TryGetValue(value, out var result))
            {
                return result ?? string.Empty;
            }
        }
        else if (_subKey.Equals(@"SOFTWARE\Microsoft\WindowsSelfHost\UI\Selection", StringComparison.OrdinalIgnoreCase))
        {
            var dict = _systemPropertiesProvider.Data.RegistryWindowsSelfHostUiSelection;
            if (dict != null && dict.TryGetValue(value, out var result))
            {
                return result ?? string.Empty;
            }
        }

        return string.Empty;
    }
}