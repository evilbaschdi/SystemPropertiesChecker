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
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        var stringBuilder = new StringBuilder();

        try
        {
            var process = new Process();
            process.SetHiddenProcessFor(@"C:\windows\system32\windowspowershell\v1.0\powershell.exe", value);
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