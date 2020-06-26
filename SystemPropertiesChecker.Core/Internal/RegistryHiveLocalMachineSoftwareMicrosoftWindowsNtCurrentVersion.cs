namespace SystemPropertiesChecker.Core.Internal
{
    /// <summary>
    ///     Class that provides RegistryValues from WindowsNT CurrentVersion.
    /// </summary>
    // ReSharper disable once ClassNeverInstantiated.Global
    public class RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion : RegistryHiveLocalMachineValueFor,
                                                                                    IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion()
            : base(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion")
        {
        }
    }
}