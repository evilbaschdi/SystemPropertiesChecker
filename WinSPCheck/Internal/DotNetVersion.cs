using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;

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
            _dotNetVersionList = new List<string> {".NET Frameworks:"};
            // .Net 2.0, 3.0, 3.5
            // .Net 4.0
            GetNetFrameworkVersionFromRegistry();
            // .Net 4.5 and hiher
            GetNetFrameworkVersionHigher4FromRegistry();
        }

        private void GetNetFrameworkVersionFromRegistry()
        {
            // Opens the registry key for the .NET Framework entry.
            using (var ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                    OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5
                // or later, you can use:
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                if (ndpKey == null)
                {
                    return;
                }
                foreach (var versionKeyName in ndpKey.GetSubKeyNames().Where(v => v.StartsWith("v")))
                {
                    var versionKey = ndpKey.OpenSubKey(versionKeyName);

                    if (versionKey != null)
                    {
                        var name = (string) versionKey.GetValue("Version", "");
                        var sp = versionKey.GetValue("SP", "").ToString();
                        var install = versionKey.GetValue("Install", "").ToString();

                        // .Net 2.0, 3.0, 3.5
                        if (!string.IsNullOrEmpty(name))
                        {
                            _dotNetVersionList.Add(install != "" && install == "1" && sp != ""
                                ? string.Format("{0} | SP{1} | {2}", versionKeyName, sp, name)
                                : string.Format("{0} | {1}", versionKeyName, name));
                        }

                        // .Net 4.0
                        if (string.IsNullOrEmpty(name))
                        {
                            foreach (var subKeyName in versionKey.GetSubKeyNames())
                            {
                                var subKey = versionKey.OpenSubKey(subKeyName);

                                if (subKey != null)
                                {
                                    name = (string) subKey.GetValue("Version", "");
                                    if (name != "")
                                    {
                                        sp = subKey.GetValue("SP", "").ToString();
                                    }
                                    install = subKey.GetValue("Install", "").ToString();
                                }

                                if (!string.IsNullOrEmpty(install) && install == "1")
                                {
                                    _dotNetVersionList.Add(!string.IsNullOrEmpty(sp)
                                        ? string.Format("{0} | SP{1} | {2}", subKeyName, sp, name)
                                        : string.Format("{0}: {1}", subKeyName, name));
                                }
                                else
                                {
                                    _dotNetVersionList.Add(string.Format("{0} | {1}", versionKeyName, name));
                                }
                            }
                        }
                    }
                }
            }
        }

        private void GetNetFrameworkVersionHigher4FromRegistry()
        {
            using (
                var ndpKey =
                    RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                        .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey != null)
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
            foreach (var key in DotNetVersionReleaseKeyList().Where(key => key.Key >= releaseKey))
            {
                return key.Value;
            }

            // This line should never execute. A non-null release key should mean
            // that 4.5 or later is installed.
            return "No 4.5 or later version detected";
        }

        private IEnumerable<KeyValuePair<int, string>> DotNetVersionReleaseKeyList()
        {
            return new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(378389, "4.5 or later"),
                new KeyValuePair<int, string>(379675, "4.5.1 or later"),
                new KeyValuePair<int, string>(379893, "4.5.2 or later")
            };
        }
    }
}