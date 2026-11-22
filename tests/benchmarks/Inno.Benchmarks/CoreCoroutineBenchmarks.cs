using System.Collections;
using BenchmarkDotNet.Attributes;
using Inno.Core.Coroutines;

namespace Inno.Benchmarks;

[MemoryDiagnoser]
public class CoreCoroutineBenchmarks
{
    private CoroutineScheduler m_scheduler = null!;

    [Params(100, 10_000)]
    public int coroutineCount;

    [GlobalSetup(Target = nameof(TickRunning))]
    public void SetupRunning()
    {
        m_scheduler = new CoroutineScheduler();
        for (int index = 0; index < coroutineCount; index++)
        {
            _ = m_scheduler.StartCoroutine(Forever());
        }
        m_scheduler.Tick(0.016f);
    }

    [GlobalSetup(Target = nameof(StartTickComplete))]
    public void SetupEmpty() => m_scheduler = new CoroutineScheduler();

    [GlobalCleanup]
    public void Cleanup() => m_scheduler.Dispose();

    [Benchmark(Description = "Tick N running coroutines (yield null)")]
    public void TickRunning() => m_scheduler.Tick(0.016f);

    [Benchmark(Description = "Start N + tick to completion")]
    public void StartTickComplete()
    {
        for (int index = 0; index < coroutineCount; index++)
        {
            _ = m_scheduler.StartCoroutine(OneFrame());
        }
        m_scheduler.Tick(0.016f);
        m_scheduler.Tick(0.016f);
        m_scheduler.Tick(0.016f);
    }

    private static IEnumerator Forever()
    {
        while (true)
        {
            yield return null;
        }
    }

    private static IEnumerator OneFrame()
    {
        yield return null;
    }
}
