namespace SystemPropertiesChecker.Model
{
    /// <inheritdoc />
    /// <summary>
    ///     Helper class.
    /// </summary>
    public class WindowsVersionInformationModel : IWindowsVersionInformationModel
    {
        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string Bits { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>

        public bool Virtual { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string BuildLab { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string BuildLabEx { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string ProductName { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string CurrentBuild { get; set; }

        /// <summary>
        /// </summary>
        public string CurrentVersion { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string ReleaseId { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string CsdVersion { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string[] BuildLabExArray { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string Manufacturer { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string Computername { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string Domain { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string UserName { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string PasswordExpirationDate { get; set; }

        /// <inheritdoc />
        /// <summary>
        /// </summary>
        public string Ubr { get; set; }
    }
    // ReSharper restore UnusedAutoPropertyAccessor.Global
}