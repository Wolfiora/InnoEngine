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

    private void DrawGridRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        ImGuiStylePtr style = NativeImGui.GetStyle();
        float sliderHeight = NativeImGui.GetFrameHeight() + style.WindowPadding.Y * 2f + style.ItemSpacing.Y;
        bool entriesVisible = NativeImGui.BeginChild(
            "##EntriesScroll",
            new Vector2(0f, -sliderHeight),
            ImGuiChildFlags.None);
        try
        {
            if (entriesVisible)
            {
                DrawGrid(context, entries);
                HandleBackgroundSelection(context);
                m_contextMenu.DrawBackground(
                    context,
                    "##asset_grid_background_context",
                    FileBrowserPresentation.Grid);
            }
        }
        finally
        {
            NativeImGui.EndChild();
        }
        DrawGridScaleSlider();
    }

    private void HandleBackgroundSelection(EditorContext context)
    {
        if (!NativeImGui.IsWindowHovered() ||
            NativeImGui.IsAnyItemHovered() ||
            !NativeImGui.IsMouseReleased(ImGuiMouseButton.Left))
        {
            return;
        }

        m_assets.browser.Select(context, null);
    }

    private void DrawGridScaleSlider()
    {
        m_gridScale = Math.Clamp(m_gridScale, EditorWidget.style.assetGridMinimumScale, EditorWidget.style.assetGridMaximumScale);
        DrawFooterTopSplitter();
        Vector2 cursor = NativeImGui.GetCursorPos();
        float labelOffsetY = MathF.Max(0f, (NativeImGui.GetFrameHeight() - NativeImGui.GetTextLineHeight()) * 0.5f);
        NativeImGui.SetCursorPosY(cursor.Y + labelOffsetY);
        NativeImGui.TextUnformatted("Scale");
        NativeImGui.SameLine();
        NativeImGui.SetCursorPosY(cursor.Y);
        NativeImGui.SetNextItemWidth(-1f);
        _ = NativeImGui.SliderFloat("##GridScale", ref m_gridScale, EditorWidget.style.assetGridMinimumScale, EditorWidget.style.assetGridMaximumScale, "%.1f");
        m_gridScale = Math.Clamp(m_gridScale, EditorWidget.style.assetGridMinimumScale, EditorWidget.style.assetGridMaximumScale);
    }

    private void DrawGrid(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        float cellSize = GetGridCellSize();
        float available = MathF.Max(cellSize, NativeImGui.GetContentRegionAvail().X);
        int columns = Math.Max(1, (int)(available / cellSize));

        bool tableStarted = NativeImGui.BeginTable(
            "##FileBrowserGrid",
            columns,
            ImGuiTableFlags.SizingFixedFit |
            ImGuiTableFlags.NoPadOuterX |
            ImGuiTableFlags.NoSavedSettings);
        if (!tableStarted)
            return;
        try
        {
            for (int i = 0; i < entries.Count; i++)
            {
                NativeImGui.TableNextColumn();
                DrawGridItem(context, entries[i]);
            }
        }
        finally
        {
            NativeImGui.EndTable();
        }
    }

    private void DrawGridItem(
        EditorContext context,
        FileBrowserDisplayEntry item
    ) {
        AssetFileEntry entry = item.entry;
        float cellSize = GetGridCellSize();
        string icon = m_assets.GetIcon(entry);
        string name = item.isPluginRoot ? item.displayName : entry.name;
        bool selected = m_assets.browser.IsSelected(context, entry);
        bool editing = m_rename.IsEditing(context, entry.assetPath.ToString(), FileBrowserPresentation.Grid);
        Vector2 itemSize = new(cellSize - EditorWidget.style.assetGridCellPadding, cellSize - EditorWidget.style.assetGridCellPadding);

        NativeImGui.PushID(entry.assetPath.ToString());
        NativeImGui.PushStyleColor(ImGuiCol.Header, EditorPalette.transparent);
        NativeImGui.PushStyleColor(ImGuiCol.HeaderHovered, EditorPalette.transparent);
        NativeImGui.PushStyleColor(ImGuiCol.HeaderActive, EditorPalette.transparent);
        try
        {
            ImGuiSelectableFlags selectableFlags = ImGuiSelectableFlags.AllowDoubleClick;
            if (editing)
                selectableFlags |= ImGuiSelectableFlags.AllowOverlap;
            bool activated = NativeImGui.Selectable(
                "##GridItem",
                selected,
                selectableFlags,
                itemSize);
            bool itemHovered = NativeImGui.IsItemHovered();
            bool itemActive = NativeImGui.IsItemActive();
            Vector2 itemMin = NativeImGui.GetItemRectMin();
            Vector2 itemMax = NativeImGui.GetItemRectMax();
            Vector2 layoutCursor = NativeImGui.GetCursorScreenPos();
            bool doubleClicked = itemHovered &&
                                 NativeImGui.IsMouseDoubleClicked(ImGuiMouseButton.Left);
            if (activated)
            {
                HandleEntryActivation(
                    context,
                    entry,
                    FileBrowserPresentation.Grid,
                    doubleClicked);
            }

            if (!editing)
            {
                m_dragDrop.DrawAssetSource(context, entry);
                m_contextMenu.DrawEntry(
                    context,
                    "##asset_grid_context",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.Grid);
                if (entry.isDirectory && !entry.isReadOnly)
                    m_dragDrop.DrawDirectoryTarget(context, entry.assetPath.ToString());
            }
            DrawGridItemVisual(
                icon,
                name,
                selected,
                m_gridScale,
                itemHovered,
                itemActive,
                itemMin,
                itemMax,
                drawName: !editing);
            if (editing)
            {
                float width = MathF.Max(
                    1f,
                    itemMax.X - itemMin.X - EditorWidget.style.assetGridLabelHorizontalPadding);
                float lineHeight = NativeImGui.GetTextLineHeight();
                float lineAdvance = MathF.Max(
                    1f,
                    lineHeight + EditorWidget.style.assetGridLabelLineSpacing);
                float labelAreaHeight = lineHeight +
                                        lineAdvance * (C_GRID_LABEL_LINE_COUNT - 1);
                float labelAreaTop = itemMax.Y -
                                     EditorWidget.style.assetGridLabelBottomPadding -
                                     labelAreaHeight;
                float y = labelAreaTop +
                          (labelAreaHeight - lineHeight) * 0.5f;
                NativeImGui.SetCursorScreenPos(new Vector2(
                    itemMin.X + EditorWidget.style.assetGridLabelHorizontalPadding * 0.5f,
                    y));
                m_rename.Draw(
                    context,
                    $"grid_{entry.assetPath.ToString()}",
                    entry.assetPath.ToString(),
                    FileBrowserPresentation.Grid,
                    width,
                    lineHeight);
                NativeImGui.SetCursorScreenPos(layoutCursor);
                NativeImGui.Dummy(Vector2.Zero);
            }
        }
        finally
        {
            NativeImGui.PopStyleColor(3);
            NativeImGui.PopID();
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

        Vector4 bg = selected ? EditorPalette.assetAccent : EditorPalette.collectionHeader;
        if (active)
            bg = EditorPalette.GetActive(bg);
        else if (hovered)
            bg = EditorPalette.GetHovered(bg);

        uint bgColor = NativeImGui.ColorConvertFloat4ToU32(bg);
        ImDrawListPtr drawList = NativeImGui.GetWindowDrawList();
        drawList.AddRectFilled(min, max, bgColor, EditorWidget.style.assetFrameRounding);

        ImFontPtr font = NativeImGui.GetFont();
        float fontSize = NativeImGui.GetFontSize();
        float iconFontSize = fontSize * scale;
        Vector4 iconBounds = EditorWidget.GetGlyphVisualBounds(font, iconFontSize, icon);
        Vector2 iconSize = new(
            iconBounds.Z - iconBounds.X,
            iconBounds.W - iconBounds.Y);
        string[] nameLines = FitTextToLines(
            name,
            MathF.Max(1f, size.X - EditorWidget.style.assetGridLabelHorizontalPadding),
            C_GRID_LABEL_LINE_COUNT);
        float lineHeight = NativeImGui.GetTextLineHeight();
        float lineAdvance = MathF.Max(
            1f,
            lineHeight + EditorWidget.style.assetGridLabelLineSpacing);
        float labelAreaHeight = lineHeight +
                                lineAdvance * (C_GRID_LABEL_LINE_COUNT - 1);
        float labelAreaTop = max.Y -
                             EditorWidget.style.assetGridLabelBottomPadding -
                             labelAreaHeight;
        float labelHeight = lineHeight +
                            lineAdvance * Math.Max(0, nameLines.Length - 1);
        float labelY = labelAreaTop + (labelAreaHeight - labelHeight) * 0.5f;
        float iconAreaTop = min.Y + EditorWidget.style.assetGridIconTopPadding;
        float iconAreaBottom = MathF.Max(
            iconAreaTop + 1f,
            labelAreaTop - EditorWidget.style.assetGridIconLabelSpacing);
        float maximumIconWidth = MathF.Max(
            1f,
            size.X - EditorWidget.style.assetGridIconHorizontalPadding * 2f);
        float maximumIconHeight = MathF.Max(1f, iconAreaBottom - iconAreaTop);
        float fit = MathF.Min(
            1f,
            MathF.Min(
                maximumIconWidth / MathF.Max(1f, iconSize.X),
                maximumIconHeight / MathF.Max(1f, iconSize.Y)));
        if (fit < 1f)
        {
            iconFontSize *= fit;
            iconBounds = EditorWidget.GetGlyphVisualBounds(font, iconFontSize, icon);
            iconSize = new(
                iconBounds.Z - iconBounds.X,
                iconBounds.W - iconBounds.Y);
        }

        float horizontalCenter = min.X + size.X * 0.5f;
        Vector2 iconAreaCenter = new(
            horizontalCenter +
            C_GRID_ICON_HORIZONTAL_OPTICAL_OFFSET * EditorWidget.style.zoom,
            iconAreaTop + maximumIconHeight * 0.5f);

        uint textColor = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.assetText);
        NativeImGui.PushClipRect(min, max, true);
        EditorWidget.AddGlyphCentered(
            drawList,
            font,
            iconFontSize,
            icon,
            iconAreaCenter,
            textColor);
        for (int i = 0; drawName && i < nameLines.Length; i++)
        {
            Vector2 lineSize = NativeImGui.CalcTextSize(nameLines[i]);
            Vector2 linePosition = new(
                horizontalCenter - lineSize.X * 0.5f,
                labelY + lineAdvance * i);
            NativeImGui.AddText(drawList, linePosition, textColor, nameLines[i]);
        }
        NativeImGui.PopClipRect();
    }

    private float GetGridCellSize()
    {
        float fontSize = NativeImGui.GetFontSize();
        return MathF.Max(
            EditorWidget.style.assetGridMinimumCellSize,
            fontSize * (m_gridScale + EditorWidget.style.assetGridScaleBias) +
            EditorWidget.style.assetGridFixedCellPadding);
    }

}
