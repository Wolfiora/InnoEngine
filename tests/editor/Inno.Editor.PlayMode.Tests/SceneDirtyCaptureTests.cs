using System;
using System.Security.Cryptography;
using System.Threading;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Core.Serialization.Converters;
using Inno.Core.Mathematics;
using Inno.Extensibility.Types;
using Inno.Scene;
using Xunit;

namespace Inno.Editor.PlayMode.Tests;

public sealed class SceneDirtyCaptureTests
{
    [Fact]
    public void OwnedRevisionCaptureKeepsDirtyStateExactAcrossHistoryRestoreAndDirectWrites()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject value = scene.CreateObject("Original");
        host.workspace.Save(scene, string.Empty);
        byte[] saved = Hash(host, scene);
        Check(false);
        host.edits.RenameGameObject(value, "Changed");
        Check(true);
        Assert.True(host.runtime.interactions.history.Undo().succeeded);
        Check(false);
        Assert.True(host.runtime.interactions.history.Redo().succeeded);
        Check(true);
        Assert.True(host.runtime.interactions.history.Undo().succeeded);
        Check(false);
        Assert.True(host.edits.ChangeProperty(value.transform, "localPosition",
            () => value.transform.localPosition = new Vector3(3, 4, 5), "Move"));
        Check(true);
        Assert.True(host.runtime.interactions.history.Undo().succeeded);
        Check(false);
        Assert.True(host.runtime.interactions.history.Redo().succeeded);
        Check(true);
        value.transform.localPosition = Vector3.ZERO;
        Check(false);

        void Check(bool expected)
        {
            Thread.Sleep(120);
            Assert.Equal(expected, !Hash(host, scene).AsSpan().SequenceEqual(saved));
            Assert.Equal(expected, host.workspace.IsDirty(scene));
        }
    }

    [Fact]
    public void CompleteSceneCapturesReuseOnlyUnchangedNeutralState()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject owner = scene.CreateObject("Owner");
        GameObject target = scene.CreateObject("Target");
        DirtyCaptureComponent component = owner.AddComponent<DirtyCaptureComponent>();
        component.target = target;
        SerializationContext context = AssetSerializationContext.Create(host.assets, new AssetDependencyCollection(includeLastKnownPaths: false));
        SerializationSnapshot before = host.serialization.CaptureSnapshot(scene, context);
        Assert.Equal(host.serialization.Serialize(scene, context), before.ToArray());
        Assert.Same(before, host.serialization.CaptureSnapshot(scene, context, before));
        component.number = 42;
        SerializationSnapshot changed = host.serialization.CaptureSnapshot(scene, context, before);
        Assert.NotSame(before, changed);
        Assert.Equal(host.serialization.Serialize(scene, context), changed.ToArray());
        Assert.Same(changed, host.serialization.CaptureSnapshot(scene, context, changed));
        component.number = 0;
        SerializationSnapshot restored = host.serialization.CaptureSnapshot(scene, context, changed);
        Assert.Equal(before.ToArray(), restored.ToArray());
        Assert.Same(restored, host.serialization.CaptureSnapshot(scene, context, restored));
    }

    [Fact]
    public void DirtyInspectionIncludesReorderedComponentSnapshots()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject owner = scene.CreateObject("Owner");
        DirtyCaptureComponent first = owner.AddComponent<DirtyCaptureComponent>();
        _ = owner.AddComponent<DirtyCaptureComponent>();
        host.workspace.Save(scene, string.Empty);
        byte[] saved = Hash(host, scene);
        Thread.Sleep(120);
        Assert.False(host.workspace.IsDirty(scene));
        int original = owner.GetComponentIndex(first);
        owner.SetComponentIndex(first, owner.GetComponents().Count - 1);
        Assert.False(saved.AsSpan().SequenceEqual(Hash(host, scene)));
        Thread.Sleep(120);
        Assert.True(host.workspace.IsDirty(scene));
        owner.SetComponentIndex(first, original);
        Assert.Equal(saved, Hash(host, scene));
        Thread.Sleep(120);
        Assert.False(host.workspace.IsDirty(scene));
    }

    [Fact]
    public void ExactDirtyInspectionFindsDirectFieldsConverterStateReferencesAndTopology()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        GameObject owner = scene.CreateObject("Owner");
        DirtyCaptureComponent component = owner.AddComponent<DirtyCaptureComponent>();
        GameObject target = scene.CreateObject("Target");
        host.workspace.Save(scene, string.Empty);
        byte[] saved = Hash(host, scene);
        Check(false);
        component.number = 7;
        Check(true);
        component.number = 0;
        Check(false);
        component.state.value = 23;
        Check(true);
        component.state.value = 0;
        Check(false);
        component.target = target;
        Check(true);
        component.target = null;
        Check(false);
        target.SetActive(false);
        Check(true);
        target.SetActive(true);
        Check(false);
        GameObject added = scene.CreateObject("Added");
        Check(true);
        scene.DestroyObject(added);
        Check(false);
        Assert.True(component.state.reads >= 10);

        void Check(bool expected)
        {
            Thread.Sleep(120);
            Assert.Equal(expected, !Hash(host, scene).AsSpan().SequenceEqual(saved));
            Assert.Equal(expected, host.workspace.IsDirty(scene));
        }
    }

    [Fact]
    public void FailedCapturePreservesTheSavedBaselineAndRecoversWithoutPublishingCleanState()
    {
        using var host = new NativeProjectPanelHost();
        GameScene scene = host.CreateScene();
        DirtyCaptureComponent component = scene.CreateObject("Owner").AddComponent<DirtyCaptureComponent>();
        host.workspace.Save(scene, string.Empty);
        Thread.Sleep(120);
        Assert.False(host.workspace.IsDirty(scene));
        component.state.reject = true;
        Thread.Sleep(120);
        Assert.True(host.workspace.IsDirty(scene));
        component.state.reject = false;
        component.number = 9;
        Thread.Sleep(120);
        Assert.True(host.workspace.IsDirty(scene));
        component.number = 0;
        Thread.Sleep(120);
        Assert.False(host.workspace.IsDirty(scene));
        component.number = 17;
        host.workspace.Save(scene, string.Empty);
        Thread.Sleep(120);
        Assert.False(host.workspace.IsDirty(scene));
        component.number = 0;
        Thread.Sleep(120);
        Assert.True(host.workspace.IsDirty(scene));
    }

    private static byte[] Hash(
        NativeProjectPanelHost host,
        GameScene scene
    ) => SHA256.HashData(host.serialization.Serialize(scene,
        AssetSerializationContext.Create(host.assets, new AssetDependencyCollection(includeLastKnownPaths: false))));
}

[StableTypeId("942e794b-1c53-45a6-80e6-a641d76fdd09")]
[AllowMultipleComponent]
internal sealed class DirtyCaptureComponent : GameComponent
{
    [SerializableProperty] public int number;
    [SerializableProperty] public GameObject? target;
    [SerializableProperty] public DirtyCaptureValue state { get; set; } = new();
}

internal sealed class DirtyCaptureValue
{
    internal int value;
    internal bool reject;
    internal int reads;
}

internal sealed class DirtyCaptureValueConverter : SerializationConverter<DirtyCaptureValue>
{
    public override void Write(
        SerializationWriter writer,
        DirtyCaptureValue value
    ) {
        value.reads++;
        writer.Write("value", value.value);
        if (value.reject)
            throw new InvalidOperationException("The test-owned converter rejected capture.");
    }

    public override DirtyCaptureValue Read(SerializationReader reader) => new() { value = reader.Read<int>("value") };
}
