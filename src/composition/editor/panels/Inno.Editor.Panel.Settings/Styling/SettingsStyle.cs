using System.Numerics;
using Inno.Editor.ImGui;

namespace Inno.Editor.Panel.Settings;

internal readonly struct SettingsStyle
{
    private readonly float m_zoom;

    internal SettingsStyle(EditorStyleMetrics metrics) => m_zoom = metrics.zoom;

    internal static Vector4 field { get; } = new(0.36f, 0.36f, 0.38f, EditorPalette.opacityTrace);
    internal static Vector4 alternateField { get; } = new(0.52f, 0.52f, 0.56f, EditorPalette.opacityTrace);
    internal Vector2 fieldPadding => new Vector2(8f, 6f) * m_zoom;
}
