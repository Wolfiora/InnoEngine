using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Adapter.Modules.DotNet;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Extensibility.Types;

namespace Inno.Benchmarks;

/// <summary>
/// Measures hot reload publication, previous-generation retirement and cold module-host restart.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class ReloadBenchmarks
{
    private ReloadFixture m_fixture = null!;
    private string m_coldRoot = string.Empty;
    private GenerationCoordinator m_coordinator = null!;

    [GlobalSetup]
    public void Setup()
    {
        m_fixture = new ReloadFixture();
        m_coldRoot = BenchmarkPaths.CreateRoot("ColdReload");
        m_coordinator = new GenerationCoordinator();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_fixture.Dispose();
        BenchmarkPaths.Delete(m_coldRoot);
    }

    [Benchmark(Description = "Hot reload V1<->V2 + wait retirement")]
    public int HotReloadAndRetire() => ReloadFixture.WaitForRetirement(m_fixture.HotReload());

    [Benchmark(Description = "Cold reload: new ModuleHost+TypeCatalog, load, dispose")]
    public int ColdReload()
    {
        var modules = new ModuleHost(
            new ModuleHostOptions { catalogSource = new DotNetAssemblyCatalogSource(typeof(ReloadFixture).Assembly) }
        );
        var types = new TypeCatalog(modules, new ReflectionTypeCatalogSource());
        _ = modules.Load(ReloadFixture.Request("V2", m_coldRoot));
        int count = modules.modules.Count;
        types.Dispose();
        modules.Dispose();
        return count;
    }

    [Benchmark(Description = "Generation read lease acquire+release")]
    public int GenerationReadLease()
    {
        using IDisposable lease = m_coordinator.AcquireRead("benchmark");
        return 1;
    }
}
