using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using SystemPropertiesChecker.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    public class DotNetCoreVersion : IDotNetCoreVersion
    {
        private readonly IDotNetCoreRuntimes _dotNetCoreRuntimes;
        private readonly IDotNetCoreSdks _dotNetCoreSdks;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="dotNetCoreRuntimes"></param>
        /// <param name="dotNetCoreSdks"></param>
        public DotNetCoreVersion(IDotNetCoreRuntimes dotNetCoreRuntimes, IDotNetCoreSdks dotNetCoreSdks)
        {
            _dotNetCoreRuntimes = dotNetCoreRuntimes ?? throw new ArgumentNullException(nameof(dotNetCoreRuntimes));
            _dotNetCoreSdks = dotNetCoreSdks ?? throw new ArgumentNullException(nameof(dotNetCoreSdks));
        }

        /// <inheritdoc />
        public string Value
        {
            get
            {
                var stringBuilder = new StringBuilder();

                stringBuilder.AppendLine("currently installed version:");

                try
                {
                    var process = new Process();
                    process.SetHiddenProcessFor("dotnet", "--version");
                    process.Start();
                    stringBuilder.AppendLine(process.StandardOutput.ReadToEnd());
                    process.WaitForExit();
                
                    //if (_dotNetCoreRuntimes.Value?.Any() == true)
                    //{
                    //    stringBuilder.AppendLine("currently installed runtimes:");
                    //    stringBuilder.AppendLine(string.Join(", ", _dotNetCoreRuntimes.Value));
                    //    stringBuilder.AppendLine();
                    //}

                    if (_dotNetCoreSdks.Value?.Any() == true)
                    {
                        stringBuilder.AppendLine("currently installed sdks:");
                        stringBuilder.AppendLine(string.Join(", ", _dotNetCoreSdks.Value));
                        stringBuilder.AppendLine();
                    }
                }
                catch
                {
                    stringBuilder.AppendLine("(none)");
                }

                return stringBuilder.ToString().Trim();
            }
        }
    }
}