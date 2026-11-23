using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
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
    private void DrawBrowser(EditorContext context)
    {
        m_navigation.SyncExternalDirectoryChange(m_assets.browser.root, m_assets.browser.currentDirectory);
        EditorUILayout style = Gui.GetLayout();
        float breadcrumbBarHeight = GetBreadcrumbBarHeight(m_assets.browser.root, m_assets.browser.currentDirectory);
        var bodySize = new Vector2(0f, -(breadcrumbBarHeight + style.itemSpacing.Y));
        bool mainVisible = Gui.BeginChild(
            "##FileBrowserMain",
            bodySize,
            ImGuiChildFlags.None,
            (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
        );
        try
        {
            if (mainVisible)
            {
                ImGuiTableFlags splitFlags = ImGuiTableFlags.NoPadOuterX
                                             | ImGuiTableFlags.NoKeepColumnsVisible
                                             | ImGuiTableFlags.SizingFixedFit
                                             | ImGuiTableFlags.NoSavedSettings;
                float splitterWidth = GetTreeSplitterWidth(style);
                float availableWidth = MathF.Max(0f, Gui.GetContentRegionAvailable().X);
                float treeWidth = ResolveTreeWidth(m_treePaneRatio, availableWidth, splitterWidth);
                Gui.PushStyleVar(ImGuiStyleVar.CellPadding, Vector2.Zero);
                try
                {
                    bool splitStarted = Gui.BeginTable("##FileBrowserSplit", 3, splitFlags, default, 0f);
                    try
                    {
                        if (splitStarted)
                        {
                            Gui.TableSetupColumn("##Tree", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), treeWidth);
                            Gui.TableSetupColumn(
                                "##Splitter",
                                (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize),
                                splitterWidth
                            );
                            Gui.TableSetupColumn("##Content", ImGuiTableColumnFlags.WidthStretch);
                            Gui.TableNextRow();
                            _ = Gui.TableSetColumnIndex(0);
                            DrawTreePane(context);
                            _ = Gui.TableSetColumnIndex(1);
                            DrawTreeSplitter(splitterWidth, availableWidth, treeWidth);
                            _ = Gui.TableSetColumnIndex(2);
                            DrawContentPane(context);
                        }
                    }
                    finally
                    {
                        if (splitStarted)
                        {
                            Gui.EndTable();
                        }
                    }
                }
                finally
                {
                    Gui.PopStyleVar();
                }
            }
        }
        finally
        {
            Gui.EndChild();
        }

        DrawBreadcrumbBar(context, breadcrumbBarHeight);
    }

    private void DrawTreeSplitter(
        float width,
        float availableWidth,
        float treeWidth
    ) {
        var size = new Vector2(width, MathF.Max(1f, Gui.GetContentRegionAvailable().Y));
        _ = Gui.InvisibleButton("##TreeSplitterGrip", size);
        bool hovered = Gui.IsItemHovered();
        bool active = Gui.IsItemActive();
        if (hovered || active)
        {
            Gui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        }
        if (active)
        {
            Vector2 delta = Gui.GetMouseDragDelta(ImGuiMouseButton.Left);
            if (MathF.Abs(delta.X) > 0f)
            {
                float requestedWidth = ClampTreeWidth(treeWidth + delta.X, availableWidth, width);
                m_treePaneRatio = CalculateTreePaneRatio(requestedWidth, availableWidth, width);
                Gui.ResetMouseDragDelta(ImGuiMouseButton.Left);
            }
        }

        Vector2 min = Gui.GetItemMinimum();
        Vector2 max = Gui.GetItemMaximum();
        Vector4 color = active ? EditorPalette.contentAccent : hovered ? EditorPalette.contentBorder : EditorPalette.contentBorderSoft;
        Gui.DrawFilledRectangle(min, max, Gui.ToPackedColor(color), 0f);
    }

    private void DrawTreePane(EditorContext context)
    {
        Gui.PushStyleColor(ImGuiCol.ChildBg, EditorPalette.collectionHeader);
        try
        {
            EditorUILayout style = Gui.GetLayout();
            var treePaneSize = new Vector2(-style.windowPadding.X, 0f);
            bool treeVisible = Gui.BeginChild(
                "##TreePane",
                treePaneSize,
                ImGuiChildFlags.None,
                (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
            );
            try
            {
                if (treeVisible)
                {
                    float footerHeight = Gui.GetFrameHeight() + style.windowPadding.Y * 2f + style.itemSpacing.Y;
                    bool treeScrollVisible = Gui.BeginChild(
                        "##TreeScroll",
                        new Vector2(0f, -footerHeight),
                        ImGuiChildFlags.None,
                        ImGuiWindowFlags.HorizontalScrollbar
                    );
                    try
                    {
                        if (treeScrollVisible)
                        {
                            m_tree.PrepareOpenRequests();
                            try
                            {
                                m_tree.Draw(context);
                                m_tree.CompleteNavigation(context);
                            }
                            finally
                            {
                                m_tree.ClearOpenRequests();
                            }

                            HandleBackgroundSelection(context);
                            m_contextMenu.DrawBackground(context, "##asset_tree_background_context", FileBrowserPresentation.Tree);
                        }
                    }
                    finally
                    {
                        Gui.EndChild();
                    }

                    DrawRootSwitchFooter(context);
                }
            }
            finally
            {
                Gui.EndChild();
            }

            if (!IsReadOnlyLocation(m_assets.edits.pipeline, m_assets.browser))
            {
                m_dragDrop.DrawDirectoryTarget(context, m_assets.browser.currentDirectory);
            }
        }
        finally
        {
            Gui.PopStyleColor();
        }
    }

    private void DrawRootSwitchFooter(EditorContext context)
    {
        AssetBrowserRoot nextRoot = m_assets.browser.root == AssetBrowserRoot.Assets ? AssetBrowserRoot.Plugins : AssetBrowserRoot.Assets;
        DrawFooterTopSplitter();
        PushButtonColors(EditorPalette.contentAccent);
        try
        {
            if (Gui.Button($"Switch to {nextRoot}##FileBrowserRoot", new Vector2(-1f, 0f)))
            {
                m_tree.RequestOpenRoot();
                m_navigation.SwitchRoot(context, nextRoot);
            }
        }
        finally
        {
            Gui.PopStyleColor(3);
        }
    }

    private void DrawContentPane(EditorContext context)
    {
        Gui.PushStyleColor(ImGuiCol.ChildBg, EditorPalette.collectionHeader);
        try
        {
            bool contentVisible = Gui.BeginChild(
                "##ContentPane",
                Vector2.Zero,
                ImGuiChildFlags.None,
                (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse)
            );
            try
            {
                if (contentVisible)
                {
                    DrawToolbar(context);
                    DrawEntriesRegion(context, m_data.CollectVisibleEntries(m_entryTypeFilter, m_entryScopeFilter, m_filter));
                }
            }
            finally
            {
                Gui.EndChild();
            }

            if (!IsReadOnlyLocation(m_assets.edits.pipeline, m_assets.browser))
            {
                m_dragDrop.DrawDirectoryTarget(context, m_assets.browser.currentDirectory);
            }
        }
        finally
        {
            Gui.PopStyleColor();
        }
    }

    private void DrawEntriesRegion(
        EditorContext context,
        IReadOnlyList<FileBrowserDisplayEntry> entries
    ) {
        m_visibleSelection = m_data.visibleSelection;
        m_drawKeyboardFocus = m_requestedKeyboardFocus;
        m_requestedKeyboardFocus = null;
        m_keyboardHandled = false;
        try
        {
            if (m_viewMode == ViewMode.List)
            {
                DrawListRegion(context, entries);
            }
            else
            {
                DrawGridRegion(context, entries);
            }
        }
        finally
        {
            m_visibleSelection = [];
            m_drawKeyboardFocus = null;
        }
        RuntimeIdentity? activation = m_pendingActivation;
        m_pendingActivation = null;
        if (activation is RuntimeIdentity identity
            && m_assets.edits.interactions.TryResolveIdentity(identity, out IdentityObject? target)
            && target is AssetFileEntry entry
        ) {
            m_navigation.OpenEntry(context, entry, m_tree);
        }
    }

    private static float GetTreeSplitterWidth(EditorUILayout style)
        => MathF.Max(EditorUI.current.widgets.style.splitterMinimumWidth, style.dockingSeparatorSize);

    private static float ClampTreeWidth(
        float requestedWidth,
        float availableWidth,
        float splitterWidth
    ) {
        float combinedPaneWidth = MathF.Max(0f, availableWidth - splitterWidth);
        float minimumPaneWidth = MathF.Min(EditorUI.current.widgets.style.paneMinimumVisibleWidth, combinedPaneWidth * 0.5f);
        float maximumTreeWidth = MathF.Max(minimumPaneWidth, combinedPaneWidth - minimumPaneWidth);
        return Math.Clamp(requestedWidth, minimumPaneWidth, maximumTreeWidth);
    }

    private static float ResolveTreeWidth(
        float treePaneRatio,
        float availableWidth,
        float splitterWidth
    ) {
        float combinedPaneWidth = MathF.Max(0f, availableWidth - splitterWidth);
        return ClampTreeWidth(combinedPaneWidth * Math.Clamp(treePaneRatio, 0f, 1f), availableWidth, splitterWidth);
    }

    private static float CalculateTreePaneRatio(
        float treeWidth,
        float availableWidth,
        float splitterWidth
    ) {
        float combinedPaneWidth = MathF.Max(0f, availableWidth - splitterWidth);
        return combinedPaneWidth > float.Epsilon ? Math.Clamp(treeWidth / combinedPaneWidth, 0f, 1f) : 0.5f;
    }

    private static void DrawFooterTopSplitter()
    {
        EditorUILayout style = Gui.GetLayout();
        Vector2 cursor = Gui.GetCursorScreenPosition();
        float width = Gui.GetContentRegionAvailable().X;
        float lineY = cursor.Y + style.windowPadding.Y;
        uint color = Gui.ToPackedColor(EditorPalette.contentBorder);
        Gui.DrawLine(new Vector2(cursor.X, lineY), new Vector2(cursor.X + width, lineY), color, EditorUI.current.widgets.style.borderSize);
        Gui.SetCursorPosY(Gui.GetCursorPosY() + style.windowPadding.Y * 2f);
    }

    private static void DrawBreadcrumbTopSeparator()
    {
        Vector2 min = Gui.GetWindowPos();
        Vector2 size = Gui.GetWindowSize();
        uint color = Gui.ToPackedColor(EditorPalette.contentBorder);
        Gui.DrawLine(min, new Vector2(min.X + size.X, min.Y), color, EditorUI.current.widgets.style.borderSize);
    }
}
