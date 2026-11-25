using System;
using System.IO;
using BenchmarkDotNet.Attributes;
using Inno.Assets;
using Inno.Assets.Pipeline;

namespace Inno.Benchmark.Engine;

/// <summary>
/// Owns a project of text assets and constructs real authoring asset pipelines over it.
/// </summary>
public sealed class AssetWorkspace : IDisposable
{
    public AssetWorkspace(int assetCount)
    {
        this.services = new BenchmarkServices();
        this.root = BenchmarkPaths.CreateRoot("Assets");
        this.assetRoot = Path.Combine(root, "Assets");
        this.libraryRoot = Path.Combine(root, "Library");
        this.paths = new AssetPath[assetCount];
        for (int index = 0; index < assetCount; index++)
        {
            string folder = $"Folder{index % 16:D2}";
            Directory.CreateDirectory(Path.Combine(assetRoot, folder));
            string local = $"{folder}/note{index:D5}.txt";
            File.WriteAllText(Path.Combine(assetRoot, local), $"asset {index} " + new string('a', 512));
            paths[index] = AssetPath.Project(local);
        }
    }

    public BenchmarkServices services { get; }
    public string root { get; }
    public string assetRoot { get; }
    public string libraryRoot { get; }
    public AssetPath[] paths { get; }

    public AssetPipeline CreatePipeline()
        => new(
            services.modules,
            services.types,
            services.serialization,
            services.identities,
            services.diagnostics,
            services.logs,
            AssetPipelineOptions.Create(assetRoot, libraryRoot) with { enableFileSystemWatcher = false }
        );

    public void ClearLibrary() => BenchmarkPaths.Delete(libraryRoot);

    public long LoadAll(AssetPipeline pipeline)
    {
        long total = 0;
        foreach (AssetPath path in paths)
        {
            total += pipeline.Load<TextAsset>(path).content.Length;
        }
        return total;
    }

    public void Dispose()
    {
        services.Dispose();
        BenchmarkPaths.Delete(root);
    }
}
