using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json;
using Inno.Editor.Interactions;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Editor.PlayMode.Tests;

public sealed class LargePanelPerformanceTests(ITestOutputHelper output)
{
    [Fact]
    [Trait("Acceptance", "Performance")]
    public void TenThousandHierarchyRowsProduceRepeatableFrameMeasurements()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        string? input = InputDirectory("hierarchy");
        if (input is not null && Directory.Exists(input))
        {
            CopyDirectory(input, Path.Combine(host.projectRoot, "Assets"));
            host.assets.Rescan();
            _ = host.workspace.Open("Untitled Scene.iscene");
        }
        else
        {
            var scene = host.CreateScene();
            for (int index = 0; index < 10_000; index++)
                _ = scene.CreateObject("Object " + index.ToString("D5"));
            if (input is not null)
            {
                Assert.Equal("Untitled Scene.iscene", host.workspace.Save(scene, string.Empty));
                CopyDirectory(Path.Combine(host.projectRoot, "Assets"), input);
            }
        }
        Assert.Equal(10_000, host.workspace.activeScene!.GetObjects().Count);
        Measure(host, host.native.GetPanel("scene.hierarchy"), "10000-hierarchy-rows");
    }

    [Fact]
    [Trait("Acceptance", "Performance")]
    public void TenThousandAssetRowsProduceRepeatableFrameMeasurements()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        string? input = InputDirectory("assets");
        if (input is not null && Directory.Exists(input))
            CopyDirectory(input, Path.Combine(host.projectRoot, "Assets"));
        else
            for (int index = 0; index < 10_000; index++)
                Directory.CreateDirectory(Path.Combine(host.projectRoot, "Assets", "Folder " + index.ToString("D5")));
        host.assets.Rescan();
        Assert.Equal(10_000, Directory.EnumerateDirectories(Path.Combine(host.projectRoot, "Assets"), "Folder *").Count());
        if (input is not null && !Directory.Exists(input))
            CopyDirectory(Path.Combine(host.projectRoot, "Assets"), input);
        EditorPanelExtension panel = host.native.GetPanel("asset.file-browser");
        Measure(host, panel, "10000-asset-directory-rows");
    }

    private static string? InputDirectory(string workload)
    {
        string? owner = Environment.GetEnvironmentVariable("INNO_UI_PERFORMANCE_INPUT_ROOT");
        return owner is null ? null : Path.Combine(Path.GetFullPath(owner), workload);
    }

    private static void CopyDirectory(
        string source,
        string destination
    ) {
        Directory.CreateDirectory(destination);
        foreach (string directory in Directory.EnumerateDirectories(source, "*", SearchOption.AllDirectories))
            Directory.CreateDirectory(Path.Combine(destination, Path.GetRelativePath(source, directory)));
        foreach (string file in Directory.EnumerateFiles(source, "*", SearchOption.AllDirectories))
            File.Copy(file, Path.Combine(destination, Path.GetRelativePath(source, file)), overwrite: false);
    }

    private void Measure(
        NativeProjectPanelHost host,
        EditorPanelExtension panel,
        string workload
    ) {
        for (int warmup = 0; warmup < 5; warmup++)
            host.native.DrawPanel(panel);
        string? requested = Environment.GetEnvironmentVariable("INNO_UI_PERFORMANCE_REPETITIONS");
        int repetitions = requested is null ? 5 : int.Parse(requested, System.Globalization.CultureInfo.InvariantCulture);
        Assert.InRange(repetitions, 1, 5);
        using Process process = Process.GetCurrentProcess();
        var samples = new List<object>();
        for (int repetition = 0; repetition < repetitions; repetition++)
        {
            var milliseconds = new double[10];
            long bytesBefore = GC.GetAllocatedBytesForCurrentThread();
            long totalBytesBefore = GC.GetTotalAllocatedBytes(precise: true);
            TimeSpan cpuBefore = process.TotalProcessorTime;
            int collectionsBefore = GC.CollectionCount(0);
            int gen1Before = GC.CollectionCount(1);
            int gen2Before = GC.CollectionCount(2);
            for (int frame = 0; frame < milliseconds.Length; frame++)
            {
                long started = Stopwatch.GetTimestamp();
                host.native.DrawPanel(panel);
                milliseconds[frame] = Stopwatch.GetElapsedTime(started).TotalMilliseconds;
            }
            long allocated = GC.GetAllocatedBytesForCurrentThread() - bytesBefore;
            double[] sorted = milliseconds.Order().ToArray();
            samples.Add(new
            {
                repetition, milliseconds, median = (sorted[4] + sorted[5]) / 2,
                p95 = sorted[9], allocatedBytes = allocated,
                totalAllocatedBytes = GC.GetTotalAllocatedBytes(precise: true) - totalBytesBefore,
                cpuMilliseconds = (process.TotalProcessorTime - cpuBefore).TotalMilliseconds,
                gen0Collections = GC.CollectionCount(0) - collectionsBefore,
                gen1Collections = GC.CollectionCount(1) - gen1Before,
                gen2Collections = GC.CollectionCount(2) - gen2Before
            });
        }
        output.WriteLine(JsonSerializer.Serialize(new
        {
            workload, configuration = "Debug", samples,
            framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            viewport = "1000x800", warmupFrames = 5, measuredFramesPerRepetition = 10,
            sharedInput = InputDirectory(workload.Contains("hierarchy", StringComparison.Ordinal) ? "hierarchy" : "assets"),
            consumer = "native-command-offset-validation", supportsVertexOffsets = true
        }));
        Assert.True(panel.isOpen);
    }
}
