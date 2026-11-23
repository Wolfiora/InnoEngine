using System;
using System.Numerics;
using Inno.Native.ImGui;

using NativeImGui = Inno.Native.ImGui.ImGui;
using NativeKey = Inno.Native.ImGui.ImGuiKey;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Provides reusable editor controls and rendering helpers built on the native ImGui API.
/// </summary>
public sealed unsafe partial class ImGuiWidget
{
    private const float C_INLINE_RENAME_FOCUS_OFFSET = 1f;

    /// <summary>
    /// Draws a compact single-line rename editor inside an existing row.
    /// </summary>
    /// <param name="id">
    /// The stable control identifier.
    /// </param>
    /// <param name="text">
    /// The mutable text buffer.
    /// </param>
    /// <param name="requestFocus">
    /// Whether keyboard focus and complete-value selection should be requested. The value remains
    /// <see langword="true"/> until the input becomes active and its current contents are selected.
    /// </param>
    /// <param name="rowHeight">
    /// The height of the row area in which the field is centered.
    /// </param>
    /// <param name="capacity">
    /// The maximum UTF-8 buffer capacity.
    /// </param>
    /// <param name="width">
    /// The control width, or a negative value to fill the available space.
    /// </param>
    /// <returns>
    /// The current rename outcome.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="rowHeight"/> is not finite or is not positive.
    /// </exception>
    public InlineRenameResult InlineRename(
        string id,
        ref string text,
        ref bool requestFocus,
        float rowHeight,
        nuint capacity = 512,
        float width = -1f
    ) {
        m_session.EnsureDrawing();
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        if (!float.IsFinite(rowHeight) || rowHeight <= 0f)
        {
            throw new ArgumentOutOfRangeException(nameof(rowHeight), rowHeight, "Inline rename row height must be finite and positive.");
        }

        float fieldWidth = MathF.Max(1f, width < 0f ? NativeImGui.GetContentRegionAvail().X : width);
        string controlID = $"##rename_{id}";
        bool selectAll = requestFocus;
        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FramePadding), style.inlineRenameFramePadding);
        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FrameRounding), style.frameRounding);
        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FrameBorderSize), style.borderSize);
        try
        {
            float fieldHeight = NativeImGui.GetFrameHeight();
            Vector2 cursor = NativeImGui.GetCursorScreenPos();
            NativeImGui.SetCursorScreenPos(new Vector2(cursor.X, cursor.Y + MathF.Max(0f, (rowHeight - fieldHeight) * 0.5f)));
            NativeImGui.SetNextItemWidth(fieldWidth);
            if (selectAll)
            {
                NativeImGui.SetKeyboardFocusHere();
            }

            bool submitted;
            NativeImGui.PushStyleColor((int)(ImGuiCol.NavCursor), EditorPalette.transparent);
            try
            {
                submitted = ImGuiUtf8Buffer.InputText(
                    controlID,
                    null,
                    ref text,
                    (nuint)capacity,
                    ImGuiInputTextFlags.EnterReturnsTrue | ImGuiInputTextFlags.AutoSelectAll
                );
            }
            finally
            {
                NativeImGui.PopStyleColor();
            }

            bool deactivated = NativeImGui.IsItemDeactivated();
            bool escapePressed = NativeImGui.IsKeyPressed((NativeKey)ImGuiKey.Escape);
            bool active = NativeImGui.IsItemActive();
            bool contextMenuRequested = active && NativeImGui.IsMouseReleased((int)(ImGuiMouseButton.Right));
            if (selectAll && active)
            {
                uint inputID = NativeImGui.GetItemID();
                ImGuiInputTextState* inputState = NativeImGui.Raw.GetInputTextState(inputID);
                if (inputState is not null)
                {
                    NativeImGui.Raw.SelectAll(inputState);
                    requestFocus = false;
                }
            }

            if (active)
            {
                var focusOffset = new Vector2(C_INLINE_RENAME_FOCUS_OFFSET);
                ImGui.DrawNativeRectangle(
                    NativeImGui.Raw.GetForegroundDrawList((ImGuiViewport*)null),
                    NativeImGui.GetItemRectMin() - focusOffset,
                    NativeImGui.GetItemRectMax() + focusOffset,
                    NativeImGui.GetColorU32((int)(ImGuiCol.NavCursor)),
                    style.frameRounding + C_INLINE_RENAME_FOCUS_OFFSET,
                    (int)(ImDrawFlags.RoundCornersAll),
                    style.interactionOverlayThickness
                );
            }

            if (escapePressed)
            {
                return InlineRenameResult.Cancel;
            }
            if (submitted)
            {
                return InlineRenameResult.Commit;
            }
            if (contextMenuRequested)
            {
                // A context menu transfers focus into a popup after this item has already been
                // submitted. Complete the edit in this frame so its active text/navigation state
                // cannot be inherited by the popup on the following frame.
                NativeImGui.Raw.ClearActiveID();
                return InlineRenameResult.FocusLost;
            }

            return deactivated ? InlineRenameResult.FocusLost : InlineRenameResult.None;
        }
        finally
        {
            NativeImGui.PopStyleVar(3);
        }
    }
}

/// <summary>
/// Describes the outcome of an inline rename control.
/// </summary>
public enum InlineRenameResult
{
    /// <summary>
    /// Indicates that the interaction remains active.
    /// </summary>
    None,
    /// <summary>
    /// Indicates that the edited text should be committed.
    /// </summary>
    Commit,
    /// <summary>
    /// Indicates that the input lost focus and its valid value should be committed before closing.
    /// </summary>
    FocusLost,
    /// <summary>
    /// Indicates that the edited text should be discarded.
    /// </summary>
    Cancel
}
