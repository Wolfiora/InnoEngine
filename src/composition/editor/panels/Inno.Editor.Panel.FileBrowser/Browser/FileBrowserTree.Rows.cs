using System;
using System.Collections.Generic;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Core.Identity;
using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Core;
using EditorUI = Inno.Editor.ImGui.EditorUiSession;
using Ui = Inno.Editor.ImGui.ImGui;

namespace Inno.Editor.Panel.FileBrowser;

internal sealed partial class FileBrowserTree
{
    private readonly record struct BrowserTreeRow(
        string path,
        string label,
        string nodeId,
        int depth,
        bool isRoot,
        bool virtualRoot,
        bool isLeaf,
        RuntimeIdentity? identity
    );

    private readonly List<BrowserTreeRow> m_rows = [];
    private long m_sourceRevision = long.MinValue;
    private ulong m_expansionRevision = ulong.MaxValue;
    private AssetBrowserRoot? m_rowsRoot;

    internal void Draw(EditorContext context)
    {
        RefreshRows();
        RuntimeIdentity? dragging = m_assets.edits.interactions.TryGetActiveDragIdentity(out RuntimeIdentity source) ? source : null;
        RuntimeIdentity? editing = m_rename.activeTarget;
        Span<int> preserved = stackalloc int[4];
        int preservedCount = 0;
        for (int index = 0; index < m_rows.Count; index++)
        {
            BrowserTreeRow row = m_rows[index];
            if (row.identity is { } identity && (identity == dragging || identity == editing || identity == m_drawKeyboardFocus)
                || row.isRoot && m_drawRootFocus)
                preserved[preservedCount++] = index;
        }
        EditorUI.current.widgets.ClippedRows(m_rows.Count, -1f, (tree: this, context), static (
            state,
            start,
            end
        ) =>
        {
            for (int index = start; index < end; index++)
                state.tree.DrawRow(state.context, state.tree.m_rows[index]);
        }, preserved[..preservedCount]);
    }

    private void RefreshRows()
    {
        var widgets = EditorUI.current.widgets;
        AssetPipeline pipeline = m_assets.edits.pipeline;
        if (!m_rootOpenRequest && !m_currentDirectoryOpenRequest && m_rowsRoot == m_assets.browser.root
            && m_sourceRevision == pipeline.revision && m_expansionRevision == widgets.treeExpansionRevision)
            return;
        var rows = new List<BrowserTreeRow>();
        var pending = new Stack<(string path, string label, bool root, int depth)>();
        if (m_assets.browser.root == AssetBrowserRoot.Assets)
            pending.Push((string.Empty, "Assets", true, 0));
        else
        {
            const string rootId = "tree_virtual_plugins";
            if (m_rootOpenRequest || m_currentDirectoryOpenRequest)
                widgets.SetTreeNodeOpen(rootId, true);
            rows.Add(new(string.Empty, "Plugins", rootId, 0, true, true, false, null));
            if (widgets.IsTreeNodeOpen(rootId))
            {
                AssetSourceMount[] mounts = pipeline.sourceMounts.Where(mount => m_assets.edits.IsPluginSource(mount.id))
                    .OrderBy(static mount => mount.id.value, StringComparer.OrdinalIgnoreCase).ToArray();
                for (int index = mounts.Length - 1; index >= 0; index--)
                    pending.Push((new AssetPath(mounts[index].id, string.Empty).ToString(), mounts[index].id.value, false, 1));
            }
        }
        while (pending.TryPop(out var next))
        {
            AssetFileEntry? entry = null;
            if (!next.root && (!pipeline.TryGetFileSystemEntry(AssetPath.Parse(next.path), out entry) || entry is null))
                continue;
            bool directory = next.root || entry!.isDirectory;
            IReadOnlyList<AssetFileEntry> children = directory ? m_data.SortTreeEntries(m_data.GetVisibleChildren(next.path)) : Array.Empty<AssetFileEntry>();
            string id = next.root ? "tree_root_" + next.path : "tree_" + next.path;
            if (ShouldOpenTreeEntry(next.path, next.root, directory))
                widgets.SetTreeNodeOpen(id, true);
            RuntimeIdentity? identity = next.root ? null : entry!.identity.runtimeIdentity
                ?? throw new InvalidOperationException("A visible asset tree entry requires a registered identity.");
            rows.Add(new(next.path, next.label, id, next.depth, next.root, false,
                !directory || !next.root && children.Count == 0, identity));
            if (!directory || !widgets.IsTreeNodeOpen(id))
                continue;
            for (int index = children.Count - 1; index >= 0; index--)
                pending.Push((children[index].assetPath.ToString(), children[index].name, false, next.depth + 1));
        }
        m_rows.Clear();
        m_rows.AddRange(rows);
        m_navigationRows.Clear();
        m_visibleSelection.Clear();
        foreach (BrowserTreeRow row in rows)
        {
            m_navigationRows.Add(new TreeNavigationRow(row.identity));
            if (row.identity is RuntimeIdentity identity)
                m_visibleSelection.Add(identity);
        }
        m_rowsRoot = m_assets.browser.root;
        m_sourceRevision = pipeline.revision;
        m_expansionRevision = widgets.treeExpansionRevision;
    }
}
