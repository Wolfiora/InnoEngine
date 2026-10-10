using System.IO;
using Inno.Assets;

namespace Inno.Audio.Runtime;

internal sealed class ArtifactAudioClipSource(ArtifactLease artifact) : IAudioClipSource
{
    /// <inheritdoc />
    public string contentHash => artifact.info.contentHash;

    /// <inheritdoc />
    public long length => artifact.info.length;

    /// <inheritdoc />
    public Stream OpenRead() => artifact.OpenRead();
}
