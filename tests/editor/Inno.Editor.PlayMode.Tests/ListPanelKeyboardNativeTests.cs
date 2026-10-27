using System;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Editor.Assets.ImGui;
using Inno.Editor.Interactions;
using Inno.Native.ImGui;
using Inno.Scene;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class ListPanelKeyboardNativeTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BrowserResolvesCurrentSourceEntriesAfterAnUnchangedRescan(bool tree)
    {
        using var host = new NativeProjectPanelHost();
        host.assets.CreateDirectory(AssetPath.Project("Alpha"));
        host.assets.CreateDirectory(AssetPath.Project("Beta"));
        host.assets.Rescan();
        EditorPanelExtension panel = host.native.GetPanel("asset.file-browser");
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        Assert.True(host.assets.TryGetFileSystemEntry(AssetPath.Project("Alpha"), out AssetFileEntry first));
        host.runtime.interactions.SetSelection(first);
        NativePanelInput.Focus(host.native, panel, tree ? "##TreeScroll" : "##EntriesScroll");
        long contentRevision = host.assets.revision;
        host.assets.Rescan();
        Assert.Equal(contentRevision, host.assets.revision);
        Assert.True(host.assets.TryGetFileSystemEntry(AssetPath.Project("Beta"), out AssetFileEntry current));
        NativePanelInput.Key(host.native, panel, ImGuiKey.End);
        Assert.Same(current, host.runtime.interactions.selection.selectedTarget);
    }

    [Fact]
    public void HierarchyUsesVisibleRangeAndKeepsTextEditingExclusive()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject[] objects = Enumerable.Range(0, 12).Select(index => scene.CreateObject("Object " + index.ToString("D2"))).ToArray();
        GameObject child = scene.CreateObject("Collapsed child");
        child.transform.SetParent(objects[10].transform);
        EditorPanelExtension panel = host.native.GetPanel("scene.hierarchy");
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        EditorInteractions interactions = host.runtime.interactions;
        interactions.SetSelection(objects[0]);
        NativePanelInput.Focus(host.native, panel, "##HierarchyContent");
        NativePanelInput.Key(host.native, panel, ImGuiKey.DownArrow);
        Assert.Same(objects[1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.End, shift: true);
        Assert.Equal(11, interactions.selection.count);
        NativePanelInput.Key(host.native, panel, ImGuiKey.UpArrow, primary: true);
        Assert.Equal(11, interactions.selection.count);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Space, primary: true);
        Assert.False(interactions.selection.Contains(objects[10]));
        Assert.Equal(10, interactions.selection.count);
        NativePanelInput.Key(host.native, panel, ImGuiKey.DownArrow);
        Assert.Same(objects[^1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Home);
        Assert.Same(scene, interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.PageDown);
        Assert.Same(objects[^1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.PageUp);
        Assert.Same(scene, interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.End);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Enter);
        NativePanelInput.Key(host.native, panel, ImGuiKey.UpArrow);
        Assert.Same(objects[^1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Escape);
        Assert.Equal("Object 11", objects[^1].name);
        Assert.False(interactions.history.canUndo);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void BrowserUsesTheFocusedVisibleOrderAndActivatesFoldersAfterTraversal(bool tree)
    {
        using var host = new NativeProjectPanelHost();
        AssetFileEntry[] entries = Enumerable.Range(0, 12).Select(index =>
        {
            AssetPath path = AssetPath.Project("Folder " + index.ToString("D2"));
            host.assets.CreateDirectory(path);
            Assert.True(host.assets.TryGetFileSystemEntry(path, out AssetFileEntry entry));
            return entry;
        }).ToArray();
        host.assets.CreateDirectory(AssetPath.Project("Folder 10/Collapsed child"));
        EditorPanelExtension panel = host.native.GetPanel("asset.file-browser");
        host.native.DrawPanel(panel);
        host.native.DrawPanel(panel);
        EditorInteractions interactions = host.runtime.interactions;
        for (int index = 0; index < entries.Length; index++)
        {
            Assert.True(host.assets.TryGetFileSystemEntry(AssetPath.Project("Folder " + index.ToString("D2")), out AssetFileEntry current));
            entries[index] = current;
        }
        interactions.SetSelection(entries[0]);
        NativePanelInput.Focus(host.native, panel, tree ? "##TreeScroll" : "##EntriesScroll");
        NativePanelInput.Key(host.native, panel, ImGuiKey.DownArrow);
        Assert.Same(entries[1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.End, shift: true);
        Assert.Equal(11, interactions.selection.count);
        NativePanelInput.Key(host.native, panel, ImGuiKey.UpArrow, primary: true);
        Assert.Equal(11, interactions.selection.count);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Space, primary: true);
        Assert.False(interactions.selection.Contains(entries[10]));
        Assert.Equal(10, interactions.selection.count);
        NativePanelInput.Key(host.native, panel, ImGuiKey.DownArrow);
        Assert.Same(entries[^1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.End);
        Assert.Same(entries[^1], interactions.selection.selectedTarget);
        NativePanelInput.Key(host.native, panel, ImGuiKey.Enter);
        Assert.True(interactions.TryGetModule<AssetBrowserModule>(out var browser));
        Assert.Equal(entries[^1].assetPath.ToString(), browser!.browser.currentDirectory);
        Assert.False(interactions.history.canUndo);
    }
}
