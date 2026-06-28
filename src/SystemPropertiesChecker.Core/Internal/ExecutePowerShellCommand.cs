using System.Diagnostics;
using System.Text;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
public class ExecutePowerShellCommand : IExecutePowerShellCommand
{
    /// <inheritdoc />
    public string ValueFor([NotNull] string value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var stringBuilder = new StringBuilder();

        try
        {
            using var process = new Process();
            process.StartInfo = new ProcessStartInfo
            {
                FileName = @"C:\windows\system32\windowspowershell\v1.0\powershell.exe",
                Arguments = "-NoProfile -NonInteractive -Command -",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardInput = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                StandardOutputEncoding = Encoding.UTF8
            };
            
            process.Start();

            using (var writer = process.StandardInput)
            {
                writer.Write(value);
            }

            stringBuilder.AppendLine(process.StandardOutput.ReadToEnd().Trim());
            process.WaitForExit();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            stringBuilder.AppendLine("(none)");
        }

        return stringBuilder.ToString();
    }
}