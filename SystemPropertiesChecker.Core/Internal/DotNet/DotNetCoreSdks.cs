namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc cref="IDotNetCoreSdks" />
/// <inheritdoc cref="DotNetCoreListAsString" />
public class DotNetCoreSdks : DotNetCoreListAsString, IDotNetCoreSdks
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public DotNetCoreSdks()
        : base("sdks")
    {
    }
}