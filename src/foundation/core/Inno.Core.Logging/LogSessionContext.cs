using System;
using System.Threading;
using Inno.Core.Execution;

namespace Inno.Core.Logging;

/// <summary>
/// Associates logs written on one asynchronous execution flow with an isolated runtime session.
/// </summary>
public static class LogSessionContext
{
    private static readonly ExecutionSlot<LogSessionID> CURRENT = new("sessionId");

    /// <summary>
    /// Gets the runtime session associated with the current asynchronous execution flow.
    /// </summary>
    public static LogSessionID current => CURRENT.TryGet(out LogSessionID session) ? session : LogSessionID.none;

    /// <summary>
    /// Enters a nested execution scope whose emitted logs belong to the supplied runtime session.
    /// </summary>
    /// <param name="sessionID">
    /// The non-empty identifier of the runtime session receiving execution.
    /// </param>
    /// <returns>
    /// A scope that restores the previous session when disposed in reverse creation order.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="sessionID"/> does not identify a runtime session.
    /// </exception>
    public static IDisposable Enter(LogSessionID sessionID)
    {
        if (!sessionID.isAssigned)
        {
            throw new ArgumentException("A runtime log session identifier is required.", nameof(sessionID));
        }
        return CURRENT.Enter(sessionID);
    }
}
