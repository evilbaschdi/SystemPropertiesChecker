using System;
using System.Collections.Generic;
using System.Management;
using SystemPropertiesChecker.Models;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    ///     Class that provides values about the current windows version.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class WindowsVersionInformation : IWindowsVersionInformation
    {
        private readonly IRegistryValue _registryValue;
        private readonly IPasswordExpirationDate _passwordExpirationDate;
        private readonly WindowsVersionInformationModel _windowsVersionInformationModel = new WindowsVersionInformationModel();
        private WindowsVersionInformationModel _cachedWindowsVersionInformationModel;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="registryValue" /> is <see langword="null" />.
        /// </exception>
        public WindowsVersionInformation(IRegistryValue registryValue, IPasswordExpirationDate passwordExpirationDate)
        {
            _registryValue = registryValue ?? throw new ArgumentNullException(nameof(registryValue));
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
                var virtualSystem = VirtualSystem();
                var domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

                var currentVersion = _registryValue.ValueFor("CurrentVersion");
                var currentMajorVersionNumber = _registryValue.ValueFor("CurrentMajorVersionNumber");
                var currentMinorVersionNumber = _registryValue.ValueFor("CurrentMinorVersionNumber");

                var csdVersion = !string.IsNullOrEmpty(_registryValue.ValueFor("CSDVersion"))
                    ? $" with {_registryValue.ValueFor("CSDVersion")}"
                    : string.Empty;

                var releaseId = !string.IsNullOrEmpty(_registryValue.ValueFor("ReleaseId"))
                    ? $" (Release {_registryValue.ValueFor("ReleaseId")})"
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
                _windowsVersionInformationModel.Virtual = virtualSystem.Key;
                _windowsVersionInformationModel.Manufacturer = virtualSystem.Value;
                _windowsVersionInformationModel.BuildLab = _registryValue.ValueFor("BuildLab");
                _windowsVersionInformationModel.BuildLabEx = _registryValue.ValueFor("BuildLabEx");
                _windowsVersionInformationModel.BuildLabExArray = _registryValue.ValueFor("BuildLabEx").Split('.');
                _windowsVersionInformationModel.CurrentBuild = _registryValue.ValueFor("CurrentBuild");
                _windowsVersionInformationModel.ProductName = _registryValue.ValueFor("ProductName");
                _windowsVersionInformationModel.CurrentVersion = version;
                _windowsVersionInformationModel.CsdVersion = csdVersion;
                _windowsVersionInformationModel.ReleaseId = releaseId;
                _windowsVersionInformationModel.Ubr = _registryValue.ValueFor("UBR");
                _cachedWindowsVersionInformationModel = _windowsVersionInformationModel;
                return _cachedWindowsVersionInformationModel;
            }
        }

        private static string Bits()
        {
            return Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
        }

        private static KeyValuePair<bool, string> VirtualSystem()
        {
            const string win32Computersystem = "SELECT * FROM Win32_ComputerSystem";
            var managementObjectSearcher = new ManagementObjectSearcher(win32Computersystem);
            var info = managementObjectSearcher.Get();
            var manufacturer = string.Empty;
            foreach (var item in info)
            {
                manufacturer = item["Manufacturer"].ToString();
                break;
            }

            return new KeyValuePair<bool, string>(true, manufacturer);
        }
    }
}