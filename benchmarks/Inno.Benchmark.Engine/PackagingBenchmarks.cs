using System.IO;
using System.Linq;
using BenchmarkDotNet.Attributes;
using Inno.Assets.Pipeline;

namespace Inno.Benchmark.Engine;

/// <summary>
/// Measures the public, backend-neutral deployment step: exporting imported runtime artifacts to a content root.
/// </summary>
/// <remarks>
/// The game build pipeline, content pack writer and plugin package builder are internal build components and the Player
/// targets only Windows x64, macOS ARM64 and Browser; they are measured by the platform build runs instead.
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class PackagingBenchmarks
{
    private AssetWorkspace m_workspace = null!;
    private AssetPipeline m_pipeline = null!;
    private string m_output = string.Empty;

    [Params(100, 1000)]
    public int assetCount;

    [GlobalSetup]
    public void Setup()
    {
        m_workspace = new AssetWorkspace(assetCount);
        m_pipeline = m_workspace.CreatePipeline();
        m_workspace.LoadAll(m_pipeline);
        m_output = Path.Combine(m_workspace.root, "Deploy");
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_pipeline.Dispose();
        m_workspace.Dispose();
    }

    [IterationSetup]
    public void ClearOutput() => BenchmarkPaths.Delete(m_output);

    [Benchmark(Description = "Deploy: ExportRuntimeArtifacts to empty content root")]
    public int Export() => m_pipeline.ExportRuntimeArtifacts(m_output).GetHashCode();

    [Benchmark(Description = "Deploy: export + enumerate deployed files")]
    public long ExportAndMeasure()
    {
        _ = m_pipeline.ExportRuntimeArtifacts(m_output);
        return Directory.EnumerateFiles(m_output, "*", SearchOption.AllDirectories).Sum(static path => new FileInfo(path).Length);
    }
}
