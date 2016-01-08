namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Interface for classes that provide a WindowsVersionInformationStack.
    /// </summary>
    public interface ICurrentVersionText
    {
        /// <summary>
        ///     Contains a string of WindowsVersionInfromation.
        /// </summary>
        string Value { get; }
    }
}