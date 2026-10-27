using System.Numerics;

namespace Inno.Editor.Graph.ImGui;

/// <summary>
/// Carries one frame's already-routed pointer input without depending on a native backend.
/// </summary>
public readonly struct GraphCanvasPointerInput
{
    /// <summary>
    /// Gets the screen-space pointer position.
    /// </summary>
    public Vector2 position { get; init; }

    /// <summary>
    /// Gets whether this canvas owns the pointer for starting an interaction.
    /// </summary>
    public bool isHovered { get; init; }

    /// <summary>
    /// Gets whether the primary pointer button was pressed this frame.
    /// </summary>
    public bool isPressed { get; init; }

    /// <summary>
    /// Gets whether the primary pointer button remains held.
    /// </summary>
    public bool isDown { get; init; }

    /// <summary>
    /// Gets whether the primary pointer button was released this frame.
    /// </summary>
    public bool isReleased { get; init; }

    /// <summary>
    /// Gets whether selection extension is requested.
    /// </summary>
    public bool shift { get; init; }

    /// <summary>
    /// Gets whether the current preview must be canceled without modifying document data.
    /// </summary>
    public bool cancel { get; init; }

    /// <summary>
    /// Gets whether selection is allowed while data-editing intents are prohibited.
    /// </summary>
    public bool readOnly { get; init; }
}
