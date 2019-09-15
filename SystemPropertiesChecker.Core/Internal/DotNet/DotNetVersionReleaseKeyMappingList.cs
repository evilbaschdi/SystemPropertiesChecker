using System;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <summary>
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DotNetVersionReleaseKeyMappingList : IDotNetVersionReleaseKeyMappingList
    {
        /// <summary>
        ///     Reads dotNet version string by release key from app.config.
        /// </summary>
        public string ValueFor(string releaseKey)
        {
            if (releaseKey == null)
            {
                throw new ArgumentNullException(nameof(releaseKey));
            }

            var fullName = DotNetVersionReleaseKeyMapping.AppSetting[releaseKey];
            return !string.IsNullOrWhiteSpace(fullName) ? fullName : releaseKey;
        }
    }
}