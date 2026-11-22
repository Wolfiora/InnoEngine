using System;
using Xunit;
using Inno.Core.Identity;

namespace Inno.Core.Identity.Tests;

public sealed class IdentityAllocatorTests
{
    private readonly IdentityAllocator m_allocator = new();

    [Fact]
    public void GetIdentity_DoesNotAutoRegisterObject()
    {
        var entity = new Entity();

        Identity identity = GetIdentity(entity);
        Assert.NotEqual(Guid.Empty, identity.persistentID);
        Assert.Null(identity.runtimeID);
    }

    [Fact]
    public void InitializePersistentIdentity_AssignsDetachedIdentityWithoutUnregisterEvent()
    {
        var entity = new Entity();
        Guid persistentID = Guid.NewGuid();
        int unregisteredCount = 0;

        void OnUnregistered(IdentityObject _) => unregisteredCount++;
        m_allocator.objectUnregistered += OnUnregistered;
        try
        {
            m_allocator.InitializePersistentIdentity(entity, persistentID);

            Assert.Equal(persistentID, GetIdentity(entity).persistentID);
            Assert.Null(GetIdentity(entity).runtimeID);
            Assert.Equal(0, unregisteredCount);
        }
        finally
        {
            m_allocator.objectUnregistered -= OnUnregistered;
        }
    }

    [Fact]
    public void Register_AndGet_ByRuntimeID_AndPersistentID_AndExplicitPersistentOverride()
    {
        var entity = new Entity();
        Assert.True(m_allocator.Register(entity, Guid.Parse("11111111-1111-1111-1111-111111111111")));
        AssertRuntimeIDNotNull(entity);

        Identity identity = GetIdentity(entity);
        Assert.Equal(Guid.Parse("11111111-1111-1111-1111-111111111111"), identity.persistentID);
        int runtimeID = identity.runtimeID!.Value;
        Assert.Same(entity, m_allocator.Get<IdentityObject>(runtimeID));
        Assert.Same(entity, m_allocator.Get<IdentityObject>(identity.persistentID));
        Assert.Same(entity, m_allocator.Get<Entity>(runtimeID));
        Assert.Null(m_allocator.Get<UnrelatedEntity>(runtimeID));
    }

    [Fact]
    public void RuntimeIdentity_RejectsAnEqualNumericIDFromAnotherDomain()
    {
        var first = new Entity();
        var second = new Entity();
        var otherAllocator = new IdentityAllocator();
        Assert.True(m_allocator.Register(first));
        Assert.True(otherAllocator.Register(second));

        RuntimeIdentity firstIdentity = first.identity.runtimeIdentity!.Value;
        RuntimeIdentity secondIdentity = second.identity.runtimeIdentity!.Value;

        Assert.Equal(firstIdentity.runtimeID, secondIdentity.runtimeID);
        Assert.NotEqual(firstIdentity.domainID, secondIdentity.domainID);
        Assert.Same(first, m_allocator.Get<Entity>(firstIdentity));
        Assert.Null(m_allocator.Get<Entity>(secondIdentity));
    }

    [Fact]
    public void Register_ReturnsFalse_WhenAlreadyRegistered()
    {
        var entity = new Entity();
        Assert.True(m_allocator.Register(entity));
        int firstRuntimeID = GetIdentity(entity).runtimeID!.Value;

        Assert.False(m_allocator.Register(entity));
        Assert.Equal(firstRuntimeID, GetIdentity(entity).runtimeID);
    }

    [Fact]
    public void Unregister_RemovesRuntimeID_AndGetReturnsNull()
    {
        var entity = new Entity();
        m_allocator.Register(entity);
        int runtimeID = GetIdentity(entity).runtimeID!.Value;

        Assert.True(m_allocator.Unregister(entity));
        AssertRuntimeIDIsNull(entity);
        Assert.Null(m_allocator.Get<IdentityObject>(runtimeID));
        Assert.Null(m_allocator.Get<IdentityObject>(Guid.Empty));
    }

