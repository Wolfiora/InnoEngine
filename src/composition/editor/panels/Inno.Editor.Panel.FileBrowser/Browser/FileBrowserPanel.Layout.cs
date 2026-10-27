using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Core.Identity;
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
    private void DrawBrowser(EditorContext context)
    {
        m_navigation.SyncExternalDirectoryChange(m_assets.browser.root, m_assets.browser.currentDirectory);
        EditorUiLayout style = Ui.GetLayout();
        float breadcrumbBarHeight = GetBreadcrumbBarHeight(m_assets.browser.root, m_assets.browser.currentDirectory);
        Vector2 bodySize = new(0f, -(breadcrumbBarHeight + style.itemSpacing.Y));
        bool mainVisible = Ui.BeginChild("##FileBrowserMain", bodySize, ImGuiChildFlags.None, (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse));
        try
        {
            if (mainVisible)
            {
                ImGuiTableFlags splitFlags = ImGuiTableFlags.NoPadOuterX | ImGuiTableFlags.NoKeepColumnsVisible | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.NoSavedSettings;
                float splitterWidth = GetTreeSplitterWidth(style);
                float availableWidth = MathF.Max(0f, Ui.GetContentRegionAvailable().X);
                float treeWidth = ResolveTreeWidth(m_treePaneRatio, availableWidth, splitterWidth);
                Ui.PushStyleVar(ImGuiStyleVar.CellPadding, Vector2.Zero);
                try
                {
                    bool splitStarted = Ui.BeginTable("##FileBrowserSplit", 3, splitFlags, default(System.Numerics.Vector2), 0f);
                    try
                    {
                        if (splitStarted)
                        {
                            Ui.TableSetupColumn("##Tree", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), treeWidth);
                            Ui.TableSetupColumn("##Splitter", (ImGuiTableColumnFlags.WidthFixed | ImGuiTableColumnFlags.NoResize), splitterWidth);
                            Ui.TableSetupColumn("##Content", ImGuiTableColumnFlags.WidthStretch);
                            Ui.TableNextRow();
                            _ = Ui.TableSetColumnIndex(0);
                            DrawTreePane(context);
                            _ = Ui.TableSetColumnIndex(1);
                            DrawTreeSplitter(splitterWidth, availableWidth, treeWidth);
                            _ = Ui.TableSetColumnIndex(2);
                            DrawContentPane(context);
                        }
                    }
                    finally
                    {
                        if (splitStarted)
                            Ui.EndTable();
                    }
                }
                finally
                {
                    Ui.PopStyleVar();
                }
            }
        }
        finally
        {
            Ui.EndChild();
        }

        DrawBreadcrumbBar(context, breadcrumbBarHeight);
    }

    private static float GetTreeSplitterWidth(EditorUiLayout style)
    {
        return MathF.Max(EditorUI.current.widgets.style.splitterMinimumWidth, style.dockingSeparatorSize);
    }

    private void DrawTreeSplitter(
        float width,
        float availableWidth,
        float treeWidth
    ) {
        Vector2 size = new(width, MathF.Max(1f, Ui.GetContentRegionAvailable().Y));
        _ = Ui.InvisibleButton("##TreeSplitterGrip", size);
        bool hovered = Ui.IsItemHovered();
        bool active = Ui.IsItemActive();
        if (hovered || active)
            Ui.SetMouseCursor(ImGuiMouseCursor.ResizeEw);
        if (active)
        {
            Vector2 delta = Ui.GetMouseDragDelta(ImGuiMouseButton.Left);
            if (MathF.Abs(delta.X) > 0f)
            {
                float requestedWidth = ClampTreeWidth(treeWidth + delta.X, availableWidth, width);
                m_treePaneRatio = CalculateTreePaneRatio(requestedWidth, availableWidth, width);
                Ui.ResetMouseDragDelta(ImGuiMouseButton.Left);
            }
        }

        Vector2 min = Ui.GetItemMinimum();
        Vector2 max = Ui.GetItemMaximum();
        Vector4 color = active ? EditorPalette.contentAccent : hovered ? EditorPalette.contentBorder : EditorPalette.contentBorderSoft;
        Ui.DrawFilledRectangle(min, max, Ui.ToPackedColor(color), 0f);
    }

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

    private void DrawTreePane(EditorContext context)
    {
        Ui.PushStyleColor(ImGuiCol.ChildBg, EditorPalette.collectionHeader);
        try
        {
            EditorUiLayout style = Ui.GetLayout();
            Vector2 treePaneSize = new(-style.windowPadding.X, 0f);
            bool treeVisible = Ui.BeginChild("##TreePane", treePaneSize, ImGuiChildFlags.None, (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse));
            try
            {
                if (treeVisible)
                {
                    float footerHeight = Ui.GetFrameHeight() + style.windowPadding.Y * 2f + style.itemSpacing.Y;
                    bool treeScrollVisible = Ui.BeginChild("##TreeScroll", new Vector2(0f, -footerHeight), ImGuiChildFlags.None, ImGuiWindowFlags.HorizontalScrollbar);
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
                        Ui.EndChild();
                    }

                    DrawRootSwitchFooter(context);
                }
            }
            finally
            {
                Ui.EndChild();
            }

            if (!IsReadOnlyLocation(m_assets.edits.pipeline, m_assets.browser))
                m_dragDrop.DrawDirectoryTarget(context, m_assets.browser.currentDirectory);
        }
        finally
        {
            Ui.PopStyleColor();
        }
    }

    private void DrawRootSwitchFooter(EditorContext context)
    {
        AssetBrowserRoot nextRoot = m_assets.browser.root == AssetBrowserRoot.Assets ? AssetBrowserRoot.Plugins : AssetBrowserRoot.Assets;
        DrawFooterTopSplitter();
        PushButtonColors(EditorPalette.contentAccent);
        try
        {
            if (Ui.Button($"Switch to {nextRoot}##FileBrowserRoot", new Vector2(-1f, 0f)))
            {
                m_tree.RequestOpenRoot();
                m_navigation.SwitchRoot(context, nextRoot);
            }
        }
        finally
        {
            Ui.PopStyleColor(3);
        }
    }

    private void DrawContentPane(EditorContext context)
    {
        Ui.PushStyleColor(ImGuiCol.ChildBg, EditorPalette.collectionHeader);
        try
        {
            bool contentVisible = Ui.BeginChild("##ContentPane", Vector2.Zero, ImGuiChildFlags.None, (ImGuiWindowFlags.NoScrollbar | ImGuiWindowFlags.NoScrollWithMouse));
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
                Ui.EndChild();
            }

            if (!IsReadOnlyLocation(m_assets.edits.pipeline, m_assets.browser))
                m_dragDrop.DrawDirectoryTarget(context, m_assets.browser.currentDirectory);
        }
        finally
        {
            Ui.PopStyleColor();
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
                DrawListRegion(context, entries);
            else
                DrawGridRegion(context, entries);
        }
        finally
        {
            m_visibleSelection = Array.Empty<Inno.Core.Identity.RuntimeIdentity>();
            m_drawKeyboardFocus = null;
        }
        RuntimeIdentity? activation = m_pendingActivation;
        m_pendingActivation = null;
        if (activation is RuntimeIdentity identity
            && m_assets.edits.interactions.TryResolveIdentity(identity, out Inno.Core.Identity.IdentityObject? target)
            && target is AssetFileEntry entry)
            m_navigation.OpenEntry(context, entry, m_tree);
    }

    private static void DrawFooterTopSplitter()
    {
        EditorUiLayout style = Ui.GetLayout();
        Vector2 cursor = Ui.GetCursorScreenPosition();
        float width = Ui.GetContentRegionAvailable().X;
        float lineY = cursor.Y + style.windowPadding.Y;
        uint color = Ui.ToPackedColor(EditorPalette.contentBorder);
        Ui.DrawLine(new Vector2(cursor.X, lineY), new Vector2(cursor.X + width, lineY), color, EditorUI.current.widgets.style.borderSize);
        Ui.SetCursorPosY(Ui.GetCursorPosY() + style.windowPadding.Y * 2f);
    }

    private static void DrawBreadcrumbTopSeparator()
    {
        Vector2 min = Ui.GetWindowPos();
        Vector2 size = Ui.GetWindowSize();
        uint color = Ui.ToPackedColor(EditorPalette.contentBorder);
        Ui.DrawLine(min, new Vector2(min.X + size.X, min.Y), color, EditorUI.current.widgets.style.borderSize);
    }
}
