using System;
using System.Linq;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;

namespace SystemPropertiesChecker.Terminal
{
    class Program
    {
        static void Main(string[] args)
        {
            IRegistryValueFor registryValueFor = new HklmSoftwareMicrosoftWindowsNtCurrentVersion();
            IPasswordExpirationDate passwordExpirationDate = new PasswordExpirationDate();
            IWindowsVersionInformation windowsVersionInformation = new WindowsVersionInformation(registryValueFor, passwordExpirationDate);
            ICurrentVersionText currentVersionText = new CurrentVersionText(windowsVersionInformation);
            IWindowsVersionText windowsVersionText = new WindowsVersionText(windowsVersionInformation);
            IOtherInformationText otherInformationText = new OtherInformationText();
            IDotNetCoreRuntimes dotNetCoreRuntimes = new DotNetCoreRuntimes();
            IDotNetCoreSdks dotNetCoreSdks = new DotNetCoreSdks();
            IDotNetCoreVersion dotNetCoreVersion = new DotNetCoreVersion();
            IDotNetVersionReleaseKeyMappingList dotNetVersionReleaseKeyMappingList = new DotNetVersionReleaseKeyMappingList();
            IDotNetVersion dotNetVersion = new DotNetVersion(dotNetVersionReleaseKeyMappingList);
            ISourceOsCollection sourceOsCollection = new HklmSystemSetupSourcesInstallDates();

            Console.WriteLine("## BASIC ##");
            Console.WriteLine(currentVersionText.Value);
            Console.WriteLine("---");

            Console.WriteLine("## WINDOWS ##");
            Console.WriteLine(windowsVersionText.Value);
            Console.WriteLine("---");

            Console.WriteLine("## HISTORY ##");
            Console.WriteLine();
            Console.WriteLine("{0,20}|{1,20}|{2,20}|{3,20}", "Build", "Product Name", "Release Id", "Install Date");
            foreach (var sourceOs in sourceOsCollection.Value)
            {
                Console.WriteLine("{0,20}|{1,20}|{2,20}|{3,20:yyyy-MM-dd hh:mm:ss}", sourceOs.Build, sourceOs.ProductName, sourceOs.ReleaseId, sourceOs.InstallDate);
            }

            Console.WriteLine("---");

            Console.WriteLine("## OTHER ##");
            Console.WriteLine(otherInformationText.Value);
            Console.WriteLine("---");

            Console.WriteLine("## .NET FRAMEWORK ##");
            Console.WriteLine(dotNetVersion.Value.Aggregate(string.Empty, (c, v) => $"{c}{v}{Environment.NewLine}"));
            Console.WriteLine("---");

            Console.WriteLine("## .NET CORE VERSION ##");
            Console.WriteLine(dotNetCoreVersion.Value);
            Console.WriteLine("---");

            Console.WriteLine("## .NET CORE SDKS ##");
            Console.WriteLine(dotNetCoreSdks.Value);
            Console.WriteLine("---");

            Console.WriteLine("## .NET CORE RUNTIMES ##");
            Console.WriteLine(dotNetCoreRuntimes.Value);

            Console.ReadLine();
        }
    }
}