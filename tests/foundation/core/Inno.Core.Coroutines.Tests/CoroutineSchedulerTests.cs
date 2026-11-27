using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.Coroutines;

namespace Inno.Core.Coroutines.Tests;

public sealed class CoroutineSchedulerTests
{
    [Fact]
    public void StartCoroutineNullYieldRunsOnNextTick()
    {
        using var scheduler = new CoroutineScheduler();
        bool ran = false;

        scheduler.StartCoroutine(Routine());
        scheduler.Tick(0.016f);
        Assert.False(ran);
        scheduler.Tick(0.016f);
        Assert.True(ran);
        return;

        IEnumerator Routine()
        {
            yield return null;
            ran = true;
        }
    }

    [Fact]
    public void WaitForSecondsWaitsByTime()
    {
        using var scheduler = new CoroutineScheduler();
        bool ran = false;

        scheduler.StartCoroutine(Routine());
        scheduler.Tick(0.3f);
        Assert.False(ran);
        scheduler.Tick(0.3f);
        Assert.False(ran);
        scheduler.Tick(0.4f);
        Assert.False(ran);
        scheduler.Tick(0.4f);
        Assert.True(ran);
        return;

        IEnumerator Routine()
        {
            yield return new WaitForSeconds(1f);
            ran = true;
        }
    }

    [Fact]
    public void WaitUntilAndWaitWhileWork()
    {
        using var scheduler = new CoroutineScheduler();
        bool flag = false;
        bool done = false;

        scheduler.StartCoroutine(Routine());
        scheduler.Tick(0.016f);
        Assert.False(done);

        flag = true;
        scheduler.Tick(0.016f);
        Assert.False(done);

        flag = false;
        scheduler.Tick(0.016f);
        Assert.True(done);
        return;

        IEnumerator Routine()
        {
            yield return new WaitUntil(() => flag);
            yield return new WaitWhile(() => flag);
            done = true;
        }
    }

    [Fact]
    public void StopCoroutineStopsTarget()
    {
        using var scheduler = new CoroutineScheduler();
        int count = 0;
        CoroutineHandle handle = scheduler.StartCoroutine(Routine());

        scheduler.Tick(0.016f);
        Assert.Equal(1, count);
        Assert.True(scheduler.StopCoroutine(handle));
        scheduler.Tick(0.016f);
        Assert.Equal(1, count);
        return;

        IEnumerator Routine()
        {
            while (true)
            {
                count++;
                yield return null;
            }
        }
    }

    [Fact]
    public void StopAllCoroutinesByOwnerStopsOnlyThatOwner()
    {
        using var scheduler = new CoroutineScheduler();
        var ownerA = new object();
        var ownerB = new object();
        int a = 0;
        int b = 0;

        CoroutineHandle handleA = scheduler.StartCoroutine(ownerA, Loop(() => a++));
        scheduler.StartCoroutine(ownerB, Loop(() => b++));
        scheduler.Tick(0.016f);
        Assert.Equal(1, a);
        Assert.Equal(1, b);

        scheduler.StopAllCoroutines(ownerA);
        Assert.False(handleA.isValid);
        scheduler.Tick(0.016f);
        Assert.Equal(1, a);
        Assert.Equal(2, b);
    }

    [Fact]
    public void StopAllCoroutinesByOwnerReleasesOwnerAndEnumeratorBeforeReturning()
    {
        using var scheduler = new CoroutineScheduler();

        (WeakReference owner, WeakReference routine) references = StopOwnedCoroutine(scheduler);
        ForceCollection();

        Assert.False(references.owner.IsAlive);
        Assert.False(references.routine.IsAlive);
    }

    [Fact]
    public void NestedCoroutineWorks()
    {
        using var scheduler = new CoroutineScheduler();
        string order = string.Empty;

        scheduler.StartCoroutine(Routine());
        scheduler.Tick(0.016f);
        scheduler.Tick(0.016f);
        scheduler.Tick(0.016f);

        Assert.Equal("ABC", order);
        return;

        IEnumerator Routine()
        {
            order += "A";
            yield return Child();
            order += "C";
        }

        IEnumerator Child()
        {
            yield return null;
            order += "B";
        }
    }

    [Fact]
    public void WaitForTaskWaitsCompletion()
    {
        using var scheduler = new CoroutineScheduler();
        var tcs = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
        bool completed = false;

        scheduler.StartCoroutine(Routine());
        scheduler.Tick(0.016f);
        Assert.False(completed);

        tcs.SetResult(true);
        scheduler.Tick(0.016f);
        Assert.True(completed);
        return;

        IEnumerator Routine()
        {
            yield return new WaitForTask(tcs.Task);
            completed = true;
        }
    }

    [Fact]
    public void ListenOnceStyleYieldCoroutineHandleWaitsTarget()
    {
        using var scheduler = new CoroutineScheduler();
        bool done = false;

        CoroutineHandle child = scheduler.StartCoroutine(Child());
        scheduler.StartCoroutine(Parent(child));
        scheduler.Tick(0.016f);
        Assert.False(done);
        scheduler.Tick(0.016f);
        Assert.True(done);
        return;

        IEnumerator Child()
        {
            yield return null;
        }

        IEnumerator Parent(CoroutineHandle handle)
        {
            yield return handle;
            done = true;
        }
    }

