using Spectre.Console;
using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteDotNetTable : IWriteDotNetTable
{
    private readonly IDotNetVersion _dotNetVersion;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dotNetVersion"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteDotNetTable(IDotNetVersion dotNetVersion)
    {
        _dotNetVersion = dotNetVersion ?? throw new ArgumentNullException(nameof(dotNetVersion));
    }

    /// <inheritdoc />
    public void Run()
    {
        var dotNetVersionText = _dotNetVersion.Value ?? new List<string>();

        var dotnetTable = new Table()
                          .Title(".NET FRAMEWORK")
                          .Centered()
                          .Border(TableBorder.Square)
                          .BorderColor(Color.Red)
                          .AddColumn(new($"[u]{dotNetVersionText[0].TrimEnd(':')}[/]"));

        foreach (var line in dotNetVersionText.GetRange(1, dotNetVersionText.Count - 1))
        {
            dotnetTable.AddRow($"[white]{line}[/]");
        }

        AnsiConsole.Write(dotnetTable);
    }
}