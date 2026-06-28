using Avalonia.Threading;
using FluentAvalonia.UI.Windowing;

namespace SystemPropertiesChecker.Avalonia.Views;

/// <inheritdoc />
public partial class MainWindow : FAAppWindow
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
    }

    /// <inheritdoc />
    protected override void OnOpened(EventArgs e)
    {
        base.OnOpened(e);

        Dispatcher.UIThread.Post(() => { MainNavigationView.SelectedItem = MainNavigationView.MenuItems.FirstOrDefault(); }, DispatcherPriority.Background);
    }
}