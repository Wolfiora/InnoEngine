using System;
using System.Threading;
using Inno.Adapter.Audio.MiniAudio;
using Inno.Audio;
using Inno.Benchmark.Runner;

namespace Inno.Benchmark.MiniAudio;

internal static class ProcessorBufferProbe
{
    internal static void Measure(ProbeReport report)
    {
        using var device = new MiniAudioDevice(new MiniAudioDeviceOptions { sampleRate = 48_000 });
        IAudioDevice backend = device;
        AudioBusHandle master = backend.CreateBus(AudioBusID.master);
        var counter = new FrameCounter();
        var configuration = new AudioProcessorConfiguration(new AudioProcessorID("benchmark.buffer"));
        if (device.state != AudioDeviceState.Ready || !device.AddBlockProcessor(master, configuration, counter))
        {
            throw new InvalidOperationException("Measuring the processor queue requires an active output device.");
        }
        int rate = device.capabilities.sampleRate;
        backend.Update(0);
        long first = counter.frames;
        report.Row("output sample rate (Hz)", rate);
        report.Row("first fill (frames)", first);
        double start = device.dspTime;
        Thread.Sleep(15);
        backend.Update(0);
        long refill = counter.frames - first;
        report.Row("output clock advance (frames)", (long)Math.Round((device.dspTime - start) * rate));
        report.Row("second fill (frames)", refill);
        // Each device pump fills the writable part of the ring. The second fill measures its newly freed space.
        report.Row("queued before second fill (frames)", first - refill);
        report.Row("queued before second fill (microseconds)", (first - refill) * 1_000_000L / rate);
        report.Row("underrun before stall (frames)", device.statistics.processorUnderrunFrames);
        Thread.Sleep(180);
        backend.Update(0);
        report.Row("underrun after stall (frames)", device.statistics.processorUnderrunFrames);
    }

    private sealed class FrameCounter : AudioBlockProcessor
    {
        public override void Process(
            Span<float> samples,
            int channels,
            int sampleRate,
            AudioProcessorConfiguration configuration
        ) => frames += samples.Length / channels;

        internal long frames { get; private set; }
    }
}
