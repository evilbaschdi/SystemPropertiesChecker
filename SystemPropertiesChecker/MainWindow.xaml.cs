using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shell;
using SystemPropertiesChecker.Core;
using SystemPropertiesChecker.Internal;
using SystemPropertiesChecker.Model;
using SystemPropertiesChecker.Properties;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Unity;

namespace SystemPropertiesChecker
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IDialogService _dialogService;
        private readonly IApplicationStyle _applicationStyle;
        private ProgressDialogController _controller;
        private string _currentVersionText;
        private string _dotNetVersionText;
        private string _otherText;
        private int _overrideProtection;
        private string _passwordExpirationMessage;
        private Task _task;
        private bool _windowShown;
        private string _windowsVersionText;

        //private read only UnityContainer _coreContainer;

        /// <inheritdoc />
        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            IAppSettingsBase applicationSettingsBase = new AppSettingsBase(Settings.Default);
            IApplicationStyleSettings coreSettings = new ApplicationStyleSettings(applicationSettingsBase);
            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            ILinkerTime linkerTime = new LinkerTime();
            _applicationStyle = new ApplicationStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);
            _applicationStyle.Load(true);

            _dialogService = new DialogService(this);

            LinkerTime.Content = linkerTime.Value;

            //LoadAsync();
        }

        /// <inheritdoc />
        /// <summary>
        ///     Executing code when window is shown.
        /// </summary>
        /// <param name="e"></param>
        protected override async void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);

            if (_windowShown)
            {
                return;
            }

            _windowShown = true;

            await ConfigureControllerAsync().ConfigureAwait(true);
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        private async Task ConfigureControllerAsync()
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;

            Cursor = Cursors.Wait;

            var options = new MetroDialogSettings
                          {
                              ColorScheme = MetroDialogColorScheme.Accented
                          };

            MetroDialogOptions = options;
            _controller = await this.ShowProgressAsync("Loading...", "Checking Properties", true, options).ConfigureAwait(true);
            _controller.SetIndeterminate();
            _controller.Canceled += ControllerCanceled;

            _task = Task.Factory.StartNew(RunVersionChecks);
            await _task.ConfigureAwait(true);
            _task.GetAwaiter().OnCompleted(TaskCompleted);
        }

        private void TaskCompleted()
        {
            CurrentVersion.Text = _currentVersionText;
            WindowsVersion.Text = _windowsVersionText;
            DotNetVersion.Text = _dotNetVersionText;
            Other.Text = _otherText;

            _overrideProtection = 1;

            if (!string.IsNullOrWhiteSpace(_passwordExpirationMessage))
            {
                _dialogService.ShowMessage("Password Expiration", _passwordExpirationMessage);
                TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                TaskbarItemInfo.ProgressValue = 1;
            }

            DomainTab.Visibility = Visibility.Hidden;
            _controller.CloseAsync();
            _controller.Closed += ControllerClosed;
        }

        private void ControllerClosed(object sender, EventArgs e)
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
            TaskbarItemInfo.ProgressValue = 1;
            Cursor = Cursors.Arrow;
        }

        private void ControllerCanceled(object sender, EventArgs e)
        {
            _controller.CloseAsync();
            _controller.Closed += ControllerClosed;
        }

        private void ReloadClick(object sender, RoutedEventArgs e)
        {
            ReloadAsync();
        }

        private async void ReloadAsync()
        {
            // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
            await ConfigureControllerAsync().ConfigureAwait(true);
        }

        private void RunVersionChecks()
        {
            var versionContainer = new UnityContainer();
            versionContainer.RegisterType<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
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

        #region Fly-out

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

        #endregion Fly-out

        #region MetroStyle

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            if (_overrideProtection != 0)
            {
                _applicationStyle.SaveStyle();
            }
        }

        private void Theme(object sender, EventArgs e)
        {
            if (_overrideProtection != 0)
            {
                _applicationStyle.SetTheme(sender);
            }
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_overrideProtection != 0)
            {
                _applicationStyle.SetAccent(sender, e);
            }
        }

        #endregion MetroStyle
    }
}