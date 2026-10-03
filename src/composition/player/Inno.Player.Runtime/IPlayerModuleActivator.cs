using System.Collections.Generic;
using Inno.Extensibility.Modules;
using Inno.Runtime;

namespace Inno.Player.Runtime;

/// <summary>
/// Activates a verified managed deployment without exposing host platform decisions to the Player.
/// </summary>
public interface IPlayerModuleActivator
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
    void Activate(
        ModuleHost modules,
        IReadOnlyList<GameRuntimeModule> deployment,
        string managedDirectory
    );
}
