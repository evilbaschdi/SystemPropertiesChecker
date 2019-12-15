using System;
using System.IO;
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
            var dotNetCoreInfo = versionContainerValue.Resolve<IDotNetCoreInfo>().Value;
            var sourceOsCollection = versionContainerValue.Resolve<ISourceOsCollection>().Value;


            versionContainerValue.Dispose();

            Console.WriteLine("## WINDOWS ##");
            Console.WriteLine("{0,-30} {1,-30}", "Key", "Value");
            Console.WriteLine("{0,-30} {1,-30}", "---", "-----");
            foreach (var item in currentVersionText)
            {
                Console.WriteLine("{0,-30} {1,-30}", item.Key, item.Value);
            }

            Console.WriteLine();

            Console.WriteLine("## HISTORY ##");
            Console.WriteLine();
            Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}", "Build", "Product Name", "Release Id", "Install Date");
            Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20}", "-----", "------------", "----------", "------------");
            foreach (var sourceOs in sourceOsCollection)
            {
                Console.WriteLine("{0,-20} {1,-20} {2,-20} {3,-20:yyyy-MM-dd HH:mm:ss}", sourceOs.Build, sourceOs.ProductName, sourceOs.ReleaseId, sourceOs.InstallDate);
            }

            Console.WriteLine();

            Console.WriteLine("## .NET FRAMEWORK ##");
            Console.WriteLine(dotNetVersionText);
            Console.WriteLine();

            Console.WriteLine("## .NET CORE ##");
            Console.WriteLine(dotNetCoreInfo.Trim());
            Console.WriteLine();

            Console.WriteLine("## OTHER ##");
            Console.WriteLine("{0,-30} {1,-30}", "Key", "Value");
            Console.WriteLine("{0,-30} {1,-30}", "---", "-----");

            using var reader = new StringReader(otherText);
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                var item = line.Split(':');
                Console.WriteLine("{0,-30} {1,-30}", item[0].Trim(), item[1].Trim());
            }


            Console.WriteLine();

            Console.ReadLine();
        }
    }
}