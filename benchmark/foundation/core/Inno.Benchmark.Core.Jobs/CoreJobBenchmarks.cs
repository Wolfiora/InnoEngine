using System;
using BenchmarkDotNet.Attributes;
using Inno.Core.Jobs;

namespace Inno.Benchmark.Core.Jobs;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class CoreJobBenchmarks
{
    private const int C_JOBS = 256;

    private readonly JobHandle[] m_handles = new JobHandle[C_JOBS];
    private JobScheduler m_scheduler = null!;
    private float[] m_data = [];

    [Params(JobExecutionMode.SingleThread, JobExecutionMode.WorkerPool)]
    public JobExecutionMode mode;

    [GlobalSetup]
    public void Setup()
    {
        m_scheduler = mode == JobExecutionMode.SingleThread
                      ? new JobScheduler(JobExecutionMode.SingleThread)
                      : new JobScheduler(
                          JobExecutionMode.WorkerPool,
                          new JobSchedulerOptions { workerCount = Math.Max(1, Math.Min(8, Environment.ProcessorCount - 1)) }
                      );
        m_data = new float[1 << 16];
    }

    [GlobalCleanup]
    public void Cleanup() => m_scheduler.Dispose();

    [Benchmark(Description = "Schedule+CompleteAll 256 empty jobs", OperationsPerInvoke = C_JOBS)]
    public void ScheduleEmpty()
    {
        m_scheduler.BeginFrame();
        for (int index = 0; index < C_JOBS; index++)
        {
            m_handles[index] = m_scheduler.Schedule(static () => {});
        }
        m_scheduler.CompleteAll(m_handles);
        m_scheduler.EndFrame();
    }

    [Benchmark(Description = "Dependency chain 256 jobs", OperationsPerInvoke = C_JOBS)]
    public void DependencyChain()
    {
        m_scheduler.BeginFrame();
        JobHandle previous = default;
        for (int index = 0; index < C_JOBS; index++)
        {
            previous = previous.isValid
                       ? m_scheduler.Schedule(static _ => {}, null, [previous])
                       : m_scheduler.Schedule(static _ => {}, null, []);
        }
        m_scheduler.Complete(previous);
        m_scheduler.EndFrame();
    }

    [Arguments(1)]
    [Arguments(32)]
    [Benchmark(Description = "ParallelFor 65536 floats batch 1024")]
    public void ParallelFor(int workPerItem)
    {
        m_scheduler.BeginFrame();
        float[] data = m_data;
        JobHandle handle = m_scheduler.ParallelFor(data.Length, 1024, (
            start,
            end
        ) =>
        {
            for (int index = start; index < end; index++)
            {
                float value = data[index];
                for (int repeat = 0; repeat < workPerItem; repeat++)
                {
                    value = value * 0.5f + 1f;
                }
                data[index] = value;
            }
        });
        m_scheduler.Complete(handle);
        m_scheduler.EndFrame();
    }

    [Benchmark(Description = "Main-thread queue 256 actions", OperationsPerInvoke = C_JOBS)]
    public void MainThreadQueue()
    {
        for (int index = 0; index < C_JOBS; index++)
        {
            m_scheduler.EnqueueMainThread(static () => {});
        }
        m_scheduler.DrainMainThreadQueue();
    }
}
