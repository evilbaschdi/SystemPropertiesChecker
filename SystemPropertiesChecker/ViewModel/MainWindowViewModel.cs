using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Shell;
using SystemPropertiesChecker.Core;
using SystemPropertiesChecker.Internal;
using SystemPropertiesChecker.Models;
using EvilBaschdi.CoreExtended.Metro;
using EvilBaschdi.CoreExtended.Mvvm;
using EvilBaschdi.CoreExtended.Mvvm.View;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using MahApps.Metro.Controls.Dialogs;
using Unity;

namespace SystemPropertiesChecker.ViewModel
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     MainWindowViewModel of TestUi.
    /// </summary>
    public class MainWindowViewModel : ApplicationStyleViewModel
    {
        private readonly IThemeManagerHelper _themeManagerHelper;
        private ProgressDialogController _controller;
        private string _currentVersionText;
        private string _dotNetCoreVersionText;
        private string _dotNetVersionText;
        private ILinkerTime _linkerTime;
        private string _otherText;
        private string _passwordExpirationMessage;

        private TaskbarItemProgressState _progressState;
        private int _progressValue;
        private ObservableCollection<SourceOs> _sourceOsCollection;
        private Task _task;
        private string _windowsVersionText;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        protected internal MainWindowViewModel([NotNull] IThemeManagerHelper themeManagerHelper)
            : base(themeManagerHelper)
        {
            _themeManagerHelper = themeManagerHelper ?? throw new ArgumentNullException(nameof(themeManagerHelper));
            Reload = new DefaultCommand
                     {
                         Text = "reload",
                         Command = new RelayCommand(async rc => await ConfigureControllerAsync().ConfigureAwait(true))
                     };
     
            AboutWindowClick = new DefaultCommand
                               {
                                   Text = "About",
                                   Command = new RelayCommand(rc => BtnAboutWindowClick())
                               };
            BuildCompositionRoot();
        }

        public ICommandViewModel AboutWindowClick { get; set; }

        public string CurrentVersionText
        {
            get => _currentVersionText;
            set
            {
                _currentVersionText = value;
                OnPropertyChanged();
            }
        }

        public string DotNetCoreVersionText
        {
            get => _dotNetCoreVersionText;
            set
            {
                _dotNetCoreVersionText = value;
                OnPropertyChanged();
            }
        }

        public string DotNetVersionText
        {
            get => _dotNetVersionText;
            set
            {
                _dotNetVersionText = value;
                OnPropertyChanged();
            }
        }

        public string LinkerTime => _linkerTime.Value;

        public string OtherText
        {
            get => _otherText;
            set
            {
                _otherText = value;
                OnPropertyChanged();
            }
        }

        public string PasswordExpirationMessage
        {
            get => _passwordExpirationMessage;
            set
            {
                _passwordExpirationMessage = value;
                OnPropertyChanged();
            }
        }

        public TaskbarItemProgressState ProgressState
        {
            get => _progressState;
            set
            {
                _progressState = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// </summary>
        public ICommandViewModel Reload { get; set; }

        public ObservableCollection<SourceOs> SourceOsCollection
        {
            get => _sourceOsCollection;

            set
            {
                _sourceOsCollection = value;
                OnPropertyChanged();
            }
        }

        public string WindowsVersionText
        {
            get => _windowsVersionText;
            set
            {
                _windowsVersionText = value;
                OnPropertyChanged();
            }
        }

        private async void BuildCompositionRoot()
        {
            _linkerTime = new LinkerTime();
            RunVersionChecks();
            //await ConfigureControllerAsync().ConfigureAwait(true);
        }


        private async Task ConfigureControllerAsync()
        {
            // _progressState = TaskbarItemProgressState.Indeterminate;

            //Cursor = Cursors.Wait;

            var options = new MetroDialogSettings
                          {
                              ColorScheme = MetroDialogColorScheme.Accented
                          };

            //MetroDialogOptions = options;
            //_controller = await ShowProgressAsync("Loading...", "Checking Properties", true, options).ConfigureAwait(true);
            //_controller.SetIndeterminate();
            //_controller.Canceled += ControllerCanceled;

            _task = Task.Factory.StartNew(RunVersionChecks);
            await _task.ConfigureAwait(true);
            _task.GetAwaiter().OnCompleted(TaskCompleted);
        }

        private void TaskCompleted()
        {
            //CurrentVersion.Text = _currentVersionText;
            //WindowsVersion.Text = _windowsVersionText;
            //DotNetVersion.Text = _dotNetVersionText;
            //Other.Text = _otherText;

            //_overrideProtection = 1;

            if (!string.IsNullOrWhiteSpace(_passwordExpirationMessage))
            {
                //_dialogService.ShowMessage("Password Expiration", _passwordExpirationMessage);
                _progressState = TaskbarItemProgressState.Normal;
                _progressValue = 1;
            }

            //DomainTab.Visibility = Visibility.Hidden;
            //_controller.CloseAsync();
            //_controller.Closed += ControllerClosed;
        }

        private void ControllerClosed(object sender, EventArgs e)
        {
            _progressState = TaskbarItemProgressState.Normal;
            _progressValue = 1;
            //Cursor = Cursors.Arrow;
        }

        private void ControllerCanceled(object sender, EventArgs e)
        {
            _controller.CloseAsync();
            _controller.Closed += ControllerClosed;
        }

        private void RunVersionChecks()
        {
            var versionContainer = new UnityContainer();
            versionContainer.RegisterType<IDotNetVersionReleaseKeyMappingList, DotNetVersionReleaseKeyMappingList>();
            versionContainer.RegisterType<IDotNetVersion, DotNetVersion>();
            versionContainer.RegisterType<IDotNetCoreSdks, DotNetCoreSdks>();
            versionContainer.RegisterType<IDotNetCoreRuntimes, DotNetCoreRuntimes>();
            versionContainer.RegisterType<IDotNetCoreVersion, DotNetCoreVersion>();
            versionContainer.RegisterType<IRegistryValueFor, HklmSoftwareMicrosoftWindowsNtCurrentVersion>();
            versionContainer.RegisterType<ISourceOsCollection, HklmSystemSetupSourcesInstallDates>();
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
            _dotNetCoreVersionText = versionContainer.Resolve<IDotNetCoreVersion>().Value;
            _passwordExpirationMessage = versionContainer.Resolve<IPasswordExpirationMessage>().Value;
            _sourceOsCollection = versionContainer.Resolve<ISourceOsCollection>().Value;

            versionContainer.Dispose();
            //var temp = string.Empty;
            //DomainInformation.Text = temp;
            //DomainTab.Visibility = (!string.IsNullOrWhiteSpace(temp)).ToVisibility();
        }


        private void BtnAboutWindowClick()
        {
            var aboutWindow = new AboutWindow();
            var assembly = typeof(MainWindow).Assembly;

            IAboutWindowContent aboutWindowContent = new AboutWindowContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");
            aboutWindow.DataContext = new AboutViewModel(aboutWindowContent, _themeManagerHelper);
            aboutWindow.Show();
        }
    }
}