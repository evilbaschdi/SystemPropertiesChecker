using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that returns a list of current installed dot net versions.
    /// </summary>
    public class DotNetVersion : IDotNetVersion
    {
        private List<string> _dotNetVersionList;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public DotNetVersion()
        {
            GetNetFrameworks();
        }

        /// <summary>
        ///     Contains a list of current installed dot net versions.
        /// </summary>
        public List<string> List => _dotNetVersionList;

        private void GetNetFrameworks()
        {
            _dotNetVersionList = new List<string>
            {
                "currently installed versions:"
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
                Parallel.ForEach(ndpKey.GetSubKeyNames().Where(v => v.StartsWith("v")), versionKeyName =>
                {
                    // ReSharper disable once AccessToDisposedClosure
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
                            Parallel.ForEach(versionKey.GetSubKeyNames(), subKeyName =>
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
                            });
                        }
                    }
                });
            }
        }

        private void GetNetFrameworkVersionHigher4FromRegistry()
        {
            using(
                var ndpKey =
                    RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                        .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if(ndpKey == null)
                {
                    return;
                }
                var releaseKey = Convert.ToInt32(ndpKey.GetValue("Release"));
                _dotNetVersionList.Add(CheckFor45DotVersion(releaseKey));
            }
        }

        // Checking the version using >= will enable forward compatibility,
        // however you should always compile your code on newer versions of
        // the framework to ensure your app works the same.
        private string CheckFor45DotVersion(int releaseKey)
        {
            var value = DotNetVersionReleaseKeyList().OrderByDescending(key => key.Key).FirstOrDefault(key => releaseKey >= key.Key).Value;
            return !string.IsNullOrWhiteSpace(value) ? value : "No 4.5 or later version detected";
        }

        private IEnumerable<KeyValuePair<int, string>> DotNetVersionReleaseKeyList()
            => new List<KeyValuePair<int, string>>
            {
                new KeyValuePair<int, string>(378389, ".NET Framework 4.5"),
                new KeyValuePair<int, string>(378675, ".Net Framework 4.5.1"),
                new KeyValuePair<int, string>(378758, ".Net Framework 4.5.1"),
                new KeyValuePair<int, string>(379893, ".Net Framework 4.5.2"),
                new KeyValuePair<int, string>(381029, ".Net Framework 4.6 Preview"),
                new KeyValuePair<int, string>(393273, ".Net Framework 4.6 RC"),
                new KeyValuePair<int, string>(393295, ".Net Framework 4.6"),
                new KeyValuePair<int, string>(394254, ".Net Framework 4.6.1"),
                new KeyValuePair<int, string>(394271, ".Net Framework 4.6.1"),
                new KeyValuePair<int, string>(394748, ".Net Framework 4.6.2 Preview")
            };
    }
}
