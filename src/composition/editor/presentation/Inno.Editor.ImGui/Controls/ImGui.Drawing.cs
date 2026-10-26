using System.Numerics;
using Inno.Native.ImGui;
using RawImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Provides frame-bounded drawing access and copied presentation values.
/// </summary>
public static partial class ImGui
{
    /// <summary>
    /// Gets the current style color including the active alpha.
    /// </summary>
    /// <param name="color">
    /// The packed RGBA color or semantic style slot.
    /// </param>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static uint GetStyleColor(ImGuiCol color)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetColorU32((int)color);
    }

    /// <summary>
    /// Derives a control identity from the current window and ID stack.
    /// </summary>
    /// <param name="label">
    /// The visible label and optional stable ID suffix.
    /// </param>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static uint GetControlId(string label)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetID(label);
    }

    /// <summary>
    /// Clips subsequent controls and drawing to a screen rectangle.
    /// </summary>
    /// <param name="minimum">
    /// The inclusive lower bound or upper-left corner.
    /// </param>
    /// <param name="maximum">
    /// The inclusive upper bound or lower-right corner.
    /// </param>
    /// <param name="intersect">
    /// Whether to intersect the enclosing clip.
    /// </param>
    public static void PushClipRect(
        Vector2 minimum,
        Vector2 maximum,
        bool intersect
    ) {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PushClipRect(minimum, maximum, intersect);
    }

    /// <summary>
    /// Restores the enclosing control clip after a matching push.
    /// </summary>
    public static void PopClipRect()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.PopClipRect();
    }
}
