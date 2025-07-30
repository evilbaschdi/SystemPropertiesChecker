using EvilBaschdi.Core.Avalonia;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SystemPropertiesChecker.Avalonia.ViewModels;

namespace SystemPropertiesChecker.Avalonia.DepencencyInjection;

/// <summary />
public static class ConfigureWindowsAndViewModels
{
    /// <summary />
    public static void AddWindowsAndViewModels(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IHandleOsDependentTitleBar, HandleOsDependentTitleBar>();
        services.TryAddSingleton<IApplicationLayout, ApplicationLayout>();
        services.TryAddSingleton<IMainWindowByApplicationLifetime, MainWindowByApplicationLifetime>();
        services.AddSingleton<MainWindowViewModel>();
    }
}