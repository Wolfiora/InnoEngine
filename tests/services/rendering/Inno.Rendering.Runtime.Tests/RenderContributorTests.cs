using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    [InlineData(32, false)]
    [InlineData(0, true)]
    [InlineData(1, true)]
    [InlineData(32, true)]
    public void EveryFrameBuildsEachContributorAndExecutesOnlyAcceptedGraphs(
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

        const int C_FRAMES = 80;
        for (int frame = 0; frame < C_FRAMES; frame++)
        {
            CompleteContributorFrame(runtime, proxy);
        }

        Assert.All(contributors, contributor => Assert.Equal(C_FRAMES, contributor.builds));
        Assert.Equal(C_FRAMES, proxy.endFrameCount);
        Assert.Equal(contributorCount != 0 && !fail ? C_FRAMES : 0, proxy.executeCount);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(32)]
    public void ContributorRetirementCyclesRebuildAfterRegistrationAndCompleteTheFollowingEmptyFrame(int contributorCount)
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

        const int C_CYCLES = 80;
        for (int cycle = 0; cycle < C_CYCLES; cycle++)
        {
            CompleteContributorRetirementCycle(runtime, proxy, contributors);
        }

        Assert.All(contributors, contributor => Assert.Equal(C_CYCLES, contributor.builds));
        Assert.Equal(2 * C_CYCLES, proxy.endFrameCount);
        Assert.Equal(contributorCount == 0 ? 0 : C_CYCLES, proxy.executeCount);
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
