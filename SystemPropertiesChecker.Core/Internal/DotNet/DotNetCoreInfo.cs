using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <inheritdoc />
    public class DotNetCoreInfo : IDotNetCoreInfo
    {
        /// <inheritdoc />
        public Dictionary<string,string> Value
        {
            get
            {
                var dictionary = new Dictionary<string,string>();

                try
                {
                    var process = new Process();
                    process.SetHiddenProcessFor("dotnet", "--info");
                    process.Start();


                    foreach (var item in process.ReadStandardOutput())
                    {
                        var line = (item.Contains("[") ? item.Split('[').First() : item).Trim();


                        if (line.EndsWith(":"))
                        {
                            dictionary.Add(line, string.Empty);
                        }
                        else
                        {
                            var lastKey = dictionary.Last().Key;
                            dictionary[lastKey] = $"{dictionary[lastKey]}{Environment.NewLine}{line}";
                        }
                    }

                    process.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    dictionary.Add("(none)",string.Empty);
                }


                return dictionary;
            }
        }
    }
}