using System;

namespace Inno.Adapter.Audio.MiniAudio;

internal sealed class MiniAudioClipSourceLease : IDisposable
{
    private MiniAudioEncodedArtifactCache? m_owner;

    internal MiniAudioClipSourceLease(
        MiniAudioEncodedArtifactCache owner,
        MiniAudioClipPreparation preparation
    ) {
        m_owner = owner;
        this.preparation = preparation;
    }

    internal MiniAudioClipPreparation preparation { get; }

    /// <inheritdoc />
    public void Dispose()
    {
        m_owner?.Release(preparation);
        m_owner = null;
    }
}
