using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary />
public static class ConfigureCoreServices
{
    /// <summary />
    public static void AddCoreServices(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<ICheckNetFrameworkVersion45OrHigher, CheckNetFrameworkVersion45OrHigher>();
        services.AddScoped<IDotNetCoreInfo, DotNetCoreInfo>();
        services.AddScoped<IDotNetVersion, DotNetVersion>();
        services.AddScoped<IDotNetVersionReleaseKeyMapping, DotNetVersionReleaseKeyMapping>();
        services.AddScoped<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
        services.AddScoped<IExecutePowerShellCommand, ExecutePowerShellCommand>();
        services.AddScoped<IHandleNetFrameworkSetupNdpKeys, HandleNetFrameworkSetupNdpKeys>();
        services.AddScoped<IHandleNetFrameworkSetupNdpSubKey, HandleNetFrameworkSetupNdpSubKey>();
        services.AddScoped<IInsiderChannel, InsiderChannel>();
        services.AddScoped<INetFrameworkVersionFromRegistry, NetFrameworkVersionFromRegistry>();
        services.AddScoped<IOtherInformationText, OtherInformationText>();
        services.AddScoped<IParseReleaseKeyByReleaseKeyMappingList, ParseReleaseKeyByReleaseKeyMappingList>();
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