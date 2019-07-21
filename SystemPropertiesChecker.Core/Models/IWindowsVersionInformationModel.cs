namespace SystemPropertiesChecker.Core.Models
{
    /// <summary>
    ///     Helper interface.
    /// </summary>
    // ReSharper disable UnusedMember.Global
    // ReSharper disable UnusedMemberInSuper.Global
    public interface IWindowsVersionInformationModel
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
        string[] BuildLabExArray { get; set; }

        /// <summary>
        /// </summary>
        string Computername { get; set; }

        /// <summary>
        /// </summary>
        string CsdVersion { get; set; }

        /// <summary>
        /// </summary>
        string CurrentBuild { get; set; }

        /// <summary>
        /// </summary>
        string CurrentVersion { get; set; }

        /// <summary>
        /// </summary>
        string Domain { get; set; }

        /// <summary>
        /// </summary>
        string Manufacturer { get; set; }

        /// <summary>
        /// </summary>
        string ManufacturerProduct { get; set; }

        /// <summary>
        /// </summary>
        string PasswordExpirationDate { get; set; }

        /// <summary>
        /// </summary>
        string ProductName { get; set; }

        /// <summary>
        /// </summary>
        string ReleaseId { get; set; }

        /// <summary>
        /// </summary>
        string Ubr { get; set; }

        /// <summary>
        /// </summary>
        string UserName { get; set; }
    }
    // ReSharper restore UnusedMemberInSuper.Global
    // ReSharper restore UnusedMember.Global
}