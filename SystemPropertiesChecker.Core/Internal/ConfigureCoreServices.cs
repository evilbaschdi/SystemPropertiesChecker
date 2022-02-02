using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
public class ConfigureCoreServices : IConfigureCoreServices
{
    /// <inheritdoc />
    public void RunFor(IServiceCollection services)
    {
        if (services == null)
        {
            throw new ArgumentNullException(nameof(services));
        }

        services.AddScoped<IDotNetCoreInfo, DotNetCoreInfo>();
        services.AddScoped<IDotNetVersion, DotNetVersion>();
        services.AddScoped<IDotNetVersionReleaseKeyMapping, DotNetVersionReleaseKeyMapping>();
        services.AddScoped<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
        services.AddScoped<IExecutePowerShellCommand, ExecutePowerShellCommand>();
        services.AddScoped<IInsiderChannel, InsiderChannel>();
        services.AddScoped<IOtherInformationText, OtherInformationText>();
        services.AddScoped<IPasswordExpirationDate, PasswordExpirationDate>();
        services.AddScoped<IPasswordExpirationMessage, PasswordExpirationMessage>();
        services.AddScoped<IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion, RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion>();
        services.AddScoped<IRegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection, RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection>();
        services.AddScoped<ISourceOsCollection, HklmSystemSetupSourcesInstallDates>();
        services.AddScoped<IWindowsFeatureExperiencePackVersion, WindowsFeatureExperiencePackVersion>();
        services.AddScoped<IWindowsVersionDictionary, WindowsVersionDictionary>();
        services.AddScoped<IWindowsVersionInformation, WindowsVersionInformation>();
        services.AddScoped<IWindowsVersionInformationModel, WindowsVersionInformationModel>();
    }
}