using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
///     Data transfer object representing all system properties gathered via PowerShell.
/// </summary>
public class SystemPropertiesData
{
    /// <summary>
    /// </summary>
    public string CimOperatingSystemInstallDate { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string CimOperatingSystemCaption { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string CimComputerSystemManufacturer { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string CimBaseBoardManufacturer { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string CimBaseBoardProduct { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Dictionary<string, string> RegistryWindowsNtCurrentVersion { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// </summary>
    public Dictionary<string, string> RegistryWindowsSelfHostUiSelection { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// </summary>
    public string RegistryInternetExplorerVersion { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public Dictionary<string, string> RegistryPowerShellStatus { get; set; } = new(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// </summary>
    public string RegistryEdgePackageFullName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public List<Browser> Browsers { get; set; } = new();

    /// <summary>
    /// </summary>
    public List<string> NetFrameworkVersions { get; set; } = new();

    /// <summary>
    /// </summary>
    public string NetFrameworkReleaseKey { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public List<SourceOsData> SourceOsHistory { get; set; } = new();
}

/// <summary>
/// </summary>
public class SourceOsData
{
    /// <summary>
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string ReleaseId { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public string Build { get; set; } = string.Empty;

    /// <summary>
    /// </summary>
    public double? InstallDate { get; set; }
}