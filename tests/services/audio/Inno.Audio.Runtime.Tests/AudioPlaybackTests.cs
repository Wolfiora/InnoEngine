using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Inno.Assets;
using Inno.Core.Diagnostics;
using Inno.Core.Events;
using Inno.Core.Execution;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Audio.Runtime.Tests;

[Collection(nameof(AudioRuntimeFixture))]
public sealed class AudioPlaybackTests : AudioRuntimeFixture
{
    [Fact]
    public void InvalidPlaybackOptionsDoNotStealOrAcquireAnArtifact()
    {
        using AudioRuntime runtime = CreateRuntime(maxVoices: 1);
        AudioVoiceHandle previous = runtime.Play(CreateClip(48_000), new AudioPlayOptions(loop: true));
        runtime.Update(0);
        AudioClipAsset next = CreateClip(48_000);
        int acquisitions = m_artifacts.acquisitions;
        Assert.Throws<ArgumentException>(() => runtime.Play(next, default));
        Assert.Equal(acquisitions, m_artifacts.acquisitions);
        Assert.Equal(0, runtime.statistics.stolenVoiceCount);
        Assert.True(runtime.TryGetVoiceState(previous, out AudioPlaybackState state));
        Assert.Equal(AudioPlaybackState.Playing, state);
        Assert.False(runtime.SetVoiceParameters(previous, default));
        Assert.True(runtime.SetVoiceParameters(previous, new AudioVoiceParameters(0.5f, 1, 0)));
    }

    [Fact]
    public void ArtifactAcquisitionRetirementBarrierDoesNotStealOrBecomeDecodeFailure()
    {
        using AudioRuntime runtime = CreateRuntime(maxVoices: 1);
        AudioVoiceHandle previous = runtime.Play(CreateClip(48_000), new AudioPlayOptions(loop: true));
        runtime.Update(0);
        AudioClipAsset next = CreateClip(48_000);
        m_artifacts.acquisitionFailure = new RetirementPendingException("Expected artifact retirement barrier.");
        Assert.Throws<RetirementPendingException>(() => runtime.Play(next));
        Assert.Equal(0, runtime.statistics.stolenVoiceCount);
        Assert.Equal(1, runtime.statistics.activeVoices);
        Assert.True(runtime.TryGetVoiceState(previous, out AudioPlaybackState state));
        Assert.Equal(AudioPlaybackState.Playing, state);
        m_artifacts.acquisitionFailure = null;
    }

    [Fact]
    public void AdmissionRetryAfterCompletionBackpressureReleasesCandidateLeaseAndStealsOnce()
    {
        var events = new EventDispatcher(queueCapacity: 1);
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.admission", "Admission"));
        using var runtime = new AudioRuntime(
            m_types,
            device,
            m_artifacts,
            events,
            reporter,
            new AudioRuntimeOptions { maxVoices = 1 }
        );
        using EventHub hub = events.CreateHub();
        List<AudioVoiceCompletedEvent> completions = [];
        hub.Listen<AudioVoiceCompletedEvent>(completions.Add);
        AudioVoiceHandle previous = runtime.Play(CreateClip(48_000), new AudioPlayOptions(loop: true));
        runtime.Update(0);
        AudioClipAsset next = CreateClip(48_000);
        events.Enqueue(new AudioVoiceCompletedEvent(new AudioVoiceAllocator().Allocate(), AudioCompletionReason.Stopped));
        m_artifacts.pendingReleaseID = next.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        Assert.Throws<InvalidOperationException>(() => runtime.Play(next));
        Assert.Empty(m_artifacts.retainedKeys);
        Assert.Equal(3, m_artifacts.releaseAttempts);
        Assert.Equal(1, runtime.statistics.stolenVoiceCount);
        Assert.Equal(1, device.stops);
        events.Flush();
        completions.Clear();

        AudioVoiceHandle accepted = runtime.Play(next);
        Assert.True(accepted.isValid);
        Assert.Equal(1, runtime.statistics.activeVoices);
        Assert.Single(m_artifacts.retainedKeys);
        Assert.Equal(1, device.stops);
        Assert.Equal(1, runtime.statistics.stolenVoiceCount);
        events.Flush();
        AudioVoiceCompletedEvent completion = Assert.Single(completions);
        Assert.Equal(previous, completion.voice);
        Assert.Equal(AudioCompletionReason.Stolen, completion.reason);
    }

