using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Inno.Editor.Assets;
using Inno.Core.Identity;
using Inno.Editor.Assets.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Core;
using Inno.Editor.ImGui;
using Inno.Editor.ImGui.ImGuiWidget;
using static Inno.Editor.Panel.FileBrowser.FileBrowserUtility;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;
internal sealed partial class FileBrowserTree
{
    private readonly FileBrowserData m_data;
    private readonly FileBrowserNavigation m_navigation;
    private readonly FileBrowserDragDrop m_dragDrop;
    private readonly FileBrowserRename m_rename;
    private readonly FileBrowserContextMenu m_contextMenu;
    private readonly AssetBrowserModule m_assets;
    private bool m_rootOpenRequest = true;
    private bool m_currentDirectoryOpenRequest;
    private string m_currentDirectoryOpenTarget = string.Empty;
    private string? m_lastCurrentDirectoryOpenTarget;
    internal FileBrowserTree(
        FileBrowserData data,
        FileBrowserNavigation navigation,
        FileBrowserDragDrop dragDrop,
        FileBrowserRename rename,
        FileBrowserContextMenu contextMenu,
        AssetBrowserModule assets
    ) {
        m_data = data;
        m_navigation = navigation;
        m_dragDrop = dragDrop;
        m_rename = rename;
        m_contextMenu = contextMenu;
        m_assets = assets;
    }

    private void DrawRow(
        EditorContext context,
        BrowserTreeRow row
    ) {
        AssetFileEntry? entry = null;
        if (row.identity is { } identity)
        {
            if (!m_assets.edits.interactions.TryResolveIdentity(identity, out Inno.Core.Identity.IdentityObject? resolved)
                || resolved is not AssetFileEntry current)
            {
                Ui.Dummy(new(1f, Ui.GetFrameHeight()));
                return;
            }
            entry = current;
        }
        bool selected = entry is not null && m_assets.edits.interactions.selection.Contains(entry);
        bool currentDirectory = row.virtualRoot ? string.IsNullOrEmpty(m_assets.browser.currentDirectory)
            : (entry?.isDirectory ?? row.isRoot) && string.Equals(m_assets.browser.currentDirectory, row.path, StringComparison.Ordinal)
                && (m_assets.browser.root == AssetBrowserRoot.Assets) == (AssetPath.Parse(row.path).source == AssetSourceId.project);
        string icon = entry is { isDirectory: false } ? m_assets.GetIcon(entry) : m_assets.folderIcon;
        bool editing = !row.isRoot && m_rename.IsEditing(context, row.path, FileBrowserPresentation.Tree);
        TreeNodeResult result = EditorUI.current.widgets.TreeNode(
            row.nodeId,
            drawContext => DrawRowContent(context, row.nodeId, row.path, row.label, icon, currentDirectory, editing, drawContext.rowHeight),
            new TreeNodeOptions { depth = row.depth, selected = selected, isLeaf = row.isLeaf,
                requestFocus = RequestsKeyboardFocus(entry, row.isRoot), keepContentActive = editing });
        if (!result.isSubmitted)
            return;
        if (entry is not null && result.isDoubleClicked)
        {
            m_rename.MarkInteraction(FileBrowserPresentation.Tree);
            m_pendingSelection = entry.identity.runtimeIdentity;
            m_pendingActivation = true;
        }
        else if (entry is not null && result.isClicked)
        {
            m_rename.MarkInteraction(FileBrowserPresentation.Tree);
            m_pendingSelection = entry.identity.runtimeIdentity;
        }
        else if (row.isRoot && (result.isClicked || result.isDoubleClicked))
            m_pendingRootNavigation = true;
        if (entry is not null && !editing)
        {
            m_dragDrop.DrawAssetSource(context, entry);
            m_contextMenu.DrawEntry(context, "##asset_tree_context_" + row.path, row.path, FileBrowserPresentation.Tree);
            if (entry.isDirectory && !entry.isReadOnly)
                m_dragDrop.DrawDirectoryTarget(context, row.path);
        }
        else if (row.isRoot && !row.virtualRoot)
        {
            m_contextMenu.DrawBackground(context, "##asset_tree_root_context", FileBrowserPresentation.Tree);
            m_dragDrop.DrawDirectoryTarget(context, row.path);
        }
    }

    private void DrawRowContent(
        EditorContext context,
        string id,
        string relativePath,
        string label,
        string icon,
        bool isCurrentDirectory,
        bool editing,
        float rowHeight
    ) {
        if (!editing)
        {
            EditorUI.current.widgets.IconText(icon, label, isCurrentDirectory);
            return;
        }

        EditorUI.current.widgets.IconText(icon, string.Empty, false);
        Ui.SameLine(0f, 0f);
        m_rename.Draw(context, id, relativePath, FileBrowserPresentation.Tree, Ui.GetContentRegionAvailable().X, rowHeight);
    }

    internal void PrepareOpenRequests()
    {
        BeginNavigation();
        string currentDirectory = NormalizePath(m_assets.browser.currentDirectory);
        if (!m_currentDirectoryOpenRequest && !string.Equals(m_lastCurrentDirectoryOpenTarget, currentDirectory, StringComparison.Ordinal))
        {
            m_currentDirectoryOpenTarget = currentDirectory;
            m_currentDirectoryOpenRequest = true;
            m_lastCurrentDirectoryOpenTarget = currentDirectory;
        }
    }

    internal void ClearOpenRequests()
    {
        m_rootOpenRequest = false;
        m_currentDirectoryOpenRequest = false;
        ClearNavigationFrame();
    }

    internal void RequestOpenRoot() => m_rootOpenRequest = true;
    internal void RequestOpenTreeToPath(string path)
    {
        string normalizedPath = NormalizePath(path);
        string treePath = IsDirectoryPath(m_assets.edits.pipeline, normalizedPath) ? normalizedPath : GetParentDirectory(normalizedPath);
        m_currentDirectoryOpenTarget = treePath;
        m_currentDirectoryOpenRequest = true;
        m_lastCurrentDirectoryOpenTarget = treePath;
    }

    private bool ShouldOpenTreeEntry(
        string relativePath,
        bool isRoot,
        bool isDirectory
    ) {
        if (!isDirectory)
            return false;
        if (isRoot)
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                return m_rootOpenRequest || m_currentDirectoryOpenRequest;
            }

            return m_currentDirectoryOpenRequest && IsAncestorOrSelf(relativePath, m_currentDirectoryOpenTarget);
        }

        return m_currentDirectoryOpenRequest && IsAncestorOrSelf(relativePath, m_currentDirectoryOpenTarget);
    }
}
