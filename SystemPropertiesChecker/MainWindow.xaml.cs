using System;
using System.Windows;
using SystemPropertiesChecker.ViewModel;
using EvilBaschdi.CoreExtended.Metro;
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
            _mainWindowViewModel = new MainWindowViewModel(themeManagerHelper);
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWindowViewModel;
        }

        protected override void OnClosed(EventArgs e)
        {
            foreach (Window currentWindow in Application.Current.Windows)
            {
                if (currentWindow != Application.Current.MainWindow)
                {
                    currentWindow.Close();
                }
            }
            base.OnClosed(e);
        }
    }
}