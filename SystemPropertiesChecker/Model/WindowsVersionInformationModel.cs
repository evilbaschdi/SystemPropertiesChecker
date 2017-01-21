namespace SystemPropertiesChecker.Model
{
    /// <summary>
    ///     Helper class.
    /// </summary>
    public class WindowsVersionInformationModel : IWindowsVersionInformationModel
    {
        /// <summary>
        /// </summary>
        public string Bits { get; set; }

        /// <summary>
        /// </summary>
        public bool Virtual { get; set; }

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

        /// <summary>
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// </summary>
        public string Computername { get; set; }

        /// <summary>
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// </summary>
        public string PasswordExpirationDate { get; set; }

        /// <summary>
        /// </summary>
        public string Ubr { get; set; }
    }
}