using System;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <inheritdoc />
    public class DotNetVersionReleaseKeyMappingList : IDotNetVersionReleaseKeyMappingList
    {
        private readonly IDotNetVersionReleaseKeyMapping _dotNetVersionReleaseKeyMapping;

        public DotNetVersionReleaseKeyMappingList(IDotNetVersionReleaseKeyMapping dotNetVersionReleaseKeyMapping)
        {
            _dotNetVersionReleaseKeyMapping = dotNetVersionReleaseKeyMapping ?? throw new ArgumentNullException(nameof(dotNetVersionReleaseKeyMapping));
        }

        /// <summary>
        ///     Reads dotNet version string by release key from app.config.
        /// </summary>
        public string ValueFor(string releaseKey)
        {
            if (releaseKey == null)
            {
                throw new ArgumentNullException(nameof(releaseKey));
            }

            var fullName = _dotNetVersionReleaseKeyMapping.Value[releaseKey];
            return !string.IsNullOrWhiteSpace(fullName) ? fullName : "unknown";
        }
    }
}