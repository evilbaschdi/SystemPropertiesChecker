using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <summary>
///     Helpers to get the system accent color
/// </summary>
public static class AccentColorHelper
{
    /// <summary>
    ///     Gets the system accent color and returns it as a <see cref="System.Drawing.Color" /> struct.
    /// </summary>
    /// <returns></returns>
    public static Color GetAccentColor()
    {
        if (OperatingSystem.IsWindows())
        {
            return GetWindowsAccentColor();
        }

        if (OperatingSystem.IsLinux())
        {
            return GetLinuxAccentColor();
        }

        return Color.Empty; // Not implemented for macOS or others
    }

    /// <summary>
    ///     Gets the system accent color and returns it as a <see cref="Spectre.Console.Color" /> struct.
    /// </summary>
    /// <returns></returns>
    public static Spectre.Console.Color GetSpectreConsoleColor()
    {
        var accent = GetAccentColor();
        return new(accent.R, accent.G, accent.B);
    }

    // ---------------- WINDOWS ----------------
    private static Color GetWindowsAccentColor()
    {
        try
        {
            if (OperatingSystem.IsWindows())
            {
                // Registry method (works on Win10/11)
                using var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\DWM");
                if (key != null)
                {
                    var dword = (int)key.GetValue("AccentColor")!;

                    // Stored as ABGR
                    return Color.FromArgb(
                        (dword >> 24) & 0xFF, // Alpha
                        dword & 0xFF, // Red
                        (dword >> 8) & 0xFF, // Green
                        (dword >> 16) & 0xFF // Blue
                    );
                }
            }
        }
        catch
        {
            // ignored
        }

        return Color.Empty;
    }

    // ---------------- LINUX ----------------
    private static Color GetLinuxAccentColor()
    {
        // Try GNOME first
        var gnomeColor = TryRunCommand("gsettings", "get org.gnome.desktop.interface accent-color");
        if (!string.IsNullOrWhiteSpace(gnomeColor) && gnomeColor != "none")
        {
            var hex = gnomeColor.Trim('\'', '"', ' ', '\n');
            if (hex.StartsWith('#') && TryParseHexColor(hex, out var color))
            {
                return color;
            }
        }

        // Try KDE
        var kdeConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".config/kdeglobals");
        // ReSharper disable once InvertIf
        if (File.Exists(kdeConfigPath))
        {
            foreach (var line in File.ReadAllLines(kdeConfigPath))
            {
                if (!line.StartsWith("Accent=") && !line.StartsWith("ForegroundNormal="))
                {
                    continue;
                }

                var value = line.Split('=')[1].Trim();
                if (TryParseKdeColor(value, out var color))
                {
                    return color;
                }
            }
        }

        return Color.Empty;
    }

    // ---------------- HELPERS ----------------
    private static string TryRunCommand(string fileName, string arguments)
    {
        try
        {
            var psi = new ProcessStartInfo
                      {
                          FileName = fileName,
                          Arguments = arguments,
                          RedirectStandardOutput = true,
                          UseShellExecute = false
                      };
            using var proc = Process.Start(psi);
            var output = proc?.StandardOutput.ReadToEnd();
            proc?.WaitForExit();
            return output?.Trim();
        }
        catch
        {
            return null;
        }
    }

    private static bool TryParseHexColor(string hex, out Color color)
    {
        try
        {
            color = ColorTranslator.FromHtml(hex);
            return true;
        }
        catch
        {
            color = Color.Empty;
            return false;
        }
    }

    private static bool TryParseKdeColor(string value, out Color color)
    {
        try
        {
            var parts = value.Split(',');
            if (parts.Length >= 3)
            {
                var red = int.Parse(parts[0]);
                var green = int.Parse(parts[1]);
                var blue = int.Parse(parts[2]);
                color = Color.FromArgb(red, green, blue);
                return true;
            }
        }
        catch
        {
            // ignored
        }

        color = Color.Empty;
        return false;
    }
}