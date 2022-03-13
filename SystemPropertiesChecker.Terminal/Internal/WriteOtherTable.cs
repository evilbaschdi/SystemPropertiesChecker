using Spectre.Console;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteOtherTable : IWriteOtherTable
{
    private readonly IOtherInformationText _otherInformationText;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="otherInformationText"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteOtherTable(IOtherInformationText otherInformationText)
    {
        _otherInformationText = otherInformationText ?? throw new ArgumentNullException(nameof(otherInformationText));
    }

    /// <inheritdoc />
    public void Run()
    {
        var otherText = _otherInformationText.Value ?? new List<KeyValuePair<string, string>>();
        var otherTable = new Table()
                         .Title("OTHER")
                         .Centered()
                         .Border(TableBorder.Square)
                         .BorderColor(Color.Red)
                         .AddColumn(new("[u]Key[/]"))
                         .AddColumn(new("[u]Value[/]"));

        foreach (var (key, value) in otherText)
        {
            otherTable.AddRow($"[blue]{key}[/]", $"[white]{value}[/]");
        }

        AnsiConsole.Write(otherTable);
    }
}