    [Fact]
    public async Task ConcurrentlyStartedCoroutinesResumeOnTheNextTick()
    {
        using var scheduler = new CoroutineScheduler();
        int count = 0;
        await RunTogetherAsync(_ => scheduler.StartCoroutine(Routine()));

        scheduler.Tick(0.016f);
        scheduler.Tick(0.016f);

        Assert.Equal(2, count);
        return;

        IEnumerator Routine()
        {
            yield return null;
            Interlocked.Increment(ref count);
        }
    }

    [Fact]
    public async Task ConcurrentlyStoppedHandlesDoNotResume()
    {
        using var scheduler = new CoroutineScheduler();
        int ran = 0;
        CoroutineHandle[] handles = [scheduler.StartCoroutine(Routine()), scheduler.StartCoroutine(Routine())];
        await RunTogetherAsync(index => scheduler.StopCoroutine(handles[index]));

        scheduler.Tick(0.016f);
        scheduler.Tick(0.016f);

        Assert.Equal(0, ran);
        return;

        IEnumerator Routine()
        {
            yield return null;
            Interlocked.Increment(ref ran);
        }
    }

    [Fact]
    public void HandleIsValidTracksLifecycle()
    {
        using var scheduler = new CoroutineScheduler();
        CoroutineHandle handle = scheduler.StartCoroutine(Routine());

        Assert.True(handle.isValid);
        scheduler.Tick(0.016f);
        Assert.True(handle.isValid);
        scheduler.Tick(0.016f);
        Assert.False(handle.isValid);
        return;

        IEnumerator Routine()
        {
            yield return null;
        }
    }

    [Fact]
    public void HandleIsValidBecomesFalseAfterStop()
    {
        using var scheduler = new CoroutineScheduler();
        CoroutineHandle handle = scheduler.StartCoroutine(Loop(() => {}));

        Assert.True(handle.isValid);
        Assert.True(scheduler.StopCoroutine(handle));
        scheduler.Tick(0.016f);
        Assert.False(handle.isValid);
    }

    [Fact]
    public void StopCoroutineFromOtherSchedulerReturnsFalseAndDoesNotAffectOwner()
    {
        using var schedulerA = new CoroutineScheduler();
        using var schedulerB = new CoroutineScheduler();
        int count = 0;

        CoroutineHandle handleA = schedulerA.StartCoroutine(Loop(() => Interlocked.Increment(ref count)));
        schedulerA.Tick(0.016f);
        Assert.Equal(1, count);

        Assert.False(schedulerB.StopCoroutine(handleA));
        schedulerA.Tick(0.016f);
        Assert.Equal(2, count);
    }

    [Fact]
    public void StartCoroutineAfterDisposeThrowsAndHandleCannotBeCreated()
    {
        using var scheduler = new CoroutineScheduler();
        scheduler.Dispose();

        Assert.Throws<ObjectDisposedException>(() => scheduler.StartCoroutine(Loop(() => {})));
    }

    [Fact]
    public void SharedExternalInstructionCreatesIndependentWaits()
    {
        using var scheduler = new CoroutineScheduler();
        var shared = new WaitTwoPolls();
        CoroutineHandle first = scheduler.StartCoroutine(Wait());
        scheduler.Tick(0.1f);
        CoroutineHandle second = scheduler.StartCoroutine(Wait());
        scheduler.Tick(0.1f);
        scheduler.Tick(0.1f);

        Assert.False(first.isValid);
        Assert.True(second.isValid);
        scheduler.Tick(0.1f);
        Assert.False(second.isValid);

        IEnumerator Wait()
        {
            yield return shared;
        }
    }

    private sealed class WaitTwoPolls : YieldInstruction
    {
        protected override Func<double, ulong, bool> CreateWaiter()
        {
            int polls = 0;
            return (
                _,
                _
            ) => ++polls < 2;
        }
    }

    private static async Task RunTogetherAsync(Action<int> action)
    {
        using var start = new Barrier(2);
        await Task.WhenAll(Task.Run(() => Run(0)), Task.Run(() => Run(1)));

        void Run(int index)
        {
            Assert.True(start.SignalAndWait(TimeSpan.FromSeconds(10)));
            action(index);
        }
    }

    private static IEnumerator Loop(Action onTick)
    {
        while (true)
        {
            onTick.Invoke();
            yield return null;
        }
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static (WeakReference owner, WeakReference routine) StopOwnedCoroutine(CoroutineScheduler scheduler)
    {
        var owner = new object();
        IEnumerator routine = Loop(static () => {});
        CoroutineHandle handle = scheduler.StartCoroutine(owner, routine);
        scheduler.Tick(0.016f);

        scheduler.StopAllCoroutines(owner);

        Assert.False(handle.isValid);
        return (new WeakReference(owner), new WeakReference(routine));
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static void ForceCollection()
    {
        for (int i = 0; i < 5; i++)
        {
            GC.Collect(GC.MaxGeneration, GCCollectionMode.Forced, blocking: true, compacting: true);
            GC.WaitForPendingFinalizers();
        }
    }
}
