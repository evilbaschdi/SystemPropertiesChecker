using System.Management;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
///     Class that provides values about the current windows version.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class WindowsVersionInformation : IWindowsVersionInformation
{
    private readonly IInsiderChannel _insiderChannel;
    private readonly IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion _localMachineSoftwareMicrosoftWindowsNtCurrentVersion;
    private readonly IPasswordExpirationDate _passwordExpirationDate;
    private readonly IWindowsFeatureExperiencePackVersion _windowsFeatureExperiencePackVersion;
    private WindowsVersionInformationModel _cachedWindowsVersionInformationModel;

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    ///     <paramref name="localMachineSoftwareMicrosoftWindowsNtCurrentVersion" /> is <see langword="null" />.
    /// </exception>
    public WindowsVersionInformation([NotNull] IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion localMachineSoftwareMicrosoftWindowsNtCurrentVersion,
                                     [NotNull] IInsiderChannel insiderChannel,
                                     [NotNull] IPasswordExpirationDate passwordExpirationDate,
                                     [NotNull] IWindowsFeatureExperiencePackVersion windowsFeatureExperiencePackVersion)
    {
        _localMachineSoftwareMicrosoftWindowsNtCurrentVersion = localMachineSoftwareMicrosoftWindowsNtCurrentVersion ??
                                                                throw new ArgumentNullException(nameof(localMachineSoftwareMicrosoftWindowsNtCurrentVersion));
        _insiderChannel = insiderChannel ?? throw new ArgumentNullException(nameof(insiderChannel));

        _passwordExpirationDate = passwordExpirationDate ?? throw new ArgumentNullException(nameof(passwordExpirationDate));
        _windowsFeatureExperiencePackVersion = windowsFeatureExperiencePackVersion ?? throw new ArgumentNullException(nameof(windowsFeatureExperiencePackVersion));
    }

    /// <summary>
    ///     Contains WindowsVersionInformation values.
    /// </summary>
    public WindowsVersionInformationModel Value
    {
        get
        {
            if (_cachedWindowsVersionInformationModel != null)
            {
                return _cachedWindowsVersionInformationModel;
            }

            var architecture = Architecture();
            var domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

            var currentVersion = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CurrentVersion");
            var currentMajorVersionNumber = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CurrentMajorVersionNumber");
            var currentMinorVersionNumber = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CurrentMinorVersionNumber");

            var csdVersion = !string.IsNullOrEmpty(_localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CSDVersion"))
                ? $" with {_localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CSDVersion")}"
                : string.Empty;

            var version = !string.IsNullOrWhiteSpace(currentMajorVersionNumber) &&
                          !string.IsNullOrWhiteSpace(currentMinorVersionNumber)
                ? $"{currentMajorVersionNumber}.{currentMinorVersionNumber}"
                : currentVersion;

            if (!string.IsNullOrWhiteSpace(domain))
            {
                field.Domain = domain;
                var passwordExpirationDate = _passwordExpirationDate.ValueFor(domain);
                field.UserName = passwordExpirationDate.UserName;
                field.PasswordExpirationDate = passwordExpirationDate.DateString;
            }

            field.ComputerName = Environment.MachineName;
            field.Architecture = architecture;
            field.Manufacturer = ManufacturerByWin32ComputerSystem();
            field.ManufacturerProduct = ManufacturerByWin32ComputerSystem().Equals(ManufacturerByWin32BaseBoard().Key)
                ? ManufacturerByWin32BaseBoard().Value
                : string.Empty;

            field.InsiderChannel = _insiderChannel.Value;
            field.BuildLab = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("BuildLab");
            field.BuildLabEx = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("BuildLabEx");
            field.BuildLabExList = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("BuildLabEx")?.Split('.').ToList();
            field.CurrentBuild = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CurrentBuild");
            field.ProductName = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("ProductName");
            field.CurrentVersion = version;
            field.CsdVersion = csdVersion;
            field.ReleaseId = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("ReleaseId");
            field.DisplayVersion = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("DisplayVersion");
            field.Ubr = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("UBR");
            field.WindowsFeatureExperiencePackVersion = _windowsFeatureExperiencePackVersion.Value;
            field.InstallDate = InstallDate();
            field.Caption = Caption();
            _cachedWindowsVersionInformationModel = field;
            return _cachedWindowsVersionInformationModel;
        }
    } = new();

    private static string Architecture() => Enum.GetName(RuntimeInformation.OSArchitecture);

    private static string ManufacturerByWin32ComputerSystem()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        const string win32ComputerSystem = "SELECT * FROM Win32_ComputerSystem";

        try
        {
            using var managementObjectSearcher = new ManagementObjectSearcher(win32ComputerSystem);
            var info = managementObjectSearcher.Get();

            foreach (var item in info)
            {
                return item["Manufacturer"].ToString();
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return string.Empty;
    }

    private static KeyValuePair<string, string> ManufacturerByWin32BaseBoard()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new("(supported on windows only)", "(supported on windows only)");
        }

        const string win32ComputerSystem = "SELECT * FROM Win32_BaseBoard";

        try
        {
            using var managementObjectSearcher = new ManagementObjectSearcher(win32ComputerSystem);
            var info = managementObjectSearcher.Get();

            foreach (var item in info)
            {
                return new(item["Manufacturer"].ToString(), item["Product"].ToString());
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return new();
    }

    private static string InstallDate()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        const string win32OperatingSystem = "SELECT * FROM Win32_OperatingSystem";
        try
        {
            using var managementObjectSearcher = new ManagementObjectSearcher(win32OperatingSystem);
            var info = managementObjectSearcher.Get();
            var installDate = string.Empty;
            foreach (var item in info)
            {
                installDate = item["InstallDate"].ToString();
                break;
            }

            return ManagementDateTimeConverter.ToDateTime(installDate ?? string.Empty).ToString("yyyy-MM-dd HH:mm:ss");
        }
        catch (Exception)
        {
            // ignored
        }

        return string.Empty;
    }

    private static string Caption()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        const string win32OperatingSystem = "SELECT * FROM Win32_OperatingSystem";
        var caption = string.Empty;
        try
        {
            using var managementObjectSearcher = new ManagementObjectSearcher(win32OperatingSystem);
            var info = managementObjectSearcher.Get();

            foreach (var item in info)
            {
                caption = item["Caption"].ToString();
                break;
            }
        }
        catch (Exception)
        {
            // ignored
        }

        return caption;
    }
}