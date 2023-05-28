using EvilBaschdi.About.Avalonia;
using EvilBaschdi.About.Avalonia.Models;
using EvilBaschdi.About.Core;
using EvilBaschdi.Core;
using EvilBaschdi.Core.Avalonia;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Avalonia.ViewModels;

namespace SystemPropertiesChecker.Avalonia.DepencencyInjection;

/// <inheritdoc />
public class ConfigureWindowsAndViewModels : IConfigureWindowsAndViewModels
{
    /// <inheritdoc />
    public void RunFor([NotNull] IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        //services.AddSingleton<AddEditAnnotationDialogViewModel>();
        //services.AddTransient(typeof(AddEditAnnotationDialog));

        services.AddSingleton<ICurrentAssembly, CurrentAssembly>();
        services.AddSingleton<IAboutContent, AboutContent>();
        services.AddSingleton<IAboutViewModelExtended, AboutViewModelExtended>();
        services.AddTransient(typeof(AboutWindow));

        services.AddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.AddSingleton<IApplicationLayout, ApplicationLayout>();
        services.AddSingleton<MainWindowViewModel>();
    }
}