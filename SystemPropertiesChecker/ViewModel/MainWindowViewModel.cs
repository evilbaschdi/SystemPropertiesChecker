using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using Unity;

namespace SystemPropertiesChecker.ViewModel
{
    /// <inheritdoc cref="INotifyPropertyChanged" />
    /// <summary>
    ///     MainWindowViewModel of SystemPropertiesChecker.
    /// </summary>
    public class MainWindowViewModel : ApplicationStyleViewModel
    {
        private readonly IScreenShot _screenShot;

        private readonly IVersionContainer _versionContainer;
        private Dictionary<string, string> _currentVersionText;
        private List<KeyValuePair<string, string>> _dotNetCoreVersionText;
        private string _dotNetVersionText;
        private List<KeyValuePair<string, string>> _otherText;
        private string _passwordExpirationMessage;
        private ObservableCollection<SourceOs> _sourceOsCollection;
        private Visibility _windowsTabVisibility;

        /// <inheritdoc />
        /// <summary>
        ///     Constructor
        /// </summary>
        protected internal MainWindowViewModel([NotNull] IVersionContainer versionContainer, [NotNull] IScreenShot screenShot)

        {
            _versionContainer = versionContainer ?? throw new ArgumentNullException(nameof(versionContainer));
            _screenShot = screenShot ?? throw new ArgumentNullException(nameof(screenShot));

            ScreenShot = new DefaultCommand
                         {
                             // ReSharper disable once StringLiteralTypo
                             Text = "screenshot",
                             Command = new RelayCommand(_ => ScreenShotCommand())
                         };
            AboutWindowClick = new DefaultCommand
                               {
                                   Text = "about",
                                   Command = new RelayCommand(_ => AboutWindowCommand())
                               };
            BuildCompositionRoot();
        }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public ICommandViewModel AboutWindowClick { get; set; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once UnusedMember.Global
        public List<KeyValuePair<string, string>> DotNetCoreVersionText
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
        // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once UnusedMember.Global
        public List<KeyValuePair<string, string>> OtherText
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
        // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
        public ICommandViewModel ScreenShot { get; set; }

        /// <summary>
        /// </summary>
        // ReSharper disable once UnusedMember.Global
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
        // ReSharper disable once UnusedMember.Global
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
        private void BuildCompositionRoot()
        {
            RunVersionChecks();
        }

        private void RunVersionChecks()
        {
            var versionContainer = _versionContainer.Value;

            _currentVersionText = versionContainer.Resolve<IWindowsVersionDictionary>().Value;
            _otherText = versionContainer.Resolve<IOtherInformationText>().Value;
            _dotNetVersionText = string.Join(Environment.NewLine, versionContainer.Resolve<IDotNetVersion>().Value);
            _dotNetCoreVersionText = versionContainer.Resolve<IDotNetCoreInfo>().Value;
            _passwordExpirationMessage = versionContainer.Resolve<IPasswordExpirationMessage>().Value;
            _sourceOsCollection = versionContainer.Resolve<ISourceOsCollection>().Value;
            _windowsTabVisibility = _sourceOsCollection.Any() ? Visibility.Visible : Visibility.Hidden;

            versionContainer.Dispose();
        }

        private void ScreenShotCommand()
        {
            var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
            var current = _screenShot.ValueFor(mainWindow);
            _screenShot.SaveToClipboard(current);
        }


        private static void AboutWindowCommand()
        {
            var assembly = typeof(MainWindow).Assembly;

            IAboutContent aboutWindowContent =
                new AboutContent(assembly, $@"{AppDomain.CurrentDomain.BaseDirectory}\spc.png");
            var aboutWindow = new AboutWindow
                              {
                                  DataContext = new AboutViewModel(aboutWindowContent)
                              };
            aboutWindow.ShowDialog();
        }
    }
}