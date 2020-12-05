using System;

namespace SystemPropertiesChecker.Core.Models
{
    /// <summary>
    ///     Source Os
    /// </summary>
    public class SourceOs
    {
        /// <summary>
        ///     CurrentBuild
        /// </summary>
        public string Build { get; init; }

        /// <summary>
        ///     Install Date
        /// </summary>
        public DateTime InstallDate { get; init; }

        /// <summary>
        ///     Product Name
        /// </summary>
        public string ProductName { get; init; }

        /// <summary>
        ///     Release Id
        /// </summary>
        public string ReleaseId { get; init; }
    }
}