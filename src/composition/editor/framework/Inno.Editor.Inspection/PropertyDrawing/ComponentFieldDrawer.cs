using System;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Inspection;

internal static unsafe class ComponentFieldDrawer
{
    internal static float GetWidth(int count)
    {
        float spacing = Gui.GetLayout().itemSpacing.X;
        return MathF.Max(1f, (Gui.GetContentRegionAvailable().X - spacing * (count - 1)) / count);
    }

    internal static bool Float(
        string path,
        string label,
        ref float value,
        float width
    ) => EditorUI.current.widgets.AxisDragFloat(path, label, ref value, width);

    internal static bool Int(
        string path,
        string label,
        ref int value,
        float width
    ) => EditorUI.current.widgets.AxisDragInt(path, label, ref value, width);

    internal static void Next() => Gui.SameLine(0f, -1f);
}
