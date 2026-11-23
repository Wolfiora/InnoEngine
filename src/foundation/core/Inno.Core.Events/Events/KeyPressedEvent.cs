using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Raised when a key is pressed.
/// </summary>
/// <param name="windowID">
/// The window id used to initialize this instance.
/// </param>
/// <param name="key">
/// The key used to initialize this instance.
/// </param>
/// <param name="modifiers">
/// The modifiers used to initialize this instance.
/// </param>
/// <param name="repeat">
/// The repeat used to initialize this instance.
/// </param>
public sealed class KeyPressedEvent(
    uint windowID,
    KeyCode key,
    KeyModifier modifiers = KeyModifier.None,
    bool repeat = false
) : KeyEvent(windowID, key, modifiers)
{
    /// <summary>
    /// Gets whether this key press is an auto-repeat event.
    /// </summary>
    public bool repeat { get; } = repeat;
}
