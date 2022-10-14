using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class DotNetCoreListAsString : IDotNetCoreListAsString
{
    private readonly string _listName;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="listName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DotNetCoreListAsString([NotNull] string listName)
    {
        _listName = listName ?? throw new ArgumentNullException(nameof(listName));
    }

    /// <inheritdoc />
    public string Value
    {
        get
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"currently installed {_listName}:");
            var list = new List<string>();

            try
            {
                using var process = new Process();
                process.SetHiddenProcessFor("dotnet", $"--list-{_listName}");
                process.Start();

                if (!process.ReadStandardError().Contains($"Unknown option: --list-{_listName}"))
                {
                    list.AddRange(from item
                                      in process.ReadStandardOutput()
                                  select item.Contains('[')
                                      ? item.Split('[').First()
                                      : item);
                }

                process.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                stringBuilder.AppendLine("(none)");
            }

            stringBuilder.AppendLine(string.Join(", ", list.OrderByDescending(i => i.Trim()).ToList()));

            return stringBuilder.ToString();
        }
    }
}