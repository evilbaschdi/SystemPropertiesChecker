using System;
using MahApps.Metro.Controls;
using Microsoft.Win32;

namespace WinSPCheck
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
        // ReSharper restore RedundantExtendsListEntry
    {
        public MainWindow()
        {
            InitializeComponent();
            RunVersionChecks();
        }

        private void RunVersionChecks()
        {
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

            CurrentVersion.Text = "Machine: " + Environment.MachineName + Environment.NewLine +
                                  "Product: " + productName + csdVersion + Environment.NewLine +
                                  "Build: " + currentVersion + " " + currentBuild +
                                  Environment.NewLine + "Build lab: " + buildLab;
        }

        private string GetRegistryValue(string name)
        {
            var regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            return regPath != null && regPath.GetValue(name) != null ? regPath.GetValue(name).ToString() : string.Empty;
        }
    }
}