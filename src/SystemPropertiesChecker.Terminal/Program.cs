using EvilBaschdi.About.Terminal.DependencyInjection;
using SystemPropertiesChecker.Core.Internal;
using SystemPropertiesChecker.Terminal;

var serviceProvider = new InitializeServiceProvider()
    .ValueFor(serviceCollection =>
              {
                  serviceCollection.AddCoreServices();
                  serviceCollection.AddAboutServices();
                  serviceCollection.AddTerminalServices();
              });

new Execute(serviceProvider).Run();

Console.ReadLine();