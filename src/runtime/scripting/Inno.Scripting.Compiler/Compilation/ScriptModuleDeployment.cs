using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Inno.Extensibility.Modules;

namespace Inno.Scripting.Compiler;

/// <summary>
/// Describes the immutable assembly artifacts and dependency topology of one compiled script module.
/// The receiving host chooses how these artifacts enter its module catalog.
/// </summary>
public sealed class ScriptModuleDeployment
{
    internal ScriptModuleDeployment(
        string moduleName,
        string mainAssemblyPath,
        AssemblyDomain domain,
        AssemblyScope scope,
        IReadOnlyList<string> preloadAssemblyPaths,
        IReadOnlyList<string> upstreamModuleNames,
        IReadOnlyDictionary<string, AssemblyScope>? assemblyScopes = null
    ) {
        this.moduleName = moduleName;
        this.mainAssemblyPath = mainAssemblyPath;
        this.domain = domain;
        this.scope = scope;
        this.preloadAssemblyPaths = Array.AsReadOnly(preloadAssemblyPaths.ToArray());
        this.upstreamModuleNames = Array.AsReadOnly(upstreamModuleNames.ToArray());
        this.assemblyScopes = new ReadOnlyDictionary<string, AssemblyScope>(
            assemblyScopes is null
                ? new Dictionary<string, AssemblyScope>(StringComparer.OrdinalIgnoreCase)
                : new Dictionary<string, AssemblyScope>(assemblyScopes, StringComparer.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Gets the logical module name used to resolve dependencies within this compiled generation.
    /// </summary>
    public string moduleName { get; }

    /// <summary>
    /// Gets the absolute path of the module's primary assembly artifact.
    /// </summary>
    public string mainAssemblyPath { get; }

    /// <summary>
    /// Gets the ownership domain of this module's assemblies.
    /// </summary>
    public AssemblyDomain domain { get; }

    /// <summary>
    /// Gets the dependency scope used for assemblies without an explicit scope override.
    /// </summary>
    public AssemblyScope scope { get; }

    /// <summary>
    /// Gets the additional assembly artifacts that belong to this same module generation.
    /// </summary>
    public IReadOnlyList<string> preloadAssemblyPaths { get; }

    /// <summary>
    /// Gets the logical names of modules that supply managed dependencies to this module.
    /// </summary>
    public IReadOnlyList<string> upstreamModuleNames { get; }

    /// <summary>
    /// Gets explicit scopes keyed by assembly simple name, with case-insensitive lookup.
    /// </summary>
    public IReadOnlyDictionary<string, AssemblyScope> assemblyScopes { get; }
}
