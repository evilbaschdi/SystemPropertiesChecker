using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Management;
using WinSPCheck.Model;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that provides values about the current windows version.
    /// </summary>
    public class GetWindowsVersionInformation : IWindowsVersionInformation
    {
        private readonly IRegistryValue _registryValue;
        private readonly IWindowsVersionInformationModel _windowsVersionInformationModel;
        private IWindowsVersionInformationModel _cachedWindowsVersionInformationModel;
        private string _passwordExpirationMessage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="registryValue" /> is <see langword="null" />.
        ///     <paramref name="windowsVersionInformationModel" /> is <see langword="null" />.
        /// </exception>
        public GetWindowsVersionInformation(IRegistryValue registryValue, IWindowsVersionInformationModel windowsVersionInformationModel)
        {
            if (registryValue == null)
            {
                throw new ArgumentNullException(nameof(registryValue));
            }
            if (windowsVersionInformationModel == null)
            {
                throw new ArgumentNullException(nameof(windowsVersionInformationModel));
            }
            _registryValue = registryValue;
            _windowsVersionInformationModel = windowsVersionInformationModel;
        }

        /// <summary>
        ///     Contains WindowsVersionInformation values.
        /// </summary>
        public IWindowsVersionInformationModel Values
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
                    var passwordExpirationDate = PasswordExpirationDate(domain);
                    _windowsVersionInformationModel.UserName = passwordExpirationDate.Key;
                    _windowsVersionInformationModel.PasswordExpirationDate = passwordExpirationDate.Value;
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

        /// <summary>
        ///     Passwordexpiration message.
        /// </summary>
        public string PasswordExpirationMessage
        {
            get { return _passwordExpirationMessage; }
        }

        private string Bits()
        {
            return Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";
        }

        private KeyValuePair<bool, string> VirtualSystem()
        {
            var win32Computersystem = "SELECT * FROM Win32_ComputerSystem";
            var managementObjectSearcher = new ManagementObjectSearcher(win32Computersystem);
            var virtualSystem = false;
            var info = managementObjectSearcher.Get();
            var manufacturer = string.Empty;
            foreach (var item in info)
            {
                manufacturer = item["Manufacturer"].ToString().ToLower();

                if ((manufacturer == "microsoft corporation" && item["Model"].ToString().ToUpperInvariant().Contains("VIRTUAL"))
                    || manufacturer.Contains("vmware")
                    || item["Model"].ToString() == "VirtualBox")
                {
                    virtualSystem = true;
                }
            }
            return new KeyValuePair<bool, string>(virtualSystem, manufacturer);
        }

        private KeyValuePair<string, string> PasswordExpirationDate(string domain)
        {
            try
            {
                var context = new DirectoryContext(DirectoryContextType.Domain, domain);
                using (var dc = DomainController.FindOne(context))
                {
                    using (var ds = dc.GetDirectorySearcher())
                    {
                        var samAccountName = UserPrincipal.Current.SamAccountName;
                        ds.Filter = $"(sAMAccountName={samAccountName})";
                        ds.SizeLimit = 10;
                        var sr = ds.FindOne();

                        if (sr != null)
                        {
                            var de = sr.GetDirectoryEntry();
                            var nextSet = (DateTime) de.InvokeGet("PasswordExpirationDate");
                            var dif = nextSet - DateTime.Now;
                            if (dif.Days < 10 && nextSet.Year != 1970)
                            {
                                _passwordExpirationMessage = $"{dif.Days} days and {dif.Hours} hours.";
                            }
                            var dateString = nextSet.ToString("g");
                            return new KeyValuePair<string, string>(samAccountName, nextSet.Year == 1970 ? "-" : dateString);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new KeyValuePair<string, string>("", "(indeterminate)");
        }
    }
}