using System.Collections.ObjectModel;
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
    }

    /// <summary>
    /// </summary>
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
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
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public bool WindowsTabVisibility => _sourceOsCollection.Value.Any();
}