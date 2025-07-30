using EvilBaschdi.Core;
using EvilBaschdi.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Terminal;

IServiceCollection serviceCollection = new ServiceCollection();
IConfigureServiceCollection startup = new Startup();
startup.RunFor(serviceCollection);

IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

IRun execute = new Execute(serviceProvider);
execute.Run();

Console.ReadLine();