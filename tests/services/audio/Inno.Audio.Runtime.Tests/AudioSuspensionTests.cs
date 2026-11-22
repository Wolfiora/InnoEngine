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
using Inno.Core.Events;
using Inno.Core.Identity;
using Inno.Extensibility.Modules;
using Inno.Extensibility.Types;
using Inno.References;

namespace Inno.Audio.Runtime.Tests;

[Collection(nameof(AudioRuntimeFixture))]
public sealed class AudioSuspensionTests : AudioRuntimeFixture
{
    [Fact]
    public void ConsumedApplicationLifecycleStillSuspendsAudioAndUnsubscribesOnStop()
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(
            new DiagnosticSource("test.suspension.consumed", "Suspension")
        );
        using EventHub consumer = m_events.CreateHub(int.MaxValue);
        consumer.Listen<ApplicationSuspensionChangedEvent>(e => e.HandleInGlobal());
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        runtime.Attach();
        var suspension = new ApplicationSuspensionChangedEvent(true);
        m_events.Emit(suspension);
        Assert.True(suspension.isGlobalHandled);
        Assert.True(device.GetBusPaused(AudioBusID.master));
        m_events.Emit(new ApplicationSuspensionChangedEvent(false));
        Assert.False(device.GetBusPaused(AudioBusID.master));
        runtime.Dispose();
        int callsAfterStop = device.busPauseCalls;
        m_events.Emit(new ApplicationSuspensionChangedEvent(true));
        Assert.Equal(callsAfterStop, device.busPauseCalls);
    }

    [Fact]
    public void FailedSuspensionRollsBackRetainedMixerRootsAndRemainsRetryable()
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(
            new DiagnosticSource("test.suspension.rollback", "Suspension")
        );
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        runtime.Attach();
        runtime.Play(CreateClip(480_000), new AudioPlayOptions(loop: true));
        runtime.Update(0);
        Assert.True(runtime.ApplyMixer(new AudioMixerAsset { mixerTypeId = "tests.audio.mixer" }));
        device.failBusPauseAt = device.busPauseCalls + 2;
        Assert.Throws<InvalidOperationException>(() => m_events.Emit(new ApplicationSuspensionChangedEvent(true)));
        Assert.All(device.GetMasterPauses(), paused => Assert.False(paused));
        Assert.Equal(2, device.GetMasterPauses().Count);
        device.failBusPauseAt = null;
        m_events.Emit(new ApplicationSuspensionChangedEvent(true));
        Assert.All(device.GetMasterPauses(), paused => Assert.True(paused));
        m_events.Emit(new ApplicationSuspensionChangedEvent(false));
        Assert.All(device.GetMasterPauses(), paused => Assert.False(paused));
    }

    [Fact]
    public void ApplicationSuspensionPreservesUserPauseAndReplacementMixerState()
    {
        var device = new ReadyAudioDevice();
        using DiagnosticReporter reporter = new DiagnosticHub().CreateReporter(new DiagnosticSource("test.suspension", "Suspension"));
        using var runtime = new AudioRuntime(m_types, device, m_artifacts, m_events, reporter);
        runtime.Attach();
        Assert.True(runtime.SetBusPaused(AudioBusID.master, true));
        m_events.Emit(new ApplicationSuspensionChangedEvent(true));
        m_events.Emit(new ApplicationSuspensionChangedEvent(false));
        Assert.True(device.GetBusPaused(AudioBusID.master));

        Assert.True(runtime.SetBusPaused(AudioBusID.master, false));
        m_events.Emit(new ApplicationSuspensionChangedEvent(true));
        Assert.True(runtime.SetBusPaused(AudioBusID.master, false));
        Assert.True(device.GetBusPaused(AudioBusID.master));
        Assert.True(runtime.ApplyMixer(new AudioMixerAsset { mixerTypeId = "tests.audio.mixer" }));
        Assert.True(device.GetBusPaused(AudioBusID.master));

        var replacement = new ReadyAudioDevice();
        runtime.ReplaceDevice(replacement);
        Assert.True(replacement.GetBusPaused(AudioBusID.master));
        m_events.Emit(new ApplicationSuspensionChangedEvent(false));
        Assert.False(replacement.GetBusPaused(AudioBusID.master));
        runtime.Dispose();
        m_events.Emit(new ApplicationSuspensionChangedEvent(true));
        Assert.Equal(1, replacement.disposals);
    }
}
