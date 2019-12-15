using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;
using EvilBaschdi.CoreExtended.Metro;
using EvilBaschdi.CoreExtended.Mvvm;
using EvilBaschdi.CoreExtended.Mvvm.View;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
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
        private readonly IVersionContainer _versionContainer;
        private Dictionary<string, string> _currentVersionText;
        private string _dotNetCoreVersionText;
        private string _dotNetVersionText;
        private string _otherText;
        private string _passwordExpirationMessage;
        private ObservableCollection<SourceOs> _sourceOsCollection;
        private Visibility _windowsTabVisibility;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        protected internal MainWindowViewModel([NotNull] IThemeManagerHelper themeManagerHelper,
                                               [NotNull] IVersionContainer versionContainer)
            : base(themeManagerHelper)
        {
            _themeManagerHelper = themeManagerHelper ?? throw new ArgumentNullException(nameof(themeManagerHelper));
            _versionContainer = versionContainer ?? throw new ArgumentNullException(nameof(versionContainer));

            AboutWindowClick = new DefaultCommand
                               {
                                   Text = "About",
                                   Command = new RelayCommand(rc => BtnAboutWindowClick())
                               };
            BuildCompositionRoot();
        }

        /// <summary>
        /// </summary>
        public ICommandViewModel AboutWindowClick { get; set; }

        /// <summary>
        /// </summary>
        public Dictionary<string, string> CurrentVersionText
        {
            get => _currentVersionText;
            set
            {
                _currentVersionText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public string DotNetCoreVersionText
        {
            get => _dotNetCoreVersionText;
            set
            {
                _dotNetCoreVersionText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public string DotNetVersionText
        {
            get => _dotNetVersionText;
            set
            {
                _dotNetVersionText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public string OtherText
        {
            get => _otherText;
            set
            {
                _otherText = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public string PasswordExpirationMessage
        {
            get => _passwordExpirationMessage;
            set
            {
                _passwordExpirationMessage = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public ICommandViewModel Reload { get; set; }

        /// <summary>
        /// </summary>
        public ObservableCollection<SourceOs> SourceOsCollection
        {
            get => _sourceOsCollection;

            set
            {
                _sourceOsCollection = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// </summary>
        public Visibility WindowsTabVisibility
        {
            get => _windowsTabVisibility;
            set
            {
                _windowsTabVisibility = value;
                OnPropertyChanged();
            }
        }


        /// <summary>
        /// </summary>
        private async void BuildCompositionRoot()
        {
            RunVersionChecks();
        }

        private void RunVersionChecks()
        {
            var versionContainer = _versionContainer.Value;

            _currentVersionText = versionContainer.Resolve<IWindowsVersionDictionary>().Value;
            _otherText = versionContainer.Resolve<IOtherInformationText>().Value;
            _dotNetVersionText = versionContainer.Resolve<IDotNetVersion>().Value
                                                 .Aggregate(string.Empty, (c, v) => $"{c}{v}{Environment.NewLine}");
            _dotNetCoreVersionText = versionContainer.Resolve<IDotNetCoreInfo>().Value;
            _passwordExpirationMessage = versionContainer.Resolve<IPasswordExpirationMessage>().Value;
            _sourceOsCollection = versionContainer.Resolve<ISourceOsCollection>().Value;
            _windowsTabVisibility = _sourceOsCollection.Any() ? Visibility.Visible : Visibility.Hidden;

            versionContainer.Dispose();
        }


        private void BtnAboutWindowClick()
        {
            var aboutWindow = new AboutWindow();
            var assembly = typeof(MainWindow).Assembly;

            IAboutWindowContent aboutWindowContent =
                new AboutWindowContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\b.png");
            aboutWindow.DataContext = new AboutViewModel(aboutWindowContent, _themeManagerHelper);
            aboutWindow.ShowDialog();
        }
    }
}