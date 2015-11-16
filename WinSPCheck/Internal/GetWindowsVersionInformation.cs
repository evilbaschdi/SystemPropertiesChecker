using System;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that provides values about the current windows version.
    /// </summary>
    public class GetWindowsVersionInformation : IWindowsVersionInformation
    {
        private readonly IRegistryValue _registryValue;
        private readonly IWindowsVersionInformationHelper _windowsVersionInformationHelper;
        private IWindowsVersionInformationHelper _values;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public GetWindowsVersionInformation(IRegistryValue registryValue, IWindowsVersionInformationHelper windowsVersionInformationHelper)
        {
            if(registryValue == null)
            {
                throw new ArgumentNullException(nameof(registryValue));
            }
            _registryValue = registryValue;
            _windowsVersionInformationHelper = windowsVersionInformationHelper;
        }

        /// <summary>
        ///     Contains WindowsVersionInformation values.
        /// </summary>
        public IWindowsVersionInformationHelper Values
        {
            get
            {
                if(_values != null)
                {
                    return _values;
                }
                var bits = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";

                var currentVersion = _registryValue.For("CurrentVersion");
                var currentMajorVersionNumber = _registryValue.For("CurrentMajorVersionNumber");
                var currentMinorVersionNumber = _registryValue.For("CurrentMinorVersionNumber");

                var csdVersion = !string.IsNullOrEmpty(_registryValue.For("CSDVersion"))
                    ? $" with {_registryValue.For("CSDVersion")}"
                    : string.Empty;

                var releaseId = !string.IsNullOrEmpty(_registryValue.For("ReleaseId"))
                    ? $" Version: {_registryValue.For("ReleaseId")}"
                    : string.Empty;

                var version = !string.IsNullOrWhiteSpace(currentMajorVersionNumber) &&
                              !string.IsNullOrWhiteSpace(currentMinorVersionNumber)
                    ? $"{currentMajorVersionNumber}.{currentMinorVersionNumber}"
                    : currentVersion;

                _windowsVersionInformationHelper.Bits = bits;
                _windowsVersionInformationHelper.BuildLab = _registryValue.For("BuildLab");
                _windowsVersionInformationHelper.BuildLabEx = _registryValue.For("BuildLabEx");
                _windowsVersionInformationHelper.BuildLabExArray = _registryValue.For("BuildLabEx").Split('.');
                _windowsVersionInformationHelper.CurrentBuild = _registryValue.For("CurrentBuild");
                _windowsVersionInformationHelper.ProductName = _registryValue.For("ProductName");
                _windowsVersionInformationHelper.CurrentVersion = version;
                _windowsVersionInformationHelper.CsdVersion = csdVersion;
                _windowsVersionInformationHelper.ReleaseId = releaseId;
                _values = _windowsVersionInformationHelper;
                return _values;
            }
        }
    }
}