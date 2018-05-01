﻿using System.Collections.Generic;
using EvilBaschdi.Core;

namespace SystemPropertiesChecker.Internal
{
    /// <inheritdoc />
    /// <summary>
    ///     Interface for classes that return a list of current installed .net versions
    /// </summary>
    public interface IDotNetVersion : IValue<List<string>>
    {
    }
}