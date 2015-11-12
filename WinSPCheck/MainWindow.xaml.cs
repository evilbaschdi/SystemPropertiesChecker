using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using WinSPCheck.Core;
using WinSPCheck.Internal;

namespace WinSPCheck
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private List<string> _dotNetVersionList;
        private readonly ApplicationStyle _style;

        public MainWindow()
        {
            _style = new ApplicationStyle(this);
            InitializeComponent();
            _style.Load();
            RunVersionChecks();
        }

        private void RunVersionChecks()
        {
            var dotNetVersion = new DotNetVersion();
            _dotNetVersionList = dotNetVersion.DotNetVersionList;
            WindowsVersion();
        }

        private ValueHelper GetValues()
        {
            var bits = Environment.Is64BitOperatingSystem ? "64-bit" : "32-bit";

            var currentVersion = GetRegistryValue("CurrentVersion");
            var currentMajorVersionNumber = GetRegistryValue("CurrentMajorVersionNumber");
            var currentMinorVersionNumber = GetRegistryValue("CurrentMinorVersionNumber");

            var csdVersion = !string.IsNullOrEmpty(GetRegistryValue("CSDVersion"))
                ? $" with {GetRegistryValue("CSDVersion")}"
                : string.Empty;

            var releaseId = !string.IsNullOrEmpty(GetRegistryValue("ReleaseId"))
                ? $" Version: {GetRegistryValue("ReleaseId")}"
                : string.Empty;

            var version = !string.IsNullOrWhiteSpace(currentMajorVersionNumber) &&
                          !string.IsNullOrWhiteSpace(currentMinorVersionNumber)
                ? $"{currentMajorVersionNumber}.{currentMinorVersionNumber}"
                : currentVersion;

            return new ValueHelper
            {
                Bits = bits,
                BuildLab = GetRegistryValue("BuildLab"),
                BuildLabEx = GetRegistryValue("BuildLabEx"),
                BuildLabExArray = GetRegistryValue("BuildLabEx").Split('.'),
                CurrentBuild = GetRegistryValue("CurrentBuild"),
                ProductName = GetRegistryValue("ProductName"),
                CurrentVersion = version,
                CsdVersion = csdVersion,
                ReleaseId = releaseId
            };
        }

        private void WindowsVersion()
        {
            var values = GetValues();

            var sb = new StringBuilder();
            sb.Append($"Computername: {Environment.MachineName} {Environment.NewLine}");
            sb.Append($"{values.ProductName}{values.CsdVersion}{values.ReleaseId}{Environment.NewLine}");
            sb.Append($"System type: {values.Bits}{Environment.NewLine}");
            sb.Append($"{Environment.NewLine}");
            sb.Append($"Version number: {values.CurrentVersion} Build: {values.CurrentBuild} (OS Build: {values.BuildLabExArray[0]}.{values.BuildLabExArray[1]})");
            sb.Append(Environment.NewLine);
            sb.Append($"Build Lab: {values.BuildLab}{Environment.NewLine}");
            sb.Append(Environment.NewLine);
            sb.Append(_dotNetVersionList.Aggregate(string.Empty, (c, v) => c + (v + Environment.NewLine)));

            CurrentVersion.Text = sb.ToString();
        }

        private string GetRegistryValue(string name)
        {
            if(name == null)
            {
                throw new ArgumentNullException(nameof(name));
            }
            var regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            return regPath?.GetValue(name) != null
                ? regPath.GetValue(name).ToString()
                : string.Empty;
        }

        #region Flyout

        private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void ToggleFlyout(int index, bool stayOpen = false)
        {
            var activeFlyout = (Flyout) Flyouts.Items[index];
            if(activeFlyout == null)
            {
                return;
            }

            foreach(
                var nonactiveFlyout in
                    Flyouts.Items.Cast<Flyout>()
                        .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if(activeFlyout.IsOpen && stayOpen)
            {
                activeFlyout.IsOpen = true;
            }
            else
            {
                activeFlyout.IsOpen = !activeFlyout.IsOpen;
            }
        }

        #endregion Flyout

        #region Style

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _style.SetAccent(sender, e);
        }

        #endregion Style
    }
}