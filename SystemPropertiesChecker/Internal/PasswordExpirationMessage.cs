using System;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PasswordExpirationMessage : IPasswordExpirationMessage
    {
        private readonly IWindowsVersionInformation _windowsVersionInformation;
        private readonly IPasswordExpirationDate _passwordExpirationDate;

        /// <summary>
        ///     Constructor of the class
        /// </summary>
        /// <param name="windowsVersionInformation"></param>
        /// <param name="passwordExpirationDate"></param>
        public PasswordExpirationMessage(IWindowsVersionInformation windowsVersionInformation, IPasswordExpirationDate passwordExpirationDate)
        {
            if (windowsVersionInformation == null)
            {
                throw new ArgumentNullException(nameof(windowsVersionInformation));
            }
            if (passwordExpirationDate == null)
            {
                throw new ArgumentNullException(nameof(passwordExpirationDate));
            }
            _windowsVersionInformation = windowsVersionInformation;
            _passwordExpirationDate = passwordExpirationDate;
        }

        /// <inheritdoc />
        public string Value
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(_windowsVersionInformation.Value.Domain))
                {
                    var nextSet = _passwordExpirationDate.ValueFor(_windowsVersionInformation.Value.Domain).PasswordExpirationDate;
                    var dif = nextSet - DateTime.Now;
                    return dif.Days < 10 && dif.Days > -50000 && nextSet.Year != 1970 ? $"{dif.Days} days and {dif.Hours} hours." : "";
                }
                return "";
            }
        }
    }
}