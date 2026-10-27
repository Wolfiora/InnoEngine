using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
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
using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;
internal sealed partial class FileBrowserPanel
{
    private void DrawListRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        bool entriesVisible = Ui.BeginChild("##EntriesScroll", Vector2.Zero, ImGuiChildFlags.None);
        try
        {
            if (entriesVisible)
            {
                float rowHeight = Ui.GetTextLineHeight() + Ui.GetLayout().cellPadding.Y * 2f + CurrentBrowserStyle.listRowSpacing;
                HandleEntryKeyboardNavigation(Math.Max(1, (int)(Ui.GetContentRegionAvailable().Y / rowHeight)));
                DrawEntriesTable(context, entries, m_assets.browser.currentDirectory);
                HandleBackgroundSelection(context);
                m_contextMenu.DrawBackground(context, "##asset_list_background_context", FileBrowserPresentation.List);
            }
        }
        finally
        {
            Ui.EndChild();
        }
    }

    private void DrawEntriesTable(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries,
        string currentDirectory
    ) {
        ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.SizingStretchProp | ImGuiTableFlags.NoSavedSettings;
        EditorUiLayout style = Ui.GetLayout();
        Vector2 tableOrigin = Ui.GetCursorScreenPosition();
        Vector2 tableSize = Ui.GetContentRegionAvailable();
        Ui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(style.windowPadding.X, style.cellPadding.Y));
        try
        {
            bool tableStarted = Ui.BeginTable("##FileBrowserEntries", 3, flags, new Vector2(0f, 0f));
            if (!tableStarted)
            {
                Ui.Dummy(Vector2.Zero);
                return;
            }

            try
            {
                Ui.TableSetupColumn("Name", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize), m_listNameSeparatorPosition);
                Ui.TableSetupColumn("Type", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize), m_listTypeSeparatorPosition - m_listNameSeparatorPosition);
                Ui.TableSetupColumn("Source", (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize), 1f - m_listTypeSeparatorPosition);
                DrawHeaderRow();
                uint rowBg = Ui.ToPackedColor(EditorPalette.collectionRow);
                uint rowAltBg = Ui.ToPackedColor(EditorPalette.collectionRowAlternate);
                float entryRowHeight = Ui.GetTextLineHeight() + style.cellPadding.Y * 2f + CurrentBrowserStyle.listRowSpacing;
                Span<int> preserved = stackalloc int[3];
                int preservedCount = CollectPreservedEntryRows(entries, preserved);
                m_ui.widgets.ClippedRows(entries.Count, -1f,
                    (panel: this, context, entries, currentDirectory, rowBg, rowAltBg, entryRowHeight), static (
                        state,
                        start,
                        end
                    ) =>
                {
                    for (int index = start; index < end; index++)
                    {
                        FileBrowserDisplayEntry item = state.entries[index];
                        Ui.TableNextRow(ImGuiTableRowFlags.None, state.entryRowHeight);
                        if (!state.panel.m_data.TryResolve(item, out AssetFileEntry? entry))
                            continue;
                        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, index % 2 == 0 ? state.rowBg : state.rowAltBg);
                        state.panel.DrawNameCell(state.context, item, entry);
                        DrawTextCell(item.isPluginRoot ? "IPLUGIN" : GetTypeText(entry), EditorPalette.contentText);
                        DrawTextCell(item.isPluginRoot ? "~" : GetSourceText(entry, state.currentDirectory), EditorPalette.contentText);
                    }
                }, preserved[..preservedCount]);
            }
            finally
            {
                Ui.EndTable();
            }

            Vector2 tableEnd = Ui.GetCursorScreenPosition();
            float tableHeight = MathF.Max(1f, tableEnd.Y - tableOrigin.Y);
            var interactionSize = new Vector2(tableSize.X, tableHeight);
            ListColumnSeparatorState separators = HandleListColumnSeparators(tableOrigin, interactionSize);
            DrawListColumnSeparators(tableOrigin, tableSize.X, tableHeight, separators);
            Ui.SetCursorScreenPosition(tableEnd);
            // Commit the restored layout cursor so overlay separator hit targets cannot implicitly
            // extend the scrolling child when UI scaling changes their physical bounds.
            Ui.Dummy(Vector2.Zero);
        }
        finally
        {
            Ui.PopStyleVar();
        }
    }

    private ListColumnSeparatorState HandleListColumnSeparators(
        Vector2 origin,
        Vector2 size
    ) {
        float width = MathF.Max(1f, size.X);
        float height = MathF.Max(1f, size.Y);
        float hitWidth = CurrentBrowserStyle.listSeparatorHitWidth;
        bool nameHovered;
        bool nameActive;
        bool typeHovered;
        bool typeActive;
        float nameX = origin.X + width * m_listNameSeparatorPosition;
        Ui.SetCursorScreenPosition(new Vector2(nameX - hitWidth * 0.5f, origin.Y));
        Ui.SetNextItemAllowOverlap();
        _ = Ui.InvisibleButton("##AssetListNameSeparator", new Vector2(hitWidth, height));
        nameHovered = Ui.IsItemHovered();
        nameActive = Ui.IsItemActive();
        if (nameHovered || nameActive)
            Ui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        if (nameActive)
        {
            float requested = (Ui.GetMousePos().X - origin.X) / width;
            SetListColumnSeparators(requested, m_listTypeSeparatorPosition);
        }

        float typeX = origin.X + width * m_listTypeSeparatorPosition;
        Ui.SetCursorScreenPosition(new Vector2(typeX - hitWidth * 0.5f, origin.Y));
        Ui.SetNextItemAllowOverlap();
        _ = Ui.InvisibleButton("##AssetListTypeSeparator", new Vector2(hitWidth, height));
        typeHovered = Ui.IsItemHovered();
        typeActive = Ui.IsItemActive();
        if (typeHovered || typeActive)
            Ui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        if (typeActive)
        {
            float requested = (Ui.GetMousePos().X - origin.X) / width;
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
        DrawListColumnSeparator(origin.X + width * m_listNameSeparatorPosition, origin.Y, bottom, state.nameHovered, state.nameActive);
        DrawListColumnSeparator(origin.X + width * m_listTypeSeparatorPosition, origin.Y, bottom, state.typeHovered, state.typeActive);
    }

    private static void DrawListColumnSeparator(
        float x,
        float top,
        float bottom,
        bool hovered,
        bool active
    ) {
        Vector4 color = active ? EditorPalette.contentAccent : hovered ? EditorPalette.contentBorder : EditorPalette.contentBorderSoft;
        Ui.DrawLine(new Vector2(x, top), new Vector2(x, bottom), Ui.ToPackedColor(color), EditorUI.current.widgets.style.borderSize);
    }

    private void SetListColumnSeparators(
        float namePosition,
        float typePosition
    ) {
        float minimum = browserStyle.listMinimumColumnRatio;
        m_listNameSeparatorPosition = Math.Clamp(namePosition, minimum, 1f - minimum * 2f);
        m_listTypeSeparatorPosition = Math.Clamp(typePosition, m_listNameSeparatorPosition + minimum, 1f - minimum);
    }

    private static void DrawHeaderRow()
    {
        Ui.TableNextRow();
        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, Ui.ToPackedColor(EditorPalette.collectionHeader));
        Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, Ui.ToPackedColor(EditorPalette.collectionHeader));
        _ = Ui.TableSetColumnIndex(0);
        InsetListCellContent();
        Ui.Text("Name");
        _ = Ui.TableSetColumnIndex(1);
        InsetListCellContent();
        Ui.Text("Type");
        _ = Ui.TableSetColumnIndex(2);
        InsetListCellContent();
        Ui.Text("Source");
    }

    private void DrawNameCell(
        EditorContext context,
        FileBrowserDisplayEntry item,
        AssetFileEntry entry
    ) {
        _ = Ui.TableSetColumnIndex(0);
        string icon = m_assets.GetIcon(entry);
        string name = item.isPluginRoot ? item.displayName : entry.nameWithoutExtension;
        bool selected = m_assets.browser.IsSelected(context, entry);
        bool editing = m_rename.IsEditing(context, entry.assetPath.ToString(), FileBrowserPresentation.List);
        var table = Ui.GetTableBounds();
        float rowMinimumY = table.rowMinimum;
        float rowMaximumY = table.rowMaximum;
        InsetListCellContent();
        Ui.PushStyleColor(ImGuiCol.Header, EditorPalette.transparent);
        Ui.PushStyleColor(ImGuiCol.HeaderHovered, EditorPalette.transparent);
        Ui.PushStyleColor(ImGuiCol.HeaderActive, EditorPalette.transparent);
        try
        {
            Vector2 iconTextPos = Ui.GetCursorScreenPosition();
            ImGuiSelectableFlags selectableFlags = ImGuiSelectableFlags.SpanAllColumns | ImGuiSelectableFlags.AllowDoubleClick | ImGuiSelectableFlags.AllowOverlap;
            RequestEntryKeyboardFocus(entry);
            bool activated = Ui.Selectable($"##entry_{entry.assetPath.ToString()}", selected, selectableFlags);
            bool itemHovered = Ui.IsItemHovered();
            bool doubleClicked = itemHovered && Ui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
            if (activated && !m_keyboardHandled)
            {
                HandleEntryActivation(context, entry, FileBrowserPresentation.List, doubleClicked);
            }

            bool itemActive = Ui.IsItemActive();
            if (!editing)
            {
                m_contextMenu.DrawEntry(context, $"##asset_context_{entry.assetPath.ToString()}", entry.assetPath.ToString(), FileBrowserPresentation.List);
            }

            if (selected || itemHovered)
            {
                Vector4 highlight = itemActive ? EditorPalette.GetHovered(EditorPalette.contentAccent) : EditorPalette.contentAccent;
                Ui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, Ui.ToPackedColor(highlight));
            }

            if (!editing)
            {
                m_dragDrop.DrawAssetSource(context, entry);
                if (entry.isDirectory && !entry.isReadOnly)
                    m_dragDrop.DrawDirectoryTarget(context, entry.assetPath.ToString());
            }

            Ui.SameLine(iconTextPos.X - Ui.GetWindowPos().X, 0f);
            if (editing)
            {
                EditorUI.current.widgets.IconText(icon, string.Empty, false);
                Ui.SameLine(0f, 0f);
                Vector2 renameCursor = Ui.GetCursorScreenPosition();
                Ui.SetCursorScreenPosition(new Vector2(renameCursor.X, rowMinimumY));
                m_rename.Draw(
                    context,
                    $"list_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.List,
                    Ui.GetContentRegionAvailable().X,
                    MathF.Max(1f, rowMaximumY - rowMinimumY)
                );
            }
            else
            {
                EditorUI.current.widgets.IconText(icon, name, false);
            }
        }
        finally
        {
            Ui.PopStyleColor(3);
        }
    }

    private static void DrawTextCell(
        string text,
        Vector4 color
    ) {
        Ui.TableNextColumn();
        InsetListCellContent();
        Ui.PushStyleColor(ImGuiCol.Text, color);
        Ui.Text(text);
        Ui.PopStyleColor();
    }

    private static void InsetListCellContent()
    {
        Ui.SetCursorPosX(Ui.GetCursorPosX() + CurrentBrowserStyle.listContentHorizontalPadding);
    }

    private readonly record struct ListColumnSeparatorState(
        bool nameHovered,
        bool nameActive,
        bool typeHovered,
        bool typeActive
    );
}
