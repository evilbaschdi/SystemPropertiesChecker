using System.Windows;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.ViewModel;
using EvilBaschdi.CoreExtended.AppHelpers;
using MahApps.Metro.Controls;

namespace SystemPropertiesChecker
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly MainWindowViewModel _mainWindowViewModel;

        /// <inheritdoc />
        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


            IVersionContainer versionContainer = new VersionContainer();
            IScreenShot screenShot = new ScreenShot();
            _mainWindowViewModel = new MainWindowViewModel(versionContainer, screenShot);
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWindowViewModel;
        }
    }
}