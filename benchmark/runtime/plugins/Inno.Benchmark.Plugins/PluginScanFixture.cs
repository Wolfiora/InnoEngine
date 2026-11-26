using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using Inno.Benchmark.Runner;
using Inno.Benchmark.Services;
using Inno.Plugins;
using Inno.Plugins.Authoring;

namespace Inno.Benchmark.Plugins;

/// <summary>
/// Writes real .iplugin archives and scans them through the public plugin source service.
/// </summary>
public sealed class PluginScanFixture : System.IDisposable
{
    public PluginScanFixture(
        int pluginCount,
        int assetsPerPlugin,
        int? payloadBytes = null
    ) {
        this.services = new BenchmarkServices(typeof(PluginScanFixture).Assembly);
        this.root = BenchmarkPaths.CreateRoot("PluginScan");
        this.plugins = Path.Combine(root, "Plugins");
        this.library = Path.Combine(root, "Library");
        Directory.CreateDirectory(plugins);
        for (int plugin = 0; plugin < pluginCount; plugin++)
        {
            WritePackage(plugin, assetsPerPlugin, payloadBytes, 0);
        }
    }

    public BenchmarkServices services { get; }
    public string root { get; }
    public string plugins { get; }
    public string library { get; }

    public void WritePackage(
        int plugin,
        int assetsPerPlugin,
        int? payloadBytes,
        int version
    ) {
        var random = new System.Random(unchecked(plugin * 397 + version));
        string id = $"bench.plugin{plugin:D3}";
        string[] dependencies = plugin == 0 ? [] : [$"bench.plugin{plugin - 1:D3}"];
        var manifest = new PluginManifest { pluginId = id, displayName = id, dependencies = dependencies };
        using FileStream stream = File.Create(Path.Combine(plugins, id + ".iplugin"));
        using var archive = new ZipArchive(stream, ZipArchiveMode.Create);
        Write(archive, "Plugin.inno", services.serialization.Serialize(manifest));
        for (int asset = 0; asset < assetsPerPlugin; asset++)
        {
            byte[] payload;
            if (payloadBytes is int size)
            {
                payload = new byte[size];
                random.NextBytes(payload);
            }
            else
            {
                payload = Encoding.UTF8.GetBytes(new string('x', 256 + asset));
            }
            Write(archive, $"Assets/text{asset:D3}.txt", payload);
            Write(archive, $"Assets/text{asset:D3}.txt.imeta", [1]);
        }
    }

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
