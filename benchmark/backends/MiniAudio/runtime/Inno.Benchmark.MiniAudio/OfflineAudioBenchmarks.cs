using System;
using System.IO;
using System.Security.Cryptography;
using System.Threading;
using BenchmarkDotNet.Attributes;
using Inno.Adapter.Audio.MiniAudio;
using Inno.Audio;

namespace Inno.Benchmark.MiniAudio;

/// <remarks>
/// BenchmarkDotNet derives its generated measurement runner from this class.
/// </remarks>
[MemoryDiagnoser]
public class OfflineAudioBenchmarks
{
    private const int C_RATE = 48_000;
    private const int C_FRAMES = 4800;

    private readonly WaveSource m_source = new();
    private IAudioDevice m_device = null!;
    private AudioClipHandle m_clip;

    [IterationSetup(Target = nameof(DecodeToReady))]
    public void Open() => m_device = new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = true, sampleRate = C_RATE });

    [Benchmark]
    public void DecodeToReady()
    {
        m_clip = m_device.CreateClip(
            new AudioClipDescriptor(AudioCodecID.wav, AudioClipLoadMode.Decode, 1, C_RATE, C_FRAMES, m_source.length),
            m_source
        );
        if (!m_clip.isValid || !SpinWait.SpinUntil(
            () => m_device.GetClipState(m_clip) != AudioClipState.Preparing,
            TimeSpan.FromSeconds(10)
        ) || m_device.GetClipState(m_clip) != AudioClipState.Ready
        ) {
            throw new InvalidDataException("The audio clip did not become ready.");
        }
    }

    [IterationSetup(Target = nameof(MixDry))]
    public void Dry() => PrepareMixer(null);

    [IterationSetup(Target = nameof(MixBiquad))]
    public void Biquad() => PrepareMixer(AudioProcessorID.lowPass);

    [IterationSetup(Target = nameof(MixDelay))]
    public void Delay() => PrepareMixer(AudioProcessorID.delay);

    [Benchmark]
    public void MixDry() => m_device.Update(0.01f);

    [Benchmark]
    public void MixBiquad() => m_device.Update(0.01f);

    [Benchmark]
    public void MixDelay() => m_device.Update(0.01f);

    [IterationCleanup]
    public void Close() => m_device.Dispose();

    private void PrepareMixer(AudioProcessorID? processor)
    {
        Open();
        DecodeToReady();
        AudioBusHandle bus = m_device.CreateBus(AudioBusID.master);
        if (!bus.isValid || (processor is { } id && !m_device.AddBusProcessor(bus, new AudioProcessorConfiguration(id))))
        {
            throw new InvalidOperationException("Failed to create offline mixer graph.");
        }
        if (!m_device.Play(m_clip, bus, new AudioPlayOptions(loop: true)).isValid)
        {
            throw new InvalidOperationException("Failed to start the PCM source.");
        }
        m_device.Update(0.01f);
    }

    private sealed class WaveSource : IAudioClipSource
    {
        private readonly byte[] m_bytes;

        internal WaveSource()
        {
            using var memory = new MemoryStream();
            using var writer = new BinaryWriter(memory);
            writer.Write("RIFF"u8);
            writer.Write(36 + C_FRAMES * 2);
            writer.Write("WAVEfmt "u8);
            writer.Write(16);
            writer.Write((short)1);
            writer.Write((short)1);
            writer.Write(C_RATE);
            writer.Write(C_RATE * 2);
            writer.Write((short)2);
            writer.Write((short)16);
            writer.Write("data"u8);
            writer.Write(C_FRAMES * 2);
            for (int i = 0; i < C_FRAMES; i++)
            {
                writer.Write((short)(10_000 * Math.Sin(i * Math.Tau * 440 / C_RATE)));
            }
            m_bytes = memory.ToArray();
            this.contentHash = Convert.ToHexString(SHA256.HashData(m_bytes));
        }

        public string contentHash { get; }
        public long length => m_bytes.Length;

        public Stream OpenRead() => new MemoryStream(m_bytes, writable: false);
    }
}