    [Fact]
    public void Unregister_EventRunsAfterRemoval_InvokesEveryHandler_AndAggregatesFailures()
    {
        var entity = new Entity();
        Assert.True(m_allocator.Register(entity));
        Identity identity = GetIdentity(entity);
        int successfulHandlers = 0;

        void FailingHandler(IdentityObject removed)
        {
            Assert.Same(entity, removed);
            Assert.Null(GetIdentity(removed).runtimeID);
            Assert.Null(m_allocator.Get<Entity>(identity.persistentID));
            throw new InvalidOperationException("expected handler failure");
        }

        void SuccessfulHandler(IdentityObject removed)
        {
            Assert.Same(entity, removed);
            successfulHandlers++;
        }

        m_allocator.objectUnregistered += FailingHandler;
        m_allocator.objectUnregistered += SuccessfulHandler;
        try
        {
            AggregateException exception = Assert.Throws<AggregateException>(() => m_allocator.Unregister(entity));

            Assert.Single(exception.InnerExceptions);
            Assert.Equal(1, successfulHandlers);
            Assert.Null(GetIdentity(entity).runtimeID);
        }
        finally
        {
            m_allocator.objectUnregistered -= FailingHandler;
            m_allocator.objectUnregistered -= SuccessfulHandler;
        }
    }

    [Fact]
    public void SlotReuse_ChangesGeneration_WhenReRegistered()
    {
        var first = new Entity();
        m_allocator.Register(first);
        int firstRuntimeID = GetIdentity(first).runtimeID!.Value;

        Assert.True(m_allocator.Unregister(first));
        Assert.Null(m_allocator.Get<Entity>(firstRuntimeID));

        var second = new Entity();
        m_allocator.Register(second);
        int secondRuntimeID = GetIdentity(second).runtimeID!.Value;

        Assert.Equal(UnpackSlot(firstRuntimeID), UnpackSlot(secondRuntimeID));
        Assert.NotEqual(UnpackGeneration(firstRuntimeID), UnpackGeneration(secondRuntimeID));
        Assert.Same(second, m_allocator.Get<IdentityObject>(secondRuntimeID));
    }

    [Fact]
    public void Register_RequiresUniquePersistentID()
    {
        Guid fixedID = Guid.NewGuid();
        var first = new Entity();
        Assert.True(m_allocator.Register(first, fixedID));
        var second = new Entity();

        Assert.Throws<InvalidOperationException>(() => m_allocator.Register(second, fixedID));
    }

    [Fact]
    public void Get_ReturnsNull_ForInvalidLookupKeys()
    {
        Assert.Null(m_allocator.Get<IdentityObject>(int.MaxValue));
        Assert.Null(m_allocator.Get<IdentityObject>(Guid.Empty));
    }

    [Fact]
    public void RegisteredIdentity_DoesNotPreventGarbageCollection()
    {
        int runtimeID;
        WeakReference<Entity> weakEntity;

        weakEntity = CreateRegisteredEntity(out runtimeID);

        for (int i = 0; i < 8; i++)
        {
            GC.Collect(2, GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced, true, true);
        }

        for (int i = 0; i < 8; i++)
        {
            GC.Collect(2, GCCollectionMode.Forced, true, true);
            GC.WaitForPendingFinalizers();
            GC.Collect(2, GCCollectionMode.Forced, true, true);
        }

        Assert.False(weakEntity.TryGetTarget(out _));
        Assert.Null(m_allocator.Get<Entity>(runtimeID));
    }

    private WeakReference<Entity> CreateRegisteredEntity(out int runtimeID)
    {
        var entity = new Entity();
        m_allocator.Register(entity);
        runtimeID = GetIdentity(entity).runtimeID!.Value;
        return new WeakReference<Entity>(entity);
    }

    private static void AssertRuntimeIDNotNull(IdentityObject target) => Assert.NotNull(target.identity.runtimeID);

    private static void AssertRuntimeIDIsNull(IdentityObject target) => Assert.Null(target.identity.runtimeID);

    private static int UnpackSlot(int runtimeID) => runtimeID & 0x000F_FFFF;

    private static int UnpackGeneration(int runtimeID) => (runtimeID >> 20) & 0x0000_0FFF;

    private static Identity GetIdentity(IdentityObject target) => target.identity;

    private sealed class Entity : IdentityObject;

    private sealed class UnrelatedEntity : IdentityObject;
}
