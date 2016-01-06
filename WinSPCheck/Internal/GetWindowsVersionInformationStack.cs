using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Class that provides a WindowsVersionInformationStack.
    /// </summary>
    public class GetWindowsVersionInformationStack : IWindowsVersionInformationStack
    {
        private readonly IDotNetVersion _dotNetVersion;
        private readonly IWindowsVersionInformation _windowsVersionInformation;

        /// <summary>
        ///     Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        public GetWindowsVersionInformationStack(IDotNetVersion dotNetVersion, IWindowsVersionInformation windowsVersionInformation)
        {
            if(dotNetVersion == null)
            {
                throw new ArgumentNullException(nameof(dotNetVersion));
            }
            if(windowsVersionInformation == null)
            {
                throw new ArgumentNullException(nameof(windowsVersionInformation));
            }
            _dotNetVersion = dotNetVersion;
            _windowsVersionInformation = windowsVersionInformation;
        }

        /// <summary>
        ///     Contains a string of WindowsVersionInfromation.
        /// </summary>
        public string Value
        {
            get
            {
                var values = _windowsVersionInformation.Values;

                var sb = new StringBuilder();
                sb.Append($"Computername: {values.Computername}{Environment.NewLine}");
                if(!string.IsNullOrWhiteSpace(values.Domain))
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
                sb.Append($"{Environment.NewLine}{Environment.NewLine}");
                sb.Append($"Version number: {values.CurrentVersion} | Current build: {values.CurrentBuild} (Build: ");
                sb.Append(!string.IsNullOrWhiteSpace(values.Ubr)
                    ? $"{values.CurrentBuild}.{values.Ubr}"
                    : $"{values.BuildLabExArray[0]}.{values.BuildLabExArray[1]}");
                sb.Append(")");
                sb.Append(Environment.NewLine);
                sb.Append($"BuildLab: {values.BuildLab}{Environment.NewLine}");
                sb.Append($"BuildLabEx: {values.BuildLabEx}{Environment.NewLine}");

                sb.Append(Environment.NewLine);
                sb.Append(_dotNetVersion.List.Aggregate(string.Empty, (c, v) => c + v + Environment.NewLine));

                return sb.ToString();
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static string GetLocalIpAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach(var ip in host.AddressList.Where(ip => ip.AddressFamily == AddressFamily.InterNetwork))
            {
                return ip.ToString();
            }
            return "Local IP Address Not Found!";
        }
    }
}