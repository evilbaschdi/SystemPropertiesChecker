using System;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;
using Spectre.Console;
using Unity;

namespace SystemPropertiesChecker.Terminal
{
    class Program
    {
        static void Main()
        {
            IVersionContainer versionContainer = new VersionContainer();
            var versionContainerValue = versionContainer.Value;

            var currentVersionText = versionContainerValue.Resolve<IWindowsVersionDictionary>().Value;
            var otherText = versionContainerValue.Resolve<IOtherInformationText>().Value;
            var dotNetVersionText = versionContainerValue.Resolve<IDotNetVersion>().Value;
            var dotNetCoreInfo = versionContainerValue.Resolve<IDotNetCoreInfo>().Value;
            var sourceOsCollection = versionContainerValue.Resolve<ISourceOsCollection>().Value;

            //WINDOWS
            var windowsTable = new Table()
                               .Title("WINDOWS")
                               .Centered()
                               .Border(TableBorder.Square)
                               .BorderColor(Color.Red)
                               .AddColumn(new TableColumn("[u]Key[/]"))
                               .AddColumn(new TableColumn("[u]Value[/]"));

            foreach (var (key, value) in currentVersionText)
            {
                windowsTable.AddRow($"[blue]{key}[/]", $"[white]{value}[/]");
            }

            AnsiConsole.Render(windowsTable);
            //HISTORY
            var historyTable = new Table()
                               .Title("HISTORY")
                               .Centered()
                               .Border(TableBorder.Square)
                               .BorderColor(Color.Red)
                               .AddColumn(new TableColumn("[u]Build[/]"))
                               .AddColumn(new TableColumn("[u]Product Name[/]"))
                               .AddColumn(new TableColumn("[u]Release Id[/]"))
                               .AddColumn(new TableColumn("[u]Product Name[/]"));
            foreach (var sourceOs in sourceOsCollection)
            {
                historyTable.AddRow($"[blue]{sourceOs.Build}[/]", $"[white]{sourceOs.ProductName}[/]", $"[white]{sourceOs.ReleaseId}[/]",
                    $"[white]{sourceOs.InstallDate:yyyy-MM-dd HH:mm:ss}[/]");
            }

            AnsiConsole.Render(historyTable);
            //.NET FRAMEWORK
            var dotnetTable = new Table()
                              .Title(".NET FRAMEWORK")
                              .Centered()
                              .Border(TableBorder.Square)
                              .BorderColor(Color.Red)
                              .AddColumn(new TableColumn($"[u]{dotNetVersionText[0].TrimEnd(':')}[/]"));

            foreach (var line in dotNetVersionText.GetRange(1, dotNetVersionText.Count - 1))
            {
                dotnetTable.AddRow($"[white]{line}[/]");
            }

            AnsiConsole.Render(dotnetTable);
            //.NET CORE
            var dotnetCoreTable = new Table()
                                  .Title(".NET CORE")
                                  .Centered()
                                  .Border(TableBorder.Square)
                                  .BorderColor(Color.Red)
                                  .AddColumn(new TableColumn("[u]Key[/]"))
                                  .AddColumn(new TableColumn("[u]Value[/]"));
            foreach (var (key, value) in dotNetCoreInfo)
            {
                dotnetCoreTable.AddRow($"[blue]{key}[/]", $"[white]{value}[/]");
            }

            AnsiConsole.Render(dotnetCoreTable);
            //OTHER
            var otherTable = new Table()
                             .Title("OTHER")
                             .Centered()
                             .Border(TableBorder.Square)
                             .BorderColor(Color.Red)
                             .AddColumn(new TableColumn("[u]Key[/]"))
                             .AddColumn(new TableColumn("[u]Value[/]"));

            foreach (var (key, value) in otherText)
            {
                otherTable.AddRow($"[blue]{key}[/]", $"[white]{value}[/]");
            }

            AnsiConsole.Render(otherTable);

            Console.ReadLine();
            versionContainerValue.Dispose();
        }
    }
}