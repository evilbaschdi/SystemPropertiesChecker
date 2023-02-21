using System.Runtime.InteropServices;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class CheckNetFrameworkVersion45OrHigher : ICheckNetFrameworkVersion45OrHigher
{
    private readonly IParseReleaseKeyByReleaseKeyMappingList _parseReleaseKeyByReleaseKeyMappingList;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="parseReleaseKeyByReleaseKeyMappingList"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CheckNetFrameworkVersion45OrHigher([NotNull] IParseReleaseKeyByReleaseKeyMappingList parseReleaseKeyByReleaseKeyMappingList)
    {
        _parseReleaseKeyByReleaseKeyMappingList = parseReleaseKeyByReleaseKeyMappingList ?? throw new ArgumentNullException(nameof(parseReleaseKeyByReleaseKeyMappingList));
    }

    /// <inheritdoc />
    public string Value
    {
        get
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return null;
            }

            using var localMachine = RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "");
            using var ndpKey = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\");
            if (ndpKey == null)
            {
                return null;
            }

            var releaseKey = ndpKey.GetValue("Release")?.ToString();
            return _parseReleaseKeyByReleaseKeyMappingList.ValueFor(releaseKey);
        }
    }
}