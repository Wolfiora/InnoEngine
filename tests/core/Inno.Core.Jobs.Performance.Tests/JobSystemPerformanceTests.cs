using System;
using System.Diagnostics;
using System.Threading;
using Xunit;
using Inno.Core.Jobs;

namespace Inno.Core.JobsSystem.Tests;

public sealed class JobSystemPerformanceTests
{
    [Fact]
    public void SameScheduleFlow_MoreWorkersIsFasterThanSingleWorker()
    {
        Assert.True(Environment.ProcessorCount >= 2, "Worker scaling measurements require at least two logical processors.");

        const int C_JOB_COUNT = 64;
        const int C_SLEEP_MILLISECONDS = 4;
        int multiWorkerCount = Math.Min(4, Environment.ProcessorCount);

        using JobScheduler oneWorker = CreateWorkStealing(1);
        using JobScheduler multiWorker = CreateWorkStealing(multiWorkerCount);

        _ = MeasureScheduleAndCompleteAll(oneWorker, 8, 1);
        _ = MeasureScheduleAndCompleteAll(multiWorker, 8, 1);

        TimeSpan oneWorkerBest = MeasureBestOf(3, () => MeasureScheduleAndCompleteAll(oneWorker, C_JOB_COUNT, C_SLEEP_MILLISECONDS));
        TimeSpan multiWorkerBest = MeasureBestOf(3, () => MeasureScheduleAndCompleteAll(multiWorker, C_JOB_COUNT, C_SLEEP_MILLISECONDS));

        Assert.True(
            multiWorkerBest < oneWorkerBest,
            $"Expected same schedule flow to be faster with more workers. oneWorker={oneWorkerBest.TotalMilliseconds:F2}ms, multiWorker={multiWorkerBest.TotalMilliseconds:F2}ms, workers={multiWorkerCount}"
        );
    }

    [Fact]
    public void WorkStealingScheduleCompleteAll_IsFasterThanSingleThread()
    {
        Assert.True(Environment.ProcessorCount >= 2, "Worker scaling measurements require at least two logical processors.");

        const int C_JOB_COUNT = 48;
        const int C_SLEEP_MILLISECONDS = 4;
        int workerCount = Math.Min(4, Environment.ProcessorCount);

        using var single = new JobScheduler(JobExecutionMode.SingleThread);
        using JobScheduler multi = CreateWorkStealing(workerCount);

        _ = MeasureScheduleAndCompleteAll(single, 8, 1);
        _ = MeasureScheduleAndCompleteAll(multi, 8, 1);

        TimeSpan singleBest = MeasureBestOf(3, () => MeasureScheduleAndCompleteAll(single, C_JOB_COUNT, C_SLEEP_MILLISECONDS));
        TimeSpan multiBest = MeasureBestOf(3, () => MeasureScheduleAndCompleteAll(multi, C_JOB_COUNT, C_SLEEP_MILLISECONDS));

        Assert.True(
            multiBest < singleBest,
            $"Expected work-stealing to be faster. single={singleBest.TotalMilliseconds:F2}ms, multi={multiBest.TotalMilliseconds:F2}ms, workers={workerCount}"
        );
    }

    [Fact]
    public void WorkStealingParallelFor_IsFasterThanSingleThread()
    {
        Assert.True(Environment.ProcessorCount >= 2, "Worker scaling measurements require at least two logical processors.");

        const int C_LENGTH = 48;
        const int C_BATCH_SIZE = 1;
        const int C_SLEEP_MILLISECONDS = 4;
        int workerCount = Math.Min(4, Environment.ProcessorCount);

        using var single = new JobScheduler(JobExecutionMode.SingleThread);
        using JobScheduler multi = CreateWorkStealing(workerCount);

        _ = MeasureParallelFor(single, 8, 1, 1);
        _ = MeasureParallelFor(multi, 8, 1, 1);

        TimeSpan singleBest = MeasureBestOf(3, () => MeasureParallelFor(single, C_LENGTH, C_BATCH_SIZE, C_SLEEP_MILLISECONDS));
        TimeSpan multiBest = MeasureBestOf(3, () => MeasureParallelFor(multi, C_LENGTH, C_BATCH_SIZE, C_SLEEP_MILLISECONDS));

        Assert.True(
            multiBest < singleBest,
            $"Expected work-stealing parallel-for to be faster. single={singleBest.TotalMilliseconds:F2}ms, multi={multiBest.TotalMilliseconds:F2}ms, workers={workerCount}"
        );
    }

    private static JobScheduler CreateWorkStealing(int workerCount)
        => new JobScheduler(JobExecutionMode.WorkerPool, new JobSchedulerOptions
        {
            workerCount = workerCount
        });

    private static TimeSpan MeasureBestOf(
        int runs,
        Func<TimeSpan> measure
    ) {
        TimeSpan best = TimeSpan.MaxValue;
        for (int i = 0; i < runs; i++)
        {
            TimeSpan current = measure();
            if (current < best)
            {
                best = current;
            }
        }

        return best;
    }

    private static TimeSpan MeasureScheduleAndCompleteAll(
        JobScheduler jobs,
        int jobCount,
        int sleepMilliseconds
    ) {
        jobs.BeginFrame();

        var handles = new JobHandle[jobCount];
        Stopwatch stopwatch = Stopwatch.StartNew();
        for (int i = 0; i < jobCount; i++)
        {
            handles[i] = jobs.Schedule(() => Thread.Sleep(sleepMilliseconds));
        }

        jobs.CompleteAll(handles);
        stopwatch.Stop();

        jobs.EndFrame();
        return stopwatch.Elapsed;
    }

    private static TimeSpan MeasureParallelFor(
        JobScheduler jobs,
        int length,
        int batchSize,
        int sleepMilliseconds
    ) {
        jobs.BeginFrame();

        Stopwatch stopwatch = Stopwatch.StartNew();
        JobHandle handle = jobs.ParallelFor(length, batchSize, (
            _,
            _
        ) => Thread.Sleep(sleepMilliseconds));
        jobs.Complete(handle);
        stopwatch.Stop();

        jobs.EndFrame();
        return stopwatch.Elapsed;
    }
}
