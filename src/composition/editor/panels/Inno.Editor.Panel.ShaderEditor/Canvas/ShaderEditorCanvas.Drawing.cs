using Inno.Native.ImGui;
using ImGuiApi = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Panel.ShaderEditor;

// Carrier addresses remain stack-owned for the duration of each synchronous native call.
internal sealed unsafe partial class ShaderEditorCanvas
{
    private static void DrawNativeFilledRectangle(
        ImDrawList* drawList,
        ImVec2C minimum,
        ImVec2C maximum,
        uint color,
        float rounding,
        int flags
    ) {
        ImGuiApi.Raw.AddRectFilled(
            drawList,
            &minimum,
            &maximum,
            color,
            rounding,
            flags);
    }

    private static void DrawNativeText(
        ImDrawList* drawList,
        ImVec2C position,
        uint color,
        sbyte* textBegin,
        sbyte* textEnd
    ) {
        ImGuiApi.Raw.AddText(
            drawList,
            &position,
            color,
            textBegin,
            textEnd);
    }

    private static void DrawNativeText(
        ImDrawList* drawList,
        ImFont* font,
        float fontSize,
        ImVec2C position,
        uint color,
        sbyte* textBegin,
        sbyte* textEnd,
        float wrapWidth,
        ImVec4C* clipRectangle
    ) {
        ImGuiApi.Raw.AddText(
            drawList,
            font,
            fontSize,
            &position,
            color,
            textBegin,
            textEnd,
            wrapWidth,
            clipRectangle);
    }

    private static void DrawNativeFilledCircle(
        ImDrawList* drawList,
        ImVec2C center,
        float radius,
        uint color,
        int segments
    ) {
        ImGuiApi.Raw.AddCircleFilled(
            drawList,
            &center,
            radius,
            color,
            segments);
    }

    private static void PushNativeClipRectangle(
        ImDrawList* drawList,
        ImVec2C minimum,
        ImVec2C maximum,
        byte intersect
    ) {
        ImGuiApi.Raw.PushClipRect(
            drawList,
            &minimum,
            &maximum,
            intersect);
    }

    private static void DrawNativeRectangle(
        ImDrawList* drawList,
        ImVec2C minimum,
        ImVec2C maximum,
        uint color,
        float rounding,
        int flags,
        float thickness
    ) {
        ImGuiApi.Raw.AddRect(
            drawList,
            &minimum,
            &maximum,
            color,
            rounding,
            flags,
            thickness);
    }

    private static void DrawNativeCurve(
        ImDrawList* drawList,
        ImVec2C p1,
        ImVec2C p2,
        ImVec2C p3,
        ImVec2C p4,
        uint color,
        float thickness,
        int segments
    ) {
        ImGuiApi.Raw.AddBezierCubic(
            drawList,
            &p1,
            &p2,
            &p3,
            &p4,
            color,
            thickness,
            segments);
    }

    private static void DrawNativeLine(
        ImDrawList* drawList,
        ImVec2C p1,
        ImVec2C p2,
        uint color,
        float thickness
    ) {
        ImGuiApi.Raw.AddLine(
            drawList,
            &p1,
            &p2,
            color,
            thickness);
    }
}
