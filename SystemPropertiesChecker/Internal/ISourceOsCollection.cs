using System.Collections.ObjectModel;
using SystemPropertiesChecker.Models;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that provide RegistryValues.
    /// </summary>
    public interface ISourceOsCollection : IValue<ObservableCollection<SourceOs>>
    {
    }
}