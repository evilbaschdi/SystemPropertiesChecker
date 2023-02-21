using Spectre.Console;
using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteDotNetCoreTable : IWriteDotNetCoreTable
{
    private readonly IDotNetCoreInfo _dotNetCoreInfo;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dotNetCoreInfo"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteDotNetCoreTable(IDotNetCoreInfo dotNetCoreInfo)
    {
        _dotNetCoreInfo = dotNetCoreInfo ?? throw new ArgumentNullException(nameof(dotNetCoreInfo));
    }

    /// <inheritdoc />
    public void Run()
    {
        var dotNetCoreInfo = _dotNetCoreInfo.Value ?? new List<KeyValuePair<string, string>>();

        var dotnetCoreTable = new Table()
                              .Title(".NET CORE")
                              .Centered()
                              .Border(TableBorder.Square)
                              .BorderColor(Color.Red)
                              .AddColumn(new("[u]Key[/]"))
                              .AddColumn(new("[u]Value[/]"));
        foreach (var (key, value) in dotNetCoreInfo)
        {
            dotnetCoreTable.AddRow($"[blue]{key}[/]", $"[white]{value}[/]");
        }

        AnsiConsole.Write(dotnetCoreTable);
    }
}