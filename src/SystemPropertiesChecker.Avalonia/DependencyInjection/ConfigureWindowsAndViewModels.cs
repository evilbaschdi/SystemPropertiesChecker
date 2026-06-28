using EvilBaschdi.Core.Avalonia.Lifetime;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using SystemPropertiesChecker.Avalonia.ViewModels;

namespace SystemPropertiesChecker.Avalonia.DependencyInjection;

/// <summary />
public static class ConfigureWindowsAndViewModels
{
    /// <summary />
    public static void AddWindowsAndViewModels(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.TryAddSingleton<IMainWindowByApplicationLifetime, MainWindowByApplicationLifetime>();
        services.AddSingleton<MainWindowViewModel>();
    }
}