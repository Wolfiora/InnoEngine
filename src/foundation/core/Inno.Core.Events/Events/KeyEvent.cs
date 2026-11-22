using Inno.Core.Input;

namespace Inno.Core.Events;

/// <summary>
/// Base class for keyboard events.
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
public abstract class KeyEvent(
    uint windowID,
    KeyCode key,
    KeyModifier modifiers = KeyModifier.None
) : Event
{
    /// <summary>
    /// Gets the source window id for this event.
    /// </summary>
    public uint windowID { get; } = windowID;

    /// <summary>
    /// Gets the key code for this keyboard event.
    /// </summary>
    public KeyCode key { get; } = key;

    /// <summary>
    /// Gets active key modifiers for this keyboard event.
    /// </summary>
    public KeyModifier modifiers { get; } = modifiers;
}
