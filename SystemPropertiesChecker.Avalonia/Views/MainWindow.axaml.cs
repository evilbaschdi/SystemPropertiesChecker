using Avalonia.Controls;
using Avalonia.Input;
using EvilBaschdi.Avalonia.Core;
using EvilBaschdi.Avalonia.Core.Controls.About;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Avalonia.Views;

/// <inheritdoc />
public partial class MainWindow : Window
{
    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        Load();
    }

    private void Load()
    {
        IHandleOsDependentTitleBar handleOsDependentTitleBar = new HandleOsDependentTitleBar();
        handleOsDependentTitleBar.RunFor((this, HeaderPanel, MainPanel));
    }

    // ReSharper disable UnusedParameter.Local
    private void LogoOnDoubleTapped(object sender, TappedEventArgs e)
        // ReSharper restore UnusedParameter.Local
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutModel aboutModel = new AboutViewModel(aboutContent);
        var aboutWindow = new AboutWindow
                          {
                              DataContext = aboutModel
                          };
        aboutWindow.ShowDialog(this);
    }
}