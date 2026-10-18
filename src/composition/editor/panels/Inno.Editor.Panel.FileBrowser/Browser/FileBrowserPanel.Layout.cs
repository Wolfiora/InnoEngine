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

    private void DrawBrowser(EditorContext context)
    {
        m_navigation.SyncExternalDirectoryChange(
            m_assets.browser.root,
            m_assets.browser.currentDirectory);

        ImGuiStylePtr style = NativeImGui.GetStyle();
        float breadcrumbBarHeight = GetBreadcrumbBarHeight(
            m_assets.browser.root,
            m_assets.browser.currentDirectory);
        Vector2 bodySize = new(0f, -(breadcrumbBarHeight + style.ItemSpacing.Y));
        bool mainVisible = NativeImGui.BeginChild(
            "##FileBrowserMain",
            bodySize,
            ImGuiChildFlags.None,
            ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
        try
        {
            if (mainVisible)
            {
                ImGuiTableFlags splitFlags =
                    ImGuiTableFlags.NoPadOuterX |
                    ImGuiTableFlags.NoKeepColumnsVisible |
                    ImGuiTableFlags.SizingFixedFit |
                    ImGuiTableFlags.NoSavedSettings;

                float splitterWidth = GetTreeSplitterWidth(style);
                float availableWidth = MathF.Max(0f, NativeImGui.GetContentRegionAvail().X);
                float treeWidth = ResolveTreeWidth(
                    m_treePaneRatio,
                    availableWidth,
                    splitterWidth);
                NativeImGui.PushStyleVar(ImGuiStyleVar.CellPadding, Vector2.Zero);
                try
                {
                    bool splitStarted = NativeImGui.BeginTable("##FileBrowserSplit", 3, splitFlags);
                    try
                    {
                        if (splitStarted)
                        {
                            NativeImGui.TableSetupColumn("##Tree", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize, treeWidth);
                            NativeImGui.TableSetupColumn("##Splitter", ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize, splitterWidth);
                            NativeImGui.TableSetupColumn("##Content", ImGuiTableColumnFlags.WidthStretch);

                            NativeImGui.TableNextRow();
                            _ = NativeImGui.TableSetColumnIndex(0);
                            DrawTreePane(context);

                            _ = NativeImGui.TableSetColumnIndex(1);
                            DrawTreeSplitter(splitterWidth, availableWidth, treeWidth);

                            _ = NativeImGui.TableSetColumnIndex(2);
                            DrawContentPane(context);
                        }
                    }
                    finally
                    {
                        if (splitStarted)
                            NativeImGui.EndTable();
                    }
                }
                finally
                {
                    NativeImGui.PopStyleVar();
                }
            }
        }
        finally
        {
            NativeImGui.EndChild();
        }
        DrawBreadcrumbBar(context, breadcrumbBarHeight);
    }

    private static float GetTreeSplitterWidth(ImGuiStylePtr style)
    {
        return MathF.Max(EditorWidget.style.assetSplitterMinimumWidth, style.DockingSeparatorSize);
    }

    private void DrawTreeSplitter(
        float width,
        float availableWidth,
        float treeWidth
    ) {
        Vector2 size = new(width, MathF.Max(1f, NativeImGui.GetContentRegionAvail().Y));
        _ = NativeImGui.InvisibleButton("##TreeSplitterGrip", size);

        bool hovered = NativeImGui.IsItemHovered();
        bool active = NativeImGui.IsItemActive();
        if (hovered || active)
            NativeImGui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);

        if (active)
        {
            Vector2 delta = NativeImGui.GetMouseDragDelta(ImGuiMouseButton.Left);
            if (MathF.Abs(delta.X) > 0f)
            {
                float requestedWidth = ClampTreeWidth(
                    treeWidth + delta.X,
                    availableWidth,
                    width);
                m_treePaneRatio = CalculateTreePaneRatio(
                    requestedWidth,
                    availableWidth,
                    width);
                NativeImGui.ResetMouseDragDelta(ImGuiMouseButton.Left);
            }
        }

        Vector2 min = NativeImGui.GetItemRectMin();
        Vector2 max = NativeImGui.GetItemRectMax();
        Vector4 color = active ? EditorPalette.assetAccent : hovered ? EditorPalette.assetBorder : EditorPalette.assetBorderSoft;
        NativeImGui.AddRectFilled(NativeImGui.GetWindowDrawList(), min, max, NativeImGui.ColorConvertFloat4ToU32(color));
    }

    private static float ClampTreeWidth(
        float requestedWidth,
        float availableWidth,
        float splitterWidth
    ) {
        float combinedPaneWidth = MathF.Max(0f, availableWidth - splitterWidth);
        float minimumPaneWidth = MathF.Min(
            EditorWidget.style.assetPaneMinimumVisibleWidth,
            combinedPaneWidth * 0.5f);
        float maximumTreeWidth = MathF.Max(minimumPaneWidth, combinedPaneWidth - minimumPaneWidth);
        return Math.Clamp(requestedWidth, minimumPaneWidth, maximumTreeWidth);
    }

    private static float ResolveTreeWidth(
        float treePaneRatio,
        float availableWidth,
        float splitterWidth
    ) {
        float combinedPaneWidth = MathF.Max(0f, availableWidth - splitterWidth);
        return ClampTreeWidth(
            combinedPaneWidth * Math.Clamp(treePaneRatio, 0f, 1f),
            availableWidth,
            splitterWidth);
    }

    private static float CalculateTreePaneRatio(
        float treeWidth,
        float availableWidth,
        float splitterWidth
    ) {
        float combinedPaneWidth = MathF.Max(0f, availableWidth - splitterWidth);
        return combinedPaneWidth > float.Epsilon
            ? Math.Clamp(treeWidth / combinedPaneWidth, 0f, 1f)
            : 0.5f;
    }

    private void DrawTreePane(EditorContext context)
    {
        NativeImGui.PushStyleColor(ImGuiCol.ChildBg, EditorPalette.collectionHeader);
        try
        {
            ImGuiStylePtr style = NativeImGui.GetStyle();
            Vector2 treePaneSize = new(-style.WindowPadding.X, 0f);
            bool treeVisible = NativeImGui.BeginChild(
                "##TreePane",
                treePaneSize,
                ImGuiChildFlags.None,
                ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
            try
            {
                if (treeVisible)
                {
                    float footerHeight = NativeImGui.GetFrameHeight() +
                                         style.WindowPadding.Y * 2f +
                                         style.ItemSpacing.Y;
                    bool treeScrollVisible = NativeImGui.BeginChild(
                        "##TreeScroll",
                        new Vector2(0f, -footerHeight),
                        ImGuiChildFlags.None,
                        ImGuiWindowFlags.HorizontalScrollbar);
                    try
                    {
                        if (treeScrollVisible)
                        {
                            m_tree.PrepareOpenRequests();
                            try
                            {
                                if (m_assets.browser.root == AssetBrowserRoot.Assets)
                                    m_tree.DrawEntry(context, string.Empty, "Assets", true);
                                else
                                    m_tree.DrawPluginRoot(context, m_assets.pipeline.sourceMounts);
                            }
                            finally
                            {
                                m_tree.ClearOpenRequests();
                            }
                            HandleBackgroundSelection(context);
                            m_contextMenu.DrawBackground(
                                context,
                                "##asset_tree_background_context",
                                FileBrowserPresentation.Tree);
                        }
                    }
                    finally
                    {
                        NativeImGui.EndChild();
                    }
                    DrawRootSwitchFooter(context);
                }
            }
            finally
            {
                NativeImGui.EndChild();
            }
            if (!IsReadOnlyLocation(m_assets.pipeline, m_assets.browser))
                m_dragDrop.DrawDirectoryTarget(context, m_assets.browser.currentDirectory);
        }
        finally
        {
            NativeImGui.PopStyleColor();
        }
    }

    private void DrawRootSwitchFooter(EditorContext context)
    {
        AssetBrowserRoot nextRoot = m_assets.browser.root == AssetBrowserRoot.Assets
            ? AssetBrowserRoot.Plugins
            : AssetBrowserRoot.Assets;
        DrawFooterTopSplitter();
        PushButtonColors(EditorPalette.assetAccent);
        try
        {
            if (NativeImGui.Button($"Switch to {nextRoot}##FileBrowserRoot", new Vector2(-1f, 0f)))
            {
                m_tree.RequestOpenRoot();
                m_navigation.SwitchRoot(context, nextRoot);
            }
        }
        finally
        {
            NativeImGui.PopStyleColor(3);
        }
    }

    private void DrawContentPane(EditorContext context)
    {
        NativeImGui.PushStyleColor(ImGuiCol.ChildBg, EditorPalette.collectionHeader);
        try
        {
            bool contentVisible = NativeImGui.BeginChild(
                "##ContentPane",
                Vector2.Zero,
                ImGuiChildFlags.None,
                ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse);
            try
            {
                if (contentVisible)
                {
                    DrawToolbar(context);
                    DrawEntriesRegion(
                        context,
                        m_data.CollectVisibleEntries(context, m_entryTypeFilter, m_entryScopeFilter, m_filter));
                }
            }
            finally
            {
                NativeImGui.EndChild();
            }
            if (!IsReadOnlyLocation(m_assets.pipeline, m_assets.browser))
                m_dragDrop.DrawDirectoryTarget(context, m_assets.browser.currentDirectory);
        }
        finally
        {
            NativeImGui.PopStyleColor();
        }
    }

    private void DrawEntriesRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        if (m_viewMode == ViewMode.List)
        {
            DrawListRegion(context, entries);
            return;
        }

        DrawGridRegion(context, entries);
    }

    private static void DrawFooterTopSplitter()
    {
        ImGuiStylePtr style = NativeImGui.GetStyle();
        Vector2 cursor = NativeImGui.GetCursorScreenPos();
        float width = NativeImGui.GetContentRegionAvail().X;
        float lineY = cursor.Y + style.WindowPadding.Y;
        uint color = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.assetBorder);
        NativeImGui.GetWindowDrawList().AddLine(
            new Vector2(cursor.X, lineY),
            new Vector2(cursor.X + width, lineY),
            color,
            EditorWidget.style.borderSize);
        NativeImGui.SetCursorPosY(NativeImGui.GetCursorPosY() + style.WindowPadding.Y * 2f);
    }

    private static void DrawBreadcrumbTopSeparator()
    {
        Vector2 min = NativeImGui.GetWindowPos();
        Vector2 size = NativeImGui.GetWindowSize();
        uint color = NativeImGui.ColorConvertFloat4ToU32(EditorPalette.assetBorder);
        NativeImGui.GetWindowDrawList().AddLine(
            min,
            new Vector2(min.X + size.X, min.Y),
            color,
            EditorWidget.style.borderSize);
    }

}
