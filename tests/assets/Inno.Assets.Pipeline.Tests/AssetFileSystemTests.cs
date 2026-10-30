using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using Inno.Assets;
using Inno.Assets.Pipeline;

using Xunit;

namespace Inno.Assets.Pipeline.Tests;

public sealed class AssetFileSystemTests
{
    [Fact]
    public void RefreshPreservesUnchangedEntryInstancesAndRuntimeIdentities()
    {
        string root = CreateRoot();
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "Folder"));
            File.WriteAllText(Path.Combine(root, "Folder", "value.txt"), "value");
            using var fileSystem = new AssetFileSystem(root, autoStart: false);
            AssetFileEntry[] previous = fileSystem.GetEntries().ToArray();
            var identities = previous.Select(static entry => entry.identity.runtimeIdentity).ToArray();
            fileSystem.Refresh();
            for (int index = 0; index < previous.Length; index++)
            {
                Assert.True(fileSystem.TryGetEntry(previous[index].assetPath, out AssetFileEntry current));
                Assert.Same(previous[index], current);
                Assert.Equal(identities[index], current.identity.runtimeIdentity);
            }
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void FailedEnumerationKeepsThePreviousCompleteIndexAndItsIdentities()
    {
        string root = CreateRoot();
        string moved = root + ".temporarily-moved";
        try
        {
            File.WriteAllText(Path.Combine(root, "value.txt"), "value");
            using var fileSystem = new AssetFileSystem(root, autoStart: false);
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("value.txt"), out AssetFileEntry previous));
            var identity = previous.identity.runtimeIdentity;
            Directory.Move(root, moved);
            try
            {
                Assert.Throws<DirectoryNotFoundException>(() => fileSystem.Refresh());
                Assert.True(fileSystem.TryGetEntry(AssetPath.Project("value.txt"), out AssetFileEntry retained));
                Assert.Same(previous, retained);
                Assert.Equal(identity, retained.identity.runtimeIdentity);
            }
            finally
            {
                Directory.Move(moved, root);
            }
            fileSystem.Refresh();
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("value.txt"), out AssetFileEntry restored));
            Assert.Same(previous, restored);
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void RefreshRetiresRemovedEntriesAndPreservesUnaffectedEntries()
    {
        string root = CreateRoot();
        try
        {
            File.WriteAllText(Path.Combine(root, "keep.txt"), "keep");
            File.WriteAllText(Path.Combine(root, "remove.txt"), "remove");
            using var fileSystem = new AssetFileSystem(root, autoStart: false);
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("keep.txt"), out AssetFileEntry keep));
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("remove.txt"), out AssetFileEntry removed));
            File.Delete(Path.Combine(root, "remove.txt"));
            File.WriteAllText(Path.Combine(root, "added.txt"), "added");
            fileSystem.Refresh();
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("keep.txt"), out AssetFileEntry retained));
            Assert.Same(keep, retained);
            Assert.False(fileSystem.TryGetEntry(AssetPath.Project("remove.txt"), out _));
            Assert.Null(removed.identity.runtimeIdentity);
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("added.txt"), out AssetFileEntry added));
            Assert.NotNull(added.identity.runtimeIdentity);
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void Queries_RejectRootedAndTraversalPaths()
    {
        string root = CreateRoot();
        try
        {
            using var fileSystem = new AssetFileSystem(root, autoStart: false);

            Assert.Throws<ArgumentException>(() => AssetPath.Project("../outside.txt"));
            Assert.Throws<ArgumentException>(() => AssetPath.Project("A/../../outside"));
            Assert.Throws<ArgumentException>(() => AssetPath.Project(Path.GetFullPath(root)));
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void Refresh_IndexesDirectoriesAndFiles()
    {
        string root = CreateRoot();
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "Config"));
            System.IO.File.WriteAllText(Path.Combine(root, "Config", "a.json"), "{}");
            System.IO.File.WriteAllText(Path.Combine(root, "readme.txt"), "hi");

            using var fs = new AssetFileSystem(root, autoStart: false);
            var entries = fs.GetEntries(includeDirectories: true);

            Assert.Contains(entries, static x => x.assetPath == AssetPath.Project(string.Empty) && x.isDirectory);
            Assert.Contains(entries, static x => x.assetPath == AssetPath.Project("Config") && x.isDirectory);
            Assert.Contains(entries, static x => x.assetPath == AssetPath.Project("Config/a.json") && !x.isDirectory);
            Assert.Contains(entries, static x => x.assetPath == AssetPath.Project("readme.txt") && !x.isDirectory);

            var rootChildren = fs.GetChildren(AssetPath.Project(string.Empty));
            Assert.Equal(2, rootChildren.Count);
            Assert.Equal(AssetPath.Project("Config"), rootChildren[0].assetPath);
            Assert.Equal(AssetPath.Project("readme.txt"), rootChildren[1].assetPath);
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void Watcher_PublishesChangesOnlyWhenOwnerPolls()
    {
        string root = CreateRoot();
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "Config"));
            string file = Path.Combine(root, "Config", "watch.txt");
            System.IO.File.WriteAllText(file, "a");

            using var fs = new AssetFileSystem(root, autoStart: true, flushDelayMs: 20);
            System.IO.File.WriteAllText(file, "b");
            IReadOnlyList<AssetChangedEvent> batch = fs.WaitForIdle();

            Assert.Contains(batch, static x => x.relativePath == "Config/watch.txt");
            Assert.True(fs.Exists(AssetPath.Project("Config/watch.txt")));
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void Entries_ExposeLastExtensionDisplayNameAndFilterDatabaseNoise()
    {
        string root = CreateRoot();
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "Scripts"));
            System.IO.File.WriteAllText(Path.Combine(root, "Scripts", "Tool.editor.cs"), "class Tool {} ");
            System.IO.File.WriteAllText(Path.Combine(root, "Scripts", "Tool.editor.cs.imeta"), "meta");
            System.IO.File.WriteAllText(Path.Combine(root, "Scripts", "Tool.pdb"), "symbols");
            System.IO.File.WriteAllText(Path.Combine(root, ".DS_Store"), "noise");

            using var fileSystem = new AssetFileSystem(root, autoStart: false);
            AssetFileEntry entry = Assert.Single(
                fileSystem.GetEntries(includeDirectories: false));

            Assert.Equal("Tool.editor.cs", entry.name);
            Assert.Equal("Tool.editor", entry.nameWithoutExtension);
            Assert.Equal(".cs", entry.extension);
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void Refresh_TreatsProjectTildeDirectoriesAsOrdinaryWritableContent()
    {
        string root = CreateRoot();
        try
        {
            Directory.CreateDirectory(Path.Combine(root, "~Starter", "Scenes"));
            System.IO.File.WriteAllText(
                Path.Combine(root, "~Starter", "Scenes", "Game.cs"),
                "public sealed class Game { }");
            System.IO.File.WriteAllText(Path.Combine(root, "~Standalone.cs"), "sample-like file");

            using var fileSystem = new AssetFileSystem(root, autoStart: false);

            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("~Starter"), out AssetFileEntry tildeRoot));
            Assert.False(tildeRoot.isSample);
            Assert.False(tildeRoot.isSampleContent);
            Assert.True(fileSystem.TryGetEntry(
                AssetPath.Project("~Starter/Scenes/Game.cs"),
                out AssetFileEntry tildeContent));
            Assert.False(tildeContent.isSample);
            Assert.False(tildeContent.isSampleContent);
            Assert.True(fileSystem.TryGetEntry(
                AssetPath.Project("~Standalone.cs"),
                out AssetFileEntry ordinaryFile));
            Assert.False(ordinaryFile.isSample);
            Assert.False(ordinaryFile.isSampleContent);
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void MultipleSourcesKeepCanonicalPathsNamesAndReadOnlyStateIsolated()
    {
        string root = CreateRoot();
        string project = Path.Combine(root, "Assets");
        string plugin = Path.Combine(root, "PluginAssets");
        try
        {
            Directory.CreateDirectory(project);
            Directory.CreateDirectory(plugin);
            System.IO.File.WriteAllText(Path.Combine(project, "same.txt"), "project");
            System.IO.File.WriteAllText(Path.Combine(plugin, "same.txt"), "plugin");
            var pluginId = new AssetSourceId("tests.mount");
            using var fileSystem = new AssetFileSystem(
                [
                    new AssetSourceMount(AssetSourceId.project, project, isReadOnly: false),
                    new AssetSourceMount(pluginId, plugin, isReadOnly: true)
                ],
                autoStart: false);

            Assert.True(fileSystem.TryGetEntry(AssetPath.Project("same.txt"), out AssetFileEntry projectEntry));
            Assert.True(fileSystem.TryGetEntry(new AssetPath(pluginId, "same.txt"), out AssetFileEntry pluginEntry));
            Assert.True(fileSystem.TryGetEntry(AssetPath.Project(string.Empty), out AssetFileEntry projectRoot));
            Assert.True(fileSystem.TryGetEntry(new AssetPath(pluginId, string.Empty), out AssetFileEntry pluginRoot));
            Assert.Equal("same.txt", projectEntry.name);
            Assert.Equal("same.txt", pluginEntry.name);
            Assert.Equal("Assets", projectRoot.name);
            Assert.Equal("tests.mount", pluginRoot.name);
            Assert.False(projectEntry.isReadOnly);
            Assert.True(pluginEntry.isReadOnly);
            Assert.Equal(AssetSourceId.project, projectEntry.source);
            Assert.Equal(pluginId, pluginEntry.source);
            Assert.Contains(
                fileSystem.GetChildren(AssetPath.Project(string.Empty)),
                entry => entry.source == AssetSourceId.project);
            Assert.Contains(
                fileSystem.GetChildren(AssetPath.Project(string.Empty)),
                entry => entry.assetPath == new AssetPath(pluginId, string.Empty));
            Assert.Contains(
                fileSystem.GetChildren(new AssetPath(pluginId, string.Empty)),
                entry => entry.assetPath == new AssetPath(pluginId, "same.txt"));
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    [Fact]
    public void Watcher_RenameCarriesOldAndNewPathsThroughOwnerPolling()
    {
        string root = CreateRoot();
        try
        {
            string oldPath = Path.Combine(root, "old.txt");
            string newPath = Path.Combine(root, "new.txt");
            System.IO.File.WriteAllText(oldPath, "value");
            using var fileSystem = new AssetFileSystem(root, autoStart: true, flushDelayMs: 20);

            System.IO.File.Move(oldPath, newPath);
            IReadOnlyList<AssetChangedEvent> changes = fileSystem.WaitForIdle();

            AssetChangedEvent moved = Assert.Single(changes, static change =>
                change.changeType.HasFlag(WatcherChangeTypes.Renamed));
            Assert.Equal("old.txt", moved.oldRelativePath);
            Assert.Equal("new.txt", moved.relativePath);
            Assert.False(fileSystem.Exists(AssetPath.Project("old.txt")));
            Assert.True(fileSystem.Exists(AssetPath.Project("new.txt")));
        }
        finally
        {
            DeleteRoot(root);
        }
    }

    private static string CreateRoot()
    {
        string root = Path.Combine(Path.GetTempPath(), "InnoAssetsIOTests", Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(root);
        return root;
    }

    private static void DeleteRoot(string root)
    {
        if (Directory.Exists(root))
            Directory.Delete(root, recursive: true);
    }
}
