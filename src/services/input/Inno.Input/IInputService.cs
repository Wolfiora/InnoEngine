namespace Inno.Input;

/// <summary>
/// Exposes the current immutable input snapshot to runtime systems and scripts.
/// </summary>
public interface IInputService
{
    /// <summary>
    /// Gets the snapshot captured at the current frame boundary.
    /// </summary>
    InputSnapshot snapshot { get; }

    /// <summary>
    /// Tries to acquire exclusive system text input for this runtime's focused input route.
    /// </summary>
    /// <returns>
    /// A caller-owned session, or null when unsupported, unfocused or busy. The runtime retires outstanding sessions on stop.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// The runtime is not attached or the call uses a thread other than the input owner thread.
    /// </exception>
    /// <exception cref="System.ObjectDisposedException">
    /// Runtime retirement has started.
    /// </exception>
    IInputTextSession? TryBeginTextInput();
}
