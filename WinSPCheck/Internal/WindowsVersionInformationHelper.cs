namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Helper class.
    /// </summary>
    public class WindowsVersionInformationHelper : IWindowsVersionInformationHelper
    {
        /// <summary>
        /// </summary>
        public string Bits { get; set; }

        /// <summary>
        /// </summary>
        public string BuildLab { get; set; }

        /// <summary>
        /// </summary>
        public string BuildLabEx { get; set; }

        /// <summary>
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// </summary>
        public string CurrentBuild { get; set; }

        /// <summary>
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <summary>
        /// </summary>
        public string ReleaseId { get; set; }

        /// <summary>
        /// </summary>
        public string CsdVersion { get; set; }

        /// <summary>
        /// </summary>
        public string[] BuildLabExArray { get; set; }
    }
}