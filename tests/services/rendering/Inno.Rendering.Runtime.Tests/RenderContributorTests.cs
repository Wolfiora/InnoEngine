using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Inno.Adapter.Modules.DotNet;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Core.Input;
using Inno.Core.Mathematics;
using Inno.Core.Serialization;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Reload;
using Inno.Input;
using Inno.References;
using Inno.Rendering;
using Inno.Rendering.Assets;
using Inno.Runtime.Contracts;
using Inno.Testing.Source;

namespace Inno.Rendering.Runtime.Tests;

[Collection(nameof(RenderRuntimeFixture))]
public sealed class RenderContributorTests : RenderRuntimeFixture
{
    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public void CompletedOrRejectedFramesReleaseCollectibleSnapshotScratchAndGraphCallbacks(bool rejectGraph)
    {
        DotNetModuleSource plugin = CreateRenderingPluginRequest();
        m_modules.Load(plugin);
        using var runtime = new RenderRuntime(
            m_types,
            TestDeviceProxy.Create(out TestDeviceProxy proxy),
            new TestDiagnosticSink()
        );
        runtime.SetPrimaryModelOutputEnabled(false);
        WeakReference contributor = CompleteCollectibleContributorFrame(runtime, proxy, rejectGraph);
        (IAssemblyUnloadProbe monitor, IRenderRuntimeReloadTransaction transaction) = RemoveRenderingPlugin(runtime, m_modules, plugin);
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
        using var runtime = new RenderRuntime(
            m_types,
            TestDeviceProxy.Create(out TestDeviceProxy proxy),
            new TestDiagnosticSink()
        );
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
        using var runtime = new RenderRuntime(
            m_types,
            TestDeviceProxy.Create(out TestDeviceProxy proxy),
            new TestDiagnosticSink()
        );
        runtime.SetPrimaryModelOutputEnabled(false);
        List<CountingContributor> contributors = [];
        for (int index = 0; index < contributorCount; index++)
        {
            var contributor = new CountingContributor { fail = fail };
            contributors.Add(contributor);
            runtime.RegisterContributor(contributor);
        }
        for (int frame = 0; frame < 16; frame++)
        {
            CompleteContributorFrame(runtime, proxy);
        }
        const int C_FRAMES = 64;
        long before = GC.GetAllocatedBytesForCurrentThread();
        long start = Stopwatch.GetTimestamp();
        for (int frame = 0; frame < C_FRAMES; frame++)
        {
            CompleteContributorFrame(runtime, proxy);
        }
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.All(contributors, contributor => Assert.Equal(C_FRAMES + 16, contributor.builds));
        Assert.Equal(C_FRAMES + 16, proxy.endFrameCount);
        Assert.Equal(contributorCount != 0 && !fail ? C_FRAMES + 16 : 0, proxy.executeCount);
        string directory = Path.Combine(
            TestRepository.GetRoot(typeof(RenderRuntimeFixture).Assembly),
            "artifacts/acceptance/architecture-cleanup/results"
        );
        Directory.CreateDirectory(directory);
        File.WriteAllText(
            Path.Combine(directory, $"frames-{contributorCount}-{fail}.json"),
            JsonSerializer.Serialize(new
            {
                contributorCount,
                fail,
                C_FRAMES,
                elapsedMs,
                allocatedBytes,
                bytesPerFrame = allocatedBytes / (double)C_FRAMES,
                millisecondsPerFrame = elapsedMs / C_FRAMES,
                graphCompileCount = ReadGraphCompileCount(runtime),
                runtime = RuntimeInformation.FrameworkDescription
            }, new JsonSerializerOptions { WriteIndented = true })
        );
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(8)]
    [InlineData(32)]
    public void ContributorRetirementMeasurementsIncludeRegistrationAndTheFollowingEmptyFrame(int contributorCount)
    {
        using var runtime = new RenderRuntime(
            m_types,
            TestDeviceProxy.Create(out TestDeviceProxy proxy),
            new TestDiagnosticSink()
        );
        runtime.SetPrimaryModelOutputEnabled(false);
        CountingContributor[] contributors = Enumerable.Range(0, contributorCount)
            .Select(static _ => new CountingContributor())
            .ToArray();
        foreach (CountingContributor contributor in contributors)
        {
            runtime.RegisterContributor(contributor);
        }
        for (int cycle = 0; cycle < 16; cycle++)
        {
            CompleteContributorRetirementCycle(runtime, proxy, contributors);
        }

        const int C_CYCLES = 64;
        long before = GC.GetAllocatedBytesForCurrentThread();
        long start = Stopwatch.GetTimestamp();
        for (int cycle = 0; cycle < C_CYCLES; cycle++)
        {
            CompleteContributorRetirementCycle(runtime, proxy, contributors);
        }
        double elapsedMs = Stopwatch.GetElapsedTime(start).TotalMilliseconds;
        long allocatedBytes = GC.GetAllocatedBytesForCurrentThread() - before;
        Assert.All(contributors, contributor => Assert.Equal(C_CYCLES + 16, contributor.builds));
        Assert.Equal(2 * (C_CYCLES + 16), proxy.endFrameCount);
        Assert.Equal(contributorCount == 0 ? 0 : C_CYCLES + 16, proxy.executeCount);

        string directory = Path.Combine(
            TestRepository.GetRoot(typeof(RenderRuntimeFixture).Assembly),
            "artifacts/acceptance/architecture-cleanup/results"
        );
        Directory.CreateDirectory(directory);
        File.WriteAllText(
            Path.Combine(directory, $"retirement-{contributorCount}.json"),
            JsonSerializer.Serialize(new
            {
                contributorCount,
                C_CYCLES,
                frames = 2 * C_CYCLES,
                elapsedMs,
                allocatedBytes,
                bytesPerCycle = allocatedBytes / (double)C_CYCLES,
                millisecondsPerCycle = elapsedMs / C_CYCLES,
                graphCompileCount = ReadGraphCompileCount(runtime),
                runtime = RuntimeInformation.FrameworkDescription
            }, new JsonSerializerOptions { WriteIndented = true })
        );
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
        IRenderFrameGraphContributor contributor = (IRenderFrameGraphContributor)Activator.CreateInstance(
            type,
            new Action<IRenderFrameGraphContributor>(value => Assert.True(runtime.UnregisterContributor(value))),
            rejectGraph
        )!;
        var weak = new WeakReference(contributor);
        runtime.RegisterContributor(contributor);
        CompleteContributorFrame(runtime, proxy);
        return weak;
    }

    private static void CompleteContributorRetirementCycle(
        RenderRuntime runtime,
        TestDeviceProxy proxy,
        IReadOnlyList<CountingContributor> contributors
    ) {
        CompleteContributorFrame(runtime, proxy);
        foreach (CountingContributor contributor in contributors)
        {
            Assert.True(runtime.UnregisterContributor(contributor));
        }
        CompleteContributorFrame(runtime, proxy);
        foreach (CountingContributor contributor in contributors)
        {
            runtime.RegisterContributor(contributor);
        }
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
            graph.AddRasterPass(
                fail ? "Discarded" : "Accepted",
                new RenderPhaseID("tests.contributor"),
                0,
                static (
                    payload,
                    context
                ) => {}
            ).HasSideEffect();
            if (fail)
            {
                throw new InvalidOperationException("Discard this contributor's complete mutation.");
            }
        }
    }
}
