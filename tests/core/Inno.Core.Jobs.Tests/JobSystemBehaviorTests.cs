using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Core.Execution;
using Inno.Core.Jobs;

namespace Inno.Core.JobsSystem.Tests;

public sealed class JobSystemBehaviorTests
{
    [Theory]
    [InlineData(JobExecutionMode.SingleThread)]
    [InlineData(JobExecutionMode.WorkerPool)]
    public void FrameAdmissionRemainsAtomicUnderRepeatedInvalidAndOversizedRequests(JobExecutionMode mode)
    {
        using var jobs = new JobScheduler(mode, new JobSchedulerOptions { workerCount = 1, maxFrameJobs = 4 });
        int completed = 0;
        for (int frame = 0; frame < 256; frame++)
        {
            jobs.BeginFrame();
            Assert.Throws<ArgumentException>(() => jobs.Schedule(_ => {}, null, [default]));
            Assert.Equal(0, jobs.statistics.frameJobs);
            Assert.Throws<InvalidOperationException>(() => jobs.ParallelFor(8, 1, (
                _,
                _
            ) => Interlocked.Increment(ref completed)));
            Assert.Equal(0, jobs.statistics.frameJobs);
            for (int index = 0; index < 4; index++)
            {
                jobs.Schedule(() => Interlocked.Increment(ref completed));
            }
            jobs.EndFrame();
            Assert.Equal(0, jobs.statistics.frameJobs);
        }
        Assert.Equal(1024, completed);
        Assert.Equal(4, jobs.statistics.peakFrameJobs);
        Assert.Equal(256 * 9, jobs.statistics.rejectedJobs);
    }

    [Theory]
    [InlineData(JobExecutionMode.SingleThread)]
    [InlineData(JobExecutionMode.WorkerPool)]
    public void PendingMainThreadCallbackRemainsOwnedUntilRetry(JobExecutionMode mode)
    {
        using var jobs = new JobScheduler(mode, new JobSchedulerOptions { workerCount = 1, mainThreadDrainBudget = 1 });
        int attempts = 0;
        jobs.EnqueueMainThread(() =>
        {
            if (++attempts == 1) throw new RetirementPendingException("pending");
        });
        Assert.Throws<RetirementPendingException>(jobs.DrainMainThreadQueue);
        Assert.Equal(1, jobs.statistics.mainThreadPending);
        jobs.DrainMainThreadQueue();
        Assert.Equal(2, attempts);
        Assert.Equal(0, jobs.statistics.mainThreadPending);
    }

    [Theory]
    [InlineData(JobExecutionMode.SingleThread)]
    [InlineData(JobExecutionMode.WorkerPool)]
    public void CallbackDrainIsBoundedAndDefersRecursivelyEnqueuedWork(JobExecutionMode mode)
    {
        using var jobs = new JobScheduler(mode, new JobSchedulerOptions
        {
            workerCount = 1,
            mainThreadCapacity = 2,
            mainThreadDrainBudget = 1,
            maxFrameJobs = 2
        });
        int executions = 0;
        Action? repeat = null;
        repeat = () =>
        {
            executions++;
            jobs.EnqueueMainThread(repeat!);
        };
        jobs.EnqueueMainThread(repeat);
        jobs.EnqueueMainThread(() => executions++);
        Assert.Throws<InvalidOperationException>(() => jobs.EnqueueMainThread(() => {}));
        jobs.DrainMainThreadQueue();
        Assert.Equal(1, executions);
        jobs.DrainMainThreadQueue();
        Assert.Equal(2, executions);
        jobs.BeginFrame();
        jobs.Schedule(() => {});
        jobs.Schedule(() => {});
        Assert.Throws<InvalidOperationException>(() => jobs.Schedule(() => {}));
        jobs.EndFrame();
    }

    [Fact]
    public void SingleThreadExecutesDeterministicallyOnMainThread()
    {
        using var jobs = new JobScheduler(JobExecutionMode.SingleThread);
        jobs.BeginFrame();

        int mainThread = Environment.CurrentManagedThreadId;
        var observed = new List<int>();
        JobHandle a = jobs.Schedule(() => observed.Add(Environment.CurrentManagedThreadId));
        JobHandle b = jobs.Schedule(() => observed.Add(Environment.CurrentManagedThreadId));

        jobs.CompleteAll([a, b]);
        jobs.EndFrame();

        Assert.Equal(2, observed.Count);
        Assert.All(observed, id => Assert.Equal(mainThread, id));
        Assert.Equal(0, jobs.workerCount);
    }

