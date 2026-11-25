using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Text;
using Inno.Assets;
using Inno.Assets.Pipeline;
using Inno.Rendering.Assets.Authoring;
using Inno.Rendering.Shaders;

namespace Inno.Benchmark.Engine;

/// <summary>
/// Single-process probes for first-use latency, retained memory and cache hit/miss counts that BenchmarkDotNet does not report.
/// </summary>
internal static class ProbeRunner
{
    private static readonly StringBuilder REPORT = new();

    internal static int Run(string outputPath)
    {
        REPORT.AppendLine("| Probe | Value |").AppendLine("|---|---|");
        ContentPack();
        Assets();
        Plugins();
        Shaders();
        Reload();
        EditorGraph();
        File.WriteAllText(outputPath, REPORT.ToString());
        Console.WriteLine(REPORT.ToString());
        return 0;
    }

    private static void ContentPack()
    {
        long baseline = Settle();
        var bench = new ContentPackBenchmarks { entryCount = 1024, payloadBytes = 4096 };
        double first = Time(bench.Setup);
        double cold = Time(() => bench.OpenAndReadAll());
        double warm = Median(() => bench.ReadAll());
        bench.Cleanup();
        Row("content.1024x4KiB.setup+first open ms", first);
        Row("content.1024x4KiB.first open+read ms", cold);
        Row("content.1024x4KiB.warm read-all median ms", warm);
        Row("content.retained after dispose KiB", (Settle() - baseline) / 1024.0);
    }

    private static void Assets()
    {
        long baseline = Settle();
        using (var workspace = new AssetWorkspace(1000))
        {
            AssetPipeline? pipeline = null;
            double cold = Time(() =>
            {
                pipeline = workspace.CreatePipeline();
                workspace.LoadAll(pipeline);
            });
            TextAsset a = pipeline!.Load<TextAsset>(workspace.paths[0]);
            TextAsset b = pipeline.Load<TextAsset>(workspace.paths[0]);
            Row("asset.1000.memory cache hit (same instance on reload)", ReferenceEquals(a, b) ? 1 : 0);
            long loaded = Settle();
            a = null!;
            b = null!;
            int unloaded = pipeline.UnloadUnusedAssets();
            Row("asset.1000.UnloadUnusedAssets released", unloaded);
            Row("asset.1000.managed heap delta after unload KiB", (Settle() - loaded) / 1024.0);
            double reload = Time(() => workspace.LoadAll(pipeline));
            Row("asset.1000.reload after unload (artifact hit) ms", reload);
            double hot = Median(() => workspace.LoadAll(pipeline));
            pipeline.Dispose();
            double warm = Time(() =>
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
                exportMs = Time(() => exporter.ExportRuntimeArtifacts(export));
            }
            long exportBytes = Directory.EnumerateFiles(export, "*", SearchOption.AllDirectories).Sum(path => new FileInfo(path).Length);
            Row("asset.1000.first open+load, empty Library (cold import) ms", cold);
            Row("asset.1000.memory hit load-all median ms", hot);
            Row("asset.1000.new pipeline+load, populated Library (warm) ms", warm);
            Row("asset.1000.Library files", artifacts);
            Row("packaging.1000.ExportRuntimeArtifacts ms", exportMs);
            Row("packaging.1000.exported KiB", exportBytes / 1024.0);
        }
        Row("asset.retained after dispose KiB", (Settle() - baseline) / 1024.0);
    }

    private static void Plugins()
    {
        long baseline = Settle();
        using (var fixture = new PluginScanFixture(64, 8))
        {
            double cold = Time(() => fixture.CreateService().Scan());
            int files = Directory.EnumerateFiles(fixture.library, "*", SearchOption.AllDirectories).Count();
            DateTime stamp = Directory.EnumerateFiles(fixture.library, "*", SearchOption.AllDirectories).Max(File.GetLastWriteTimeUtc);
            double warm = Median(() => fixture.CreateService().Scan());
            DateTime after = Directory.EnumerateFiles(fixture.library, "*", SearchOption.AllDirectories).Max(File.GetLastWriteTimeUtc);
            Row("plugin.64.cold scan (empty Library) ms", cold);
            Row("plugin.64.warm scan median ms", warm);
            Row("plugin.64.Library files after scan", files);
            Row("plugin.64.warm scan rewrote cache (0=reuse)", after > stamp ? 1 : 0);
        }
        Row("plugin.retained after dispose KiB", (Settle() - baseline) / 1024.0);
    }

