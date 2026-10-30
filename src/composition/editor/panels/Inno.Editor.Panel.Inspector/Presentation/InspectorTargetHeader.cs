using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using System;
using System.Numerics;
using Inno.Editor.Inspection;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using Inno.Editor.Interactions;
using Ui = Inno.Editor.ImGui.ImGui;

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
    /// <returns>
    /// The current valid Inspector target, or <see langword="null"/> when none is available.
    /// </returns>
    /// <param name="interactions">
    /// Shared interaction service that resolves targets in their owning identity domains.
    /// </param>
    /// <param name="selectedTarget">
    /// The current inspection target used to render the header.
    /// </param>
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
            Ui.BeginDisabled(readOnly);
            try
            {
                DrawContent(drawer, context);
            }
            finally
            {
                Ui.EndDisabled();
            }

            if (notice is null)
                return;
            Ui.Spacing();
            EditorUI.current.widgets.HelpBox(notice, ImGuiIcon.CircleInfo, new Vector4(0.42f, 0.66f, 0.88f, EditorPalette.opacityOpaque));
        }, spanWindowPadding: true);
    }

    private void DrawContent(
        IInspectionDrawer drawer,
        InspectionDrawContext context
    ) {
        Ui.PushStyleVar(ImGuiStyleVar.ItemSpacing, EditorUI.current.widgets.style.compactItemSpacing);
        Ui.PushStyleVar(ImGuiStyleVar.FramePadding, EditorUI.current.widgets.style.compactFramePadding);
        try
        {
            float rowHeight = Ui.GetFrameHeight();
            float rowSpacing = EditorUI.current.widgets.style.targetHeaderRowSpacing;
            float headerContentHeight = rowHeight * 2f + rowSpacing;
            DrawIcon(drawer.GetIcon(context), headerContentHeight);
            Ui.SameLine(0f, -1f);
            Ui.BeginGroup();
            try
            {
                DrawNameRow(drawer, context, rowHeight);
                Ui.SetCursorPosY(Ui.GetCursorPosY() + rowSpacing);
                DrawCustomRow(drawer, context, rowHeight);
            }
            finally
            {
                Ui.EndGroup();
            }
        }
        finally
        {
            Ui.PopStyleVar(2);
        }
    }

    private void DrawNameRow(
        IInspectionDrawer drawer,
        InspectionDrawContext context,
        float rowHeight
    ) {
        Vector2 lockSize = EditorUI.current.widgets.GetCompactIconSize();
        float itemSpacing = Ui.GetLayout().itemSpacing.X;
        float nameWidth = MathF.Max(1f, Ui.GetContentRegionAvailable().X - lockSize.X - itemSpacing);
        (string name, Action<string>? nameSetter) = drawer.BindName(context);
        if (nameSetter is not null)
        {
            Ui.SetNextItemWidth(nameWidth);
            if (Ui.InputText($"##inspector_target_name_{GetTargetId(context.target)}", ref name, C_NAME_BUFFER_SIZE, ImGuiInputTextFlags.None))
            {
                nameSetter(name);
            }

            Ui.SameLine(0f, itemSpacing);
        }
        else
        {
            Ui.AlignTextToFramePadding();
            Vector2 textOrigin = Ui.GetCursorScreenPosition();
            EditorDrawList drawList = Ui.GetDrawList();
            drawList.PushClip(textOrigin, textOrigin + new Vector2(nameWidth, rowHeight));
            try
            {
                Ui.DrawText(textOrigin, Ui.GetStyleColor(ImGuiCol.Text), name);
            }
            finally
            {
                drawList.PopClip();
            }

            Ui.Dummy(new Vector2(nameWidth, rowHeight));
            Ui.SameLine(0f, itemSpacing);
        }

        string lockIcon = m_lock.isLocked ? ImGuiIcon.Lock : ImGuiIcon.LockOpen;
        string tooltip = m_lock.isLocked ? "Unlock Inspector" : "Lock Inspector";
        if (EditorUI.current.widgets.ClickableIcon("inspector_target_lock", lockIcon, tooltip))
            m_lock.Toggle(context.interactions, context.target);
    }

    private static void DrawIcon(
        string icon,
        float slotSize
    ) {
        Vector2 minimum = Ui.GetCursorScreenPosition();
        EditorUI.current.widgets.AddGlyphCentered(
            Ui.GetFontSize() * EditorUI.current.widgets.style.targetIconScale,
            icon,
            minimum + new Vector2(slotSize * 0.5f),
            Ui.GetStyleColor(ImGuiCol.Text)
        );
        Ui.Dummy(new Vector2(slotSize));
    }

    private static void DrawCustomRow(
        IInspectionDrawer drawer,
        InspectionDrawContext context,
        float rowHeight
    ) {
        ImGuiWindowFlags flags = ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse | ImGuiWindowFlags.NoSavedSettings;
        bool visible = Ui.BeginChild($"##inspector_target_custom_{GetTargetId(context.target)}", new Vector2(0f, rowHeight), ImGuiChildFlags.None, flags);
        try
        {
            if (visible)
                drawer.DrawHeader(context);
        }
        finally
        {
            Ui.EndChild();
        }
    }

    private static string GetTargetId(object target) => $"{target.GetType().FullName}_{System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(target)}";
}
