using System;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <inheritdoc />
    public class DotNetCoreInfo : IDotNetCoreInfo
    {
        /// <inheritdoc />
        public string Value
        {
            get
            {
                var stringBuilder = new StringBuilder();

                try
                {
                    var process = new Process();
                    process.SetHiddenProcessFor("dotnet", "--info");
                    process.Start();

                    foreach (var item in process.ReadStandardOutput())
                    {
                        stringBuilder.AppendLine(item.Contains("[") ? item.Split('[').First() : item);
                    }

                    process.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    stringBuilder.AppendLine("(none)");
                }


                return stringBuilder.ToString();
            }
        }
    }
}