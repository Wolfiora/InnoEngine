using System;
using Inno.Core.Identity;
using Inno.Core.Serialization;
using Inno.Extensibility.Types;
using Inno.Scene;
using Inno.Scene.Components;
using Xunit;

namespace Inno.Editor.Interactions.Tests;
public sealed partial class SceneHistoryTests
{
    [Fact]
    public void BatchPropertyEditPreservesIndependentBeforeValuesInOneUndo()
    {
        GameScene scene = CreateScene();
        var first = scene.CreateObject("First").AddComponent<HistoryComponent>();
        var second = scene.CreateObject("Second").AddComponent<HistoryComponent>();
        first.value = 12;
        second.value = 34;
        Assert.False(m_edits.HaveSamePropertyValues([first, second], "value"));
        Assert.True(m_edits.ChangeProperties([first, second], "value", () => first.value = 42, "Edit Selection"));
        Assert.Equal(42, first.value);
        Assert.Equal(42, second.value);
        Assert.True(m_edits.HaveSamePropertyValues([first, second], "value"));
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(12, first.value);
        Assert.Equal(34, second.value);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        Assert.Equal(42, first.value);
        Assert.Equal(42, second.value);
    }

    [Fact]
    public void BatchAxisEditPreservesUneditedComponentsIncludingEqualPrimarySamples()
    {
        GameScene scene = CreateScene();
        var first = scene.CreateObject("First").transform;
        var second = scene.CreateObject("Second").transform;
        first.localPosition = new(1, 2, 3);
        second.localPosition = new(4, 5, 6);
        Assert.True(m_edits.ChangeProperties([first, second], "localPosition", () => first.localPosition = new(9, 2, 3), "Edit X", componentName: "x"));
        Assert.Equal(new Inno.Core.Mathematics.Vector3(9, 5, 6), second.localPosition);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Equal(new Inno.Core.Mathematics.Vector3(4, 5, 6), second.localPosition);
        Assert.True(m_edits.ChangeProperties([first, second], "localPosition", () => { }, "Use Primary X", componentName: "x"));
        Assert.Equal(new Inno.Core.Mathematics.Vector3(1, 5, 6), second.localPosition);
        Assert.Throws<ArgumentException>(() => m_edits.ChangeProperties([first, second], "localPosition", () => first.localPosition = default, "Rejected Component", componentName: "notAComponent"));
        Assert.Equal(new Inno.Core.Mathematics.Vector3(1, 2, 3), first.localPosition);
    }

    [Fact]
    public void LaterBatchSetterFailureRestoresAllEarlierTargets()
    {
        GameScene scene = CreateScene();
        var first = scene.CreateObject("First").AddComponent<BatchValidationComponent>();
        var second = scene.CreateObject("Second").AddComponent<BatchValidationComponent>();
        first.value = 12;
        second.value = 34;
        second.rejectAnswer = true;
        Assert.ThrowsAny<Exception>(() => m_edits.ChangeProperties([first, second], "value", () => first.value = 42, "Rejected Selection"));
        Assert.Equal(12, first.value);
        Assert.Equal(34, second.value);
        Assert.False(m_runtime.interactions.history.canUndo);
    }

    [Fact]
    public void BatchDeleteNormalizesSelectedAncestorsAndRestoresEverySubtree()
    {
        GameScene scene = CreateScene();
        GameObject root = scene.CreateObject("Root");
        GameObject child = scene.CreateObject("Child");
        GameObject other = scene.CreateObject("Other");
        child.transform.SetParent(root.transform);
        Guid rootId = root.identity.persistentId;
        Guid childId = child.identity.persistentId;
        Guid otherId = other.identity.persistentId;
        m_edits.DeleteGameObjects([child, root, other]);
        Assert.Null(IdentityAllocator.current.Get<GameObject>(rootId));
        Assert.Null(IdentityAllocator.current.Get<GameObject>(childId));
        Assert.Null(IdentityAllocator.current.Get<GameObject>(otherId));
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        GameObject restoredRoot = Assert.IsType<GameObject>(IdentityAllocator.current.Get<GameObject>(rootId));
        GameObject restoredChild = Assert.IsType<GameObject>(IdentityAllocator.current.Get<GameObject>(childId));
        Assert.Same(restoredRoot.transform, restoredChild.transform.parent);
        Assert.NotNull(IdentityAllocator.current.Get<GameObject>(otherId));
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        Assert.Null(IdentityAllocator.current.Get<GameObject>(rootId));
    }

    [Fact]
    public void BatchMovePreservesRootOrderAndRestoresIndependentPlacements()
    {
        GameScene scene = CreateScene();
        GameObject first = scene.CreateObject("First");
        GameObject second = scene.CreateObject("Second");
        GameObject destination = scene.CreateObject("Destination");
        GameObject child = scene.CreateObject("Child");
        child.transform.SetParent(first.transform);
        m_edits.MoveGameObjects([first, child, second], scene, destination);
        Assert.Same(destination.transform, first.transform.parent);
        Assert.Same(destination.transform, second.transform.parent);
        Assert.Same(first.transform, child.transform.parent);
        Assert.Equal(0, first.transform.siblingIndex);
        Assert.Equal(1, second.transform.siblingIndex);
        Assert.True(m_runtime.interactions.history.Undo().succeeded);
        Assert.Null(first.transform.parent);
        Assert.Null(second.transform.parent);
        Assert.Same(first.transform, child.transform.parent);
        Assert.True(m_runtime.interactions.history.Redo().succeeded);
        Assert.Same(destination.transform, second.transform.parent);
        Assert.Throws<InvalidOperationException>(() => m_edits.MoveGameObjects([destination], scene, child));
        Assert.Same(destination.transform, first.transform.parent);
    }
}

[StableTypeId("9ea6d04d-4ed3-48f3-ac3a-b09372a575ab")]
internal sealed class BatchValidationComponent : GameComponent
{
    [SerializableProperty]
    public int value { get; set; }

    [SerializableProperty]
    public bool rejectAnswer { get; set; }

    [OnSerializableRestored]
    private void Validate()
    {
        if (rejectAnswer && value == 42)
            throw new InvalidOperationException("The current owner rejected this batch value.");
    }
}
