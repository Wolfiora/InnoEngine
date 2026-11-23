using System;
using Inno.Core.Execution;

namespace Inno.Storage;

/// <summary>
/// Binds application storage to the current asynchronous execution context.
/// </summary>
public static class StorageExecutionContext
{
    private static readonly ExecutionSlot<IApplicationStorage> CURRENT_SCOPE = new("storage");

    /// <summary>
    /// Gets the storage service bound to the current execution context.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no storage scope is active.
    /// </exception>
    public static IApplicationStorage current => CURRENT_SCOPE.current;

    /// <summary>
    /// Binds a storage service until the returned strict last-in-first-out scope is disposed.
    /// </summary>
    /// <param name="storage">
    /// The host-owned application storage service.
    /// </param>
    /// <returns>
    /// A strict last-in-first-out execution scope.
    /// </returns>
    public static IDisposable EnterScope(IApplicationStorage storage)
    {
        ArgumentNullException.ThrowIfNull(storage);
        return CURRENT_SCOPE.Enter(storage);
    }
}
