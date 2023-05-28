using Avalonia.Controls;
using Avalonia.Input;
using EvilBaschdi.About.Avalonia;
using EvilBaschdi.About.Avalonia.Models;
using EvilBaschdi.About.Core;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Avalonia;

namespace SystemPropertiesChecker.Avalonia.Views;

/// <inheritdoc />
public partial class MainWindow : Window
{
    private readonly IHandleOsDependentTitleBar _handleOsDependentTitleBar;
    private readonly IApplicationLayout _applicationLayout;

    /// <summary>
    ///     Constructor
    /// </summary>
    public MainWindow()
    {
        InitializeComponent();
        _handleOsDependentTitleBar = new HandleOsDependentTitleBar();
        _applicationLayout = new ApplicationLayout();
        ApplyLayout();
    }

    private void ApplyLayout()
    {
        _handleOsDependentTitleBar.RunFor(this);
        _applicationLayout.RunFor((this, true, false));
    }

    // ReSharper disable UnusedParameter.Local
    private void LogoOnTapped(object sender, TappedEventArgs e)
        // ReSharper restore UnusedParameter.Local
    {
        ICurrentAssembly currentAssembly = new CurrentAssembly();
        IAboutContent aboutContent = new AboutContent(currentAssembly);
        IAboutViewModelExtended aboutViewModelExtended = new AboutViewModelExtended(aboutContent);
        var aboutWindow = new AboutWindow(aboutViewModelExtended, _applicationLayout, _handleOsDependentTitleBar);
        aboutWindow.ShowDialog(this);
    }
}