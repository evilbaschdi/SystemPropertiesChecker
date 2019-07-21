using System.Collections.ObjectModel;
using SystemPropertiesChecker.Core.Models;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Core.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public interface ISourceOsCollection : IValue<ObservableCollection<SourceOs>>
    {
    }
}