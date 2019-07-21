using System;
using System.Collections.Generic;
using System.Management;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <summary>
    ///     Class that provides values about the current windows version.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WindowsVersionInformation : IWindowsVersionInformation
    {
        private readonly IPasswordExpirationDate _passwordExpirationDate;
        private readonly IRegistryValueFor _registryValueFor;
        private readonly WindowsVersionInformationModel _windowsVersionInformationModel = new WindowsVersionInformationModel();
        private WindowsVersionInformationModel _cachedWindowsVersionInformationModel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="registryValueFor" /> is <see langword="null" />.
        /// </exception>
        public WindowsVersionInformation(IRegistryValueFor registryValueFor, IPasswordExpirationDate passwordExpirationDate)
        {
            _registryValueFor = registryValueFor ?? throw new ArgumentNullException(nameof(registryValueFor));
            _passwordExpirationDate = passwordExpirationDate ?? throw new ArgumentNullException(nameof(passwordExpirationDate));
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

                var currentVersion = _registryValueFor.ValueFor("CurrentVersion");
                var currentMajorVersionNumber = _registryValueFor.ValueFor("CurrentMajorVersionNumber");
                var currentMinorVersionNumber = _registryValueFor.ValueFor("CurrentMinorVersionNumber");

                var csdVersion = !string.IsNullOrEmpty(_registryValueFor.ValueFor("CSDVersion"))
                    ? $" with {_registryValueFor.ValueFor("CSDVersion")}"
                    : string.Empty;

                var releaseId = !string.IsNullOrEmpty(_registryValueFor.ValueFor("ReleaseId"))
                    ? $" (Release {_registryValueFor.ValueFor("ReleaseId")})"
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


                _windowsVersionInformationModel.BuildLab = _registryValueFor.ValueFor("BuildLab");
                _windowsVersionInformationModel.BuildLabEx = _registryValueFor.ValueFor("BuildLabEx");
                _windowsVersionInformationModel.BuildLabExArray = _registryValueFor.ValueFor("BuildLabEx").Split('.');
                _windowsVersionInformationModel.CurrentBuild = _registryValueFor.ValueFor("CurrentBuild");
                _windowsVersionInformationModel.ProductName = _registryValueFor.ValueFor("ProductName");
                _windowsVersionInformationModel.CurrentVersion = version;
                _windowsVersionInformationModel.CsdVersion = csdVersion;
                _windowsVersionInformationModel.ReleaseId = releaseId;
                _windowsVersionInformationModel.Ubr = _registryValueFor.ValueFor("UBR");
                _windowsVersionInformationModel.InstallDate = InstallDate();
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
            const string win32ComputerSystem = "SELECT * FROM Win32_BaseBoard";
            var managementObjectSearcher = new ManagementObjectSearcher(win32ComputerSystem);
            var info = managementObjectSearcher.Get();

            foreach (var item in info)
            {
                return new KeyValuePair<string, string>(item["Manufacturer"].ToString(), item["Product"].ToString());
            }

            return new KeyValuePair<string, string>();
        }

        private static string InstallDate()
        {
            const string win32OperatingSystem = "SELECT * FROM Win32_OperatingSystem";
            var managementObjectSearcher = new ManagementObjectSearcher(win32OperatingSystem);
            var info = managementObjectSearcher.Get();
            var installDate = string.Empty;
            foreach (var item in info)
            {
                installDate = item["InstallDate"].ToString();
                break;
            }

            return ManagementDateTimeConverter.ToDateTime(installDate).ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}