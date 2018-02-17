using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    ///     Class that returns a list of current installed dot net versions.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DotNetVersion : IDotNetVersion
    {
        private readonly IDotNetVersionReleaseKeyMappingList _dotNetVersionReleaseKeyMappingList;
        private List<string> _dotNetVersionList;

        /// <summary>
        ///     Constructor.
        /// </summary>
        public DotNetVersion(IDotNetVersionReleaseKeyMappingList dotNetVersionReleaseKeyMappingList)
        {
            _dotNetVersionReleaseKeyMappingList = dotNetVersionReleaseKeyMappingList ?? throw new ArgumentNullException(nameof(dotNetVersionReleaseKeyMappingList));
            GetNetFrameworks();
        }


        /// <summary>
        ///     Contains a list of current installed dot net versions.
        /// </summary>
        public List<string> Value => _dotNetVersionList;

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
            using (var ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
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
                    // ReSharper disable once AccessToDisposedClosure
                    var versionKey = ndpKey.OpenSubKey(versionKeyName);

                    if (versionKey != null)
                    {
                        var name = (string) versionKey.GetValue("Version", "");
                        var sp = versionKey.GetValue("SP", "").ToString();
                        var install = versionKey.GetValue("Install", "").ToString();

                        // .Net 2.0, 3.0, 3.5
                        if (!string.IsNullOrEmpty(name))
                        {
                            _dotNetVersionList.Add(install != "" && install.Equals("1") && sp != ""
                                ? $"{versionKeyName} | SP{sp} | {name}"
                                : $"{versionKeyName} | {name}");
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

                                if (!string.IsNullOrEmpty(install) && install.Equals("1"))
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
            using (
                var ndpKey =
                    RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "")
                               .OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full\"))
            {
                if (ndpKey == null)
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
            if (releaseKey <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(releaseKey));
            }

            //releaseKey = 460900;
            var value = _dotNetVersionReleaseKeyMappingList.ValueFor(releaseKey);
            return !string.IsNullOrWhiteSpace(value) ? $"{value} (Release key: '{releaseKey}')" : "No 4.5 or later version detected";
        }
    }
}