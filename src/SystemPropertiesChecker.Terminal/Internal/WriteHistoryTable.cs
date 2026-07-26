using EvilBaschdi.About.Terminal;
using Spectre.Console;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteHistoryTable : IWriteHistoryTable
{
    private readonly ISourceOsCollection _sourceOsCollection;
    private readonly IAccentColorHelper _accentColorHelper;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="sourceOsCollection"></param>
    /// <param name="accentColorHelper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteHistoryTable(ISourceOsCollection sourceOsCollection, IAccentColorHelper accentColorHelper)
    {
        _sourceOsCollection = sourceOsCollection ?? throw new ArgumentNullException(nameof(sourceOsCollection));
        _accentColorHelper = accentColorHelper ?? throw new ArgumentNullException(nameof(accentColorHelper));
    }

    /// <inheritdoc />
    public void Run()
    {
        AnsiConsole.Status()
                   .Start("Processing...", _ =>
                                           {
                                               var sourceOsCollection = _sourceOsCollection.Value ?? [];

                                               var color = _accentColorHelper.SpectreConsoleColor;
                                               var markup = color.ToMarkup();

                                               var historyTable = new Table()
                                                                  .Title("HISTORY")
                                                                  //.Centered()
                                                                  .Border(TableBorder.Square)
                                                                  .BorderColor(color)
                                                                  .AddColumn(new("[u]Build[/]"))
                                                                  .AddColumn(new("[u]Product Name[/]"))
                                                                  .AddColumn(new("[u]Release Id[/]"))
                                                                  .AddColumn(new("[u]Product Name[/]"));
                                               foreach (var sourceOs in sourceOsCollection)
                                               {
                                                   historyTable.AddRow($"[{markup}]{sourceOs.Build}[/]", $"[white]{sourceOs.ProductName}[/]",
                                                       $"[white]{sourceOs.ReleaseId}[/]",
                                                       $"[white]{sourceOs.InstallDate:yyyy-MM-dd HH:mm:ss}[/]");
                                               }

                                               if (sourceOsCollection.Any())
                                               {
                                                   AnsiConsole.Write(historyTable);
                                               }
                                           }
                   );
    }
}