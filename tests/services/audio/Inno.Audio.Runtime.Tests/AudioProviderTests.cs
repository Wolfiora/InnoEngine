using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Assets;
using Inno.Audio.Runtime;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Core.Identity;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Audio.Runtime.Tests;

[Collection(nameof(AudioRuntimeFixture))]
public sealed class AudioProviderTests : AudioRuntimeFixture
{
    [Fact]
    public void ProviderFailureDiscardsItsEntireContributionAndReleasesItsIdentities()
    {
        var content = new TestAudioContent();
        var identities = new IdentityAllocator();
        identities.Register(content);
        AudioClipAsset clip = CreateClip(48_000);
        var emitter = new AudioEmitterSnapshot(Guid.NewGuid(), clip, new AudioPlayOptions(loop: true), true);
        var listener = new AudioListenerSnapshot(Guid.NewGuid(), 0, default, true);
        content.first = context =>
        {
            context.Submit(emitter);
            context.Submit(listener);
            throw new InvalidOperationException("Expected partial provider failure.");
        };
        content.second = context =>
        {
            context.Submit(emitter);
            context.Submit(listener);
        };
        var diagnostics = new DiagnosticHub();
        var sink = new RecordingSink();
        diagnostics.RegisterSink(sink);
        using DiagnosticReporter reporter = diagnostics.CreateReporter(new DiagnosticSource("test.content", "Content"));
        var device = new ReadyAudioDevice();
        using var runtime = new AudioRuntime(
            m_types,
            device,
            m_artifacts,
            m_events,
            reporter,
            contentScopeProvider: () => new ContentReadScope([content.identity])
        );

        runtime.Update(0);
        Assert.Equal(new AudioContentStatistics(1024, 2, 1), runtime.contentStatistics);
        Assert.Equal(1, runtime.statistics.activeVoices);
        Assert.Equal(1, device.listenerCreations);
        Assert.Single(sink.issues.Where(issue => issue.code == "AUDIO_CONTENT_PROVIDER_FAILED"));
        content.first = null;
        runtime.Update(0);
        Assert.Equal(0, runtime.contentStatistics.rejectedProviders);
        Assert.DoesNotContain(sink.issues, issue => issue.code == "AUDIO_CONTENT_PROVIDER_FAILED");
    }

    [Fact]
    public void CrossProviderDuplicateRejectsTheWholeBatchWithoutClaimingOtherIdentities()
    {
        var content = new TestAudioContent();
        var identities = new IdentityAllocator();
        identities.Register(content);
        AudioClipAsset clip = CreateClip(48_000);
        var first = new AudioEmitterSnapshot(Guid.NewGuid(), clip, new AudioPlayOptions(loop: true), true);
        var second = new AudioEmitterSnapshot(Guid.NewGuid(), clip, new AudioPlayOptions(loop: true), true);
        content.first = context => context.Submit(first);
        content.second = context =>
        {
            context.Submit(second);
            context.Submit(first);
        };
        content.third = context => context.Submit(second);
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.duplicate", "Duplicate"));
        using var runtime = new AudioRuntime(
            m_types,
            new ReadyAudioDevice(),
            m_artifacts,
            m_events,
            reporter,
            contentScopeProvider: () => new ContentReadScope([content.identity])
        );

        runtime.Update(0);
        Assert.Equal(new AudioContentStatistics(1024, 2, 1), runtime.contentStatistics);
        Assert.Equal(2, runtime.statistics.activeVoices);
    }

    [Fact]
    public void RepeatedOverflowIsBoundedAndCannotBeHiddenByAProviderCatch()
    {
        var content = new TestAudioContent();
        var identities = new IdentityAllocator();
        identities.Register(content);
        AudioClipAsset clip = CreateClip(48_000);
        var first = new AudioEmitterSnapshot(Guid.NewGuid(), clip, new AudioPlayOptions(loop: true), true);
        var overflow = new AudioEmitterSnapshot(Guid.NewGuid(), clip, new AudioPlayOptions(loop: true), true);
        var last = new AudioEmitterSnapshot(Guid.NewGuid(), clip, new AudioPlayOptions(loop: true), true);
        content.first = context => context.Submit(first);
        content.second = context =>
        {
            context.Submit(overflow);
            try
            {
                context.Submit(last);
            }
            catch (InvalidOperationException) { }
        };
        content.third = context => context.Submit(last);
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.capacity", "Capacity"));
        var options = new AudioRuntimeOptions { maxContentSnapshots = 2 };
        using var runtime = new AudioRuntime(
            m_types,
            new ReadyAudioDevice(),
            m_artifacts,
            m_events,
            reporter,
            options,
            () => new ContentReadScope([content.identity])
        );
        options.maxContentSnapshots = 100;

        for (int frame = 0; frame < 256; frame++)
        {
            runtime.Update(0);
            Assert.Equal(new AudioContentStatistics(2, 2, 1), runtime.contentStatistics);
            Assert.Equal(2, runtime.statistics.activeVoices);
            Assert.Equal(1, runtime.statistics.loadedClips);
        }
        content.first = content.second = content.third = null;
        runtime.Update(0);
        Assert.Equal(new AudioContentStatistics(2, 0, 0), runtime.contentStatistics);
        Assert.Equal(0, runtime.statistics.activeVoices);
    }

