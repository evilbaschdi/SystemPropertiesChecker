using System;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that provides a WindowsVersionInformationStack.
    /// </summary>
    public class CurrentVersionText : ICurrentVersionText
    {
        private readonly IWindowsVersionInformation _windowsVersionInformation;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public CurrentVersionText(IWindowsVersionInformation windowsVersionInformation)
        {
            if (windowsVersionInformation == null)
            {
                throw new ArgumentNullException(nameof(windowsVersionInformation));
            }
            _windowsVersionInformation = windowsVersionInformation;
        }

        /// <summary>
        ///     Contains a string of WindowsVersionInfromation.
        /// </summary>
        public string Value
        {
            get
            {
                var values = _windowsVersionInformation.Value;

                var sb = new StringBuilder();
                sb.Append($"Computername: {values.Computername}{Environment.NewLine}");
                if (!string.IsNullOrWhiteSpace(values.Domain))
                {
                    sb.Append($"Domain: {values.Domain}{Environment.NewLine}");
                    sb.Append($"Current user: {values.UserName} | Password expiration date: {values.PasswordExpirationDate}{Environment.NewLine}");
                }
                sb.Append($"Current IP: {GetLocalIpAddress()}{Environment.NewLine}");
                sb.Append($"Productname: {values.ProductName}{values.CsdVersion}{values.ReleaseId}{Environment.NewLine}");
                sb.Append($"System type: {values.Bits}{Environment.NewLine}");
                sb.Append(values.Virtual
                    ? $"Virtual system: {values.Manufacturer}"
                    : $"Virtual system: -");

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddress()
        {
            var networkInterfaces = NetworkInterface.GetAllNetworkInterfaces().Where(i => i.OperationalStatus == OperationalStatus.Up);

            foreach (NetworkInterface networkInterface in networkInterfaces)
            {
                var adapterProperties = networkInterface.GetIPProperties();

                if (adapterProperties.GatewayAddresses.FirstOrDefault() != null)
                {
                    foreach (UnicastIPAddressInformation ip in networkInterface.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            return ip.Address.ToString();
                        }
                    }
                }
            }
            return "";
        }
    }
}