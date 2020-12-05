using System;

namespace SystemPropertiesChecker.Core.Models
{
    /// <summary>
    /// </summary>
    public class PasswordExpirationModel
    {
        /// <summary>
        /// </summary>
        public string DateString { get; init; }


        /// <summary>
        /// </summary>
        public DateTime PasswordExpirationDate { get; init; }

        /// <summary>
        /// </summary>
        public string UserName { get; init; }
    }
}