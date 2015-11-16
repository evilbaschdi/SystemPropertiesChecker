namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Interface for classes that provide a WindowsVersionInformationStack.
    /// </summary>
    public interface IWindowsVersionInformationStack
    {
        /// <summary>
        ///     Contains a string of WindowsVersionInfromation.
        /// </summary>
        string Value { get; }
    }
}