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
    private readonly WindowsVersionInformationModel _windowsVersionInformationModel = new();
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

            var bits = Bits();
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
                _windowsVersionInformationModel.Domain = domain;
                var passwordExpirationDate = _passwordExpirationDate.ValueFor(domain);
                _windowsVersionInformationModel.UserName = passwordExpirationDate.UserName;
                _windowsVersionInformationModel.PasswordExpirationDate = passwordExpirationDate.DateString;
            }

            _windowsVersionInformationModel.Computername = Environment.MachineName;
            _windowsVersionInformationModel.Bits = bits;
            _windowsVersionInformationModel.Manufacturer = ManufacturerByWin32ComputerSystem();
            _windowsVersionInformationModel.ManufacturerProduct = ManufacturerByWin32ComputerSystem().Equals(ManufacturerByWin32BaseBoard().Key)
                ? ManufacturerByWin32BaseBoard().Value
                : string.Empty;

            _windowsVersionInformationModel.InsiderChannel = _insiderChannel.Value;
            _windowsVersionInformationModel.BuildLab = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("BuildLab");
            _windowsVersionInformationModel.BuildLabEx = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("BuildLabEx");
            _windowsVersionInformationModel.BuildLabExArray = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("BuildLabEx").Split('.');
            _windowsVersionInformationModel.CurrentBuild = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("CurrentBuild");
            _windowsVersionInformationModel.ProductName = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("ProductName");
            _windowsVersionInformationModel.CurrentVersion = version;
            _windowsVersionInformationModel.CsdVersion = csdVersion;
            _windowsVersionInformationModel.ReleaseId = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("ReleaseId");
            _windowsVersionInformationModel.DisplayVersion = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("DisplayVersion");
            _windowsVersionInformationModel.Ubr = _localMachineSoftwareMicrosoftWindowsNtCurrentVersion.ValueFor("UBR");
            _windowsVersionInformationModel.WindowsFeatureExperiencePackVersion = _windowsFeatureExperiencePackVersion.Value;
            _windowsVersionInformationModel.InstallDate = InstallDate();
            _windowsVersionInformationModel.Caption = Caption();
            _cachedWindowsVersionInformationModel = _windowsVersionInformationModel;
            return _cachedWindowsVersionInformationModel;
        }
    }

    private static string Bits()
    {
        return Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
    }

    private static string ManufacturerByWin32ComputerSystem()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        const string win32ComputerSystem = "SELECT * FROM Win32_ComputerSystem";
        var managementObjectSearcher = new ManagementObjectSearcher(win32ComputerSystem);
        var info = managementObjectSearcher.Get();

        foreach (var item in info)
        {
            return item["Manufacturer"].ToString();
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
        var managementObjectSearcher = new ManagementObjectSearcher(win32ComputerSystem);
        var info = managementObjectSearcher.Get();

        foreach (var item in info)
        {
            return new(item["Manufacturer"].ToString(), item["Product"].ToString());
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
        var managementObjectSearcher = new ManagementObjectSearcher(win32OperatingSystem);
        var info = managementObjectSearcher.Get();
        var installDate = string.Empty;
        foreach (var item in info)
        {
            installDate = item["InstallDate"].ToString();
            break;
        }

        return ManagementDateTimeConverter.ToDateTime(installDate ?? string.Empty).ToString("yyyy-MM-dd HH:mm:ss");
    }

    private static string Caption()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        const string win32OperatingSystem = "SELECT * FROM Win32_OperatingSystem";
        var managementObjectSearcher = new ManagementObjectSearcher(win32OperatingSystem);
        var info = managementObjectSearcher.Get();
        var caption = string.Empty;
        foreach (var item in info)
        {
            caption = item["Caption"].ToString();
            break;
        }

        return caption;
    }
}