using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Inno.Core.Identity;
using Inno.Editor.Core;
using Inno.Editor.Interactions;
using Xunit;

namespace Inno.Editor.Interactions.Tests;
public sealed partial class EditorRuntimeTests
{
    [Fact]
    public void MissingIdentityRejectsTheEntireVisibleRangeWithoutChangingSelection()
    {
        var first = new SelectionQueueTarget();
        var last = new SelectionQueueTarget();
        Assert.True(m_identities.Register(first));
        Assert.True(m_identities.Register(last));
        m_runtime.interactions.SetSelection(first);
        EditorSelectionSnapshot original = m_runtime.interactions.selection.Capture();
        RuntimeIdentity firstId = first.identity.runtimeIdentity!.Value;
        RuntimeIdentity missing = new(firstId.domainId, int.MaxValue);
        Assert.Throws<InvalidOperationException>(() => m_runtime.interactions.SelectRange(
            [firstId, missing, last.identity.runtimeIdentity!.Value], last));
        Assert.Same(original, m_runtime.interactions.selection.Capture());
        Assert.Same(first, m_runtime.interactions.selection.selectedTarget);
    }

    [Fact]
    public void LargeIdentityRangesKeepOrderedPrimaryAndIndexedMembership()
    {
        var targets = new SelectionQueueTarget[10_000];
        var order = new RuntimeIdentity[targets.Length];
        for (int index = 0; index < targets.Length; index++)
        {
            targets[index] = new SelectionQueueTarget();
            Assert.True(m_identities.Register(targets[index]));
            order[index] = targets[index].identity.runtimeIdentity!.Value;
        }
        m_runtime.interactions.SetSelection(targets[0]);
        m_runtime.interactions.SelectRange(order, targets[^1]);
        Assert.Equal(targets.Length, m_runtime.interactions.selection.count);
        Assert.Same(targets[^1], m_runtime.interactions.selection.selectedTarget);
        foreach (SelectionQueueTarget target in targets)
            Assert.True(m_runtime.interactions.selection.Contains(target));
        m_runtime.interactions.SetSelection(targets[0]);
        Assert.False(m_runtime.interactions.selection.Contains(targets[^1]));
    }

    [Fact]
    public void BoxedTargetsDoNotKeepReferencedObjectsAliveThroughSelectionFocusOrSnapshots()
    {
        WeakReference reference = SelectBoxedReferenceTarget();
        EditorSelectionSnapshot snapshot = m_runtime.interactions.selection.Capture();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(reference.IsAlive);
        Assert.Null(m_runtime.interactions.selection.selectedTarget);
        Assert.Null(m_runtime.interactions.focusedTarget);
        Assert.Equal(0, snapshot.CopyTargets(m_runtime.interactions, new List<object>()));
        GC.KeepAlive(snapshot);
    }

