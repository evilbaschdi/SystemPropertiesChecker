using System.Runtime.InteropServices;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <summary>
///     Class that returns a list of current installed dot net versions.
/// </summary>
public class DotNetVersion : IDotNetVersion
{
    private readonly ICheckNetFrameworkVersion45OrHigher _checkNetFrameworkVersion45OrHigher;
    private readonly INetFrameworkVersionFromRegistry _netFrameworkVersionFromRegistry;

    /// <summary>
    ///     Constructor.
    /// </summary>
    public DotNetVersion(INetFrameworkVersionFromRegistry netFrameworkVersionFromRegistry, ICheckNetFrameworkVersion45OrHigher checkNetFrameworkVersion45OrHigher)
    {
        _netFrameworkVersionFromRegistry = netFrameworkVersionFromRegistry ?? throw new ArgumentNullException(nameof(netFrameworkVersionFromRegistry));
        _checkNetFrameworkVersion45OrHigher = checkNetFrameworkVersion45OrHigher ?? throw new ArgumentNullException(nameof(checkNetFrameworkVersion45OrHigher));
    }

    /// <summary>
    ///     Contains a list of current installed dot net versions.
    /// </summary>
    public List<string> Value
    {
        get
        {
            var list = new List<string>
                       {
                           "currently installed versions:"
                       };

            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return list;
            }

            // .Net 2.0, 3.0, 3.5, 4.0
            list.AddRange(_netFrameworkVersionFromRegistry.Value);
            // .Net 4.5 and higher
            var netFrameworkVersion4OrHigher = _checkNetFrameworkVersion45OrHigher.Value;
            if (!string.IsNullOrWhiteSpace(netFrameworkVersion4OrHigher))
            {
                list.Add(netFrameworkVersion4OrHigher);
            }

            return list;
        }
    }
}