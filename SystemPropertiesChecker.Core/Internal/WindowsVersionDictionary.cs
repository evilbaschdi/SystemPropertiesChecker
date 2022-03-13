using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
/// <summary>
///     Class that provides a WindowsVersionInformationStack.
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class WindowsVersionDictionary : IWindowsVersionDictionary
{
    private static string _ipAddressV4;
    private static string _ipAddressV6;
    private readonly IWindowsVersionInformation _windowsVersionInformation;

    /// <summary>
    ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
    /// </summary>
    public WindowsVersionDictionary(IWindowsVersionInformation windowsVersionInformation)
    {
        _windowsVersionInformation = windowsVersionInformation ?? throw new ArgumentNullException(nameof(windowsVersionInformation));
    }

    /// <summary>
    ///     Contains a string of WindowsVersionInfromation.
    /// </summary>
    public Dictionary<string, string> Value
    {
        get
        {
            var dictionary = new Dictionary<string, string>();
            var values = _windowsVersionInformation.Value;
            GetLocalIpAddress();

            dictionary.Add("Device Name", values.ComputerName);
            dictionary.Add("Current IP v4", _ipAddressV4);
            dictionary.Add("Current IP v6", _ipAddressV6);
            if (!string.IsNullOrWhiteSpace(values.Domain))
            {
                dictionary.Add("Domain", values.Domain);
                dictionary.Add("Current user", values.UserName);
                dictionary.Add("Password expiration date", values.PasswordExpirationDate);
            }

            dictionary.Add("Edition", $"{values.Caption}{values.CsdVersion}");

            if (!string.IsNullOrWhiteSpace(values.DisplayVersion))
            {
                dictionary.Add("Version", values.DisplayVersion);
            }

            if (!string.IsNullOrWhiteSpace(values.ReleaseId))
            {
                dictionary.Add("Release Id", values.ReleaseId);
            }

            dictionary.Add("Internal Version", values.CurrentVersion);
            dictionary.Add("Current Build", values.CurrentBuild);
            if (!string.IsNullOrWhiteSpace(values.WindowsFeatureExperiencePackVersion))
            {
                dictionary.Add("Windows Feature Experience Pack", values.WindowsFeatureExperiencePackVersion);
            }

            if (!string.IsNullOrWhiteSpace(values.InsiderChannel))
            {
                dictionary.Add("Insider Channel", values.InsiderChannel);
            }

            if (OperatingSystem.IsWindows())
            {
                dictionary.Add("OS Build",
                    !string.IsNullOrWhiteSpace(values.Ubr) ? $"{values.CurrentBuild}.{values.Ubr}" : $"{values.BuildLabExList[0]}.{values.BuildLabExList[1]}");
                dictionary.Add("BuildLab", values.BuildLab);
                dictionary.Add("BuildLabEx", values.BuildLabEx);
                dictionary.Add("Architecture", values.Bits);
                dictionary.Add("Install Date", values.InstallDate);
            }

            dictionary.Add("Manufacturer", values.Manufacturer);

            if (!string.IsNullOrWhiteSpace(values.ManufacturerProduct))
            {
                dictionary.Add("Manufacturer Product", values.ManufacturerProduct);
            }

            return dictionary;
        }
    }

    /// <summary>
    /// </summary>
    /// <returns></returns>
    private static void GetLocalIpAddress()
    {
        var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus == OperationalStatus.Up);

        foreach (var networkInterface in networkInterfaces)
        {
            var adapterProperties = networkInterface.GetIPProperties();

            if (adapterProperties.GatewayAddresses.FirstOrDefault() == null)
            {
                continue;
            }

            foreach (var ip in networkInterface.GetIPProperties().UnicastAddresses)
            {
                switch (ip.Address.AddressFamily)
                {
                    case AddressFamily.InterNetwork:
                        _ipAddressV4 = ip.Address.ToString();
                        break;
                    case AddressFamily.InterNetworkV6:
                        _ipAddressV6 = ip.Address.ToString();
                        break;
                }
            }
        }
    }
}