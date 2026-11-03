using System;
using System.Linq;
using System.Runtime.CompilerServices;

using Inno.Assets;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;
using Xunit;

namespace Inno.Editor.Interactions.Tests;

public sealed partial class SceneHistoryTests
{
    [Fact]
    public void PrefabCreationAndDependentEditingReplayWithStableInstanceIdentity()
    {
        GameScene scene = CreateScene();
        GameObject source = scene.CreateObject("Source");
        source.AddComponent<HistoryComponent>().value = 7;
        PrefabAsset captured = PrefabAsset.Capture(source, m_host.serialization, m_assets);
        var path = AssetPath.Project("Prefabs/History.iprefab");
        Assert.True(m_assets.Save(path, captured));
        PrefabAsset prefab = m_assets.Load<PrefabAsset>(path);
        Guid instanceId;
        Guid componentId;
        using (EditorHistoryTransaction transaction = m_runtime.interactions.history.BeginTransaction("Create configured prefab"))
        {
            GameObject instance = m_edits.InstantiatePrefab(prefab, scene);
            HistoryComponent component = instance.GetComponent<HistoryComponent>();
            instanceId = instance.identity.persistentId;
            componentId = component.identity.persistentId;
            m_edits.RenameGameObject(instance, "Instance");
            Assert.True(m_edits.ChangeProperty(component, nameof(HistoryComponent.value), () => component.value = 42, "Configure"));
            transaction.Commit();
        }
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Null(IdentityAllocator.current.Get<GameObject>(instanceId));
        EditorHistoryResult replay = m_runtime.interactions.history.Redo();
        Assert.True(replay.succeeded, replay.message);
        var restored = Assert.IsType<GameObject>(IdentityAllocator.current.Get<GameObject>(instanceId));
        Assert.Equal("Instance", restored.name);
        Assert.True(restored.isPartOfPrefabInstance);
        Assert.Equal(prefab.identity.persistentId, restored.prefabInstance!.sourceAssetId);
        var restoredComponent = Assert.IsType<HistoryComponent>(IdentityAllocator.current.Get<GameComponent>(componentId));
        Assert.Same(restored, restoredComponent.gameObject);
        Assert.Equal(42, restoredComponent.value);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(7, source.GetComponent<HistoryComponent>().value);
    }

    [Fact]
    public void PropertyAvailabilityDoesNotReadOrSerializeLiveValues()
    {
        var component = CreateScene().CreateObject().AddComponent<HistoryValueProbe>();
        Assert.True(m_edits.ChangeProperty(component, nameof(HistoryValueProbe.value), () => component.value = 3, "Change value"));
        HistoryValueProbe.reads = 0;
        for (int i = 0; i < 10; i++)
        {
            Assert.True(m_runtime.interactions.history.canUndo);
            Assert.Null(m_runtime.interactions.history.undoUnavailableReason);
        }
        Assert.Equal(0, HistoryValueProbe.reads);
    }

    [Fact]
    public void FailedDependentRedoCompensatesCreationAndPreservesTheEntryForRetry()
    {
        GameScene scene = CreateScene();
        Guid objectId;
        using (EditorHistoryTransaction outer = m_runtime.interactions.history.BeginTransaction("Create validated image"))
        {
            GameObject owner = m_edits.CreateGameObject(scene);
            objectId = owner.identity.persistentId;
            m_edits.RenameGameObject(owner, "Image");
            using (EditorHistoryTransaction inner = m_runtime.interactions.history.BeginTransaction("Configure image"))
            {
                var component = (HistoryValueProbe)m_edits.AddComponent(owner, typeof(HistoryValueProbe));
                Assert.True(m_edits.ChangeProperty(component, nameof(HistoryValueProbe.value), () => component.value = 42, "Set value"));
                inner.Commit();
            }
            outer.Commit();
        }
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        HistoryValueProbe.rejectAnswer = true;
        try
        {
            EditorHistoryResult failure = m_runtime.interactions.history.Redo();
            Assert.False(failure.succeeded);
            Assert.True(failure.statePreserved, failure.message);
            Assert.Null(IdentityAllocator.current.Get<GameObject>(objectId));
            Assert.Equal("Create validated image", m_runtime.interactions.history.redoName);
        }
        finally
        {
            HistoryValueProbe.rejectAnswer = false;
        }
        EditorHistoryResult retry = m_runtime.interactions.history.Redo();
        Assert.True(retry.succeeded, retry.message);
        Assert.Equal("Image", IdentityAllocator.current.Get<GameObject>(objectId)!.name);
    }

