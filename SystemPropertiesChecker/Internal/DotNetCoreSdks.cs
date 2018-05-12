using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SystemPropertiesChecker.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    public class DotNetCoreSdks : IDotNetCoreSdks
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
                    process.SetHiddenProcessFor("dotnet", "--list-sdks");
                    process.Start();
                    if (!process.ReadStandardError().Contains("Unknown option: --list-sdks"))
                    {
                        list.AddRange(from item in process.ReadStandardOutput() select item.Contains("[") ? item.Split('[').First() : item);
                    }

                    process.WaitForExit();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                return list;
            }
        }
    }
}