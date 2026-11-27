using System;
using System.IO;
using System.Runtime.InteropServices;
using Inno.Audio;
using Inno.Native.MiniAudio;

using NativeApi = Inno.Native.MiniAudio.MiniAudio;

namespace Inno.Adapter.Audio.MiniAudio;

internal sealed unsafe class MiniAudioDecoderBuffer : IDisposable
{
    private const int C_BLOCK_FRAMES = 256;
    private const uint C_CAPACITY = 4096;
    private readonly AudioStreamReader m_reader;
    private readonly AudioClipDescriptor m_descriptor;
    private readonly float[] m_block;
    private MaPcmRb* m_ring;
    private bool m_initialized;
    private bool m_end;

    internal MiniAudioDecoderBuffer(
        AudioClipDescriptor descriptor,
        AudioStreamReader reader,
        long start = 0
    ) {
        m_reader = reader;
        m_descriptor = descriptor;
        try
        {
            if (start != 0 && (!reader.canSeek || !reader.Seek(start)))
            {
                throw new NotSupportedException("The decoder cannot seek to the requested frame.");
            }
            m_block = new float[checked(C_BLOCK_FRAMES * descriptor.channels)];
            m_ring = (MaPcmRb*)NativeMemory.AllocZeroed((nuint)sizeof(MaPcmRb));
            Check(NativeApi.Raw.PcmRbInit(MaFormat.ma_format_f32, (uint)descriptor.channels, C_CAPACITY, null, null, m_ring));
            m_initialized = true;
            NativeApi.Raw.PcmRbSetSampleRate(m_ring, (uint)descriptor.sampleRate);
            Fill(C_BLOCK_FRAMES, false);
        }
        catch
        {
            Dispose();
            throw;
        }
    }

    internal void* source => m_ring;
    internal bool drained => m_end && NativeApi.Raw.PcmRbAvailableRead(m_ring) == 0;
    internal bool canSeek => m_reader.canSeek;

    internal void Fill(
        uint limit,
        bool loop
    ) {
        uint remaining = Math.Min(limit, NativeApi.Raw.PcmRbAvailableWrite(m_ring));
        bool justRewound = false;
        while (remaining != 0)
        {
            if (m_end)
            {
                if (!loop || justRewound || !m_reader.Seek(0)) return;
                m_end = false;
                justRewound = true;
            }
            uint count = Math.Min(remaining, C_BLOCK_FRAMES);
            void* output = null;
            Check(NativeApi.Raw.PcmRbAcquireWrite(m_ring, &count, &output));
            if (count == 0) return;
            Span<float> samples = m_block.AsSpan(0, (int)count * m_descriptor.channels);
            int read = m_reader.Read(samples);
            if ((uint)read > count) throw new InvalidDataException("The decoder returned an invalid frame count.");
            if (read == 0)
            {
                m_end = true;
                continue;
            }
            justRewound = false;
            int length = read * m_descriptor.channels;
            samples[..length].CopyTo(new Span<float>(output, length));
            Check(NativeApi.Raw.PcmRbCommitWrite(m_ring, (uint)read));
            remaining -= (uint)read;
        }
    }

    public void Dispose()
    {
        if (m_initialized) NativeApi.Raw.PcmRbUninit(m_ring);
        m_initialized = false;
        NativeMemory.Free(m_ring);
        m_ring = null;
        m_reader.Dispose();
    }

    private static void Check(MaResult result)
    {
        if (result != MaResult.MA_SUCCESS) throw new InvalidOperationException($"Decoded audio buffer failed: {result}.");
    }
}
