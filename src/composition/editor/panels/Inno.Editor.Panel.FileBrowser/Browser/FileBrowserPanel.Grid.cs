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
    private void DrawGridRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        EditorUILayout style = Gui.GetLayout();
        float sliderHeight = Gui.GetFrameHeight() + style.windowPadding.Y * 2f + style.itemSpacing.Y;
        bool entriesVisible = Gui.BeginChild("##EntriesScroll", new Vector2(0f, -sliderHeight), ImGuiChildFlags.None);
        try
        {
            if (entriesVisible)
            {
                if (Gui.IsWindowFocused())
                {
                    m_rename.MarkInteraction(FileBrowserPresentation.Grid);
                }
                float cellSize = GetGridCellSize();
                int columns = Math.Max(1, (int)(Gui.GetContentRegionAvailable().X / cellSize));
                int rows = Math.Max(1, (int)(Gui.GetContentRegionAvailable().Y / cellSize));
                HandleEntryKeyboardNavigation(columns * rows);
                DrawGrid(context, entries);
                HandleBackgroundSelection(context);
                m_contextMenu.DrawBackground(context, "##asset_grid_background_context", FileBrowserPresentation.Grid);
            }
        }
        finally
        {
            Gui.EndChild();
        }

        DrawGridScaleSlider();
    }

    private void HandleBackgroundSelection(EditorContext context)
    {
        if (!Gui.IsWindowHovered() || Gui.IsAnyItemHovered() || !Gui.IsMouseReleased(ImGuiMouseButton.Left))
        {
            return;
        }

        m_assets.browser.Select(context, null);
    }

    private void DrawGridScaleSlider()
    {
        m_gridScale = Math.Clamp(m_gridScale, currentBrowserStyle.gridMinimumScale, currentBrowserStyle.gridMaximumScale);
        DrawFooterTopSplitter();
        Vector2 cursor = Gui.GetCursorPosition();
        float labelOffsetY = MathF.Max(0f, (Gui.GetFrameHeight() - Gui.GetTextLineHeight()) * 0.5f);
        Gui.SetCursorPosY(cursor.Y + labelOffsetY);
        Gui.Text("Scale");
        Gui.SameLine(0f, -1f);
        Gui.SetCursorPosY(cursor.Y);
        Gui.SetNextItemWidth(-1f);
        _ = Gui.SliderFloat("##GridScale", ref m_gridScale, currentBrowserStyle.gridMinimumScale, currentBrowserStyle.gridMaximumScale);
        m_gridScale = Math.Clamp(m_gridScale, currentBrowserStyle.gridMinimumScale, currentBrowserStyle.gridMaximumScale);
    }

    private void DrawGrid(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        float cellSize = GetGridCellSize();
        float available = MathF.Max(cellSize, Gui.GetContentRegionAvailable().X);
        int columns = Math.Max(1, (int)(available / cellSize));
        bool tableStarted = Gui.BeginTable(
            "##FileBrowserGrid",
            columns,
            (ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.NoSavedSettings),
            default,
            0f
        );
        if (!tableStarted)
        {
            return;
        }
        try
        {
            int rowCount = (entries.Count + columns - 1) / columns;
            Span<int> preserved = stackalloc int[3];
            int preservedCount = CollectPreservedEntryRows(entries, preserved, columns);
            m_ui.widgets.ClippedRows(rowCount, -1f, (panel: this, context, entries, columns), static (
                state,
                start,
                end
            ) =>
            {
                for (int row = start; row < end; row++)
                {
                    Gui.TableNextRow();
                    for (int column = 0; column < state.columns; column++)
                    {
                        int index = row * state.columns + column;
                        if (index >= state.entries.Count)
                        {
                            break;
                        }
                        _ = Gui.TableSetColumnIndex(column);
                        state.panel.DrawGridItem(state.context, state.entries[index]);
                    }
                }
            }, preserved[..preservedCount]);
        }
        finally
        {
            Gui.EndTable();
        }
    }

    private void DrawGridItem(
        EditorContext context,
        FileBrowserDisplayEntry item
    ) {
        float cellSize = GetGridCellSize();
        if (!m_data.TryResolve(item, out AssetFileEntry? entry))
        {
            Gui.Dummy(new Vector2(cellSize, cellSize));
            return;
        }
        string icon = m_assets.GetIcon(entry);
        string name = item.isPluginRoot ? item.displayName : entry.name;
        bool selected = m_assets.browser.IsSelected(context, entry);
        bool editing = m_rename.IsEditing(context, entry.assetPath.ToString(), FileBrowserPresentation.Grid);
        var itemSize = new Vector2(cellSize - currentBrowserStyle.gridCellPadding, cellSize - currentBrowserStyle.gridCellPadding);
        Gui.PushID(entry.assetPath.ToString());
        Gui.PushStyleColor(ImGuiCol.Header, EditorPalette.transparent);
        Gui.PushStyleColor(ImGuiCol.HeaderHovered, EditorPalette.transparent);
        Gui.PushStyleColor(ImGuiCol.HeaderActive, EditorPalette.transparent);
        try
        {
            ImGuiSelectableFlags selectableFlags = ImGuiSelectableFlags.AllowDoubleClick;
            if (editing)
            {
                selectableFlags |= ImGuiSelectableFlags.AllowOverlap;
            }
            RequestEntryKeyboardFocus(entry);
            bool activated = Gui.Selectable("##GridItem", selected, selectableFlags, itemSize);
            bool itemHovered = Gui.IsItemHovered();
            bool itemActive = Gui.IsItemActive();
            Vector2 itemMin = Gui.GetItemMinimum();
            Vector2 itemMax = Gui.GetItemMaximum();
            Vector2 layoutCursor = Gui.GetCursorScreenPosition();
            bool doubleClicked = itemHovered && Gui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
            if (activated && !m_keyboardHandled)
            {
                HandleEntryActivation(context, entry, FileBrowserPresentation.Grid, doubleClicked);
            }

            if (!editing)
            {
                m_dragDrop.DrawAssetSource(context, entry);
                m_contextMenu.DrawEntry(context, "##asset_grid_context", entry.assetPath.ToString(), FileBrowserPresentation.Grid);
                if (entry.isDirectory && !entry.isReadOnly)
                {
                    m_dragDrop.DrawDirectoryTarget(context, entry.assetPath.ToString());
                }
            }

            DrawGridItemVisual(icon, name, selected, m_gridScale, itemHovered, itemActive, itemMin, itemMax, drawName: !editing);
            if (editing)
            {
                float width = MathF.Max(1f, itemMax.X - itemMin.X - currentBrowserStyle.gridLabelHorizontalPadding);
                float lineHeight = Gui.GetTextLineHeight();
                float lineAdvance = MathF.Max(1f, lineHeight + currentBrowserStyle.gridLabelLineSpacing);
                float labelAreaHeight = lineHeight + lineAdvance * (C_GRID_LABEL_LINE_COUNT - 1);
                float labelAreaTop = itemMax.Y - currentBrowserStyle.gridLabelBottomPadding - labelAreaHeight;
                float y = labelAreaTop + (labelAreaHeight - lineHeight) * 0.5f;
                Gui.SetCursorScreenPosition(new Vector2(itemMin.X + currentBrowserStyle.gridLabelHorizontalPadding * 0.5f, y));
                m_rename.Draw(
                    context,
                    $"grid_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.Grid,
                    width,
                    lineHeight
                );
                Gui.SetCursorScreenPosition(layoutCursor);
                Gui.Dummy(Vector2.Zero);
            }
        }
        finally
        {
            Gui.PopStyleColor(3);
            Gui.PopID();
        }
    }

    private float GetGridCellSize()
    {
        float fontSize = Gui.GetFontSize();
        return MathF.Max(
            currentBrowserStyle.gridMinimumCellSize,
            fontSize * (m_gridScale + currentBrowserStyle.gridScaleBias) + currentBrowserStyle.gridFixedCellPadding
        );
    }

    private static void DrawGridItemVisual(
        string icon,
        string name,
        bool selected,
        float scale,
        bool hovered,
        bool active,
        Vector2 min,
        Vector2 max,
        bool drawName
    ) {
        Vector2 size = max - min;
        Vector4 bg = selected ? EditorPalette.contentAccent : EditorPalette.collectionHeader;
        if (active)
        {
            bg = EditorPalette.GetActive(bg);
        }
        else if (hovered)
        {
            bg = EditorPalette.GetHovered(bg);
        }
        uint bgColor = Gui.ToPackedColor(bg);
        Gui.DrawFilledRectangle(min, max, bgColor, currentBrowserStyle.frameRounding);
        float fontSize = Gui.GetFontSize();
        float iconFontSize = fontSize * scale;
        Vector4 iconBounds = EditorUI.current.widgets.GetGlyphVisualBounds(iconFontSize, icon);
        var iconSize = new Vector2(iconBounds.Z - iconBounds.X, iconBounds.W - iconBounds.Y);
        string[] nameLines = FitTextToLines(
            name,
            MathF.Max(1f, size.X - currentBrowserStyle.gridLabelHorizontalPadding),
            C_GRID_LABEL_LINE_COUNT
        );
        float lineHeight = Gui.GetTextLineHeight();
        float lineAdvance = MathF.Max(1f, lineHeight + currentBrowserStyle.gridLabelLineSpacing);
        float labelAreaHeight = lineHeight + lineAdvance * (C_GRID_LABEL_LINE_COUNT - 1);
        float labelAreaTop = max.Y - currentBrowserStyle.gridLabelBottomPadding - labelAreaHeight;
        float labelHeight = lineHeight + lineAdvance * Math.Max(0, nameLines.Length - 1);
        float labelY = labelAreaTop + (labelAreaHeight - labelHeight) * 0.5f;
        float iconAreaTop = min.Y + currentBrowserStyle.gridIconTopPadding;
        float iconAreaBottom = MathF.Max(iconAreaTop + 1f, labelAreaTop - currentBrowserStyle.gridIconLabelSpacing);
        float maximumIconWidth = MathF.Max(1f, size.X - currentBrowserStyle.gridIconHorizontalPadding * 2f);
        float maximumIconHeight = MathF.Max(1f, iconAreaBottom - iconAreaTop);
        float fit = MathF.Min(1f, MathF.Min(maximumIconWidth / MathF.Max(1f, iconSize.X), maximumIconHeight / MathF.Max(1f, iconSize.Y)));
        if (fit < 1f)
        {
            iconFontSize *= fit;
            iconBounds = EditorUI.current.widgets.GetGlyphVisualBounds(iconFontSize, icon);
            iconSize = new(iconBounds.Z - iconBounds.X, iconBounds.W - iconBounds.Y);
        }

        float horizontalCenter = min.X + size.X * 0.5f;
        var iconAreaCenter = new Vector2(
            horizontalCenter + C_GRID_ICON_HORIZONTAL_OPTICAL_OFFSET * EditorUI.current.widgets.style.zoom,
            iconAreaTop + maximumIconHeight * 0.5f
        );
        uint textColor = Gui.ToPackedColor(EditorPalette.contentText);
        Gui.PushClipRect(min, max, true);
        EditorUI.current.widgets.AddGlyphCentered(iconFontSize, icon, iconAreaCenter, textColor);
        for (int i = 0; drawName && i < nameLines.Length; i++)
        {
            Vector2 lineSize = Gui.MeasureText(nameLines[i]);
            var linePosition = new Vector2(horizontalCenter - lineSize.X * 0.5f, labelY + lineAdvance * i);
            Gui.DrawText(linePosition, textColor, nameLines[i]);
        }

        Gui.PopClipRect();
    }
}
