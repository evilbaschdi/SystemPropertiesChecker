using System.Runtime.InteropServices;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class NetFrameworkVersionFromRegistry : INetFrameworkVersionFromRegistry
{
    private readonly ISystemPropertiesProvider _systemPropertiesProvider;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="systemPropertiesProvider"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public NetFrameworkVersionFromRegistry([NotNull] ISystemPropertiesProvider systemPropertiesProvider)
    {
        _systemPropertiesProvider = systemPropertiesProvider ?? throw new ArgumentNullException(nameof(systemPropertiesProvider));
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

            if (_systemPropertiesProvider.Data.NetFrameworkVersions != null)
            {
                list.AddRange(_systemPropertiesProvider.Data.NetFrameworkVersions);
            }

            return list;
        }
    }
}