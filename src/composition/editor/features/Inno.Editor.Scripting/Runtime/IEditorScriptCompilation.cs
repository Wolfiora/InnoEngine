using Inno.Scripting.Compiler;

namespace Inno.Editor.Scripting;
/// <summary>
/// Identifies whether the active project script generation can safely start game simulation.
/// </summary>
public enum EditorScriptCompilationState
{
    /// <summary>
    /// The script service has not produced its initial active generation.
    /// </summary>
    Initializing,
    /// <summary>
    /// A requested script generation is compiling, activating, or completing unload verification.
    /// </summary>
    Compiling,
    /// <summary>
    /// The most recent compilation succeeded, its generation is active, and retired generations passed GC verification.
    /// </summary>
    Ready,
    /// <summary>
    /// Compilation, activation, or generation retirement failed, so game simulation must not start.
    /// </summary>
    Failed
}

/// <summary>
/// Exposes the minimal script-generation readiness contract required by editor workflows.
/// </summary>
public interface IEditorScriptCompilation
{
    /// <summary>
    /// Gets whether compiler or activation work currently requires a blocking progress presentation.
    /// </summary>
    bool requiresExclusiveProgress { get; }

    /// <summary>
    /// Gets one immutable snapshot of the current reload operation and its cancellation policy.
    /// </summary>
    EditorScriptCompilationProgress progress { get; }

    /// <summary>
    /// Requests cancellation while retaining ownership until compilation and retirement finish.
    /// </summary>
    /// <param name="operationId">
    /// The request observed by the presentation; stale requests have no effect.
    /// </param>
    void CancelCompilation(long operationId);
    /// <summary>
    /// Queues a fresh cache-aware compilation for a build or simulation workflow.
    /// </summary>
    /// <returns>
    /// A ticket bound to the exact request. Success requires activation and completed retirement verification;
    /// a faulted host returns a failed ticket and requires restart.
    /// </returns>
    IScriptCompilationTicket RequestCompilation();
    /// <summary>
    /// Gets the most recently requested compilation ticket, or <see langword="null"/> when no explicit request exists.
    /// </summary>
    IScriptCompilationTicket? currentTicket { get; }

    /// <summary>
    /// Gets the current script-generation readiness state.
    /// </summary>
    EditorScriptCompilationState state { get; }


    /// <summary>
    /// Gets the most recently completed compilation, or <see langword="null"/> before the first attempt completes.
    /// </summary>
    ScriptCompilationResult? lastCompilation { get; }
}
