using System;
using System.Collections;
using Inno.Core.Coroutines;
using Inno.Core.Execution;

namespace Inno.Runtime;

/// <summary>
/// Runs owner-bound routines in the current session and script generation.
/// </summary>
public static class SessionCoroutines
{
    private static readonly ExecutionSlot<RuntimeSession> SESSION = new("session coroutines");

    /// <summary>
    /// Transfers an enumerator to the active session. Retirement of its owner, executing module or wait ends the routine.
    /// </summary>
    /// <param name="owner">
    /// The object that owns this routine until completion or explicit retirement.
    /// </param>
    /// <param name="routine">
    /// The enumerator to advance on the session thread.
    /// </param>
    /// <returns>
    /// A handle valid until completion, cancellation or retirement.
    /// </returns>
    /// <exception cref="InvalidOperationException">
    /// No live session execution scope is bound on this thread.
    /// </exception>
    public static CoroutineHandle Start(
        object owner,
        IEnumerator routine
    ) {
        ArgumentNullException.ThrowIfNull(owner);
        ArgumentNullException.ThrowIfNull(routine);
        return SESSION.current.StartCoroutine(owner, routine);
    }

    /// <summary>
    /// Requests cancellation when the handle belongs to the current session generation.
    /// </summary>
    /// <param name="handle">
    /// The handle returned on admission.
    /// </param>
    /// <returns>
    /// Whether the live routine was scheduled for cancellation.
    /// </returns>
    public static bool Stop(CoroutineHandle handle) => SESSION.current.StopCoroutine(handle);

    /// <summary>
    /// Stops the current generation's routines owned by the supplied object.
    /// </summary>
    /// <param name="owner">
    /// The exact owner supplied on admission.
    /// </param>
    public static void StopAll(object owner) => SESSION.current.StopCoroutines(owner);

    internal static IDisposable EnterScope(RuntimeSession session) => SESSION.Enter(session, threadAffine: true);

    internal static IDisposable Suspend() => SESSION.Suspend();
}
