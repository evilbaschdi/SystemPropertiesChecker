using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <inheritdoc />
    public class DotNetCoreSdks : IDotNetCoreSdks
    {
        /// <inheritdoc />
        public string Value
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("currently installed sdks:");
                var list = new List<string>();

                try
                {
                    var process = new Process();
                    process.SetHiddenProcessFor("dotnet", "--list-sdks");
                    process.Start();

                    if (!process.ReadStandardError().Contains("Unknown option: --list-sdks"))
                    {
                        list.AddRange(from item
                                          in process.ReadStandardOutput()
                                      select item.Contains("[")
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
}