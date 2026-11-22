using System;
using BenchmarkDotNet.Attributes;
using Inno.Core.Events;
using Inno.Core.Input;

namespace Inno.Benchmarks;

[MemoryDiagnoser]
public class CoreEventBenchmarks
{
    private EventDispatcher m_dispatcher = null!;
    private EventHub m_hub = null!;
    private KeyPressedEvent m_event = null!;
    private int m_received;

    [Params(1, 16, 256)]
    public int listenerCount;

    [GlobalSetup]
    public void Setup()
    {
        m_dispatcher = new EventDispatcher();
        m_hub = m_dispatcher.CreateHub();
        for (int index = 0; index < listenerCount; index++)
        {
            _ = m_hub.Listen<KeyPressedEvent>(_ => m_received++);
        }
        m_event = new KeyPressedEvent(1, KeyCode.A);
    }

    [Benchmark(Description = "Emit cached KeyPressedEvent")]
    public void EmitCached() => m_dispatcher.Emit(m_event);

    [Benchmark(Description = "Emit fresh KeyPressedEvent")]
    public void EmitFresh() => m_dispatcher.Emit(new KeyPressedEvent(1, KeyCode.A));

    [Benchmark(Description = "Enqueue+Flush 64 events", OperationsPerInvoke = 64)]
    public void EnqueueFlush()
    {
        for (int index = 0; index < 64; index++)
        {
            m_dispatcher.Enqueue(new KeyPressedEvent(1, KeyCode.A));
        }
        m_dispatcher.Flush();
    }

    [Benchmark(Description = "Listen+Dispose churn")]
    public void SubscribeChurn()
    {
        IDisposable subscription = m_hub.Listen<KeyPressedEvent>(static _ => {});
        subscription.Dispose();
    }
}
