using System;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using SystemPropertiesChecker.Models;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class PasswordExpirationDate : IPasswordExpirationDate
    {
        /// <inheritdoc />
        public PasswordExpirationModel ValueFor(string domain)
        {
            try
            {
                var context = new DirectoryContext(DirectoryContextType.Domain, domain);
                using (var dc = DomainController.FindOne(context))
                {
                    using (var ds = dc.GetDirectorySearcher())
                    {
                        var samAccountName = UserPrincipal.Current.SamAccountName;
                        ds.Filter = $"(sAMAccountName={samAccountName})";
                        ds.SizeLimit = 10;
                        var sr = ds.FindOne();
                        var de = sr.GetDirectoryEntry();
                        var nextSet = (DateTime) de.InvokeGet("PasswordExpirationDate");
                        var dateString = nextSet.ToString("g");
                        return new PasswordExpirationModel
                               {
                                   DateString = nextSet.Year == 1970 ? "-" : dateString,
                                   UserName = samAccountName,
                                   PasswordExpirationDate = nextSet
                               };
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return new PasswordExpirationModel
                   {
                       DateString = "(undefined)",
                       UserName = ""
                   };
        }
    }
}