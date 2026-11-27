using System;
using System.Runtime.InteropServices;
using Inno.Audio;
using Inno.Native.MiniAudio;

using NativeApi = Inno.Native.MiniAudio.MiniAudio;

namespace Inno.Adapter.Audio.MiniAudio;

internal sealed unsafe class MiniAudioBufferedProcessor : IDisposable
{
    internal const int C_CAPACITY = 4096;
    private const int C_BLOCK = 256;

    private readonly int m_channels;
    private readonly int m_sampleRate;
    private readonly float[] m_block;
    private readonly AudioProcessorConfiguration m_configuration;
    private AudioBlockProcessor? m_processor;
    private MaNodeGraph* m_input;
    private MaPcmRb* m_output;
    private MaDataSourceNode* m_node;
    private bool m_graphReady;
    private bool m_ringReady;
    private bool m_nodeReady;
    private bool m_processing;
    private ulong m_submitted;

    internal MiniAudioBufferedProcessor(
        MaNodeGraph* engineGraph,
        int channels,
        int sampleRate,
        AudioProcessorConfiguration configuration
    ) {
        m_channels = channels;
        m_sampleRate = sampleRate;
        m_configuration = configuration;
        m_block = new float[C_BLOCK * channels];
        try
        {
            m_input = (MaNodeGraph*)NativeMemory.AllocZeroed((nuint)sizeof(MaNodeGraph));
            MaNodeGraphConfig graph = NativeApi.NodeGraphConfigInit((uint)channels);
            Check(NativeApi.Raw.NodeGraphInit(&graph, null, m_input));
            m_graphReady = true;
            m_output = (MaPcmRb*)NativeMemory.AllocZeroed((nuint)sizeof(MaPcmRb));
            Check(NativeApi.Raw.PcmRbInit(MaFormat.ma_format_f32, (uint)channels, C_CAPACITY, null, null, m_output));
            m_ringReady = true;
            NativeApi.Raw.PcmRbSetSampleRate(m_output, (uint)sampleRate);
            m_node = (MaDataSourceNode*)NativeMemory.AllocZeroed((nuint)sizeof(MaDataSourceNode));
            MaDataSourceNodeConfig output;
            NativeApi.Raw.DataSourceNodeConfigInit(m_output, &output);
            Check(NativeApi.Raw.DataSourceNodeInit(engineGraph, &output, null, m_node));
            m_nodeReady = true;
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    internal void* output => m_node;

    internal long underrunFrames
    {
        get
        {
            ulong requested = NativeApi.Raw.NodeGetTime(m_node);
            ulong consumed = m_submitted - NativeApi.Raw.PcmRbAvailableRead(m_output);
            return requested > consumed ? checked((long)(requested - consumed)) : 0;
        }
    }

    internal bool Attach(
        void* upstream,
        void* target,
        ulong frame,
        AudioBlockProcessor processor
    ) {
        Check(NativeApi.Raw.NodeGraphSetTime(m_input, frame));
        if (NativeApi.Raw.NodeAttachOutputBus(m_node, 0, target, 0) != MaResult.MA_SUCCESS) return false;
        if (NativeApi.Raw.NodeAttachOutputBus(upstream, 0, NativeApi.Raw.NodeGraphGetEndpoint(m_input), 0) != MaResult.MA_SUCCESS)
        {
            NativeApi.Raw.NodeDetachOutputBus(m_node, 0);
            return false;
        }
        m_processor = processor;
        return true;
    }

    internal void Pump(uint maximumFrames)
    {
        if (m_processing) throw new InvalidOperationException("An audio effect cannot recursively process its own bus.");
        uint remaining = Math.Min(maximumFrames, NativeApi.Raw.PcmRbAvailableWrite(m_output));
        m_processing = true;
        try
        {
            while (remaining != 0)
            {
                uint count = Math.Min(remaining, C_BLOCK);
                void* destination = null;
                Check(NativeApi.Raw.PcmRbAcquireWrite(m_output, &count, &destination));
                if (count == 0) return;
                Span<float> samples = m_block.AsSpan(0, checked((int)count * m_channels));
                ulong read = 0;
                fixed (float* buffer = samples)
                {
                    Check(NativeApi.Raw.NodeGraphReadPcmFrames(m_input, buffer, count, &read));
                }
                samples[checked((int)read * m_channels)..].Clear();
                try
                {
                    m_processor!.Process(samples, m_channels, m_sampleRate, m_configuration);
                }
                catch
                {
                    samples.Clear();
                    samples.CopyTo(new Span<float>(destination, samples.Length));
                    Check(NativeApi.Raw.PcmRbCommitWrite(m_output, count));
                    m_submitted += count;
                    throw;
                }
                samples.CopyTo(new Span<float>(destination, samples.Length));
                Check(NativeApi.Raw.PcmRbCommitWrite(m_output, count));
                m_submitted += count;
                remaining -= count;
            }
        }
        finally
        {
            m_processing = false;
        }
    }

    public void Dispose()
    {
        if (m_processing) throw new InvalidOperationException("An executing audio processor cannot release its block.");
        if (m_nodeReady) NativeApi.Raw.DataSourceNodeUninit(m_node, null);
        if (m_graphReady) NativeApi.Raw.NodeGraphUninit(m_input, null);
        if (m_ringReady) NativeApi.Raw.PcmRbUninit(m_output);
        m_nodeReady = m_graphReady = m_ringReady = false;
        NativeMemory.Free(m_node);
        NativeMemory.Free(m_input);
        NativeMemory.Free(m_output);
        m_node = null;
        m_input = null;
        m_output = null;
        AudioBlockProcessor? processor = m_processor;
        m_processor = null;
        processor?.Dispose();
    }

    private static void Check(MaResult result)
    {
        if (result != MaResult.MA_SUCCESS) throw new InvalidOperationException($"Audio block processing failed: {result}.");
    }
}
