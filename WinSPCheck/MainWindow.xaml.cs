using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using Microsoft.Win32;
using WinSPCheck.Core;
using WinSPCheck.Internal;

namespace WinSPCheck
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
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

        private void WindowsVersion()
        {
            var buildLab = GetRegistryValue("BuildLab");
            var productName = GetRegistryValue("ProductName");
            var currentBuild = GetRegistryValue("CurrentBuild");
            var currentVersion = GetRegistryValue("CurrentVersion");
            var currentMajorVersionNumber = GetRegistryValue("CurrentMajorVersionNumber");
            var currentMinorVersionNumber = GetRegistryValue("CurrentMinorVersionNumber");
            var releaseId = GetRegistryValue("ReleaseId");
            var csdVersion = !string.IsNullOrEmpty(GetRegistryValue("CSDVersion"))
                ? $" | {GetRegistryValue("CSDVersion")}"
                : string.Empty;

            var version = !string.IsNullOrWhiteSpace(currentMajorVersionNumber) &&
                          !string.IsNullOrWhiteSpace(currentMinorVersionNumber)
                ? $"{currentMajorVersionNumber}.{currentMinorVersionNumber}"
                : currentVersion;
            var dotNet = _dotNetVersionList.Aggregate(string.Empty,
                (c, v) => c + (v + Environment.NewLine));

            //CurrentVersion.Text =
            //    string.Format("Machine: {0}{1}Product: {2}{3}{1}Version: {4}.{5}{1}Build lab: {6}{1}{1}{7}",
            //        Environment.MachineName, Environment.NewLine, productName, csdVersion, version, currentBuild,
            //        buildLab, _dotNetVersionList.Aggregate(string.Empty,
            //            (c, v) => c + (v + Environment.NewLine)));

            CurrentVersion.Text =
                $"Machine: {Environment.MachineName}{Environment.NewLine}" +
                $"Product: {productName}{csdVersion}{Environment.NewLine}" +
                $"Version: {version} | " +
                $"Build: {currentBuild} | " +
                $"Release: {releaseId}{Environment.NewLine}" +
                $"Build Lab: {buildLab}{Environment.NewLine}{Environment.NewLine}" +
                $"{dotNet}";
        }

        private string GetRegistryValue(string name)
        {
            var regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            return regPath?.GetValue(name) != null ? regPath.GetValue(name).ToString() : string.Empty;
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