    [Fact]
    public void OrdinaryMissingMetadataStillCompletesThroughTheAsynchronousVoiceProtocol()
    {
        using AudioRuntime runtime = CreateRuntime();
        using EventHub hub = m_events.CreateHub();
        List<AudioVoiceCompletedEvent> completed = [];
        hub.Listen<AudioVoiceCompletedEvent>(completed.Add);
        AudioVoiceHandle voice = runtime.Play(new AudioClipAsset());
        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState preparing));
        Assert.Equal(AudioPlaybackState.Preparing, preparing);
        runtime.Update(0);
        m_events.Flush();
        Assert.Equal(voice, Assert.Single(completed).voice);
        Assert.Equal(AudioCompletionReason.DecodeFailed, completed[0].reason);
    }

    [Fact]
    public void UndefinedPreloadModeIsRejectedBeforeArtifactAcquisition()
    {
        using AudioRuntime runtime = CreateRuntime();
        AudioClipAsset clip = CreateClip(48_000);
        Assert.Throws<ArgumentOutOfRangeException>(() => runtime.PreloadAsync(clip, (AudioClipLoadMode)999));
        Assert.Equal(0, m_artifacts.acquisitions);
        Assert.Equal(0, runtime.statistics.loadedClips);
    }

    [Fact]
    public void MutedBackendRejectsInvalidValuesWithTheSameAdmissionPolicy()
    {
        using var device = new MutedAudioDevice();
        IAudioDevice backend = device;
        AudioClipAsset asset = CreateClip(48_000);
        using ArtifactLease artifact = m_artifacts.AcquireArtifact(asset.identity.persistentID, "audio-data");
        AudioBusHandle master = backend.CreateBus(AudioBusID.master);
        AudioClipHandle clip = backend.CreateClip(
            new AudioClipDescriptor(            AudioCodecID.wav, AudioClipLoadMode.Decode, 2, 48_000, 48_000, artifact.info.length),
            new LeaseAudioTestSource(artifact)
        );
        Assert.False(backend.Play(clip, master, default).isValid);
        foreach (double invalid in new[] { double.NaN, double.PositiveInfinity, double.NegativeInfinity, -1d })
        {
            Assert.False(backend.Play(clip, master, AudioPlayOptions.defaultValue, invalid).isValid);
        }
        foreach (float invalid in new[] { float.NaN, float.PositiveInfinity, float.NegativeInfinity })
        {
            Assert.False(backend.SetBusVolume(master, invalid));
        }
        Assert.Equal(0, backend.statistics.activeVoices);
        AudioDeviceVoiceHandle voice = backend.Play(clip, master, AudioPlayOptions.defaultValue);
        Assert.False(backend.SetVoiceParameters(voice, default));
        Assert.True(backend.SetVoiceParameters(voice, new AudioVoiceParameters(0.5f, 1, 0)));
        Assert.True(backend.Stop(voice));
        Assert.True(backend.DestroyClip(clip));
        Assert.True(backend.DestroyBus(master));
    }

    [Fact]
    public void CompletionBackpressureRetainsTheVoiceUntilItsEventIsAccepted()
    {
        var events = new EventDispatcher(queueCapacity: 1);
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.completion", "Completion"));
        using var runtime = new AudioRuntime(m_types, new ReadyAudioDevice(), m_artifacts, events, reporter);
        using EventHub hub = events.CreateHub();
        List<AudioVoiceCompletedEvent> completed = [];
        hub.Listen<AudioVoiceCompletedEvent>(completed.Add);
        AudioVoiceHandle voice = runtime.Play(CreateClip(4800));
        events.Enqueue(new AudioVoiceCompletedEvent(voice, AudioCompletionReason.Stopped));

        Assert.Throws<InvalidOperationException>(() => runtime.Update(1));
        Assert.Equal(1, runtime.statistics.activeVoices);
        events.Flush();
        completed.Clear();
        runtime.Update(0);
        events.Flush();
        Assert.Equal(voice, Assert.Single(completed).voice);
        Assert.Equal(0, runtime.statistics.activeVoices);
        runtime.Update(0);
        events.Flush();
        Assert.Single(completed);
    }

    [Fact]
    public void PlayTransitionsFromPreparingToNaturalCompletionOnMainThread()
    {
        AudioClipAsset clip = CreateClip(frameCount: 4800);
        using AudioRuntime runtime = CreateRuntime();
        using EventHub hub = m_events.CreateHub();
        List<AudioVoiceCompletedEvent> completions = [];
        hub.Listen<AudioVoiceCompletedEvent>(completions.Add);

        AudioVoiceHandle voice = runtime.Play(clip);
        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState preparing));
        Assert.Equal(AudioPlaybackState.Preparing, preparing);

        runtime.Update(0.01f);
        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState playing));
        Assert.Equal(AudioPlaybackState.Playing, playing);
        runtime.Update(0.2f);
        m_events.Flush();

        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState completed));
        Assert.Equal(AudioPlaybackState.Completed, completed);
        Assert.Single(completions);
        Assert.Equal(AudioCompletionReason.NaturalEnd, completions[0].reason);
    }

    [Fact]
    public void ScheduledLoopingVoiceSupportsPauseResumeAndSeek()
    {
        AudioClipAsset clip = CreateClip(frameCount: 4800);
        using AudioRuntime runtime = CreateRuntime();
        AudioVoiceHandle voice = runtime.PlayScheduled(clip, 1d, new AudioPlayOptions(loop: true));

        runtime.Update(0.1f);
        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState scheduled));
        Assert.Equal(AudioPlaybackState.Scheduled, scheduled);
        Assert.True(runtime.Pause(voice));
        Assert.True(runtime.Seek(voice, TimeSpan.FromMilliseconds(50)));
        Assert.True(runtime.Resume(voice));
        runtime.Update(1f);
        runtime.Update(1f);

        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState playing));
        Assert.Equal(AudioPlaybackState.Playing, playing);
    }

    [Fact]
    public void VoiceBudgetStealsLowestPriorityThenOldestVoice()
    {
        AudioClipAsset clip = CreateClip(frameCount: 480_000);
        using AudioRuntime runtime = CreateRuntime(maxVoices: 2);
        using EventHub hub = m_events.CreateHub();
        List<AudioVoiceCompletedEvent> completions = [];
        hub.Listen<AudioVoiceCompletedEvent>(completions.Add);
        AudioVoiceHandle first = runtime.Play(clip, new AudioPlayOptions(priority: 1));
        AudioVoiceHandle second = runtime.Play(clip, new AudioPlayOptions(priority: 10));

        AudioVoiceHandle third = runtime.Play(clip, new AudioPlayOptions(priority: 0));
        m_events.Flush();

        Assert.Equal(2, runtime.statistics.activeVoices);
        Assert.True(runtime.TryGetVoiceState(first, out AudioPlaybackState firstState));
        Assert.Equal(AudioPlaybackState.Completed, firstState);
        Assert.True(runtime.TryGetVoiceState(second, out AudioPlaybackState secondState));
        Assert.Equal(AudioPlaybackState.Preparing, secondState);
        Assert.True(runtime.TryGetVoiceState(third, out AudioPlaybackState thirdState));
        Assert.Equal(AudioPlaybackState.Preparing, thirdState);
        Assert.Single(completions);
        Assert.Equal(AudioCompletionReason.Stolen, completions[0].reason);
    }

    [Fact]
    public async Task APartiallyRetiredClipCannotBeReusedByANewVoice()
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.reuse", "Reuse"));
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        AudioClipAsset clip = CreateClip(4800);
        await runtime.PreloadAsync(clip);
        m_artifacts.pendingReleaseID = clip.identity.persistentID;
        m_artifacts.pendingReleases = 1;
        Assert.Throws<RetirementPendingException>(() => runtime.ReleasePreload(clip));
        AudioVoiceHandle voice = runtime.Play(clip, new AudioPlayOptions(loop: true));
        runtime.Update(0);
        Assert.True(runtime.TryGetVoiceState(voice, out AudioPlaybackState state));
        Assert.Equal(AudioPlaybackState.Playing, state);
        Assert.Equal(1, device.clipRetirements);
        Assert.Single(m_artifacts.retainedKeys);
        runtime.Stop(voice);
        runtime.Update(0);
        Assert.Equal(2, device.clipRetirements);
        Assert.Empty(m_artifacts.retainedKeys);
    }
}
