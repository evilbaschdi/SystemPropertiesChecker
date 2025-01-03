using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.ActiveDirectory;
using System.Runtime.InteropServices;
using JetBrains.Annotations;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class PasswordExpirationDate : IPasswordExpirationDate
{
    /// <inheritdoc />
    public PasswordExpirationModel ValueFor([NotNull] string domain)
    {
        ArgumentNullException.ThrowIfNull(domain);

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return new()
                   {
                       DateString = "(undefined)",
                       UserName = ""
                   };
        }

        try
        {
            var context = new DirectoryContext(DirectoryContextType.Domain, domain);
            using var dc = DomainController.FindOne(context);
            using var ds = dc.GetDirectorySearcher();
            var samAccountName = UserPrincipal.Current.SamAccountName;
            ds.Filter = $"(sAMAccountName={samAccountName})";
            ds.SizeLimit = 10;
            var sr = ds.FindOne();
            if (sr != null)
            {
                var directoryEntry = sr.GetDirectoryEntry();
                var passwordExpirationDate = directoryEntry.InvokeGet("PasswordExpirationDate");
                if (passwordExpirationDate != null)
                {
                    var nextSet = (DateTime)passwordExpirationDate;
                    var dateString = nextSet.ToString("g");
                    return new()
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

        return new()
               {
                   DateString = "(undefined)",
                   UserName = ""
               };
    }
}