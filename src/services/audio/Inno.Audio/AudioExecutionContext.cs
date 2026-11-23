using System;
using Inno.Core.Execution;

namespace Inno.Audio;

/// <summary>
/// Binds one host-owned audio service to the current asynchronous execution context.
/// </summary>
public static class AudioExecutionContext
{
    private static readonly ExecutionSlot<IAudioService> CURRENT_SCOPE = new("audio");

    /// <summary>
    /// Gets the audio service bound to the current asynchronous execution context.
    /// </summary>
    /// <exception cref="InvalidOperationException">
    /// Thrown when no audio service is active for the caller.
    /// </exception>
    public static IAudioService current => CURRENT_SCOPE.current;

    /// <summary>
    /// Binds an audio service until the returned strict last-in-first-out scope is disposed.
    /// </summary>
    /// <param name="audio">
    /// Host-owned audio service exposed to script-facing operations.
    /// </param>
    /// <returns>
    /// A strict last-in-first-out scope owned by the caller.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="audio"/> is <see langword="null"/>.
    /// </exception>
    public static IDisposable EnterScope(IAudioService audio)
    {
        ArgumentNullException.ThrowIfNull(audio);
        return CURRENT_SCOPE.Enter(audio);
    }
}
