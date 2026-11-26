using BenchmarkDotNet.Attributes;
using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Input;

namespace Inno.Benchmark.Input;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class InputBenchmarks : IInputService
{
    private EventInputBackend m_backend = null!;
    private KeyPressedEvent m_press = null!;
    private MouseMovedEvent m_move = null!;
    private System.IDisposable? m_scope;
    private long m_frame;
    public InputSnapshot snapshot { get; private set; } = InputSnapshot.empty;

    [GlobalSetup(Targets = [nameof(CaptureIdle), nameof(ProcessAndCapture), nameof(QuerySnapshot), nameof(SameFrameEdges)])]
    public void Setup()
    {
        m_backend = new EventInputBackend(1);
        m_press = new KeyPressedEvent(1, KeyCode.W);
        m_move = new MouseMovedEvent(1, 10, 20);
        m_backend.ProcessEvent(m_press);
        snapshot = m_backend.Capture(++m_frame);
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_scope?.Dispose();
        m_backend.Dispose();
    }

    [GlobalSetup(Target = nameof(ScopedQuery))]
    public void SetupScope()
    {
        Setup();
        m_scope = InputExecutionContext.EnterScope(this);
    }

    [Benchmark(Description = "Capture idle frame (1 key held)")]
    public InputSnapshot CaptureIdle() => m_backend.Capture(++m_frame);

    [Benchmark(Description = "Process 8 events + Capture")]
    public InputSnapshot ProcessAndCapture()
    {
        for (int index = 0; index < 4; index++)
        {
            m_backend.ProcessEvent(m_move);
            m_backend.ProcessEvent(m_press);
        }
        return m_backend.Capture(++m_frame);
    }

    [Benchmark(Description = "InputSnapshot.IsKeyDown x16")]
    public int QuerySnapshot()
    {
        int count = 0;
        for (int index = 0; index < 16; index++)
        {
            count += snapshot.IsKeyDown(KeyCode.W) ? 1 : 0;
        }
        return count;
    }

    [Benchmark(Description = "Press and release within one captured frame")]
    public bool SameFrameEdges()
    {
        m_backend.ProcessEvent(new KeyPressedEvent(1, KeyCode.A));
        m_backend.ProcessEvent(new KeyReleasedEvent(1, KeyCode.A));
        InputSnapshot result = m_backend.Capture(++m_frame);
        return result.WasKeyPressed(KeyCode.A) && result.WasKeyReleased(KeyCode.A);
    }

    [Benchmark(Description = "16 public queries through a bound input scope")]
    public int ScopedQuery()
    {
        int count = 0;
        for (int index = 0; index < 16; index++)
        {
            count += Inno.Input.Input.IsKeyDown(KeyCode.W) ? 1 : 0;
        }
        return count;
    }
}
