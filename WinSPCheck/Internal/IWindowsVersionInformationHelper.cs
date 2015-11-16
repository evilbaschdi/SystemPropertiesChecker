namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Helper interface.
    /// </summary>
    public interface IWindowsVersionInformationHelper
    {
        /// <summary>
        /// </summary>
        string Bits { get; set; }

        /// <summary>
        /// </summary>
        string BuildLab { get; set; }

        /// <summary>
        /// </summary>
        string BuildLabEx { get; set; }

        /// <summary>
        /// </summary>
        string ProductName { get; set; }

        /// <summary>
        /// </summary>
        string CurrentBuild { get; set; }

        /// <summary>
        /// </summary>
        string CurrentVersion { get; set; }

        /// <summary>
        /// </summary>
        string ReleaseId { get; set; }

        /// <summary>
        /// </summary>
        string CsdVersion { get; set; }

        /// <summary>
        /// </summary>
        string[] BuildLabExArray { get; set; }
    }
}