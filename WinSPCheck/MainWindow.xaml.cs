using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Wpf;
using MahApps.Metro.Controls;
using WinSPCheck.Core;
using WinSPCheck.Extension;
using WinSPCheck.Internal;

namespace WinSPCheck
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IMetroStyle _style;

        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly ISettings _coreSettings;

        private int _overrideProtection;

        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            _coreSettings = new CoreSettings();
            InitializeComponent();
            _style = new MetroStyle(this, Accent, Dark, Light, _coreSettings);
            _style.Load();
            Load();
            RunVersionChecks();
        }

        private void Load()
        {
            _overrideProtection = 1;
        }

        private void RunVersionChecks()
        {
            var dotNetVersion = new DotNetVersion();
            var registryValue = new HklmSoftwareMicrosoftWindowsNtCurrentVersion();
            var windowsVersionInformationHelper = new WindowsVersionInformationHelper();
            var windowsVersionInformation = new GetWindowsVersionInformation(registryValue, windowsVersionInformationHelper);
            var currentVersionText = new GetCurrentVersionText(windowsVersionInformation);
            var windowsVersionText = new GetWindowsVersionText(windowsVersionInformation);
            CurrentVersion.Text = currentVersionText.Value;
            WindowsVersion.Text = windowsVersionText.Value;
            var temp = string.Empty;
            DomainInformation.Text = temp;
            DomainTab.Visibility = ControlHelpers.BoolToVisibilityConverter(!string.IsNullOrWhiteSpace(temp));
            DotNetVersion.Text = dotNetVersion.List.Aggregate(string.Empty, (c, v) => c + v + Environment.NewLine);
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

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(_overrideProtection == 0)
            {
                return;
            }
            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle
    }
}