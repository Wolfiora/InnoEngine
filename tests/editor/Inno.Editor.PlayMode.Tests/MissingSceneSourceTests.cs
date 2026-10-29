using System;
using System.IO;
using System.Threading;
using Inno.Assets;
using Inno.Scene;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class MissingSceneSourceTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    [Trait("Acceptance", "Workflows")]
    public void UnavailableSourceKeepsAssignmentEditsAndIdentityUntilRecovery(bool edited)
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject value = scene.CreateObject("Retained object");
        string path = host.workspace.Save(scene, string.Empty);
        string physical = Path.Combine(host.projectRoot, "Assets", path);
        byte[] source = File.ReadAllBytes(physical);
        byte[] metadata = File.ReadAllBytes(physical + ".imeta");
        Guid id = host.assets.Load<SceneAsset>(AssetPath.Project(path)).identity.persistentId;
        if (edited)
            value.name = "Unsaved edit";
        File.Delete(physical);
        Assert.True(host.workspace.TryGetSourcePath(scene, out string pendingPath));
        Assert.Equal(path, pendingPath);
        host.assets.Rescan();
        Assert.True(host.workspace.IsDirty(scene));
        Assert.True(host.workspace.TryGetSourcePath(scene, out string knownPath));
        Assert.Equal(path, knownPath);
        Assert.Throws<InvalidOperationException>(() => host.workspace.Save(scene, string.Empty));
        Assert.False(File.Exists(physical));
        Assert.Equal(edited ? "Unsaved edit" : "Retained object", value.name);
        File.WriteAllBytes(physical, source);
        File.WriteAllBytes(physical + ".imeta", metadata);
        host.assets.Rescan();
        Thread.Sleep(120);
        Assert.True(host.workspace.TryGetSourcePath(scene, out string recovered));
        Assert.Equal(path, recovered);
        Assert.Equal(id, host.assets.Load<SceneAsset>(AssetPath.Project(path)).identity.persistentId);
        Assert.Equal(edited, host.workspace.IsDirty(scene));
        host.assets.Move(AssetPath.Project(path), AssetPath.Project("Recovered.iscene"));
        Assert.True(host.workspace.TryGetSourcePath(scene, out string renamed));
        Assert.Equal("Recovered.iscene", renamed);
        Assert.Equal("Recovered.iscene", host.workspace.Save(scene, string.Empty));
        Assert.True(host.edits.CloseScene(scene));
        GameScene reopened = host.workspace.Open("Recovered.iscene");
        Assert.Equal(edited ? "Unsaved edit" : "Retained object", Assert.Single(reopened.GetObjects()).name);
        Assert.False(host.workspace.IsDirty(reopened));
        Assert.Equal(id, host.assets.Load<SceneAsset>(AssetPath.Project("Recovered.iscene")).identity.persistentId);
    }

    [Fact]
    [Trait("Acceptance", "Workflows")]
    public void ExplicitSaveToAnotherDirectoryDoesNotRecreateTheMissingAssignedSource()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject value = scene.CreateObject("Retained object");
        string original = host.workspace.Save(scene, string.Empty);
        string physical = Path.Combine(host.projectRoot, "Assets", original);
        Guid originalId = host.assets.Load<SceneAsset>(AssetPath.Project(original)).identity.persistentId;
        File.Delete(physical);
        host.assets.Rescan();
        Assert.True(host.workspace.IsDirty(scene));
        host.assets.CreateDirectory(AssetPath.Project("Recovery"));
        value.name = "Saved elsewhere";
        string replacement = host.workspace.SaveToDirectory(scene, "Recovery");
        Assert.StartsWith("Recovery/", replacement, StringComparison.Ordinal);
        Assert.False(File.Exists(physical));
        Assert.True(host.workspace.TryGetSourcePath(scene, out string assigned));
        Assert.Equal(replacement, assigned);
        Assert.NotEqual(originalId, host.assets.Load<SceneAsset>(AssetPath.Project(replacement)).identity.persistentId);
        Assert.False(host.workspace.IsDirty(scene));
        Assert.True(host.edits.CloseScene(scene));
        GameScene reopened = host.workspace.Open(replacement);
        Assert.Equal("Saved elsewhere", Assert.Single(reopened.GetObjects()).name);
    }
}
