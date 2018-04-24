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
using SystemPropertiesChecker.ViewModel;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Metro;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
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
        private readonly MainWindowViewModel _mainWindowViewModel;
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
            IApplicationStyleSettings applicationStyleSettings = new ApplicationStyleSettings(applicationSettingsBase);
            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();

            _mainWindowViewModel = new MainWindowViewModel(applicationStyleSettings, themeManagerHelper);
            Loaded += MainWindowLoaded;

            //_applicationStyle = StyleContainer().Resolve<IApplicationStyle>();


            _dialogService = new DialogService(this);

            //LoadAsync();
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWindowViewModel;
        }


        //public UnityContainer StyleContainer()
        //{
        //    //IAppSettingsBase applicationSettingsBase = new AppSettingsBase(Settings.Default);
        //    //IApplicationStyleSettings coreSettings = new ApplicationStyleSettings(applicationSettingsBase);
        //    //IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();

        //    //_applicationStyle = new ApplicationStyle(this, Accent, ThemeSwitch, coreSettings, themeManagerHelper);

        //    var styleContainer = new UnityContainer();
        //    //styleContainer.RegisterInstance(Settings.Default);

        //    var settings = styleContainer.Resolve<Settings>();
        //    styleContainer.RegisterInstance(settings.Default)


        //    styleContainer.RegisterInstance(this);
        //    styleContainer.RegisterInstance(Accent);
        //    styleContainer.RegisterInstance(ThemeSwitch);


        //    styleContainer.RegisterType<IAppSettingsBase, AppSettingsBase>();
        //    styleContainer.RegisterType<IApplicationStyleSettings, ApplicationStyleSettings>();
        //    styleContainer.RegisterType<IThemeManagerHelper, ThemeManagerHelper>();
        //    styleContainer.RegisterType<IApplicationStyle, ApplicationStyle>();

        //    return styleContainer;
        //}

        /// <inheritdoc />
        /// <summary>
        ///     Executing code when window is shown.
        /// </summary>
        /// <param name="e"></param>
        //protected override async void OnContentRendered(EventArgs e)
        //{
        //    base.OnContentRendered(e);

        //    if (_windowShown)
        //    {
        //        return;
        //    }

        //    _windowShown = true;

        //    await ConfigureControllerAsync().ConfigureAwait(true);
        //}

        /// <summary>
        /// </summary>
        /// <returns></returns>
        //private async Task ConfigureControllerAsync()
        //{
        //    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;

        //    Cursor = Cursors.Wait;

        //    var options = new MetroDialogSettings
        //                  {
        //                      ColorScheme = MetroDialogColorScheme.Accented
        //                  };

        //    MetroDialogOptions = options;
        //    _controller = await this.ShowProgressAsync("Loading...", "Checking Properties", true, options).ConfigureAwait(true);
        //    _controller.SetIndeterminate();
        //    _controller.Canceled += ControllerCanceled;

        //    _task = Task.Factory.StartNew(RunVersionChecks);
        //    await _task.ConfigureAwait(true);
        //    _task.GetAwaiter().OnCompleted(TaskCompleted);
        //}

        //private void TaskCompleted()
        //{
        //    CurrentVersion.Text = _currentVersionText;
        //    WindowsVersion.Text = _windowsVersionText;
        //    DotNetVersion.Text = _dotNetVersionText;
        //    Other.Text = _otherText;

        //    _overrideProtection = 1;

        //    if (!string.IsNullOrWhiteSpace(_passwordExpirationMessage))
        //    {
        //        _dialogService.ShowMessage("Password Expiration", _passwordExpirationMessage);
        //        TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        //        TaskbarItemInfo.ProgressValue = 1;
        //    }

        //    DomainTab.Visibility = Visibility.Hidden;
        //    _controller.CloseAsync();
        //    _controller.Closed += ControllerClosed;
        //}

        //private void ControllerClosed(object sender, EventArgs e)
        //{
        //    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
        //    TaskbarItemInfo.ProgressValue = 1;
        //    Cursor = Cursors.Arrow;
        //}

        //private void ControllerCanceled(object sender, EventArgs e)
        //{
        //    _controller.CloseAsync();
        //    _controller.Closed += ControllerClosed;
        //}

        //private void ReloadClick(object sender, RoutedEventArgs e)
        //{
        //    ReloadAsync();
        //}

        //private async void ReloadAsync()
        //{
        //    // ReSharper disable once AsyncConverter.AsyncAwaitMayBeElidedHighlighting
        //    await ConfigureControllerAsync().ConfigureAwait(true);
        //}

    }
}