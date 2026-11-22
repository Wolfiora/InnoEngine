using System.Numerics;

namespace Inno.Editor.ImGui.ImGuiWidget;

internal readonly struct TreeWidgetNodeState
{
    internal string id { get; init; }
    internal Vector2 cursor { get; init; }
    internal float rowMaxY { get; init; }
    internal bool hasNextSibling { get; init; }
}
