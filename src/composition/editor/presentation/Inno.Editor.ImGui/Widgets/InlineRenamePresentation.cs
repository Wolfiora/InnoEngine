using System;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Describes the view-owned geometry used to present an active inline rename action.
/// </summary>
public sealed class InlineRenamePresentation
{
    /// <summary>
    /// Creates inline rename presentation data.
    /// </summary>
    /// <param name="id">
    /// The stable ImGui identifier used by the input field.
    /// </param>
    /// <param name="width">
    /// The requested input width in logical pixels.
    /// </param>
    /// <param name="rowHeight">
    /// The height of the row area in which the field is centered.
    /// </param>
    /// <param name="bufferSize">
    /// The maximum UTF-8 buffer size accepted by the input.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name = "width"/> or <paramref name="rowHeight"/> is not finite and
    /// positive, or when <paramref name="bufferSize"/> is zero.
    /// </exception>
    public InlineRenamePresentation(
        string id,
        float width,
        float rowHeight,
        nuint bufferSize = 512
    ) {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("An inline rename identifier is required.", nameof(id));
        }
        if (!float.IsFinite(width) || width <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(width), width, "The inline rename width must be finite and positive.");
        }

        if (!float.IsFinite(rowHeight) || rowHeight <= 0f)
        {
            throw new ArgumentOutOfRangeException(
                nameof(rowHeight),
                rowHeight,
                "The inline rename row height must be finite and positive."
            );
        }

        if (bufferSize == 0)
        {
            throw new ArgumentOutOfRangeException(nameof(bufferSize), bufferSize, "The inline rename buffer size must be positive.");
        }

        this.id = id;
        this.width = width;
        this.rowHeight = rowHeight;
        this.bufferSize = bufferSize;
    }

    /// <summary>
    /// Gets the stable ImGui identifier used by the input field.
    /// </summary>
    public string id { get; }

    /// <summary>
    /// Gets the requested input width in logical pixels.
    /// </summary>
    public float width { get; }

    /// <summary>
    /// Gets the height of the row area in which the input field is centered.
    /// </summary>
    public float rowHeight { get; }

    /// <summary>
    /// Gets the maximum UTF-8 buffer size accepted by the input.
    /// </summary>
    public nuint bufferSize { get; }
}
