using EvilBaschdi.Core;
using Microsoft.Extensions.DependencyInjection;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <inheritdoc />
    public interface IConfigureCoreServices : IRunFor<IServiceCollection>
    {
    }
}