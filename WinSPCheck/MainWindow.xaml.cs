using System;
using System.ComponentModel;
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
using WinSPCheck.Internal;

namespace WinSPCheck
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly BackgroundWorker _bw;
        private readonly IMetroStyle _style;
        private int _executionCount;
        private int _overrideProtection;
        private string _currentVersionText;
        private string _windowsVersionText;
        private string _dotNetVersionText;
        private string _otherText;
        private string _passwordExpirationMessage;

        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            ISettings coreSettings = new CoreSettings();
            InitializeComponent();
            _style = new MetroStyle(this, Accent, ThemeSwitch, coreSettings);
            _style.Load(true);
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);
            _bw = new BackgroundWorker();
            Load();
            //RunVersionChecks();
        }


        private void Load()
        {
            _executionCount++;
            _overrideProtection = 1;
            if (_executionCount == 1)
            {
                _bw.DoWork += (o, args) => RunVersionChecks();
                _bw.WorkerReportsProgress = true;
                _bw.RunWorkerCompleted += BackgroundWorkerWorkerCompleted;
            }
            _bw.RunWorkerAsync();
            // ShowMessage("title", "message");
        }

        private void BackgroundWorkerWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CurrentVersion.Text = _currentVersionText;
            WindowsVersion.Text = _windowsVersionText;
            DotNetVersion.Text = _dotNetVersionText;
            Other.Text = _otherText;

            if (!string.IsNullOrWhiteSpace(_passwordExpirationMessage))
            {
                ShowMessage("Password Expiration", _passwordExpirationMessage);
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemInfo.ProgressValue = 1;
            }
            DomainTab.Visibility = Visibility.Hidden;
        }

        private void RunVersionChecks()
        {
            var dotNetVersion = new DotNetVersion();
            var registryValue = new HklmSoftwareMicrosoftWindowsNtCurrentVersion();
            var windowsVersionInformationHelper = new WindowsVersionInformationHelper();
            var windowsVersionInformation = new GetWindowsVersionInformation(registryValue, windowsVersionInformationHelper);
            var currentVersionText = new GetCurrentVersionText(windowsVersionInformation);
            var windowsVersionText = new GetWindowsVersionText(windowsVersionInformation);
            var otherText = new GetOtherInformationText();
            _currentVersionText = currentVersionText.Value;
            _windowsVersionText = windowsVersionText.Value;
            _otherText = otherText.Value;
            _dotNetVersionText = dotNetVersion.List.Aggregate(string.Empty, (c, v) => c + v + Environment.NewLine);
            _passwordExpirationMessage = windowsVersionInformation.PasswordExpirationMessage;
            //var temp = string.Empty;
            //DomainInformation.Text = temp;
            //DomainTab.Visibility = (!string.IsNullOrWhiteSpace(temp)).ToVisibility();
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
    }
}