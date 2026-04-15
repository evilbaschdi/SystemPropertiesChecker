using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

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
    public string Value => _executePowerShellCommand.ValueFor("Get-AppPackage -Name  MicrosoftWindows.Client.CBS " +
                                                              "| select -First 1 -ExpandProperty Version")?.Trim();
}