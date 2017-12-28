using System;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    ///     Class that provides RegisryValues from WindowsNT CurrentVersion.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HklmSoftwareMicrosoftWindowsNtCurrentVersion : IRegistryValue
    {
        /// <summary>
        ///     Contains a string providing a registry value.
        /// </summary>
        /// <param name="name">Name of the RegistryKey</param>
        /// <returns></returns>
        public string ValueFor(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }

            var bits = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

            var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, bits);
            var regPath = localMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");


            return regPath?.GetValue(name) != null
                ? regPath.GetValue(name).ToString()
                : string.Empty;
        }
    }
}