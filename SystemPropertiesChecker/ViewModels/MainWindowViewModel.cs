using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using EvilBaschdi.CoreExtended.Controls.About;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel;
using EvilBaschdi.CoreExtended.Mvvm.ViewModel.Command;
using JetBrains.Annotations;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.ViewModels;

/// <inheritdoc cref="INotifyPropertyChanged" />
/// <summary>
///     MainWindowViewModel of SystemPropertiesChecker.
/// </summary>
public class MainWindowViewModel : ApplicationStyleViewModel
{
    private readonly IDotNetCoreInfo _dotNetCoreInfo;
    private readonly IDotNetVersion _dotNetVersion;
    private readonly IOtherInformationText _otherInformationText;
    private readonly IPasswordExpirationMessage _passwordExpirationMessage;
    private readonly IScreenShot _screenShot;
    private readonly ISourceOsCollection _sourceOsCollection;
    private readonly IWindowsVersionDictionary _windowsVersionDictionary;

    /// <inheritdoc />
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindowViewModel([NotNull] IScreenShot screenShot,
                               [NotNull] IWindowsVersionDictionary windowsVersionDictionary,
                               [NotNull] IOtherInformationText otherInformationText,
                               [NotNull] IDotNetVersion dotNetVersion,
                               [NotNull] IDotNetCoreInfo dotNetCoreInfo,
                               [NotNull] ISourceOsCollection sourceOsCollection,
                               [NotNull] IPasswordExpirationMessage passwordExpirationMessage,
                               [NotNull] IRoundCorners roundCorners)
        : base(roundCorners)

    {
        if (roundCorners == null)
        {
            throw new ArgumentNullException(nameof(roundCorners));
        }

        _screenShot = screenShot ?? throw new ArgumentNullException(nameof(screenShot));
        _windowsVersionDictionary = windowsVersionDictionary ?? throw new ArgumentNullException(nameof(windowsVersionDictionary));
        _otherInformationText = otherInformationText ?? throw new ArgumentNullException(nameof(otherInformationText));
        _dotNetVersion = dotNetVersion ?? throw new ArgumentNullException(nameof(dotNetVersion));
        _dotNetCoreInfo = dotNetCoreInfo ?? throw new ArgumentNullException(nameof(dotNetCoreInfo));
        _sourceOsCollection = sourceOsCollection ?? throw new ArgumentNullException(nameof(sourceOsCollection));
        _passwordExpirationMessage = passwordExpirationMessage ?? throw new ArgumentNullException(nameof(passwordExpirationMessage));

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
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    // ReSharper disable once MemberCanBePrivate.Global
    // ReSharper disable once AutoPropertyCanBeMadeGetOnly.Global
    public ICommandViewModel AboutWindowClick { get; set; }

    /// <summary>
    /// </summary>
    [NotNull]
    // ReSharper disable once UnusedMember.Global
    public Dictionary<string, string> CurrentVersionText
    {
        get => _windowsVersionDictionary.Value;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// </summary>
    [NotNull]
    // ReSharper disable once UnusedMember.Global
    public List<KeyValuePair<string, string>> DotNetCoreVersionText
    {
        get => _dotNetCoreInfo.Value;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// </summary>
    [NotNull]
    // ReSharper disable once UnusedMember.Global

    public string DotNetVersionText
    {
        get => string.Join(Environment.NewLine, _dotNetVersion.Value);
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// </summary>
    [NotNull]
    // ReSharper disable once UnusedMember.Global
    public List<KeyValuePair<string, string>> OtherText
    {
        get => _otherInformationText.Value;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// </summary>
    [NotNull]
    // ReSharper disable once UnusedMember.Global
    public string PasswordExpirationMessage
    {
        get => _passwordExpirationMessage.Value;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

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
    [NotNull]
    // ReSharper disable once UnusedMember.Global
    public ObservableCollection<SourceOs> SourceOsCollection
    {
        get => _sourceOsCollection.Value;
        set
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            OnPropertyChanged();
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public Visibility WindowsTabVisibility
    {
        get => _sourceOsCollection.Value.Any() ? Visibility.Visible : Visibility.Hidden;
        set
        {
            if (!Enum.IsDefined(typeof(Visibility), value))
            {
                throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(Visibility));
            }

            OnPropertyChanged();
        }
    }

    private void ScreenShotCommand()
    {
        var mainWindow = Application.Current.Windows.OfType<MainWindow>().FirstOrDefault();
        var current = _screenShot.ValueFor(mainWindow);
        _screenShot.SaveToClipboard(current);
    }

    private static void AboutWindowCommand()
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutModel aboutModel = new AboutViewModel(aboutContent);
        var aboutWindow = new AboutWindow(aboutModel);
        aboutWindow.ShowDialog();
    }
}