using System;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PasswordExpirationMessage : IPasswordExpirationMessage
    {
        private readonly IPasswordExpirationDate _passwordExpirationDate;
        private readonly IWindowsVersionInformation _windowsVersionInformation;

        /// <summary>
        ///     Constructor of the class
        /// </summary>
        /// <param name="windowsVersionInformation"></param>
        /// <param name="passwordExpirationDate"></param>
        public PasswordExpirationMessage(IWindowsVersionInformation windowsVersionInformation, IPasswordExpirationDate passwordExpirationDate)
        {
            _windowsVersionInformation = windowsVersionInformation ?? throw new ArgumentNullException(nameof(windowsVersionInformation));
            _passwordExpirationDate = passwordExpirationDate ?? throw new ArgumentNullException(nameof(passwordExpirationDate));
        }

        /// <inheritdoc />
        public string Value
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_windowsVersionInformation.Value.Domain))
                {
                    return "";
                }

                var nextSet = _passwordExpirationDate.ValueFor(_windowsVersionInformation.Value.Domain).PasswordExpirationDate;
                var dif = nextSet - DateTime.Now;
                return dif.Days is < 10 and > -50000 && nextSet.Year != 1970 ? $"{dif.Days} days and {dif.Hours} hours." : "";
            }
        }
    }
}