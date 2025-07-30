using EvilBaschdi.Core;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public interface IHandleNetFrameworkSetupNdpSubKey : IValueFor<(string SubKeyName, RegistryKey VersionKey), string>;