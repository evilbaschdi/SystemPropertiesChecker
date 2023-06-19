using EvilBaschdi.Core;
using EvilBaschdi.DependencyInjection;
using SystemPropertiesChecker.Terminal;

IHostInstance hostInstance = new HostInstance();
IConfigureDelegateForConfigureServices configureDelegateForConfigureServices = new ConfigureDelegateForConfigureServices();
IConfigureServicesByHostBuilderAndConfigureDelegate configureServicesByHostBuilderAndConfigureDelegate =
    new ConfigureServicesByHostBuilderAndConfigureDelegate(hostInstance, configureDelegateForConfigureServices);
IServiceProvider serviceProvider = configureServicesByHostBuilderAndConfigureDelegate.Value;
IRun execute = new Execute(serviceProvider);
execute.Run();

Console.ReadLine();