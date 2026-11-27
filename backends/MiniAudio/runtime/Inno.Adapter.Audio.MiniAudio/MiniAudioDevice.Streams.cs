using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Inno.Audio;
using Inno.Core.Execution;
using Inno.Native.MiniAudio;

using NativeApi = Inno.Native.MiniAudio.MiniAudio;

namespace Inno.Adapter.Audio.MiniAudio;

public sealed unsafe partial class MiniAudioDevice
{
    private readonly Dictionary<ulong, StreamClip> m_streamClips = [];
    private readonly List<MiniAudioDecoderBuffer> m_retiringDecoders = [];

    /// <inheritdoc/>
    public AudioClipHandle CreateStreamClip(
        AudioClipDescriptor descriptor,
        Func<AudioStreamReader> open
    ) {
        EnsureActive();
        ArgumentNullException.ThrowIfNull(open);
        if (descriptor.loadMode != AudioClipLoadMode.Stream || m_clips.Count >= m_options.limits.clips)
        {
            return default;
        }
        var clip = new StreamClip(descriptor, open);
        ulong identity = m_nextIdentity++;
        m_streamClips.Add(identity, clip);
        m_clips.Add(identity, descriptor);
        return CreateClipHandle(identity, generation);
    }

    private void PumpStreams()
    {
        if (m_streamClips.Count == 0) return;
        foreach ((ulong id, VoiceRecord voice) in m_voices.ToArray())
        {
            if (voice.stream is null || voice.state == AudioPlaybackState.Paused) continue;
            try
            {
                voice.stream.Fill(C_PROCESS_BLOCK_FRAMES, voice.loop);
                if (voice.startPending && voice.stream.ready)
                {
                    if (NativeApi.Raw.SoundStart(voice.sound) != MaResult.MA_SUCCESS)
                    {
                        throw new InvalidOperationException("The prepared decoder voice could not start.");
                    }
                    voice.startPending = false;
                }
            }
            catch (Exception exception) when (RetirementPendingException.Find(exception) is null)
            {
                CompleteVoice(id, voice, AudioCompletionReason.DecodeFailed);
            }
        }
    }

    private void RetireDecoder(MiniAudioDecoderBuffer decoder)
    {
        try
        {
            decoder.Dispose();
        }
        catch (Exception pending) when (RetirementPendingException.Find(pending) is not null)
        {
            m_retiringDecoders.Add(decoder);
        }
    }

    private void DrainDecoders(bool requireCompletion)
    {
        for (int index = m_retiringDecoders.Count - 1; index >= 0; index--)
        {
            try
            {
                m_retiringDecoders[index].Dispose();
            }
            catch (Exception pending) when (!requireCompletion && RetirementPendingException.Find(pending) is not null)
            {
                continue;
            }
            m_retiringDecoders.RemoveAt(index);
        }
    }

    private bool SeekStream(
        VoiceRecord voice,
        long frame
    ) {
        if (!voice.stream!.canSeek) return false;
        AudioClipDescriptor descriptor = m_clips[voice.clipIdentity];
        if (descriptor.frameCount > 0 && frame > descriptor.frameCount) return false;
        MiniAudioDecoderBuffer replacement;
        try
        {
            replacement = m_streamClips[voice.clipIdentity].OpenAt(frame);
        }
        catch (NotSupportedException)
        {
            return false;
        }
        MaSound* sound = (MaSound*)NativeMemory.AllocZeroed((nuint)sizeof(MaSound));
        bool initialized = false;
        try
        {
            if (NativeApi.Raw.SoundInitFromDataSource(m_engine, replacement.source, 0, voice.bus.group, sound) != MaResult.MA_SUCCESS)
            {
                return false;
            }
            initialized = true;
            ApplyVoiceParameters(sound, voice.parameters);
            if (voice.state == AudioPlaybackState.Scheduled && voice.scheduledDspTime is double start)
            {
                NativeApi.Raw.SoundSetStartTimeInPcmFrames(sound, checked((ulong)Math.Round(start * capabilities.sampleRate)));
            }
            bool deferred = !replacement.ready;
            if (!deferred && voice.state != AudioPlaybackState.Paused && NativeApi.Raw.SoundStart(sound) != MaResult.MA_SUCCESS)
            {
                return false;
            }
            MaSound* old = voice.sound;
            MiniAudioDecoderBuffer previous = voice.stream;
            voice.sound = sound;
            voice.stream = replacement;
            voice.startPending = deferred;
            sound = null;
            NativeApi.Raw.SoundUninit(old);
            NativeMemory.Free(old);
            RetireDecoder(previous);
            return true;
        }
        finally
        {
            if (sound != null)
            {
                if (initialized) NativeApi.Raw.SoundUninit(sound);
                NativeMemory.Free(sound);
                RetireDecoder(replacement);
            }
        }
    }

    private sealed class StreamClip(
        AudioClipDescriptor descriptor,
        Func<AudioStreamReader> open
    ) : IDisposable
    {
        private MiniAudioDecoderBuffer? m_prepared = new(descriptor, open());

        /// <inheritdoc/>
        public void Dispose()
        {
            m_prepared?.Dispose();
            m_prepared = null;
        }

        internal bool Prepare()
        {
            if (m_prepared is null || m_prepared.ready) return true;
            m_prepared.Fill(256, false);
            return m_prepared.ready;
        }

        internal MiniAudioDecoderBuffer Take()
        {
            MiniAudioDecoderBuffer? first = m_prepared;
            m_prepared = null;
            return first ?? new MiniAudioDecoderBuffer(descriptor, open());
        }

        internal MiniAudioDecoderBuffer OpenAt(long frame) => new(descriptor, open(), frame);
    }
}
