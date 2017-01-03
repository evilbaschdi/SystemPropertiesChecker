using System;

namespace WinSPCheck.Model
{
    public interface IPasswordExpirationModel
    {
        string UserName { get; set; }
        string DateString { get; set; }
        DateTime PasswordExpirationDate { get; set; }
    }
}