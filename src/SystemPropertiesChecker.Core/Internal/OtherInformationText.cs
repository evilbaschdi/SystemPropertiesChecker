using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class OtherInformationText : IOtherInformationText
{
    private readonly ISystemPropertiesProvider _systemPropertiesProvider;

    /// <summary>
    ///     Constructor
    /// </summary>
    public OtherInformationText([NotNull] ISystemPropertiesProvider systemPropertiesProvider)
    {
        _systemPropertiesProvider = systemPropertiesProvider ?? throw new ArgumentNullException(nameof(systemPropertiesProvider));
    }

    /// <summary>
    ///     Other information text.
    /// </summary>
    public List<KeyValuePair<string, string>> Value
    {
        get
        {
            var list = new List<KeyValuePair<string, string>>();
            var psVersion = "0";
            if (PowerShellExists(3))
            {
                psVersion = GetPowerShellVersion(3);
            }
            else if (PowerShellExists(1))
            {
                psVersion = GetPowerShellVersion(1);
            }

            list.Add(new("Internet Explorer", GetIeVersion()));

            list.AddRange(GetBrowsers().Select(browser => new KeyValuePair<string, string>(browser.Name, browser.Version)));

            list.Add(new("PowerShell", psVersion));
            list.Add(new("PowerShell Core", GetPowerShellCoreVersion()));
            list.Add(new("Git", GetGitVersion()));
            list.Add(new("Visual Studio", VsVersion()));
            list.Add(new("Code", VsCodeVersion()));

            return list;
        }
    }

    private string GetIeVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        return _systemPropertiesProvider.Data.RegistryInternetExplorerVersion ?? "0";
    }

    private static string GetGitVersion()
    {
        var programFiles = Directory.Exists(@"C:\Program Files\Git\bin")
            ? @"C:\Program Files\Git\bin"
            : Directory.Exists(@"C:\Program Files (x86)\Git\bin")
                ? @"C:\Program Files (x86)\Git\bin"
                : string.Empty;

        if (string.IsNullOrWhiteSpace(programFiles))
        {
            return "(not found)";
        }

        var versionInfo = FileVersionInfo.GetVersionInfo(Path.Combine(programFiles, "git.exe"));
        return $"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}.{versionInfo.FilePrivatePart}";
    }

    private string GetPowerShellVersion(int version)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        var dict = _systemPropertiesProvider.Data.RegistryPowerShellStatus;
        if (dict != null)
        {
            var key = version == 3 ? "PS3CompatibleVersion" : "PS1CompatibleVersion";
            if (dict.TryGetValue(key, out var result) && !string.IsNullOrWhiteSpace(result))
            {
                return result.Split(',').LastOrDefault()?.Trim() ?? "0";
            }
        }

        return "0";
    }

    private bool PowerShellExists(int version)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        var dict = _systemPropertiesProvider.Data.RegistryPowerShellStatus;
        if (dict != null)
        {
            var key = version == 3 ? "PS3Install" : "PS1Install";
            if (dict.TryGetValue(key, out var result))
            {
                return !string.IsNullOrWhiteSpace(result) && result.Equals("1");
            }
        }

        return false;
    }

    private static string GetPowerShellCoreVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        var list = new List<string>();
        var basePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "PowerShell");

        if (Directory.Exists(basePath))
        {
            foreach (var dir in Directory.GetDirectories(basePath))
            {
                var exePath = Path.Combine(dir, "pwsh.exe");
                if (File.Exists(exePath))
                {
                    var versionInfo = FileVersionInfo.GetVersionInfo(exePath);
                    list.Add(versionInfo.ProductVersion?.Split(' ').FirstOrDefault());
                }
            }
        }

        return list.Count != 0 ? string.Join(", ", list) : "(none)";
    }

    private IEnumerable<Browser> GetBrowsers()
    {
        var list = new List<Browser>();
        if (_systemPropertiesProvider.Data.Browsers != null)
        {
            list.AddRange(_systemPropertiesProvider.Data.Browsers);
        }

        var edgeBrowser = GetEdgeVersion();
        if (edgeBrowser != null)
        {
            list.Add(edgeBrowser);
        }

        return list;
    }

    private Browser GetEdgeVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return null;
        }

        var result = _systemPropertiesProvider.Data.RegistryEdgePackageFullName;
        if (string.IsNullOrWhiteSpace(result))
        {
            return null;
        }

        var match = Regex.Match(result, "(((([0-9.])\\d)+){1})");
        if (match.Success)
        {
            return new()
                   {
                       Name = "MicrosoftEdge",
                       Version = match.Value
                   };
        }

        return null;
    }

    private static string VsVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        var list = new List<string>();

        var vswherePath =
            Environment.ExpandEnvironmentVariables(
                @"%ProgramFiles(x86)%\Microsoft Visual Studio\Installer\vswhere.exe");

        if (!File.Exists(vswherePath))
        {
            return ("(none)");
        }

        var psi = new ProcessStartInfo
                  {
                      FileName = vswherePath,
                      Arguments = "-all -products * -prerelease -format json",
                      RedirectStandardOutput = true,
                      UseShellExecute = false,
                      CreateNoWindow = true
                  };

        using var process = Process.Start(psi);
        // ReSharper disable once InvertIf
        if (process != null)
        {
            var output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            var instances = JsonSerializer.Deserialize<JsonElement>(output);

            list.AddRange(from instance in instances.EnumerateArray()
                          let catalog = instance.GetProperty("catalog")
                          where catalog.GetProperty("productName").GetString() == "Visual Studio"
                          let displayName = instance.GetProperty("displayName").GetString()
                          let installationVersion = instance.GetProperty("installationVersion").GetString()
                          let productDisplayVersion = catalog.GetProperty("productDisplayVersion").GetString()
                          let productMilestone = catalog.GetProperty("productMilestone").GetString()
                          select $"{displayName} ({productMilestone}) v{productDisplayVersion} ({installationVersion})");
        }

        return list.Count != 0 ? string.Join(Environment.NewLine, list) : "(none)";
    }

    private static string VsCodeVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        List<string> possiblePaths =
        [
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Microsoft VS Code\Code.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft VS Code\Code.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft VS Code\Code.exe")
        ];

        List<string> possibleVsCodeInsidersPaths =
        [
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), @"Programs\Microsoft VS Code Insiders\Code - Insiders.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Microsoft VS Code Insiders\Code - Insiders.exe"),
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), @"Microsoft VS Code Insiders\Code - Insiders.exe")
        ];

        var list = possiblePaths
                   .Where(File.Exists)
                   .Select(FileVersionInfo.GetVersionInfo)
                   .Select(versionInfo => versionInfo.ProductVersion)
                   .ToList();
        list.AddRange(possibleVsCodeInsidersPaths
                      .Where(File.Exists)
                      .Select(FileVersionInfo.GetVersionInfo)
                      .Select(versionInfo => versionInfo.ProductVersion + "(Insider)"));

        return list.Count != 0 ? string.Join(", ", list) : "(none)";
    }
}