using System.Numerics;
using Inno.Editor.ImGui;

namespace Inno.Editor.Panel.Hierarchy;

internal readonly struct HierarchyStyle
{
    private readonly float m_zoom;

    internal HierarchyStyle(EditorStyleMetrics metrics) => m_zoom = metrics.zoom;

    /// <summary>
    /// Gets the text color of inactive hierarchy entries.
    /// </summary>
    internal static Vector4 inactiveText { get; } = new(0.52f, 0.52f, 0.54f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets hierarchy item spacing.
    /// </summary>
    internal Vector2 itemSpacing => new Vector2(3f, 2f) * m_zoom;

    /// <summary>
    /// Gets the minimum inline hierarchy rename width.
    /// </summary>
    internal float renameMinimumWidth => 48f * m_zoom;

    /// <summary>
    /// Gets the hierarchy rename trailing-control gap.
    /// </summary>
    internal float renameTrailingGap => 8f * m_zoom;

    /// <summary>
    /// Gets the minimum height of the hierarchy blank drop area.
    /// </summary>
    internal float blankMinimumHeight => 24f * m_zoom;
}
