using System;
using Inno.Core.Execution;

namespace Inno.Animation;

/// <summary>
/// Binds one animation service to the current asynchronous execution context.
/// </summary>
public static class AnimationExecutionContext
{
    private static readonly ExecutionSlot<IAnimationService> CURRENT_SCOPE = new("animation");

    /// <summary>
    /// Gets the animation service bound to the current execution context.
    /// </summary>
    public static IAnimationService current => CURRENT_SCOPE.current;

    /// <summary>
    /// Binds one animation service until the returned strict scope is disposed.
    /// </summary>
    /// <param name="animation">
    /// Session-owned animation service.
    /// </param>
    /// <returns>
    /// A strict last-in-first-out execution scope.
    /// </returns>
    public static IDisposable EnterScope(IAnimationService animation)
    {
        ArgumentNullException.ThrowIfNull(animation);
        return CURRENT_SCOPE.Enter(animation);
    }
}
