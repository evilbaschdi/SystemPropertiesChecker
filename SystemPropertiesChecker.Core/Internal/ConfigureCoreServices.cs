using System;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPropertiesChecker.Core.Internal
{
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

            services.AddScoped<IDotNetVersionReleaseKeyMapping, DotNetVersionReleaseKeyMapping>();
            services.AddScoped<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
            services.AddScoped<IDotNetVersion, DotNetVersion>();
            services.AddScoped<IDotNetCoreInfo, DotNetCoreInfo>();
            services.AddScoped<IRegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion, RegistryHiveLocalMachineSoftwareMicrosoftWindowsNtCurrentVersion>();
            services.AddScoped<IRegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection, RegistryHiveLocalMachineSoftwareMicrosoftWindowsSelfHostUiSelection>();
            services.AddScoped<IInsiderChannel, InsiderChannel>();
            services.AddScoped<ISourceOsCollection, HklmSystemSetupSourcesInstallDates>();
            services.AddScoped<IWindowsVersionInformationModel, WindowsVersionInformationModel>();
            services.AddScoped<IWindowsVersionInformation, WindowsVersionInformation>();
            services.AddScoped<IWindowsVersionDictionary, WindowsVersionDictionary>();
            services.AddScoped<IOtherInformationText, OtherInformationText>();
            services.AddScoped<IPasswordExpirationDate, PasswordExpirationDate>();
            services.AddScoped<IPasswordExpirationMessage, PasswordExpirationMessage>();
        }
    }
}