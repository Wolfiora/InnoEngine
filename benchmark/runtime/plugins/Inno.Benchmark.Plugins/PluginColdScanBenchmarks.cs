using BenchmarkDotNet.Attributes;
using Inno.Plugins.Authoring;

namespace Inno.Benchmark.Plugins;

/// <summary>
/// Measures plugin discovery with an empty Library, including archive validation and extraction.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
[InvocationCount(1)]
public class PluginColdScanBenchmarks
{
    private PluginScanFixture m_fixture = null!;

    [Params(8, 64)]
    public int pluginCount;

    [GlobalSetup]
    public void Setup() => m_fixture = new PluginScanFixture(pluginCount, 8);

    [IterationSetup]
    public void ClearLibrary() => m_fixture.ClearLibrary();

    [GlobalCleanup]
    public void Cleanup() => m_fixture.Dispose();

    [Benchmark(Description = "Scan, empty Library (cold extract)")]
    public int ScanCold() => m_fixture.CreateService().Scan().candidates.Count;
}
