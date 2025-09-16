using System.Diagnostics;
using System.Drawing;
using Microsoft.Win32;

namespace SystemPropertiesChecker.Terminal.Internal;

/// <summary>
///     Helpers to get the system accent color
/// </summary>
public class AccentColorHelper : IAccentColorHelper
{
    private Color? _cachedAccentColor;
    private Spectre.Console.Color? _cachedSpectreColor;

    /// <inheritdoc />
    public Color AccentColor
    {
        get
        {
            if (_cachedAccentColor.HasValue)
            {
                return _cachedAccentColor.Value;
            }

            Color color;
            if (OperatingSystem.IsWindows())
            {
                color = GetWindowsAccentColor();
            }
            else if (OperatingSystem.IsLinux())
            {
                color = GetLinuxAccentColor();
            }
            else
            {
                color = Color.Empty; // Not implemented for macOS or others
            }

            _cachedAccentColor = color;
            return color;
        }
    }

    /// <inheritdoc />
    public Spectre.Console.Color SpectreConsoleColor
    {
        get
        {
            if (_cachedSpectreColor.HasValue)
            {
                return _cachedSpectreColor.Value;
            }

            var accent = AccentColor;
            var spectreColor = new Spectre.Console.Color(accent.R, accent.G, accent.B);
            _cachedSpectreColor = spectreColor;
            return spectreColor;
        }
    }

    // ---------------- WINDOWS ----------------
    private Color GetWindowsAccentColor()
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
    private Color GetLinuxAccentColor()
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
        var kdeConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            ".config/kdeglobals");
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
    private string TryRunCommand(string fileName, string arguments)
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

    private bool TryParseHexColor(string hex, out Color color)
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

    private bool TryParseKdeColor(string value, out Color color)
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