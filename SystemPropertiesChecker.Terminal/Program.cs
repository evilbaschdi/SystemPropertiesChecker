using System;
using System.Linq;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;
using Unity;

namespace SystemPropertiesChecker.Terminal
{
    class Program
    {
        static void Main()
        {
            IVersionContainer versionContainer = new VersionContainer();
            var versionContainerValue = versionContainer.Value;

            var currentVersionText = versionContainerValue.Resolve<IWindowsVersionDictionary>().Value;
            var otherText = versionContainerValue.Resolve<IOtherInformationText>().Value;
            var dotNetVersionText = versionContainerValue.Resolve<IDotNetVersion>().Value.Aggregate(string.Empty, (c, v) => $"{c}{v}{Environment.NewLine}");
            //var dotNetCoreRuntimes = versionContainerValue.Resolve<IDotNetCoreRuntimes>().Value;
            //var dotNetCoreSdks = versionContainerValue.Resolve<IDotNetCoreSdks>().Value;
            //var dotNetCoreVersion = versionContainerValue.Resolve<IDotNetCoreVersion>().Value;
            var dotNetCoreInfo = versionContainerValue.Resolve<IDotNetCoreInfo>().Value;
            var sourceOsCollection = versionContainerValue.Resolve<ISourceOsCollection>().Value;


            versionContainerValue.Dispose();

            Console.WriteLine("## WINDOWS ##");
            Console.WriteLine("{0,50}|{1,50}", "Key", "Value");
            foreach (var item in currentVersionText)
            {
                Console.WriteLine("{0,50}|{1,50}", item.Key, item.Value);
            }

            Console.WriteLine("---");

            Console.WriteLine("## HISTORY ##");
            Console.WriteLine();
            Console.WriteLine("{0,20}|{1,20}|{2,20}|{3,20}", "Build", "Product Name", "Release Id", "Install Date");
            foreach (var sourceOs in sourceOsCollection)
            {
                Console.WriteLine("{0,20}|{1,20}|{2,20}|{3,20:yyyy-MM-dd HH:mm:ss}", sourceOs.Build, sourceOs.ProductName, sourceOs.ReleaseId, sourceOs.InstallDate);
            }

            Console.WriteLine("---");

            Console.WriteLine("## OTHER ##");
            Console.WriteLine(otherText);
            Console.WriteLine("---");

            Console.WriteLine("## .NET FRAMEWORK ##");
            Console.WriteLine(dotNetVersionText);
            Console.WriteLine("---");

            //Console.WriteLine("## .NET CORE VERSION ##");
            //Console.WriteLine(dotNetCoreVersion);
            //Console.WriteLine("---");

            //Console.WriteLine("## .NET CORE SDKS ##");
            //Console.WriteLine(dotNetCoreSdks);
            //Console.WriteLine("---");

            //Console.WriteLine("## .NET CORE RUNTIMES ##");
            //Console.WriteLine(dotNetCoreRuntimes);

            Console.WriteLine(dotNetCoreInfo);

            Console.ReadLine();
        }
    }
}