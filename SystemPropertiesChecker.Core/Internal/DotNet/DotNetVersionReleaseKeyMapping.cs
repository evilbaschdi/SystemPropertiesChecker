using EvilBaschdi.Settings;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{ // ReSharper disable once ClassNeverInstantiated.Global
    /// <inheritdoc cref="SettingsFromJsonFile" />
    public class DotNetVersionReleaseKeyMapping : SettingsFromJsonFile, IDotNetVersionReleaseKeyMapping
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DotNetVersionReleaseKeyMapping()
            : base("DotNetVersionReleaseKeyMapping.json")
        {
        }
    }
}