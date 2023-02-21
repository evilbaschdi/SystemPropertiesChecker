using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using EvilBaschdi.Core;
using Microsoft.Win32;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc cref="ISourceOsCollection" />
// ReSharper disable once ClassNeverInstantiated.Global
public class HklmSystemSetupSourcesInstallDates : CachedValue<ObservableCollection<SourceOs>>, ISourceOsCollection
{
    /// <inheritdoc />
    [SupportedOSPlatform("windows")]
    protected override ObservableCollection<SourceOs> NonCachedValue
    {
        get
        {
            if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                return new(new());
            }

            var bits = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

            using var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, bits);
            using var regPath = localMachine.OpenSubKey(@"System\Setup");

            var list = (from source in regPath?.GetSubKeyNames().Where(name => name.StartsWith("Source"))
                        select regPath?.OpenSubKey(source)
                        into currentSubKey
                        where currentSubKey != null
                        select new SourceOs
                               {
                                   ProductName = currentSubKey.GetValue("ProductName")?.ToString(),
                                   ReleaseId = currentSubKey.GetValue("ReleaseId")?.ToString(),
                                   Build = currentSubKey.GetValue("CurrentBuild")?.ToString(),
                                   InstallDate = new DateTime(1970, 1, 1).AddSeconds(Convert.ToDouble(currentSubKey.GetValue("InstallDate")?.ToString()))
                               }).ToList();

            return new(list.OrderByDescending(i => i.InstallDate));
        }
    }
}