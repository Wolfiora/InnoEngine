using Inno.Runtime.Contracts;
using Inno.Scene;

namespace Inno.Runtime;

internal sealed class SceneRuntimeSubsystem(
    SceneWorld scenes,
    RuntimeSessionKind sessionKind
) : RuntimeSubsystem
{
    /// <summary>
    /// Advances non-editor scene systems by one deterministic interval.
    /// </summary>
    /// <param name="frame">
    /// The immutable fixed-step state.
    /// </param>
    protected override void OnFixedUpdate(RuntimeFixedFrame frame)
    {
        if (sessionKind != RuntimeSessionKind.Edit)
        {
            scenes.FixedUpdate(frame.deltaTime);
        }
    }

    /// <summary>
    /// Advances non-editor scene systems on the scaled variable clock.
    /// </summary>
    /// <param name="frame">
    /// The immutable variable-frame state.
    /// </param>
    protected override void OnUpdate(RuntimeFrame frame)
    {
        if (sessionKind != RuntimeSessionKind.Edit && !frame.isPaused)
        {
            scenes.Update(frame.deltaTime);
        }
    }

    /// <summary>
    /// Advances non-editor scene systems that depend on completed variable simulation.
    /// </summary>
    /// <param name="frame">
    /// The immutable variable-frame state.
    /// </param>
    protected override void OnLateUpdate(RuntimeFrame frame)
    {
        if (sessionKind != RuntimeSessionKind.Edit && !frame.isPaused)
        {
            scenes.LateUpdate(frame.deltaTime);
        }
    }
}
