using System;
using System.Linq;
using Inno.Assets;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Xunit;

namespace Inno.Scene.Tests;

public sealed partial class SceneSerializationTests
{
    [Fact]
    public void ScalarPropertyPreparationDoesNotAllocateAnIndexOfUnrelatedSceneObjects()
    {
        var scene = new GameScene("Property allocation");
        GameObject owner = scene.CreateObject("Owner");
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        long small = Measure();
        for (int index = 0; index < 5_000; index++)
            _ = scene.CreateObject("Unrelated " + index);
        long large = Measure();
        Assert.True(large <= small + 64 * 1024,
            $"Ten scalar property captures allocated {small} bytes in a small Scene and {large} with 5,000 unrelated objects.");

        long Measure()
        {
            for (int index = 0; index < 10; index++)
                _ = ScenePropertySerialization.CapturePropertySnapshots(owner.transform, m_serialization, assets);
            long before = GC.GetAllocatedBytesForCurrentThread();
            for (int index = 0; index < 10; index++)
                Assert.Equal(3, ScenePropertySerialization.CapturePropertySnapshots(owner.transform, m_serialization, assets).Count);
            return GC.GetAllocatedBytesForCurrentThread() - before;
        }
    }

    [Fact]
    public void PropertyReferencesRestoreCurrentObjectComponentAndSystemInstancesAfterReplacement()
    {
        var scene = new GameScene("Property references");
        SceneManager.LoadScene(scene);
        GameObject owner = scene.CreateObject("Owner");
        GameObject target = scene.CreateObject("Target");
        ReferenceComponent component = target.AddComponent<ReferenceComponent>();
        StateSystem system = scene.AddSystem<StateSystem>();
        PropertyReferenceProbe probe = owner.AddComponent<PropertyReferenceProbe>();
        probe.targetObject = target;
        probe.targetComponent = component;
        probe.targetSystem = system;
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        byte[] properties = ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets);
        byte[] graph = m_serialization.Serialize(scene, m_serializationContext);
        Assert.True(SceneManager.UnloadScene(scene));
        GameScene replacement = m_serialization.Deserialize<GameScene>(graph, m_serializationContext);
        SceneManager.LoadScene(replacement);
        PropertyReferenceProbe restored = replacement.GetObjects().Single(value => value.name == "Owner").GetComponents()
            .OfType<PropertyReferenceProbe>().Single();
        GameObject current = replacement.GetObjects().Single(value => value.name == "Target");
        restored.targetObject = null;
        restored.targetComponent = null;
        restored.targetSystem = null;
        _ = ScenePropertySerialization.RestoreProperties(restored, properties, m_serialization, assets);
        Assert.Same(current, restored.targetObject);
        Assert.Same(current.GetComponents().OfType<ReferenceComponent>().Single(), restored.targetComponent);
        Assert.Same(replacement.GetSystems().OfType<StateSystem>().Single(), restored.targetSystem);
        Assert.Equal(target.identity.persistentId, current.identity.persistentId);
        Assert.NotSame(target, current);
        Assert.True(target.isDestroyed);
    }

    [Fact]
    public void PropertyReferencesRejectForeignScenesAndIdentityDomains()
    {
        var scene = new GameScene("Owner");
        PropertyReferenceProbe probe = scene.CreateObject("Owner").AddComponent<PropertyReferenceProbe>();
        var other = new GameScene("Other");
        probe.targetObject = other.CreateObject("Foreign scene");
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        Assert.ThrowsAny<Exception>(() => ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets));
        var identities = new IdentityAllocator();
        using var world = new SceneWorld(identities, m_types);
        using (world.EnterScope())
        {
            var foreign = new GameScene("Foreign domain");
            probe.targetObject = foreign.CreateObject("Foreign domain");
        }
        Assert.ThrowsAny<Exception>(() => ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets));
        probe.targetObject = null;
        Assert.NotEmpty(ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets));
    }

    [Fact]
    public void MissingPropertyTargetRejectsRestoreWithoutAssigningTheDestroyedObject()
    {
        var scene = new GameScene("Missing property target");
        PropertyReferenceProbe probe = scene.CreateObject("Owner").AddComponent<PropertyReferenceProbe>();
        GameObject target = scene.CreateObject("Target");
        probe.targetObject = target;
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        byte[] property = ScenePropertySerialization.CaptureProperty(probe, nameof(PropertyReferenceProbe.targetObject), m_serialization, assets);
        scene.DestroyObject(target);
        probe.targetObject = null;
        Assert.ThrowsAny<Exception>(() => ScenePropertySerialization.RestoreProperties(probe, property, m_serialization, assets));
        Assert.Null(probe.targetObject);
        probe.targetObject = target;
        Assert.ThrowsAny<Exception>(() => ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets));
        probe.targetObject = null;
    }

    [Fact]
    public void NestedPropertyReadsAndGetterFailuresRestoreTheOuterReferenceScope()
    {
        var scene = new GameScene("Outer");
        PropertyReferenceProbe probe = scene.CreateObject("Owner").AddComponent<PropertyReferenceProbe>();
        probe.targetObject = scene.CreateObject("Outer target");
        var nestedScene = new GameScene("Nested");
        ReferenceComponent nested = nestedScene.CreateObject("Nested owner").AddComponent<ReferenceComponent>();
        nested.targetObject = nestedScene.CreateObject("Nested target");
        IAssetReferenceResolver assets = m_serializationContext.GetRequired<IAssetReferenceResolver>();
        probe.beforeRead = () =>
        {
            byte[] value = ScenePropertySerialization.CaptureProperty(nested, nameof(ReferenceComponent.targetObject), m_serialization, assets);
            nested.targetObject = null;
            _ = ScenePropertySerialization.RestoreProperties(nested, value, m_serialization, assets);
            Assert.NotNull(nested.targetObject);
        };
        byte[] properties = ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets);
        GameObject original = Assert.IsType<GameObject>(probe.targetObject);
        probe.targetObject = null;
        _ = ScenePropertySerialization.RestoreProperties(probe, properties, m_serialization, assets);
        Assert.Same(original, probe.targetObject);
        probe.beforeRead = () => throw new InvalidOperationException("Expected property getter failure.");
        Assert.ThrowsAny<Exception>(() => ScenePropertySerialization.CaptureProperties(probe, m_serialization, assets));
        probe.beforeRead = null;
        // A leaked property scope would treat this independent prefab root as an out-of-bound reference.
        Assert.NotEmpty(m_serialization.Serialize(nestedScene.CreateObject("Independent prefab"), m_serializationContext));
    }
}

[StableTypeId("486290eb-6cc4-4a96-918a-c8022f51ad85")]
internal sealed class PropertyReferenceProbe : GameComponent
{
    internal Action? beforeRead { get; set; }

    [SerializableProperty(order = -1)]
    public int trigger
    {
        get
        {
            beforeRead?.Invoke();
            return 0;
        }
        set { }
    }

    [SerializableProperty]
    public GameObject? targetObject { get; set; }

    [SerializableProperty]
    public GameComponent? targetComponent { get; set; }

    [SerializableProperty]
    public GameSystem? targetSystem { get; set; }
}
