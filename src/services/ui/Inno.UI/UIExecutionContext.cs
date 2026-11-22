using System;
using Inno.Core.Execution;

namespace Inno.UI;

/// <summary>
/// Binds one UI service to the current asynchronous execution context.
/// </summary>
public static class UIExecutionContext
{
    private static readonly ExecutionSlot<IUIService> CURRENT_SCOPE = new("ui");

    /// <summary>
    /// Gets the UI service bound to the current execution context.
    /// </summary>
    public static IUIService current => CURRENT_SCOPE.current;

    /// <summary>
    /// Binds a UI service until the returned strict last-in-first-out scope is disposed.
    /// </summary>
    /// <param name="ui">
    /// The host-owned UI service.
    /// </param>
    /// <returns>
    /// The caller-owned binding scope.
    /// </returns>
    public static IDisposable EnterScope(IUIService ui)
    {
        ArgumentNullException.ThrowIfNull(ui);
        return CURRENT_SCOPE.Enter(ui);
    }
}
