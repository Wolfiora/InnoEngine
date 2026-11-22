using System;
using Inno.Audio;

namespace Inno.Adapter.Audio.MiniAudio;

/// <summary>
/// Supplies the MiniAudio implementation through the neutral audio creation boundary.
/// </summary>
public sealed class MiniAudioBackendProvider : AudioBackendProvider
{
    /// <summary>
    /// Creates an explicitly composed registration for the bundled implementation.
    /// </summary>
    public MiniAudioBackendProvider()
        : base(AudioBackendID.miniAudio) {}

    /// <inheritdoc/>
    public override IAudioDevice CreateDevice(AudioBackendOptions options)
        => new MiniAudioDevice(new MiniAudioDeviceOptions { noDevice = options.noDevice });
}
