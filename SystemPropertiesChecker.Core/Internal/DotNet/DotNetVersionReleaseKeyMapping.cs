using System.IO;
using Microsoft.Extensions.Configuration;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <summary>
    /// </summary>
    public static class DotNetVersionReleaseKeyMapping
    {
        static DotNetVersionReleaseKeyMapping()
        {
            AppSetting = new ConfigurationBuilder()
                         .SetBasePath(Directory.GetCurrentDirectory())
                         .AddJsonFile("DotNetVersionReleaseKeyMapping.json")
                         .Build();
        }

        /// <summary>
        /// </summary>
        public static IConfiguration AppSetting { get; }
    }
}