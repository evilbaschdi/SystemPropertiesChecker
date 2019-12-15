using System;

namespace SystemPropertiesChecker.Core.Models
{
    /// <summary>
    /// </summary>
    public class PasswordExpirationModel
    {
        /// <summary>
        /// </summary>
        public string DateString { get; set; }


        /// <summary>
        /// </summary>
        public DateTime PasswordExpirationDate { get; set; }

        /// <summary>
        /// </summary>
        public string UserName { get; set; }
    }
}