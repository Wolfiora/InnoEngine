using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Modules;
using Inno.Runtime;

namespace Inno.Player.Runtime;

/// <summary>
/// Activates explicit linked code without probing assemblies or opening runtime code files.
/// </summary>
public sealed class StaticPlayerModuleActivator : IPlayerModuleActivator
{
    private readonly GameCodeDeployment m_linkedDeployment;
    private readonly IReadOnlyList<IModuleSource> m_sources;

    /// <summary>
    /// Validates and freezes linked assembly ownership before the Player opens its runtime session.
    /// </summary>
    /// <param name="deployment">
    /// The build-generated code identity closure.
    /// </param>
    /// <param name="assemblies">
    /// Explicit linked assembly instances grouped by logical module; values are copied.
    /// </param>
    /// <exception cref="InvalidDataException">
    /// A linked assembly is absent, duplicated, or owned by an undeclared module.
    /// </exception>
    public StaticPlayerModuleActivator(
        GameCodeDeployment deployment,
        IReadOnlyDictionary<string, IReadOnlyList<Assembly>> assemblies
    ) {
        ArgumentNullException.ThrowIfNull(deployment);
        ArgumentNullException.ThrowIfNull(assemblies);
        if (assemblies.Count != deployment.modules.Count)
            throw new InvalidDataException("Linked module ownership differs from the code deployment.");
        var sources = new List<IModuleSource>(deployment.modules.Count);
        foreach (GameCodeModule module in deployment.modules)
        {
            if (!assemblies.TryGetValue(module.name, out IReadOnlyList<Assembly>? linked)
                || linked is null || linked.Any(static assembly => assembly is null)
                || !module.assemblies.Select(static assembly => assembly.name).SequenceEqual(
                    linked.Select(static assembly => assembly.GetName().Name), StringComparer.Ordinal))
                throw new InvalidDataException($"Linked code for module '{module.name}' differs from its deployment.");
            sources.Add(new StaticModuleSource(module.name, module.domain,
                AssemblyScope.Runtime, linked, module.dependencies));
        }
        m_linkedDeployment = deployment;
        m_sources = sources.AsReadOnly();
    }

    /// <inheritdoc />
    public void Activate(
        ModuleHost modules,
        GameCodeDeployment deployment
    ) {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(deployment);
        deployment.ValidateMatches(m_linkedDeployment);
        if (m_sources.Count == 0)
            return;
        using AssemblyReloadSession activation = modules.BeginReload(m_sources);
        activation.Activate();
        _ = activation.Complete();
    }
}
