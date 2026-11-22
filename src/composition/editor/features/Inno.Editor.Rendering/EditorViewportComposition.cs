using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Rendering;

namespace Inno.Editor.Rendering;

/// <summary>
/// Describes a complete ordered set of rendering-model layers targeting one Editor viewport.
/// </summary>
public sealed class EditorViewportComposition
{
    private readonly IReadOnlyList<EditorViewportLayer> m_layers;

    /// <summary>
    /// Creates an immutable viewport composition.
    /// </summary>
    /// <param name="viewportID">
    /// Stable panel viewport identity.
    /// </param>
    /// <param name="pixelWidth">
    /// Positive target width.
    /// </param>
    /// <param name="pixelHeight">
    /// Positive target height.
    /// </param>
    /// <param name="targetFormat">
    /// Shared presentation target format required by every layer.
    /// </param>
    /// <param name="layers">
    /// Ordered non-empty rendering-model layer collection.
    /// </param>
    public EditorViewportComposition(
        string viewportID,
        int pixelWidth,
        int pixelHeight,
        RenderTextureFormat targetFormat,
        IEnumerable<EditorViewportLayer> layers
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(viewportID);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelWidth);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(pixelHeight);
        ArgumentNullException.ThrowIfNull(layers);
        EditorViewportLayer[] materializedLayers = layers.ToArray();
        if (materializedLayers.Length == 0)
        {
            throw new ArgumentException("A viewport composition requires at least one model layer.", nameof(layers));
        }
        if (materializedLayers.Any(static layer => layer is null))
        {
            throw new ArgumentException("A viewport composition cannot contain null model layers.", nameof(layers));
        }
        if (materializedLayers.Select(static layer => layer.contributorID).Distinct(StringComparer.Ordinal).Count()
            != materializedLayers.Length
        ) {
            throw new ArgumentException("Viewport contributor identities must be unique.", nameof(layers));
        }
        m_layers = Array.AsReadOnly(
            materializedLayers
                .OrderBy(static layer => layer.order)
                .ThenBy(static layer => layer.contributorID, StringComparer.Ordinal)
                .ToArray()
        );
        this.viewportID = viewportID;
        this.pixelWidth = pixelWidth;
        this.pixelHeight = pixelHeight;
        this.targetFormat = targetFormat;
    }

    /// <summary>
    /// Gets the stable viewport identity.
    /// </summary>
    public string viewportID { get; }

    /// <summary>
    /// Gets the target width.
    /// </summary>
    public int pixelWidth { get; }

    /// <summary>
    /// Gets the target height.
    /// </summary>
    public int pixelHeight { get; }

    /// <summary>
    /// Gets the shared presentation target format.
    /// </summary>
    public RenderTextureFormat targetFormat { get; }

    /// <summary>
    /// Gets the ordered rendering-model layers.
    /// </summary>
    public IReadOnlyList<EditorViewportLayer> layers => m_layers;
}
