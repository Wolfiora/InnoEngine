using System;
using System.Collections.Generic;
using Inno.Core.Identity;
using Inno.Core.Mathematics;

namespace Inno.Editor.Rendering;

/// <summary>
/// Holds the transient gizmo primitives for one Scene viewport frame.
/// </summary>
public sealed class EditorGizmoFrame : IEditorGizmoSink
{
    private readonly List<EditorGizmoIcon> m_icons = [];
    private readonly List<EditorGizmoLine> m_lines = [];

    /// <summary>
    /// Gets the ordered selectable icons.
    /// </summary>
    public IReadOnlyList<EditorGizmoIcon> icons => m_icons;

    /// <summary>
    /// Gets the ordered non-interactive lines.
    /// </summary>
    public IReadOnlyList<EditorGizmoLine> lines => m_lines;

    /// <summary>
    /// Appends a selectable scene icon to the current gizmo frame.
    /// </summary>
    /// <param name="owner">
    /// The object that owns the resulting lifetime and state.
    /// </param>
    /// <param name="position">
    /// The position consumed by icon; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="iconID">
    /// The stable icon identity resolved by the Scene View.
    /// </param>
    public void Icon(
        Identity owner,
        Vector3 position,
        string iconID
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(iconID);
        m_icons.Add(new EditorGizmoIcon(owner, position, iconID));
    }

    /// <summary>
    /// Appends a world space line to the current gizmo frame.
    /// </summary>
    /// <param name="start">
    /// The start consumed by line; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    /// <param name="end">
    /// The end consumed by line; ownership remains with the caller unless explicitly stated otherwise.
    /// </param>
    public void Line(
        Vector3 start,
        Vector3 end
    ) => m_lines.Add(new EditorGizmoLine(start, end));
}
