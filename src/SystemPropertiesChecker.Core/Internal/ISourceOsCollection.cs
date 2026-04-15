using System.Collections.ObjectModel;
using EvilBaschdi.Core;
using SystemPropertiesChecker.Core.Models;

namespace SystemPropertiesChecker.Core.Internal;

/// <inheritdoc />
/// <summary>
///     Interface for classes that provide RegistryValues.
/// </summary>
public interface ISourceOsCollection : IValue<ObservableCollection<SourceOs>>;