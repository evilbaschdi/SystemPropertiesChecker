using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPropertiesChecker.Terminal;

/// <summary>
///     Provides access to the application's service provider for dependency injection.
/// </summary>
public interface IInitializeServiceProvider : IValueFor<Action<IServiceCollection>, IServiceProvider>;