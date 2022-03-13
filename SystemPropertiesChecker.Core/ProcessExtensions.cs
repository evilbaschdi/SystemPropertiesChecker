using System.Diagnostics;
using JetBrains.Annotations;

namespace SystemPropertiesChecker.Core;

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
    public static IEnumerable<string> ReadStandardOutput([NotNull] this Process process)
    {
        if (process == null)
        {
            throw new ArgumentNullException(nameof(process));
        }

        using var reader = process.StandardOutput;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    /// <summary>
    ///     Reads all lines of a process standard error and returns them as an enumerable of string
    /// </summary>
    /// <param name="process"></param>
    /// <returns></returns>
    public static IEnumerable<string> ReadStandardError([NotNull] this Process process)
    {
        if (process == null)
        {
            throw new ArgumentNullException(nameof(process));
        }

        using var reader = process.StandardError;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            yield return line;
        }
    }

    /// <summary>
    ///     Adds StartInfo for given process by filename and arguments
    /// </summary>
    /// <param name="process"></param>
    /// <param name="fileName"></param>
    /// <param name="arguments"></param>
    public static void SetHiddenProcessFor([NotNull] this Process process, [NotNull] string fileName, [NotNull] string arguments)
    {
        if (process == null)
        {
            throw new ArgumentNullException(nameof(process));
        }

        if (fileName == null)
        {
            throw new ArgumentNullException(nameof(fileName));
        }

        if (arguments == null)
        {
            throw new ArgumentNullException(nameof(arguments));
        }

        process.StartInfo = new(fileName, arguments)
                            {
                                UseShellExecute = false,
                                CreateNoWindow = true,
                                RedirectStandardOutput = true,
                                RedirectStandardError = true
                            };
    }
}