using System.Numerics;
using Inno.Native.ImGui;
using RawImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Provides pointer-free controls within the active Editor UI session.
/// </summary>
public static partial class ImGui
{
    /// <summary>
    /// Submits an explicitly drawn control interaction rectangle.
    /// </summary>
    /// <param name="label">
    /// The visible label and optional stable ID suffix.
    /// </param>
    /// <param name="size">
    /// The explicit two-dimensional extent.
    /// </param>
    /// <param name="flags">
    /// The explicit control and interaction options.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool InvisibleButton(
        string label,
        Vector2 size,
        ImGuiButtonFlags flags = ImGuiButtonFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.InvisibleButton(label, size, (int)flags);
    }

    /// <summary>
    /// Draws a menu item with explicit availability and checked state.
    /// </summary>
    /// <param name="label">
    /// The visible label and optional stable ID suffix.
    /// </param>
    /// <param name="shortcut">
    /// A display label only; execution remains with the Action router.
    /// </param>
    /// <param name="selected">
    /// Whether the item is currently selected.
    /// </param>
    /// <param name="enabled">
    /// Whether the control can accept input.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool MenuItem(
        string label,
        string shortcut = "",
        bool selected = false,
        bool enabled = true
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.MenuItem(label, shortcut, selected, enabled);
    }

    /// <summary>
    /// Edits an integer within explicit inclusive bounds.
    /// </summary>
    /// <param name="label">
    /// The visible label and optional stable ID suffix.
    /// </param>
    /// <param name="value">
    /// The caller-owned value updated only after an accepted edit.
    /// </param>
    /// <param name="minimum">
    /// The inclusive lower bound or upper-left corner.
    /// </param>
    /// <param name="maximum">
    /// The inclusive upper bound or lower-right corner.
    /// </param>
    /// <param name="format">
    /// The numeric display format.
    /// </param>
    /// <param name="flags">
    /// The explicit control and interaction options.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool SliderInt(
        string label,
        ref int value,
        int minimum,
        int maximum,
        string format = "%d",
        ImGuiSliderFlags flags = ImGuiSliderFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.SliderInt(label, ref value, minimum, maximum, format, (int)flags);
    }

    /// <summary>
    /// Edits an integer by pointer drag with explicit speed and optional bounds.
    /// </summary>
    /// <param name="label">
    /// The visible label and optional stable ID suffix.
    /// </param>
    /// <param name="value">
    /// The caller-owned value updated only after an accepted edit.
    /// </param>
    /// <param name="speed">
    /// The pointer movement multiplier.
    /// </param>
    /// <param name="minimum">
    /// The inclusive lower bound or upper-left corner.
    /// </param>
    /// <param name="maximum">
    /// The inclusive upper bound or lower-right corner.
    /// </param>
    /// <param name="format">
    /// The numeric display format.
    /// </param>
    /// <param name="flags">
    /// The explicit control and interaction options.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool DragInt(
        string label,
        ref int value,
        float speed = 1f,
        int minimum = 0,
        int maximum = 0,
        string format = "%d",
        ImGuiSliderFlags flags = ImGuiSliderFlags.None
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.DragInt(label, ref value, speed, minimum, maximum, format, (int)flags);
    }
}
