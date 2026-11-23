using System.Numerics;

namespace Inno.Editor.ImGui.ImGuiWidget;

internal readonly struct TreeWidgetHighlightRect
{
    internal Vector2 min { get; init; }
    internal Vector2 max { get; init; }
}
