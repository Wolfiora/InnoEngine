using System.Numerics;

namespace Inno.Editor.ImGui.ImGuiWidget;

internal struct TreeWidgetNodeState
{
    internal string id;
    internal Vector2 cursor;
    internal float rowMaxY;
    internal bool hasNextSibling;
}
