using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemPropertiesChecker.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    public class DotNetCoreRuntimes : IDotNetCoreRuntimes
    {
        /// <inheritdoc />
        public List<string> Value
        {
            get
            {
                var list = new List<string>();
                try
                {
                    var process = new Process();
                    process.SetHiddenProcessFor("dotnet", "--list-runtimes");
                    process.Start();
                    if (!process.ReadStandardError().Contains("Unknown option: --list-runtimes"))
                    {
                        list.AddRange(from item in process.ReadStandardOutput() select item.Contains("[") ? item.Split('[').First() : item);
                    }

                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return list.OrderByDescending(i => i).ToList();
            }
        }
    }
}