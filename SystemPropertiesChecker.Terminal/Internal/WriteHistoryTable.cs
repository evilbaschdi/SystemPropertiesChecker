using System.Collections.ObjectModel;
using Spectre.Console;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteHistoryTable : IWriteHistoryTable
{
    private readonly ISourceOsCollection _sourceOsCollection;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="sourceOsCollection"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteHistoryTable(ISourceOsCollection sourceOsCollection)
    {
        _sourceOsCollection = sourceOsCollection ?? throw new ArgumentNullException(nameof(sourceOsCollection));
    }

    /// <inheritdoc />
    public void Run()
    {
        var sourceOsCollection = _sourceOsCollection.Value ?? new ObservableCollection<SourceOs>();

        var historyTable = new Table()
                           .Title("HISTORY")
                           .Centered()
                           .Border(TableBorder.Square)
                           .BorderColor(Color.Red)
                           .AddColumn(new("[u]Build[/]"))
                           .AddColumn(new("[u]Product Name[/]"))
                           .AddColumn(new("[u]Release Id[/]"))
                           .AddColumn(new("[u]Product Name[/]"));
        foreach (var sourceOs in sourceOsCollection)
        {
            historyTable.AddRow($"[blue]{sourceOs.Build}[/]", $"[white]{sourceOs.ProductName}[/]", $"[white]{sourceOs.ReleaseId}[/]",
                $"[white]{sourceOs.InstallDate:yyyy-MM-dd HH:mm:ss}[/]");
        }

        if (sourceOsCollection.Any())
        {
            AnsiConsole.Write(historyTable);
        }
    }
}