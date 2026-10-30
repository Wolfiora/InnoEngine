using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported tab item flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiTabItemFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiTabItemFlags.None,
    /// <summary>
    /// Display a dot next to the title + set ImGuiTabItemFlags_NoAssumedClosure.
    /// </summary>
    UnsavedDocument = (int)Inno.Native.ImGui.ImGuiTabItemFlags.UnsavedDocument,
    /// <summary>
    /// Trigger flag to programmatically make the tab selected when calling BeginTabItem()
    /// </summary>
    SetSelected = (int)Inno.Native.ImGui.ImGuiTabItemFlags.SetSelected,
    /// <summary>
    /// Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You may handle this behavior manually on user's side with if (IsItemHovered() &amp; &amp; IsMouseClicked(2)) *p_open = false.
    /// </summary>
    NoCloseWithMiddleMouseButton = (int)Inno.Native.ImGui.ImGuiTabItemFlags.NoCloseWithMiddleMouseButton,
    /// <summary>
    /// Don't call PushID()/PopID() on BeginTabItem()/EndTabItem()
    /// </summary>
    NoPushId = (int)Inno.Native.ImGui.ImGuiTabItemFlags.NoPushId,
    /// <summary>
    /// Disable tooltip for the given tab
    /// </summary>
    NoTooltip = (int)Inno.Native.ImGui.ImGuiTabItemFlags.NoTooltip,
    /// <summary>
    /// Disable reordering this tab or having another tab cross over this tab
    /// </summary>
    NoReorder = (int)Inno.Native.ImGui.ImGuiTabItemFlags.NoReorder,
    /// <summary>
    /// Enforce the tab position to the left of the tab bar (after the tab list popup button)
    /// </summary>
    Leading = (int)Inno.Native.ImGui.ImGuiTabItemFlags.Leading,
    /// <summary>
    /// Enforce the tab position to the right of the tab bar (before the scrolling buttons)
    /// </summary>
    Trailing = (int)Inno.Native.ImGui.ImGuiTabItemFlags.Trailing,
    /// <summary>
    /// Tab is selected when trying to close + closure is not immediately assumed (will wait for user to stop submitting the tab). Otherwise closure is assumed when pressing the X, so if you keep submitting the tab may reappear at end of tab bar.
    /// </summary>
    NoAssumedClosure = (int)Inno.Native.ImGui.ImGuiTabItemFlags.NoAssumedClosure,
}
