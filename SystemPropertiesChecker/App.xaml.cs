using System.Windows;
using EvilBaschdi.CoreExtended;
using EvilBaschdi.CoreExtended.AppHelpers;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.ViewModels;
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
        private readonly IHost _host;

        /// <inheritdoc />
        public App()
        {
            _host = Host.CreateDefaultBuilder()
                        .ConfigureServices((_, services) => { ConfigureServices(services); })
                        .Build();

            ServiceProvider = _host.Services;
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

            await _host.StartAsync();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices([NotNull] IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            IConfigureCoreServices configureCoreServices = new ConfigureCoreServices();
            configureCoreServices.RunFor(services);
            services.AddScoped<IScreenShot, ScreenShot>();
            services.AddScoped<IRoundCorners, RoundCorners>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddTransient(typeof(MainWindow));
        }

        /// <inheritdoc />
        protected override async void OnExit([NotNull] ExitEventArgs e)
        {
            if (e == null)
            {
                throw new ArgumentNullException(nameof(e));
            }

            using (_host)
            {
                await _host.StopAsync(TimeSpan.FromSeconds(5));
            }

            base.OnExit(e);
        }
    }
}