    [Fact]
    public void SingleThreadRejectsCrossThreadUsage()
    {
        using var jobs = new JobScheduler(JobExecutionMode.SingleThread);
        jobs.BeginFrame();

        Exception? fault = null;
        var thread = new Thread(() =>
        {
            try
            {
                _ = jobs.Schedule(() => {});
            }
            catch (Exception ex)
            {
                fault = ex;
            }
        });
        thread.Start();
        thread.Join();

        jobs.EndFrame();
        Assert.IsType<InvalidOperationException>(fault);
    }

    [Fact]
    public void WorkStealingScheduleAndCompleteRunsJob()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        int value = 0;
        JobHandle handle = jobs.Schedule(() => value = 7);
        jobs.Complete(handle);
        jobs.EndFrame();

        Assert.Equal(7, value);
    }

    [Fact]
    public void WorkStealingScheduleWithDependenciesRunsInOrder()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        var order = new ConcurrentQueue<int>();
        JobHandle first = jobs.Schedule(() => order.Enqueue(1));
        JobHandle second = jobs.Schedule(_ => order.Enqueue(2), null, [first]);

        jobs.Complete(second);
        jobs.EndFrame();

        Assert.Equal(new[] { 1, 2 }, order.ToArray());
    }

    [Fact]
    public void WorkStealingCombineDependenciesCompletesAfterAll()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        int sum = 0;
        JobHandle a = jobs.Schedule(() => Interlocked.Add(ref sum, 3));
        JobHandle b = jobs.Schedule(() => Interlocked.Add(ref sum, 4));
        JobHandle barrier = jobs.CombineDependencies([a, b]);

        jobs.Complete(barrier);
        jobs.EndFrame();

        Assert.Equal(7, sum);
    }

    [Fact]
    public void WorkStealingParallelForCoversEveryIndexExactlyOnce()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        const int C_LENGTH = 1024;
        var values = new int[C_LENGTH];
        JobHandle handle = jobs.ParallelFor(C_LENGTH, 32, (
            start,
            end
        ) =>
        {
            for (int i = start; i < end; i++)
            {
                Interlocked.Increment(ref values[i]);
            }
        });

        jobs.Complete(handle);
        jobs.EndFrame();

        for (int i = 0; i < values.Length; i++)
        {
            Assert.Equal(1, values[i]);
        }
    }

    [Fact]
    public void WorkStealingMainThreadQueueDrainExecutesOnMainThread()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        int mainThread = Environment.CurrentManagedThreadId;
        var threadIDs = new List<int>();
        jobs.EnqueueMainThread(() => threadIDs.Add(Environment.CurrentManagedThreadId));
        jobs.EnqueueMainThread(() => threadIDs.Add(Environment.CurrentManagedThreadId));
        jobs.DrainMainThreadQueue();
        jobs.EndFrame();

        Assert.Equal(2, threadIDs.Count);
        Assert.All(threadIDs, id => Assert.Equal(mainThread, id));
    }

    [Fact]
    public void WorkStealingMainThreadQueueDrainFromWorkerThreadThrows()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        Exception? fault = null;
        var thread = new Thread(() =>
        {
            try
            {
                jobs.DrainMainThreadQueue();
            }
            catch (Exception ex)
            {
                fault = ex;
            }
        });
        thread.Start();
        thread.Join();

        jobs.EndFrame();
        Assert.IsType<InvalidOperationException>(fault);
    }

    [Fact]
    public void WorkStealingWhenJobThrowsCompleteRethrowsAndEndFrameAggregates()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        JobHandle handle = jobs.Schedule(() => throw new InvalidOperationException("boom"));
        InvalidOperationException completeEx = Assert.Throws<InvalidOperationException>(() => jobs.Complete(handle));
        Assert.IsType<InvalidOperationException>(completeEx.InnerException);

        AggregateException endEx = Assert.Throws<AggregateException>(() => jobs.EndFrame());
        Assert.NotEmpty(endEx.InnerExceptions);
    }

    [Fact]
    public void WorkStealingEndFrameRecyclesHandleAndStaleHandleThrows()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        JobHandle handle = jobs.Schedule(() => {});
        jobs.EndFrame();

        jobs.BeginFrame();
        InvalidOperationException ex = Assert.Throws<InvalidOperationException>(() => jobs.Complete(handle));
        jobs.EndFrame();

        Assert.Contains("stale", ex.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void WorkStealingConcurrentSchedulingAcrossThreadsIsStable()
    {
        using JobScheduler jobs = CreateWorkStealing(workerCount: 4);
        jobs.BeginFrame();

        const int C_TASK_COUNT = 2000;
        int count = 0;
        Parallel.For(0, C_TASK_COUNT, _ =>
        {
            JobHandle handle = jobs.Schedule(() => Interlocked.Increment(ref count));
            jobs.Complete(handle);
        });

        jobs.EndFrame();
        Assert.Equal(C_TASK_COUNT, count);
    }

    [Fact]
    public void WorkStealingWorkerThreadsParticipateUnderLoad()
    {
        using JobScheduler jobs = CreateWorkStealing(workerCount: 4);
        jobs.BeginFrame();

        var threadIDs = new ConcurrentDictionary<int, byte>();
        var handles = new JobHandle[256];
        for (int i = 0; i < handles.Length; i++)
        {
            handles[i] = jobs.Schedule(() =>
            {
                threadIDs.TryAdd(Environment.CurrentManagedThreadId, 0);
                Thread.Sleep(1);
            });
        }

        jobs.CompleteAll(handles);
        jobs.EndFrame();

        Assert.True(threadIDs.Count >= 2);
    }

    [Fact]
    public void WorkStealingNestedScheduleFromWorkerIsSupported()
    {
        using JobScheduler jobs = CreateWorkStealing(workerCount: 2);
        jobs.BeginFrame();

        int value = 0;
        JobHandle outer = jobs.Schedule(() =>
        {
            JobHandle inner = jobs.Schedule(() => Interlocked.Add(ref value, 3));
            jobs.Complete(inner);
            Interlocked.Add(ref value, 2);
        });

        jobs.Complete(outer);
        jobs.EndFrame();

        Assert.Equal(5, value);
    }

    [Fact]
    public void WorkStealingAutoWorkerCountIsInValidRange()
    {
        using var jobs = new JobScheduler(JobExecutionMode.WorkerPool, new JobSchedulerOptions { workerCount = 0 });
        Assert.InRange(jobs.workerCount, 1, 64);
    }

    [Fact]
    public void WorkStealingCombineManyDependenciesCompletesReliably()
    {
        using JobScheduler jobs = CreateWorkStealing(workerCount: 4);
        jobs.BeginFrame();

        const int C_COUNT = 300;
        var handles = new JobHandle[C_COUNT];
        int sum = 0;
        for (int i = 0; i < C_COUNT; i++)
        {
            handles[i] = jobs.Schedule(() => Interlocked.Increment(ref sum));
        }

        JobHandle barrier = jobs.CombineDependencies(handles);
        jobs.Complete(barrier);
        jobs.EndFrame();

        Assert.Equal(C_COUNT, sum);
    }

    [Fact]
    public void CommonApiEndFrameWithoutBeginFrameThrows()
    {
        using JobScheduler jobs = CreateWorkStealing();
        Assert.Throws<InvalidOperationException>(() => jobs.EndFrame());
    }

    [Fact]
    public void CommonApiBeginFrameTwiceThrows()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();
        Assert.Throws<InvalidOperationException>(() => jobs.BeginFrame());
        jobs.EndFrame();
    }

    [Fact]
    public void CommonApiParallelForInvalidArgumentsThrows()
    {
        using JobScheduler jobs = CreateWorkStealing();
        jobs.BeginFrame();

        Assert.Throws<ArgumentOutOfRangeException>(() => jobs.ParallelFor(-1, 8, (
            _,
            _
        ) => {}));
        Assert.Throws<ArgumentOutOfRangeException>(() => jobs.ParallelFor(1, 0, (
            _,
            _
        ) => {}));

        jobs.EndFrame();
    }

    [Fact]
    public void CommonApiDisposePreventsFurtherUse()
    {
        JobScheduler jobs = CreateWorkStealing();
        jobs.Dispose();

        Assert.Throws<ObjectDisposedException>(() => jobs.BeginFrame());
        Assert.Throws<ObjectDisposedException>(() => jobs.EnqueueMainThread(() => {}));
    }

    private static JobScheduler CreateWorkStealing(int workerCount = 2)
        => new JobScheduler(JobExecutionMode.WorkerPool, new JobSchedulerOptions
    {
        workerCount = workerCount
    });
}
