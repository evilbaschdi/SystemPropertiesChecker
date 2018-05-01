using System.Collections.Generic;
using System.Diagnostics;

namespace SystemPropertiesChecker.Core
{
    /// <summary>
    ///     Process extensions
    /// </summary>
    public static class ProcessExtensions
    {
        /// <summary>
        ///     Reads all lines of a process standard output and returns them as an enumerable of string
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadStandardOutput(this Process process)
        {
            using (var reader = process.StandardOutput)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        ///     Reads all lines of a process standard error and returns them as an enumerable of string
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public static IEnumerable<string> ReadStandardError(this Process process)
        {
            using (var reader = process.StandardError)
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }

        /// <summary>
        ///     Adds StartInfo for given process by filename and arguments
        /// </summary>
        /// <param name="process"></param>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        public static void SetHiddenProcessFor(this Process process, string fileName, string arguments)
        {
            process.StartInfo = new ProcessStartInfo
                                {
                                    UseShellExecute = false,
                                    CreateNoWindow = true,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true,
                                    FileName = fileName,
                                    Arguments = arguments
                                };
        }
    }
}