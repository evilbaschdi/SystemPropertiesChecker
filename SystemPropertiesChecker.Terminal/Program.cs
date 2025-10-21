using EvilBaschdi.About.Terminal.DependencyInjection;
using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Terminal;

IServiceCollection serviceCollection = new ServiceCollection();

serviceCollection.AddCoreServices();
serviceCollection.AddAboutServices();
serviceCollection.AddTerminalServices();

IServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

IRun execute = new Execute(serviceProvider);
execute.Run();

Console.ReadLine();