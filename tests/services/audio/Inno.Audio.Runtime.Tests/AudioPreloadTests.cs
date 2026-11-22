using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Assets;
using Inno.Audio.Runtime;
using Inno.Core.Diagnostics;
using Inno.Core.Execution;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Audio.Runtime.Tests;

[Collection(nameof(AudioRuntimeFixture))]
public sealed class AudioPreloadTests : AudioRuntimeFixture
{
    [Fact]
    public async Task PreloadRetainsDistinctDecodeAndStreamEntriesUntilReleased()
    {
        AudioClipAsset clip = CreateClip(frameCount: 4800);
        using AudioRuntime runtime = CreateRuntime();

        await runtime.PreloadAsync(clip, AudioClipLoadMode.Decode);
        await runtime.PreloadAsync(clip, AudioClipLoadMode.Stream);
        Assert.Equal(2, runtime.statistics.loadedClips);

        runtime.ReleasePreload(clip);
        runtime.ReleasePreload(clip);
        Assert.Equal(0, runtime.statistics.loadedClips);
    }

    [Fact]
    public async Task AutomaticPreparationStreamsWhenDecodedFootprintExceedsBudget()
    {
        AudioClipAsset clip = CreateClip(frameCount: 4800);
        using AudioRuntime runtime = CreateRuntime(
            decodedCacheBudgetBytes: 1024,
            automaticStreamingThresholdBytes: long.MaxValue
        );

        await runtime.PreloadAsync(clip);

        Assert.Equal(1, runtime.statistics.loadedClips);
        Assert.Equal(0, runtime.statistics.decodedBytes);
        Assert.Throws<InvalidOperationException>(() => runtime.PreloadAsync(clip, AudioClipLoadMode.Decode));
    }

    [Fact]
    public async Task PreloadCompletesOnlyAfterPreparationAndCancellationReleasesReservation()
    {
        var device = new ReadyAudioDevice { clipState = AudioClipState.Preparing };
        using var runtime = new AudioRuntime(
            m_types,
            device,
            m_artifacts,
            m_events,
            new DiagnosticHub().CreateReporter(new DiagnosticSource("test.preload", "Preload"))
        );
        AudioClipAsset clip = CreateClip(4800);
        ValueTask ready = runtime.PreloadAsync(clip);
        Assert.False(ready.IsCompleted);
        device.clipState = AudioClipState.Ready;
        runtime.Update(0f);
        await ready;
        runtime.ReleasePreload(clip);
        Assert.Equal(0, runtime.statistics.loadedClips);
        device.clipState = AudioClipState.Preparing;
        using var cancellation = new CancellationTokenSource();
        ValueTask canceled = runtime.PreloadAsync(clip, cancellationToken: cancellation.Token);
        cancellation.Cancel();
        runtime.Update(0f);
        await Assert.ThrowsAnyAsync<OperationCanceledException>(() => canceled.AsTask());
        Assert.Equal(0, runtime.statistics.loadedClips);
    }

