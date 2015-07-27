using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace WinSPCheck.Internal
{
    public class DotNetVersion
    {
        public List<string> DotNetVersionList;
        private List<string> _dotNetVersionList;

        public DotNetVersion()
        {
            GetNetFrameworks();
            DotNetVersionList = _dotNetVersionList;
        }

        private void GetNetFrameworks()
        {
            _dotNetVersionList = new List<string>
            {
                ".Net Frameworks:"
            };
            // .Net 2.0, 3.0, 3.5
            // .Net 4.0
            GetNetFrameworkVersionFromRegistry();
            // .Net 4.5 and higher
            GetNetFrameworkVersionHigher4FromRegistry();
        }

        private void GetNetFrameworkVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using(var ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                    OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                if(ndpKey == null)
                {
                    return;
                }
                foreach(var versionKeyName in ndpKey.GetSubKeyNames().Where(v => v.StartsWith("v")))
                {
                    var versionKey = ndpKey.OpenSubKey(versionKeyName);

                    if(versionKey != null)
                    {
                        var name = (string) versionKey.GetValue("Version", "");
                        var sp = versionKey.GetValue("SP", "").ToString();
                        var install = versionKey.GetValue("Install", "").ToString();

                        // .Net 2.0, 3.0, 3.5
                        if(!string.IsNullOrEmpty(name))
                        {
                            _dotNetVersionList.Add(install != "" && install == "1" && sp != ""
                                ? $"{versionKeyName} | SP{sp} | {name}"
                                : $"{versionKeyName} | {name}");
                        }

                        // .Net 4.0
                        if(string.IsNullOrEmpty(name))
                        {
                            foreach(var subKeyName in versionKey.GetSubKeyNames())
                            {
                                var subKey = versionKey.OpenSubKey(subKeyName);

                                if(subKey != null)
                                {
                                    name = (string) subKey.GetValue("Version", "");
                                    if(name != "")
                                    {
                                        sp = subKey.GetValue("SP", "").ToString();
                                    }
                                    install = subKey.GetValue("Install", "").ToString();
                                }

                                if(!string.IsNullOrEmpty(install) && install == "1")
                                {
                                    _dotNetVersionList.Add(!string.IsNullOrEmpty(sp)
                                        ? $"{subKeyName} | SP{sp} | {name}"
                                        : $"{subKeyName}: {name}");
                                }
                                else
                                {
                                    _dotNetVersionList.Add($"{versionKeyName} | {name}");
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetNetFrameworkVersionHigher4FromRegistry()
        {
            using(
                var ndpKey =
                    RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                        .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if(ndpKey != null)
                {
                    var releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                    _dotNetVersionList.Add(CheckFor45DotVersion(releaseKey));
                }
            }
        }

        // Checking the version using >= will enable forward compatibility,
        // however you should always compile your code on newer versions of
        // the framework to ensure your app works the same.
        private string CheckFor45DotVersion(int releaseKey)
        {
            foreach(
                var key in
                    DotNetVersionReleaseKeyList().OrderByDescending(key => key.Key).Where(key => releaseKey >= key.Key))
            {
                return key.Value;
            }

            // This line should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }

        private IEnumerable<KeyValuePair<int, string>> DotNetVersionReleaseKeyList()
            => new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(378389, ".NET Framework 4.5"),
                new KeyValuePair<int, string>(378675,
                    ".Net Framework 4.5.1 installed with Windows 8.1 or Windows Server 2012 R2"),
                new KeyValuePair<int, string>(378758,
                    ".Net Framework 4.5.1 installed on Windows 8, Windows 7 SP1, or Windows Vista SP2"),
                new KeyValuePair<int, string>(379893, ".Net Framework 4.5.2"),
                new KeyValuePair<int, string>(381029, ".Net Framework 4.6 Preview"),
                new KeyValuePair<int, string>(393273, ".Net Framework 4.6 RC"),
                new KeyValuePair<int, string>(393295, ".Net Framework 4.6 or later")
            };
    }
}