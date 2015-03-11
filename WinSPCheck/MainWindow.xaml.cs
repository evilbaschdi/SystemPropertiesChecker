using System;
using System.Collections.Generic;
using System.Linq;
using MahApps.Metro.Controls;
using Microsoft.Win32;
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

        public MainWindow()
        {
            InitializeComponent();
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
            var csdVersion = !string.IsNullOrEmpty(GetRegistryValue("CSDVersion"))
                ? string.Format(" | {0}", GetRegistryValue("CSDVersion"))
                : string.Empty;

            CurrentVersion.Text =
                string.Format("Machine: {0}{1}Product: {2}{3}{1}Version: {4}.{5}{1}Build lab: {6}{1}{1}{7}",
                    Environment.MachineName, Environment.NewLine, productName, csdVersion, currentVersion, currentBuild,
                    buildLab, _dotNetVersionList.Aggregate(string.Empty,
                        (current, version) => current + (version + Environment.NewLine)));
        }

        private string GetRegistryValue(string name)
        {
            var regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            return regPath != null && regPath.GetValue(name) != null ? regPath.GetValue(name).ToString() : string.Empty;
        }
    }
}