    [Theory]
    [InlineData(false)]
    [InlineData(true)]
    public async Task PreloadedArtifactRetirementResumesWithoutDestroyingTheNativeClipTwice(bool shutdown)
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.artifact", "Artifact"));
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        AudioClipAsset clip = CreateClip(4800);
        await runtime.PreloadAsync(clip);
        m_artifacts.pendingReleaseID = clip.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        Action release = shutdown ? runtime.Dispose : () => runtime.ReleasePreload(clip);
        Assert.Throws<RetirementPendingException>(release);
        Assert.Equal(1, device.clipRetirements);
        Assert.Single(m_artifacts.retainedKeys);
        Assert.Equal(0, device.disposals);
        if (shutdown)
        {
            runtime.Dispose();
        }
        else
        {
            runtime.Update(0);
        }
        Assert.Equal(1, device.clipRetirements);
        Assert.Empty(m_artifacts.retainedKeys);
        if (!shutdown)
        {
            Assert.Equal(0, runtime.statistics.loadedClips);
        }
    }

    [Fact]
    public void CanceledPreloadDoesNotCompleteBeforeItsPendingArtifactRetires()
    {
        var device = new ReadyAudioDevice { clipState = AudioClipState.Preparing };
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.cancel", "Cancel"));
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        AudioClipAsset clip = CreateClip(4800);
        using var cancellation = new CancellationTokenSource();
        Task pending = runtime.PreloadAsync(clip, cancellationToken: cancellation.Token).AsTask();
        m_artifacts.pendingReleaseID = clip.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        cancellation.Cancel();
        Assert.Throws<RetirementPendingException>(() => runtime.Update(0));
        Assert.False(pending.IsCompleted);
        Assert.Single(m_artifacts.retainedKeys);
        Assert.Equal(1, device.clipRetirements);
        runtime.Update(0);
        Assert.True(pending.IsCanceled);
        Assert.Equal(1, device.clipRetirements);
        Assert.Empty(m_artifacts.retainedKeys);
        Assert.Equal(0, runtime.statistics.loadedClips);
    }

    [Fact]
    public async Task CacheHitRequestRetiresItsExtraArtifactThroughTheSharedGenerationBarrier()
    {
        using AudioRuntime runtime = CreateRuntime();
        AudioClipAsset clip = CreateClip(4800);
        await runtime.PreloadAsync(clip);
        m_artifacts.pendingReleaseID = clip.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        await runtime.PreloadAsync(clip);
        Assert.Equal(2, m_artifacts.releaseAttempts);
        runtime.ReleasePreload(clip);
        Assert.Single(m_artifacts.retainedKeys);
        runtime.ReleasePreload(clip);
        Assert.Empty(m_artifacts.retainedKeys);
    }

    [Fact]
    public async Task RetryingPreloadReleaseDoesNotConsumeAnotherLoadModeReservation()
    {
        using AudioRuntime runtime = CreateRuntime();
        AudioClipAsset clip = CreateClip(4800);
        await runtime.PreloadAsync(clip, AudioClipLoadMode.Decode);
        await runtime.PreloadAsync(clip, AudioClipLoadMode.Stream);
        m_artifacts.pendingReleaseID = clip.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        Assert.Throws<RetirementPendingException>(() => runtime.ReleasePreload(clip));
        runtime.ReleasePreload(clip);
        Assert.Equal(1, runtime.statistics.loadedClips);
        Assert.Single(m_artifacts.retainedKeys);
        runtime.ReleasePreload(clip);
        Assert.Equal(0, runtime.statistics.loadedClips);
        Assert.Empty(m_artifacts.retainedKeys);
    }

    [Theory]
    [InlineData(false, false)]
    [InlineData(false, true)]
    [InlineData(true, false)]
    [InlineData(true, true)]
    public async Task FailedPreloadAdmissionRetiresOnlyItsOwnReservation(
        bool cached,
        bool queryThrows
    ) {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.query", "Query"));
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        AudioClipAsset clip = CreateClip(4800);
        if (cached)
        {
            await runtime.PreloadAsync(clip);
        }
        if (queryThrows)
        {
            device.clipQueryFailure = new InvalidOperationException("Expected clip query failure.");
        }
        else
        {
            device.clipState = AudioClipState.Failed;
        }
        m_artifacts.pendingReleaseID = clip.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        InvalidOperationException failure = Assert.Throws<InvalidOperationException>(() => runtime.PreloadAsync(clip));
        Assert.Contains(queryThrows ? "query failure" : "preparation failed", failure.Message);
        Assert.Equal(cached ? 1 : 0, runtime.statistics.loadedClips);
        Assert.Equal(cached ? 0 : 1, device.clipRetirements);
        Assert.Equal(2, m_artifacts.releaseAttempts);
        device.clipQueryFailure = null;
        device.clipState = AudioClipState.Ready;
        if (cached)
        {
            Assert.Single(m_artifacts.retainedKeys);
            runtime.ReleasePreload(clip);
        }
        Assert.Empty(m_artifacts.retainedKeys);
        Assert.Equal(0, runtime.statistics.loadedClips);
        await runtime.PreloadAsync(clip);
        runtime.ReleasePreload(clip);
        Assert.Empty(m_artifacts.retainedKeys);
    }

    [Fact]
    public void PreloadQueryAndRollbackFailureRetainBothDiagnosticsAndFaultTheGeneration()
    {
        var device = new ReadyAudioDevice
        {
            clipQueryFailure = new InvalidOperationException("Expected clip query failure."),
            failClipRetirement = true
        };
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.rollback", "Rollback"));
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        AudioClipAsset clip = CreateClip(4800);
        AggregateException failure = Assert.Throws<AggregateException>(() => runtime.PreloadAsync(clip));
        Assert.Contains(failure.Flatten().InnerExceptions, error => error.Message == "Expected clip query failure.");
        Assert.Contains(failure.Flatten().InnerExceptions, error => error.Message == "Expected clip retirement failure.");
        Assert.Empty(m_artifacts.retainedKeys);
        Assert.Equal("Faulted", m_modules.generations.state.ToString());
        Assert.Throws<InvalidOperationException>(() => runtime.Update(0));
        runtime.Dispose();
        Assert.Equal(1, device.clipRetirements);
        Assert.Equal(1, device.disposals);
    }
}
