using System;
using System.Collections.Generic;
using BenchmarkDotNet.Attributes;
using Inno.Assets.Pipeline;
using Inno.Benchmark.AssetFixtures;

namespace Inno.Benchmark.Editor.Search;

/// <summary>
/// Reproduces the file-browser search: enumerate pipeline entries and filter display names case-insensitively.
/// </summary>
/// <remarks>
/// BenchmarkDotNet derives a generated runner from this class, so it is not sealed.
/// </remarks>
[MemoryDiagnoser]
public class EditorSearchBenchmarks
{
    private AssetWorkspace m_workspace = null!;
    private AssetPipeline m_pipeline = null!;

    [Params(1000, 5000)]
    public int assetCount;

    [Params("note0042", "folder")]
    public string query = string.Empty;

    [GlobalSetup]
    public void Setup()
    {
        m_workspace = new AssetWorkspace(assetCount);
        m_pipeline = m_workspace.CreatePipeline();
        m_pipeline.Rescan();
    }

    [GlobalCleanup]
    public void Cleanup()
    {
        m_pipeline.Dispose();
        m_workspace.Dispose();
    }

    [Benchmark(Description = "Search: entries + name filter")]
    public int Search()
    {
        var entries = new List<AssetFileEntry>(m_pipeline.GetFileSystemEntries());
        string filter = query.Trim();
        entries.RemoveAll(item => !item.name.Contains(filter, StringComparison.OrdinalIgnoreCase));
        return entries.Count;
    }
}
