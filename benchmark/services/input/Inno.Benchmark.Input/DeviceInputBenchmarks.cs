using BenchmarkDotNet.Attributes;
using Inno.Adapter.Input;
using Inno.Core.Events;
using Inno.Core.Identity;
using Inno.Input;
using Inno.Input.Runtime;

namespace Inno.Benchmark.Input;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class DeviceInputBenchmarks
{
    private InputRuntime m_runtime = null!;
    private InputDeviceConnection m_left = null!;
    private InputDeviceConnection m_right = null!;
    private InputSnapshot m_snapshot = null!;

    [GlobalSetup]
    public void Setup()
    {
        m_runtime = new InputRuntime(new EventInputBackend(0), new IdentityAllocator(), new EventDispatcher());
        m_runtime.Attach();
        m_left = m_runtime.ConnectDevice("benchmark.dial");
        m_right = m_runtime.ConnectDevice("benchmark.dial");
        m_snapshot = CaptureTwoDevices();
    }

    [Benchmark]
    public InputSnapshot CaptureTwoDevices()
    {
        m_left.SetValue("position", 0.25);
        m_right.SetValue("position", 0.75);
        m_runtime.BeginFrame(default);
        InputSnapshot snapshot = m_runtime.snapshot;
        m_runtime.EndFrame(default);
        return snapshot;
    }

    [Benchmark]
    public double ReadTwoDevices() => m_snapshot.devices[0].controls["position"] + m_snapshot.devices[1].controls["position"];

    [GlobalCleanup]
    public void Cleanup() => m_runtime.Dispose();
}
