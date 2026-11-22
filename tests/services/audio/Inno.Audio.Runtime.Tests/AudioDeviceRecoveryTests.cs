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
public sealed class AudioDeviceRecoveryTests : AudioRuntimeFixture
{
    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task PendingClipRetirementPreservesDeviceAndRetriesExactlyOnce(bool playing)
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.pending", "Pending"));
        var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        AudioClipAsset clip = CreateClip(48_000);
        if (playing)
        {
            runtime.Play(clip);
            runtime.Update(0);
        }
        else
        {
            await runtime.PreloadAsync(clip);
        }
        device.pendingClipRetirements = 1;

        Assert.Throws<RetirementPendingException>(runtime.Dispose);
        Assert.Equal(0, device.disposals);
        Assert.Equal(0, device.busRetirements);
        Assert.Throws<InvalidOperationException>(() => runtime.Play(clip));

        runtime.Dispose();
        Assert.Equal(2, device.clipRetirements);
        Assert.Equal(1, device.busRetirements);
        Assert.Equal(1, device.disposals);
        runtime.Dispose();
        Assert.Equal(1, device.disposals);
    }

    [Fact]
    public void PendingBusRetirementDoesNotDestroyTheDeviceBeforeRetry()
    {
        var device = new ReadyAudioDevice { pendingBusRetirements = 1 };
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.bus", "Bus"));
        var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        Assert.Throws<RetirementPendingException>(runtime.Dispose);
        Assert.Equal(0, device.disposals);
        runtime.Dispose();
        Assert.Equal(2, device.busRetirements);
        Assert.Equal(1, device.disposals);
    }

    [Fact]
    public void PendingReplacementRetirementDrainsBeforePublishingTheNewDevice()
    {
        var previous = new ReadyAudioDevice { pendingBusRetirements = 1 };
        var replacement = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.replacement", "Replacement"));
        using var runtime = new AudioRuntime(m_types, previous, m_artifacts, m_events, reporter);
        AudioVoiceHandle oldVoice = runtime.Play(CreateClip(48_000));
        runtime.ReplaceDevice(replacement);
        Assert.Equal(2, previous.busRetirements);
        Assert.Equal(1, previous.disposals);
        Assert.Equal(0, replacement.disposals);
        Assert.False(runtime.Stop(oldVoice));
        Assert.True(runtime.TryGetVoiceState(oldVoice, out AudioPlaybackState completed));
        Assert.Equal(AudioPlaybackState.Completed, completed);
        Assert.NotEqual("Faulted", m_modules.generations.state.ToString());
    }

    [Fact]
    public void CandidateCleanupFailureFaultsInsteadOfReportingRecoverableMixerFailure()
    {
        using AudioRuntime runtime = CreateRuntime();
        runtime.Update(0);
        RetirementProviderFirst.fail = true;
        RetirementProviderSecond.failConstruction = true;
        try
        {
            Assert.ThrowsAny<Exception>(m_types.Rebuild);
            Assert.Equal("Faulted", m_modules.generations.state.ToString());
            Assert.Throws<InvalidOperationException>(() => runtime.ApplyMixer(new AudioMixerAsset()));
        }
        finally
        {
            RetirementProviderFirst.fail = false;
            RetirementProviderSecond.failConstruction = false;
        }
    }

    [Fact]
    public async Task CacheRetirementAttemptsEveryClipBeforeDeviceDisposal()
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.retire", "Retire"));
        var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        await runtime.PreloadAsync(CreateClip(4800));
        await runtime.PreloadAsync(CreateClip(9600));
        device.failClipRetirement = true;
        Assert.ThrowsAny<Exception>(runtime.Dispose);
        Assert.Equal(2, device.clipRetirements);
        Assert.Equal(1, device.disposals);
        runtime.Dispose();
    }

    [Fact]
    public void FailedOldDeviceRetirementConsumesCandidateAndFaultsWithoutFalseRecovery()
    {
        var previous = new ReadyAudioDevice { failDisposal = true };
        var candidate = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.replace", "Replace"));
        var runtime = new AudioRuntime(m_types, previous, m_artifacts, m_events, reporter);
        Assert.Throws<AggregateException>(() => runtime.ReplaceDevice(candidate));
        Assert.Equal(1, previous.disposals);
        Assert.Equal(1, candidate.disposals);
        Assert.Equal("Faulted", m_modules.generations.state.ToString());
        Assert.Throws<InvalidOperationException>(() => runtime.Update(0));
        previous.failDisposal = false;
        runtime.Dispose();
    }

    [Fact]
    public void RecoveryClearsMutedAndRecoveryFailureDiagnostics()
    {
        var diagnostics = new DiagnosticHub();
        var sink = new RecordingSink();
        diagnostics.RegisterSink(sink);
        using DiagnosticReporter reporter = diagnostics.CreateReporter(new DiagnosticSource("test.device", "Device"));
        using var runtime = new AudioRuntime(m_types, new MutedAudioDevice(), m_artifacts, m_events, reporter);
        Assert.Contains(sink.issues, issue => issue.code == "AUDIO_NO_DEVICE");
        Assert.False(runtime.TryRecoverDevice(static () => throw new InvalidOperationException("Device unavailable.")));
        Assert.Contains(sink.issues, issue => issue.code == "AUDIO_DEVICE_RECOVERY_FAILED");
        Assert.True(runtime.TryRecoverDevice(static () => new ReadyAudioDevice()));
        Assert.DoesNotContain(sink.issues, issue => issue.code is "AUDIO_NO_DEVICE" or "AUDIO_DEVICE_RECOVERY_FAILED");
    }

    [Fact]
    public void RecoveredMixerResolvesItsPreviousMissingDiagnostic()
    {
        var diagnostics = new DiagnosticHub();
        var sink = new RecordingSink();
        diagnostics.RegisterSink(sink);
        using DiagnosticReporter reporter = diagnostics.CreateReporter(new DiagnosticSource("test.mixer", "Mixer"));
        using var runtime = new AudioRuntime(m_types, new MutedAudioDevice(), m_artifacts, m_events, reporter);
        var mixer = new AudioMixerAsset { mixerTypeId = "tests.unavailable" };
        Assert.False(runtime.ApplyMixer(mixer));
        Assert.Contains(sink.issues, issue => issue.code == "AUDIO_MIXER_EXTENSION_MISSING");
        mixer.mixerTypeId = string.Empty;
        Assert.True(runtime.ApplyMixer(mixer));
        Assert.DoesNotContain(sink.issues, issue => issue.code == "AUDIO_MIXER_EXTENSION_MISSING");
    }

    [Fact]
    public void MixerExtensionBuildsOpenBusGraphAndMissingCandidateKeepsLastGood()
    {
        using AudioRuntime runtime = CreateRuntime();
        var mixer = new AudioMixerAsset { mixerTypeId = "tests.audio.mixer" };

        Assert.True(runtime.ApplyMixer(mixer));
        Assert.True(runtime.SetBusVolume(new AudioBusID("tests.audio.bus.music"), 0.5f));

        mixer.mixerTypeId = "tests.audio.missing";
        Assert.False(runtime.ApplyMixer(mixer));
        Assert.True(runtime.SetBusMuted(new AudioBusID("tests.audio.bus.music"), true));
    }

    [Fact]
    public void DeviceReplacementCompletesOldHandlesAndUsesANewGeneration()
    {
        AudioClipAsset clip = CreateClip(frameCount: 480_000);
        using AudioRuntime runtime = CreateRuntime();
        AudioVoiceHandle oldVoice = runtime.Play(clip);
        runtime.Update(0f);

        runtime.ReplaceDevice(new MutedAudioDevice());
        AudioVoiceHandle newVoice = runtime.Play(clip);

        Assert.NotEqual(oldVoice.ownerGeneration, newVoice.ownerGeneration);
        Assert.True(runtime.TryGetVoiceState(oldVoice, out AudioPlaybackState state));
        Assert.Equal(AudioPlaybackState.Completed, state);
        Assert.False(runtime.Stop(oldVoice));
    }

    [Fact]
    public void RecoveryCandidatePreservesTheLastGoodMixerGraph()
    {
        using AudioRuntime runtime = CreateRuntime();
        var mixer = new AudioMixerAsset { mixerTypeId = "tests.audio.mixer" };
        Assert.True(runtime.ApplyMixer(mixer));
        var music = new AudioBusID("tests.audio.bus.music");
        Assert.True(runtime.SetBusVolume(music, 0.25f));
        Assert.True(runtime.SetBusPaused(music, true));
        var replacement = new ReadyAudioDevice();

        Assert.True(runtime.TryRecoverDevice(() => replacement));

        Assert.Equal(AudioDeviceState.Ready, runtime.deviceState);
        Assert.Equal(0.25f, replacement.GetBusVolume(music));
        Assert.True(replacement.GetBusPaused(music));
    }
}
