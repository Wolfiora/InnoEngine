using System;

namespace Inno.Platform;

/// <summary>
/// Describes an optional window capability for exclusive input-method and virtual-keyboard ownership.
/// </summary>
public interface IPlatformTextInputWindow : IPlatformWindow
{
    /// <summary>
    /// Attempts to acquire system text input without replacing another live owner.
    /// </summary>
    /// <returns>
    /// A caller-owned session, or null when the window is unavailable for input or another session already owns text input.
    /// </returns>
    /// <exception cref="ObjectDisposedException">
    /// The window has been disposed.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// The request runs outside the window thread or system text input cannot be started.
    /// </exception>
    IPlatformTextInputSession? TryBeginTextInput();
}
