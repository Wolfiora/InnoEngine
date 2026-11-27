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

    private sealed class StreamClip(
        AudioClipDescriptor descriptor,
        Func<AudioStreamReader> open
    ) : IDisposable
    {
        private MiniAudioDecoderBuffer? m_prepared = new(descriptor, open());

        internal MiniAudioDecoderBuffer Take()
        {
            MiniAudioDecoderBuffer? first = m_prepared;
            m_prepared = null;
            return first ?? new MiniAudioDecoderBuffer(descriptor, open());
        }

        internal MiniAudioDecoderBuffer OpenAt(long frame) => new(descriptor, open(), frame);

        public void Dispose()
        {
            MiniAudioDecoderBuffer? first = m_prepared;
            m_prepared = null;
            first?.Dispose();
        }
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
            }
            catch (Exception exception) when (RetirementPendingException.Find(exception) is null)
            {
                CompleteVoice(id, voice, AudioCompletionReason.DecodeFailed);
            }
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
            if (voice.state != AudioPlaybackState.Paused && NativeApi.Raw.SoundStart(sound) != MaResult.MA_SUCCESS)
            {
                return false;
            }
            MaSound* old = voice.sound;
            MiniAudioDecoderBuffer previous = voice.stream;
            voice.sound = sound;
            voice.stream = replacement;
            sound = null;
            NativeApi.Raw.SoundUninit(old);
            NativeMemory.Free(old);
            previous.Dispose();
            return true;
        }
        finally
        {
            if (sound != null)
            {
                if (initialized) NativeApi.Raw.SoundUninit(sound);
                NativeMemory.Free(sound);
                replacement.Dispose();
            }
        }
    }
}
