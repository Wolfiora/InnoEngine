using BenchmarkDotNet.Attributes;
using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Input;
using Inno.Input;

namespace Inno.Benchmarks;

[MemoryDiagnoser]
public class InputBenchmarks
{
    private EventInputBackend m_backend = null!;
    private KeyPressedEvent m_press = null!;
    private MouseMovedEvent m_move = null!;
    private long m_frame;

    [GlobalSetup]
    public void Setup()
    {
        m_backend = new EventInputBackend(1);
        m_press = new KeyPressedEvent(1, KeyCode.W);
        m_move = new MouseMovedEvent(1, 10, 20);
        m_backend.ProcessEvent(m_press);
    }

    [GlobalCleanup]
    public void Cleanup() => m_backend.Dispose();

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
        InputSnapshot snapshot = m_backend.Capture(++m_frame);
        int count = 0;
        for (int index = 0; index < 16; index++)
        {
            count += snapshot.IsKeyDown(KeyCode.W) ? 1 : 0;
        }
        return count;
    }
}
