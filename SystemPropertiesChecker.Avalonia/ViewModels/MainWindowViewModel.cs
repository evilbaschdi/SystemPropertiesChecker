using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using EvilBaschdi.About.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Core.Internal.DotNet;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Avalonia.ViewModels;

/// <inheritdoc />
public class MainWindowViewModel : ViewModelBase
{
    private readonly IDotNetCoreInfo _dotNetCoreInfo;
    private readonly IDotNetVersion _dotNetVersion;
    private readonly IOtherInformationText _otherInformationText;
    private readonly IPasswordExpirationMessage _passwordExpirationMessage;
    private readonly ISourceOsCollection _sourceOsCollection;
    private readonly IWindowsVersionDictionary _windowsVersionDictionary;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dotNetCoreInfo"></param>
    /// <param name="dotNetVersion"></param>
    /// <param name="otherInformationText"></param>
    /// <param name="passwordExpirationMessage"></param>
    /// <param name="sourceOsCollection"></param>
    /// <param name="windowsVersionDictionary"></param>
    public MainWindowViewModel(IDotNetCoreInfo dotNetCoreInfo,
                               IDotNetVersion dotNetVersion,
                               IOtherInformationText otherInformationText,
                               IPasswordExpirationMessage passwordExpirationMessage,
                               ISourceOsCollection sourceOsCollection,
                               IWindowsVersionDictionary windowsVersionDictionary
    )
    {
        _dotNetCoreInfo = dotNetCoreInfo ?? throw new ArgumentNullException(nameof(dotNetCoreInfo));
        _dotNetVersion = dotNetVersion ?? throw new ArgumentNullException(nameof(dotNetVersion));
        _otherInformationText = otherInformationText ?? throw new ArgumentNullException(nameof(otherInformationText));
        _passwordExpirationMessage = passwordExpirationMessage ?? throw new ArgumentNullException(nameof(passwordExpirationMessage));
        _sourceOsCollection = sourceOsCollection ?? throw new ArgumentNullException(nameof(sourceOsCollection));
        _windowsVersionDictionary = windowsVersionDictionary ?? throw new ArgumentNullException(nameof(windowsVersionDictionary));

        AboutWindowCommand = ReactiveCommand.Create(AboutWindowCommandAction);
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public Dictionary<string, string> CurrentVersionText
    {
        get => _windowsVersionDictionary.Value;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public List<KeyValuePair<string, string>> DotNetCoreVersionText
    {
        get => _dotNetCoreInfo.Value;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global

    public string DotNetVersionText
    {
        get => string.Join(Environment.NewLine, _dotNetVersion.Value);
        set
        {
            ArgumentNullException.ThrowIfNull(value);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public string Greeting => "Welcome to Avalonia!";

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public List<KeyValuePair<string, string>> OtherText
    {
        get => _otherInformationText.Value;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public string PasswordExpirationMessage
    {
        get => _passwordExpirationMessage.Value;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public ObservableCollection<SourceOs> SourceOsCollection
    {
        get => _sourceOsCollection.Value;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
        }
    }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> AboutWindowCommand { get; set; }

    private void AboutWindowCommandAction()
    {
        var aboutWindow = App.ServiceProvider.GetRequiredService<AboutWindow>();
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop ? desktop.MainWindow : null;
        if (mainWindow != null)
        {
            aboutWindow.ShowDialog(mainWindow);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public bool WindowsTabVisibility => _sourceOsCollection.Value.Any();
}