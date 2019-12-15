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
        public string Build { get; set; }

        /// <summary>
        ///     Install Date
        /// </summary>
        public DateTime InstallDate { get; set; }

        /// <summary>
        ///     Product Name
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        ///     Release Id
        /// </summary>
        public string ReleaseId { get; set; }
    }
}