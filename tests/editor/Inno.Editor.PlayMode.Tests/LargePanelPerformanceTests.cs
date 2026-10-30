using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using Inno.Assets;
using Inno.Core.Serialization;
using Inno.Editor.Interactions;
using Inno.Scene;
using Xunit;
using Xunit.Abstractions;

namespace Inno.Editor.PlayMode.Tests;

public sealed partial class LargePanelPerformanceTests(ITestOutputHelper output)
{
    [Fact]
    [Trait("Acceptance", "Performance")]
    public void DueDirtyInspectionsMeasureTheCompleteSavedSceneTraversal()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        Inno.Scene.GameScene scene = LoadSavedHierarchy(host);
        Measure(host, null, "10000-hierarchy-dirty-inspections",
            () => Assert.False(host.workspace.IsDirty(scene)), static () => Thread.Sleep(110));
    }

    [Fact]
    [Trait("Acceptance", "Performance")]
    public void ChangedDirtyInspectionsMeasureTheCompleteSavedSceneTraversal()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        GameScene scene = LoadSavedHierarchy(host);
        GameObject value = scene.GetObjects()[0];
        int revision = 0;
        Measure(host, null, "10000-hierarchy-dirty-edits", () =>
        {
            value.transform.localPosition = new Inno.Core.Mathematics.Vector3(++revision % 2 + 1, 0, 0);
            Assert.True(host.workspace.IsDirty(scene));
        }, static () => Thread.Sleep(110), consumer: "complete-authoritative-dirty-inspection-after-each-owned-write");
    }

    [Fact]
    [Trait("Acceptance", "Performance")]
    public void NeutralCaptureSeparatesSerializationFromSourceFreshness()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        Inno.Scene.GameScene scene = LoadSavedHierarchy(host);
        SerializationContext context = AssetSerializationContext.Create(host.assets, new AssetDependencyCollection(includeLastKnownPaths: false));
        SerializationSnapshot? previous = null;
        Measure(host, null, "10000-hierarchy-neutral-capture",
            () => previous = host.serialization.CaptureSnapshot(scene, context, previous),
            consumer: "complete-authoritative-serialization-capture");
        Assert.NotNull(previous);
    }

    [Fact]
    [Trait("Acceptance", "Performance")]
    public void SourceFreshnessMeasuresTheActualSavedSceneLoadBoundary()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        Inno.Scene.GameScene scene = LoadSavedHierarchy(host);
        Assert.True(host.workspace.TryGetSourcePath(scene, out string path));
        SceneAsset asset = host.assets.Load<SceneAsset>(AssetPath.Project(path));
        Measure(host, null, "10000-hierarchy-source-freshness",
            () => Assert.Same(asset, host.assets.Load<SceneAsset>(asset.identity.persistentId)),
            consumer: "canonical-source-load-with-freshness-verification");
    }

    [Fact]
    [Trait("Acceptance", "Performance")]
    public void SceneMetadataReadsMeasureTheOwnedAttachmentBoundary()
    {
        using var host = new NativeProjectPanelHost(supportsVertexOffsets: true);
        GameScene scene = LoadSavedHierarchy(host);
        IReadOnlyList<GameObject> objects = scene.GetObjects();
        long checksum = 0;
        Measure(host, null, "10000-hierarchy-metadata-reads", () =>
        {
            foreach (GameObject value in objects)
            {
                checksum += value.name.Length + value.tag.Length + value.layer.index;
                checksum += value.activeSelf ? 1 : 0;
                checksum += value.transform.parent is null ? 1 : 0;
                checksum += value.transform.siblingIndex;
            }
        }, consumer: "public-metadata-getters-with-current-attachment-validation");
        Assert.True(checksum > 0);
    }

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

    private static Inno.Scene.GameScene LoadSavedHierarchy(NativeProjectPanelHost host)
    {
        string? input = InputDirectory("hierarchy");
        Inno.Scene.GameScene scene;
        if (input is not null && Directory.Exists(input))
        {
            CopyDirectory(input, Path.Combine(host.projectRoot, "Assets"));
            host.assets.Rescan();
            scene = host.workspace.Open("Untitled Scene.iscene");
        }
        else
        {
            scene = host.CreateScene();
            for (int index = 0; index < 10_000; index++)
                _ = scene.CreateObject("Object " + index.ToString("D5"));
            _ = host.workspace.Save(scene, string.Empty);
        }
        Assert.Equal(10_000, scene.GetObjects().Count);
        return scene;
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
        EditorPanelExtension? panel,
        string workload,
        Action? operation = null,
        Action? prepare = null,
        string? consumer = null
    ) {
        operation ??= () => host.native.DrawPanel(panel!);
        for (int warmup = 0; warmup < 5; warmup++)
        {
            prepare?.Invoke();
            operation();
        }
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
                prepare?.Invoke();
                long started = Stopwatch.GetTimestamp();
                operation();
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
            workload,
            configuration = typeof(LargePanelPerformanceTests).Assembly.GetCustomAttribute<AssemblyConfigurationAttribute>()?.Configuration
                ?? throw new InvalidOperationException("The benchmark assembly has no SDK configuration metadata."),
            samples,
            framework = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
            viewport = "1000x800", warmupFrames = 5, measuredFramesPerRepetition = 10,
            sharedInput = InputDirectory(workload.Contains("hierarchy", StringComparison.Ordinal) ? "hierarchy" : "assets"),
            consumer = consumer ?? (prepare is null ? "native-command-offset-validation" : "complete-authoritative-dirty-inspection"),
            preparationExcludedFromFrameTiming = prepare is not null,
            supportsVertexOffsets = true
        }));
        if (panel is not null)
            Assert.True(panel.isOpen);
    }
}
