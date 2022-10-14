using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class HandleNetFrameworkSetupNdpSubKey : IHandleNetFrameworkSetupNdpSubKey
{
    /// <inheritdoc />
    public string ValueFor((string SubKeyName, RegistryKey VersionKey) value)
    {
        var (subKeyName, versionKey) = value;

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return string.Empty;
        }

        var subKey = versionKey.OpenSubKey(subKeyName);

        if (subKey == null)
        {
            return string.Empty;
        }

        var name = subKey.GetValue("Version", "")?.ToString();
        if (string.IsNullOrWhiteSpace(name))
        {
            return string.Empty;
        }

        var sp = subKey.GetValue("SP", "")?.ToString();

        var install = subKey.GetValue("Install", "")?.ToString();

        if (string.IsNullOrEmpty(install) || !install.Equals("1"))
        {
            return string.Empty;
        }

        return !string.IsNullOrEmpty(sp)
            ? $"{subKeyName} | SP{sp} | {name}"
            : $"{subKeyName}: {name}";
    }
}