    [Fact]
    public void DependentPropertyReplayWaitsForItsPluginGenerationToReturn()
    {
        ReloadHistoryModule(install: true);
        GameScene scene = CreateScene();
        RecordReloadableCreation(scene);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        ReloadHistoryModule(install: false);
        m_host.modules.generations.Wait();
        Assert.False(m_runtime.interactions.history.canRedo);
        Assert.False(m_runtime.interactions.history.Redo().succeeded);
        Assert.Equal("Create reloadable image", m_runtime.interactions.history.redoName);
        Assert.Empty(scene.GetObjects());
        ReloadHistoryModule(install: true);
        Assert.True(m_runtime.interactions.history.canRedo, m_runtime.interactions.history.redoUnavailableReason);
        EditorHistoryResult redo = m_runtime.interactions.history.Redo();
        Assert.True(redo.succeeded, redo.message);
        Assert.Single(scene.GetObjects());
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        ReloadHistoryModule(install: false);
        m_host.modules.generations.Wait();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void RecordReloadableCreation(GameScene scene)
    {
        Assert.True(m_host.types.TryResolve(new TypeRef(new Guid("9f67d41e-082b-46d5-aaf0-dfc76c693182")), out Type? type));
        using EditorHistoryTransaction transaction = m_runtime.interactions.history.BeginTransaction("Create reloadable image");
        GameObject owner = m_edits.CreateGameObject(scene);
        GameComponent component = m_edits.AddComponent(owner, type!);
        SerializedProperty property = m_host.serialization.GetProperties(component).Single(value => value.name == "value");
        Assert.True(m_edits.ChangeProperty(component, property.name, () => property.SetValue(42), "Set value"));
        transaction.Commit();
    }

    [Fact]
    public void DependentCreationReplaysAsOneAtomicHistoryEntry()
    {
        GameScene scene = CreateScene();
        Guid objectId;
        Guid componentId;
        using (EditorHistoryTransaction transaction = m_runtime.interactions.history.BeginTransaction("Create image"))
        {
            GameObject owner = m_edits.CreateGameObject(scene);
            var component = (HistoryComponent)m_edits.AddComponent(owner, typeof(HistoryComponent));
            objectId = owner.identity.persistentId;
            componentId = component.identity.persistentId;
            Assert.True(m_edits.ChangeProperty(component, nameof(HistoryComponent.value), () => component.value = 42, "Set image value"));
            transaction.Commit();
        }

        Assert.Equal("Create image", m_runtime.interactions.history.undoName);
        EditorHistoryResult undo = m_runtime.interactions.history.Undo();
        Assert.True(undo.succeeded, undo.message);
        Assert.Null(IdentityAllocator.current.Get<GameObject>(objectId));
        Assert.Null(IdentityAllocator.current.Get<GameComponent>(componentId));
        Assert.True(m_runtime.interactions.history.canRedo, m_runtime.interactions.history.redoUnavailableReason);
        EditorHistoryResult redo = m_runtime.interactions.history.Redo();
        Assert.True(redo.succeeded, redo.message);
        var restored = Assert.IsType<HistoryComponent>(IdentityAllocator.current.Get<GameComponent>(componentId));
        Assert.Equal(42, restored.value);
        Assert.Same(IdentityAllocator.current.Get<GameObject>(objectId), restored.gameObject);
        Assert.Equal("Create image", m_runtime.interactions.history.undoName);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Null(IdentityAllocator.current.Get<GameObject>(objectId));
    }
}
