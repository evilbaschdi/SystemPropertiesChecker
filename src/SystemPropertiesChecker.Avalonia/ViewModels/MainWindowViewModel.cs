using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using EvilBaschdi.About.Avalonia;
using EvilBaschdi.Core.Avalonia.DependencyInjection;
using FluentAvalonia.UI.Controls;
using ReactiveUI;
using SystemPropertiesChecker.Avalonia.Views;
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

    // ReSharper disable once ReplaceWithFieldKeyword
    private object _selectedMenuItem;
    private Control _currentPage;

    /// <summary>
    ///     Constructor
    /// </summary>
    /// <param name="dotNetCoreInfo"></param>
    /// <param name="dotNetVersion"></param>
    /// <param name="otherInformationText"></param>
    /// <param name="passwordExpirationMessage"></param>
    /// <param name="sourceOsCollection"></param>
    /// <param name="windowsVersionDictionary"></param>
    public MainWindowViewModel(
        IDotNetCoreInfo dotNetCoreInfo,
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

        AboutWindowCommand = ReactiveCommand.CreateFromTask(AboutWindowCommandAction);

        // Initial page
        _currentPage = new BasicView { DataContext = this };
    }

    /// <summary>
    /// </summary>
    public object SelectedMenuItem
    {
        get => _selectedMenuItem;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedMenuItem, value);
            UpdateCurrentPage();
        }
    }

    /// <summary>
    /// </summary>
    public Control CurrentPage
    {
        get => _currentPage;
        set => this.RaiseAndSetIfChanged(ref _currentPage, value);
    }

    private void UpdateCurrentPage()
    {
        if (SelectedMenuItem is FANavigationViewItem nvi)
        {
            var tag = nvi.Tag?.ToString();

            if (tag == "About")
            {
                _ = AboutWindowCommand.Execute().Subscribe();
                return;
            }

            CurrentPage = tag switch
            {
                "Basic" => new BasicView { DataContext = this },
                "History" => new HistoryView { DataContext = this },
                "DotNet" => new DotNetView { DataContext = this },
                "Other" => new OtherView { DataContext = this },
                _ => CurrentPage
            };
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public Dictionary<string, string> CurrentVersionText
    {
        get => _windowsVersionDictionary.Value;
        set => ArgumentNullException.ThrowIfNull(value);
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public List<KeyValuePair<string, string>> DotNetCoreVersionText
    {
        get => _dotNetCoreInfo.Value;
        set => ArgumentNullException.ThrowIfNull(value);
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public string DotNetVersionText
    {
        get => string.Join(Environment.NewLine, _dotNetVersion.Value);
        set => ArgumentNullException.ThrowIfNull(value);
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public List<KeyValuePair<string, string>> OtherText
    {
        get => _otherInformationText.Value;
        set => ArgumentNullException.ThrowIfNull(value);
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public string PasswordExpirationMessage
    {
        get => _passwordExpirationMessage.Value;
        set => ArgumentNullException.ThrowIfNull(value);
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public ObservableCollection<SourceOs> SourceOsCollection
    {
        get => _sourceOsCollection.Value;
        set => ArgumentNullException.ThrowIfNull(value);
    }

    /// <summary>
    /// </summary>
    public ReactiveCommand<Unit, Unit> AboutWindowCommand { get; set; }

    private async Task AboutWindowCommandAction()
    {
        var aboutWindow = ApplicationServices.GetRequiredService<AboutWindow>();
        var mainWindow = Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop
            ? desktop.MainWindow
            : null;
        if (mainWindow != null)
        {
            await aboutWindow.ShowDialog(mainWindow);
        }
    }

    /// <summary>
    /// </summary>
    // ReSharper disable once UnusedMember.Global
    public bool WindowsTabVisibility => _sourceOsCollection.Value.Any();
}