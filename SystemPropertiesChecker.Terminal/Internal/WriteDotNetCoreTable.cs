using EvilBaschdi.About.Terminal;
using Spectre.Console;
using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteDotNetCoreTable : IWriteDotNetCoreTable
{
    private readonly IDotNetCoreInfo _dotNetCoreInfo;
    private readonly IAccentColorHelper _accentColorHelper;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dotNetCoreInfo"></param>
    /// <param name="accentColorHelper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteDotNetCoreTable(IDotNetCoreInfo dotNetCoreInfo, IAccentColorHelper accentColorHelper)
    {
        _dotNetCoreInfo = dotNetCoreInfo ?? throw new ArgumentNullException(nameof(dotNetCoreInfo));
        _accentColorHelper = accentColorHelper ?? throw new ArgumentNullException(nameof(accentColorHelper));
    }

    /// <inheritdoc />
    public void Run()
    {
        var dotNetCoreInfo = _dotNetCoreInfo.Value ?? new List<KeyValuePair<string, string>>();

        var color = _accentColorHelper.SpectreConsoleColor;
        var markup = color.ToMarkup();

        var dotnetCoreTable = new Table()
                              .Title(".NET CORE")
                              .Centered()
                              .Border(TableBorder.Square)
                              .BorderColor(color)
                              .AddColumn(new("[u]Key[/]"))
                              .AddColumn(new("[u]Value[/]"));
        foreach (var (key, value) in dotNetCoreInfo)
        {
            dotnetCoreTable.AddRow($"[{markup}]{key}[/]", $"[white]{value}[/]");
        }

        AnsiConsole.Write(dotnetCoreTable);
    }
}