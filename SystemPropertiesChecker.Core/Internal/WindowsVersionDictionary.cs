using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace SystemPropertiesChecker.Core.Internal
{
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

                dictionary.Add("Device Name", values.Computername);
                dictionary.Add("Current IP v4", _ipAddressV4);
                dictionary.Add("Current IP v6", _ipAddressV6);
                if (!string.IsNullOrWhiteSpace(values.Domain))
                {
                    dictionary.Add("Domain", values.Domain);
                    dictionary.Add("Current user", values.UserName);
                    dictionary.Add("Password expiration date", values.PasswordExpirationDate);
                }

                dictionary.Add("Edition", $"{values.ProductName}{values.CsdVersion}{values.ReleaseId}");
                dictionary.Add("Internal Version", values.CurrentVersion);
                dictionary.Add("Current Build", values.CurrentBuild);
                dictionary.Add("OS Build",
                    !string.IsNullOrWhiteSpace(values.Ubr) ? $"{values.CurrentBuild}.{values.Ubr}" : $"{values.BuildLabExArray[0]}.{values.BuildLabExArray[1]}");
                dictionary.Add("BuildLab", values.BuildLab);
                dictionary.Add("BuildLabEx", values.BuildLabEx);
                dictionary.Add("Architecture", values.Bits);
                dictionary.Add("Install Date", values.InstallDate);

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
                        case AddressFamily.Unknown:
                            break;
                        case AddressFamily.Unspecified:
                            break;
                        case AddressFamily.Unix:
                            break;
                        case AddressFamily.ImpLink:
                            break;
                        case AddressFamily.Pup:
                            break;
                        case AddressFamily.Chaos:
                            break;
                        case AddressFamily.NS:
                            break;
                        case AddressFamily.Iso:
                            break;
                        case AddressFamily.Ecma:
                            break;
                        case AddressFamily.DataKit:
                            break;
                        case AddressFamily.Ccitt:
                            break;
                        case AddressFamily.Sna:
                            break;
                        case AddressFamily.DecNet:
                            break;
                        case AddressFamily.DataLink:
                            break;
                        case AddressFamily.Lat:
                            break;
                        case AddressFamily.HyperChannel:
                            break;
                        case AddressFamily.AppleTalk:
                            break;
                        case AddressFamily.NetBios:
                            break;
                        case AddressFamily.VoiceView:
                            break;
                        case AddressFamily.FireFox:
                            break;
                        case AddressFamily.Banyan:
                            break;
                        case AddressFamily.Atm:
                            break;
                        case AddressFamily.Cluster:
                            break;
                        case AddressFamily.Ieee12844:
                            break;
                        case AddressFamily.Irda:
                            break;
                        case AddressFamily.NetworkDesigners:
                            break;
                        case AddressFamily.Max:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}