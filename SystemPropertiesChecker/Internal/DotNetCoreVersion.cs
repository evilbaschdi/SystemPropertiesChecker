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

                if (_dotNetCoreRuntimes.Value?.Any() == true)
                {
                    stringBuilder.AppendLine("currently installed runtimes:");
                    _dotNetCoreRuntimes.Value.ForEach(x => stringBuilder.AppendLine(x));
                    stringBuilder.AppendLine();
                }

                if (_dotNetCoreSdks.Value?.Any() == true)
                {
                    stringBuilder.AppendLine("currently installed sdks:");
                    _dotNetCoreSdks.Value.ForEach(x => stringBuilder.AppendLine(x));
                    stringBuilder.AppendLine();
                }

                if (!string.IsNullOrWhiteSpace(stringBuilder.ToString()))
                {
                    return stringBuilder.ToString().Trim();
                }

                stringBuilder.AppendLine("currently installed version:");

                var process = new Process();
                process.SetHiddenProcessFor("dotnet", "--version");
                process.Start();
                stringBuilder.AppendLine(process.StandardOutput.ReadToEnd());
                process.WaitForExit();

                return stringBuilder.ToString().Trim();
            }
        }
    }
}