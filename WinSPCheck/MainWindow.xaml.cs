using Microsoft.Win32;
using System;
using System.Windows;
using WinSPCheck.Internal;

namespace WinSPCheck
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable RedundantExtendsListEntry
    public partial class MainWindow : Window
        // ReSharper restore RedundantExtendsListEntry
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => GlassEffectHelper.EnableGlassEffect(this);
            RunVersionChecks();
        }

        private void RunVersionChecks()
        {
            WindowsVersion();
            NetFrameworks();
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

            CurrentVersion.Text = string.Format("{0}{1} | {2}.{3} | {4}", productName, csdVersion, currentVersion,
                currentBuild, buildLab);
        }

        private string GetRegistryValue(string name)
        {
            var regPath = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion");

            return regPath != null && regPath.GetValue(name) != null ? regPath.GetValue(name).ToString() : string.Empty;
        }

        private void NetFrameworks()
        {
            var dotNetVersion = new DotNetVersion();

            foreach (var dotNetFramework in dotNetVersion.DotNetVersionList)
            {
                NetFramework.Text += dotNetFramework;
                NetFramework.Text += Environment.NewLine;
            }
        }
    }
}