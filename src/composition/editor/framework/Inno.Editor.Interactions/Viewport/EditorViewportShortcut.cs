using Inno.Core.Input;

namespace Inno.Editor.Interactions;

/// <summary>
/// Contains one presentation-independent shortcut sample for a focused viewport tool.
/// </summary>
public readonly record struct EditorViewportShortcut
{
    /// <summary>
    /// Creates one viewport shortcut sample.
    /// </summary>
    /// <param name="key">
    /// Pressed key.
    /// </param>
    /// <param name="modifiers">
    /// Active keyboard modifiers.
    /// </param>
    /// <param name="repeat">
    /// Whether the platform generated an auto-repeat press.
    /// </param>
    public EditorViewportShortcut(
        KeyCode key,
        KeyModifier modifiers = KeyModifier.None,
        bool repeat = false
    ) {
        this.key = key;
        this.modifiers = modifiers;
        this.repeat = repeat;
    }

    /// <summary>
    /// Gets the pressed key.
    /// </summary>
    public KeyCode key { get; }

    /// <summary>
    /// Gets active keyboard modifiers.
    /// </summary>
    public KeyModifier modifiers { get; }

    /// <summary>
    /// Gets whether the shortcut is an auto-repeat press.
    /// </summary>
    public bool repeat { get; }
}
