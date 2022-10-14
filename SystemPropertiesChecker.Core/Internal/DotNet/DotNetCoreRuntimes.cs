namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc cref="IDotNetCoreRunTimes" />
/// <inheritdoc cref="DotNetCoreListAsString" />
public class DotNetCoreRunTimes : DotNetCoreListAsString, IDotNetCoreRunTimes
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public DotNetCoreRunTimes()
        : base("runtimes")
    {
    }
}