using System;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Wpf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
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
            _style = new MetroStyle(this, Accent, ThemeSwitch, _coreSettings);
            _style.Load(true);
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);

            Load();
            RunVersionChecks();
        }


        private void Load()
        {
            _overrideProtection = 1;
            // ShowMessage("title", "message");
        }

        private void RunVersionChecks()
        {
            var dotNetVersion = new DotNetVersion();
            var registryValue = new HklmSoftwareMicrosoftWindowsNtCurrentVersion();
            var windowsVersionInformationHelper = new WindowsVersionInformationHelper();
            var windowsVersionInformation = new GetWindowsVersionInformation(registryValue, windowsVersionInformationHelper, this);
            var currentVersionText = new GetCurrentVersionText(windowsVersionInformation);
            var windowsVersionText = new GetWindowsVersionText(windowsVersionInformation);
            var otherText = new GetOtherInformationText();
            CurrentVersion.Text = currentVersionText.Value;
            WindowsVersion.Text = windowsVersionText.Value;
            Other.Text = otherText.Value;
            var temp = string.Empty;
            DomainInformation.Text = temp;
            DomainTab.Visibility = (!string.IsNullOrWhiteSpace(temp)).ToVisibility();
            DotNetVersion.Text = dotNetVersion.List.Aggregate(string.Empty, (c, v) => c + v + Environment.NewLine);
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public async void ShowMessage(string title, string message)
        {
            var options = new MetroDialogSettings
                          {
                              ColorScheme = MetroDialogColorScheme.Theme
                          };

            MetroDialogOptions = options;
            var result = await this.ShowMessageAsync(title, message);
            if (result == MessageDialogResult.Affirmative)
            {
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
            }
        }

        #region Flyout

        private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void ToggleFlyout(int index, bool stayOpen = false)
        {
            var activeFlyout = (Flyout) Flyouts.Items[index];
            if (activeFlyout == null)
            {
                return;
            }

            foreach (
                var nonactiveFlyout in
                Flyouts.Items.Cast<Flyout>()
                       .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if (activeFlyout.IsOpen && stayOpen)
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
            if (_overrideProtection == 0)
            {
                return;
            }
            _style.SaveStyle();
        }

        private void Theme(object sender, EventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }
            var routedEventArgs = e as RoutedEventArgs;
            if (routedEventArgs != null)
            {
                _style.SetTheme(sender, routedEventArgs);
            }
            else
            {
                _style.SetTheme(sender);
            }
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_overrideProtection == 0)
            {
                return;
            }
            _style.SetAccent(sender, e);
        }

        #endregion MetroStyle

        private void TestButton_OnClick(object sender, RoutedEventArgs e)
        {
            ShowMessage("title", "test");
        }
    }
}