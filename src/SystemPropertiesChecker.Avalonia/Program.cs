using Avalonia;
using EvilBaschdi.About.Avalonia.DependencyInjection;
using EvilBaschdi.Core.Avalonia.AppBuilderImplementations;
using SystemPropertiesChecker.Avalonia.DependencyInjection;
using SystemPropertiesChecker.Core.Internal;

namespace SystemPropertiesChecker.Avalonia;

// ReSharper disable once ClassNeverInstantiated.Global
internal class Program
{
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args) => BuildAvaloniaApp()
        .StartWithClassicDesktopLifetime(args);

    // Avalonia configuration, don't remove; also used by visual designer.
    // ReSharper disable once MemberCanBePrivate.Global
    public static AppBuilder BuildAvaloniaApp()
        => new AppBuilderImplementationToUseReactiveUIWithMicrosoftDependencyResolver<App>()
            .ValueFor(serviceCollection =>
                      {
                          serviceCollection.AddCoreServices();
                          serviceCollection.AddAboutServices();
                          serviceCollection.AddWindowsAndViewModels();
                      });
}