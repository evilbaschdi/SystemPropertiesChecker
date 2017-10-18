using System;

namespace SystemPropertiesChecker.Model
{
    /// <summary>
    /// </summary>
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMemberInSuper.Global
    public interface IPasswordExpirationModel

    {
        /// <summary>
        /// </summary>

        string UserName { get; set; }

        /// <summary>
        /// </summary>

        string DateString { get; set; }

        /// <summary>
        /// </summary>

        DateTime PasswordExpirationDate { get; set; }
    }
    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}