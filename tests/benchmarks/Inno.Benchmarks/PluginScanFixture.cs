using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using BenchmarkDotNet.Attributes;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Plugins;
using Inno.Plugins.Authoring;

namespace Inno.Benchmarks;

/// <summary>
/// Writes real .iplugin archives and scans them through the public plugin source service.
/// </summary>
public sealed class PluginScanFixture : System.IDisposable
{
    public PluginScanFixture(
        int pluginCount,
        int assetsPerPlugin
    ) {
        this.services = new BenchmarkServices();
        this.root = BenchmarkPaths.CreateRoot("PluginScan");
        this.plugins = Path.Combine(root, "Plugins");
        this.library = Path.Combine(root, "Library");
        Directory.CreateDirectory(plugins);
        for (int plugin = 0; plugin < pluginCount; plugin++)
        {
            string id = $"bench.plugin{plugin:D3}";
            string[] dependencies = plugin == 0 ? [] : [$"bench.plugin{plugin - 1:D3}"];
            var manifest = new PluginManifest { pluginId = id, displayName = id, dependencies = dependencies };
            using FileStream stream = File.Create(Path.Combine(plugins, id + ".iplugin"));
            using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
            Write(archive, "Plugin.inno", services.serialization.Serialize(manifest));
            for (int asset = 0; asset < assetsPerPlugin; asset++)
            {
                Write(archive, $"Assets/text{asset:D3}.txt", Encoding.UTF8.GetBytes(new string('x', 256 + asset)));
                Write(archive, $"Assets/text{asset:D3}.txt.imeta", [1]);
            }
        }
    }

    public BenchmarkServices services { get; }
    public string root { get; }
    public string plugins { get; }
    public string library { get; }

    public PluginSourceService CreateService() => new(services.serialization, plugins, library);

    public void ClearLibrary() => BenchmarkPaths.Delete(library);

    public void Dispose()
    {
        services.Dispose();
        BenchmarkPaths.Delete(root);
    }

    private static void Write(
        ZipArchive archive,
        string path,
        byte[] bytes
    ) {
        using Stream output = archive.CreateEntry(path, CompressionLevel.Optimal).Open();
        output.Write(bytes);
    }
}
