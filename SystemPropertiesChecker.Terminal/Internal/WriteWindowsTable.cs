using Spectre.Console;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteWindowsTable : IWriteWindowsTable
{
    private readonly IWindowsVersionDictionary _windowsVersionDictionary;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="windowsVersionDictionary"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteWindowsTable(IWindowsVersionDictionary windowsVersionDictionary)
    {
        _windowsVersionDictionary = windowsVersionDictionary ?? throw new ArgumentNullException(nameof(windowsVersionDictionary));
    }

    /// <inheritdoc />
    public void Run()
    {
        var currentVersionText = _windowsVersionDictionary.Value ?? new Dictionary<string, string>();

        var windowsTable = new Table()
                           .Title("WINDOWS")
                           .Centered()
                           .Border(TableBorder.Square)
                           .BorderColor(Color.Red)
                           .AddColumn(new("[u]Key[/]"))
                           .AddColumn(new("[u]Value[/]"));

        foreach (var (key, value) in currentVersionText)
        {
            windowsTable.AddRow($"[blue]{key}[/]", $"[white]{value}[/]");
        }

        AnsiConsole.Write(windowsTable);
    }
}