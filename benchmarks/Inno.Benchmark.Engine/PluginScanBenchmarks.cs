using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using BenchmarkDotNet.Attributes;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Plugins;
using Inno.Plugins.Authoring;

namespace Inno.Benchmark.Engine;

/// <summary>
/// Measures warm plugin discovery and catalog projection over an existing extraction cache.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class PluginScanBenchmarks
{
    private PluginScanFixture m_fixture = null!;
    private PluginSourceService m_service = null!;
    private PluginScanResult m_result = null!;

    [Params(8, 64)]
    public int pluginCount;

    [GlobalSetup]
    public void Setup()
    {
        m_fixture = new PluginScanFixture(pluginCount, 8);
        m_service = m_fixture.CreateService();
        m_result = m_service.Scan();
        if (m_result.candidates.Count != pluginCount)
        {
            throw new InvalidDataException($"Expected {pluginCount} candidates, scanned {m_result.candidates.Count}.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => m_fixture.Dispose();

    [Benchmark(Description = "Scan, same service (warm cache)")]
    public int ScanWarm() => m_service.Scan().candidates.Count;

    [Benchmark(Description = "Scan, new service over existing Library (editor restart)")]
    public int ScanNewServiceWarmLibrary() => m_fixture.CreateService().Scan().candidates.Count;

    [Benchmark(Description = "Catalog: activatable candidates + mounts")]
    public int Catalog()
    {
        IReadOnlyList<PluginCandidate> candidates = PluginSourceService.GetActivatableCandidates(m_result);
        IReadOnlyList<AssetSourceMount> mounts = PluginSourceService.GetActivatableMounts(m_result);
        return candidates.Count + mounts.Count;
    }
}
