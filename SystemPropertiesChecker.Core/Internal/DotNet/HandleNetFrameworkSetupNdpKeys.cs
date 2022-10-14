using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class HandleNetFrameworkSetupNdpKeys : IHandleNetFrameworkSetupNdpKeys
{
    private readonly IHandleNetFrameworkSetupNdpSubKey _handleNetFrameworkSetupNdpSubKey;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="handleNetFrameworkSetupNdpSubKey"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public HandleNetFrameworkSetupNdpKeys(IHandleNetFrameworkSetupNdpSubKey handleNetFrameworkSetupNdpSubKey)
    {
        _handleNetFrameworkSetupNdpSubKey = handleNetFrameworkSetupNdpSubKey ?? throw new ArgumentNullException(nameof(handleNetFrameworkSetupNdpSubKey));
    }

    /// <inheritdoc />
    [SupportedOSPlatform("windows")]
    public List<string> ValueFor((string VersionKeyName, RegistryKey NdpKey) value)
    {
        var (versionKeyName, ndpKey) = value;

        var list = new List<string>();

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return list;
        }

        // ReSharper disable once AccessToDisposedClosure
        var versionKey = ndpKey.OpenSubKey(versionKeyName);

        if (versionKey == null)
        {
            return list;
        }

        var name = versionKey.GetValue("Version", "")?.ToString();
        var sp = versionKey.GetValue("SP", "")?.ToString();
        var install = versionKey.GetValue("Install", "")?.ToString();

        // .Net 2.0, 3.0, 3.5
        if (!string.IsNullOrEmpty(name))
        {
            list.Add(!string.IsNullOrWhiteSpace(install) && install.Equals("1") && sp != ""
                ? $"{versionKeyName} | SP{sp} | {name}"
                : $"{versionKeyName} | {name}");
        }

        // .Net 4.0
        if (!string.IsNullOrEmpty(name))
        {
            return list;
        }

        list.AddRange(versionKey.GetSubKeyNames()
                                .Select(subKeyName => _handleNetFrameworkSetupNdpSubKey.ValueFor((subKeyName, versionKey)))
                                .Where(handleNetFrameworkSetupNdpSubKey => !string.IsNullOrWhiteSpace(handleNetFrameworkSetupNdpSubKey)));

        return list;
    }
}