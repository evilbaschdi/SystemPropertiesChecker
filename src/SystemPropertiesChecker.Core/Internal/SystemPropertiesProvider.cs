using System.Runtime.InteropServices;
using System.Text.Json;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public class SystemPropertiesProvider : ISystemPropertiesProvider
{
    private readonly Lazy<SystemPropertiesData> _data;

    /// <summary>
    ///     Constructor
    /// </summary>
    public SystemPropertiesProvider([NotNull] IExecutePowerShellCommand executePowerShellCommand)
    {
        ArgumentNullException.ThrowIfNull(executePowerShellCommand);

        _data = new(() =>
                    {
                        var data = new SystemPropertiesData();
                        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                        {
                            return data;
                        }

                        var script = GetPowerShellScript();
                        var json = executePowerShellCommand.ValueFor(script);

                        if (!string.IsNullOrWhiteSpace(json) && !json.Trim().Equals("(none)", StringComparison.OrdinalIgnoreCase))
                        {
                            try
                            {
                                data = JsonSerializer.Deserialize<SystemPropertiesData>(json, new JsonSerializerOptions
                                                                                              {
                                                                                                  PropertyNameCaseInsensitive = true
                                                                                              }) ?? new SystemPropertiesData();
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine("JSON DESERIALIZE ERROR: " + ex.Message);
                                Console.Error.WriteLine("JSON WAS: " + json);
                            }
                        }

                        return data;
                    });
    }

    /// <inheritdoc />
    public SystemPropertiesData Data => _data.Value;

    private static string GetPowerShellScript() =>
        "$currentVersionProps = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion' -ErrorAction SilentlyContinue; " +
        "$selfHostProps = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\WindowsSelfHost\\UI\\Selection' -ErrorAction SilentlyContinue; " +
        "$ieProps = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\Software\\Microsoft\\Internet Explorer' -ErrorAction SilentlyContinue; " +
        "$ps3Props = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\PowerShell\\3\\PowerShellEngine' -ErrorAction SilentlyContinue; " +
        "$ps3InstallProps = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\PowerShell\\3' -ErrorAction SilentlyContinue; " +
        "$ps1Props = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\PowerShell\\1\\PowerShellEngine' -ErrorAction SilentlyContinue; " +
        "$ps1InstallProps = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\PowerShell\\1' -ErrorAction SilentlyContinue; " +
        "$edgeProps = Get-ItemProperty -Path 'Registry::HKEY_CURRENT_USER\\SOFTWARE\\Classes\\Local Settings\\Software\\Microsoft\\Windows\\CurrentVersion\\AppModel\\SystemAppData\\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\\Schemas' -ErrorAction SilentlyContinue; " +
        "$ndpProps = Get-ItemProperty -Path 'Registry::HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP\\v4\\Full\\' -ErrorAction SilentlyContinue; " +
        "$res = [PSCustomObject]@{ " +
        "CimOperatingSystemInstallDate = (Get-CimInstance Win32_OperatingSystem -ErrorAction SilentlyContinue).InstallDate.ToString('yyyy-MM-dd HH:mm:ss'); " +
        "CimOperatingSystemCaption = (Get-CimInstance Win32_OperatingSystem -ErrorAction SilentlyContinue).Caption; " +
        "CimComputerSystemManufacturer = (Get-CimInstance Win32_ComputerSystem -ErrorAction SilentlyContinue).Manufacturer; " +
        "CimBaseBoardManufacturer = (Get-CimInstance Win32_BaseBoard -ErrorAction SilentlyContinue).Manufacturer; " +
        "CimBaseBoardProduct = (Get-CimInstance Win32_BaseBoard -ErrorAction SilentlyContinue).Product; " +
        "RegistryWindowsNtCurrentVersion = @{ " +
        "CurrentVersion = \"$($currentVersionProps.CurrentVersion)\"; " +
        "CurrentMajorVersionNumber = \"$($currentVersionProps.CurrentMajorVersionNumber)\"; " +
        "CurrentMinorVersionNumber = \"$($currentVersionProps.CurrentMinorVersionNumber)\"; " +
        "CSDVersion = \"$($currentVersionProps.CSDVersion)\"; " +
        "BuildLab = \"$($currentVersionProps.BuildLab)\"; " +
        "BuildLabEx = \"$($currentVersionProps.BuildLabEx)\"; " +
        "CurrentBuild = \"$($currentVersionProps.CurrentBuild)\"; " +
        "ProductName = \"$($currentVersionProps.ProductName)\"; " +
        "ReleaseId = \"$($currentVersionProps.ReleaseId)\"; " +
        "DisplayVersion = \"$($currentVersionProps.DisplayVersion)\"; " +
        "UBR = \"$($currentVersionProps.UBR)\" " +
        "}; " +
        "RegistryWindowsSelfHostUiSelection = @{ " +
        "UIBranch = \"$($selfHostProps.UIBranch)\"; " +
        "UIContentType = \"$($selfHostProps.UIContentType)\"; " +
        "UIRing = \"$($selfHostProps.UIRing)\" " +
        "}; " +
        "RegistryInternetExplorerVersion = \"$($ieProps.svcVersion)\"; " +
        "RegistryPowerShellStatus = @{ " +
        "PS3Install = \"$($ps3InstallProps.Install)\"; " +
        "PS3CompatibleVersion = \"$($ps3Props.PSCompatibleVersion)\"; " +
        "PS1Install = \"$($ps1InstallProps.Install)\"; " +
        "PS1CompatibleVersion = \"$($ps1Props.PSCompatibleVersion)\" " +
        "}; " +
        "RegistryEdgePackageFullName = \"$($edgeProps.PackageFullName)\"; " +
        "Browsers = @( " +
        "$paths = @('HKLM:\\SOFTWARE\\WOW6432Node\\Clients\\StartMenuInternet', 'HKLM:\\SOFTWARE\\Clients\\StartMenuInternet'); " +
        "$parentPath = $paths | Where-Object { Test-Path $_ } | Select-Object -First 1; " +
        "if ($parentPath) { " +
        "Get-ChildItem -Path $parentPath -ErrorAction SilentlyContinue | ForEach-Object { " +
        "$name = (Get-ItemProperty -Path $_.PSPath).'(default)'; " +
        "$cmdKey = Join-Path $_.PSPath 'shell\\open\\command'; " +
        "$cmd = (Get-ItemProperty -Path $cmdKey -ErrorAction SilentlyContinue).'(default)'; " +
        "if ($cmd) { " +
        "$path = $cmd.Replace('\"', ''); " +
        "$version = (Get-Item -Path $path -ErrorAction SilentlyContinue).VersionInfo.FileVersion; " +
        "[PSCustomObject]@{ Name = \"$($name)\"; Path = \"$($path)\"; Version = \"$($version)\" } " +
        "} " +
        "} " +
        "} " +
        "); " +
        "NetFrameworkVersions = @( " +
        "Get-ChildItem -Path 'HKLM:\\SOFTWARE\\Microsoft\\NET Framework Setup\\NDP' -ErrorAction SilentlyContinue | Where-Object { $_.PSChildName -like 'v*' } | ForEach-Object { " +
        "$v = (Get-ItemProperty -Path $_.PSPath -ErrorAction SilentlyContinue); " +
        "if ($v.Version) { " +
        "if ($v.Install -eq 1 -and $v.SP) { " +
        "\"$($_.PSChildName) | SP$($v.SP) | $($v.Version)\" " +
        "} else { " +
        "\"$($_.PSChildName) | $($v.Version)\" " +
        "} " +
        "} else { " +
        "Get-ChildItem -Path $_.PSPath -ErrorAction SilentlyContinue | ForEach-Object { " +
        "$sub = (Get-ItemProperty -Path $_.PSPath -ErrorAction SilentlyContinue); " +
        "if ($sub.Version -and $sub.Install -eq 1) { " +
        "if ($sub.SP) { " +
        "\"$($_.PSChildName) | SP$($sub.SP) | $($sub.Version)\" " +
        "} else { " +
        "\"$($_.PSChildName): $($sub.Version)\" " +
        "} " +
        "} " +
        "} " +
        "} " +
        "} " +
        "); " +
        "NetFrameworkReleaseKey = \"$($ndpProps.Release)\"; " +
        "SourceOsHistory = @( " +
        "Get-ChildItem -Path 'HKLM:\\System\\Setup' -ErrorAction SilentlyContinue | Where-Object { $_.PSChildName -like 'Source*' } | ForEach-Object { " +
        "$path = $_.PSPath; " +
        "$props = Get-ItemProperty -Path $path -ErrorAction SilentlyContinue; " +
        "[PSCustomObject]@{ " +
        "ProductName = \"$($props.ProductName)\"; " +
        "ReleaseId = \"$($props.ReleaseId)\"; " +
        "Build = \"$($props.CurrentBuild)\"; " +
        "InstallDate = $props.InstallDate " +
        "} " +
        "} " +
        ") " +
        "}; " +
        "$res | ConvertTo-Json -Depth 5";
}