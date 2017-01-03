using System;

namespace WinSPCheck.Model
{
    public class PasswordExpirationModel : IPasswordExpirationModel
    {
        public string UserName { get; set; }

        public string DateString { get; set; }

        public DateTime PasswordExpirationDate { get; set; }
    }
}