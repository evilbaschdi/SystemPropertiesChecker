using System.Collections.ObjectModel;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public class HklmSystemSetupSourcesInstallDates : CachedValue<ObservableCollection<SourceOs>>, ISourceOsCollection
{
    private readonly ISystemPropertiesProvider _systemPropertiesProvider;

    /// <summary>
    ///     Constructor
    /// </summary>
    public HklmSystemSetupSourcesInstallDates([NotNull] ISystemPropertiesProvider systemPropertiesProvider)
    {
        _systemPropertiesProvider = systemPropertiesProvider ?? throw new ArgumentNullException(nameof(systemPropertiesProvider));
    }

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

            var historyData = _systemPropertiesProvider.Data.SourceOsHistory;
            if (historyData == null)
            {
                return new(new());
            }

            var list = historyData.Select(s => new SourceOs
            {
                ProductName = s.ProductName,
                ReleaseId = s.ReleaseId,
                Build = s.Build,
                InstallDate = s.InstallDate.HasValue ? new DateTime(1970, 1, 1).AddSeconds(s.InstallDate.Value) : new DateTime(1970, 1, 1)
            }).ToList();

            return new(list.OrderByDescending(i => i.InstallDate).ToList());
        }
    }
}