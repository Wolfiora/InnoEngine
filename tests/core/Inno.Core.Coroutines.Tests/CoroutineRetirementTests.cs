using System;
using System.Collections;
using System.Collections.Generic;

using Inno.Core.Coroutines;

using Xunit;

namespace Inno.Core.Coroutines.Tests;

public sealed class CoroutineRetirementTests
{
    [Theory]
    [InlineData("handle")]
    [InlineData("owner")]
    [InlineData("all")]
    [InlineData("dispose")]
    public void EveryStopPathDisposesNestedEnumeratorsInReverseOrder(string stop)
    {
        using var scheduler = new CoroutineScheduler();
        var owner = new object();
        var disposed = new List<string>();
        IEnumerator child = new Probe(() => true, () => null, () => disposed.Add("child"));
        IEnumerator root = new Probe(() => true, () => child, () => disposed.Add("root"));
        CoroutineHandle handle = scheduler.StartCoroutine(owner, root);
        scheduler.Tick(0);

        switch (stop)
        {
            case "handle":
                Assert.True(scheduler.StopCoroutine(handle));
                scheduler.Tick(0);
                break;
            case "owner":
                scheduler.StopAllCoroutines(owner);
                break;
            case "all":
                scheduler.StopAllCoroutines();
                scheduler.Tick(0);
                break;
            case "dispose":
                scheduler.Dispose();
                break;
        }

        Assert.Equal(new[] { "child", "root" }, disposed);
        Assert.False(handle.isValid);
        scheduler.Dispose();
        Assert.Equal(2, disposed.Count);
    }

