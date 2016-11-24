using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;

namespace WinSPCheck.Internal
{
    /// <summary>
    /// </summary>
    public class GetOtherInformationText : IOtherInformationText
    {/// <summary>
    /// Other information text.
    /// </summary>
        public string Value
        {
            get
            {
                var psVersion = "0";
                if (PowerShell3Exists())
                {
                    psVersion = GetPowerShell3Version();
                }
                else if (PowerShell1Exists())
                {
                    psVersion = GetPowerShell1Version();
                }

                
                return $"Internet Explorer: {GetIEVersion()}{Environment.NewLine}PowerShell: {psVersion}{Environment.NewLine}Git for Windows: {GetGitVersion()}";
            }
        }

        private string GetIEVersion()
        {
            var key = @"Software\Microsoft\Internet Explorer";
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

        private string GetPowerShell3Version()
        {
            var key = @"SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine";
            var subKey = Registry.LocalMachine.OpenSubKey(key, false);
            if (subKey == null)
            {
                return "0";
            }
            var value = subKey.GetValue("PSCompatibleVersion").ToString().Split(',');
            return value.Last().Trim();
        }

        private string GetPowerShell1Version()
        {
            var key = @"SOFTWARE\Microsoft\PowerShell\1\PowerShellEngine";
            var subKey = Registry.LocalMachine.OpenSubKey(key, false);
            if (subKey == null)
            {
                return "0";
            }
            var value = subKey.GetValue("PSCompatibleVersion").ToString().Split(',');
            return value.Last().Trim();
        }

        private bool PowerShell1Exists()
        {
            var value = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\1", "Install", null).ToString();
            return value.Equals("1");
        }

        private bool PowerShell3Exists()
        {
            var value = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\3", "Install", null).ToString();
            return value.Equals("1");
        }
    }
}