using EvilBaschdi.About.Terminal;
using Spectre.Console;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteOtherTable : IWriteOtherTable
{
    private readonly IOtherInformationText _otherInformationText;
    private readonly IAccentColorHelper _accentColorHelper;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="otherInformationText"></param>
    /// <param name="accentColorHelper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteOtherTable(IOtherInformationText otherInformationText, IAccentColorHelper accentColorHelper)
    {
        _otherInformationText = otherInformationText ?? throw new ArgumentNullException(nameof(otherInformationText));
        _accentColorHelper = accentColorHelper ?? throw new ArgumentNullException(nameof(accentColorHelper));
    }

    /// <inheritdoc />
    public void Run()
    {
        AnsiConsole.Status()
                   .Start("Processing...", _ =>
                                           {
                                               var otherText = _otherInformationText.Value ?? [];

                                               var color = _accentColorHelper.SpectreConsoleColor;
                                               var markup = color.ToMarkup();

                                               var otherTable = new Table()
                                                                .Title("OTHER")
                                                                //.Centered()
                                                                .Border(TableBorder.Square)
                                                                .BorderColor(color)
                                                                .AddColumn(new("[u]Key[/]"))
                                                                .AddColumn(new("[u]Value[/]"));

                                               foreach (var (key, value) in otherText)
                                               {
                                                   otherTable.AddRow($"[{markup}]{key}[/]", $"[white]{value.Replace('[', '\'').Replace(']', '\'')}[/]");
                                               }

                                               AnsiConsole.Write(otherTable);
                                           }
                   );
    }
}