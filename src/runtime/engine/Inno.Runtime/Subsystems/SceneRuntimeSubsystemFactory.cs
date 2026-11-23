using Inno.Runtime.Contracts;
using Inno.Scene;

namespace Inno.Runtime;

internal sealed class SceneRuntimeSubsystemFactory(
    SceneWorld scenes,
    RuntimeSessionKind kind
) : IRuntimeSubsystemFactory
{
    internal static readonly RuntimeSubsystemID SUBSYSTEM_ID = new("inno.runtime.scene");

    /// <summary>
    /// Gets the stable descriptor for the built-in scene simulation subsystem.
    /// </summary>
    public RuntimeSubsystemDescriptor descriptor { get; } = new(SUBSYSTEM_ID, order: 0);

    /// <summary>
    /// Creates the scene subsystem for the isolated runtime session.
    /// </summary>
    /// <param name="context">
    /// The session context that supplies the scene world and session kind.
    /// </param>
    /// <returns>
    /// A new subsystem that advances the context's scene world.
    /// </returns>
    public IRuntimeSubsystem Create(RuntimeSubsystemContext context) => new SceneRuntimeSubsystem(scenes, kind);
}
