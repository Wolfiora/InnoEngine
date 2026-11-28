using System.Collections;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Inno.Core.Coroutines;

namespace Inno.Benchmark.Core.Coroutines;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class CoroutineWaitBenchmarks
{
    private CoroutineScheduler m_scheduler = null!;
    private object m_wait = null!;

    [Params("frames", "time", "task", "handle")]
    public string kind = string.Empty;

    [IterationSetup(Target = nameof(RegisterWait))]
    public void Setup()
    {
        m_scheduler = new CoroutineScheduler();
        m_wait = kind switch
        {
            "frames" => new WaitForFrames(int.MaxValue),
            "time" => new WaitForSeconds(1_000_000),
            "task" => new TaskCompletionSource().Task,
            "handle" => m_scheduler.StartCoroutine(Wait(new WaitForFrames(int.MaxValue))),
            _ => throw new System.InvalidOperationException(kind)
        };
        for (int i = 0; i < 100; i++)
        {
            m_scheduler.StartCoroutine(Wait(m_wait));
        }
    }

    [IterationSetup(Target = nameof(TickWaiting))]
    public void PrepareWaiting()
    {
        Setup();
        m_scheduler.Tick(0.016f);
    }

    [Benchmark(OperationsPerInvoke = 100)]
    public void RegisterWait() => m_scheduler.Tick(0.016f);

    [Benchmark(OperationsPerInvoke = 100)]
    public void TickWaiting() => m_scheduler.Tick(0.016f);

    [IterationSetup(Target = nameof(CompleteWait))]
    public void PrepareCompletion()
    {
        m_scheduler = new CoroutineScheduler();
        Task ready = Task.CompletedTask;
        object wait = kind switch
        {
            "frames" => new WaitForFrames(1),
            "time" => new WaitForSeconds(0.01f),
            "task" => ready,
            "handle" => m_scheduler.StartCoroutine(Wait(null!)),
            _ => throw new System.InvalidOperationException(kind)
        };
        for (int index = 0; index < 100; index++)
        {
            m_scheduler.StartCoroutine(Wait(wait));
        }
        m_scheduler.Tick(0.016f);
    }

    [Benchmark(OperationsPerInvoke = 100)]
    public void CompleteWait() => m_scheduler.Tick(0.016f);

    [IterationCleanup]
    public void Cleanup() => m_scheduler.Dispose();

    private static IEnumerator Wait(object instruction)
    {
        yield return instruction;
    }
}
