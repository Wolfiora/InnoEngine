using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Scene.Layers;
using Xunit;

namespace Inno.Scene.Tests;

public sealed partial class SceneSerializationTests
{
    [Fact]
    public void StructuralCaptureDoesNotRetainRemovedSystemsWhileTheSceneRemainsAlive()
    {
        var scene = new GameScene("System capture lifetime");
        scene.CreateObject("Root");
        SceneManager.LoadScene(scene);
        WeakReference removed = CaptureAndRemoveSystem(scene);
        SceneManager.Update(0.016f);
        SceneManager.LateUpdate(0.016f);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(removed.IsAlive);

        var replacement = scene.AddSystem<OrderSerializationSystemB>();
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(scene, m_serializationContext);
        Assert.Same(replacement, Assert.Single(scene.GetSystems()));
        Assert.Equal(m_serialization.Serialize(scene, m_serializationContext), snapshot.ToArray());
        GC.KeepAlive(scene);
    }

    [Fact]
    public void CompleteSceneCapturesObserveAllOwnedMetadataTransformAndStructureWrites()
    {
        var scene = new GameScene("Capture coverage");
        SceneManager.LoadScene(scene);
        GameObject first = scene.CreateObject("First");
        GameObject second = scene.CreateObject("Second");
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(scene, m_serializationContext);
        Action[] mutations =
        [
            () => scene.name = "Changed scene",
            () => first.name = "Changed object",
            () => first.tag = "Player",
            () => first.layer = new GameLayer(4),
            () => first.SetActive(false),
            () => first.transform.localPosition = new Vector3(2, 3, 4),
            () => first.transform.localRotation = new Quaternion(0.2f, 0.1f, 0, 0.9f),
            () => first.transform.localScale = new Vector3(2, 3, 4),
            () => first.transform.worldPosition = new Vector3(5, 6, 7),
            () => first.transform.worldRotation = new Quaternion(0, 0.3f, 0.1f, 0.8f),
            () => first.transform.worldScale = new Vector3(3, 4, 5),
            () => second.transform.SetParent(first.transform),
            () => second.transform.SetWorldTransform(new Vector3(1, 2, 3), Quaternion.identity, Vector3.ONE),
            () => second.transform.SetParent(null),
            () => second.transform.SetSiblingIndex(0),
            () => first.AddComponent<OrderComponentA>(),
            () => first.AddComponent<OrderComponentB>(),
            () => first.SetComponentIndex(first.GetComponent<OrderComponentB>(), 1),
            () => first.RemoveComponent(first.GetComponent<OrderComponentA>()),
            () => scene.AddSystem<OrderSerializationSystemA>(),
            () => scene.AddSystem<OrderSerializationSystemB>(),
            () => scene.GetSystems().OfType<OrderSerializationSystemA>().Single().enabled = false,
            () => scene.SetSystemIndex(scene.GetSystems().OfType<OrderSerializationSystemB>().Single(), 0),
            () => scene.RemoveSystem(scene.GetSystems().OfType<OrderSerializationSystemA>().Single()),
            () => scene.CreateObject("Added object"),
            () => scene.DestroyObject(second)
        ];
        foreach (Action mutate in mutations)
        {
            byte[] before = snapshot.ToArray();
            mutate();
            snapshot = m_serialization.CaptureSnapshot(scene, m_serializationContext, snapshot);
            Assert.NotEqual(before, snapshot.ToArray());
            Assert.Equal(m_serialization.Serialize(scene, m_serializationContext), snapshot.ToArray());
            Assert.Same(snapshot, m_serialization.CaptureSnapshot(scene, m_serializationContext, snapshot));
        }
    }

    [Fact]
    public void PropertyAndCompleteSceneRestoreInvalidatePreviousCaptureInputs()
    {
        var scene = new GameScene("Restore capture");
        SceneManager.LoadScene(scene);
        GameObject value = scene.CreateObject("Original");
        byte[] properties = m_serialization.CapturePropertiesData(value.transform, m_serializationContext);
        byte[] source = m_serialization.Serialize(scene, m_serializationContext);
        value.transform.localPosition = new Vector3(3, 4, 5);
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(scene, m_serializationContext);
        m_serialization.RestorePropertiesData(value.transform, properties, context: m_serializationContext);
        SerializationSnapshot restored = m_serialization.CaptureSnapshot(scene, m_serializationContext, changed);
        Assert.Equal(source, restored.ToArray());
        Assert.NotEqual(changed.ToArray(), restored.ToArray());
        value.name = "Changed";
        changed = m_serialization.CaptureSnapshot(scene, m_serializationContext, restored);
        m_serialization.Restore(scene, source, m_serializationContext);
        restored = m_serialization.CaptureSnapshot(scene, m_serializationContext, changed);
        Assert.Equal(source, restored.ToArray());
        Assert.Equal(m_serialization.Serialize(scene, m_serializationContext), restored.ToArray());
        Assert.Same(restored, m_serialization.CaptureSnapshot(scene, m_serializationContext, restored));
    }

    [Fact]
    public void UntrackedPluginPropertiesAndObjectReferencesRemainAuthoritativelyCaptured()
    {
        var scene = new GameScene("Untracked state");
        SceneManager.LoadScene(scene);
        GameObject first = scene.CreateObject("First");
        GameObject second = scene.CreateObject("Second");
        ReferenceComponent component = first.AddComponent<ReferenceComponent>();
        SerializationSnapshot before = m_serialization.CaptureSnapshot(scene, m_serializationContext);
        component.value = 12;
        component.targetObject = second;
        SerializationSnapshot changed = m_serialization.CaptureSnapshot(scene, m_serializationContext, before);
        Assert.NotEqual(before.ToArray(), changed.ToArray());
        Assert.Equal(m_serialization.Serialize(scene, m_serializationContext), changed.ToArray());
        component.value = 0;
        component.targetObject = null;
        SerializationSnapshot restored = m_serialization.CaptureSnapshot(scene, m_serializationContext, changed);
        Assert.Equal(before.ToArray(), restored.ToArray());
    }
    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference CaptureAndRemoveSystem(GameScene scene)
    {
        var system = scene.AddSystem<OrderSerializationSystemA>();
        SerializationSnapshot snapshot = m_serialization.CaptureSnapshot(scene, m_serializationContext);
        Assert.Equal(m_serialization.Serialize(scene, m_serializationContext), snapshot.ToArray());
        Assert.True(scene.RemoveSystem(system));
        return new WeakReference(system);
    }
}
