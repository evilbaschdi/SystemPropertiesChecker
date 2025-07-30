namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class ParseReleaseKeyByReleaseKeyMappingList : IParseReleaseKeyByReleaseKeyMappingList
{
    private readonly IDotNetVersionReleaseKeyMappingList _dotNetVersionReleaseKeyMappingList;

    /// <summary>
    ///     Constructor.
    /// </summary>
    public ParseReleaseKeyByReleaseKeyMappingList(IDotNetVersionReleaseKeyMappingList dotNetVersionReleaseKeyMappingList)
    {
        _dotNetVersionReleaseKeyMappingList = dotNetVersionReleaseKeyMappingList ?? throw new ArgumentNullException(nameof(dotNetVersionReleaseKeyMappingList));
    }

    /// <inheritdoc />
    public string ValueFor(string releaseKey)
    {
        ArgumentNullException.ThrowIfNull(releaseKey);

        //releaseKey = 460900;
        var value = _dotNetVersionReleaseKeyMappingList.ValueFor(releaseKey);

        return !string.IsNullOrWhiteSpace(value) ? $"{value} (Release key: '{releaseKey}')" : "No 4.5 or later version detected";
    }
}