using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class CheckNetFrameworkVersion45OrHigher : ICheckNetFrameworkVersion45OrHigher
{
    private readonly ISystemPropertiesProvider _systemPropertiesProvider;
    private readonly IParseReleaseKeyByReleaseKeyMappingList _parseReleaseKeyByReleaseKeyMappingList;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="parseReleaseKeyByReleaseKeyMappingList"></param>
    /// <param name="systemPropertiesProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public CheckNetFrameworkVersion45OrHigher([NotNull] IParseReleaseKeyByReleaseKeyMappingList parseReleaseKeyByReleaseKeyMappingList,
                                               [NotNull] ISystemPropertiesProvider systemPropertiesProvider)
    {
        _parseReleaseKeyByReleaseKeyMappingList = parseReleaseKeyByReleaseKeyMappingList ?? throw new ArgumentNullException(nameof(parseReleaseKeyByReleaseKeyMappingList));
        _systemPropertiesProvider = systemPropertiesProvider ?? throw new ArgumentNullException(nameof(systemPropertiesProvider));
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

            var releaseKey = _systemPropertiesProvider.Data.NetFrameworkReleaseKey;
            return string.IsNullOrWhiteSpace(releaseKey)
                ? null
                : _parseReleaseKeyByReleaseKeyMappingList.ValueFor(releaseKey);
        }
    }
}