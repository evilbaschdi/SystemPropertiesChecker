using JetBrains.Annotations;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public abstract class RegistryHiveLocalMachineValueFor : RegistryValueFor, IRegistryHiveLocalMachineValueFor
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        protected RegistryHiveLocalMachineValueFor([NotNull] string subKey)
            : base(subKey, RegistryHive.LocalMachine)
        {
        }
    }
}