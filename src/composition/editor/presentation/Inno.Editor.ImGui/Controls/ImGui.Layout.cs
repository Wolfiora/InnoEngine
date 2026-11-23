using System.Numerics;
using Inno.Native.ImGui;

using RawImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;

/// <summary>
/// Provides scoped layout operations without exposing native context storage.
/// </summary>
public static partial class ImGui
{
    /// <summary>
    /// Places the next control at a screen-space location.
    /// </summary>
    /// <param name="position">
    /// The requested coordinate in UI units.
    /// </param>
    public static void SetCursorScreenPosition(Vector2 position)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetCursorScreenPos(position);
    }

    /// <summary>
    /// Changes only the local horizontal layout cursor.
    /// </summary>
    /// <param name="position">
    /// The requested coordinate in UI units.
    /// </param>
    public static void SetCursorPosX(float position)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetCursorPosX(position);
    }

    /// <summary>
    /// Changes only the local vertical layout cursor.
    /// </summary>
    /// <param name="position">
    /// The requested coordinate in UI units.
    /// </param>
    public static void SetCursorPosY(float position)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetCursorPosY(position);
    }

    /// <summary>
    /// Gets the local horizontal layout cursor.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetCursorPosX()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetCursorPosX();
    }

    /// <summary>
    /// Gets the local vertical layout cursor.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetCursorPosY()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetCursorPosY();
    }

    /// <summary>
    /// Gets the current window screen-space origin.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static Vector2 GetWindowPos()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetWindowPos();
    }

    /// <summary>
    /// Gets the current window size in UI units.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static Vector2 GetWindowSize()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetWindowSize();
    }

    /// <summary>
    /// Gets the active font size in UI units.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetFontSize()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetFontSize();
    }

    /// <summary>
    /// Gets the standard control height including frame padding.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetFrameHeight()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetFrameHeight();
    }

    /// <summary>
    /// Gets the active font line height without spacing.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetTextLineHeight()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetTextLineHeight();
    }

    /// <summary>
    /// Gets the standard control height and following vertical spacing.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetFrameHeightWithSpacing()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetFrameHeightWithSpacing();
    }

    /// <summary>
    /// Gets the native viewport scale for the current window.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetWindowDpiScale()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetWindowDpiScale();
    }

    /// <summary>
    /// Aligns the next text baseline with framed controls.
    /// </summary>
    public static void AlignTextToFramePadding()
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.AlignTextToFramePadding();
    }

    /// <summary>
    /// Allows explicitly layered controls to share the next item bounds.
    /// </summary>
    public static void SetNextItemAllowOverlap()
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetNextItemAllowOverlap();
    }

    /// <summary>
    /// Requests a size for the next window under an explicit layout condition.
    /// </summary>
    /// <param name="size">
    /// The explicit two-dimensional extent.
    /// </param>
    /// <param name="condition">
    /// The condition controlling when the request is applied.
    /// </param>
    public static void SetNextWindowSize(
        Vector2 size,
        ImGuiCond condition = ImGuiCond.None
    ) {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetNextWindowSize(size, (int)condition);
    }

    /// <summary>
    /// Declares virtual content size for the next scrollable window.
    /// </summary>
    /// <param name="size">
    /// The explicit two-dimensional extent.
    /// </param>
    public static void SetNextWindowContentSize(Vector2 size)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetNextWindowContentSize(size);
    }

    /// <summary>
    /// Indents subsequent controls by an explicit amount or the standard step.
    /// </summary>
    /// <param name="amount">
    /// The requested layout distance.
    /// </param>
    public static void Indent(float amount = 0f)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.Indent(amount);
    }

    /// <summary>
    /// Restores a matching indentation step.
    /// </summary>
    /// <param name="amount">
    /// The requested layout distance.
    /// </param>
    public static void Unindent(float amount = 0f)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.Unindent(amount);
    }

    /// <summary>
    /// Restricts subsequent text wrapping to a local horizontal boundary.
    /// </summary>
    /// <param name="position">
    /// The requested coordinate in UI units.
    /// </param>
    public static void PushTextWrapPos(float position = 0f)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.PushTextWrapPos(position);
    }

    /// <summary>
    /// Restores the enclosing text-wrap boundary.
    /// </summary>
    public static void PopTextWrapPos()
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.PopTextWrapPos();
    }

    /// <summary>
    /// Gets the current vertical scroll position.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetScrollY()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetScrollY();
    }

    /// <summary>
    /// Gets the current vertical scroll range.
    /// </summary>
    /// <returns>
    /// A copied value from the active presentation context.
    /// </returns>
    public static float GetScrollMaxY()
    {
        EditorUISession.current.EnsureDrawing();
        return RawImGui.GetScrollMaxY();
    }

    /// <summary>
    /// Scrolls the most recently submitted control into the requested relative position.
    /// </summary>
    /// <param name="ratio">
    /// The relative alignment from zero through one.
    /// </param>
    public static void SetScrollHereY(float ratio = 0.5f)
    {
        EditorUISession.current.EnsureDrawing();
        RawImGui.SetScrollHereY(ratio);
    }
}
