using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;

using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;

using EditorUI = Inno.Editor.ImGui.EditorUISession;
using Gui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;

internal sealed partial class FileBrowserPanel
{
    private void DrawListRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        bool entriesVisible = Gui.BeginChild("##EntriesScroll", Vector2.Zero, ImGuiChildFlags.None);
        try
        {
            if (entriesVisible)
            {
                if (Gui.IsWindowFocused())
                {
                    m_rename.MarkInteraction(FileBrowserPresentation.List);
                }
                float rowHeight = Gui.GetTextLineHeight() + Gui.GetLayout().cellPadding.Y * 2f + currentBrowserStyle.listRowSpacing;
                HandleEntryKeyboardNavigation(Math.Max(1, (int)(Gui.GetContentRegionAvailable().Y / rowHeight)));
                DrawEntriesTable(context, entries, m_assets.browser.currentDirectory);
                HandleBackgroundSelection(context);
                m_contextMenu.DrawBackground(context, "##asset_list_background_context", FileBrowserPresentation.List);
            }
        }
        finally
        {
            Gui.EndChild();
        }
    }

    private void DrawEntriesTable(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries,
        string currentDirectory
    ) {
        ImGuiTableFlags flags = ImGuiTableFlags.RowBg
                                | ImGuiTableFlags.NoPadOuterX
                                | ImGuiTableFlags.SizingStretchProp
                                | ImGuiTableFlags.NoSavedSettings;
        EditorUILayout style = Gui.GetLayout();
        Vector2 tableOrigin = Gui.GetCursorScreenPosition();
        Vector2 tableSize = Gui.GetContentRegionAvailable();
        Gui.PushStyleVar(ImGuiStyleVar.CellPadding, new Vector2(style.windowPadding.X, style.cellPadding.Y));
        try
        {
            bool tableStarted = Gui.BeginTable("##FileBrowserEntries", 3, flags, new Vector2(0f, 0f));
            if (!tableStarted)
            {
                Gui.Dummy(Vector2.Zero);
                return;
            }

            try
            {
                Gui.TableSetupColumn(
                    "Name",
                    (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize),
                    m_listNameSeparatorPosition
                );
                Gui.TableSetupColumn(
                    "Type",
                    (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize),
                    m_listTypeSeparatorPosition - m_listNameSeparatorPosition
                );
                Gui.TableSetupColumn(
                    "Source",
                    (ImGuiTableColumnFlags.WidthStretch | ImGuiTableColumnFlags.NoResize),
                    1f - m_listTypeSeparatorPosition
                );
                DrawHeaderRow();
                uint rowBg = Gui.ToPackedColor(EditorPalette.collectionRow);
                uint rowAltBg = Gui.ToPackedColor(EditorPalette.collectionRowAlternate);
                float entryRowHeight = Gui.GetTextLineHeight() + style.cellPadding.Y * 2f + currentBrowserStyle.listRowSpacing;
                Span<int> preserved = stackalloc int[3];
                int preservedCount = CollectPreservedEntryRows(entries, preserved);
                m_ui.widgets.ClippedRows(
                    entries.Count,
                    -1f,
                    (panel: this, context, entries, currentDirectory, rowBg, rowAltBg, entryRowHeight),
                    static (
                        state,
                        start,
                        end
                    ) =>
                {
                    for (int index = start; index < end; index++)
                    {
                        FileBrowserDisplayEntry item = state.entries[index];
                        Gui.TableNextRow(ImGuiTableRowFlags.None, state.entryRowHeight);
                        if (!state.panel.m_data.TryResolve(item, out AssetFileEntry? entry))
                        {
                            continue;
                        }
                        Gui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, index % 2 == 0 ? state.rowBg : state.rowAltBg);
                        state.panel.DrawNameCell(state.context, item, entry);
                        DrawTextCell(item.isPluginRoot ? "IPLUGIN" : GetTypeText(entry), EditorPalette.contentText);
                        DrawTextCell(item.isPluginRoot ? "~" : GetSourceText(entry, state.currentDirectory), EditorPalette.contentText);
                    }
                },
                    preserved[..preservedCount]
                );
            }
            finally
            {
                Gui.EndTable();
            }

            Vector2 tableEnd = Gui.GetCursorScreenPosition();
            float tableHeight = MathF.Max(1f, tableEnd.Y - tableOrigin.Y);
            var interactionSize = new Vector2(tableSize.X, tableHeight);
            ListColumnSeparatorState separators = HandleListColumnSeparators(tableOrigin, interactionSize);
            DrawListColumnSeparators(tableOrigin, tableSize.X, tableHeight, separators);
            Gui.SetCursorScreenPosition(tableEnd);
            // Commit the restored layout cursor so overlay separator hit targets cannot implicitly
            // extend the scrolling child when UI scaling changes their physical bounds.
            Gui.Dummy(Vector2.Zero);
        }
        finally
        {
            Gui.PopStyleVar();
        }
    }

    private ListColumnSeparatorState HandleListColumnSeparators(
        Vector2 origin,
        Vector2 size
    ) {
        float width = MathF.Max(1f, size.X);
        float height = MathF.Max(1f, size.Y);
        float hitWidth = currentBrowserStyle.listSeparatorHitWidth;
        bool nameHovered;
        bool nameActive;
        bool typeHovered;
        bool typeActive;
        float nameX = origin.X + width * m_listNameSeparatorPosition;
        Gui.SetCursorScreenPosition(new Vector2(nameX - hitWidth * 0.5f, origin.Y));
        Gui.SetNextItemAllowOverlap();
        _ = Gui.InvisibleButton("##AssetListNameSeparator", new Vector2(hitWidth, height));
        nameHovered = Gui.IsItemHovered();
        nameActive = Gui.IsItemActive();
        if (nameHovered || nameActive)
        {
            Gui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        }
        if (nameActive)
        {
            float requested = (Gui.GetMousePos().X - origin.X) / width;
            SetListColumnSeparators(requested, m_listTypeSeparatorPosition);
        }

        float typeX = origin.X + width * m_listTypeSeparatorPosition;
        Gui.SetCursorScreenPosition(new Vector2(typeX - hitWidth * 0.5f, origin.Y));
        Gui.SetNextItemAllowOverlap();
        _ = Gui.InvisibleButton("##AssetListTypeSeparator", new Vector2(hitWidth, height));
        typeHovered = Gui.IsItemHovered();
        typeActive = Gui.IsItemActive();
        if (typeHovered || typeActive)
        {
            Gui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        }
        if (typeActive)
        {
            float requested = (Gui.GetMousePos().X - origin.X) / width;
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

    private void SetListColumnSeparators(
        float namePosition,
        float typePosition
    ) {
        float minimum = browserStyle.listMinimumColumnRatio;
        m_listNameSeparatorPosition = Math.Clamp(namePosition, minimum, 1f - minimum * 2f);
        m_listTypeSeparatorPosition = Math.Clamp(typePosition, m_listNameSeparatorPosition + minimum, 1f - minimum);
    }

    private void DrawNameCell(
        EditorContext context,
        FileBrowserDisplayEntry item,
        AssetFileEntry entry
    ) {
        _ = Gui.TableSetColumnIndex(0);
        string icon = m_assets.GetIcon(entry);
        string name = item.isPluginRoot ? item.displayName : entry.nameWithoutExtension;
        bool selected = m_assets.browser.IsSelected(context, entry);
        bool editing = m_rename.IsEditing(context, entry.assetPath.ToString(), FileBrowserPresentation.List);
        (float minimumX, float maximumX, float rowMinimum, float rowMaximum) table = Gui.GetTableBounds();
        float rowMinimumY = table.rowMinimum;
        float rowMaximumY = table.rowMaximum;
        InsetListCellContent();
        Gui.PushStyleColor(ImGuiCol.Header, EditorPalette.transparent);
        Gui.PushStyleColor(ImGuiCol.HeaderHovered, EditorPalette.transparent);
        Gui.PushStyleColor(ImGuiCol.HeaderActive, EditorPalette.transparent);
        try
        {
            Vector2 iconTextPos = Gui.GetCursorScreenPosition();
            ImGuiSelectableFlags selectableFlags = ImGuiSelectableFlags.SpanAllColumns
                                                   | ImGuiSelectableFlags.AllowDoubleClick
                                                   | ImGuiSelectableFlags.AllowOverlap;
            RequestEntryKeyboardFocus(entry);
            bool activated = Gui.Selectable($"##entry_{entry.assetPath.ToString()}", selected, selectableFlags);
            bool itemHovered = Gui.IsItemHovered();
            bool doubleClicked = itemHovered && Gui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
            if (activated && !m_keyboardHandled)
            {
                HandleEntryActivation(context, entry, FileBrowserPresentation.List, doubleClicked);
            }

            bool itemActive = Gui.IsItemActive();
            if (!editing)
            {
                m_contextMenu.DrawEntry(
                    context,
                    $"##asset_context_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.List
                );
            }

            if (selected || itemHovered)
            {
                Vector4 highlight = itemActive ? EditorPalette.GetHovered(EditorPalette.contentAccent) : EditorPalette.contentAccent;
                Gui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, Gui.ToPackedColor(highlight));
            }

            if (!editing)
            {
                m_dragDrop.DrawAssetSource(context, entry);
                if (entry.isDirectory && !entry.isReadOnly)
                {
                    m_dragDrop.DrawDirectoryTarget(context, entry.assetPath.ToString());
                }
            }

            Gui.SameLine(iconTextPos.X - Gui.GetWindowPos().X, 0f);
            if (editing)
            {
                EditorUI.current.widgets.IconText(icon, string.Empty, false);
                Gui.SameLine(0f, 0f);
                Vector2 renameCursor = Gui.GetCursorScreenPosition();
                Gui.SetCursorScreenPosition(new Vector2(renameCursor.X, rowMinimumY));
                m_rename.Draw(
                    context,
                    $"list_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.List,
                    Gui.GetContentRegionAvailable().X,
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
            Gui.PopStyleColor(3);
        }
    }

    private static void DrawListColumnSeparator(
        float x,
        float top,
        float bottom,
        bool hovered,
        bool active
    ) {
        Vector4 color = active ? EditorPalette.contentAccent : hovered ? EditorPalette.contentBorder : EditorPalette.contentBorderSoft;
        Gui.DrawLine(new Vector2(x, top), new Vector2(x, bottom), Gui.ToPackedColor(color), EditorUI.current.widgets.style.borderSize);
    }

    private static void DrawHeaderRow()
    {
        Gui.TableNextRow();
        Gui.TableSetBgColor(ImGuiTableBgTarget.RowBg0, Gui.ToPackedColor(EditorPalette.collectionHeader));
        Gui.TableSetBgColor(ImGuiTableBgTarget.RowBg1, Gui.ToPackedColor(EditorPalette.collectionHeader));
        _ = Gui.TableSetColumnIndex(0);
        InsetListCellContent();
        Gui.Text("Name");
        _ = Gui.TableSetColumnIndex(1);
        InsetListCellContent();
        Gui.Text("Type");
        _ = Gui.TableSetColumnIndex(2);
        InsetListCellContent();
        Gui.Text("Source");
    }

    private static void DrawTextCell(
        string text,
        Vector4 color
    ) {
        Gui.TableNextColumn();
        InsetListCellContent();
        Gui.PushStyleColor(ImGuiCol.Text, color);
        Gui.Text(text);
        Gui.PopStyleColor();
    }

    private static void InsetListCellContent() => Gui.SetCursorPosX(Gui.GetCursorPosX() + currentBrowserStyle.listContentHorizontalPadding);

    private readonly record struct ListColumnSeparatorState(
        bool nameHovered,
        bool nameActive,
        bool typeHovered,
        bool typeActive
    );
}
