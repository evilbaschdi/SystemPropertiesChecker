using System;
using Microsoft.Win32;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that provides RegisryValues from WindowsNT CurrentVersion.
    /// </summary>
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
            var regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            return regPath?.GetValue(name) != null
                ? regPath.GetValue(name).ToString()
                : string.Empty;
        }
    }
}