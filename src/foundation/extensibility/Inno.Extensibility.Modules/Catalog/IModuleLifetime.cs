using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Owns implementation-specific resources acquired for one unpublished or active module generation.
/// </summary>
public interface IModuleLifetime
{
    /// <summary>
    /// Begins retirement once all catalog participants have released the generation.
    /// </summary>
    /// <returns>
    /// A non-owning probe that completes only after code and generation storage are released.
    /// </returns>
    IAssemblyUnloadProbe BeginRetirement();
}
