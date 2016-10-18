using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Management;
using System.Windows.Shell;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that provides values about the current windows version.
    /// </summary>
    public class GetWindowsVersionInformation : IWindowsVersionInformation
    {
        private readonly IRegistryValue _registryValue;
        private readonly IWindowsVersionInformationHelper _windowsVersionInformationHelper;
        private readonly MainWindow _mainWindow;

        private IWindowsVersionInformationHelper _cachedWindowsVersionInformationHelper;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     <paramref name="registryValue" /> is <see langword="null" />.
        ///     <paramref name="windowsVersionInformationHelper" /> is <see langword="null" />.
        ///     <paramref name="mainWindow" /> is <see langword="null" />.
        /// </exception>
        public GetWindowsVersionInformation(IRegistryValue registryValue, IWindowsVersionInformationHelper windowsVersionInformationHelper, MainWindow mainWindow)
        {
            if (registryValue == null)
            {
                throw new ArgumentNullException(nameof(registryValue));
            }
            if (windowsVersionInformationHelper == null)
            {
                throw new ArgumentNullException(nameof(windowsVersionInformationHelper));
            }
            if (mainWindow == null)
            {
                throw new ArgumentNullException(nameof(mainWindow));
            }
            _registryValue = registryValue;
            _windowsVersionInformationHelper = windowsVersionInformationHelper;
            _mainWindow = mainWindow;
        }

        /// <summary>
        ///     Contains WindowsVersionInformation values.
        /// </summary>
        public IWindowsVersionInformationHelper Values
        {
            get
            {
                if (_cachedWindowsVersionInformationHelper != null)
                {
                    return _cachedWindowsVersionInformationHelper;
                }
                var bits = Bits();
                var virtualSystem = VirtualSystem();
                var domain = System.Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties().DomainName;

                var currentVersion = _registryValue.For("CurrentVersion");
                var currentMajorVersionNumber = _registryValue.For("CurrentMajorVersionNumber");
                var currentMinorVersionNumber = _registryValue.For("CurrentMinorVersionNumber");

                var csdVersion = !string.IsNullOrEmpty(_registryValue.For("CSDVersion"))
                    ? $" with {_registryValue.For("CSDVersion")}"
                    : string.Empty;

                var releaseId = !string.IsNullOrEmpty(_registryValue.For("ReleaseId"))
                    ? $" (Release {_registryValue.For("ReleaseId")})"
                    : string.Empty;

                var version = !string.IsNullOrWhiteSpace(currentMajorVersionNumber) &&
                              !string.IsNullOrWhiteSpace(currentMinorVersionNumber)
                    ? $"{currentMajorVersionNumber}.{currentMinorVersionNumber}"
                    : currentVersion;

                if (!string.IsNullOrWhiteSpace(domain))
                {
                    _windowsVersionInformationHelper.Domain = domain;
                    var passwordExpirationDate = PasswordExpirationDate(domain);
                    _windowsVersionInformationHelper.UserName = passwordExpirationDate.Key;
                    _windowsVersionInformationHelper.PasswordExpirationDate = passwordExpirationDate.Value;
                }

                _windowsVersionInformationHelper.Computername = Environment.MachineName;
                _windowsVersionInformationHelper.Bits = bits;
                _windowsVersionInformationHelper.Virtual = virtualSystem.Key;
                _windowsVersionInformationHelper.Manufacturer = virtualSystem.Value;
                _windowsVersionInformationHelper.BuildLab = _registryValue.For("BuildLab");
                _windowsVersionInformationHelper.BuildLabEx = _registryValue.For("BuildLabEx");
                _windowsVersionInformationHelper.BuildLabExArray = _registryValue.For("BuildLabEx").Split('.');
                _windowsVersionInformationHelper.CurrentBuild = _registryValue.For("CurrentBuild");
                _windowsVersionInformationHelper.ProductName = _registryValue.For("ProductName");
                _windowsVersionInformationHelper.CurrentVersion = version;
                _windowsVersionInformationHelper.CsdVersion = csdVersion;
                _windowsVersionInformationHelper.ReleaseId = releaseId;
                _windowsVersionInformationHelper.Ubr = _registryValue.For("UBR");
                _cachedWindowsVersionInformationHelper = _windowsVersionInformationHelper;
                return _cachedWindowsVersionInformationHelper;
            }
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
                            //if (VersionHelper.IsWindows10 || VersionHelper.IsWindows8)
                            //{
                            //    _toast.Show("Your password will expire in", $"{dif.Days} days and {dif.Hours} hours.");
                            //}
                            //else
                            //{
                            //    MessageBox.Show($"Your password will expire in {dif.Days} days and {dif.Hours} hours.");
                            //}
                            _mainWindow.ShowMessage("Password Expiration Date", $"Your password will expire in {dif.Days} days and {dif.Hours} hours.");
                            _mainWindow.TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                            _mainWindow.TaskbarItemInfo.ProgressValue = 1;
                        }
                        var dateString = nextSet.ToString("g");
                        return new KeyValuePair<string, string>(samAccountName, nextSet.Year == 1970 ? "-" : dateString);
                    }
                }
            }
            return new KeyValuePair<string, string>();
        }
    }
}