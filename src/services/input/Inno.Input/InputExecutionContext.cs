using System;
using Inno.Core.Execution;

namespace Inno.Input;

/// <summary>
/// Binds one input service to the current asynchronous execution context.
/// </summary>
public static class InputExecutionContext
{
    private static readonly ExecutionSlot<IInputService> CURRENT_SCOPE = new("input");

    /// <summary>
    /// Gets the input service bound to the current execution context.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no input scope is active.
    /// </exception>
    public static IInputService current => CURRENT_SCOPE.current;

    /// <summary>
    /// Tries to read input when a host has installed an input subsystem for this frame.
    /// </summary>
    /// <param name="input">
    /// Bound service, or null when unavailable.
    /// </param>
    /// <returns>
    /// Whether a live input service is bound.
    /// </returns>
    public static bool TryGet(out IInputService? input) => CURRENT_SCOPE.TryGet(out input);

    /// <summary>
    /// Binds an input service until the returned strict last-in-first-out scope is disposed.
    /// </summary>
    /// <param name="input">
    /// The host-owned input service.
    /// </param>
    /// <returns>
    /// A strict last-in-first-out execution scope.
    /// </returns>
    public static IDisposable EnterScope(IInputService input)
    {
        ArgumentNullException.ThrowIfNull(input);
        return CURRENT_SCOPE.Enter(input);
    }
}
