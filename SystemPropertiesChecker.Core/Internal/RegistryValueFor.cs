using System;
using System.ComponentModel;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <inheritdoc />
    public abstract class RegistryValueFor : IRegistryValueFor
    {
        private readonly RegistryHive _registryHive;
        private readonly string _subKey;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="subKey"></param>
        /// <param name="registryHive"></param>
        protected RegistryValueFor([NotNull] string subKey, RegistryHive registryHive)
        {
            if (!Enum.IsDefined(typeof(RegistryHive), registryHive))
            {
                throw new InvalidEnumArgumentException(nameof(registryHive), (int) registryHive, typeof(RegistryHive));
            }

            _subKey = subKey ?? throw new ArgumentNullException(nameof(subKey));
            _registryHive = registryHive;
        }

        /// <inheritdoc />
        public string ValueFor([NotNull] string value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (!OperatingSystem.IsWindows())
            {
                return string.Empty;
            }

            var bits = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

            var localMachine = RegistryKey.OpenBaseKey(_registryHive, bits);
            var regPath = localMachine.OpenSubKey(_subKey);

            return regPath?.GetValue(value) != null
                ? regPath.GetValue(value)?.ToString()
                : string.Empty;
        }
    }
}