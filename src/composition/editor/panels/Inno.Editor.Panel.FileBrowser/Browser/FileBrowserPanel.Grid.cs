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
    private void DrawGridRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        EditorUiLayout style = Ui.GetLayout();
        float sliderHeight = Ui.GetFrameHeight() + style.windowPadding.Y * 2f + style.itemSpacing.Y;
        bool entriesVisible = Ui.BeginChild("##EntriesScroll", new Vector2(0f, -sliderHeight), ImGuiChildFlags.None);
        try
        {
            if (entriesVisible)
            {
                float cellSize = GetGridCellSize();
                int columns = Math.Max(1, (int)(Ui.GetContentRegionAvailable().X / cellSize));
                int rows = Math.Max(1, (int)(Ui.GetContentRegionAvailable().Y / cellSize));
                HandleEntryKeyboardNavigation(columns * rows);
                DrawGrid(context, entries);
                HandleBackgroundSelection(context);
                m_contextMenu.DrawBackground(context, "##asset_grid_background_context", FileBrowserPresentation.Grid);
            }
        }
        finally
        {
            Ui.EndChild();
        }

        DrawGridScaleSlider();
    }

    private void HandleBackgroundSelection(EditorContext context)
    {
        if (!Ui.IsWindowHovered() || Ui.IsAnyItemHovered() || !Ui.IsMouseReleased(ImGuiMouseButton.Left))
        {
            return;
        }

        m_assets.browser.Select(context, null);
    }

    private void DrawGridScaleSlider()
    {
        m_gridScale = Math.Clamp(m_gridScale, CurrentBrowserStyle.gridMinimumScale, CurrentBrowserStyle.gridMaximumScale);
        DrawFooterTopSplitter();
        Vector2 cursor = Ui.GetCursorPosition();
        float labelOffsetY = MathF.Max(0f, (Ui.GetFrameHeight() - Ui.GetTextLineHeight()) * 0.5f);
        Ui.SetCursorPosY(cursor.Y + labelOffsetY);
        Ui.Text("Scale");
        Ui.SameLine(0f, -1f);
        Ui.SetCursorPosY(cursor.Y);
        Ui.SetNextItemWidth(-1f);
        _ = Ui.SliderFloat("##GridScale", ref m_gridScale, CurrentBrowserStyle.gridMinimumScale, CurrentBrowserStyle.gridMaximumScale);
        m_gridScale = Math.Clamp(m_gridScale, CurrentBrowserStyle.gridMinimumScale, CurrentBrowserStyle.gridMaximumScale);
    }

    private void DrawGrid(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        float cellSize = GetGridCellSize();
        float available = MathF.Max(cellSize, Ui.GetContentRegionAvailable().X);
        int columns = Math.Max(1, (int)(available / cellSize));
        bool tableStarted = Ui.BeginTable(
            "##FileBrowserGrid",
            columns,
            (ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.NoSavedSettings),
            default(System.Numerics.Vector2),
            0f
        );
        if (!tableStarted)
            return;
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
                    Ui.TableNextRow();
                    for (int column = 0; column < state.columns; column++)
                    {
                        int index = row * state.columns + column;
                        if (index >= state.entries.Count)
                            break;
                        _ = Ui.TableSetColumnIndex(column);
                        state.panel.DrawGridItem(state.context, state.entries[index]);
                    }
                }
            }, preserved[..preservedCount]);
        }
        finally
        {
            Ui.EndTable();
        }
    }

    private void DrawGridItem(
        EditorContext context,
        FileBrowserDisplayEntry item
    ) {
        float cellSize = GetGridCellSize();
        if (!m_data.TryResolve(item, out AssetFileEntry? entry))
        {
            Ui.Dummy(new(cellSize, cellSize));
            return;
        }
        string icon = m_assets.GetIcon(entry);
        string name = item.isPluginRoot ? item.displayName : entry.name;
        bool selected = m_assets.browser.IsSelected(context, entry);
        bool editing = m_rename.IsEditing(context, entry.assetPath.ToString(), FileBrowserPresentation.Grid);
        Vector2 itemSize = new(cellSize - CurrentBrowserStyle.gridCellPadding, cellSize - CurrentBrowserStyle.gridCellPadding);
        Ui.PushId(entry.assetPath.ToString());
        Ui.PushStyleColor(ImGuiCol.Header, EditorPalette.transparent);
        Ui.PushStyleColor(ImGuiCol.HeaderHovered, EditorPalette.transparent);
        Ui.PushStyleColor(ImGuiCol.HeaderActive, EditorPalette.transparent);
        try
        {
            ImGuiSelectableFlags selectableFlags = ImGuiSelectableFlags.AllowDoubleClick;
            if (editing)
                selectableFlags |= ImGuiSelectableFlags.AllowOverlap;
            RequestEntryKeyboardFocus(entry);
            bool activated = Ui.Selectable("##GridItem", selected, selectableFlags, itemSize);
            bool itemHovered = Ui.IsItemHovered();
            bool itemActive = Ui.IsItemActive();
            Vector2 itemMin = Ui.GetItemMinimum();
            Vector2 itemMax = Ui.GetItemMaximum();
            Vector2 layoutCursor = Ui.GetCursorScreenPosition();
            bool doubleClicked = itemHovered && Ui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
            if (activated && !m_keyboardHandled)
            {
                HandleEntryActivation(context, entry, FileBrowserPresentation.Grid, doubleClicked);
            }

            if (!editing)
            {
                m_dragDrop.DrawAssetSource(context, entry);
                m_contextMenu.DrawEntry(context, "##asset_grid_context", entry.assetPath.ToString(), FileBrowserPresentation.Grid);
                if (entry.isDirectory && !entry.isReadOnly)
                    m_dragDrop.DrawDirectoryTarget(context, entry.assetPath.ToString());
            }

            DrawGridItemVisual(icon, name, selected, m_gridScale, itemHovered, itemActive, itemMin, itemMax, drawName: !editing);
            if (editing)
            {
                float width = MathF.Max(1f, itemMax.X - itemMin.X - CurrentBrowserStyle.gridLabelHorizontalPadding);
                float lineHeight = Ui.GetTextLineHeight();
                float lineAdvance = MathF.Max(1f, lineHeight + CurrentBrowserStyle.gridLabelLineSpacing);
                float labelAreaHeight = lineHeight + lineAdvance * (C_GRID_LABEL_LINE_COUNT - 1);
                float labelAreaTop = itemMax.Y - CurrentBrowserStyle.gridLabelBottomPadding - labelAreaHeight;
                float y = labelAreaTop + (labelAreaHeight - lineHeight) * 0.5f;
                Ui.SetCursorScreenPosition(new Vector2(itemMin.X + CurrentBrowserStyle.gridLabelHorizontalPadding * 0.5f, y));
                m_rename.Draw(context, $"grid_{entry.assetPath.ToString()}", entry.assetPath.ToString(), FileBrowserPresentation.Grid, width, lineHeight);
                Ui.SetCursorScreenPosition(layoutCursor);
                Ui.Dummy(Vector2.Zero);
            }
        }
        finally
        {
            Ui.PopStyleColor(3);
            Ui.PopId();
        }
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
            bg = EditorPalette.GetActive(bg);
        else if (hovered)
            bg = EditorPalette.GetHovered(bg);
        uint bgColor = Ui.ToPackedColor(bg);
        Ui.DrawFilledRectangle(min, max, bgColor, CurrentBrowserStyle.frameRounding);
        float fontSize = Ui.GetFontSize();
        float iconFontSize = fontSize * scale;
        Vector4 iconBounds = EditorUI.current.widgets.GetGlyphVisualBounds(iconFontSize, icon);
        Vector2 iconSize = new(iconBounds.Z - iconBounds.X, iconBounds.W - iconBounds.Y);
        string[] nameLines = FitTextToLines(name, MathF.Max(1f, size.X - CurrentBrowserStyle.gridLabelHorizontalPadding), C_GRID_LABEL_LINE_COUNT);
        float lineHeight = Ui.GetTextLineHeight();
        float lineAdvance = MathF.Max(1f, lineHeight + CurrentBrowserStyle.gridLabelLineSpacing);
        float labelAreaHeight = lineHeight + lineAdvance * (C_GRID_LABEL_LINE_COUNT - 1);
        float labelAreaTop = max.Y - CurrentBrowserStyle.gridLabelBottomPadding - labelAreaHeight;
        float labelHeight = lineHeight + lineAdvance * Math.Max(0, nameLines.Length - 1);
        float labelY = labelAreaTop + (labelAreaHeight - labelHeight) * 0.5f;
        float iconAreaTop = min.Y + CurrentBrowserStyle.gridIconTopPadding;
        float iconAreaBottom = MathF.Max(iconAreaTop + 1f, labelAreaTop - CurrentBrowserStyle.gridIconLabelSpacing);
        float maximumIconWidth = MathF.Max(1f, size.X - CurrentBrowserStyle.gridIconHorizontalPadding * 2f);
        float maximumIconHeight = MathF.Max(1f, iconAreaBottom - iconAreaTop);
        float fit = MathF.Min(1f, MathF.Min(maximumIconWidth / MathF.Max(1f, iconSize.X), maximumIconHeight / MathF.Max(1f, iconSize.Y)));
        if (fit < 1f)
        {
            iconFontSize *= fit;
            iconBounds = EditorUI.current.widgets.GetGlyphVisualBounds(iconFontSize, icon);
            iconSize = new(iconBounds.Z - iconBounds.X, iconBounds.W - iconBounds.Y);
        }

        float horizontalCenter = min.X + size.X * 0.5f;
        Vector2 iconAreaCenter = new(horizontalCenter + C_GRID_ICON_HORIZONTAL_OPTICAL_OFFSET * EditorUI.current.widgets.style.zoom, iconAreaTop + maximumIconHeight * 0.5f);
        uint textColor = Ui.ToPackedColor(EditorPalette.contentText);
        Ui.PushClipRect(min, max, true);
        EditorUI.current.widgets.AddGlyphCentered(iconFontSize, icon, iconAreaCenter, textColor);
        for (int i = 0; drawName && i < nameLines.Length; i++)
        {
            Vector2 lineSize = Ui.MeasureText(nameLines[i]);
            Vector2 linePosition = new(horizontalCenter - lineSize.X * 0.5f, labelY + lineAdvance * i);
            Ui.DrawText(linePosition, textColor, nameLines[i]);
        }

        Ui.PopClipRect();
    }

    private float GetGridCellSize()
    {
        float fontSize = Ui.GetFontSize();
        return MathF.Max(
            CurrentBrowserStyle.gridMinimumCellSize,
            fontSize * (m_gridScale + CurrentBrowserStyle.gridScaleBias) + CurrentBrowserStyle.gridFixedCellPadding
        );
    }
}
