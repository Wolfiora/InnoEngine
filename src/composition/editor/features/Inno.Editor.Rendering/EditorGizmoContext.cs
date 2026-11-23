using System;
using Inno.Core.Identity;
using Inno.References;

namespace Inno.Editor.Rendering;

/// <summary>
/// Provides scene content and selection for transient Editor gizmos.
/// </summary>
public sealed class EditorGizmoContext
{
    /// <summary>
    /// Creates a frame-scoped gizmo request.
    /// </summary>
    /// <param name="content">
    /// The scenes presented by the Scene viewport.
    /// </param>
    /// <param name="selected">
    /// The selected live object, when available.
    /// </param>
    /// <param name="pixelWidth">
    /// The Scene viewport width.
    /// </param>
    /// <param name="pixelHeight">
    /// The Scene viewport height.
    /// </param>
    public EditorGizmoContext(
        ContentReadScope content,
        RuntimeIdentity? selected,
        int pixelWidth,
        int pixelHeight
    ) {
        ArgumentNullException.ThrowIfNull(content);
        this.content = content;
        this.selected = selected;
        this.pixelWidth = pixelWidth;
        this.pixelHeight = pixelHeight;
    }

    /// <summary>
    /// Gets the scene roots visible in this Editor viewport.
    /// </summary>
    public ContentReadScope content { get; }

    /// <summary>
    /// Gets the transient selected identity.
    /// </summary>
    public RuntimeIdentity? selected { get; }

    /// <summary>
    /// Gets the viewport width in physical pixels.
    /// </summary>
    public int pixelWidth { get; }

    /// <summary>
    /// Gets the viewport height in physical pixels.
    /// </summary>
    public int pixelHeight { get; }
}