    [Fact]
    public void NormalCompletionDisposesChildBeforeResumingParent()
    {
        using var scheduler = new CoroutineScheduler();
        var order = new List<string>();
        var child = new Probe(() => false, () => null, () => order.Add("child"));
        scheduler.StartCoroutine(Root());
        scheduler.Tick(0);
        Assert.Equal(new[] { "child", "resume", "root" }, order);

        IEnumerator Root()
        {
            try
            {
                yield return child;
                order.Add("resume");
            }
            finally
            {
                order.Add("root");
            }
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void PendingEnumeratorIsDisposedWithoutBeingAdvanced(bool ownerStop)
    {
        using var scheduler = new CoroutineScheduler();
        var owner = new object();
        int moves = 0;
        int disposed = 0;
        CoroutineHandle handle = scheduler.StartCoroutine(owner,
            new Probe(() => { moves++; return true; }, () => null, () => disposed++));
        if (ownerStop)
            scheduler.StopAllCoroutines(owner);
        else
            scheduler.Dispose();
        Assert.False(handle.isValid);
        Assert.Equal(0, moves);
        Assert.Equal(1, disposed);
    }

    [Theory]
    [InlineData("move")]
    [InlineData("current")]
    [InlineData("wait")]
    public void CallbackFaultRetiresItsStackAndDoesNotSkipOtherCoroutines(string stage)
    {
        using var scheduler = new CoroutineScheduler();
        var failure = new InvalidOperationException(stage);
        int disposed = 0;
        int survivorTicks = 0;
        CoroutineHandle failed = scheduler.StartCoroutine(new Probe(
            () => stage == "move" ? throw failure : true,
            () => stage == "current" ? throw failure : new WaitUntil(() => throw failure),
            () => disposed++));
        scheduler.StartCoroutine(Loop(() => survivorTicks++));
        if (stage == "wait")
            scheduler.Tick(0);
        AggregateException error = Assert.Throws<AggregateException>(() => scheduler.Tick(0));
        Assert.Contains(failure, error.InnerExceptions);
        Assert.False(failed.isValid);
        Assert.Equal(1, disposed);
        Assert.Equal(stage == "wait" ? 2 : 1, survivorTicks);
        scheduler.Tick(0);
        Assert.Equal(stage == "wait" ? 3 : 2, survivorTicks);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CleanupFailuresDoNotPreventParentOrOtherCoroutineCleanup(bool dispose)
    {
        using var scheduler = new CoroutineScheduler();
        var failures = new[] { new Exception("child"), new Exception("parent"), new Exception("other") };
        var cleaned = new List<int>();
        var child = new Probe(() => true, () => null, () => Fail(0));
        scheduler.StartCoroutine(new Probe(() => true, () => child, () => Fail(1)));
        scheduler.StartCoroutine(new Probe(() => true, () => null, () => Fail(2)));
        scheduler.Tick(0);
        AggregateException error;
        if (dispose)
            error = Assert.Throws<AggregateException>(scheduler.Dispose);
        else
        {
            scheduler.StopAllCoroutines();
            error = Assert.Throws<AggregateException>(() => scheduler.Tick(0));
        }
        Assert.Equal(new[] { 0, 1, 2 }, cleaned);
        Assert.Equal(failures, error.InnerExceptions);
        scheduler.Dispose();
        Assert.Equal(3, cleaned.Count);

        void Fail(int index)
        {
            cleaned.Add(index);
            throw failures[index];
        }
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void SelfOwnerStopDefersDisposeUntilMoveNextReturnsAndPreservesOtherCoroutines(bool survivor)
    {
        using var scheduler = new CoroutineScheduler();
        var owner = new object();
        bool moving = false;
        int disposed = 0;
        int ticks = 0;
        CoroutineHandle self = scheduler.StartCoroutine(owner, new Probe(
            () =>
            {
                moving = true;
                scheduler.StopAllCoroutines(owner);
                Assert.Equal(0, disposed);
                moving = false;
                return true;
            },
            () => throw new Exception("A retired coroutine must not read Current."),
            () => { Assert.False(moving); disposed++; }));
        CoroutineHandle other = survivor ? scheduler.StartCoroutine(Loop(() => ticks++)) : default;
        scheduler.Tick(0);
        scheduler.Tick(0);
        Assert.False(self.isValid);
        Assert.Equal(1, disposed);
        Assert.Equal(survivor ? 2 : 0, ticks);
        Assert.Equal(survivor, other.isValid);
    }

    [Fact]
    public void StopEarlierOwnerDuringCallbackDoesNotSkipFollowingCoroutine()
    {
        using var scheduler = new CoroutineScheduler();
        var owner = new object();
        int following = 0;
        scheduler.StartCoroutine(owner, Loop(() => { }));
        scheduler.StartCoroutine(Loop(() => scheduler.StopAllCoroutines(owner)));
        scheduler.StartCoroutine(Loop(() => following++));
        scheduler.Tick(0);
        scheduler.Tick(0);
        Assert.Equal(2, following);
    }

    [Fact]
    public void ReentrantDisposeRetiresExecutingEnumeratorAfterItsCallback()
    {
        using var scheduler = new CoroutineScheduler();
        bool moving = false;
        int disposed = 0;
        scheduler.StartCoroutine(new Probe(
            () => { moving = true; scheduler.Dispose(); Assert.Equal(0, disposed); moving = false; return true; },
            () => null,
            () => { Assert.False(moving); disposed++; }));
        scheduler.Tick(0);
        Assert.Equal(1, disposed);
        Assert.Throws<ObjectDisposedException>(() => scheduler.Tick(0));
    }

    [Fact]
    public void ReentrantCleanupCommandsAreDrainedWithoutDoubleDisposal()
    {
        using var scheduler = new CoroutineScheduler();
        var owner = new object();
        int disposed = 0;
        scheduler.StartCoroutine(owner, new Probe(() => true, () => null, () =>
        {
            disposed++;
            scheduler.StartCoroutine(owner, new Probe(() => true, () => null, () => disposed++));
            scheduler.StopAllCoroutines(owner);
        }));
        scheduler.StopAllCoroutines(owner);
        Assert.Equal(2, disposed);
        scheduler.Tick(0);
        Assert.Equal(2, disposed);
    }

    [Fact]
    public void RecursiveTickIsRejectedBeforeAdvancingAnotherFrame()
    {
        using var scheduler = new CoroutineScheduler();
        int disposed = 0;
        scheduler.StartCoroutine(new Probe(() => { scheduler.Tick(0); return true; }, () => null, () => disposed++));
        AggregateException error = Assert.Throws<AggregateException>(() => scheduler.Tick(0));
        Assert.IsType<InvalidOperationException>(Assert.Single(error.InnerExceptions));
        Assert.Equal(1, disposed);
        scheduler.Tick(0);
    }

    [Fact]
    public void SelfYieldIsRejectedAndDisposedOnce()
    {
        using var scheduler = new CoroutineScheduler();
        int disposed = 0;
        Probe? routine = null;
        routine = new Probe(() => true, () => routine, () => disposed++);
        scheduler.StartCoroutine(routine);
        Assert.Throws<AggregateException>(() => scheduler.Tick(0));
        Assert.Equal(1, disposed);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void OwnerStopFromCurrentOrWaitPredicateCannotResumeRetiredState(bool wait)
    {
        using var scheduler = new CoroutineScheduler();
        var owner = new object();
        int disposed = 0;
        int moves = 0;
        CoroutineHandle handle = scheduler.StartCoroutine(owner, new Probe(
            () => { moves++; return true; },
            () =>
            {
                if (wait)
                    return new WaitUntil(() => { scheduler.StopAllCoroutines(owner); return true; });
                scheduler.StopAllCoroutines(owner);
                return null;
            },
            () => disposed++));
        scheduler.Tick(0);
        scheduler.Tick(0);
        Assert.False(handle.isValid);
        Assert.Equal(1, disposed);
        Assert.Equal(1, moves);
    }

    [Fact]
    public void ChildCleanupFailureRetiresParentWithoutResumingItsBody()
    {
        using var scheduler = new CoroutineScheduler();
        bool resumed = false;
        bool parentCleaned = false;
        var child = new Probe(() => false, () => null, () => throw new Exception("cleanup"));
        scheduler.StartCoroutine(Root());
        Assert.Throws<AggregateException>(() => scheduler.Tick(0));
        Assert.False(resumed);
        Assert.True(parentCleaned);

        IEnumerator Root()
        {
            try
            {
                yield return child;
                resumed = true;
            }
            finally
            {
                parentCleaned = true;
            }
        }
    }

    [Fact]
    public void CoroutineStartedByCallbackWaitsForNextTick()
    {
        using var scheduler = new CoroutineScheduler();
        int childTicks = 0;
        scheduler.StartCoroutine(Root());
        scheduler.Tick(0);
        Assert.Equal(0, childTicks);
        scheduler.Tick(0);
        Assert.Equal(1, childTicks);

        IEnumerator Root()
        {
            scheduler.StartCoroutine(Loop(() => childTicks++));
            yield break;
        }
    }

    [Theory]
    [InlineData("handle")]
    [InlineData("all")]
    public void QueuedStopFromCoroutineDoesNotCorruptActiveDispatch(string stop)
    {
        using var scheduler = new CoroutineScheduler();
        int disposed = 0;
        int following = 0;
        CoroutineHandle self = default;
        self = scheduler.StartCoroutine(new Probe(
            () =>
            {
                if (stop == "handle")
                    Assert.True(scheduler.StopCoroutine(self));
                else
                    scheduler.StopAllCoroutines();
                return true;
            },
            () => null,
            () => disposed++));
        scheduler.StartCoroutine(Loop(() => following++));
        scheduler.Tick(0);
        Assert.Equal(1, following);
        scheduler.Tick(0);
        Assert.Equal(stop == "handle" ? 2 : 1, following);
        Assert.False(self.isValid);
        Assert.Equal(1, disposed);
    }

    private static IEnumerator Loop(Action tick)
    {
        while (true)
        {
            tick();
            yield return null;
        }
    }

    private sealed class Probe(
        Func<bool> move,
        Func<object?> current,
        Action dispose
    ) : IEnumerator, IDisposable
    {
        public object? Current => current();
        public bool MoveNext() => move();
        public void Reset() => throw new NotSupportedException();
        public void Dispose() => dispose();
    }
}
