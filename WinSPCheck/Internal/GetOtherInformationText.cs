using System;
using System.Linq;
using Microsoft.Win32;

namespace WinSPCheck.Internal
{
    /// <summary>
    /// 
    /// </summary>
    public class GetOtherInformationText : IOtherInformationText
    {
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

                return $"Internet Explorer: {GetIEVersion()}{Environment.NewLine}PowerShell: {psVersion}";
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

        private string GetPowerShell3Version()
        {
            var key = @"SOFTWARE\Microsoft\PowerShell\3\PowerShellEngine";
            var subKey = Registry.LocalMachine.OpenSubKey(key, false);
            if (subKey == null)
            {
                return "0";
            }
            var value = subKey.GetValue("PSCompatibleVersion").ToString().Split(',');
            return value.Last();
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
            return value.Last();
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