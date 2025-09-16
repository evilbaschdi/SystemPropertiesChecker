using Spectre.Console;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteWindowsTable : IWriteWindowsTable
{
    private readonly IWindowsVersionDictionary _windowsVersionDictionary;
    private readonly IAccentColorHelper _accentColorHelper;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="windowsVersionDictionary"></param>
    /// <param name="accentColorHelper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteWindowsTable(IWindowsVersionDictionary windowsVersionDictionary, IAccentColorHelper accentColorHelper)
    {
        _windowsVersionDictionary = windowsVersionDictionary ??
                                    throw new ArgumentNullException(nameof(windowsVersionDictionary));
        _accentColorHelper = accentColorHelper ?? throw new ArgumentNullException(nameof(accentColorHelper));
    }

    /// <inheritdoc />
    public void Run()
    {
        var currentVersionText = _windowsVersionDictionary.Value ?? new Dictionary<string, string>();

        var color = _accentColorHelper.SpectreConsoleColor;
        var markup = color.ToMarkup();

        var windowsTable = new Table()
                           .Title("WINDOWS")
                           .Centered()
                           .Border(TableBorder.Square)
                           .BorderColor(color)
                           .AddColumn(new("[u]Key[/]"))
                           .AddColumn(new("[u]Value[/]"));

        foreach (var (key, value) in currentVersionText)
        {
            windowsTable.AddRow($"[{markup}]{key}[/]", $"[white]{value}[/]");
        }

        AnsiConsole.Write(windowsTable);
    }
}