using System.Linq;
using System.Windows;
using System.Windows.Controls;
using MahApps.Metro.Controls;
using WinSPCheck.Core;
using WinSPCheck.Internal;

namespace WinSPCheck
{
    // ReSharper disable once RedundantExtendsListEntry
    public partial class MainWindow : MetroWindow
    {
        private readonly ApplicationStyle _style;

        /// <summary>
        ///     MainWindow
        /// </summary>
        public MainWindow()
        {
            _style = new ApplicationStyle(this);
            InitializeComponent();
            _style.Load();
            RunVersionChecks();
        }

        private void RunVersionChecks()
        {
            var dotNetVersion = new DotNetVersion();
            var registryValue = new HklmSoftwareMicrosoftWindowsNtCurrentVersion();
            var windowsVersionInformationHelper = new WindowsVersionInformationHelper();
            var windowsVersionInformation = new GetWindowsVersionInformation(registryValue, windowsVersionInformationHelper);
            var windowsVersionInformationStack = new GetWindowsVersionInfromationStack(dotNetVersion, windowsVersionInformation);
            CurrentVersion.Text = windowsVersionInformationStack.Value;
        }

        #region Flyout

        private void ToggleSettingsFlyoutClick(object sender, RoutedEventArgs e)
        {
            ToggleFlyout(0);
        }

        private void ToggleFlyout(int index, bool stayOpen = false)
        {
            var activeFlyout = (Flyout) Flyouts.Items[index];
            if(activeFlyout == null)
            {
                return;
            }

            foreach(
                var nonactiveFlyout in
                    Flyouts.Items.Cast<Flyout>()
                        .Where(nonactiveFlyout => nonactiveFlyout.IsOpen && nonactiveFlyout.Name != activeFlyout.Name))
            {
                nonactiveFlyout.IsOpen = false;
            }

            if(activeFlyout.IsOpen && stayOpen)
            {
                activeFlyout.IsOpen = true;
            }
            else
            {
                activeFlyout.IsOpen = !activeFlyout.IsOpen;
            }
        }

        #endregion Flyout

        #region Style

        private void SaveStyleClick(object sender, RoutedEventArgs e)
        {
            _style.SaveStyle();
        }

        private void Theme(object sender, RoutedEventArgs e)
        {
            _style.SetTheme(sender, e);
        }

        private void AccentOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _style.SetAccent(sender, e);
        }

        #endregion Style
    }
}