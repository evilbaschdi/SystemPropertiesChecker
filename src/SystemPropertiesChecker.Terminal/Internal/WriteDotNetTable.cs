using EvilBaschdi.About.Terminal;
using Spectre.Console;
using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <inheritdoc />
public class WriteDotNetTable : IWriteDotNetTable
{
    private readonly IDotNetVersion _dotNetVersion;
    private readonly IAccentColorHelper _accentColorHelper;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dotNetVersion"></param>
    /// <param name="accentColorHelper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WriteDotNetTable(IDotNetVersion dotNetVersion, IAccentColorHelper accentColorHelper)
    {
        _dotNetVersion = dotNetVersion ?? throw new ArgumentNullException(nameof(dotNetVersion));
        _accentColorHelper = accentColorHelper ?? throw new ArgumentNullException(nameof(accentColorHelper));
    }

    /// <inheritdoc />
    public void Run()
    {
        AnsiConsole.Status()
                   .Start("Processing...", _ =>
                                           {
                                               var dotNetVersionText = _dotNetVersion.Value ?? [];

                                               var color = _accentColorHelper.SpectreConsoleColor;
                                               var markup = color.ToMarkup();

                                               var dotnetTable = new Table()
                                                                 .Title(".NET FRAMEWORK")
                                                                 //.Centered()
                                                                 .Border(TableBorder.Square)
                                                                 .BorderColor(color)
                                                                 .AddColumn(new($"[u]{dotNetVersionText[0].TrimEnd(':')}[/]"));

                                               foreach (var line in dotNetVersionText.GetRange(1, dotNetVersionText.Count - 1))
                                               {
                                                   dotnetTable.AddRow($"[{markup}]{line}[/]");
                                               }

                                               AnsiConsole.Write(dotnetTable);
                                           }
                   );
    }
}