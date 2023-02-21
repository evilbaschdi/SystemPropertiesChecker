using System.Runtime.InteropServices;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class NetFrameworkVersionFromRegistry : INetFrameworkVersionFromRegistry
{
    private readonly IHandleNetFrameworkSetupNdpKeys _handleNetFrameworkSetupNdpKeys;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="handleNetFrameworkSetupNdpKeys"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public NetFrameworkVersionFromRegistry(IHandleNetFrameworkSetupNdpKeys handleNetFrameworkSetupNdpKeys)
    {
        _handleNetFrameworkSetupNdpKeys = handleNetFrameworkSetupNdpKeys ?? throw new ArgumentNullException(nameof(handleNetFrameworkSetupNdpKeys));
    }

    /// <inheritdoc />
    public List<string> Value
    {
        get
        {
            var list = new List<string>();

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return list;
            }

            // Opens the registry key for the .NET Framework entry.
            using var localMachine = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "");
            using var ndpKey = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\");

            if (ndpKey == null)
            {
                return list;
            }

            foreach (var versionKeyName in ndpKey.GetSubKeyNames().Where(v => v.StartsWith("v")))
            {
                list.AddRange(_handleNetFrameworkSetupNdpKeys.ValueFor((versionKeyName, ndpKey)));
            }

            return list;
        }
    }
}