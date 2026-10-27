using Inno.Editor.Assets;
using Inno.Editor.Assets.ImGui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics.CodeAnalysis;
using Inno.Core.Identity;
using Inno.Assets;
using Inno.Assets.Pipeline;

namespace Inno.Editor.Panel.FileBrowser;
internal enum FileBrowserEntryTypeFilter
{
    All,
    FoldersOnly,
    FilesOnly
}

internal enum FileBrowserEntryScopeFilter
{
    CurrentOnly,
    Recursive
}

internal readonly record struct FileBrowserDisplayEntry(
    RuntimeIdentity identity,
    AssetPath path,
    string displayName,
    string extension,
    bool isDirectory,
    bool isPluginRoot = false
);
internal sealed class FileBrowserData(AssetBrowserModule assets)
{
    private ViewInput? m_input;
    private IReadOnlyList<FileBrowserDisplayEntry> m_entries = Array.Empty<FileBrowserDisplayEntry>();
    internal IReadOnlyList<RuntimeIdentity> visibleSelection { get; private set; } = Array.Empty<RuntimeIdentity>();

    internal IReadOnlyList<FileBrowserDisplayEntry> CollectVisibleEntries(
        FileBrowserEntryTypeFilter typeFilter,
        FileBrowserEntryScopeFilter scopeFilter,
        string searchFilter
    ) {
        var input = new ViewInput(assets.edits.pipeline.revision, assets.browser.root, assets.browser.currentDirectory, typeFilter, scopeFilter, searchFilter);
        if (m_input == input)
            return m_entries;
        List<FileBrowserDisplayEntry> entries = [];
        if (assets.browser.root == AssetBrowserRoot.Plugins && string.IsNullOrEmpty(assets.browser.currentDirectory))
        {
            CollectPluginRoots(entries, scopeFilter);
        }
        else if (scopeFilter == FileBrowserEntryScopeFilter.CurrentOnly)
        {
            IReadOnlyList<AssetFileEntry> children = GetVisibleChildren(assets.browser.currentDirectory);
            for (int i = 0; i < children.Count; i++)
                entries.Add(Describe(children[i], children[i].name));
        }
        else
            CollectEntriesRecursive(assets.browser.currentDirectory, entries);
        ApplyTypeFilter(entries, typeFilter);
        ApplySearchFilter(entries, searchFilter);
        entries.Sort(static (
            left,
            right
        ) =>
        {
            int byName = string.Compare(left.displayName, right.displayName, StringComparison.OrdinalIgnoreCase);
            if (byName != 0)
                return byName;
            int byExtension = string.Compare(left.extension, right.extension, StringComparison.OrdinalIgnoreCase);
            return byExtension != 0 ? byExtension : string.CompareOrdinal(left.path.ToString(), right.path.ToString());
        });
        visibleSelection = Array.AsReadOnly(entries.Select(static entry => entry.identity).ToArray());
        m_entries = entries.AsReadOnly();
        m_input = input;
        return m_entries;
    }

    internal bool TryResolve(
        FileBrowserDisplayEntry item,
        [NotNullWhen(true)] out AssetFileEntry? entry
    ) {
        if (assets.edits.interactions.TryResolveIdentity(item.identity, out IdentityObject? target)
            && target is AssetFileEntry resolved && resolved.assetPath == item.path)
        {
            entry = resolved;
            return true;
        }
        entry = null;
        return false;
    }

    private static FileBrowserDisplayEntry Describe(
        AssetFileEntry entry,
        string label,
        bool pluginRoot = false
    ) => new(entry.identity.runtimeIdentity ?? throw new InvalidOperationException("A visible asset requires a registered identity."),
        entry.assetPath, label, entry.extension, entry.isDirectory, pluginRoot);

    private static void ApplyTypeFilter(
        List<FileBrowserDisplayEntry> entries,
        FileBrowserEntryTypeFilter typeFilter
    ) {
        switch (typeFilter)
        {
            case FileBrowserEntryTypeFilter.FoldersOnly:
                entries.RemoveAll(static item => !item.isDirectory);
                break;
            case FileBrowserEntryTypeFilter.FilesOnly:
                entries.RemoveAll(static item => item.isDirectory);
                break;
        }
    }

    private static void ApplySearchFilter(
        List<FileBrowserDisplayEntry> entries,
        string searchFilter
    ) {
        string filter = searchFilter.Trim();
        if (!string.IsNullOrEmpty(filter))
        {
            entries.RemoveAll(item => !item.displayName.Contains(filter, StringComparison.OrdinalIgnoreCase));
        }
    }

    private void CollectPluginRoots(
        List<FileBrowserDisplayEntry> entries,
        FileBrowserEntryScopeFilter scopeFilter
    ) {
        AssetSourceMount[] mounts = assets.edits.pipeline.sourceMounts.Where(mount => assets.edits.IsPluginSource(mount.id)).OrderBy(
            static mount => mount.id.value,
            StringComparer.OrdinalIgnoreCase
        ).ToArray();
        for (int i = 0; i < mounts.Length; i++)
        {
            AssetSourceMount mount = mounts[i];
            var rootPath = new AssetPath(mount.id, string.Empty);
            if (!assets.edits.pipeline.TryGetFileSystemEntry(rootPath, out AssetFileEntry root))
                continue;
            entries.Add(Describe(root, mount.id.value, pluginRoot: true));
            if (scopeFilter == FileBrowserEntryScopeFilter.Recursive)
                CollectEntriesRecursive(root.assetPath.ToString(), entries);
        }
    }

    private void CollectEntriesRecursive(
        string directory,
        List<FileBrowserDisplayEntry> entries
    ) {
        var pending = new Stack<string>();
        pending.Push(directory);
        while (pending.TryPop(out string? current))
        {
            foreach (AssetFileEntry child in GetVisibleChildren(current))
            {
                entries.Add(Describe(child, child.name));
                if (child.isDirectory)
                    pending.Push(child.assetPath.ToString());
            }
        }
    }

    internal List<AssetFileEntry> SortTreeEntries(IReadOnlyList<AssetFileEntry> entries)
    {
        List<AssetFileEntry> sorted = new(entries);
        sorted.Sort(static (
            left,
            right
        ) =>
        {
            if (left.isDirectory != right.isDirectory)
                return left.isDirectory ? -1 : 1;
            return string.Compare(left.name, right.name, StringComparison.OrdinalIgnoreCase);
        });
        return sorted;
    }

    internal IReadOnlyList<AssetFileEntry> GetVisibleChildren(string relativePath)
    {
        AssetSourceId source = AssetPath.Parse(relativePath).source;
        IReadOnlyList<AssetFileEntry> children = assets.edits.pipeline.GetFileSystemChildren(AssetPath.Parse(relativePath));
        if (children.Count == 0)
            return children;
        List<AssetFileEntry> isolated = [];
        for (int i = 0; i < children.Count; i++)
        {
            if (children[i].source == source)
                isolated.Add(children[i]);
        }

        return isolated;
    }

    private readonly record struct ViewInput(
        long revision,
        AssetBrowserRoot root,
        string directory,
        FileBrowserEntryTypeFilter type,
        FileBrowserEntryScopeFilter scope,
        string search
    );
}
