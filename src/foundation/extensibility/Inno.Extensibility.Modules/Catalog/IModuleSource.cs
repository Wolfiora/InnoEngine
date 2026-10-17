using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Prepares one module generation without publishing it to the owning catalog.
/// </summary>
public interface IModuleSource
{
    /// <summary>
    /// Gets the stable logical name used by dependency ordering and replacement transactions.
    /// </summary>
    string moduleName { get; }
    /// <summary>
    /// Gets the ownership domain of the contribution.
    /// </summary>
    AssemblyDomain domain { get; }
    /// <summary>
    /// Gets the default dependency scope of contributed assemblies.
    /// </summary>
    AssemblyScope scope { get; }
    /// <summary>
    /// Gets whether retirement can release this source's code generation.
    /// </summary>
    bool collectible { get; }
    /// <summary>
    /// Gets the explicit logical upstream module names.
    /// </summary>
    IReadOnlyList<string> upstreamModuleNames { get; }
    /// <summary>
    /// Gets explicit scope overrides keyed by contributed assembly simple name.
    /// </summary>
    IReadOnlyDictionary<string, AssemblyScope> assemblyScopes { get; }
    /// <summary>
    /// Reads the complete owned assembly identities before any generation is acquired.
    /// </summary>
    /// <returns>
    /// Distinct, nonempty simple names; invalid or unavailable source data causes an exception.
    /// </returns>
    IReadOnlyList<string> GetAssemblyNames();
    /// <summary>
    /// Acquires a validated candidate and transfers its lifetime to the caller on success.
    /// </summary>
    /// <param name="context">
    /// The immutable candidate dependency closure and retirement owner.
    /// </param>
    /// <returns>
    /// A complete unpublished contribution; failure retires every acquired resource through the context.
    /// </returns>
    ModuleCatalogContribution Prepare(ModuleSourceContext context);
}
