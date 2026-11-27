using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Xunit;
using Inno.Adapter.Audio.MiniAudio;
using Inno.Audio;
using Inno.Core.Mathematics;

namespace Inno.Adapter.Audio.MiniAudio.Native.Tests;

public sealed class MiniAudioDeviceTests
{
    [Fact]
    public void ExternalEffectsProcessKnownSamplesInBusOrderAndRetireWithTheBus()
    {
        using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
        IAudioDevice backend = device;
        AudioBusHandle master = backend.CreateBus(AudioBusID.master);
        var configuration = new AudioProcessorConfiguration(new AudioProcessorID("fixture.effect"), []);
        var source = new FixtureProcessor(fill: true);
        var half = new FixtureProcessor(fill: false);
        Assert.True(device.AddBlockProcessor(master, configuration, source));
        Assert.True(device.AddBlockProcessor(master, configuration, half));
        float[] samples = new float[512];

        device.RenderOffline(samples);

        Assert.All(samples, sample => Assert.Equal(0.25f, sample, precision: 5));
        Assert.True(backend.DestroyBus(master));
        Assert.True(source.disposed);
        Assert.True(half.disposed);
    }

    [Fact]
    public void StreamDecoderPreservesShortReadsAndSeeksEachPlaybackIndependently()
    {
        using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true, channels = 1 });
        IAudioDevice backend = device;
        AudioBusHandle master = backend.CreateBus(AudioBusID.master);
        var opened = new List<SampleCursor>();
        var descriptor = new AudioClipDescriptor(new AudioCodecID("fixture/pcm"), AudioClipLoadMode.Stream, 1, 48000, 1024, 1024);
        AudioClipHandle clip = device.CreateStreamClip(descriptor, () =>
        {
            var cursor = new SampleCursor();
            opened.Add(cursor);
            return cursor;
        });
        Assert.Equal(256, Assert.Single(opened).position);
        AudioDeviceVoiceHandle first = backend.Play(clip, master, AudioPlayOptions.defaultValue);
        AudioDeviceVoiceHandle second = backend.Play(clip, master, AudioPlayOptions.defaultValue);
        Assert.True(first.isValid && second.isValid);
        Assert.Equal(2, opened.Count);
        Assert.True(backend.Pause(second));
        float[] samples = new float[32];
        device.RenderOffline(samples);
        // MiniAudio's pitch resampler primes its two-frame history on each fresh voice.
        Assert.Equal(new float[2], samples[..2]);
        Assert.All(samples[2..], sample => Assert.Equal(0.25f, sample, precision: 5));
        device.RenderOffline(samples);
        Assert.All(samples, sample => Assert.Equal(0.25f, sample, precision: 5));
        Assert.True(backend.Seek(first, TimeSpan.FromSeconds(768d / 48000)));
        Assert.True(opened[0].disposed);
        Assert.False(opened[1].disposed);
        device.RenderOffline(samples);
        Assert.All(samples[2..], sample => Assert.Equal(0.75f, sample, precision: 5));
        backend.Update(0.02f);
        Assert.True(backend.TryDequeueCompletion(out AudioDeviceCompletion done));
        Assert.Equal(first, done.voice);
        Assert.Equal(AudioCompletionReason.NaturalEnd, done.reason);
        Assert.True(backend.Resume(second));
        device.RenderOffline(samples);
        Assert.All(samples[2..], sample => Assert.Equal(0.25f, sample, precision: 5));
        Assert.True(backend.Stop(second));
        Assert.True(backend.DestroyClip(clip));
        Assert.All(opened, cursor => Assert.True(cursor.disposed));
    }

    private sealed class SampleCursor : AudioStreamReader
    {
        internal int position { get; private set; }
        internal bool disposed { get; private set; }
        public override bool canSeek => true;

        public override int Read(Span<float> samples)
        {
            ObjectDisposedException.ThrowIf(disposed, this);
            int frames = Math.Min(7, Math.Min(samples.Length, 1024 - position));
            for (int index = 0; index < frames; index++) samples[index] = position++ < 512 ? 0.25f : 0.75f;
            return frames;
        }

        public override bool Seek(long frame)
        {
            position = checked((int)frame);
            return true;
        }

        public override void Dispose() => disposed = true;
    }

    private sealed class FixtureProcessor(bool fill) : AudioBlockProcessor
    {
        internal bool disposed { get; private set; }

        public override void Process(
            Span<float> samples,
            int channels,
            int sampleRate,
            AudioProcessorConfiguration configuration
        ) {
            Assert.False(disposed);
            if (fill)
            {
                samples.Fill(0.5f);
                return;
            }
            for (int index = 0; index < samples.Length; index++)
            {
                samples[index] *= 0.5f;
            }
        }

        public override void Dispose() => disposed = true;
    }

    [Fact]
    public void NativeAdmissionIncludesUndeliveredCompletionsAndReopensAfterDrain()
    {
        string path = CreateWaveFile(4800);
        try
        {
            using var device = new MiniAudioDevice(new MiniAudioDeviceOptions
            {
                noDevice = true,
                limits = new AudioDeviceLimits(clips: 1, voices: 1, buses: 1)
            });
            IAudioDevice backend = device;
            var descriptor = new AudioClipDescriptor(
                AudioCodecID.wav,
                AudioClipLoadMode.Decode,
                1,
                48_000,
                4800,
                new FileInfo(path).Length
            );
            var source = new EncodedAudioTestSource(File.ReadAllBytes(path));
            AudioClipHandle clip = backend.CreateClip(descriptor, source);
            WaitForClip(backend, clip);
            AudioBusHandle master = backend.CreateBus(AudioBusID.master);
            Assert.False(backend.CreateClip(descriptor, source).isValid);
            Assert.False(backend.CreateBus(new AudioBusID("extra"), master).isValid);
            for (int iteration = 0; iteration < 32; iteration++)
            {
                AudioDeviceVoiceHandle voice = backend.Play(clip, master, new AudioPlayOptions(loop: true));
                Assert.True(voice.isValid);
                Assert.False(backend.Play(clip, master, AudioPlayOptions.defaultValue).isValid);
                Assert.True(backend.Stop(voice));
                Assert.False(backend.Play(clip, master, AudioPlayOptions.defaultValue).isValid);
                Assert.True(backend.TryDequeueCompletion(out AudioDeviceCompletion completion));
                Assert.Equal(voice, completion.voice);
            }
        }
        finally
        {
            File.Delete(path);
        }
    }

    [Fact]
    public void InvalidInputsDoNotAllocateNativeVoicesOrPoisonExistingPlayback()
    {
        string clipPath = CreateWaveFile(4800);
        try
        {
            using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
            IAudioDevice backend = device;
            AudioBusHandle master = backend.CreateBus(AudioBusID.master);
            AudioClipHandle clip = backend.CreateClip(
                new AudioClipDescriptor(
                    AudioCodecID.wav,
                    AudioClipLoadMode.Decode,
                    1,
                    48_000,
                    4800,
                    new FileInfo(clipPath).Length
                ),
                new EncodedAudioTestSource(File.ReadAllBytes(clipPath))
            );
            WaitForClip(backend, clip);
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
            AudioDeviceVoiceHandle voice = backend.Play(clip, master, new AudioPlayOptions(loop: true));
            WaitForVoice(backend, voice);
            Assert.False(backend.SetVoiceParameters(voice, default));
            Assert.True(backend.SetVoiceParameters(voice, new AudioVoiceParameters(0.5f, 1, 0)));
            Assert.True(backend.SetBusVolume(master, 1));
            backend.Update(0.02f);
            Assert.True(backend.TryGetVoiceState(voice, out AudioPlaybackState state));
            Assert.Equal(AudioPlaybackState.Playing, state);
            Assert.True(backend.Stop(voice));
            Assert.True(backend.DestroyClip(clip));
            Assert.True(backend.DestroyBus(master));
        }
        finally
        {
            File.Delete(clipPath);
        }
    }

    [Fact]
    public void HeadlessDeviceAdvancesNativeGraphAndCompletesVoice()
    {
        string clipPath = CreateWaveFile(frameCount: 2400);
        try
        {
            using var device = new MiniAudioDevice(new MiniAudioDeviceOptions
            {
                noDevice = true,
                channels = 2,
                sampleRate = 48_000,
                listenerCount = 2
            });
            IAudioDevice backend = device;
            AudioBusHandle master = backend.CreateBus(AudioBusID.master);
            AudioClipHandle clip = backend.CreateClip(
                new AudioClipDescriptor(
                    AudioCodecID.wav,
                    AudioClipLoadMode.Decode,
                    1,
                    48_000,
                    2400,
                    new FileInfo(clipPath).Length
                ),
                new EncodedAudioTestSource(File.ReadAllBytes(clipPath))
            );
            WaitForClip(backend, clip);
            AudioListenerHandle listener = backend.CreateListener(
                new AudioListenerState(Vector3.ZERO, Vector3.FORWARD, Vector3.UP, Vector3.ZERO)
            );
            AudioDeviceVoiceHandle voice = backend.Play(
                clip,
                master,
                new AudioPlayOptions(
                    spatial: new AudioSpatialOptions(new Vector3(2f, 0f, 0f), Vector3.FORWARD, Vector3.ZERO)
                ),
                null
            );

            WaitForVoice(backend, voice);
            Assert.Equal(AudioDeviceState.Muted, backend.state);
            Assert.Equal(2, backend.capabilities.maxListeners);
            Assert.True(voice.isValid);
            Assert.True(
                backend.SetListener(
                    listener,
                    new AudioListenerState(new Vector3(1f, 0f, 0f), Vector3.FORWARD, Vector3.UP, Vector3.ZERO)
                )
            );

            backend.Update(0.1f);

            Assert.True(backend.dspTime >= 0.05d);
            Assert.True(backend.TryDequeueCompletion(out AudioDeviceCompletion completion));
            Assert.Equal(voice, completion.voice);
            Assert.Equal(AudioCompletionReason.NaturalEnd, completion.reason);
            Assert.False(backend.TryGetVoiceState(voice, out AudioPlaybackState terminalState));
            Assert.Equal(AudioPlaybackState.Invalid, terminalState);
            Assert.True(backend.DestroyListener(listener));
            Assert.True(backend.DestroyClip(clip));
            Assert.True(backend.DestroyBus(master));
        }
        finally
        {
            File.Delete(clipPath);
        }
    }

    [Fact]
    public void StandardProcessorsAndScheduledControlsUseNativeNodes()
    {
        string clipPath = CreateWaveFile(frameCount: 4800);
        try
        {
            using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
            IAudioDevice backend = device;
            AudioBusHandle master = backend.CreateBus(AudioBusID.master);
            AudioBusHandle effects = backend.CreateBus(new AudioBusID("test.effects"), master);
            foreach (AudioProcessorID processorID in StandardProcessors())
            {
                Assert.True(backend.AddBusProcessor(effects, CreateProcessor(processorID)));
            }

            AudioClipHandle clip = backend.CreateClip(
                new AudioClipDescriptor(
                    AudioCodecID.wav,
                    AudioClipLoadMode.Stream,
                    1,
                    48_000,
                    4800,
                    new FileInfo(clipPath).Length
                ),
                new EncodedAudioTestSource(File.ReadAllBytes(clipPath))
            );
            WaitForClip(backend, clip);
            AudioDeviceVoiceHandle voice = backend.Play(
                clip,
                effects,
                new AudioPlayOptions(loop: true),
                backend.dspTime + 0.05d
            );

            WaitForVoice(backend, voice);
            Assert.True(backend.TryGetVoiceState(voice, out AudioPlaybackState scheduled));
            Assert.Equal(AudioPlaybackState.Scheduled, scheduled);
            backend.Update(0.06f);
            Assert.True(backend.TryGetVoiceState(voice, out AudioPlaybackState playing));
            Assert.Equal(AudioPlaybackState.Playing, playing);
            Assert.True(backend.Pause(voice));
            Assert.True(backend.Seek(voice, TimeSpan.FromMilliseconds(10)));
            Assert.True(backend.SetVoiceParameters(voice, new AudioVoiceParameters(0.5f, 1.25f, -0.25f)));
            Assert.True(backend.Resume(voice));
            Assert.True(backend.Stop(voice));
            Assert.True(backend.TryDequeueCompletion(out AudioDeviceCompletion completion));
            Assert.Equal(AudioCompletionReason.Stopped, completion.reason);
            Assert.True(backend.DestroyClip(clip));
            Assert.True(backend.DestroyBus(effects));
            Assert.True(backend.DestroyBus(master));
        }
        finally
        {
            File.Delete(clipPath);
        }
    }

    [Fact]
    public void MixerGenerationsCanOverlapUntilOldVoicesReleaseTheirGraph()
    {
        string clipPath = CreateWaveFile(frameCount: 48_000);
        try
        {
            using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true });
            IAudioDevice backend = device;
            AudioBusHandle oldMaster = backend.CreateBus(AudioBusID.master);
            AudioBusHandle oldMusic = backend.CreateBus(new AudioBusID("test.music"), oldMaster);
            AudioClipHandle clip = backend.CreateClip(
                new AudioClipDescriptor(
                    AudioCodecID.wav,
                    AudioClipLoadMode.Decode,
                    1,
                    48_000,
                    48_000,
                    new FileInfo(clipPath).Length
                ),
                new EncodedAudioTestSource(File.ReadAllBytes(clipPath))
            );
            WaitForClip(backend, clip);
            AudioDeviceVoiceHandle voice = backend.Play(clip, oldMusic, new AudioPlayOptions(loop: true), null);

            AudioBusHandle newMaster = backend.CreateBus(AudioBusID.master);
            AudioBusHandle newMusic = backend.CreateBus(new AudioBusID("test.music"), newMaster);

            Assert.True(newMaster.isValid);
            Assert.True(newMusic.isValid);
            Assert.False(backend.DestroyBus(oldMusic));
            Assert.True(backend.Stop(voice));
            Assert.True(backend.DestroyClip(clip));
            Assert.True(backend.DestroyBus(oldMusic));
            Assert.True(backend.DestroyBus(oldMaster));
            Assert.True(backend.DestroyBus(newMusic));
            Assert.True(backend.DestroyBus(newMaster));
        }
        finally
        {
            File.Delete(clipPath);
        }
    }

    private static void WaitForClip(
        IAudioDevice device,
        AudioClipHandle clip
    ) {
        Assert.True(SpinWait.SpinUntil(() => device.GetClipState(clip) != AudioClipState.Preparing, TimeSpan.FromSeconds(5)));
        Assert.Equal(AudioClipState.Ready, device.GetClipState(clip));
    }

    private static void WaitForVoice(
        IAudioDevice device,
        AudioDeviceVoiceHandle voice
    ) => Assert.True(
        SpinWait.SpinUntil(() => (
            device.TryGetVoiceState(voice, out AudioPlaybackState state)
            && state != AudioPlaybackState.Preparing
        ), TimeSpan.FromSeconds(5))
    );

    private static IEnumerable<AudioProcessorID> StandardProcessors()
    {
        yield return AudioProcessorID.lowPass;
        yield return AudioProcessorID.highPass;
        yield return AudioProcessorID.bandPass;
        yield return AudioProcessorID.notch;
        yield return AudioProcessorID.peak;
        yield return AudioProcessorID.lowShelf;
        yield return AudioProcessorID.highShelf;
        yield return AudioProcessorID.delay;
    }

    private static AudioProcessorConfiguration CreateProcessor(AudioProcessorID id)
    {
        if (id == AudioProcessorID.delay)
        {
            return new AudioProcessorConfiguration(
                id,
                [
                    new AudioProcessorParameter(AudioParameterID.delayMilliseconds, 5f),
                    new AudioProcessorParameter(AudioParameterID.decay, 0.2f)
                ]
            );
        }
        return new AudioProcessorConfiguration(
            id,
            [
                new AudioProcessorParameter(AudioParameterID.frequency, 1000f),
                new AudioProcessorParameter(AudioParameterID.quality, 0.707f),
                new AudioProcessorParameter(AudioParameterID.gainDecibels, 2f),
                new AudioProcessorParameter(AudioParameterID.shelfSlope, 1f)
            ]
        );
    }

    private static string CreateWaveFile(int frameCount)
    {
        string path = Path.Combine(Path.GetTempPath(), $"inno-miniaudio-{Guid.NewGuid():N}.wav");
        const int C_SAMPLE_RATE = 48_000;
        const short C_CHANNELS = 1;
        const short C_BITS_PER_SAMPLE = 16;
        int dataLength = frameCount * C_CHANNELS * (C_BITS_PER_SAMPLE / 8);
        using FileStream stream = File.Create(path);
        using var writer = new BinaryWriter(stream);
        writer.Write("RIFF"u8);
        writer.Write(36 + dataLength);
        writer.Write("WAVE"u8);
        writer.Write("fmt "u8);
        writer.Write(16);
        writer.Write((short)1);
        writer.Write(C_CHANNELS);
        writer.Write(C_SAMPLE_RATE);
        writer.Write(C_SAMPLE_RATE * C_CHANNELS * (C_BITS_PER_SAMPLE / 8));
        writer.Write((short)(C_CHANNELS * (C_BITS_PER_SAMPLE / 8)));
        writer.Write(C_BITS_PER_SAMPLE);
        writer.Write("data"u8);
        writer.Write(dataLength);
        for (int frame = 0; frame < frameCount; frame++)
        {
            double phase = 2d * Math.PI * 440d * frame / C_SAMPLE_RATE;
            writer.Write((short)(Math.Sin(phase) * short.MaxValue * 0.1d));
        }
        return path;
    }
}
