namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public interface IRegistryValue
    {
        /// <summary>
        ///     Contains a string providing a registry value.
        /// </summary>
        /// <param name="name">Name of the RegistryKey</param>
        /// <returns></returns>
        string For(string name);
    }
}