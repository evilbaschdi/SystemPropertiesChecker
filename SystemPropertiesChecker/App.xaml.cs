using System.Windows;
using EvilBaschdi.Core;
using EvilBaschdi.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
#if (!DEBUG)
using ControlzEx.Theming;

#endif

namespace SystemPropertiesChecker
{
    /// <inheritdoc />
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    // ReSharper disable once RedundantExtendsListEntry
    public partial class App : Application
    {
        private readonly IHandleAppExit _handleAppExit;
        private readonly IHandleAppStartup<MainWindow> _handleAppStartup;
        private MainWindow _mainWindow;

        /// <inheritdoc />
        public App()
        {
            IHostInstance hostInstance = new HostInstance();
            IValueFor<Action<HostBuilderContext, IServiceCollection>, IServiceProvider> initServiceProviderByHostBuilder = new InitServiceProviderByHostBuilder(hostInstance);

            ServiceProvider = initServiceProviderByHostBuilder.ValueFor(new ConfigureServices().RunFor);

            _handleAppStartup = new HandleAppStartup<MainWindow>(hostInstance);
            _handleAppExit = new HandleAppExit(hostInstance);
        }

        /// <summary>
        ///     ServiceProvider for DependencyInjection
        /// </summary>
        public static IServiceProvider ServiceProvider { get; private set; }

        /// <inheritdoc />
        protected override async void OnStartup(StartupEventArgs e)
        {
#if (!DEBUG)
            ThemeManager.Current.SyncTheme(ThemeSyncMode.SyncAll);
#endif
            _mainWindow = await _handleAppStartup.ValueForAsync(ServiceProvider);
            _mainWindow.Show();
        }

        /// <inheritdoc />
        protected override async void OnExit([NotNull] ExitEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            await _handleAppExit.RunAsync();

            base.OnExit(e);
        }
    }
}