using System.Diagnostics;
using System.Text;

namespace SystemPropertiesChecker.Core.Internal.DotNet;

/// <inheritdoc />
public class DotNetCoreVersion : IDotNetCoreVersion
{
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
                stringBuilder.AppendLine(process.StandardOutput.ReadToEnd().Trim());

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