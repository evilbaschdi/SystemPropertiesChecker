using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Win32;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <summary>
/// </summary>
// ReSharper disable once ClassNeverInstantiated.Global
public class OtherInformationText : IOtherInformationText
{
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

    private static string GetIeVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        const string key = @"Software\Microsoft\Internet Explorer";
        var subKey = Registry.LocalMachine.OpenSubKey(key, false);
        if (subKey == null)
        {
            return "0";
        }

        var value = subKey.GetValue("svcVersion")?.ToString();
        return value;
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

    private static string GetPowerShellVersion(int version)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return "(supported on windows only)";
        }

        var key = $@"SOFTWARE\Microsoft\PowerShell\{version}\PowerShellEngine";
        var subKey = Registry.LocalMachine.OpenSubKey(key, false);
        if (subKey == null)
        {
            return "0";
        }

        var value = subKey.GetValue("PSCompatibleVersion")?.ToString()?.Split(',');
        return value?.Last().Trim();
    }

    private static bool PowerShellExists(int version)
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return false;
        }

        var value = Registry.GetValue($@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\PowerShell\{version}", "Install", null)?.ToString();
        return !string.IsNullOrWhiteSpace(value) && value.Equals("1");
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

    private static IEnumerable<Browser> GetBrowsers()
    {
        var browsers = new List<Browser>();

        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return browsers;
        }

        try
        {
            var browserKeys = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\WOW6432Node\Clients\StartMenuInternet") ??
                              Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Clients\StartMenuInternet");

            var browserNames = browserKeys?.GetSubKeyNames();
            if (browserNames != null)
            {
                foreach (var browserName in browserNames)
                {
                    var browserKey = browserKeys.OpenSubKey(browserName);
                    if (browserKey == null)
                    {
                        continue;
                    }

                    var browser = new Browser
                                  {
                                      Name = (string)browserKey.GetValue(null)
                                  };

                    if (browser.Name is "Internet Explorer")
                    {
                        continue;
                    }

                    var browserKeyPath = browserKey.OpenSubKey(@"shell\open\command");
                    if (browserKeyPath != null)
                    {
                        browser.Path = browserKeyPath.GetValue(null)?.ToString()?.Replace("\"", "");
                    }

                    browser.Version = browser.Path != null ? FileVersionInfo.GetVersionInfo(browser.Path).FileVersion : "unknown";
                    browsers.Add(browser);
                }
            }

            var edgeBrowser = GetEdgeVersion();
            if (edgeBrowser != null)
            {
                browsers.Add(edgeBrowser);
            }
        }
        catch
        {
            // ignored
        }

        return browsers;
    }

    private static Browser GetEdgeVersion()
    {
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            return null;
        }

        var edgeKey =
            Registry.CurrentUser.OpenSubKey(
                @"SOFTWARE\Classes\Local Settings\Software\Microsoft\Windows\CurrentVersion\AppModel\SystemAppData\Microsoft.MicrosoftEdge_8wekyb3d8bbwe\Schemas");
        if (edgeKey == null)
        {
            return null;
        }

        var version = edgeKey.GetValue("PackageFullName")?.ToString()?.Replace("\"", "");
        var result = Regex.Match(version ?? string.Empty, "(((([0-9.])\\d)+){1})");
        if (result.Success)
        {
            return new()
                   {
                       Name = "MicrosoftEdge",
                       Version = result.Value
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