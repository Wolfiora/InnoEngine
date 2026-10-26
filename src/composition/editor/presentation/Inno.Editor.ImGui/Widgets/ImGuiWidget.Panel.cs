using System;
using System.Numerics;

using Inno.Scripting.Api;
using Inno.Native.ImGui;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.ImGui.ImGuiWidget;

/// <summary>
/// Reusable editor widgets built on top of <see cref="ImGui"/>.
/// </summary>
public static unsafe partial class ImGuiWidget
{
    /// <summary>
    /// Opens a standard panel window and executes panel body.
    /// </summary>
    /// <param name="title">
    /// Window title.
    /// </param>
    /// <param name="isOpen">
    /// Visible state.
    /// </param>
    /// <param name="drawBody">
    /// Panel body callback.
    /// </param>
    /// <param name="flags">
    /// Window flags.
    /// </param>
    /// <param name="useWindowPadding">
    /// Whether the panel body should use the current standard window padding.
    /// </param>
    [ScriptingApiIgnore]
    public static void PanelWindow(
        string title,
        ref bool isOpen,
        Action drawBody,
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoCollapse,
        bool useWindowPadding = true
    ) {
        if (!isOpen)
            return;

        bool pushedPadding = false;
        bool beganWindow = false;
        try
        {
            if (!useWindowPadding)
            {
                NativeImGui.PushStyleVar((int)(ImGuiStyleVar.WindowPadding), Vector2.Zero);
                pushedPadding = true;
            }
            // Dock tabs need their own vertical breathing room. Applying the metric only while
            // Begin builds the window decorations keeps normal inputs compact without clipping
            // the first tab row against the main-menu work rect.
            NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FramePadding), style.panelTabFramePadding);
            // Native title separators use FrameBorderSize, which is intended for inputs in the
            // editor theme. Suppress that extra strip only while window decorations are built.
            NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FrameBorderSize), 0f);
            bool visible;
            try
            {
                visible = ImGui.BeginNativeWindow(title, flags, modal: false);
            }
            finally
            {
                NativeImGui.PopStyleVar(2);
            }
            beganWindow = true;
            if (pushedPadding)
            {
                NativeImGui.PopStyleVar();
                pushedPadding = false;
            }
            if (visible)
            {
                if (DrawPanelCloseButton(title))
                    isOpen = false;

                if (isOpen)
                    drawBody();
            }
        }
        finally
        {
            if (pushedPadding)
                NativeImGui.PopStyleVar();
            if (beganWindow)
                NativeImGui.End();
        }
    }

    /// <summary>
    /// Draws a vertically auto-sized content region that is constrained to the current available
    /// width and cannot create an independent scroll range.
    /// </summary>
    /// <param name="id">
    /// Stable identifier used by ImGui to track the content region.
    /// </param>
    /// <param name="drawContent">
    /// Callback that draws the complete region contents.
    /// </param>
    /// <param name="useWindowPadding">
    /// Whether the region should apply the centralized standard window padding exactly once.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="drawContent"/> is <see langword="null"/>.
    /// </exception>
    public static void ConstrainedContent(
        string id,
        Action drawContent,
        bool useWindowPadding = true
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(drawContent);

        float width = MathF.Max(1f, NativeImGui.GetContentRegionAvail().X);
        Vector2 padding = useWindowPadding
            ? ImGui.FromNative(NativeImGui.Raw.GetStyle()->WindowPadding)
            : Vector2.Zero;
        float contentWidth = MathF.Max(1f, width - padding.X * 2f);
        ImGuiChildFlags childFlags = ImGuiChildFlags.AutoResizeY |
                                     ImGuiChildFlags.AlwaysAutoResize;
        if (useWindowPadding)
            childFlags |= ImGuiChildFlags.AlwaysUseWindowPadding;
        ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoScrollbar |
                                       ImGuiWindowFlags.NoScrollWithMouse |
                                       ImGuiWindowFlags.NoSavedSettings;
        NativeImGui.SetNextWindowContentSize(new Vector2(contentWidth, 0f));
        bool visible = NativeImGui.BeginChild(id, new Vector2(width, 0f), (int)(childFlags), (int)(windowFlags));
        try
        {
            if (visible)
                drawContent();
        }
        finally
        {
            NativeImGui.EndChild();
        }
    }

    /// <summary>
    /// Draws a square-cornered editor header surface using the shared target-header palette,
    /// border, and padding.
    /// </summary>
    /// <param name="id">
    /// Stable identifier used by ImGui to track the header region.
    /// </param>
    /// <param name="drawContent">
    /// Callback that draws the complete header content.
    /// </param>
    /// <param name="spanWindowPadding">
    /// Whether the header should extend through the current parent window padding to both edges.
    /// </param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="id"/> is empty.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="drawContent"/> is <see langword="null"/>.
    /// </exception>
    public static void HeaderSurface(
        string id,
        Action drawContent,
        bool spanWindowPadding = false
    ) {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(drawContent);

        ImGuiWindow* parentWindow = NativeImGui.Raw.GetCurrentWindow();
        Vector2 contentCursor = NativeImGui.GetCursorScreenPos();
        Vector2 parentPadding = spanWindowPadding
            ? ImGui.FromNative(parentWindow->WindowPadding)
            : Vector2.Zero;
        Vector2 headerOrigin = contentCursor - parentPadding;
        float width = MathF.Max(
            1f,
            NativeImGui.GetContentRegionAvail().X + parentPadding.X * 2f);
        NativeImGui.SetCursorScreenPos(headerOrigin);

        NativeImGui.PushStyleColor((int)(ImGuiCol.FrameBg), EditorPalette.inspectorTargetHeader);
        NativeImGui.PushStyleColor((int)(ImGuiCol.Border), EditorPalette.inspectorTargetHeaderBorder);
        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FramePadding), style.inspectorTargetHeaderPadding);
        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FrameBorderSize), style.borderSize);
        NativeImGui.PushStyleVar((int)(ImGuiStyleVar.FrameRounding), 0f);
        try
        {
            ImGuiChildFlags childFlags = ImGuiChildFlags.FrameStyle | ImGuiChildFlags.AutoResizeY;
            ImGuiWindowFlags windowFlags = ImGuiWindowFlags.NoScrollbar |
                                           ImGuiWindowFlags.NoScrollWithMouse |
                                           ImGuiWindowFlags.NoSavedSettings;
            bool visible = NativeImGui.BeginChild(id, new Vector2(width, 0f), (int)(childFlags), (int)(windowFlags));
            // FrameStyle consumed the square outer-container rounding in BeginChild. Restore the
            // normal editor rounding before drawing controls inside the header.
            NativeImGui.PopStyleVar();
            try
            {
                if (visible)
                    drawContent();
            }
            finally
            {
                NativeImGui.EndChild();
            }
        }
        finally
        {
            NativeImGui.PopStyleVar(2);
            NativeImGui.PopStyleColor(2);
        }

        NativeImGui.SetCursorScreenPos(new Vector2(
            contentCursor.X,
            NativeImGui.GetCursorScreenPos().Y));
    }

    private static bool DrawPanelCloseButton(string title)
    {
        if (!NativeImGui.IsWindowDocked())
            return DrawFloatingPanelCloseButton(title);

        uint dockId = NativeImGui.GetWindowDockID();
        ImGuiDockNode* dockNode = NativeImGui.Raw.DockBuilderGetNode(dockId);
        if (dockNode == null || NativeImGui.Raw.DockNodeBeginAmendTabBar(dockNode) == 0)
            return false;

        try
        {
            ImGuiTabBar* tabBar = dockNode->TabBar;
            if (tabBar == null)
                return false;
            ImRect tabBarBounds = tabBar->BarRect;
            float tabBarHeight = MathF.Max(1f, tabBarBounds.Max.Y - tabBarBounds.Min.Y);
            float iconSlotSize = MathF.Min(GetCompactIconSize().X, tabBarHeight);
            Vector2 itemCenter = new(
                tabBarBounds.Max.X - iconSlotSize * 0.5f,
                (tabBarBounds.Min.Y + tabBarBounds.Max.Y) * 0.5f);
            Vector2 itemMinimum = itemCenter - new Vector2(iconSlotSize * 0.5f);
            Vector2 itemMaximum = itemCenter + new Vector2(iconSlotSize * 0.5f);
            ImRect itemBounds = new()
            {
                Min = ImGui.ToNative(itemMinimum),
                Max = ImGui.ToNative(itemMaximum)
            };
            uint itemId = NativeImGui.GetID($"##panel_close_{dockId}");
            bool hovered = false;
            bool held = false;
            ImGuiButtonFlags buttonFlags = (ImGuiButtonFlags)(
                (int)ImGuiButtonFlagsPrivate.AllowOverlap |
                (int)ImGuiButtonFlagsPrivate.NoNavFocus |
                (int)ImGuiButtonFlagsPrivate.PressedOnClickRelease);
            ImGuiWindow* hoveredWindow = NativeImGui.Raw.GetCurrentContext()->HoveredWindow;
            bool hostHovered = hoveredWindow != null &&
                               hoveredWindow->RootWindowDockTree == dockNode->HostWindow->RootWindowDockTree;
            bool mouseHovered = hostHovered && NativeImGui.IsMouseHoveringRect(itemMinimum, itemMaximum);
            bool itemAdded = NativeImGui.Raw.ItemAdd(&itemBounds, itemId, null, 0) != 0;
            bool pressed = hostHovered && itemAdded &&
                           NativeImGui.ButtonBehavior(itemBounds, itemId, out hovered, out held, (int)buttonFlags);
            hovered |= mouseHovered;
            pressed |= mouseHovered && NativeImGui.IsMouseClicked((int)(ImGuiMouseButton.Left));

            uint iconColor = hovered || held
                ? NativeImGui.ColorConvertFloat4ToU32(EditorPalette.compactControlHovered)
                : NativeImGui.GetColorU32((int)(ImGuiCol.Text));
            DrawPanelCloseMark(
                NativeImGui.Raw.GetWindowDrawList(),
                itemCenter,
                iconSlotSize,
                iconColor);

            if (hovered && BeginMenuTooltip())
            {
                NativeImGui.TextUnformatted($"Close {title}");
                EndMenuTooltip();
            }

            return pressed;
        }
        finally
        {
            NativeImGui.Raw.DockNodeEndAmendTabBar();
        }
    }

    private static bool DrawFloatingPanelCloseButton(string title)
    {
        ImGuiWindow* window = NativeImGui.Raw.GetCurrentWindow();
        if ((window->Flags & (int)ImGuiWindowFlags.NoTitleBar) != 0)
            return false;
        ImGuiStyle* nativeStyle = NativeImGui.Raw.GetStyle();
        float size = GetCompactIconSize().X;
        Vector2 maximum = new(window->Pos.X + window->Size.X - nativeStyle->WindowBorderSize - nativeStyle->FramePadding.X,
            window->Pos.Y + (window->TitleBarHeight + size) * 0.5f);
        Vector2 minimum = maximum - new Vector2(size);
        ImRect bounds = new() { Min = ImGui.ToNative(minimum), Max = ImGui.ToNative(maximum) };
        ImRect previousClip = window->ClipRect;
        ImDrawList* draw = NativeImGui.Raw.GetWindowDrawList();
        Vector2 windowMaximum = ImGui.FromNative(window->Pos) + ImGui.FromNative(window->Size);
        window->ClipRect = new ImRect { Min = window->Pos, Max = ImGui.ToNative(windowMaximum) };
        ImGui.PushNativeClipRectangle(draw, ImGui.FromNative(window->Pos), windowMaximum, 0);
        try
        {
            uint id = NativeImGui.GetID("##floating_panel_close");
            bool hovered = false, held = false;
            bool pressed = NativeImGui.Raw.ItemAdd(&bounds, id, null, 0) != 0 && NativeImGui.ButtonBehavior(
                bounds, id, out hovered, out held,
                (int)ImGuiButtonFlagsPrivate.NoNavFocus | (int)ImGuiButtonFlagsPrivate.PressedOnClickRelease);
            uint iconColor = hovered || held
                ? NativeImGui.ColorConvertFloat4ToU32(EditorPalette.compactControlHovered)
                : NativeImGui.GetColorU32((int)(ImGuiCol.Text));
            DrawPanelCloseMark(draw, minimum + new Vector2(size * 0.5f), size, iconColor);
            if (hovered)
                DrawItemTooltip($"Close {title}");
            return pressed;
        }
        finally
        {
            NativeImGui.Raw.PopClipRect(draw);
            window->ClipRect = previousClip;
        }
    }

    private static void DrawPanelCloseMark(
        ImDrawList* drawList,
        Vector2 center,
        float slotSize,
        uint color
    ) {
        float thickness = MathF.Max(1f, style.borderSize);
        float extent = MathF.Max(
            thickness,
            slotSize * 0.5f * 0.7071f - thickness);
        var diagonal = new Vector2(extent);
        ImGui.DrawNativeLine(drawList, center - diagonal, center + diagonal, color, thickness);
        ImGui.DrawNativeLine(
            drawList,
            center + new Vector2(-extent, extent),
            center + new Vector2(extent, -extent),
            color,
            thickness
        );
    }

    /// <summary>
    /// Draws disabled hint text that wraps to the current content width.
    /// </summary>
    /// <param name="text">
    /// Hint text.
    /// </param>
    /// <exception cref="ArgumentNullException">
    /// Thrown when <paramref name="text"/> is <see langword="null"/>.
    /// </exception>
    public static void Hint(string text)
    {
        ArgumentNullException.ThrowIfNull(text);
        NativeImGui.BeginDisabled(true);
        try
        {
            NativeImGui.PushTextWrapPos(
                NativeImGui.GetCursorPosX() + MathF.Max(1f, NativeImGui.GetContentRegionAvail().X));
            try
            {
                NativeImGui.TextUnformatted(text);
            }
            finally
            {
                NativeImGui.PopTextWrapPos();
            }
        }
        finally
        {
            NativeImGui.EndDisabled();
        }
    }
}
