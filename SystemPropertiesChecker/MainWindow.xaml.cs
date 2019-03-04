using System.Threading.Tasks;
using System.Windows;
using SystemPropertiesChecker.Properties;
using SystemPropertiesChecker.ViewModel;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.Metro;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace SystemPropertiesChecker
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly IApplicationStyle _applicationStyle;
        private readonly IDialogService _dialogService;
        private readonly MainWindowViewModel _mainWindowViewModel;
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

            //if (Settings.Default.UpgradeRequired)
            //{
            //    Settings.Default.Upgrade();
            //    Settings.Default.UpgradeRequired = false;
            //    Settings.Default.Save();
            //}

            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            //_dialogService = new DialogService(this);
            _mainWindowViewModel = new MainWindowViewModel(themeManagerHelper);
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWindowViewModel;
        }
    }
}