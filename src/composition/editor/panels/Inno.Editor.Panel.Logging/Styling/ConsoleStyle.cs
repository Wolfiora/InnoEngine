using System.Numerics;
using Inno.Editor.ImGui;

namespace Inno.Editor.Panel.Logging;

internal readonly struct ConsoleStyle
{
    private readonly float m_zoom;

    internal ConsoleStyle(EditorStyleMetrics metrics) => m_zoom = metrics.zoom;

    /// <summary>
    /// Gets debug log color.
    /// </summary>
    internal static Vector4 debug { get; } = new(0.80f, 0.90f, 0.85f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets informational log color.
    /// </summary>
    internal static Vector4 info { get; } = new(0.20f, 1f, 0.20f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets warning log color.
    /// </summary>
    internal static Vector4 warning { get; } = new(1f, 1f, 0.20f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets error log color.
    /// </summary>
    internal static Vector4 error { get; } = new(1f, 0.20f, 0.20f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets fatal log color.
    /// </summary>
    internal static Vector4 fatal { get; } = new(1f, 0.20f, 1f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets collapsed log card background.
    /// </summary>
    internal static Vector4 collapsedCard { get; } = new(0.61960787f, 0.5764706f, 0.76862746f, EditorPalette.opacityMuted);

    /// <summary>
    /// Gets collapsed log card border.
    /// </summary>
    internal static Vector4 collapsedBorder { get; } = new(0.32f, 0.34f, 0.37f, EditorPalette.opacityMedium);

    /// <summary>
    /// Gets the base expanded log card background.
    /// </summary>
    internal static Vector4 expandedBase { get; } = new(0.10f, 0.10f, 0.10f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets the base expanded log card border.
    /// </summary>
    internal static Vector4 expandedBorderBase { get; } = new(0.24f, 0.24f, 0.24f, EditorPalette.opacityOpaque);

    /// <summary>
    /// Gets log header button background.
    /// </summary>
    internal static Vector4 toggle { get; } = new(0.61960787f, 0.5764706f, 0.76862746f, EditorPalette.opacityNone);

    /// <summary>
    /// Gets hovered log header button background.
    /// </summary>
    internal static Vector4 toggleHovered { get; } = new(0.7372549f, 0.69411767f, 0.8862745f, EditorPalette.opacityMedium);

    /// <summary>
    /// Gets active log header button background.
    /// </summary>
    internal static Vector4 toggleActive { get; } = new(0.8156863f, 0.77254903f, 0.9647059f, EditorPalette.opacityMedium);

    /// <summary>
    /// Gets the expanded log card background derived from a severity color.
    /// </summary>
    /// <param name="severityColor">
    /// The palette color representing the log severity.
    /// </param>
    /// <returns>
    /// The standard expanded-card base blended with the severity color.
    /// </returns>
    internal static Vector4 GetExpandedCard(Vector4 severityColor) => EditorPalette.Lerp(expandedBase, severityColor, 0.12f);

    /// <summary>
    /// Gets the expanded log card border derived from a severity color.
    /// </summary>
    /// <param name="severityColor">
    /// The palette color representing the log severity.
    /// </param>
    /// <returns>
    /// The standard expanded-border base blended with the severity color.
    /// </returns>
    internal static Vector4 GetExpandedBorder(Vector4 severityColor) => EditorPalette.Lerp(expandedBorderBase, severityColor, 0.20f);

    /// <summary>
    /// Gets the separator color used inside an expanded log card.
    /// </summary>
    /// <param name="cardColor">
    /// The resolved background color of the expanded card.
    /// </param>
    /// <returns>
    /// The card color blended toward the shared text color.
    /// </returns>
    internal static Vector4 GetSeparator(Vector4 cardColor) => EditorPalette.Lerp(cardColor, EditorPalette.text, 0.12f);

    /// <summary>
    /// Gets the distance from the bottom treated as auto-scroll.
    /// </summary>
    internal float autoScrollTolerance => m_zoom;

    /// <summary>
    /// Gets log disclosure padding.
    /// </summary>
    internal Vector2 disclosurePadding => new Vector2(6f, 1f) * m_zoom;
}
