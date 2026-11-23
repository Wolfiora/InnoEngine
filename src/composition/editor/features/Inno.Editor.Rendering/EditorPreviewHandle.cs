using System;

namespace Inno.Editor.Rendering;

/// <summary>
/// Identifies one preview texture owned by a specific rendering-device generation.
/// </summary>
public readonly record struct EditorPreviewHandle
{
    /// <summary>
    /// Creates one generation-scoped preview handle.
    /// </summary>
    /// <param name="value">
    /// Non-zero preview identity.
    /// </param>
    /// <param name="deviceGeneration">
    /// Non-zero rendering-device generation.
    /// </param>
    /// <param name="pixelWidth">
    /// Positive source width.
    /// </param>
    /// <param name="pixelHeight">
    /// Positive source height.
    /// </param>
    public EditorPreviewHandle(
        ulong value,
        uint deviceGeneration,
        int pixelWidth,
        int pixelHeight
    ) {
        if (value == 0 || deviceGeneration == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(value), "Preview identity and device generation must be non-zero.");
        }
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelHeight);
        this.value = value;
        this.deviceGeneration = deviceGeneration;
        this.pixelWidth = pixelWidth;
        this.pixelHeight = pixelHeight;
    }

    /// <summary>
    /// Gets the opaque preview identity.
    /// </summary>
    public ulong value { get; }

    /// <summary>
    /// Gets the rendering-device generation that owns this handle.
    /// </summary>
    public uint deviceGeneration { get; }

    /// <summary>
    /// Gets source width in pixels.
    /// </summary>
    public int pixelWidth { get; }

    /// <summary>
    /// Gets source height in pixels.
    /// </summary>
    public int pixelHeight { get; }

    /// <summary>
    /// Gets whether this handle identifies a usable generation-scoped preview.
    /// </summary>
    public bool isValid => value != 0 && deviceGeneration != 0 && pixelWidth > 0 && pixelHeight > 0;
}
