using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Inno.Extensibility.Modules;
using Inno.Runtime;

namespace Inno.Player.Runtime;

/// <summary>
/// Activates deployment files through the existing candidate transaction and collectible module owner.
/// </summary>
public sealed class CollectiblePlayerModuleActivator : IPlayerModuleActivator
{
    /// <summary>
    /// Activates all declared modules after the Player has verified the managed file closure.
    /// </summary>
    /// <param name="modules">
    /// The engine's module owner.
    /// </param>
    /// <param name="deployment">
    /// The immutable runtime module declarations.
    /// </param>
    /// <param name="managedDirectory">
    /// The verified directory containing exactly the declared assemblies.
    /// </param>
    public void Activate(
        ModuleHost modules,
        IReadOnlyList<GameRuntimeModule> deployment,
        string managedDirectory
    ) {
        ArgumentNullException.ThrowIfNull(modules);
        ArgumentNullException.ThrowIfNull(deployment);
        ArgumentException.ThrowIfNullOrWhiteSpace(managedDirectory);
        AssemblyLoadRequest[] requests = deployment.Select(module => new AssemblyLoadRequest
        {
            moduleName = module.name,
            mainAssemblyPath = Path.Combine(managedDirectory, module.mainAssembly),
            preloadAssemblyPaths = module.preloadAssemblies
                .Select(fileName => Path.Combine(managedDirectory, fileName)).ToArray(),
            upstreamModuleNames = module.dependencies,
            collectible = true,
            domain = module.domain,
            scope = AssemblyScope.Runtime
        }).ToArray();
        using AssemblyReloadSession activation = modules.BeginReload(requests);
        activation.Activate();
        _ = activation.Complete();
    }
}
