using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Plugins.Authoring;

namespace Inno.Benchmark.Plugins;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class ChangedPluginBenchmarks
{
    private PluginScanFixture m_fixture = null!;
    private PluginSourceService m_service = null!;
    private int m_version;

    [Params(8, 64)]
    public int packages;

    [Params(256, 16_384)]
    public int bytesPerPackage;

    [GlobalSetup]
    public void Setup()
    {
        m_fixture = new PluginScanFixture(packages, 1, bytesPerPackage);
        m_service = m_fixture.CreateService();
        Scan();
    }

    [IterationSetup]
    public void ReplaceOne() => m_fixture.WritePackage(packages - 1, 1, bytesPerPackage, ++m_version);

    [Benchmark]
    public void Scan()
    {
        if (PluginSourceService.GetActivatableCandidates(m_service.Scan()).Count != packages)
        {
            throw new InvalidDataException("Expected all package candidates.");
        }
    }

    [GlobalCleanup]
    public void Cleanup() => m_fixture.Dispose();
}
