using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.Json;
using Inno.Adapter.Modules.DotNet;
using Inno.Extensibility.Reload;
using Inno.Rendering;
using Xunit;

namespace Inno.Rendering.Runtime.Tests;

public sealed partial class RenderRuntimeGenerationTests
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CompletedOrRejectedFramesReleaseCollectibleSnapshotScratchAndGraphCallbacks(bool rejectGraph)
    {
        DotNetModuleSource plugin = CreateRenderingPluginRequest();
        m_modules.Load(plugin);
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy proxy), new TestDiagnosticSink());
        runtime.SetPrimaryModelOutputEnabled(false);
        WeakReference contributor = CompleteCollectibleContributorFrame(runtime, proxy, rejectGraph);
        (IAssemblyUnloadProbe monitor, IRenderRuntimeReloadTransaction transaction) =
            RemoveRenderingPlugin(runtime, m_modules, plugin);
        ForceCollection();
        Assert.False(contributor.IsAlive);
        Assert.True(monitor.isCompleted);
        CompleteContributorFrame(runtime, proxy);
        GC.KeepAlive(transaction);
        GC.KeepAlive(runtime);
    }

    [Fact]
    public void ContributorRegistrationChangesTakeEffectOnTheNextFrame()
    {
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy proxy), new TestDiagnosticSink());
        runtime.SetPrimaryModelOutputEnabled(false);
        var first = new CountingContributor();
        var next = new CountingContributor();
        runtime.RegisterContributor(first);
        first.onPrepare = () =>
        {
            Assert.True(runtime.UnregisterContributor(first));
            runtime.RegisterContributor(next);
        };
        CompleteContributorFrame(runtime, proxy);
        Assert.Equal(1, first.builds);
        Assert.Equal(0, next.builds);
        CompleteContributorFrame(runtime, proxy);
        Assert.Equal(1, first.builds);
        Assert.Equal(1, next.builds);
    }

    [Fact]
    public void FailedContributorDoesNotPublishResourcesPassesOrPersistentDiagnostics()
    {
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy proxy), diagnostics);
        runtime.SetPrimaryModelOutputEnabled(false);
        var failing = new CountingContributor { fail = true };
        var accepted = new CountingContributor();
        runtime.RegisterContributor(failing);
        runtime.RegisterContributor(accepted);
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Contains(diagnostics.items, diagnostic => diagnostic.code == "RENDER_FRAME_CONTRIBUTOR_FAILED");
        Assert.DoesNotContain(proxy.lastGraph!.passes, pass => pass.name.Contains("Discarded", StringComparison.Ordinal));
        proxy.ReleaseRecordedGraph();
        Assert.True(runtime.UnregisterContributor(failing));
        CompleteContributorFrame(runtime, proxy);
        Assert.DoesNotContain(diagnostics.items, diagnostic => diagnostic.code == "RENDER_FRAME_CONTRIBUTOR_FAILED");
        Assert.Equal(2, accepted.builds);
    }

    [Fact]
    public void UnresolvedModelDiagnosticRemainsUntilTheCauseIsResolved()
    {
        var diagnostics = new TestDiagnosticSink();
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy proxy), diagnostics);
        for (int frame = 0; frame < 3; frame++)
        {
            BeginRenderFrame(runtime, 0f);
            runtime.Render(default);
            runtime.AfterRender(default);
            runtime.EndFrame(default);
            Assert.Contains(diagnostics.items, diagnostic => diagnostic.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE");
            proxy.ReleaseRecordedGraph();
        }
        runtime.SetPrimaryModelOutputEnabled(false);
        CompleteContributorFrame(runtime, proxy);
        Assert.DoesNotContain(diagnostics.items, diagnostic => diagnostic.code == "RENDER_OUTPUT_MODEL_UNAVAILABLE");
    }

    [Theory]
    [InlineData(0, false)]
    [InlineData(1, false)]
    [InlineData(8, false)]
    [InlineData(32, false)]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(8, true)]
    [InlineData(32, true)]
    public void FrameAllocationMeasurementsUseTheRealPublicLifecycle(
        int contributorCount,
        bool fail
    ) {
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy proxy), new TestDiagnosticSink());
        runtime.SetPrimaryModelOutputEnabled(false);
        var contributors = new List<CountingContributor>();
        for (int index = 0; index < contributorCount; index++)
        {
            var contributor = new CountingContributor { fail = fail };
            contributors.Add(contributor);
            runtime.RegisterContributor(contributor);
        }
        for (int frame = 0; frame < 16; frame++)
            CompleteContributorFrame(runtime, proxy);
        const int frames = 64;
        long before = GC.GetAllocatedBytesForCurrentThread();
        long start = Stopwatch.GetTimestamp();
        for (int frame = 0; frame < frames; frame++)
            CompleteContributorFrame(runtime, proxy);
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.All(contributors, contributor => Assert.Equal(frames + 16, contributor.builds));
        Assert.Equal(frames + 16, proxy.endFrameCount);
        Assert.Equal(contributorCount != 0 && !fail ? frames + 16 : 0, proxy.executeCount);
        DirectoryInfo? root = new(AppContext.BaseDirectory);
        while (root is not null && !File.Exists(Path.Combine(root.FullName, "InnoEngine.sln")))
            root = root.Parent;
        Assert.NotNull(root);
        string directory = Path.Combine(root!.FullName, "artifacts/acceptance/architecture-cleanup/results");
        Directory.CreateDirectory(directory);
        File.WriteAllText(Path.Combine(directory, $"frames-{contributorCount}-{fail}.json"), JsonSerializer.Serialize(new
        {
            contributorCount, fail, frames, elapsedMs, allocatedBytes,
            bytesPerFrame = allocatedBytes / (double)frames,
            millisecondsPerFrame = elapsedMs / frames,
            graphCompileCount = ReadGraphCompileCount(runtime),
            runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
        }, new JsonSerializerOptions { WriteIndented = true }));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(8)]
    [InlineData(32)]
    public void ContributorRetirementMeasurementsIncludeRegistrationAndTheFollowingEmptyFrame(int contributorCount)
    {
        using var runtime = new RenderRuntime(m_types, TestDeviceProxy.Create(out TestDeviceProxy proxy), new TestDiagnosticSink());
        runtime.SetPrimaryModelOutputEnabled(false);
        CountingContributor[] contributors = Enumerable.Range(0, contributorCount)
            .Select(static _ => new CountingContributor()).ToArray();
        foreach (CountingContributor contributor in contributors)
            runtime.RegisterContributor(contributor);
        for (int cycle = 0; cycle < 16; cycle++)
            CompleteContributorRetirementCycle(runtime, proxy, contributors);

        const int cycles = 64;
        long before = GC.GetAllocatedBytesForCurrentThread();
        long start = Stopwatch.GetTimestamp();
        for (int cycle = 0; cycle < cycles; cycle++)
            CompleteContributorRetirementCycle(runtime, proxy, contributors);
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.All(contributors, contributor => Assert.Equal(cycles + 16, contributor.builds));
        Assert.Equal(2 * (cycles + 16), proxy.endFrameCount);
        Assert.Equal(contributorCount == 0 ? 0 : cycles + 16, proxy.executeCount);

        DirectoryInfo? root = new(AppContext.BaseDirectory);
        while (root is not null && !File.Exists(Path.Combine(root.FullName, "InnoEngine.sln")))
            root = root.Parent;
        Assert.NotNull(root);
        string directory = Path.Combine(root!.FullName, "artifacts/acceptance/architecture-cleanup/results");
        Directory.CreateDirectory(directory);
        File.WriteAllText(Path.Combine(directory, $"retirement-{contributorCount}.json"), JsonSerializer.Serialize(new
        {
            contributorCount, cycles, frames = 2 * cycles, elapsedMs, allocatedBytes,
            bytesPerCycle = allocatedBytes / (double)cycles,
            millisecondsPerCycle = elapsedMs / cycles,
            graphCompileCount = ReadGraphCompileCount(runtime),
            runtime = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription
        }, new JsonSerializerOptions { WriteIndented = true }));
    }

    private static void CompleteContributorRetirementCycle(
        RenderRuntime runtime,
        TestDeviceProxy proxy,
        IReadOnlyList<CountingContributor> contributors
    ) {
        CompleteContributorFrame(runtime, proxy);
        foreach (CountingContributor contributor in contributors)
            Assert.True(runtime.UnregisterContributor(contributor));
        CompleteContributorFrame(runtime, proxy);
        foreach (CountingContributor contributor in contributors)
            runtime.RegisterContributor(contributor);
    }

    private static void CompleteContributorFrame(
        RenderRuntime runtime,
        TestDeviceProxy proxy
    ) {
        BeginRenderFrame(runtime, 0f);
        runtime.AfterRender(default);
        runtime.EndFrame(default);
        Assert.Equal(1, ReadGraphCompileCount(runtime));
        proxy.ReleaseRecordedGraph();
    }

    private static int ReadGraphCompileCount(RenderRuntime runtime)
    {
        using IDisposable scope = runtime.EnterExecutionScope();
        return GraphicsSettings.frameStatistics!.graphCompileCount;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private WeakReference CompleteCollectibleContributorFrame(
        RenderRuntime runtime,
        TestDeviceProxy proxy,
        bool rejectGraph
    ) {
        Type type = m_types.current.GetTypesImplementing<IRenderFrameGraphContributor>()
            .Select(reference => reference.Resolve(m_types))
            .Single(type => type.Name == "ReloadableContributor");
        var contributor = (IRenderFrameGraphContributor)Activator.CreateInstance(type,
            new Action<IRenderFrameGraphContributor>(value => Assert.True(runtime.UnregisterContributor(value))),
            rejectGraph)!;
        var weak = new WeakReference(contributor);
        runtime.RegisterContributor(contributor);
        CompleteContributorFrame(runtime, proxy);
        return weak;
    }

    private sealed class CountingContributor : IRenderFrameGraphContributor
    {
        internal int builds;
        internal bool fail;
        internal Action? onPrepare;

        public void PrepareFrame(ulong frameIndex) => onPrepare?.Invoke();

        public void AddRenderPasses(
            RenderGraphBuilder graph,
            ulong frameIndex
        ) {
            builds++;
            graph.AddRasterPass(fail ? "Discarded" : "Accepted", new("tests.contributor"), 0,
                static (
                    payload,
                    context
                ) => { }).HasSideEffect();
            if (fail)
                throw new InvalidOperationException("Discard this contributor's complete mutation.");
        }
    }
}
