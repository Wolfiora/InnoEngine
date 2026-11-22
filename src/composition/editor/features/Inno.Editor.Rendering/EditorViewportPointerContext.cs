using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Supplies normalized pointer interaction over one rendered viewport.
/// </summary>
public sealed class EditorViewportPointerContext
{
    internal EditorViewportPointerContext(
        EditorViewportContext viewport,
        float x,
        float y,
        int button
    ) {
        this.viewport = viewport;
        this.x = Math.Clamp(x, 0f, 1f);
        this.y = Math.Clamp(y, 0f, 1f);
        this.button = button;
    }

    /// <summary>
    /// Gets the owning frame-only viewport context.
    /// </summary>
    public EditorViewportContext viewport { get; }

    /// <summary>
    /// Gets normalized horizontal pointer position.
    /// </summary>
    public float x { get; }

    /// <summary>
    /// Gets normalized vertical pointer position.
    /// </summary>
    public float y { get; }

    /// <summary>
    /// Gets the platform-independent pointer button index.
    /// </summary>
    public int button { get; }
}
