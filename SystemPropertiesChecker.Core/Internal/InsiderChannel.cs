using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
// ReSharper disable once ClassNeverInstantiated.Global
public class InsiderChannel : IInsiderChannel
{
    private readonly IRegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection _localMachineSoftwareMicrosoftWindowsSelfHostUiSelection;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="localMachineSoftwareMicrosoftWindowsSelfHostUiSelection"></param>
    public InsiderChannel([NotNull] IRegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection localMachineSoftwareMicrosoftWindowsSelfHostUiSelection)
    {
        _localMachineSoftwareMicrosoftWindowsSelfHostUiSelection = localMachineSoftwareMicrosoftWindowsSelfHostUiSelection ??
                                                                   throw new ArgumentNullException(nameof(localMachineSoftwareMicrosoftWindowsSelfHostUiSelection));
    }

    /// <inheritdoc />
    public string Value
    {
        get
        {
            var uIBranch = _localMachineSoftwareMicrosoftWindowsSelfHostUiSelection.ValueFor("UIBranch") ?? string.Empty;
            var uIContentType = _localMachineSoftwareMicrosoftWindowsSelfHostUiSelection.ValueFor("UIContentType") ?? string.Empty;
            var uIRing = _localMachineSoftwareMicrosoftWindowsSelfHostUiSelection.ValueFor("UIRing") ?? string.Empty;

            if (!uIBranch.Equals("external", StringComparison.OrdinalIgnoreCase))
            {
                return uIBranch;
            }

            return uIContentType switch
            {
                "Active" => uIRing switch
                {
                    "WIF" => "Canary",
                    "WIS" => "Beta",
                    _ => uIRing
                },
                "Current" => uIRing switch
                {
                    "RP" => "Release Preview",
                    _ => uIRing,
                },
                _ => uIBranch
            };
        }
    }
}