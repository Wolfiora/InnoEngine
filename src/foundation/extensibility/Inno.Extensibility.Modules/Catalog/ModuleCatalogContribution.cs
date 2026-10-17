using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Reload;

namespace Inno.Extensibility.Modules;

/// <summary>
/// Freezes the assemblies, classification and lifetime of one acquired module generation.
/// </summary>
public sealed class ModuleCatalogContribution
{
    /// <summary>
    /// Acquires immutable views while transferring the generation lifetime to the contribution.
    /// </summary>
    /// <param name="moduleName">
    /// The stable module name.
    /// </param>
    /// <param name="domain">
    /// The ownership domain.
    /// </param>
    /// <param name="scope">
    /// The module's default dependency scope.
    /// </param>
    /// <param name="assemblies">
    /// The complete acquired assembly set.
    /// </param>
    /// <param name="assemblyScopes">
    /// The actual dependency scope of every acquired assembly.
    /// </param>
    /// <param name="lifetime">
    /// The transferred generation owner; null denotes externally owned static code.
    /// </param>
    /// <exception cref="ArgumentException">
    /// The name is empty, the assembly set is empty or contains duplicates, or scope metadata is incomplete.
    /// </exception>
    public ModuleCatalogContribution(
        string moduleName,
        AssemblyDomain domain,
        AssemblyScope scope,
        IReadOnlyList<Assembly> assemblies,
        IReadOnlyDictionary<Assembly, AssemblyScope> assemblyScopes,
        IModuleLifetime? lifetime = null
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(moduleName);
        ArgumentNullException.ThrowIfNull(assemblies);
        ArgumentNullException.ThrowIfNull(assemblyScopes);
        if (assemblies.Count == 0 || assemblies.Any(static value => value is null || value.IsDynamic) ||
            assemblies.Distinct().Count() != assemblies.Count ||
            assemblies.Any(assembly => !assemblyScopes.ContainsKey(assembly)) ||
            assemblyScopes.Count != assemblies.Count)
            throw new ArgumentException("A contribution requires distinct assemblies with complete scopes.");
        this.moduleName = moduleName;
        this.domain = domain;
        this.scope = scope;
        this.assemblies = Array.AsReadOnly(assemblies.ToArray());
        this.assemblyScopes = new ReadOnlyDictionary<Assembly, AssemblyScope>(
            assemblyScopes.ToDictionary(static pair => pair.Key, static pair => pair.Value));
        this.lifetime = lifetime;
    }
    /// <summary>
    /// Gets the stable module name.
    /// </summary>
    public string moduleName { get; }
    /// <summary>
    /// Gets the ownership domain.
    /// </summary>
    public AssemblyDomain domain { get; }
    /// <summary>
    /// Gets the default dependency scope.
    /// </summary>
    public AssemblyScope scope { get; }
    /// <summary>
    /// Gets the immutable owned assembly set.
    /// </summary>
    public IReadOnlyList<Assembly> assemblies { get; }
    /// <summary>
    /// Gets the complete per-assembly scope map.
    /// </summary>
    public IReadOnlyDictionary<Assembly, AssemblyScope> assemblyScopes { get; }
    /// <summary>
    /// Gets the owned generation lifetime, or null for externally owned code.
    /// </summary>
    public IModuleLifetime? lifetime { get; }
}
