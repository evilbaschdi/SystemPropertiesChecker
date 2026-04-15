using Avalonia.Controls;
using EvilBaschdi.Core.Avalonia;

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
        ApplyLayout();
    }

    private void ApplyLayout()
    {
        var handleOsDependentTitleBar = ApplicationServices.GetRequiredService<IHandleOsDependentTitleBar>();
        handleOsDependentTitleBar?.RunFor(this);

        var applicationLayout = ApplicationServices.GetRequiredService<IApplicationLayout>();
        applicationLayout?.RunFor((this, true, false));
    }
}