using System.Numerics;
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
    public static EditorUiLayout GetLayout()
    {
        EditorUiSession.current.EnsureDrawing();
        return new EditorUiLayout(NativeImGui.Raw.GetStyle());
    }

    /// <summary>
    /// Copies the input facts for this frame.
    /// </summary>
    /// <returns>
    /// A neutral value snapshot; no pointer or callback is retained.
    /// </returns>
    public static EditorUiInput GetInput()
    {
        EditorUiSession.current.EnsureDrawing();
        return new EditorUiInput(NativeImGui.Raw.GetIO());
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
        EditorUiSession.current.EnsureDrawing();
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
        EditorUiSession.current.EnsureDrawing();
        var viewport = NativeImGui.Raw.GetWindowViewport();
        return (viewport->ID, FromNative(viewport->Pos));
    }

    /// <summary>
    /// Gets the table and current row bounds needed by row overlays.
    /// </summary>
    /// <returns>
    /// The table's horizontal bounds and current row's vertical bounds.
    /// </returns>
    public static (float minimumX, float maximumX, float rowMinimum, float rowMaximum) GetTableBounds()
    {
        EditorUiSession.current.EnsureDrawing();
        var table = NativeImGui.Raw.GetCurrentTable();
        if (table is null)
            throw new System.InvalidOperationException("A table row layout requires an active table.");
        return (table->OuterRect.Min.X, table->OuterRect.Max.X, table->RowPosY1, table->RowPosY2);
    }

    /// <summary>
    /// Gets copied inner and work bounds for the current window.
    /// </summary>
    /// <returns>
    /// Screen-space horizontal bounds and work maximum.
    /// </returns>
    public static (float minimumX, float maximumX, Vector2 workMaximum, Vector2 padding) GetWindowBounds()
    {
        EditorUiSession.current.EnsureDrawing();
        var window = NativeImGui.Raw.GetCurrentWindow();
        return (window->InnerRect.Min.X, window->InnerRect.Max.X, FromNative(window->WorkRect.Max), FromNative(window->WindowPadding));
    }

    /// <summary>
    /// Borrows drawing commands for the current window and frame without exposing native memory.
    /// </summary>
    /// <returns>
    /// A value that rejects use after a window, frame, context or session change.
    /// </returns>
    public static EditorDrawList GetDrawList() => new(EditorUiSession.current);
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
        EditorUiSession.current.EnsureDrawing();
        var payload = NativeImGui.Raw.GetDragDropPayload();
        if (payload is null)
            return false;
        using var text = BGCS.Runtime.Utf8Codec.EncodeNullTerminated(protocol, allowsNull: false);
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
        EditorUiSession.current.EnsureDrawing();
        return NativeImGui.CalcTextSize(text) * scale;
    }
}
