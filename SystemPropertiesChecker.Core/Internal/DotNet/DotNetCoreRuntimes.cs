using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <inheritdoc />
    public class DotNetCoreRuntimes : IDotNetCoreRuntimes
    {
        /// <inheritdoc />
        public string Value
        {
            get
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.AppendLine("currently installed runtimes:");
                var list = new List<string>();

                //try
                //{
                var process = new Process();
                process.SetHiddenProcessFor("dotnet", "--list-runtimes");
                process.Start();
                    if (!process.ReadStandardError().Contains("Unknown option: --list-runtimes"))
                    {
                        list.AddRange(from item in process.ReadStandardOutput() select item.Contains("[") ? item.Split('[').First() : item);
                    }

                   // process.WaitForExit();
                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine(ex.Message);
                //    stringBuilder.AppendLine("(none)");
                //}

                stringBuilder.AppendLine(string.Join(", ", list.OrderByDescending(i => i).ToList()));
                stringBuilder.AppendLine();

                return stringBuilder.ToString();
            }
        }
    }
}