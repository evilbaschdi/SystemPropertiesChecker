using System;

namespace SystemPropertiesChecker.Model
{/// <summary>
/// 
/// </summary>
    public interface IPasswordExpirationModel
    {/// <summary>
    /// 
    /// </summary>
        string UserName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string DateString { get; set; }
        /// <summary>
        /// 
        /// </summary>
        DateTime PasswordExpirationDate { get; set; }
    }
}