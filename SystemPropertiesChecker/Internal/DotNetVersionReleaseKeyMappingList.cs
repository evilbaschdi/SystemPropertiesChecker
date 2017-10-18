using System;
using System.Configuration;
using System.Linq;

namespace SystemPropertiesChecker.Internal
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DotNetVersionReleaseKeyMappingList : IDotNetVersionReleaseKeyMappingList
    {
        /// <summary>
        ///     Reads dotNet version string by release key from app.config.
        /// </summary>
        public string ValueFor(int releaseKey)
        {
            if (releaseKey <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(releaseKey));
            }

            var value = ConfigurationManager.AppSettings.AllKeys.OrderByDescending(key => key).FirstOrDefault(key => releaseKey >= Convert.ToInt32(key));

            var fullName = ConfigurationManager.AppSettings[value];
            return !string.IsNullOrWhiteSpace(fullName) ? fullName : releaseKey.ToString();
        }
    }
}