using System;
using Inno.Adapter.Presentation;

namespace Inno.Editor.Rendering;

/// <summary>
/// Identifies a renderer-owned editor viewport image through an opaque presentation token.
/// </summary>
public readonly record struct EditorViewportOutput
{
    /// <summary>
    /// Creates an editor viewport output snapshot.
    /// </summary>
    /// <param name="viewportID">
    /// Stable viewport identity.
    /// </param>
    /// <param name="texture">
    /// Opaque presentation token, or an invalid token while warming up.
    /// </param>
    /// <param name="pixelWidth">
    /// Current target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Current target height.
    /// </param>
    public EditorViewportOutput(
        string viewportID,
        PresentationTextureHandle texture,
        int pixelWidth,
        int pixelHeight
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelHeight);
        this.viewportID = viewportID;
        this.texture = texture;
        this.pixelWidth = pixelWidth;
        this.pixelHeight = pixelHeight;
    }

    /// <summary>
    /// Gets the stable viewport identity.
    /// </summary>
    public string viewportID { get; }

    /// <summary>
    /// Gets the opaque presentation token.
    /// </summary>
    public PresentationTextureHandle texture { get; }

    /// <summary>
    /// Gets the current target width.
    /// </summary>
    public int pixelWidth { get; }

    /// <summary>
    /// Gets the current target height.
    /// </summary>
    public int pixelHeight { get; }

    /// <summary>
    /// Gets whether a completed target can be drawn.
    /// </summary>
    public bool isReady => texture.isValid;
}
