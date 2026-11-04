namespace Inno.Input;

/// <summary>
/// Provides optional system text-input ownership from an input acquisition backend.
/// </summary>
/// <remarks>
/// The backend retires all outstanding sessions before its own successful disposal. This acquisition capability
/// is not a script export and does not expose a platform window or native input-method context.
/// </remarks>
public interface IInputTextBackend
{
    /// <summary>
    /// Tries to acquire exclusive text input on the backend's current focused route.
    /// </summary>
    /// <returns>
    /// A caller-owned session, or null when unsupported, unfocused or already owned. Failure to acquire changes no owner.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// The call uses a thread other than the input owner thread.
    /// </exception>
    IInputTextSession? TryBeginTextInput();
}
