using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets;
using Inno.Assets.Pipeline;

namespace Inno.Benchmarks;

/// <summary>
/// Measures asset lookup and in-memory cache behavior on an open authoring pipeline.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class AssetBenchmarks
{
    private AssetWorkspace m_workspace = null!;
    private AssetPipeline m_pipeline = null!;
    private Guid[] m_ids = [];

    [Params(100, 1000)]
    public int assetCount;

    [GlobalSetup]
    public void Setup()
    {
        m_workspace = new AssetWorkspace(assetCount);
        m_pipeline = m_workspace.CreatePipeline();
        m_workspace.LoadAll(m_pipeline);
        m_ids = new Guid[assetCount];
        for (int index = 0; index < assetCount; index++)
        {
            if (!m_pipeline.TryGetInfo(m_workspace.paths[index], out AssetInfo? info) || info is null)
            {
                throw new InvalidDataException($"Missing asset {m_workspace.paths[index]}.");
            }
            m_ids[index] = info.persistentID;
        }
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_pipeline.Dispose();
        m_workspace.Dispose();
    }

    [Benchmark(Description = "Lookup TryGetInfo(path) x N")]
    public int LookupByPath()
    {
        int found = 0;
        foreach (AssetPath path in m_workspace.paths)
        {
            found += m_pipeline.TryGetInfo(path, out _) ? 1 : 0;
        }
        return found;
    }

    [Benchmark(Description = "Load<T>(path) x N, memory cache hit")]
    public long LoadByPathHit() => m_workspace.LoadAll(m_pipeline);

    [Benchmark(Description = "Load<T>(guid) x N, memory cache hit")]
    public long LoadByIDHit()
    {
        long total = 0;
        foreach (Guid id in m_ids)
        {
            total += m_pipeline.Load<TextAsset>(id).content.Length;
        }
        return total;
    }

    [Benchmark(Description = "Rescan unchanged project")]
    public int Rescan()
    {
        m_pipeline.Rescan();
        return m_pipeline.GetFileSystemEntries(includeDirectories: false).Count;
    }
}
