using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;
using Unity;

namespace SystemPropertiesChecker.Core.Internal
{
    public class VersionContainer : IVersionContainer
    {
        public UnityContainer Value
        {
            get
            {
                var versionContainer = new UnityContainer();
                versionContainer.RegisterType<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
                versionContainer.RegisterType<IDotNetVersion, DotNetVersion>();
                versionContainer.RegisterType<IDotNetCoreSdks, DotNetCoreSdks>();
                versionContainer.RegisterType<IDotNetCoreRuntimes, DotNetCoreRuntimes>();
                versionContainer.RegisterType<IDotNetCoreVersion, DotNetCoreVersion>();
                versionContainer.RegisterType<IRegistryValueFor, HklmSoftwareMicrosoftWindowsNtCurrentVersion>();
                versionContainer.RegisterType<ISourceOsCollection, HklmSystemSetupSourcesInstallDates>();
                versionContainer.RegisterType<IWindowsVersionInformationModel, WindowsVersionInformationModel>();
                versionContainer.RegisterType<IWindowsVersionInformation, WindowsVersionInformation>();
                versionContainer.RegisterType<ICurrentVersionText, CurrentVersionText>();
                versionContainer.RegisterType<IWindowsVersionText, WindowsVersionText>();
                versionContainer.RegisterType<IOtherInformationText, OtherInformationText>();
                versionContainer.RegisterType<IPasswordExpirationDate, PasswordExpirationDate>();
                versionContainer.RegisterType<IPasswordExpirationMessage, PasswordExpirationMessage>();

                return versionContainer;
            }
        }
    }
}