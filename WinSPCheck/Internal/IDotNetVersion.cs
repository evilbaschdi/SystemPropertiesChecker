using System.Collections.Generic;

namespace WinSPCheck.Internal
{
    /// <summary>
    ///     Interface for classes that return a list of current installed dot net versions.
    /// </summary>
    public interface IDotNetVersion
    {
        /// <summary>
        ///     Contains a list of current installed dot net versions.
        /// </summary>
        List<string> List { get; }
    }
}