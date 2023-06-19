using EvilBaschdi.Core;
using EvilBaschdi.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Terminal;

IServiceCollection serviceCollection = new ServiceCollection();
IConfigureServiceCollection starup = new Startup();
starup.RunFor(serviceCollection);

IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

IRun execute = new Execute(serviceProvider);
execute.Run();

Console.ReadLine();