using System;
using System.Windows;
using SystemPropertiesChecker.Core.Internal;
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

        /// <inheritdoc />
        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();


            IThemeManagerHelper themeManagerHelper = new ThemeManagerHelper();
            IVersionContainer versionContainer = new VersionContainer();
            _mainWindowViewModel = new MainWindowViewModel(themeManagerHelper, versionContainer);
            Loaded += MainWindowLoaded;
        }

        private void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            DataContext = _mainWindowViewModel;
        }

        /// <inheritdoc />
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