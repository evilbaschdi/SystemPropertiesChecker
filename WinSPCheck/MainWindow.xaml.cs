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
using Microsoft.Practices.Unity;
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
        //private readonly UnityContainer _coreContainer;

        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            ISettings coreSettings = new CoreSettings();
            InitializeComponent();

            //_coreContainer = new UnityContainer();
            //_coreContainer.RegisterType<ISettings, CoreSettings>();
            //_coreContainer.RegisterInstance(this);
            //_coreContainer.RegisterInstance(Accent);
            //_coreContainer.RegisterInstance(ThemeSwitch);
            //_coreContainer.RegisterType<IMetroStyle, MetroStyle>();

            //_style = _coreContainer.Resolve<IMetroStyle>();

            _style = new MetroStyle(this, Accent, ThemeSwitch, coreSettings);
            _style.Load(true);
            var linkerTime = Assembly.GetExecutingAssembly().GetLinkerTime();
            LinkerTime.Content = linkerTime.ToString(CultureInfo.InvariantCulture);
            _bw = new BackgroundWorker();
            Load();
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    _coreContainer.Teardown(_style);
        //    _coreContainer.Dispose();
        //    base.OnClosing(e);
        //}

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
            // ShowMessageAsync("title", "message");
        }

        private void BackgroundWorkerWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CurrentVersion.Text = _currentVersionText;
            WindowsVersion.Text = _windowsVersionText;
            DotNetVersion.Text = _dotNetVersionText;
            Other.Text = _otherText;

            if (!string.IsNullOrWhiteSpace(_passwordExpirationMessage))
            {
                ShowMessageAsync("Password Expiration", _passwordExpirationMessage);
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemInfo.ProgressValue = 1;
            }
            DomainTab.Visibility = Visibility.Hidden;
        }

        private void RunVersionChecks()
        {
            var versionContainer = new UnityContainer();
            versionContainer.RegisterType<IDotNetVersion, DotNetVersion>();
            versionContainer.RegisterType<IRegistryValue, HklmSoftwareMicrosoftWindowsNtCurrentVersion>();
            versionContainer.RegisterType<IWindowsVersionInformationHelper, WindowsVersionInformationHelper>();
            versionContainer.RegisterType<IWindowsVersionInformation, GetWindowsVersionInformation>();
            versionContainer.RegisterType<ICurrentVersionText, GetCurrentVersionText>();
            versionContainer.RegisterType<IWindowsVersionText, GetWindowsVersionText>();
            versionContainer.RegisterType<IOtherInformationText, GetOtherInformationText>();

            _currentVersionText = versionContainer.Resolve<ICurrentVersionText>().Value;
            _windowsVersionText = versionContainer.Resolve<IWindowsVersionText>().Value;
            _otherText = versionContainer.Resolve<IOtherInformationText>().Value;
            _dotNetVersionText = versionContainer.Resolve<IDotNetVersion>().List.Aggregate(string.Empty, (c, v) => c + v + Environment.NewLine);
            _passwordExpirationMessage = versionContainer.Resolve<IWindowsVersionInformation>().PasswordExpirationMessage;
            versionContainer.Dispose();
            //var temp = string.Empty;
            //DomainInformation.Text = temp;
            //DomainTab.Visibility = (!string.IsNullOrWhiteSpace(temp)).ToVisibility();
        }

        /// <summary>
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public async void ShowMessageAsync(string title, string message)
        {
            var options = new MetroDialogSettings
                          {
                              ColorScheme = MetroDialogColorScheme.Theme
                          };

            MetroDialogOptions = options;
            var result = await DialogManager.ShowMessageAsync(this, title, message);
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