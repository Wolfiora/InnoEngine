using System;
using System.Collections.Generic;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets.Pipeline;
using Inno.Core.Graphs;
using Inno.Core.Input;
using Inno.Editor.Core;
using Inno.Editor.Graph;
using Inno.Editor.Inspection;
using Inno.Editor.Interactions;

namespace Inno.Benchmark.Engine;

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
