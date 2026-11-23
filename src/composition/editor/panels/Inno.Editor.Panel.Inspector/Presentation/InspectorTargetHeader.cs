using System;
using System.Numerics;
using System.Runtime.CompilerServices;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.Inspector;

/// <summary>
/// Draws the persistent identity header shared by every Inspector target type.
/// </summary>
internal sealed class InspectorTargetHeader
{
    private const int C_NAME_BUFFER_SIZE = 512;
    private readonly InspectorLockControl m_lock = new();

    /// <summary>
    /// Resolves the target that should be presented after applying Inspector lock state.
    /// </summary>
    /// <param name="interactions">
    /// Shared interaction service that resolves targets in their owning identity domains.
    /// </param>
    /// <param name="selectedTarget">
    /// The current inspection target used to render the header.
    /// </param>
    /// <returns>
    /// The current valid Inspector target, or <see langword="null"/> when none is available.
    /// </returns>
    internal object? Resolve(
        EditorInteractions interactions,
        object? selectedTarget
    ) => m_lock.Resolve(interactions, selectedTarget);

    /// <summary>
    /// Draws the common framed header for a resolved Inspector target.
    /// </summary>
    /// <param name="drawer">
    /// The resolved target-specific Inspector drawer.
    /// </param>
    /// <param name="context">
    /// The drawing context for the current target.
    /// </param>
    /// <param name="readOnly">
    /// Whether target controls must be disabled while the notice remains visible.
    /// </param>
    /// <param name="notice">
    /// The optional contextual message placed beneath the regular header rows.
    /// </param>
    internal void Draw(
        IInspectionDrawer drawer,
        InspectionDrawContext context,
        bool readOnly,
        string? notice
    ) {
        ArgumentNullException.ThrowIfNull(drawer);
        ArgumentNullException.ThrowIfNull(context);
        EditorUI.current.widgets.HeaderSurface("##inspector_target_header", () =>
        {
            Gui.BeginDisabled(readOnly);
            try
            {
                DrawContent(drawer, context);
            }
            finally
            {
                Gui.EndDisabled();
            }

            if (notice is null)
            {
                return;
            }
            Gui.Spacing();
            EditorUI.current.widgets.HelpBox(
                notice,
                ImGuiIcon.C_CIRCLE_INFO,
                new Vector4(0.42f, 0.66f, 0.88f, EditorPalette.C_OPACITY_OPAQUE)
            );
        }, spanWindowPadding: true);
    }

    private void DrawContent(
        IInspectionDrawer drawer,
        InspectionDrawContext context
    ) {
        Gui.PushStyleVar(ImGuiStyleVar.ItemSpacing, EditorUI.current.widgets.style.compactItemSpacing);
        Gui.PushStyleVar(ImGuiStyleVar.FramePadding, EditorUI.current.widgets.style.compactFramePadding);
        try
        {
            float rowHeight = Gui.GetFrameHeight();
            float rowSpacing = EditorUI.current.widgets.style.targetHeaderRowSpacing;
            float headerContentHeight = rowHeight * 2f + rowSpacing;
            DrawIcon(drawer.GetIcon(context), headerContentHeight);
            Gui.SameLine(0f, -1f);
            Gui.BeginGroup();
            try
            {
                DrawNameRow(drawer, context, rowHeight);
                Gui.SetCursorPosY(Gui.GetCursorPosY() + rowSpacing);
                DrawCustomRow(drawer, context, rowHeight);
            }
            finally
            {
                Gui.EndGroup();
            }
        }
        finally
        {
            Gui.PopStyleVar(2);
        }
    }

    private void DrawNameRow(
        IInspectionDrawer drawer,
        InspectionDrawContext context,
        float rowHeight
    ) {
        Vector2 lockSize = EditorUI.current.widgets.GetCompactIconSize();
        float itemSpacing = Gui.GetLayout().itemSpacing.X;
        float nameWidth = MathF.Max(1f, Gui.GetContentRegionAvailable().X - lockSize.X - itemSpacing);
        (string name, Action<string>? nameSetter) = drawer.BindName(context);
        if (nameSetter is not null)
        {
            Gui.SetNextItemWidth(nameWidth);
            if (Gui.InputText(
                $"##inspector_target_name_{GetTargetID(context.target)}",
                ref name,
                C_NAME_BUFFER_SIZE,
                ImGuiInputTextFlags.None
            )) {
                nameSetter(name);
            }

            Gui.SameLine(0f, itemSpacing);
        }
        else
        {
            Gui.AlignTextToFramePadding();
            Vector2 textOrigin = Gui.GetCursorScreenPosition();
            EditorDrawList drawList = Gui.GetDrawList();
            drawList.PushClip(textOrigin, textOrigin + new Vector2(nameWidth, rowHeight));
            try
            {
                Gui.DrawText(textOrigin, Gui.GetStyleColor(ImGuiCol.Text), name);
            }
            finally
            {
                drawList.PopClip();
            }

            Gui.Dummy(new Vector2(nameWidth, rowHeight));
            Gui.SameLine(0f, itemSpacing);
        }

        string lockIcon = m_lock.isLocked ? ImGuiIcon.C_LOCK : ImGuiIcon.C_LOCK_OPEN;
        string tooltip = m_lock.isLocked ? "Unlock Inspector" : "Lock Inspector";
        if (EditorUI.current.widgets.ClickableIcon("inspector_target_lock", lockIcon, tooltip))
        {
            m_lock.Toggle(context.interactions, context.target);
        }
    }

    private static void DrawIcon(
        string icon,
        float slotSize
    ) {
        Vector2 minimum = Gui.GetCursorScreenPosition();
        EditorUI.current.widgets.AddGlyphCentered(
            Gui.GetFontSize() * EditorUI.current.widgets.style.targetIconScale,
            icon,
            minimum + new Vector2(slotSize * 0.5f),
            Gui.GetStyleColor(ImGuiCol.Text)
        );
        Gui.Dummy(new Vector2(slotSize));
    }

    private static void DrawCustomRow(
        IInspectionDrawer drawer,
        InspectionDrawContext context,
        float rowHeight
    ) {
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoSavedSettings;
        bool visible = Gui.BeginChild(
            $"##inspector_target_custom_{GetTargetID(context.target)}",
            new Vector2(0f, rowHeight),
            ImGuiChildFlags.None,
            flags
        );
        try
        {
            if (visible)
            {
                drawer.DrawHeader(context);
            }
        }
        finally
        {
            Gui.EndChild();
        }
    }

    private static string GetTargetID(object target)
        => $"{target.GetType().FullName}_{RuntimeHelpers.GetHashCode(target)}";
}