    private static void Shaders()
    {
        long baseline = Settle();
        var bench = new ShaderBenchmarks { chainLength = 8 };
        bench.Setup();
        CountingShaderToolchain counter = bench.toolchain;
        int start = counter.invocations;
        var compiler = new ShaderCompiler(counter);
        for (int index = 0; index < 100; index++)
        {
            _ = compiler.CompileAsync(bench.stage, bench.target).AsTask().GetAwaiter().GetResult();
        }
        Row("shader.100 compiles same stage: toolchain calls (misses)", counter.invocations - start);
        ShaderIrStage relowered = bench.Lower();
        int beforeRelower = counter.invocations;
        _ = compiler.CompileAsync(relowered, bench.target).AsTask().GetAwaiter().GetResult();
        Row("shader.re-lowered identical graph: misses", counter.invocations - beforeRelower);
        List<ShaderIrStage> stages = [];
        for (int length = 1; length <= 257; length++)
        {
            var variant = new ShaderBenchmarks { chainLength = length };
            variant.Setup();
            stages.Add(variant.stage);
            variant.Cleanup();
        }
        int beforeFill = counter.invocations;
        foreach (ShaderIrStage stage in stages)
        {
            _ = compiler.CompileAsync(stage, bench.target).AsTask().GetAwaiter().GetResult();
        }
        int fill = counter.invocations - beforeFill;
        int beforeRevisit = counter.invocations;
        foreach (ShaderIrStage stage in stages.Take(255))
        {
            _ = compiler.CompileAsync(stage, bench.target).AsTask().GetAwaiter().GetResult();
        }
        Row("shader.257 distinct stages: misses", fill);
        Row("shader.revisit first 255 after 257 (cap 256, clear-all): misses", counter.invocations - beforeRevisit);
        bench.Cleanup();
        Row("shader.retained after dispose KiB", (Settle() - baseline) / 1024.0);
    }

    private static void Reload()
    {
        long baseline = Settle();
        int contextsBefore = AssemblyLoadContext.All.Count();
        using (var fixture = new ReloadFixture())
        {
            double first = Time(() => ReloadFixture.WaitForRetirement(fixture.HotReload()));
            long afterFirst = Settle();
            List<int> collections = [];
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int index = 0; index < 50; index++)
            {
                collections.Add(ReloadFixture.WaitForRetirement(fixture.HotReload()));
            }
            stopwatch.Stop();
            Row("reload.first hot reload+retire ms", first);
            Row("reload.50 hot reloads avg ms", stopwatch.Elapsed.TotalMilliseconds / 50);
            Row("reload.GC passes to retire avg", collections.Average());
            Row("reload.heap growth over 50 reloads KiB", (Settle() - afterFirst) / 1024.0);
            Row("reload.live AssemblyLoadContexts delta while host alive", AssemblyLoadContext.All.Count() - contextsBefore);
        }
        Settle();
        Row("reload.live AssemblyLoadContexts delta after dispose", AssemblyLoadContext.All.Count() - contextsBefore);
        Row("reload.retained after dispose KiB", (Settle() - baseline) / 1024.0);
    }

    private static void EditorGraph()
    {
        long baseline = Settle();
        var bench = new EditorGraphBenchmarks { nodeCount = 512 };
        double setup = Time(bench.Setup);
        double first = Time(() => bench.EditDocument());
        long warm = Settle();
        for (int index = 0; index < 50; index++)
        {
            bench.EditDocument();
        }
        Row("editor.runtime start ms", setup);
        Row("editor.graph.512 first edit session ms", first);
        Row("editor.graph.heap growth over 50 open/edit/close KiB", (Settle() - warm) / 1024.0);
        bench.Cleanup();
        Row("editor.retained after dispose KiB", (Settle() - baseline) / 1024.0);
    }

    private static void Row(
        string name,
        double value
    ) {
        string text = $"| {name} | {value:0.###} |";
        REPORT.AppendLine(text);
        Console.Error.WriteLine(text);
    }

    private static double Time(Action action)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        action();
        return stopwatch.Elapsed.TotalMilliseconds;
    }

    private static double Median(Action action)
    {
        var samples = new double[9];
        for (int index = 0; index < samples.Length; index++)
        {
            samples[index] = Time(action);
        }
        Array.Sort(samples);
        return samples[samples.Length / 2];
    }

    private static long Settle()
    {
        for (int index = 0; index < 3; index++)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        return GC.GetTotalMemory(forceFullCollection: true);
    }
}
