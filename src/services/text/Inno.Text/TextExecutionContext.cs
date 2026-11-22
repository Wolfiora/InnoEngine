using System;
using Inno.Core.Execution;

namespace Inno.Text;

/// <summary>
/// Binds one text service to the current asynchronous execution context.
/// </summary>
public static class TextExecutionContext
{
    private static readonly ExecutionSlot<ITextService> CURRENT_SCOPE = new("text");

    /// <summary>
    /// Gets the text service bound to the current execution context.
    /// </summary>
    public static ITextService current => CURRENT_SCOPE.current;

    /// <summary>
    /// Binds a text service until the returned strict last-in-first-out scope is disposed.
    /// </summary>
    /// <param name="text">
    /// The host-owned text service.
    /// </param>
    /// <returns>
    /// The caller-owned binding scope.
    /// </returns>
    public static IDisposable EnterScope(ITextService text)
    {
        ArgumentNullException.ThrowIfNull(text);
        return CURRENT_SCOPE.Enter(text);
    }
}
