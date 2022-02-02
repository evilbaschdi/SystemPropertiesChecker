using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using EvilBaschdi.Core;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
public abstract class RegistryValueFor : IRegistryValueFor
{
    private readonly RegistryHive _registryHive;
    private readonly string _subKey;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="subKey"></param>
    /// <param name="registryHive"></param>
    protected RegistryValueFor([NotNull] string subKey, RegistryHive registryHive)
    {
        if (!Enum.IsDefined(typeof(RegistryHive), registryHive))
        {
            throw new InvalidEnumArgumentException(nameof(registryHive), (int)registryHive, typeof(RegistryHive));
        }

        _subKey = subKey ?? throw new ArgumentNullException(nameof(subKey));
        _registryHive = registryHive;
    }

    /// <inheritdoc />
    public string ValueFor([NotNull] string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (!OperatingSystem.IsWindows())
        {
            return string.Empty;
        }

        var bits = Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32;

        var localMachine = RegistryKey.OpenBaseKey(_registryHive, bits);
        var regPath = localMachine.OpenSubKey(_subKey);

        return regPath?.GetValue(value) != null
            ? regPath.GetValue(value)?.ToString()
            : string.Empty;
    }
}

/// <inheritdoc />
public interface IExecutePowerShellCommand : IValueFor<string, string>
{
}

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

/// <inheritdoc />
public interface IWindowsFeatureExperiencePackVersion : IValue<string>
{
}

/// <inheritdoc />
public class WindowsFeatureExperiencePackVersion : IWindowsFeatureExperiencePackVersion
{
    private readonly IExecutePowerShellCommand _executePowerShellCommand;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="executePowerShellCommand"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public WindowsFeatureExperiencePackVersion([NotNull] IExecutePowerShellCommand executePowerShellCommand)
    {
        _executePowerShellCommand = executePowerShellCommand ?? throw new ArgumentNullException(nameof(executePowerShellCommand));
    }

    /// <inheritdoc />
    public string Value => _executePowerShellCommand.ValueFor("Get-AppPackage -Name  MicrosoftWindows.Client.CBS | select -First 1 -ExpandProperty Version");
}