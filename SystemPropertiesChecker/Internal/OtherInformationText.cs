using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class OtherInformationText : IOtherInformationText
    {
        /// <summary>
        ///     Other information text.
        /// </summary>
        public string Value
        {
            get
            {
                var psVersion = "0";
                if (PowerShellExists(3))
                {
                    psVersion = GetPowerShellVersion(3);
                }
                else if (PowerShellExists(1))
                {
                    psVersion = GetPowerShellVersion(1);
                }


                return $"Internet Explorer: {GetIEVersion()}{Environment.NewLine}PowerShell: {psVersion}{Environment.NewLine}Git for Windows: {GetGitVersion()}";
            }
        }

        private string GetIEVersion()
        {
            const string key = @"Software\Microsoft\Internet Explorer";
            var subKey = Registry.LocalMachine.OpenSubKey(key, false);
            if (subKey == null)
            {
                return "0";
            }
            var value = subKey.GetValue("svcVersion").ToString();
            return value;
        }

        private string GetGitVersion()
        {
            var programFiles = Directory.Exists(@"C:\Program Files\Git\bin")
                ? @"C:\Program Files\Git\bin"
                : Directory.Exists(@"C:\Program Files (x86)\Git\bin")
                    ? @"C:\Program Files (x86)\Git\bin"
                    : string.Empty;

            if (string.IsNullOrWhiteSpace(programFiles))
            {
                return "(not found)";
            }
            var versInfo = FileVersionInfo.GetVersionInfo(Path.Combine(programFiles, "git.exe"));
            return $"{versInfo.FileMajorPart}.{versInfo.FileMinorPart}.{versInfo.FileBuildPart}.{versInfo.FilePrivatePart}";
        }

        private string GetPowerShellVersion(int version)
        {
            var key = $@"SOFTWARE\Microsoft\PowerShell\{version}\PowerShellEngine";
            var subKey = Registry.LocalMachine.OpenSubKey(key, false);
            if (subKey == null)
            {
                return "0";
            }
            var value = subKey.GetValue("PSCompatibleVersion").ToString().Split(',');
            return value.Last().Trim();
        }

        private bool PowerShellExists(int version)
        {
            var value = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\{version}", "Install", null)?.ToString();
            return !string.IsNullOrWhiteSpace(value) && value.Equals("1");
        }
    }
}