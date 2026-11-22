using System;
using System.Numerics;
using BGCS.Runtime;
using Inno.Native.ImGui;

using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui;

/// <summary>
/// Copies presentation values and creates frame-bounded drawing borrows.
/// </summary>
public static unsafe partial class ImGui
{
    /// <summary>
    /// Copies the current UI layout metrics.
    /// </summary>
    /// <returns>
    /// Value metrics valid independently from the native style pointer.
    /// </returns>
    public static EditorUILayout GetLayout()
    {
        EditorUISession.current.EnsureDrawing();
        return new EditorUILayout(NativeImGui.Raw.GetStyle());
    }

    /// <summary>
    /// Copies the input facts for this frame.
    /// </summary>
    /// <returns>
    /// A neutral value snapshot; no pointer or callback is retained.
    /// </returns>
    public static EditorUIInput GetInput()
    {
        EditorUISession.current.EnsureDrawing();
        return new EditorUIInput(NativeImGui.Raw.GetIO());
    }

    /// <summary>
    /// Copies an unmodulated style color for compound presentation.
    /// </summary>
    /// <param name="color">
    /// The semantic style color slot.
    /// </param>
    /// <returns>
    /// The copied linear vector of UI color components.
    /// </returns>
    public static Vector4 GetStyleColorValue(ImGuiCol color)
    {
        EditorUISession.current.EnsureDrawing();
        return FromNative(NativeImGui.Raw.GetStyle()->Colors[(int)color]);
    }

    /// <summary>
    /// Gets the logical viewport identity and copied position for the current window.
    /// </summary>
    /// <returns>
    /// The viewport ID and its screen-space origin.
    /// </returns>
    public static (uint id, Vector2 position) GetViewport()
    {
        EditorUISession.current.EnsureDrawing();
        ImGuiViewport* viewport = NativeImGui.Raw.GetWindowViewport();
        return (viewport->ID, FromNative(viewport->Pos));
    }

    /// <summary>
    /// Gets the table and current row bounds needed by row overlays.
    /// </summary>
    /// <returns>
    /// The table's horizontal bounds and current row's vertical bounds.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// No valid drawing frame or active table exists.
    /// </exception>
    public static (float minimumX, float maximumX, float rowMinimum, float rowMaximum) GetTableBounds()
    {
        _ = EditorUISession.current.RequireDrawingFrame();
        ImGuiTable* table = NativeImGui.Raw.GetCurrentTable();
        if (table is null)
        {
            throw new InvalidOperationException("A table row layout requires an active table.");
        }
        return (table->OuterRect.Min.X, table->OuterRect.Max.X, table->RowPosY1, table->RowPosY2);
    }

    /// <summary>
    /// Gets copied inner and work bounds for the current window.
    /// </summary>
    /// <returns>
    /// Screen-space horizontal bounds and work maximum.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// No valid drawing session or native frame is active.
    /// </exception>
    public static (float minimumX, float maximumX, Vector2 workMaximum, Vector2 padding) GetWindowBounds()
    {
        _ = EditorUISession.current.RequireDrawingFrame();
        ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
        return (window->InnerRect.Min.X, window->InnerRect.Max.X, FromNative(window->WorkRect.Max), FromNative(window->WindowPadding));
    }

    /// <summary>
    /// Copies the current window's effective clip after parent and viewport clipping.
    /// </summary>
    /// <returns>
    /// Screen-space minimum and maximum values for bounding frame-local submission work.
    /// </returns>
    /// <exception cref="System.InvalidOperationException">
    /// No valid drawing session or native frame is active.
    /// </exception>
    public static (Vector2 minimum, Vector2 maximum) GetClipBounds()
    {
        _ = EditorUISession.current.RequireDrawingFrame();
        ImRect clip = NativeImGui.Raw.GetCurrentWindow()->ClipRect;
        return (FromNative(clip.Min), FromNative(clip.Max));
    }

    /// <summary>
    /// Borrows drawing commands for the current window and frame without exposing native memory.
    /// </summary>
    /// <returns>
    /// A value that rejects use after a window, frame, context or session change.
    /// </returns>
    public static EditorDrawList GetDrawList() => new(EditorUISession.current);

    /// <summary>
    /// Checks the active payload's logical protocol without exposing its native buffer.
    /// </summary>
    /// <param name="protocol">
    /// The exact drag protocol name.
    /// </param>
    /// <returns>
    /// True when a drag of that protocol is currently active.
    /// </returns>
    public static bool HasDragPayload(string protocol)
    {
        EditorUISession.current.EnsureDrawing();
        ImGuiPayload* payload = NativeImGui.Raw.GetDragDropPayload();
        if (payload is null)
        {
            return false;
        }
        using NativeStringLease text = Utf8Codec.EncodeNullTerminated(protocol, allowsNull: false);
        return NativeImGui.Raw.IsDataType(payload, (sbyte*)text.pointer) != 0;
    }

    /// <summary>
    /// Measures literal text at a relative font scale.
    /// </summary>
    /// <param name="text">
    /// The literal text to measure.
    /// </param>
    /// <param name="scale">
    /// Scale relative to the current font.
    /// </param>
    /// <returns>
    /// The copied width and height in UI pixels.
    /// </returns>
    public static Vector2 MeasureText(
        string text,
        float scale
    ) {
        EditorUISession.current.EnsureDrawing();
        return NativeImGui.CalcTextSize(text) * scale;
    }
}
