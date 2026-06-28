using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
///     Class that provides RegistryValues from WindowsNT CurrentVersion.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection : RegistryHiveLocalMachineValueFor,
                                                                                   IRegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection([NotNull] ISystemPropertiesProvider systemPropertiesProvider)
        : base(@"SOFTWARE\Microsoft\WindowsSelfHost\UI\Selection", systemPropertiesProvider)
    {
    }
}