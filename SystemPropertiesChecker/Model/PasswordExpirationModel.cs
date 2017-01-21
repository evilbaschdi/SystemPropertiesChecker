using System;

namespace SystemPropertiesChecker.Model
{/// <summary>
/// 
/// </summary>
    public class PasswordExpirationModel : IPasswordExpirationModel
    {/// <summary>
    /// 
    /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DateString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime PasswordExpirationDate { get; set; }
    }
}