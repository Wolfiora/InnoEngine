using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets;
using Inno.Assets.Pipeline;

namespace Inno.Benchmark.Engine;

/// <summary>
/// Measures pipeline open + full load with an empty Library (import) and with a populated Library (artifact cache).
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
[InvocationCount(1)]
public class AssetColdWarmBenchmarks
{
    private AssetWorkspace m_workspace = null!;

    [Params(100, 1000)]
    public int assetCount;

    [GlobalSetup]
    public void Setup()
    {
        m_workspace = new AssetWorkspace(assetCount);
        using AssetPipeline pipeline = m_workspace.CreatePipeline();
        m_workspace.LoadAll(pipeline);
    }

    [IterationSetup(Target = nameof(OpenAndLoadCold))]
    public void ClearLibrary() => m_workspace.ClearLibrary();

    [GlobalCleanup]
    public void Cleanup() => m_workspace.Dispose();

    [Benchmark(Description = "Open+load all, empty Library (cold import)")]
    public long OpenAndLoadCold()
    {
        using AssetPipeline pipeline = m_workspace.CreatePipeline();
        return m_workspace.LoadAll(pipeline);
    }

    [Benchmark(Description = "Open+load all, populated Library (warm artifacts)")]
    public long OpenAndLoadWarm()
    {
        using AssetPipeline pipeline = m_workspace.CreatePipeline();
        return m_workspace.LoadAll(pipeline);
    }
}
