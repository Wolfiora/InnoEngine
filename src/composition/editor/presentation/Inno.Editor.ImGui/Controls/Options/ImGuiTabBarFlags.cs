using System;

namespace Inno.Editor.ImGui;
/// <summary>
/// Defines the supported tab bar flags choices at the Editor presentation boundary.
/// </summary>
[Flags]
public enum ImGuiTabBarFlags : int
{
    /// <summary>
    /// Uses the ordinary behavior without additional options.
    /// </summary>
    None = (int)Inno.Native.ImGui.ImGuiTabBarFlags.None,
    /// <summary>
    /// Allow manually dragging tabs to re-order them + New tabs are appended at the end of list
    /// </summary>
    Reorderable = (int)Inno.Native.ImGui.ImGuiTabBarFlags.Reorderable,
    /// <summary>
    /// Automatically select new tabs when they appear
    /// </summary>
    AutoSelectNewTabs = (int)Inno.Native.ImGui.ImGuiTabBarFlags.AutoSelectNewTabs,
    /// <summary>
    /// Disable buttons to open the tab list popup
    /// </summary>
    TabListPopupButton = (int)Inno.Native.ImGui.ImGuiTabBarFlags.TabListPopupButton,
    /// <summary>
    /// Disable behavior of closing tabs (that are submitted with p_open != NULL) with middle mouse button. You may handle this behavior manually on user's side with if (IsItemHovered() &amp; &amp; IsMouseClicked(2)) *p_open = false.
    /// </summary>
    NoCloseWithMiddleMouseButton = (int)Inno.Native.ImGui.ImGuiTabBarFlags.NoCloseWithMiddleMouseButton,
    /// <summary>
    /// Disable scrolling buttons (apply when fitting policy is ImGuiTabBarFlags_FittingPolicyScroll)
    /// </summary>
    NoTabListScrollingButtons = (int)Inno.Native.ImGui.ImGuiTabBarFlags.NoTabListScrollingButtons,
    /// <summary>
    /// Disable tooltips when hovering a tab
    /// </summary>
    NoTooltip = (int)Inno.Native.ImGui.ImGuiTabBarFlags.NoTooltip,
    /// <summary>
    /// Draw selected overline markers over selected tab
    /// </summary>
    DrawSelectedOverline = (int)Inno.Native.ImGui.ImGuiTabBarFlags.DrawSelectedOverline,
    /// <summary>
    /// Shrink down tabs when they don't fit, until width is style.TabMinWidthShrink, then enable scrolling buttons.
    /// </summary>
    FittingPolicyMixed = (int)Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyMixed,
    /// <summary>
    /// Shrink down tabs when they don't fit
    /// </summary>
    FittingPolicyShrink = (int)Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyShrink,
    /// <summary>
    /// Enable scrolling buttons when tabs don't fit
    /// </summary>
    FittingPolicyScroll = (int)Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyScroll,
    /// <summary>
    /// Selects the fitting policy mask presentation behavior.
    /// </summary>
    FittingPolicyMask = (int)Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyMask,
    /// <summary>
    /// Selects the fitting policy default presentation behavior.
    /// </summary>
    FittingPolicyDefault = (int)Inno.Native.ImGui.ImGuiTabBarFlags.FittingPolicyDefault,
}
