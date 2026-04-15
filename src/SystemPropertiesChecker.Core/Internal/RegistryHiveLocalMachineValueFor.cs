using JetBrains.Annotations;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
///     Interface for classes that provide RegistryValues.
/// </summary>
public abstract class RegistryHiveLocalMachineValueFor : RegistryValueFor, IRegistryHiveLocalMachineValueFor
{
    /// <summary>
    ///     Constructor
    /// </summary>
    protected RegistryHiveLocalMachineValueFor([NotNull] string subKey)
#pragma warning disable CA1416 // Validate platform compatibility
        : base(subKey, RegistryHive.LocalMachine)
#pragma warning restore CA1416 // Validate platform compatibility
    {
    }
}