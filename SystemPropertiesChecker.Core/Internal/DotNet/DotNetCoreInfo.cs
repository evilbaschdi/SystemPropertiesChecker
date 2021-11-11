using System.Diagnostics;

namespace SystemPropertiesChecker.Core.Internal.DotNet
{
    /// <inheritdoc />
    public class DotNetCoreInfo : IDotNetCoreInfo
    {
        /// <inheritdoc />
        public List<KeyValuePair<string, string>> Value
        {
            get
            {
                var dictionary = new List<KeyValuePair<string, string>>();

                try
                {
                    var process = new Process();
                    process.SetHiddenProcessFor("dotnet", "--info");
                    process.Start();


                    dictionary.AddRange(process.ReadStandardOutput().Select(item => (item.Contains("[") ? item.Split('[').First() : item).Trim())
                                               .Select(line => line.EndsWith(":")
                                                           ? new(line, string.Empty)
                                                           : new KeyValuePair<string, string>(string.Empty, line)));

                    process.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    dictionary.Add(new("(none)", string.Empty));
                }


                return dictionary;
            }
        }
    }
}