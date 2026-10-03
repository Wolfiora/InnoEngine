using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Inno.Extensibility.Modules;
using Inno.Runtime;

namespace Inno.Player.Runtime;

/// <summary>
/// Registers deployment assemblies already linked into the process runtime.
/// </summary>
public sealed class LinkedPlayerModuleActivator : IPlayerModuleActivator
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
        foreach (GameRuntimeModule module in deployment)
        {
            Assembly[] assemblies = module.preloadAssemblies.Prepend(module.mainAssembly)
                .Select(static fileName => Assembly.Load(
                    new AssemblyName(Path.GetFileNameWithoutExtension(fileName)))).ToArray();
            modules.Register(module.name, assemblies);
        }
    }
}