    [Fact]
    public void ProviderContextAndContentLookupAreRevokedAfterTheUpdate()
    {
        var content = new TestAudioContent();
        var identities = new IdentityAllocator();
        identities.Register(content);
        AudioContentProviderContext? captured = null;
        ContentReadScope? capturedScope = null;
        content.first = context =>
        {
            captured = context;
            capturedScope = context.content;
        };
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.scope", "Scope"));
        using var runtime = new AudioRuntime(
            m_types,
            new ReadyAudioDevice(),
            m_artifacts,
            m_events,
            reporter,
            contentScopeProvider: () => new ContentReadScope([content.identity])
        );
        runtime.Update(0);
        Assert.NotNull(captured);
        Assert.Throws<ObjectDisposedException>(() => captured!.Submit(default(AudioListenerSnapshot)));
        Assert.Throws<ObjectDisposedException>(() => _ = captured!.emitters);
        Assert.Throws<ObjectDisposedException>(() => capturedScope!.GetValues<TestAudioContent>());
    }

    [Fact]
    public void ProviderRetirementPendingIsNotConvertedToARejectedContribution()
    {
        var content = new TestAudioContent();
        var identities = new IdentityAllocator();
        identities.Register(content);
        bool nextInvoked = false;
        AudioContentProviderContext? captured = null;
        content.first = context =>
        {
            captured = context;
            throw new RetirementPendingException("Expected provider work drain.");
        };
        content.second = _ => nextInvoked = true;
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.pending-content", "Pending"));
        using var runtime = new AudioRuntime(
            m_types,
            new ReadyAudioDevice(),
            m_artifacts,
            m_events,
            reporter,
            contentScopeProvider: () => new ContentReadScope([content.identity])
        );
        Assert.Throws<RetirementPendingException>(() => runtime.Update(0));
        Assert.False(nextInvoked);
        Assert.Equal(0, runtime.contentStatistics.rejectedProviders);
        Assert.Throws<ObjectDisposedException>(() => _ = captured!.listeners);
    }

    [Theory]
    [InlineData(-1f)]
    [InlineData(float.NaN)]
    [InlineData(float.PositiveInfinity)]
    public void InvalidUpdateTimeIsRejectedBeforeProviderExecution(float deltaTime)
    {
        using AudioRuntime runtime = CreateRuntime();
        Assert.Throws<ArgumentOutOfRangeException>(() => runtime.Update(deltaTime));
    }

    [Fact]
    public void PendingProviderIsRetriedBeforeItsGenerationIsReleased()
    {
        using AudioRuntime runtime = CreateRuntime();
        runtime.Update(0);
        RetirementProviderFirst.disposals = 0;
        RetirementProviderFirst.pendingRetirements = 1;
        try
        {
            m_types.Rebuild();
            Assert.Equal(2, RetirementProviderFirst.disposals);
            Assert.NotEqual("Faulted", m_modules.generations.state.ToString());
        }
        finally
        {
            RetirementProviderFirst.pendingRetirements = 0;
        }
    }

    [Fact]
    public void ProviderRetirementAttemptsEveryInstanceAndFaultsSharedGeneration()
    {
        using AudioRuntime runtime = CreateRuntime();
        runtime.Update(0);
        RetirementProviderFirst.disposals = 0;
        RetirementProviderSecond.disposals = 0;
        RetirementProviderFirst.fail = true;
        try
        {
            Assert.ThrowsAny<Exception>(m_types.Rebuild);
            Assert.Equal(1, RetirementProviderFirst.disposals);
            Assert.Equal(1, RetirementProviderSecond.disposals);
            Assert.Equal("Faulted", m_modules.generations.state.ToString());
        }
        finally
        {
            RetirementProviderFirst.fail = false;
        }
    }
}
