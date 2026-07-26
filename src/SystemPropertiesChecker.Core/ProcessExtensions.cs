using System.Diagnostics;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core;

/// <summary>
///     Process extensions
/// </summary>
public static class ProcessExtensions
{
    /// <param name="process"></param>
    extension([NotNull] Process process)
    {
        /// <summary>
        ///     Reads all lines of a process standard output and returns them as enumerable of string
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ReadStandardOutput()
        {
            ArgumentNullException.ThrowIfNull(process);

            using var reader = process.StandardOutput;
            while (reader.ReadLine() is { } line)
            {
                yield return line;
            }
        }

        /// <summary>
        ///     Reads all lines of a process standard error and returns them as enumerable of string
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> ReadStandardError()
        {
            ArgumentNullException.ThrowIfNull(process);

            using var reader = process.StandardError;
            while (reader.ReadLine() is { } line)
            {
                yield return line;
            }
        }

        /// <summary>
        ///     Adds StartInfo for given process by filename and arguments
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="arguments"></param>
        public void SetHiddenProcessFor([NotNull] string fileName, [NotNull] string arguments)
        {
            ArgumentNullException.ThrowIfNull(process);

            ArgumentNullException.ThrowIfNull(fileName);

            ArgumentNullException.ThrowIfNull(arguments);

            process.StartInfo = new(fileName, arguments)
                                {
                                    UseShellExecute = false,
                                    CreateNoWindow = true,
                                    RedirectStandardOutput = true,
                                    RedirectStandardError = true
                                };
        }
    }
}