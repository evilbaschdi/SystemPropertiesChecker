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
                versionContainer.RegisterType<IDotNetVersionReleaseKeyMapping, DotNetVersionReleaseKeyMapping>();
                versionContainer.RegisterType<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
                versionContainer.RegisterType<IDotNetVersion, DotNetVersion>();
                versionContainer.RegisterType<IDotNetCoreInfo, DotNetCoreInfo>();
                versionContainer.RegisterType<IRegistryValueFor, HklmSoftwareMicrosoftWindowsNtCurrentVersion>();
                versionContainer.RegisterType<ISourceOsCollection, HklmSystemSetupSourcesInstallDates>();
                versionContainer.RegisterType<IWindowsVersionInformationModel, WindowsVersionInformationModel>();
                versionContainer.RegisterType<IWindowsVersionInformation, WindowsVersionInformation>();
                versionContainer.RegisterType<IWindowsVersionDictionary, WindowsVersionDictionary>();
                versionContainer.RegisterType<IOtherInformationText, OtherInformationText>();
                versionContainer.RegisterType<IPasswordExpirationDate, PasswordExpirationDate>();
                versionContainer.RegisterType<IPasswordExpirationMessage, PasswordExpirationMessage>();

                return versionContainer;
            }
        }
    }
}