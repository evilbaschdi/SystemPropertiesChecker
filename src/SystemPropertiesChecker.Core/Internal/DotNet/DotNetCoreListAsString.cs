using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class DotNetCoreListAsString : IDotNetCoreListAsString
{
    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="listName"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public DotNetCoreListAsString([NotNull] string listName)
    {
        Value = listName ?? throw new ArgumentNullException(nameof(listName));
    }

    /// <inheritdoc />
    public string Value
    {
        get
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"currently installed {field}:");
            var list = new List<string>();

            try
            {
                using var process = new Process();
                process.SetHiddenProcessFor("dotnet", $"--list-{field}");
                process.Start();

                if (!process.ReadStandardError().Contains($"Unknown option: --list-{field}"))
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