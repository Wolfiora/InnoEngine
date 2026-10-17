using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using EditorWidget = Inno.Editor.ImGui.ImGuiWidget.ImGuiWidget;
using Inno.Native.ImGui;
using Inno.Adapter.Presentation.ImGui;
using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;
using NativeImGui = Inno.Native.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;

internal sealed partial class FileBrowserPanel

{

    private void DrawListRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        bool entriesVisible = NativeImGui.BeginChild(
            "##EntriesScroll",
            Vector2.Zero,
            ImGuiChildFlags.None);
        try
        {
            if (entriesVisible)
            {
                DrawEntriesTable(context, entries, m_assets.browser.currentDirectory);
                HandleBackgroundSelection(context);
                m_contextMenu.DrawBackground(
                    context,
                    "##asset_list_background_context",
                    FileBrowserPresentation.List);
            }
        }
        finally
        {
            NativeImGui.EndChild();
        }
    }

    private void DrawEntriesTable(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries,
        string currentDirectory
    ) {
        ImGuiTableFlags flags =
            ImGuiTableFlags.RowBg |
            ImGuiTableFlags.NoPadOuterX |
            ImGuiTableFlags.SizingStretchProp |
            ImGuiTableFlags.NoSavedSettings;

        ImGuiStylePtr style = NativeImGui.GetStyle();
        Vector2 tableOrigin = NativeImGui.GetCursorScreenPos();
        Vector2 tableSize = NativeImGui.GetContentRegionAvail();
        NativeImGui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(style.WindowPadding.X, style.CellPadding.Y));
        try
        {
            bool tableStarted = NativeImGui.BeginTable(
                "##FileBrowserEntries",
                3,
                flags,
                new Vector2(0f, 0f));
            if (!tableStarted)
            {
                NativeImGui.Dummy(Vector2.Zero);
                return;
            }
            try
            {
                NativeImGui.TableSetupColumn(
                    "Name",
                    ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize,
                    m_listNameSeparatorPosition);
                NativeImGui.TableSetupColumn(
                    "Type",
                    ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize,
                    m_listTypeSeparatorPosition - m_listNameSeparatorPosition);
                NativeImGui.TableSetupColumn(
                    "Source",
                    ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize,
                    1f - m_listTypeSeparatorPosition);
                DrawHeaderRow();

                uint rowBg = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.collectionRow);
                uint rowAltBg = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.collectionRowAlternate);
                float entryRowHeight = NativeImGui.GetTextLineHeight() + style.CellPadding.Y * 2f;
                for (int i = 0; i < entries.Count; i++)
                {
                    FileBrowserDisplayEntry item = entries[i];
                    AssetFileEntry entry = item.entry;
                    if (i > 0)
                        NativeImGui.TableNextRow(ImGuiTableRowFlags.None, EditorWidget.style.assetListRowSpacing);
                    NativeImGui.TableNextRow(ImGuiTableRowFlags.None, entryRowHeight);
                    NativeImGui.TableSetBgColor(
                        ImGuiTableBgTarget.RowBg0,
                        i % 2 == 0 ? rowBg : rowAltBg);

                    DrawNameCell(context, item);
                    DrawTextCell(item.isPluginRoot ? "IPLUGIN" : GetTypeText(entry), EditorPalette.assetText);
                    DrawTextCell(
                        item.isPluginRoot
                            ? "~"
                            : GetSourceText(entry, currentDirectory),
                        EditorPalette.assetText);
                }
            }
            finally
            {
                NativeImGui.EndTable();
            }

            Vector2 tableEnd = NativeImGui.GetCursorScreenPos();
            float tableHeight = MathF.Max(1f, tableEnd.Y - tableOrigin.Y);
            var interactionSize = new Vector2(tableSize.X, tableHeight);
            ListColumnSeparatorState separators = HandleListColumnSeparators(tableOrigin, interactionSize);
            DrawListColumnSeparators(tableOrigin, tableSize.X, tableHeight, separators);
            NativeImGui.SetCursorScreenPos(tableEnd);
            // Commit the restored layout cursor so overlay separator hit targets cannot implicitly
            // extend the scrolling child when UI scaling changes their physical bounds.
            NativeImGui.Dummy(Vector2.Zero);
        }
        finally
        {
            NativeImGui.PopStyleVar();
        }
    }

    private ListColumnSeparatorState HandleListColumnSeparators(
        Vector2 origin,
        Vector2 size
    ) {
        float width = MathF.Max(1f, size.X);
        float height = MathF.Max(1f, size.Y);
        float hitWidth = EditorWidget.style.assetListSeparatorHitWidth;
        bool nameHovered;
        bool nameActive;
        bool typeHovered;
        bool typeActive;

        float nameX = origin.X + width * m_listNameSeparatorPosition;
        NativeImGui.SetCursorScreenPos(new Vector2(nameX - hitWidth * 0.5f, origin.Y));
        NativeImGui.SetNextItemAllowOverlap();
        _ = NativeImGui.InvisibleButton("##AssetListNameSeparator", new Vector2(hitWidth, height));
        nameHovered = NativeImGui.IsItemHovered();
        nameActive = NativeImGui.IsItemActive();
        if (nameHovered || nameActive)
            NativeImGui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);

        if (nameActive)
        {
            float requested = (NativeImGui.GetMousePos().X - origin.X) / width;
            SetListColumnSeparators(requested, m_listTypeSeparatorPosition);
        }

        float typeX = origin.X + width * m_listTypeSeparatorPosition;
        NativeImGui.SetCursorScreenPos(new Vector2(typeX - hitWidth * 0.5f, origin.Y));
        NativeImGui.SetNextItemAllowOverlap();
        _ = NativeImGui.InvisibleButton("##AssetListTypeSeparator", new Vector2(hitWidth, height));
        typeHovered = NativeImGui.IsItemHovered();
        typeActive = NativeImGui.IsItemActive();
        if (typeHovered || typeActive)
            NativeImGui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);

        if (typeActive)
        {
            float requested = (NativeImGui.GetMousePos().X - origin.X) / width;
            SetListColumnSeparators(m_listNameSeparatorPosition, requested);
        }

        return new ListColumnSeparatorState(nameHovered, nameActive, typeHovered, typeActive);
    }

    private void DrawListColumnSeparators(
        Vector2 origin,
        float width,
        float height,
        ListColumnSeparatorState state
    ) {
        float bottom = origin.Y + MathF.Max(1f, height);
        DrawListColumnSeparator(
            origin.X + width * m_listNameSeparatorPosition,
            origin.Y,
            bottom,
            state.nameHovered,
            state.nameActive);
        DrawListColumnSeparator(
            origin.X + width * m_listTypeSeparatorPosition,
            origin.Y,
            bottom,
            state.typeHovered,
            state.typeActive);
    }

    private static void DrawListColumnSeparator(
        float x,
        float top,
        float bottom,
        bool hovered,
        bool active
    ) {
        Vector4 color = active
            ? EditorPalette.assetAccent
            : hovered
                ? EditorPalette.assetBorder
                : EditorPalette.assetBorderSoft;
        NativeImGui.GetWindowDrawList().AddLine(
            new Vector2(x, top),
            new Vector2(x, bottom),
            NativeImGui.ColorConvertFloat4ToU32(color),
            EditorWidget.style.borderSize);
    }

    private void SetListColumnSeparators(
        float namePosition,
        float typePosition
    ) {
        float minimum = EditorWidget.style.assetListMinimumColumnRatio;
        m_listNameSeparatorPosition = Math.Clamp(
            namePosition,
            minimum,
            1f - minimum * 2f);
        m_listTypeSeparatorPosition = Math.Clamp(
            typePosition,
            m_listNameSeparatorPosition + minimum,
            1f - minimum);
    }

    private static void DrawHeaderRow()
    {
        NativeImGui.TableNextRow();
        NativeImGui.TableSetBgColor(
            ImGuiTableBgTarget.RowBg0,
            NativeImGui.ColorConvertFloat4ToU32(EditorPalette.collectionHeader));
        NativeImGui.TableSetBgColor(
            ImGuiTableBgTarget.RowBg1,
            NativeImGui.ColorConvertFloat4ToU32(EditorPalette.collectionHeader));
        _ = NativeImGui.TableSetColumnIndex(0);
        InsetListCellContent();
        NativeImGui.TextUnformatted("Name");
        _ = NativeImGui.TableSetColumnIndex(1);
        InsetListCellContent();
        NativeImGui.TextUnformatted("Type");
        _ = NativeImGui.TableSetColumnIndex(2);
        InsetListCellContent();
        NativeImGui.TextUnformatted("Source");
    }

    private void DrawNameCell(
        EditorContext context,
        FileBrowserDisplayEntry item
    ) {
        AssetFileEntry entry = item.entry;
        _ = NativeImGui.TableSetColumnIndex(0);
        string icon = m_assets.GetIcon(entry);
        string name = item.isPluginRoot
            ? item.displayName
            : entry.nameWithoutExtension;
        bool selected = m_assets.browser.IsSelected(context, entry);
        bool editing = m_rename.IsEditing(context, entry.assetPath.ToString(), FileBrowserPresentation.List);
        ImGuiTablePtr table = ImGuiP.GetCurrentTable();
        float rowMinimumY = table.RowPosY1;
        float rowMaximumY = table.RowPosY2;

        InsetListCellContent();
        NativeImGui.PushStyleColor(ImGuiCol.Header, EditorPalette.transparent);
        NativeImGui.PushStyleColor(ImGuiCol.HeaderHovered, EditorPalette.transparent);
        NativeImGui.PushStyleColor(ImGuiCol.HeaderActive, EditorPalette.transparent);
        try
        {
            Vector2 iconTextPos = NativeImGui.GetCursorScreenPos();
            ImGuiSelectableFlags selectableFlags =
                ImGuiSelectableFlags.SpanAllColumns |
                ImGuiSelectableFlags.AllowDoubleClick |
                ImGuiSelectableFlags.AllowOverlap;
            bool activated = NativeImGui.Selectable(
                $"##entry_{entry.assetPath.ToString()}",
                selected,
                selectableFlags);
            bool itemHovered = NativeImGui.IsItemHovered();
            bool doubleClicked = itemHovered && NativeImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
            if (activated)
            {
                HandleEntryActivation(
                    context,
                    entry,
                    FileBrowserPresentation.List,
                    doubleClicked);
            }

            bool itemActive = NativeImGui.IsItemActive();
            if (!editing)
            {
                m_contextMenu.DrawEntry(
                    context,
                    $"##asset_context_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.List);
            }
            if (selected || itemHovered)
            {
                Vector4 highlight = itemActive
                    ? EditorPalette.GetHovered(EditorPalette.assetAccent)
                    : EditorPalette.assetAccent;
                NativeImGui.TableSetBgColor(
                    ImGuiTableBgTarget.RowBg1,
                    NativeImGui.ColorConvertFloat4ToU32(highlight));
            }
            if (!editing)
            {
                m_dragDrop.DrawAssetSource(context, entry);
                if (entry.isDirectory && !entry.isReadOnly)
                    m_dragDrop.DrawDirectoryTarget(context, entry.assetPath.ToString());
            }

            NativeImGui.SameLine(iconTextPos.X - NativeImGui.GetWindowPos().X, 0f);
            if (editing)
            {
                EditorWidget.IconText(icon, string.Empty, false);
                NativeImGui.SameLine(0f, 0f);
                Vector2 renameCursor = NativeImGui.GetCursorScreenPos();
                NativeImGui.SetCursorScreenPos(new Vector2(renameCursor.X, rowMinimumY));
                m_rename.Draw(
                    context,
                    $"list_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.List,
                    NativeImGui.GetContentRegionAvail().X,
                    MathF.Max(1f, rowMaximumY - rowMinimumY));
            }
            else
            {
                EditorWidget.IconText(icon, name, false);
            }
        }
        finally
        {
            NativeImGui.PopStyleColor(3);
        }
    }

    private static void DrawTextCell(
        string text,
        Vector4 color
    ) {
        NativeImGui.TableNextColumn();
        InsetListCellContent();
        NativeImGui.PushStyleColor(ImGuiCol.Text, color);
        NativeImGui.TextUnformatted(text);
        NativeImGui.PopStyleColor();
    }

    private static void InsetListCellContent()
    {
        NativeImGui.SetCursorPosX(
            NativeImGui.GetCursorPosX() + EditorWidget.style.assetListContentHorizontalPadding);
    }

    private readonly record struct ListColumnSeparatorState(
        bool nameHovered,
        bool nameActive,
        bool typeHovered,
        bool typeActive
    );

}
