using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Raised when a key is released.
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
public sealed class KeyReleasedEvent(
    uint windowID,
    KeyCode key,
    KeyModifier modifiers = KeyModifier.None
) : KeyEvent(windowID, key, modifiers);
