using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Modules;

namespace Inno.Runtime;

/// <summary>
/// Contributes statically linked module code to the same catalog transactions used by authoring hosts.
/// </summary>
public sealed class StaticModuleSource : IModuleSource
{
    private readonly ModuleCatalogContribution m_contribution;

    /// <summary>
    /// Freezes a linked module's explicit assemblies and logical dependencies.
    /// </summary>
    /// <param name="moduleName">
    /// The stable logical module name.
    /// </param>
    /// <param name="domain">
    /// The code ownership domain.
    /// </param>
    /// <param name="scope">
    /// The dependency scope of every supplied assembly.
    /// </param>
    /// <param name="assemblies">
    /// The linked assemblies; the runtime owns their process lifetime.
    /// </param>
    /// <param name="dependencies">
    /// The explicitly declared logical upstream modules.
    /// </param>
    public StaticModuleSource(
        string moduleName,
        AssemblyDomain domain,
        AssemblyScope scope,
        IReadOnlyList<Assembly> assemblies,
        IReadOnlyList<string> dependencies
    ) {
        ArgumentNullException.ThrowIfNull(dependencies);
        ArgumentNullException.ThrowIfNull(assemblies);
        if (assemblies.Any(static assembly => assembly is null || assembly.IsCollectible))
            throw new ArgumentException("A static module requires noncollectible linked assemblies.", nameof(assemblies));
        m_contribution = new ModuleCatalogContribution(moduleName, domain, scope, assemblies,
            assemblies.ToDictionary(static assembly => assembly, _ => scope));
        upstreamModuleNames = Array.AsReadOnly(dependencies.ToArray());
        assemblyScopes = new ReadOnlyDictionary<string, AssemblyScope>(
            assemblies.ToDictionary(static assembly => assembly.GetName().Name!, _ => scope, StringComparer.OrdinalIgnoreCase));
    }

    /// <inheritdoc />
    public string moduleName => m_contribution.moduleName;
    /// <inheritdoc />
    public AssemblyDomain domain => m_contribution.domain;
    /// <inheritdoc />
    public AssemblyScope scope => m_contribution.scope;
    /// <inheritdoc />
    public bool collectible => false;
    /// <inheritdoc />
    public IReadOnlyList<string> upstreamModuleNames { get; }
    /// <inheritdoc />
    public IReadOnlyDictionary<string, AssemblyScope> assemblyScopes { get; }
    /// <inheritdoc />
    public IReadOnlyList<string> GetAssemblyNames()
        => m_contribution.assemblies.Select(static assembly => assembly.GetName().Name!).ToArray();
    /// <inheritdoc />
    public ModuleCatalogContribution Prepare(ModuleSourceContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return m_contribution;
    }
}