    [Fact]
    public void NeutralScalarSelectionRemainsAvailableWithoutAnotherOwner()
    {
        Guid value = Guid.NewGuid();
        SelectScalarTarget(value);
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.Equal(value, Assert.IsType<Guid>(m_runtime.interactions.selection.selectedTarget));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private void SelectScalarTarget(Guid value) => m_runtime.interactions.SetSelection(value);

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference SelectBoxedReferenceTarget()
    {
        var payload = new object();
        object target = new KeyValuePair<string, object>("payload", payload);
        m_runtime.interactions.SetSelection(target);
        m_runtime.interactions.For("tests/boxed-target", target).Focus();
        return new WeakReference(payload);
    }

    [Fact]
    public void RejectedDomainRetirementOnAnotherThreadPreservesTheRoutingLease()
    {
        var allocator = new IdentityAllocator();
        var target = new SelectionQueueTarget();
        Assert.True(allocator.Register(target));
        using IDisposable registration = m_runtime.interactions.RegisterIdentityDomain(allocator);
        Exception? failure = null;
        var worker = new System.Threading.Thread(() => failure = Record.Exception(registration.Dispose));
        worker.Start();
        Assert.True(worker.Join(TimeSpan.FromSeconds(5)));
        Assert.IsType<InvalidOperationException>(failure);
        Assert.True(m_runtime.interactions.TryResolveIdentity(target.identity.runtimeIdentity!.Value, out IdentityObject? resolved));
        Assert.Same(target, resolved);
    }

    [Fact]
    public void TemporaryIdentityDomainsResolveSelectionAndDragOnlyWhileTheirRoutingLeaseIsAlive()
    {
        var allocator = new IdentityAllocator();
        var target = new SelectionQueueTarget();
        Assert.True(allocator.Register(target));
        RuntimeIdentity identity = target.identity.runtimeIdentity!.Value;
        Assert.False(m_runtime.interactions.TryResolveIdentity(identity, out _));
        IDisposable registration = m_runtime.interactions.RegisterIdentityDomain(allocator);
        Assert.Throws<InvalidOperationException>(() => m_runtime.interactions.RegisterIdentityDomain(allocator));
        Assert.True(m_runtime.interactions.TryResolveIdentity(identity, out IdentityObject? resolved));
        Assert.Same(target, resolved);
        m_runtime.interactions.SetSelection(target);
        Assert.Same(target, m_runtime.interactions.selection.selectedTarget);
        RuntimeIdentity drag = m_runtime.interactions.For("tests/temporary", target).BeginDrag(new EditorDragData(target, "temporary"));
        Assert.True(m_runtime.interactions.TryGetDragSource(drag, out _));
        registration.Dispose();
        registration.Dispose();
        Assert.False(m_runtime.interactions.TryResolveIdentity(identity, out _));
        Assert.False(m_runtime.interactions.TryGetDragSource(drag, out _));
        Assert.Null(m_runtime.interactions.selection.selectedTarget);
        Assert.Equal(1, m_runtime.interactions.selection.count);
        Assert.NotNull(allocator.Get<IdentityObject>(identity));
        using IDisposable rejoined = m_runtime.interactions.RegisterIdentityDomain(allocator);
        registration.Dispose();
        Assert.Same(target, m_runtime.interactions.selection.selectedTarget);
        Assert.False(m_runtime.interactions.TryGetActiveDragIdentity(out _));
    }

    [Fact]
    public void SelectionFinishesPendingEditsBeforeChangingAndPreservesStateWhenPreparationRejects()
    {
        var first = new SelectionQueueTarget();
        var second = new SelectionQueueTarget();
        Assert.True(m_identities.Register(first));
        Assert.True(m_identities.Register(second));
        m_runtime.interactions.SetSelection(first);
        using var hub = m_runtime.interactions.events.CreateHub();
        using var gesture = new EditorEditGesture(m_runtime.interactions.history, "Pending Input");
        using (hub.Listen<EditorOperationPreparingEvent>(_ =>
        {
            Assert.Same(first, m_runtime.interactions.selection.selectedTarget);
            gesture.Commit();
        }))
        {
            m_runtime.interactions.SetSelection(second);
        }
        Assert.False(gesture.isActive);
        ulong revision = m_runtime.interactions.selection.version;
        using var rejecting = hub.Listen<EditorOperationPreparingEvent>(_ => throw new InvalidOperationException("Pending edit cannot finish."));
        Assert.Throws<InvalidOperationException>(() => m_runtime.interactions.SetSelection(first));
        Assert.Throws<InvalidOperationException>(() => m_runtime.interactions.SetSelection([first], first));
        Assert.Same(second, m_runtime.interactions.selection.selectedTarget);
        Assert.Equal(revision, m_runtime.interactions.selection.version);
    }

    [Fact]
    public void OrderedSelectionDeduplicatesIdentitiesAndRetainsItsSnapshotUntilItChanges()
    {
        var first = new SelectionQueueTarget();
        var second = new SelectionQueueTarget();
        Assert.True(m_identities.Register(first));
        Assert.True(m_identities.Register(second));
        m_runtime.interactions.SetSelection([first, second, first], first);
        Assert.Equal(2, m_runtime.interactions.selection.count);
        Assert.Same(second, m_runtime.interactions.selection.selectedTarget);
        Assert.Same(first, m_runtime.interactions.selection.anchor);
        EditorSelectionSnapshot snapshot = m_runtime.interactions.selection.Capture();
        Assert.Equal(first.identity.runtimeIdentity, snapshot.identities[0]);
        Assert.Equal(second.identity.runtimeIdentity, snapshot.primary);
        ulong revision = m_runtime.interactions.selection.version;
        m_runtime.interactions.SetSelection([first, second], first);
        Assert.Equal(revision, m_runtime.interactions.selection.version);
        Assert.Same(snapshot, m_runtime.interactions.selection.Capture());
    }

    [Fact]
    public void RangeSelectionUsesVisibleOrderAndTogglePreservesTheAnchor()
    {
        var first = new SelectionQueueTarget();
        var second = new SelectionQueueTarget();
        var third = new SelectionQueueTarget();
        Assert.True(m_identities.Register(first));
        Assert.True(m_identities.Register(second));
        Assert.True(m_identities.Register(third));
        m_runtime.interactions.SetSelection(first);
        m_runtime.interactions.SelectRange([first.identity.runtimeIdentity!.Value, second.identity.runtimeIdentity!.Value, third.identity.runtimeIdentity!.Value], third);
        Assert.Equal(3, m_runtime.interactions.selection.count);
        m_runtime.interactions.ToggleSelection(second);
        Assert.False(m_runtime.interactions.selection.Contains(second));
        Assert.Same(first, m_runtime.interactions.selection.anchor);
        Assert.Same(third, m_runtime.interactions.selection.selectedTarget);
        m_runtime.interactions.SelectRange([third.identity.runtimeIdentity!.Value, second.identity.runtimeIdentity!.Value, first.identity.runtimeIdentity!.Value], second);
        var targets = new List<SelectionQueueTarget>();
        Assert.Equal(2, m_runtime.interactions.selection.CopyTargets(targets));
        Assert.Equal([first, second], targets);
    }

    [Fact]
    public void SelectionNeverConflatesPersistentIdentitiesFromDifferentDomains()
    {
        Guid persistent = Guid.NewGuid();
        var first = new SelectionQueueTarget();
        var second = new SelectionQueueTarget();
        Assert.True(m_identities.Register(first, persistent));
        Assert.True(m_secondaryIdentities.Register(second, persistent));
        m_runtime.interactions.SetSelection([first, second], first);
        Assert.Equal(2, m_runtime.interactions.selection.count);
        Assert.True(m_runtime.interactions.selection.Contains(first));
        Assert.True(m_runtime.interactions.selection.Contains(second));
        Assert.NotEqual(first.identity.runtimeIdentity, second.identity.runtimeIdentity);
    }

    [Fact]
    public void RejectedSelectionDoesNotModifyItsPreviousMembershipOrRevision()
    {
        var registered = new SelectionQueueTarget();
        Assert.True(m_identities.Register(registered));
        m_runtime.interactions.SetSelection(registered);
        ulong revision = m_runtime.interactions.selection.version;
        var unregistered = new SelectionQueueTarget();
        Assert.Throws<InvalidOperationException>(() => m_runtime.interactions.SetSelection([registered, unregistered], registered));
        Assert.Equal(revision, m_runtime.interactions.selection.version);
        Assert.Same(registered, m_runtime.interactions.selection.selectedTarget);
    }

    [Fact]
    public void QueuedActionsRejectRetiredTargetsInsteadOfExecutingTheirReplacement()
    {
        Guid persistent = Guid.NewGuid();
        var previous = new SelectionQueueTarget();
        Assert.True(m_identities.Register(previous, persistent));
        m_runtime.interactions.For("tests/identity-queue", previous).Enqueue("tests/identity-queue");
        Assert.True(m_identities.Unregister(previous));
        var replacement = new SelectionQueueTarget();
        Assert.True(m_identities.Register(replacement, persistent));
        m_runtime.Update(new EditorFrame());
        Assert.Equal(0, previous.executions);
        Assert.Equal(0, replacement.executions);
        m_runtime.interactions.For("tests/identity-queue", replacement).Enqueue("tests/identity-queue");
        m_runtime.Update(new EditorFrame());
        Assert.Equal(1, replacement.executions);
    }

    [Fact]
    public void QueuedSelectionAndFocusDoNotKeepRetiredIdentityObjectsAlive()
    {
        WeakReference reference = QueueAndRetireSelectionTarget();
        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();
        Assert.False(reference.IsAlive);
        Assert.Null(m_runtime.interactions.selection.selectedTarget);
        Assert.Null(m_runtime.interactions.focusedTarget);
        m_runtime.Update(new EditorFrame());
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference QueueAndRetireSelectionTarget()
    {
        var target = new SelectionQueueTarget();
        Assert.True(m_identities.Register(target));
        m_runtime.interactions.SetSelection(target);
        m_runtime.interactions.For("tests/identity-queue", target).Focus();
        m_runtime.interactions.For("tests/identity-queue", target).Enqueue("tests/identity-queue");
        Assert.True(m_identities.Unregister(target));
        return new WeakReference(target);
    }

    public sealed class SelectionQueueTarget : IdentityObject
    {
        public int executions { get; set; }
    }

    [EditorAction("tests/identity-queue")]
    public sealed class IdentityQueuedAction : EditorAction<SelectionQueueTarget>
    {
        protected override void Execute(EditorActionContext<SelectionQueueTarget> context) => context.target.executions++;
    }
}
