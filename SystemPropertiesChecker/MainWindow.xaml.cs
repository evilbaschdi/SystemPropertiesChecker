using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shell;
using SystemPropertiesChecker.Core;
using SystemPropertiesChecker.Internal;
using SystemPropertiesChecker.Model;
using EvilBaschdi.Core.Application;
using EvilBaschdi.Core.Wpf;
using MahApps.Metro.Controls;
using Microsoft.Practices.Unity;

namespace SystemPropertiesChecker
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IMetroStyle _style;
        private readonly IDialogService _dialogService;
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
            InitializeComponent();
            ISettings coreSettings = new CoreSettings(Properties.Settings.Default);
            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            ILinkerTime linkerTime = new LinkerTime();
            _style = new MetroStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);
            _style.Load(true);

            _dialogService = new DialogService(this);

            LinkerTime.Content = linkerTime.Value;
            LoadAsync();
        }

        private async void LoadAsync()
        {
            var task = Task.Factory.StartNew(RunVersionChecks);

            _executionCount++;
            _overrideProtection = 1;
            if (_executionCount == 1)
            {
                await task;
            }

            CurrentVersion.Text = _currentVersionText;
            WindowsVersion.Text = _windowsVersionText;
            DotNetVersion.Text = _dotNetVersionText;
            Other.Text = _otherText;

            if (!string.IsNullOrWhiteSpace(_passwordExpirationMessage))
            {
                await _dialogService.ShowMessage("Password Expiration", _passwordExpirationMessage);
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemInfo.ProgressValue = 1;
            }
            DomainTab.Visibility = Visibility.Hidden;
        }

        private void ReloadClick(object sender, RoutedEventArgs e)
        {
            LoadAsync();
        }

        private void RunVersionChecks()
        {
            var versionContainer = new UnityContainer();
            versionContainer.RegisterType<IDotNetVersion, DotNetVersion>();
            versionContainer.RegisterType<IRegistryValue, HklmSoftwareMicrosoftWindowsNtCurrentVersion>();
            versionContainer.RegisterType<IWindowsVersionInformationModel, WindowsVersionInformationModel>();
            versionContainer.RegisterType<IWindowsVersionInformation, WindowsVersionInformation>();
            versionContainer.RegisterType<ICurrentVersionText, CurrentVersionText>();
            versionContainer.RegisterType<IWindowsVersionText, WindowsVersionText>();
            versionContainer.RegisterType<IOtherInformationText, OtherInformationText>();
            versionContainer.RegisterType<IPasswordExpirationDate, PasswordExpirationDate>();
            versionContainer.RegisterType<IPasswordExpirationMessage, PasswordExpirationMessage>();

            _currentVersionText = versionContainer.Resolve<ICurrentVersionText>().Value;
            _windowsVersionText = versionContainer.Resolve<IWindowsVersionText>().Value;
            _otherText = versionContainer.Resolve<IOtherInformationText>().Value;
            _dotNetVersionText = versionContainer.Resolve<IDotNetVersion>().Value.Aggregate(string.Empty, (c, v) => $"{c}{v}{Environment.NewLine}");
            _passwordExpirationMessage = versionContainer.Resolve<IPasswordExpirationMessage>().Value;
            versionContainer.Dispose();
            //var temp = string.Empty;
            //DomainInformation.Text = temp;
            //DomainTab.Visibility = (!string.IsNullOrWhiteSpace(temp)).ToVisibility();
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