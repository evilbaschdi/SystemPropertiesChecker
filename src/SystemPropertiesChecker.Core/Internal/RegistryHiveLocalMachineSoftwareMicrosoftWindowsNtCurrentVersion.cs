using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
///     Class that provides RegistryValues from WindowsNT CurrentVersion.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion : RegistryHiveLocalMachineValueFor,
                                                                                IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion([NotNull] ISystemPropertiesProvider systemPropertiesProvider)
        : base(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion", systemPropertiesProvider)
    {
    }
}