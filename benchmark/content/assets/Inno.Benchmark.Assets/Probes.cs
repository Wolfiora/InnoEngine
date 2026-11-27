using System.IO;
using System.Linq;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Benchmark.AssetFixtures;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.Assets;

internal static class Probes
{
    internal static void Assets(ProbeReport report)
    {
        long baseline = ProbeReport.Settle();
        using (var workspace = new AssetWorkspace(1000))
        {
            AssetPipeline? pipeline = null;
            double cold = ProbeReport.Time(() =>
            {
                pipeline = workspace.CreatePipeline();
                workspace.LoadAll(pipeline);
            });
            TextAsset a = pipeline!.Load<TextAsset>(workspace.paths[0]);
            TextAsset b = pipeline.Load<TextAsset>(workspace.paths[0]);
            report.Row("asset.1000.memory cache hit (same instance on reload)", ReferenceEquals(a, b) ? 1 : 0);
            long loaded = ProbeReport.Settle();
            a = null!;
            b = null!;
            int unloaded = pipeline.UnloadUnusedAssets();
            report.Row("asset.1000.UnloadUnusedAssets released", unloaded);
            report.Row("asset.1000.managed heap delta after unload KiB", (ProbeReport.Settle() - loaded) / 1024.0);
            double reload = ProbeReport.Time(() => workspace.LoadAll(pipeline));
            report.Row("asset.1000.reload after unload (artifact hit) ms", reload);
            double hot = ProbeReport.Median(() => workspace.LoadAll(pipeline));
            pipeline.Dispose();
            double warm = ProbeReport.Time(() =>
            {
                using AssetPipeline next = workspace.CreatePipeline();
                workspace.LoadAll(next);
            });
            int artifacts = Directory.Exists(workspace.libraryRoot)
                            ? Directory.EnumerateFiles(workspace.libraryRoot, "*", SearchOption.AllDirectories).Count()
                            : 0;
            string export = Path.Combine(workspace.root, "Export");
            double exportMs = 0;
            using (AssetPipeline exporter = workspace.CreatePipeline())
            {
                workspace.LoadAll(exporter);
                exportMs = ProbeReport.Time(() => exporter.ExportRuntimeArtifacts(export));
            }
            long exportBytes = Directory.EnumerateFiles(export, "*", SearchOption.AllDirectories).Sum(path => new FileInfo(path).Length);
            report.Row("asset.1000.first open+load, empty Library (cold import) ms", cold);
            report.Row("asset.1000.memory hit load-all median ms", hot);
            report.Row("asset.1000.new pipeline+load, populated Library (warm) ms", warm);
            report.Row("asset.1000.Library files", artifacts);
            report.Row("packaging.1000.ExportRuntimeArtifacts ms", exportMs);
            report.Row("packaging.1000.exported KiB", exportBytes / 1024.0);
        }
        report.Row("asset.retained after dispose KiB", (ProbeReport.Settle() - baseline) / 1024.0);
    }
}
