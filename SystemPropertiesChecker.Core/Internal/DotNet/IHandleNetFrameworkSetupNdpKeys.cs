using EvilBaschdi.Core;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public interface IHandleNetFrameworkSetupNdpKeys : IValueFor<(string VersionKeyName, RegistryKey NdpKey), List<string>>;