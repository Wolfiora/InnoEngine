using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Benchmark.AssetFixtures;

namespace Inno.Benchmark.Assets;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class IncrementalAssetBenchmarks
{
    private readonly AssetPath m_added = AssetPath.Project("added.txt");
    private AssetWorkspace m_workspace = null!;
    private AssetPipeline m_pipeline = null!;

    [Params(100, 1000)]
    public int existingAssets;

    [IterationSetup]
    public void Setup()
    {
        m_workspace = new AssetWorkspace(existingAssets);
        m_pipeline = m_workspace.CreatePipeline();
        m_workspace.LoadAll(m_pipeline);
        File.WriteAllText(Path.Combine(m_workspace.assetRoot, "added.txt"), "new text asset");
    }

    [Benchmark]
    public void ImportOne()
    {
        if (!m_pipeline.Import(m_added)) throw new InvalidDataException("The added asset was not imported.");
    }

    [IterationCleanup]
    public void Cleanup()
    {
        m_pipeline.Dispose();
        m_workspace.Dispose();
    }
}
