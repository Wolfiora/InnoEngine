using System.Numerics;
using Inno.Native.ImGui;
using RawImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;
/// <summary>
/// Reads copied input values from the active Editor UI session.
/// </summary>
public static partial class ImGui
{
    /// <summary>
    /// Gets the current pointer position in screen-space UI units.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static Vector2 GetMousePos()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetMousePos();
    }

    /// <summary>
    /// Checks a pointer-button press in the current frame.
    /// </summary>
    /// <param name="button">
    /// The pointer button to inspect.
    /// </param>
    /// <param name="repeat">
    /// Whether a held key or button may produce repeats.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsMouseClicked(
        ImGuiMouseButton button,
        bool repeat = false
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsMouseClicked((int)button, repeat);
    }

    /// <summary>
    /// Checks a pointer-button release in the current frame.
    /// </summary>
    /// <param name="button">
    /// The pointer button to inspect.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsMouseReleased(ImGuiMouseButton button)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsMouseReleased((int)button);
    }

    /// <summary>
    /// Checks the backend-classified double-click event.
    /// </summary>
    /// <param name="button">
    /// The pointer button to inspect.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsMouseDoubleClicked(ImGuiMouseButton button)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsMouseDoubleClicked((int)button);
    }

    /// <summary>
    /// Checks whether a presentation key remains held.
    /// </summary>
    /// <param name="key">
    /// The presentation key to inspect.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsKeyDown(ImGuiKey key)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsKeyDown((Inno.Native.ImGui.ImGuiKey)key);
    }

    /// <summary>
    /// Checks a presentation-key press with explicit repeat policy.
    /// </summary>
    /// <param name="key">
    /// The presentation key to inspect.
    /// </param>
    /// <param name="repeat">
    /// Whether a held key or button may produce repeats.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsKeyPressed(
        ImGuiKey key,
        bool repeat = true
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsKeyPressed((Inno.Native.ImGui.ImGuiKey)key, repeat);
    }

    /// <summary>
    /// Resets the consumed pointer-drag distance.
    /// </summary>
    /// <param name="button">
    /// The pointer button to inspect.
    /// </param>
    public static void ResetMouseDragDelta(ImGuiMouseButton button)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.ResetMouseDragDelta((int)button);
    }

    /// <summary>
    /// Requests the semantic cursor for the hovered control.
    /// </summary>
    /// <param name="cursor">
    /// The semantic pointer shape.
    /// </param>
    public static void SetMouseCursor(ImGuiMouseCursor cursor)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SetMouseCursor((int)cursor);
    }

    /// <summary>
    /// Checks pointer containment in a screen rectangle with optional window clipping.
    /// </summary>
    /// <param name="minimum">
    /// The inclusive lower bound or upper-left corner.
    /// </param>
    /// <param name="maximum">
    /// The inclusive upper bound or lower-right corner.
    /// </param>
    /// <param name="clip">
    /// Whether the current clipping rectangle participates.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsMouseHoveringRect(
        Vector2 minimum,
        Vector2 maximum,
        bool clip = true
    ) {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsMouseHoveringRect(minimum, maximum, clip);
    }

    /// <summary>
    /// Checks window hover under the declared interaction policy.
    /// </summary>
    /// <param name="flags">
    /// The explicit control and interaction options.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsWindowHovered(ImGuiHoveredFlags flags = ImGuiHoveredFlags.None)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsWindowHovered((int)flags);
    }

    /// <summary>
    /// Checks keyboard focus within the declared window scope.
    /// </summary>
    /// <param name="flags">
    /// The explicit control and interaction options.
    /// </param>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsWindowFocused(ImGuiFocusedFlags flags = ImGuiFocusedFlags.None)
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsWindowFocused((int)flags);
    }

    /// <summary>
    /// Checks whether a submitted control currently owns hover.
    /// </summary>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsAnyItemHovered()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsAnyItemHovered();
    }

    /// <summary>
    /// Checks whether a submitted control owns an active interaction.
    /// </summary>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsAnyItemActive()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsAnyItemActive();
    }

    /// <summary>
    /// Checks the first frame of the last control interaction.
    /// </summary>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsItemActivated()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsItemActivated();
    }

    /// <summary>
    /// Checks the completion frame of an edited control.
    /// </summary>
    /// <returns>
    /// True when the documented control event or condition is present; otherwise false.
    /// </returns>
    public static bool IsItemDeactivatedAfterEdit()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.IsItemDeactivatedAfterEdit();
    }

    /// <summary>
    /// Requests focus for a relative control position.
    /// </summary>
    /// <param name="offset">
    /// The relative focus offset from the next submitted control.
    /// </param>
    public static void SetKeyboardFocusHere(int offset = 0)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SetKeyboardFocusHere(offset);
    }

    /// <summary>
    /// Chooses the last item as the default selection in an appearing popup.
    /// </summary>
    public static void SetItemDefaultFocus()
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SetItemDefaultFocus();
    }

    /// <summary>
    /// Copies literal text through the host clipboard adapter.
    /// </summary>
    /// <param name="text">
    /// Literal text; no formatting interpretation is applied.
    /// </param>
    public static void SetClipboardText(string text)
    {
        EditorUiSession.current.EnsureDrawing();
        RawImGui.SetClipboardText(text);
    }

    /// <summary>
    /// Gets the current native frame identity for transient diagnostics.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static int GetFrameCount()
    {
        EditorUiSession.current.EnsureDrawing();
        return RawImGui.GetFrameCount();
    }
}
