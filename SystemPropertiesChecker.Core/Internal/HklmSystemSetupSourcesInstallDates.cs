using System;
using System.Collections.ObjectModel;
using System.Linq;
using SystemPropertiesChecker.Core.Models;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <inheritdoc />
    // ReSharper disable once ClassNeverInstantiated.Global
    public class HklmSystemSetupSourcesInstallDates : ISourceOsCollection
    {
        /// <inheritdoc />
        public ObservableCollection<SourceOs> Value
        {
            get
            {
                var bits = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

                var localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, bits);
                var regPath = localMachine.OpenSubKey(@"System\Setup");

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

                return new ObservableCollection<SourceOs>(list.OrderByDescending(i => i.InstallDate));
            }
        }
    }
}